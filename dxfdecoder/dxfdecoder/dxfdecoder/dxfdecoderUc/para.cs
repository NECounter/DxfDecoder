using System;
using System.Windows.Forms;
using DxfdecoderDef;

namespace Dxfdecoder.DxfdecoderUc
{

    public partial class Para : Form
    {

        private ExcelFileSelect efs = new ExcelFileSelect();
        public Para()
        {
            InitializeComponent();
        }

        private void para_Load(object sender, EventArgs e)
        {
            if(Dxfdecoder.pathsCopy != null)
            {
                Path[] pathsCopy = Dxfdecoder.pathsCopy;
                ListViewItem[] items = new ListViewItem[pathsCopy.Length];
                for (int i = 0; i < pathsCopy.Length; i++)
                {
                    items[i] = new ListViewItem("路径" + i.ToString());
                    if(pathsCopy[i].Type == LineType.Line)
                    {
                        items[i].SubItems.Add("直线");
                        items[i].SubItems.Add(pathsCopy[i].StartPoint.X.ToString("0.000") + " , " + pathsCopy[i].StartPoint.Y.ToString("0.000"));
                        items[i].SubItems.Add(pathsCopy[i].EndPoint.X.ToString("0.000") + " , " + pathsCopy[i].EndPoint.Y.ToString("0.000"));
                        items[i].SubItems.Add(" ");
                        items[i].SubItems.Add(" ");
                        items[i].SubItems.Add(" ");
                        items[i].SubItems.Add(" ");
                        items[i].SubItems.Add(pathsCopy[i].GetPathLength().ToString("0.000"));
                    }
                    if (pathsCopy[i].Type == LineType.Arc)
                    {
                        items[i].SubItems.Add("圆弧");
                        Point[] p2 = pathsCopy[i].GetArcTeminal();
                        items[i].SubItems.Add(p2[0].X.ToString("0.000") + " , " + p2[0].Y.ToString("0.000"));
                        items[i].SubItems.Add(p2[1].X.ToString("0.000") + " , " + p2[1].Y.ToString("0.000"));
                        items[i].SubItems.Add(pathsCopy[i].CentrePoint.X.ToString("0.000") + " , " + pathsCopy[i].CentrePoint.Y.ToString("0.000"));
                        items[i].SubItems.Add(pathsCopy[i].Radium.ToString("0.000"));
                        items[i].SubItems.Add(pathsCopy[i].StartAngle.ToString("0.000"));
                        items[i].SubItems.Add(pathsCopy[i].EndAngle.ToString("0.000"));
                        items[i].SubItems.Add(pathsCopy[i].GetPathLength().ToString("0.000"));
                    }
                    if (pathsCopy[i].Type == LineType.Circle)
                    {
                        items[i].SubItems.Add("整圆");             
                        items[i].SubItems.Add(" ");
                        items[i].SubItems.Add(" ");
                        items[i].SubItems.Add(pathsCopy[i].CentrePoint.X.ToString("0.000") + " , " + pathsCopy[i].CentrePoint.Y.ToString("0.000"));
                        items[i].SubItems.Add(pathsCopy[i].Radium.ToString("0.000"));
                        items[i].SubItems.Add(" ");
                        items[i].SubItems.Add(" ");
                        items[i].SubItems.Add(pathsCopy[i].GetPathLength().ToString("0.000"));
                    }
                    listView1.Items.Add(items[i]);
                }
            }
    
            listView1.Columns[0].Width = listView1.Width / 16;
            listView1.Columns[1].Width = listView1.Width / 16 * 1;
            listView1.Columns[2].Width = listView1.Width / 16 * 3;
            listView1.Columns[3].Width = listView1.Width / 16 * 3;
            listView1.Columns[4].Width = listView1.Width / 16 * 3;
            listView1.Columns[5].Width = listView1.Width / 16 * 1;
            listView1.Columns[6].Width = listView1.Width / 16 * 1;
            listView1.Columns[7].Width = listView1.Width / 16 * 1;
            listView1.Columns[8].Width = listView1.Width / 16 * 1;
            listView1.Columns[9].Width = listView1.Width / 16 * 1;
        }

        private void listView1_SizeChanged(object sender, EventArgs e)
        {
          
            listView1.Columns[0].Width = listView1.Width / 16;
            listView1.Columns[1].Width = listView1.Width / 16 * 1;
            listView1.Columns[2].Width = listView1.Width / 16 * 3;
            listView1.Columns[3].Width = listView1.Width / 16 * 3;
            listView1.Columns[4].Width = listView1.Width / 16 * 3;
            listView1.Columns[5].Width = listView1.Width / 16 * 1;
            listView1.Columns[6].Width = listView1.Width / 16 * 1;
            listView1.Columns[7].Width = listView1.Width / 16 * 1;
            listView1.Columns[8].Width = listView1.Width / 16 * 1;
            listView1.Columns[9].Width = listView1.Width / 16 * 1;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (efs.IsDisposed == true)
            {
                efs = new ExcelFileSelect();
                efs.Show();
                this.TopMost = false;
            }
            else
            {
                efs.Show();
                efs.Focus();
                this.TopMost = false;
            }      
        }
    }
}
