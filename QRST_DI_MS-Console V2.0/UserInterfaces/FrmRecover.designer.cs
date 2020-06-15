namespace QRST_DI_MS_Desktop.UserInterfaces
{
    partial class FrmRecover
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
            this.marqueeProgressBarControl1 = new DevExpress.XtraEditors.MarqueeProgressBarControl();
            this.memoBackUpMsg = new DevExpress.XtraEditors.MemoEdit();
            this.btnCancle = new DevExpress.XtraEditors.SimpleButton();
            this.btnFinish = new DevExpress.XtraEditors.SimpleButton();
            this.labelControl4 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl3 = new DevExpress.XtraEditors.LabelControl();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.labelFinished = new DevExpress.XtraEditors.LabelControl();
            this.labelError = new DevExpress.XtraEditors.LabelControl();
            ((System.ComponentModel.ISupportInitialize)(this.marqueeProgressBarControl1.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.memoBackUpMsg.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // marqueeProgressBarControl1
            // 
            this.marqueeProgressBarControl1.EditValue = "正在进行数据库恢复,请稍后......";
            this.marqueeProgressBarControl1.Location = new System.Drawing.Point(37, 303);
            this.marqueeProgressBarControl1.Name = "marqueeProgressBarControl1";
            this.marqueeProgressBarControl1.Properties.MarqueeAnimationSpeed = 20;
            this.marqueeProgressBarControl1.Properties.ShowTitle = true;
            this.marqueeProgressBarControl1.Size = new System.Drawing.Size(367, 18);
            this.marqueeProgressBarControl1.TabIndex = 14;
            // 
            // memoBackUpMsg
            // 
            this.memoBackUpMsg.Location = new System.Drawing.Point(37, 69);
            this.memoBackUpMsg.Name = "memoBackUpMsg";
            this.memoBackUpMsg.Properties.Appearance.BackColor = System.Drawing.Color.Transparent;
            this.memoBackUpMsg.Properties.Appearance.Options.UseBackColor = true;
            this.memoBackUpMsg.Properties.ReadOnly = true;
            this.memoBackUpMsg.Size = new System.Drawing.Size(367, 215);
            this.memoBackUpMsg.TabIndex = 13;
            // 
            // btnCancle
            // 
            this.btnCancle.Location = new System.Drawing.Point(209, 337);
            this.btnCancle.Name = "btnCancle";
            this.btnCancle.Size = new System.Drawing.Size(75, 23);
            this.btnCancle.TabIndex = 12;
            this.btnCancle.Text = "取消";
            this.btnCancle.Click += new System.EventHandler(this.btnCancle_Click);
            // 
            // btnFinish
            // 
            this.btnFinish.Enabled = false;
            this.btnFinish.Location = new System.Drawing.Point(128, 337);
            this.btnFinish.Name = "btnFinish";
            this.btnFinish.Size = new System.Drawing.Size(75, 23);
            this.btnFinish.TabIndex = 11;
            this.btnFinish.Text = "完成";
            this.btnFinish.Click += new System.EventHandler(this.btnFinish_Click);
            // 
            // labelControl4
            // 
            this.labelControl4.Location = new System.Drawing.Point(37, 49);
            this.labelControl4.Name = "labelControl4";
            this.labelControl4.Size = new System.Drawing.Size(36, 14);
            this.labelControl4.TabIndex = 10;
            this.labelControl4.Text = "错误：";
            // 
            // labelControl3
            // 
            this.labelControl3.Location = new System.Drawing.Point(37, 29);
            this.labelControl3.Name = "labelControl3";
            this.labelControl3.Size = new System.Drawing.Size(48, 14);
            this.labelControl3.TabIndex = 9;
            this.labelControl3.Text = "已处理：";
            // 
            // timer1
            // 
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // labelFinished
            // 
            this.labelFinished.Location = new System.Drawing.Point(91, 29);
            this.labelFinished.Name = "labelFinished";
            this.labelFinished.Size = new System.Drawing.Size(0, 14);
            this.labelFinished.TabIndex = 15;
            // 
            // labelError
            // 
            this.labelError.Location = new System.Drawing.Point(85, 49);
            this.labelError.Name = "labelError";
            this.labelError.Size = new System.Drawing.Size(0, 14);
            this.labelError.TabIndex = 16;
            // 
            // FrmRecover
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(437, 401);
            this.Controls.Add(this.labelError);
            this.Controls.Add(this.labelFinished);
            this.Controls.Add(this.marqueeProgressBarControl1);
            this.Controls.Add(this.memoBackUpMsg);
            this.Controls.Add(this.btnCancle);
            this.Controls.Add(this.btnFinish);
            this.Controls.Add(this.labelControl4);
            this.Controls.Add(this.labelControl3);
            this.Name = "FrmRecover";
            this.Text = "数据库恢复";
            this.Load += new System.EventHandler(this.FrmRecover_Load);
            ((System.ComponentModel.ISupportInitialize)(this.marqueeProgressBarControl1.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.memoBackUpMsg.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.MarqueeProgressBarControl marqueeProgressBarControl1;
        private DevExpress.XtraEditors.MemoEdit memoBackUpMsg;
        private DevExpress.XtraEditors.SimpleButton btnCancle;
        private DevExpress.XtraEditors.SimpleButton btnFinish;
        private DevExpress.XtraEditors.LabelControl labelControl4;
        private DevExpress.XtraEditors.LabelControl labelControl3;
        private System.Windows.Forms.Timer timer1;
        private DevExpress.XtraEditors.LabelControl labelFinished;
        private DevExpress.XtraEditors.LabelControl labelError;
    }
}