namespace QRST_DI_MS_Desktop.UserInterfaces
{
    partial class ColorRange
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
            this.labelMin = new DevExpress.XtraEditors.LabelControl();
            this.labelControlMax = new DevExpress.XtraEditors.LabelControl();
            this.SuspendLayout();
            // 
            // labelMin
            // 
            this.labelMin.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.labelMin.Location = new System.Drawing.Point(32, 227);
            this.labelMin.Name = "labelMin";
            this.labelMin.Size = new System.Drawing.Size(7, 14);
            this.labelMin.TabIndex = 1;
            this.labelMin.Text = "0";
            // 
            // labelControlMax
            // 
            this.labelControlMax.Location = new System.Drawing.Point(32, 0);
            this.labelControlMax.Name = "labelControlMax";
            this.labelControlMax.Size = new System.Drawing.Size(21, 14);
            this.labelControlMax.TabIndex = 2;
            this.labelControlMax.Text = "100";
            // 
            // ColorRange
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.labelControlMax);
            this.Controls.Add(this.labelMin);
            this.Name = "ColorRange";
            this.Size = new System.Drawing.Size(56, 244);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.ColorRange_Paint);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.LabelControl labelMin;
        private DevExpress.XtraEditors.LabelControl labelControlMax;
    }
}
