using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DxfdecoderDef;
using Dxfdecoder;
using System.IO;

namespace Dxfdecoder.DxfdecoderUc
{
    public partial class NCGenerator : Form
    {
        DxfdecoderDef.Path[] pathcopy;//用于生成G代码的临时路径集

        int bLength, bWidth, scalePer, dDeepth, dHeight;
        double scalePara;

        string GfilePath, GfileName;


        private void NCGenerator_Load(object sender, EventArgs e)
        {
            if (Dxfdecoder.pathsCopy != null)
            {
                pathcopy = new DxfdecoderDef.Path[Dxfdecoder.pathsCopy.Length];   //将路径的集合保存到副本
                for (int i = 0; i < Dxfdecoder.pathsCopy.Length; i++)
                {
                    pathcopy[i] = Dxfdecoder.pathsCopy[i].Clone();
                }
                bLength = Convert.ToInt32(textBox2.Text);
                bWidth = Convert.ToInt32(textBox3.Text);
                dDeepth = 0 - Convert.ToInt32(textBox4.Text);
                dHeight = Convert.ToInt32(textBox5.Text);
                scalePer = Convert.ToInt32(textBox6.Text);
                textBox7.Text = DateTime.Now.Year.ToString() + "-" + DateTime.Now.Month.ToString() + "-" + DateTime.Now.Day.ToString() + "_" + DateTime.Now.Hour.ToString() + "-" + DateTime.Now.Minute.ToString() + "-" + DateTime.Now.Second.ToString();
            }
            else
            {
                MessageBox.Show("请先选择文件！");
                this.Close();
                return;
            }
        }
        public NCGenerator()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 选择文件夹
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd1 = new FolderBrowserDialog();
            fbd1.ShowNewFolderButton = true;
            fbd1.ShowDialog();
            if(fbd1.SelectedPath != "")
            {
                textBox1.Text = fbd1.SelectedPath;
            }
            else
            {
                MessageBox.Show("未选择路径！");
                return;
            }
            
        }

        /// <summary>
        /// 生成按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button2_Click(object sender, EventArgs e)
        {
            GfilePath = textBox1.Text;
            GfileName = textBox7.Text + ".nc";

            if(GfileName == "" || GfilePath == "")
            {
                MessageBox.Show("请输入正确的路径或者文件名！");
                return;
            }

            StreamWriter sw1 = new StreamWriter(GfilePath + @"\" + GfileName, false, Encoding.UTF8);

            DxfdecoderDef.Path[] pathTem = new DxfdecoderDef.Path[pathcopy.Length];
            for (int i = 0; i < pathcopy.Length; i++) //复制路径的拷贝，用作处理
            {
                pathTem[i] = pathcopy[i].Clone();
            }

            double[] limit = getLimit(pathTem); //读取路径的边界，用来计算缩放因子
            if (bLength / (limit[1] - limit[0]) > bWidth / (limit[3] - limit[2]))
            {
                scalePara = bWidth / (limit[3] - limit[2]);
            }
            else
            {
                scalePara = bLength / (limit[1] - limit[0]);
            }
            scale(pathTem, scalePara * scalePer / 100);//进行缩放
            limit = getLimit(pathTem);//读取缩放后的边界，用作图像平移
            //偏移
            bias(pathTem, 0 - (limit[0] + limit[1]) / 2, 0 - (limit[2] + limit[3]) / 2);

            sw1.WriteLine("G00 X0.000 Y0.000 Z" + dHeight.ToString("0.000"));
            for (int i = 0; i < pathTem.Length; i++)
            {  
                if (pathTem[i].Type == LineType.Arc)
                {
                    DxfdecoderDef.Point[] ArcTeminal = pathTem[i].GetArcTeminal();

                    sw1.WriteLine("G00 X" + ArcTeminal[0].X.ToString("0.000") + " Y" + ArcTeminal[0].Y.ToString("0.000") + " Z" + dHeight.ToString("0.000"));
                    sw1.WriteLine("G00 X" + ArcTeminal[0].X.ToString("0.000") + " Y" + ArcTeminal[0].Y.ToString("0.000") + " Z" + dDeepth.ToString("0.000"));
                    sw1.WriteLine("G03 G17 X" + ArcTeminal[1].X.ToString("0.000") + " Y" + ArcTeminal[1].Y.ToString("0.000") + " R" + pathTem[i].Radium.ToString("0.000"));
                    sw1.WriteLine("G00 X" + ArcTeminal[1].X.ToString("0.000") + " Y" + ArcTeminal[1].Y.ToString("0.000") + " Z" + dHeight.ToString("0.000"));

                }
                if (pathTem[i].Type == LineType.Circle)
                {
                    double onePointX = pathTem[i].CentrePoint.X, onePointY = pathTem[i].CentrePoint.Y - pathTem[i].Radium;
                    sw1.WriteLine("G00 X" + onePointX.ToString("0.000") + " Y" + onePointY.ToString("0.000") + " Z" + dHeight.ToString("0.000"));
                    sw1.WriteLine("G00 X" + onePointX.ToString("0.000") + " Y" + onePointY.ToString("0.000") + " Z" + dDeepth.ToString("0.000"));
                    sw1.WriteLine("G03 G17 X" + onePointX.ToString("0.000") + " Y" + onePointY.ToString("0.000") + " R" + pathTem[i].Radium.ToString("0.000"));
                    sw1.WriteLine("G00 X" + onePointX.ToString("0.000") + " Y" + onePointY.ToString("0.000") + " Z" + dHeight.ToString("0.000"));


                }
                if (pathTem[i].Type == LineType.Line)
                {
                    sw1.WriteLine("G00 X" + pathTem[i].StartPoint.X.ToString("0.000") + " Y" + pathTem[i].StartPoint.Y.ToString("0.000") + " Z" + dHeight.ToString("0.000"));
                    sw1.WriteLine("G00 X" + pathTem[i].StartPoint.X.ToString("0.000") + " Y" + pathTem[i].StartPoint.Y.ToString("0.000") + " Z" + dDeepth.ToString("0.000"));
                    sw1.WriteLine("G01 X" + pathTem[i].EndPoint.X.ToString("0.000") + " Y" + pathTem[i].EndPoint.Y.ToString("0.000") + " Z" + dDeepth.ToString("0.000"));
                    sw1.WriteLine("G00 X" + pathTem[i].EndPoint.X.ToString("0.000") + " Y" + pathTem[i].EndPoint.Y.ToString("0.000") + " Z" + dHeight.ToString("0.000"));
                }
       
            }
            sw1.WriteLine("M30");
            sw1.Close();
            MessageBox.Show("已生成");

        }

        

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


    }
}
