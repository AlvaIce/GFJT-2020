namespace QRST_DI_MS_Console.UserInterfaces
{
    partial class FrmDownLoad
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
            this.label1 = new System.Windows.Forms.Label();
            this.textBoxDataPath = new System.Windows.Forms.TextBox();
            this.radioGroup1 = new DevExpress.XtraEditors.RadioGroup();
            this.label2 = new System.Windows.Forms.Label();
            this.btnChooseBtn = new DevExpress.XtraEditors.SimpleButton();
            this.btnStartDownLoad = new DevExpress.XtraEditors.SimpleButton();
            this.btnCancle = new DevExpress.XtraEditors.SimpleButton();
            this.lblAvailableSpace = new DevExpress.XtraEditors.LabelControl();
            ((System.ComponentModel.ISupportInitialize)(this.radioGroup1.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 28);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(55, 14);
            this.label1.TabIndex = 0;
            this.label1.Text = "存放位置";
            // 
            // textBoxDataPath
            // 
            this.textBoxDataPath.Location = new System.Drawing.Point(73, 25);
            this.textBoxDataPath.Name = "textBoxDataPath";
            this.textBoxDataPath.Size = new System.Drawing.Size(366, 22);
            this.textBoxDataPath.TabIndex = 1;
            // 
            // radioGroup1
            // 
            this.radioGroup1.Location = new System.Drawing.Point(73, 79);
            this.radioGroup1.Name = "radioGroup1";
            this.radioGroup1.Properties.Items.AddRange(new DevExpress.XtraEditors.Controls.RadioGroupItem[] {
            new DevExpress.XtraEditors.Controls.RadioGroupItem(null, "原始数据"),
            new DevExpress.XtraEditors.Controls.RadioGroupItem(null, "校正数据")});
            this.radioGroup1.Size = new System.Drawing.Size(366, 46);
            this.radioGroup1.TabIndex = 3;
            this.radioGroup1.SelectedIndexChanged += new System.EventHandler(this.radioGroup1_SelectedIndexChanged_1);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 79);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(55, 14);
            this.label2.TabIndex = 4;
            this.label2.Text = "下载选项";
            // 
            // btnChooseBtn
            // 
            this.btnChooseBtn.Location = new System.Drawing.Point(445, 24);
            this.btnChooseBtn.Name = "btnChooseBtn";
            this.btnChooseBtn.Size = new System.Drawing.Size(70, 23);
            this.btnChooseBtn.TabIndex = 5;
            this.btnChooseBtn.Text = "...";
            this.btnChooseBtn.Click += new System.EventHandler(this.btnChooseBtn_Click);
            // 
            // btnStartDownLoad
            // 
            this.btnStartDownLoad.Location = new System.Drawing.Point(364, 157);
            this.btnStartDownLoad.Name = "btnStartDownLoad";
            this.btnStartDownLoad.Size = new System.Drawing.Size(75, 23);
            this.btnStartDownLoad.TabIndex = 6;
            this.btnStartDownLoad.Text = "开始下载";
            this.btnStartDownLoad.Click += new System.EventHandler(this.btnStartDownLoad_Click);
            // 
            // btnCancle
            // 
            this.btnCancle.Location = new System.Drawing.Point(445, 157);
            this.btnCancle.Name = "btnCancle";
            this.btnCancle.Size = new System.Drawing.Size(75, 23);
            this.btnCancle.TabIndex = 7;
            this.btnCancle.Text = "取消";
            this.btnCancle.Click += new System.EventHandler(this.btnCancle_Click);
            // 
            // lblAvailableSpace
            // 
            this.lblAvailableSpace.Location = new System.Drawing.Point(73, 54);
            this.lblAvailableSpace.Name = "lblAvailableSpace";
            this.lblAvailableSpace.Size = new System.Drawing.Size(0, 14);
            this.lblAvailableSpace.TabIndex = 8;
            // 
            // FrmDownLoad
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(545, 192);
            this.Controls.Add(this.lblAvailableSpace);
            this.Controls.Add(this.btnCancle);
            this.Controls.Add(this.btnStartDownLoad);
            this.Controls.Add(this.btnChooseBtn);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.radioGroup1);
            this.Controls.Add(this.textBoxDataPath);
            this.Controls.Add(this.label1);
            this.Name = "FrmDownLoad";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "下载选项";
            ((System.ComponentModel.ISupportInitialize)(this.radioGroup1.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBoxDataPath;
        private DevExpress.XtraEditors.RadioGroup radioGroup1;
        private System.Windows.Forms.Label label2;
        private DevExpress.XtraEditors.SimpleButton btnChooseBtn;
        private DevExpress.XtraEditors.SimpleButton btnStartDownLoad;
        private DevExpress.XtraEditors.SimpleButton btnCancle;
        private DevExpress.XtraEditors.LabelControl lblAvailableSpace;
    }
}