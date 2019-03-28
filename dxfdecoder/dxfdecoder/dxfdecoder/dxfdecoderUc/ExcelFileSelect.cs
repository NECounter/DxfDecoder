using System;
using System.Text;
using System.Windows.Forms;
using System.IO;
using DxfdecoderDef;


namespace Dxfdecoder.DxfdecoderUc
{

    public partial class ExcelFileSelect : Form
    {
        private string excelFilePath = "";
        private string excelFileName = "";

        public ExcelFileSelect()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd1 = new FolderBrowserDialog();
            fbd1.ShowDialog();
            if(fbd1.SelectedPath == "")
            {
                return;
            }
            excelFilePath = fbd1.SelectedPath;
            textBox1.Text = excelFilePath;
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            excelFileName = textBox2.Text + ".csv";
        }

        private void button2_Click(object sender, EventArgs e)
        {

            if(excelFilePath == "" || excelFileName == "")
            {
                MessageBox.Show("请输入正确的路径以及文件名！");
                return;
            }
            

            StreamWriter sw1 = new StreamWriter(excelFilePath +@"\" +excelFileName, false, Encoding.UTF8);
            if (Dxfdecoder.pathsCopy != null)
            {
                DxfdecoderDef.Path[] pathsCopy = Dxfdecoder.pathsCopy;
                
                string num = "", type = "", spoint = "", epoint = "", centrel = "", radium = "", sangle = "", eangle = "", length = "";
                sw1.WriteLine("序号,路径类型,起点,终点,圆心,半径,起始角度,终止角度,长度");
                for (int i = 0; i < pathsCopy.Length; i++)
                {
                    num = "路径" + (i + 1).ToString();
                    if (pathsCopy[i].Type == LineType.Line)
                    {
                        type = ("直线");
                        spoint = (pathsCopy[i].StartPoint.X.ToString("0.000") + "，" + pathsCopy[i].StartPoint.Y.ToString("0.000"));
                        epoint = (pathsCopy[i].EndPoint.X.ToString("0.000") + "，" + pathsCopy[i].EndPoint.Y.ToString("0.000"));
                        centrel = (" ");
                        radium = (" ");
                        sangle = (" ");
                        eangle = (" ");
                        length = (pathsCopy[i].GetPathLength().ToString("0.000"));
                    }
                    if (pathsCopy[i].Type == LineType.Arc)
                    {
                        type = ("圆弧");
                        Point[] p2 = pathsCopy[i].GetArcTeminal();
                        spoint = (p2[0].X.ToString("0.000") + "，" + p2[0].Y.ToString("0.000"));
                        epoint = (p2[1].X.ToString("0.000") + "，" + p2[1].Y.ToString("0.000"));
                        centrel = (pathsCopy[i].CentrePoint.X.ToString("0.000") + "，" + pathsCopy[i].CentrePoint.Y.ToString("0.000"));
                        radium = (pathsCopy[i].Radium.ToString("0.000"));
                        sangle = (pathsCopy[i].StartAngle.ToString("0.000"));
                        eangle = (pathsCopy[i].EndAngle.ToString("0.000"));
                        length = (pathsCopy[i].GetPathLength().ToString("0.000"));
                    }
                    if (pathsCopy[i].Type == LineType.Circle)
                    {
                        type = ("整圆");

                        spoint = (" ");
                        epoint = (" ");
                        centrel= (pathsCopy[i].CentrePoint.X.ToString("0.000") + "，" + pathsCopy[i].CentrePoint.Y.ToString("0.000"));
                        radium = (pathsCopy[i].Radium.ToString("0.000"));
                        sangle = (" ");
                        eangle = (" ");
                        length = (pathsCopy[i].GetPathLength().ToString("0.000"));
                    }
                    sw1.WriteLine(num + "," + type + "," + spoint + "," + epoint + "," + centrel + "," + radium + "," + sangle + "," + eangle + "," + length);                    
                }
                sw1.Close();
                MessageBox.Show("保存成功!");
                this.Close();
            }
            else
            {
                MessageBox.Show("没有可以导出的路径集!");
            }
            
        }

        private void ExcelFileSelect_FormClosed(object sender, FormClosedEventArgs e)
        {
            Dxfdecoder.a.TopMost = true;
        }

    }
}
