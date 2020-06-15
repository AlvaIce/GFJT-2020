namespace QRST_DI_DataImportTool.ctrlSingleImport
{
    partial class ctrlVectorMetaData
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.textSrc = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.textOrg = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.textMetaData = new System.Windows.Forms.TextBox();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.textSrc);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.textOrg);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.textMetaData);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(586, 326);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "矢量数据属性信息";
            // 
            // textSrc
            // 
            this.textSrc.Location = new System.Drawing.Point(368, 240);
            this.textSrc.Name = "textSrc";
            this.textSrc.Size = new System.Drawing.Size(194, 21);
            this.textSrc.TabIndex = 4;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(297, 243);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(65, 12);
            this.label2.TabIndex = 3;
            this.label2.Text = "数据来源：";
            // 
            // textOrg
            // 
            this.textOrg.Location = new System.Drawing.Point(79, 236);
            this.textOrg.Name = "textOrg";
            this.textOrg.Size = new System.Drawing.Size(194, 21);
            this.textOrg.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(18, 243);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 12);
            this.label1.TabIndex = 1;
            this.label1.Text = "生产机构：";
            // 
            // textMetaData
            // 
            this.textMetaData.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.textMetaData.Location = new System.Drawing.Point(6, 20);
            this.textMetaData.Multiline = true;
            this.textMetaData.Name = "textMetaData";
            this.textMetaData.ReadOnly = true;
            this.textMetaData.Size = new System.Drawing.Size(574, 210);
            this.textMetaData.TabIndex = 0;
            // 
            // ctrlVectorMetaData
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBox1);
            this.Name = "ctrlVectorMetaData";
            this.Size = new System.Drawing.Size(586, 326);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox textSrc;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textOrg;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textMetaData;
    }
}
