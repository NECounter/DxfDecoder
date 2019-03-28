namespace Dxfdecoder
{
    partial class Dxfdecoder
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

        #region 组件设计器生成的代码

        /// <summary> 
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.drawbox = new System.Windows.Forms.PictureBox();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.工具ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.选择文件ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.显示图像信息ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.生成NC代码ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem3 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripComboBox2 = new System.Windows.Forms.ToolStripComboBox();
            ((System.ComponentModel.ISupportInitialize)(this.drawbox)).BeginInit();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // drawbox
            // 
            this.drawbox.Cursor = System.Windows.Forms.Cursors.Default;
            this.drawbox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.drawbox.InitialImage = null;
            this.drawbox.Location = new System.Drawing.Point(0, 25);
            this.drawbox.Name = "drawbox";
            this.drawbox.Size = new System.Drawing.Size(878, 469);
            this.drawbox.TabIndex = 9;
            this.drawbox.TabStop = false;
            this.drawbox.SizeChanged += new System.EventHandler(this.drawbox_SizeChanged);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.工具ToolStripMenuItem,
            this.toolStripMenuItem1});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(878, 25);
            this.menuStrip1.TabIndex = 10;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // 工具ToolStripMenuItem
            // 
            this.工具ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.选择文件ToolStripMenuItem,
            this.显示图像信息ToolStripMenuItem,
            this.生成NC代码ToolStripMenuItem});
            this.工具ToolStripMenuItem.Name = "工具ToolStripMenuItem";
            this.工具ToolStripMenuItem.Size = new System.Drawing.Size(44, 21);
            this.工具ToolStripMenuItem.Text = "工具";
            // 
            // 选择文件ToolStripMenuItem
            // 
            this.选择文件ToolStripMenuItem.Name = "选择文件ToolStripMenuItem";
            this.选择文件ToolStripMenuItem.Size = new System.Drawing.Size(148, 22);
            this.选择文件ToolStripMenuItem.Text = "选择文件";
            this.选择文件ToolStripMenuItem.Click += new System.EventHandler(this.选择文件ToolStripMenuItem_Click);
            // 
            // 显示图像信息ToolStripMenuItem
            // 
            this.显示图像信息ToolStripMenuItem.Name = "显示图像信息ToolStripMenuItem";
            this.显示图像信息ToolStripMenuItem.Size = new System.Drawing.Size(148, 22);
            this.显示图像信息ToolStripMenuItem.Text = "显示图像信息";
            this.显示图像信息ToolStripMenuItem.Click += new System.EventHandler(this.显示图像信息ToolStripMenuItem_Click);
            // 
            // 生成NC代码ToolStripMenuItem
            // 
            this.生成NC代码ToolStripMenuItem.Name = "生成NC代码ToolStripMenuItem";
            this.生成NC代码ToolStripMenuItem.Size = new System.Drawing.Size(148, 22);
            this.生成NC代码ToolStripMenuItem.Text = "生成NC代码";
            this.生成NC代码ToolStripMenuItem.Click += new System.EventHandler(this.生成NC代码ToolStripMenuItem_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem2,
            this.toolStripMenuItem3});
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(68, 21);
            this.toolStripMenuItem1.Text = "画笔参数";
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(124, 22);
            this.toolStripMenuItem2.Text = "画笔颜色";
            this.toolStripMenuItem2.Click += new System.EventHandler(this.toolStripMenuItem2_Click);
            // 
            // toolStripMenuItem3
            // 
            this.toolStripMenuItem3.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripComboBox2});
            this.toolStripMenuItem3.Name = "toolStripMenuItem3";
            this.toolStripMenuItem3.Size = new System.Drawing.Size(124, 22);
            this.toolStripMenuItem3.Text = "画笔宽度";
            // 
            // toolStripComboBox2
            // 
            this.toolStripComboBox2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.toolStripComboBox2.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.toolStripComboBox2.Items.AddRange(new object[] {
            "1像素",
            "2像素",
            "3像素",
            "4像素",
            "5像素",
            "6像素",
            "7像素",
            "8像素"});
            this.toolStripComboBox2.Name = "toolStripComboBox2";
            this.toolStripComboBox2.Size = new System.Drawing.Size(121, 25);
            this.toolStripComboBox2.SelectedIndexChanged += new System.EventHandler(this.toolStripComboBox2_SelectedIndexChanged_1);
            // 
            // Dxfdecoder
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.Controls.Add(this.drawbox);
            this.Controls.Add(this.menuStrip1);
            this.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.MinimumSize = new System.Drawing.Size(100, 200);
            this.Name = "Dxfdecoder";
            this.Size = new System.Drawing.Size(878, 494);
            this.Load += new System.EventHandler(this.dxfdecoder1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.drawbox)).EndInit();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.PictureBox drawbox;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem 工具ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 选择文件ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 显示图像信息ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem3;
        private System.Windows.Forms.ToolStripComboBox toolStripComboBox2;
        private System.Windows.Forms.ToolStripMenuItem 生成NC代码ToolStripMenuItem;
    }
}
