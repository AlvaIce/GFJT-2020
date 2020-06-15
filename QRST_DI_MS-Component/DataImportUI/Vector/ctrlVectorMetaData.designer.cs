namespace QRST_DI_MS_Component_DataImportorUI.Vector
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
            this.textMetaData = new System.Windows.Forms.TextBox();
            this.userMDSettingCtrl = new QRST_DI_MS_Component_DataImportorUI.Vector.CtrlVectorUserMetaDataSetting();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.textMetaData);
            this.groupBox1.Controls.Add(this.userMDSettingCtrl);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(586, 328);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "矢量数据属性信息";
            // 
            // textMetaData
            // 
            this.textMetaData.Location = new System.Drawing.Point(6, 20);
            this.textMetaData.Multiline = true;
            this.textMetaData.Name = "textMetaData";
            this.textMetaData.ReadOnly = true;
            this.textMetaData.Size = new System.Drawing.Size(574, 170);
            this.textMetaData.TabIndex = 0;
            // 
            // userMDSettingCtrl
            // 
            this.userMDSettingCtrl.Location = new System.Drawing.Point(33, 188);
            this.userMDSettingCtrl.Name = "userMDSettingCtrl";
            this.userMDSettingCtrl.Size = new System.Drawing.Size(529, 169);
            this.userMDSettingCtrl.TabIndex = 1;
            // 
            // ctrlVectorMetaData
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBox1);
            this.Name = "ctrlVectorMetaData";
            this.Size = new System.Drawing.Size(586, 328);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox textMetaData;
        private CtrlVectorUserMetaDataSetting userMDSettingCtrl;
    }
}
