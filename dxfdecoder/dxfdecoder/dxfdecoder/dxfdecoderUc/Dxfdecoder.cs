using System;
using System.Drawing;
using System.Windows.Forms;
using Dxfdecoder.DxfdecoderUc;
using Pathdecoder;
using DxfdecoderDef;
using dxfdecoder.dxfdecoderUc;

using System.IO;
using System.Resources;
using System.Runtime.InteropServices;
using System.Runtime.Serialization.Formatters.Binary;

using CommonSnappableTypes;
using ShapeRuntime;
using System.ComponentModel;

namespace Dxfdecoder
{
    [Guid("D71B2219-E698-4210-8DED-4A95DB457C3A")]
    [ComVisible(true)]
    [ClassInterface(ClassInterfaceType.AutoDispatch)]
    public partial class Dxfdecoder : UserControl, IDCCEControl, IControlShape
    {
        #region 0000

        [Browsable(false)]
        public bool isRuning { get; set; }
        public event GetValue GetValueEvent;
        public event SetValue SetValueEvent;
        public event GetDataBase GetDataBaseEvent;
        public event GetVarTable GetVarTableEvent;
        public event GetValue GetSystemItemEvent;
        public event EventHandler TreeNodeClicked;
        public event EventHandler TreeNodeDoubleClicked;
        public event EventHandler TreeNodeSelectedChanged;

        // 序列化
        public Byte[] Serialize()
        {
            SAVE s = saveData;

            BinaryFormatter formatter = new BinaryFormatter();
            MemoryStream memstream = new MemoryStream();
            formatter.Serialize(memstream, s);
            byte[] data = memstream.ToArray();
            memstream.Dispose();
            return data;
        }

        // 反序列化
        public void DeSerialize(Byte[] bytes)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            MemoryStream memstream = new MemoryStream(bytes);
            saveData = (SAVE)formatter.Deserialize(memstream);

            memstream.Dispose();
        }

        // 为HMI环境返回图标	
        public static Image GetLogoStatic()
        {
            ResourceManager rm = new ResourceManager(typeof(图标));
            Bitmap bm = (Bitmap)rm.GetObject("icon");
            return bm;
        }

        public Bitmap GetLogo()
        {
            ResourceManager rm = new ResourceManager(typeof(图标));
            Bitmap bm = (Bitmap)rm.GetObject("icon");
            return bm;
        }

        public void Stop()
        {

        }




        public event EventHandler IDChanged;
        string id = "";
        [DisplayName("名称"), CategoryAttribute("设计"), DescriptionAttribute("设定控件名称。"), ReadOnlyAttribute(false)]
        public string ID
        {
            get { return id; }
            set
            {
                id = value;
                if (IDChanged != null)
                    IDChanged(this, null);
            }
        }

        SAVE saveData = new SAVE();

        #endregion

        public Dxfdecoder()
        {
            InitializeComponent();
        }


        public DxfdecoderDef.Path[] paths; //路径集合，用作处理
        public static DxfdecoderDef.Path[] pathsCopy; //路径集合的副本
        public DxfdecoderDef.Path[] pathTem;
        public  string  fileName; //文件名，路径
        public  double scalePara = 1; //缩放因子
        public Color PenColor = Color.Black;
        public int penScale = 1;
        public static Para a = new Para();

        private DxfdecoderDef.Point currentPoint = new DxfdecoderDef.Point(0.0, 0.0);  //当前执行点

        #region 属性定义
        public double currentPointX
        {
            get { return currentPoint.X; }
            set { currentPoint.X = value; }
        }

        public double currentPointY
        {
            get { return currentPoint.Y; }
            set { currentPoint.Y = value; }
        }
        #endregion 

        #region 外部方法

