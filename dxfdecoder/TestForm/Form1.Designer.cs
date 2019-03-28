namespace TestForm
{
    partial class Form1
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.dxfdecoder11 = new Dxfdecoder.Dxfdecoder();
            this.SuspendLayout();
            // 
            // dxfdecoder11
            // 
            this.dxfdecoder11.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.dxfdecoder11.currentPointX = 0D;
            this.dxfdecoder11.currentPointY = 0D;
            this.dxfdecoder11.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dxfdecoder11.Location = new System.Drawing.Point(0, 0);
            this.dxfdecoder11.Name = "dxfdecoder11";
            this.dxfdecoder11.Size = new System.Drawing.Size(920, 548);
            this.dxfdecoder11.TabIndex = 0;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(920, 548);
            this.Controls.Add(this.dxfdecoder11);
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Form1";
            this.ResumeLayout(false);

        }

        #endregion

        private Dxfdecoder.Dxfdecoder dxfdecoder11;
    }
}

