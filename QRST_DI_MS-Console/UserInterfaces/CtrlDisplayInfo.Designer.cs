namespace QRST_DI_MS_Console.UserInterfaces
{
    partial class CtrlDisplayInfo
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.labelMsg = new DevExpress.XtraEditors.LabelControl();
            this.SuspendLayout();
            // 
            // labelMsg
            // 
            this.labelMsg.Appearance.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.labelMsg.Appearance.ForeColor = System.Drawing.Color.Black;
            this.labelMsg.Location = new System.Drawing.Point(228, 129);
            this.labelMsg.Name = "labelMsg";
            this.labelMsg.Size = new System.Drawing.Size(102, 21);
            this.labelMsg.TabIndex = 0;
            this.labelMsg.Text = "labelControl1";
            // 
            // CtrlDisplayInfo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.labelMsg);
            this.Name = "CtrlDisplayInfo";
            this.Size = new System.Drawing.Size(624, 312);
            this.Load += new System.EventHandler(this.CtrlDisplayInfo_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.LabelControl labelMsg;
    }
}