        /// <summary>
        /// 重新绘图
        /// </summary>
        /// <returns></returns>
        public bool redraw()
        {
            try
            {
                if (paths != null) //路径不为空
                {
                    pathTem = new DxfdecoderDef.Path[pathsCopy.Length];

                    for (int i = 0; i < pathsCopy.Length; i++) //复制路径的拷贝，用作处理
                    {
                        pathTem[i] = pathsCopy[i].Clone();
                    }

                    double[] limit = getLimit(pathTem); //读取图像的边界

                    if (drawbox.Width / (limit[1] - limit[0]) > drawbox.Height / (limit[3] - limit[2])) //计算缩放因子
                    {
                        scalePara = drawbox.Height / (limit[3] - limit[2]);
                    }
                    else
                    {
                        scalePara = drawbox.Width / (limit[1] - limit[0]);
                    }

                    scale(pathTem, scalePara);//缩放
                    limit = getLimit(pathTem);//计算缩放后的图像边界，用作偏移时的参数

                    //图像偏移，使图像居中显示
                    bias(pathTem, drawbox.Width / 2 - (limit[0] + limit[1]) / 2, drawbox.Height / 2 - (limit[2] + limit[3]) / 2);
                    draw(pathTem);//绘制图像
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
        }

        #endregion

        #region 函数内部实现

        private void 选择文件ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog ncFile = new OpenFileDialog(); //新建打开文件对话框
            ncFile.Filter = "All files (*.*)|*.*|txt files (*.txt)|*.txt|dxf files (*.dxf)|*.dxf"; //设置文件类型过滤器
            ncFile.FilterIndex = 3;  //默认文件类型
            ncFile.ShowDialog(); //显示对话框
            if ((fileName = ncFile.FileName) == "")
            {
                return;  //文件名为空，返回
            }
            Pathdecoder.Pathdecoder decoder1 = new Pathdecoder.Pathdecoder(ncFile.FileName);     //新建一个Pathdecoder实例
            paths = decoder1.getPaths();//用getPath方法获得路径的集合

            pathsCopy = new DxfdecoderDef.Path[paths.Length];   //将路径的集合保存到副本
            for (int i = 0; i < paths.Length; i++)
            {
                pathsCopy[i] = paths[i].Clone();
            }

            pathTem = new DxfdecoderDef.Path[pathsCopy.Length];
            for (int i = 0; i < pathsCopy.Length; i++) //复制路径的拷贝，用作处理
            {
                pathTem[i] = pathsCopy[i].Clone();
            }

            double[] limit = getLimit(pathTem); //读取路径的边界，用来计算缩放因子
            if (drawbox.Width / (limit[1] - limit[0]) > drawbox.Height / (limit[3] - limit[2]))
            {
                scalePara = drawbox.Height / (limit[3] - limit[2]);
            }
            else
            {
                scalePara = drawbox.Width / (limit[1] - limit[0]);
            }
            scale(pathTem, scalePara);//进行缩放
            limit = getLimit(pathTem);//读取缩放后的边界，用作图像平移
            //偏移
            bias(pathTem, drawbox.Width / 2 - (limit[0] + limit[1]) / 2, drawbox.Height / 2 - (limit[2] + limit[3]) / 2);
            draw(pathTem);//画图
        }//获取文件并绘图

        private void drawbox_SizeChanged(object sender, EventArgs e)//窗口大小改变后，图像重绘
        {
            if (paths != null) //路径不为空
            {
                pathTem = new DxfdecoderDef.Path[pathsCopy.Length];

                for (int i = 0; i < pathsCopy.Length; i++) //复制路径的拷贝，用作处理
                {
                    pathTem[i] = pathsCopy[i].Clone();
                }

                double[] limit = getLimit(pathTem); //读取图像的边界

                if (drawbox.Width / (limit[1] - limit[0]) > drawbox.Height / (limit[3] - limit[2])) //计算缩放因子
                {
                    scalePara = drawbox.Height / (limit[3] - limit[2]);
                }
                else
                {
                    scalePara = drawbox.Width / (limit[1] - limit[0]);
                }

                scale(pathTem, scalePara);//缩放
                limit = getLimit(pathTem);//计算缩放后的图像边界，用作偏移时的参数

                //图像偏移，使图像居中显示
                bias(pathTem, drawbox.Width / 2 - (limit[0] + limit[1]) / 2, drawbox.Height / 2 - (limit[2] + limit[3]) / 2);
                draw(pathTem);//绘制图像
            }
        }

        /// <summary>
        /// 绘图函数
        /// </summary>
        /// <param name="paths">
        /// 需要绘制的路径集合
        /// </param>
        private void draw(DxfdecoderDef.Path[] paths)
        {
            drawbox.Image = new Bitmap(drawbox.Width, drawbox.Height); //新建一个大小为pictruebox的位图，并用作其图像源
            Graphics g1 = Graphics.FromImage(drawbox.Image); //新建绘图工具
            g1.Clear(Color.White); //图像清空
            Pen pen1 = new Pen(PenColor, Convert.ToInt32(penScale)); //新建一支笔

            scale(paths, 0.997); //图像缩小，不让其与边框重合

            for (int i = 0; i < paths.Length; i++)
            {
                switch (paths[i].Type) //判断路径类型
                {
                    case LineType.Line: //直线类型
                        g1.DrawLine(pen1, Convert.ToSingle(paths[i].StartPoint.X), Convert.ToSingle(paths[i].StartPoint.Y), Convert.ToSingle(paths[i].EndPoint.X), Convert.ToSingle(paths[i].EndPoint.Y)); break;
                    case LineType.Arc: //圆弧类型
                        //新建参考矩形
                        RectangleF rf1 = new RectangleF(Convert.ToSingle(paths[i].CentrePoint.X - paths[i].Radium), Convert.ToSingle(paths[i].CentrePoint.Y - paths[i].Radium), Convert.ToSingle(2 * paths[i].Radium), Convert.ToSingle(2 * paths[i].Radium));
                        g1.DrawArc(pen1, rf1, Convert.ToSingle(paths[i].StartAngle), Convert.ToSingle(paths[i].GetCentrelAngle()));
                        break;
                    case LineType.Circle://圆类型
                        //新建参考矩形
                        RectangleF rf2 = new RectangleF(Convert.ToSingle(paths[i].CentrePoint.X - paths[i].Radium), Convert.ToSingle(paths[i].CentrePoint.Y - paths[i].Radium), Convert.ToSingle(2 * paths[i].Radium), Convert.ToSingle(2 * paths[i].Radium));
                        g1.DrawArc(pen1, rf2, 0.0f, 360.0f);
                        break;
                    default:
                   
                        break;
                }
            }
            drawbox.Image.RotateFlip(RotateFlipType.RotateNoneFlipY);//图像Y翻转
        }

        /// <summary>
        /// 获取图形XY边界
        /// </summary>
        /// <param name="paths">
        /// 需要获取边界的路经集合
        /// </param>
        /// <returns>
        /// 返回double[4]数组，依次为X坐标最小值，X坐标最大值，Y坐标最小值，Y坐标最大值
        /// </returns>
        private double[] getLimit(DxfdecoderDef.Path[] paths)
        {
            double Xmax = -99999999999.0, Ymax = -99999999999.0, Ymin = 99999999999.0, Xmin = 99999999999.0;

            for (int i = 0; i < paths.Length; i++)
            {
                if (paths[i].Type == LineType.Line)
                {
                    Xmax = Math.Max(Xmax, paths[i].StartPoint.X);
                    Xmax = Math.Max(Xmax, paths[i].EndPoint.X);
                    Ymax = Math.Max(Ymax, paths[i].StartPoint.Y);
                    Ymax = Math.Max(Ymax, paths[i].EndPoint.Y);

                    Xmin = Math.Min(Xmin, paths[i].StartPoint.X);
                    Xmin = Math.Min(Xmin, paths[i].EndPoint.X);
                    Ymin = Math.Min(Ymin, paths[i].StartPoint.Y);
                    Ymin = Math.Min(Ymin, paths[i].EndPoint.Y);
                }

                if (paths[i].Type == LineType.Circle)
                {
                    Xmax = Math.Max(Xmax, paths[i].CentrePoint.X + paths[i].Radium);
                    Ymax = Math.Max(Ymax, paths[i].CentrePoint.Y + paths[i].Radium);

                    Xmin = Math.Min(Xmin, paths[i].CentrePoint.X - paths[i].Radium);
                    Ymin = Math.Min(Ymin, paths[i].CentrePoint.Y - paths[i].Radium);
                }

                if (paths[i].Type == LineType.Arc)
                {
                    if (paths[i].GetNode() == 1111)
                    {
                        Xmax = Math.Max(Xmax, paths[i].CentrePoint.X + paths[i].Radium);
                        Ymax = Math.Max(Ymax, paths[i].CentrePoint.Y + paths[i].Radium);

                        Xmin = Math.Min(Xmin, paths[i].CentrePoint.X - paths[i].Radium);
                        Ymin = Math.Min(Ymin, paths[i].CentrePoint.Y - paths[i].Radium);
                    }

                    if (paths[i].GetNode() == 0001)
                    {
                        Xmax = Math.Max(Xmax, paths[i].CentrePoint.X + paths[i].Radium);
                        DxfdecoderDef.Point[] points = new DxfdecoderDef.Point[2];
                        points = paths[i].GetArcTeminal();
                        Xmax = Math.Max(Xmax, points[0].X);
                        Xmax = Math.Max(Xmax, points[1].X);
                        Xmin = Math.Min(Xmin, points[0].X);
                        Xmin = Math.Min(Xmin, points[1].X);
                        Ymax = Math.Max(Ymax, points[0].Y);
                        Ymax = Math.Max(Ymax, points[1].Y);
                        Ymin = Math.Min(Ymin, points[0].Y);
                        Ymin = Math.Min(Ymin, points[1].Y);
                    }

                    if (paths[i].GetNode() == 0010)
                    {
                        Ymax = Math.Max(Ymax, paths[i].CentrePoint.Y + paths[i].Radium);
                        DxfdecoderDef.Point[] points = new DxfdecoderDef.Point[2];
                        points = paths[i].GetArcTeminal();
                        Xmax = Math.Max(Xmax, points[0].X);
                        Xmax = Math.Max(Xmax, points[1].X);
                        Xmin = Math.Min(Xmin, points[0].X);
                        Xmin = Math.Min(Xmin, points[1].X);
                        Ymax = Math.Max(Ymax, points[0].Y);
                        Ymax = Math.Max(Ymax, points[1].Y);
                        Ymin = Math.Min(Ymin, points[0].Y);
                        Ymin = Math.Min(Ymin, points[1].Y);
                    }

                    if (paths[i].GetNode() == 0100)
                    {
                        Xmin = Math.Min(Xmin, paths[i].CentrePoint.X - paths[i].Radium);
                        DxfdecoderDef.Point[] points = new DxfdecoderDef.Point[2];
                        points = paths[i].GetArcTeminal();
                        Xmax = Math.Max(Xmax, points[0].X);
                        Xmax = Math.Max(Xmax, points[1].X);
                        Xmin = Math.Min(Xmin, points[0].X);
                        Xmin = Math.Min(Xmin, points[1].X);
                        Ymax = Math.Max(Ymax, points[0].Y);
                        Ymax = Math.Max(Ymax, points[1].Y);
                        Ymin = Math.Min(Ymin, points[0].Y);
                        Ymin = Math.Min(Ymin, points[1].Y);
                    }

                    if (paths[i].GetNode() == 1000)
                    {
                        Ymin = Math.Min(Ymin, paths[i].CentrePoint.Y + paths[i].Radium);
                        DxfdecoderDef.Point[] points = new DxfdecoderDef.Point[2];
                        points = paths[i].GetArcTeminal();
                        Xmax = Math.Max(Xmax, points[0].X);
                        Xmax = Math.Max(Xmax, points[1].X);
                        Xmin = Math.Min(Xmin, points[0].X);
                        Xmin = Math.Min(Xmin, points[1].X);
                        Ymax = Math.Max(Ymax, points[0].Y);
                        Ymax = Math.Max(Ymax, points[1].Y);
                        Ymin = Math.Min(Ymin, points[0].Y);
                        Ymin = Math.Min(Ymin, points[1].Y);
                    }

                    if (paths[i].GetNode() == 0011)
                    {
                        Xmax = Math.Max(Xmax, paths[i].CentrePoint.X + paths[i].Radium);
                        Ymax = Math.Max(Ymax, paths[i].CentrePoint.Y + paths[i].Radium);
                        DxfdecoderDef.Point[] points = new DxfdecoderDef.Point[2];
                        points = paths[i].GetArcTeminal();
                        Xmax = Math.Max(Xmax, points[0].X);
                        Xmax = Math.Max(Xmax, points[1].X);
                        Xmin = Math.Min(Xmin, points[0].X);
                        Xmin = Math.Min(Xmin, points[1].X);
                        Ymax = Math.Max(Ymax, points[0].Y);
                        Ymax = Math.Max(Ymax, points[1].Y);
                        Ymin = Math.Min(Ymin, points[0].Y);
                        Ymin = Math.Min(Ymin, points[1].Y);
                    }

                    if (paths[i].GetNode() == 0110)
                    {
                        Xmin = Math.Min(Xmin, paths[i].CentrePoint.X - paths[i].Radium);
                        Ymax = Math.Max(Ymax, paths[i].CentrePoint.Y + paths[i].Radium);
                        DxfdecoderDef.Point[] points = new DxfdecoderDef.Point[2];
                        points = paths[i].GetArcTeminal();
                        Xmax = Math.Max(Xmax, points[0].X);
                        Xmax = Math.Max(Xmax, points[1].X);
                        Xmin = Math.Min(Xmin, points[0].X);
                        Xmin = Math.Min(Xmin, points[1].X);
                        Ymax = Math.Max(Ymax, points[0].Y);
                        Ymax = Math.Max(Ymax, points[1].Y);
                        Ymin = Math.Min(Ymin, points[0].Y);
                        Ymin = Math.Min(Ymin, points[1].Y);
                    }

                    if (paths[i].GetNode() == 1100)
                    {
                        Xmin = Math.Min(Xmin, paths[i].CentrePoint.X - paths[i].Radium);
                        Ymin = Math.Min(Ymin, paths[i].CentrePoint.Y - paths[i].Radium);
                        DxfdecoderDef.Point[] points = new DxfdecoderDef.Point[2];
                        points = paths[i].GetArcTeminal();
                        Xmax = Math.Max(Xmax, points[0].X);
                        Xmax = Math.Max(Xmax, points[1].X);
                        Xmin = Math.Min(Xmin, points[0].X);
                        Xmin = Math.Min(Xmin, points[1].X);
                        Ymax = Math.Max(Ymax, points[0].Y);
                        Ymax = Math.Max(Ymax, points[1].Y);
                        Ymin = Math.Min(Ymin, points[0].Y);
                        Ymin = Math.Min(Ymin, points[1].Y);
                    }

                    if (paths[i].GetNode() == 1001)
                    {
                        Xmax = Math.Max(Xmax, paths[i].CentrePoint.X + paths[i].Radium);
                        Ymin = Math.Min(Ymin, paths[i].CentrePoint.Y - paths[i].Radium);
                        DxfdecoderDef.Point[] points = new DxfdecoderDef.Point[2];
                        points = paths[i].GetArcTeminal();
                        Xmax = Math.Max(Xmax, points[0].X);
                        Xmax = Math.Max(Xmax, points[1].X);
                        Xmin = Math.Min(Xmin, points[0].X);
                        Xmin = Math.Min(Xmin, points[1].X);
                        Ymax = Math.Max(Ymax, points[0].Y);
                        Ymax = Math.Max(Ymax, points[1].Y);
                        Ymin = Math.Min(Ymin, points[0].Y);
                        Ymin = Math.Min(Ymin, points[1].Y);
                    }

                    if (paths[i].GetNode() == 0111)
                    {
                        Xmax = Math.Max(Xmax, paths[i].CentrePoint.X + paths[i].Radium);
                        Ymax = Math.Max(Ymax, paths[i].CentrePoint.Y + paths[i].Radium);
                        Xmin = Math.Min(Xmin, paths[i].CentrePoint.X - paths[i].Radium);
                        DxfdecoderDef.Point[] points = new DxfdecoderDef.Point[2];
                        points = paths[i].GetArcTeminal();
                        Xmax = Math.Max(Xmax, points[0].X);
                        Xmax = Math.Max(Xmax, points[1].X);
                        Xmin = Math.Min(Xmin, points[0].X);
                        Xmin = Math.Min(Xmin, points[1].X);
                        Ymax = Math.Max(Ymax, points[0].Y);
                        Ymax = Math.Max(Ymax, points[1].Y);
                        Ymin = Math.Min(Ymin, points[0].Y);
                        Ymin = Math.Min(Ymin, points[1].Y);
                    }

                    if (paths[i].GetNode() == 1110)
                    {
                        Ymin = Math.Min(Ymin, paths[i].CentrePoint.Y - paths[i].Radium);
                        Ymax = Math.Max(Ymax, paths[i].CentrePoint.Y + paths[i].Radium);
                        Xmin = Math.Min(Xmin, paths[i].CentrePoint.X - paths[i].Radium);
                        DxfdecoderDef.Point[] points = new DxfdecoderDef.Point[2];
                        points = paths[i].GetArcTeminal();
                        Xmax = Math.Max(Xmax, points[0].X);
                        Xmax = Math.Max(Xmax, points[1].X);
                        Xmin = Math.Min(Xmin, points[0].X);
                        Xmin = Math.Min(Xmin, points[1].X);
                        Ymax = Math.Max(Ymax, points[0].Y);
                        Ymax = Math.Max(Ymax, points[1].Y);
                        Ymin = Math.Min(Ymin, points[0].Y);
                        Ymin = Math.Min(Ymin, points[1].Y);
                    }

                    if (paths[i].GetNode() == 1101)
                    {
                        Ymin = Math.Min(Ymin, paths[i].CentrePoint.Y - paths[i].Radium);
                        Xmax = Math.Max(Xmax, paths[i].CentrePoint.X + paths[i].Radium);
                        Xmin = Math.Min(Xmin, paths[i].CentrePoint.X - paths[i].Radium);
                        DxfdecoderDef.Point[] points = new DxfdecoderDef.Point[2];
                        points = paths[i].GetArcTeminal();
                        Xmax = Math.Max(Xmax, points[0].X);
                        Xmax = Math.Max(Xmax, points[1].X);
                        Xmin = Math.Min(Xmin, points[0].X);
                        Xmin = Math.Min(Xmin, points[1].X);
                        Ymax = Math.Max(Ymax, points[0].Y);
                        Ymax = Math.Max(Ymax, points[1].Y);
                        Ymin = Math.Min(Ymin, points[0].Y);
                        Ymin = Math.Min(Ymin, points[1].Y);
                    }

                    if (paths[i].GetNode() == 1011)
                    {
                        Xmax = Math.Max(Xmax, paths[i].CentrePoint.X + paths[i].Radium);
                        Ymax = Math.Max(Ymax, paths[i].CentrePoint.Y + paths[i].Radium);
                        Ymin = Math.Min(Ymin, paths[i].CentrePoint.Y - paths[i].Radium);
                        DxfdecoderDef.Point[] points = new DxfdecoderDef.Point[2];
                        points = paths[i].GetArcTeminal();
                        Xmax = Math.Max(Xmax, points[0].X);
                        Xmax = Math.Max(Xmax, points[1].X);
                        Xmin = Math.Min(Xmin, points[0].X);
                        Xmin = Math.Min(Xmin, points[1].X);
                        Ymax = Math.Max(Ymax, points[0].Y);
                        Ymax = Math.Max(Ymax, points[1].Y);
                        Ymin = Math.Min(Ymin, points[0].Y);
                        Ymin = Math.Min(Ymin, points[1].Y);
                    }

                    if (paths[i].GetNode() == 0000)
                    {
                        DxfdecoderDef.Point[] points = new DxfdecoderDef.Point[2];
                        points = paths[i].GetArcTeminal();
                        Xmax = Math.Max(Xmax, points[0].X);
                        Xmax = Math.Max(Xmax, points[1].X);
                        Xmin = Math.Min(Xmin, points[0].X);
                        Xmin = Math.Min(Xmin, points[1].X);
                        Ymax = Math.Max(Ymax, points[0].Y);
                        Ymax = Math.Max(Ymax, points[1].Y);
                        Ymin = Math.Min(Ymin, points[0].Y);
                        Ymin = Math.Min(Ymin, points[1].Y);
                    }
                }
            }
            double[] temp = new double[4] { Xmin, Xmax, Ymin, Ymax };

            // MessageBox.Show(Xmin.ToString("0.0") + Xmax.ToString("0.0") + Ymin.ToString("0.0") + Ymax.ToString("0.0"));


            return temp;
        }

        /// <summary>
        /// 图像缩放函数
        /// </summary>
        /// <param name="paths">
        /// 需要缩放的路径集合
        /// </param>
        /// <param name="scalePara">
        /// 缩放的比例因子
        /// </param>
        private void scale(DxfdecoderDef.Path[] paths, double scalePara)
        {

            for (int i = 0; i < paths.Length; i++)
            {
                paths[i].CentrePoint.X *= scalePara;
                paths[i].CentrePoint.Y *= scalePara;
                paths[i].EndPoint.X *= scalePara;
                paths[i].EndPoint.Y *= scalePara;
                paths[i].Radium *= scalePara;
                paths[i].StartPoint.X *= scalePara;
                paths[i].StartPoint.Y *= scalePara;
            }
        }

        /// <summary>
        /// 图像偏移函数
        /// </summary>
        /// <param name="paths">
        /// 需要偏移的路径集合
        /// </param> 
        /// <param name="biasX">
        /// X偏移
        /// </param> 
        /// <param name="biasY">
        /// Y偏移
        /// </param> 
        private void bias(DxfdecoderDef.Path[] paths, double biasX, double biasY)
        {
            for (int i = 0; i < paths.Length; i++)
            {
                paths[i].CentrePoint.X += biasX;
                paths[i].CentrePoint.Y += biasY;
                paths[i].EndPoint.X += biasX;
                paths[i].EndPoint.Y += biasY;
                paths[i].StartPoint.X += biasX;
                paths[i].StartPoint.Y += biasY;
            }

        }

        private void dxfdecoder1_Load(object sender, EventArgs e)
        {
            toolStripComboBox2.Text = penScale.ToString() + "像素";
        }

        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            ColorDialog colorchooser1 = new ColorDialog();
            colorchooser1.ShowDialog();
            PenColor = colorchooser1.Color;
            if (pathTem != null)
            {
                draw(pathTem);
            }
        }

        private void toolStripComboBox2_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            penScale = toolStripComboBox2.SelectedIndex + 1;
            if (pathTem != null)
            {
                draw(pathTem);
            }
        }

        private void 显示图像信息ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(paths != null)
            {
                if (a.IsDisposed == true)
                {
                    a = new Para();
                    a.Show();
                }

                else
                {
                    a.Show();
                    a.Focus();
                }
            }
            else
            {
                MessageBox.Show("请先选择文件！");
                return;
            }
       
        }
        #endregion

        private void 生成NC代码ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            NCGenerator nc1 = new NCGenerator();
            nc1.Show();
            nc1.TopMost = true;
        }

    }
    // 控件信息保存类，序列化数据报表控件的信息
    [Serializable]
    public class SAVE
    {


    }
}
