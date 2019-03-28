namespace Dxfdecoder.DxfdecoderUc
{
    partial class Para
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.listView1 = new System.Windows.Forms.ListView();
            this.Index = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.Type = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.StartPoint = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.EndPoint = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.CentrelPoint = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.Radium = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.StartAngle = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.EndAngle = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.Length = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.Others = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.button1 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // listView1
            // 
            this.listView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.Index,
            this.Type,
            this.StartPoint,
            this.EndPoint,
            this.CentrelPoint,
            this.Radium,
            this.StartAngle,
            this.EndAngle,
            this.Length,
            this.Others});
            this.listView1.Location = new System.Drawing.Point(12, 12);
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(918, 503);
            this.listView1.TabIndex = 0;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.View = System.Windows.Forms.View.Details;
            this.listView1.SizeChanged += new System.EventHandler(this.listView1_SizeChanged);
            // 
            // Index
            // 
            this.Index.Text = "序号";
            // 
            // Type
            // 
            this.Type.Text = "路径类型";
            this.Type.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // StartPoint
            // 
            this.StartPoint.Text = "起点";
            this.StartPoint.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // EndPoint
            // 
            this.EndPoint.Text = "终点";
            this.EndPoint.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // CentrelPoint
            // 
            this.CentrelPoint.Text = "圆心";
            this.CentrelPoint.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // Radium
            // 
            this.Radium.Text = "半径";
            this.Radium.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // StartAngle
            // 
            this.StartAngle.Text = "起始角";
            this.StartAngle.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // EndAngle
            // 
            this.EndAngle.Text = "终止角";
            this.EndAngle.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // Length
            // 
            this.Length.Text = "长度";
            this.Length.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // Others
            // 
            this.Others.Text = "备注";
            this.Others.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // button1
            // 
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button1.Location = new System.Drawing.Point(855, 527);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 1;
            this.button1.Text = "保存到文件";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // para
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(942, 555);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.listView1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.MinimumSize = new System.Drawing.Size(500, 300);
            this.Name = "para";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "para";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.para_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListView listView1;
        private System.Windows.Forms.ColumnHeader Index;
        private System.Windows.Forms.ColumnHeader Type;
        private System.Windows.Forms.ColumnHeader StartPoint;
        private System.Windows.Forms.ColumnHeader EndPoint;
        private System.Windows.Forms.ColumnHeader CentrelPoint;
        private System.Windows.Forms.ColumnHeader Radium;
        private System.Windows.Forms.ColumnHeader StartAngle;
        private System.Windows.Forms.ColumnHeader EndAngle;
        private System.Windows.Forms.ColumnHeader Length;
        private System.Windows.Forms.ColumnHeader Others;
        private System.Windows.Forms.Button button1;
    }
}