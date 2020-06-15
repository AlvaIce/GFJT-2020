namespace QRST_DI_MS_Desktop.UserInterfaces.DisComputeUI
{
    partial class CtrlInputArg
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
            this.groupControl = new DevExpress.XtraEditors.GroupControl();
            this.textEdit = new DevExpress.XtraEditors.TextEdit();
            this.labelControl = new DevExpress.XtraEditors.LabelControl();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl)).BeginInit();
            this.groupControl.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.textEdit.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // groupControl
            // 
            this.groupControl.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(207)))), ((int)(((byte)(221)))), ((int)(((byte)(238)))));
            this.groupControl.Appearance.Options.UseBackColor = true;
            this.groupControl.CaptionLocation = DevExpress.Utils.Locations.Top;
            this.groupControl.Controls.Add(this.textEdit);
            this.groupControl.Controls.Add(this.labelControl);
            this.groupControl.Location = new System.Drawing.Point(3, 7);
            this.groupControl.LookAndFeel.Style = DevExpress.LookAndFeel.LookAndFeelStyle.UltraFlat;
            this.groupControl.LookAndFeel.UseDefaultLookAndFeel = false;
            this.groupControl.Name = "groupControl";
            this.groupControl.Size = new System.Drawing.Size(215, 100);
            this.groupControl.TabIndex = 0;
            this.groupControl.Text = "groupControl1";
            // 
            // textEdit
            // 
            this.textEdit.Location = new System.Drawing.Point(87, 45);
            this.textEdit.Name = "textEdit";
            this.textEdit.Size = new System.Drawing.Size(117, 21);
            this.textEdit.TabIndex = 1;
            // 
            // labelControl
            // 
            this.labelControl.Location = new System.Drawing.Point(11, 48);
            this.labelControl.Name = "labelControl";
            this.labelControl.Size = new System.Drawing.Size(70, 14);
            this.labelControl.TabIndex = 0;
            this.labelControl.Text = "labelControl1";
            // 
            // CtrlInputArg
            // 
            this.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(207)))), ((int)(((byte)(221)))), ((int)(((byte)(238)))));
            this.Appearance.Options.UseBackColor = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupControl);
            this.Name = "CtrlInputArg";
            this.Size = new System.Drawing.Size(220, 115);
            ((System.ComponentModel.ISupportInitialize)(this.groupControl)).EndInit();
            this.groupControl.ResumeLayout(false);
            this.groupControl.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.textEdit.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        public DevExpress.XtraEditors.TextEdit textEdit;
        public DevExpress.XtraEditors.GroupControl groupControl;
        public DevExpress.XtraEditors.LabelControl labelControl;
    }
}
