namespace QRST_DI_MS_Console.UserInterfaces
{
    partial class FrmDisplayInfo
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
            this.components = new System.ComponentModel.Container();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl3 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl4 = new DevExpress.XtraEditors.LabelControl();
            this.btnFinish = new DevExpress.XtraEditors.SimpleButton();
            this.btnCancle = new DevExpress.XtraEditors.SimpleButton();
            this.memoBackUpMsg = new DevExpress.XtraEditors.MemoEdit();
            this.marqueeProgressBarControl1 = new DevExpress.XtraEditors.MarqueeProgressBarControl();
            this.labelBackUpPath = new DevExpress.XtraEditors.LabelControl();
            this.labelFinished = new DevExpress.XtraEditors.LabelControl();
            this.labelError = new DevExpress.XtraEditors.LabelControl();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.memoBackUpMsg.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.marqueeProgressBarControl1.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // labelControl2
            // 
            this.labelControl2.Location = new System.Drawing.Point(23, 27);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(48, 14);
            this.labelControl2.TabIndex = 1;
            this.labelControl2.Text = "备份到：";
            // 
            // labelControl3
            // 
            this.labelControl3.Location = new System.Drawing.Point(23, 47);
            this.labelControl3.Name = "labelControl3";
            this.labelControl3.Size = new System.Drawing.Size(48, 14);
            this.labelControl3.TabIndex = 2;
            this.labelControl3.Text = "已处理：";
            // 
            // labelControl4
            // 
            this.labelControl4.Location = new System.Drawing.Point(23, 67);
            this.labelControl4.Name = "labelControl4";
            this.labelControl4.Size = new System.Drawing.Size(36, 14);
            this.labelControl4.TabIndex = 3;
            this.labelControl4.Text = "错误：";
            // 
            // btnFinish
            // 
            this.btnFinish.Enabled = false;
            this.btnFinish.Location = new System.Drawing.Point(114, 355);
            this.btnFinish.Name = "btnFinish";
            this.btnFinish.Size = new System.Drawing.Size(75, 23);
            this.btnFinish.TabIndex = 5;
            this.btnFinish.Text = "完成";
            this.btnFinish.Click += new System.EventHandler(this.btnFinish_Click);
            // 
            // btnCancle
            // 
            this.btnCancle.Location = new System.Drawing.Point(195, 355);
            this.btnCancle.Name = "btnCancle";
            this.btnCancle.Size = new System.Drawing.Size(75, 23);
            this.btnCancle.TabIndex = 6;
            this.btnCancle.Text = "取消";
            this.btnCancle.Click += new System.EventHandler(this.btnCancle_Click);
            // 
            // memoBackUpMsg
            // 
            this.memoBackUpMsg.Location = new System.Drawing.Point(23, 100);
            this.memoBackUpMsg.Name = "memoBackUpMsg";
            this.memoBackUpMsg.Properties.Appearance.BackColor = System.Drawing.Color.Transparent;
            this.memoBackUpMsg.Properties.Appearance.Options.UseBackColor = true;
            this.memoBackUpMsg.Properties.ReadOnly = true;
            this.memoBackUpMsg.Size = new System.Drawing.Size(367, 215);
            this.memoBackUpMsg.TabIndex = 7;
            this.memoBackUpMsg.EditValueChanged += new System.EventHandler(this.memoBackUpMsg_EditValueChanged);
            // 
            // marqueeProgressBarControl1
            // 
            this.marqueeProgressBarControl1.EditValue = "正在进行数据备份,请稍后......";
            this.marqueeProgressBarControl1.Location = new System.Drawing.Point(23, 321);
            this.marqueeProgressBarControl1.Name = "marqueeProgressBarControl1";
            this.marqueeProgressBarControl1.Properties.MarqueeAnimationSpeed = 20;
            this.marqueeProgressBarControl1.Properties.ShowTitle = true;
            this.marqueeProgressBarControl1.Size = new System.Drawing.Size(367, 18);
            this.marqueeProgressBarControl1.TabIndex = 8;
            // 
            // labelBackUpPath
            // 
            this.labelBackUpPath.Location = new System.Drawing.Point(77, 27);
            this.labelBackUpPath.Name = "labelBackUpPath";
            this.labelBackUpPath.Size = new System.Drawing.Size(0, 14);
            this.labelBackUpPath.TabIndex = 9;
            // 
            // labelFinished
            // 
            this.labelFinished.Location = new System.Drawing.Point(77, 47);
            this.labelFinished.Name = "labelFinished";
            this.labelFinished.Size = new System.Drawing.Size(0, 14);
            this.labelFinished.TabIndex = 10;
            // 
            // labelError
            // 
            this.labelError.Location = new System.Drawing.Point(77, 67);
            this.labelError.Name = "labelError";
            this.labelError.Size = new System.Drawing.Size(0, 14);
            this.labelError.TabIndex = 11;
            // 
            // timer1
            // 
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // FrmDisplayInfo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(420, 390);
            this.Controls.Add(this.labelError);
            this.Controls.Add(this.labelFinished);
            this.Controls.Add(this.labelBackUpPath);
            this.Controls.Add(this.marqueeProgressBarControl1);
            this.Controls.Add(this.memoBackUpMsg);
            this.Controls.Add(this.btnCancle);
            this.Controls.Add(this.btnFinish);
            this.Controls.Add(this.labelControl4);
            this.Controls.Add(this.labelControl3);
            this.Controls.Add(this.labelControl2);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmDisplayInfo";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "数据备份";
            this.Load += new System.EventHandler(this.FrmDisplayInfo_Load);
            ((System.ComponentModel.ISupportInitialize)(this.memoBackUpMsg.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.marqueeProgressBarControl1.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraEditors.LabelControl labelControl3;
        private DevExpress.XtraEditors.LabelControl labelControl4;
        private DevExpress.XtraEditors.SimpleButton btnFinish;
        private DevExpress.XtraEditors.SimpleButton btnCancle;
        private DevExpress.XtraEditors.MemoEdit memoBackUpMsg;
        private DevExpress.XtraEditors.MarqueeProgressBarControl marqueeProgressBarControl1;
        private DevExpress.XtraEditors.LabelControl labelBackUpPath;
        private DevExpress.XtraEditors.LabelControl labelFinished;
        private DevExpress.XtraEditors.LabelControl labelError;
        private System.Windows.Forms.Timer timer1;
    }
}