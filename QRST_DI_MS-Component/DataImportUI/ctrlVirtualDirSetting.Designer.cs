namespace QRST_DI_MS_Component.DataImportUI
{
    partial class ctrlVirtualDirSetting
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
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.chk_DirImportMode = new System.Windows.Forms.CheckBox();
            this.button1 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // textBox2
            // 
            this.textBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox2.Enabled = false;
            this.textBox2.Location = new System.Drawing.Point(129, 3);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(360, 21);
            this.textBox2.TabIndex = 42;
            this.textBox2.Text = "root\\sdfsd\\";
            // 
            // chk_DirImportMode
            // 
            this.chk_DirImportMode.AutoSize = true;
            this.chk_DirImportMode.Location = new System.Drawing.Point(3, 5);
            this.chk_DirImportMode.Name = "chk_DirImportMode";
            this.chk_DirImportMode.Size = new System.Drawing.Size(120, 16);
            this.chk_DirImportMode.TabIndex = 40;
            this.chk_DirImportMode.Text = "添加至虚拟文件夹";
            this.chk_DirImportMode.UseVisualStyleBackColor = true;
            this.chk_DirImportMode.CheckedChanged += new System.EventHandler(this.chk_DirImportMode_CheckedChanged);
            // 
            // button1
            // 
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button1.Enabled = false;
            this.button1.Location = new System.Drawing.Point(495, 1);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(32, 23);
            this.button1.TabIndex = 43;
            this.button1.Text = "...";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // ctrlVirtualDirSetting
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.button1);
            this.Controls.Add(this.textBox2);
            this.Controls.Add(this.chk_DirImportMode);
            this.Name = "ctrlVirtualDirSetting";
            this.Size = new System.Drawing.Size(531, 26);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public System.Windows.Forms.TextBox textBox2;
        public System.Windows.Forms.Button button1;
        public System.Windows.Forms.CheckBox chk_DirImportMode;
    }
}
