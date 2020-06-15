namespace QRST_DI_MS_Desktop.UserInterfaces.DisComputeUI
{
    partial class CtrlSelectArg
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
            this.memoEdit = new DevExpress.XtraEditors.MemoEdit();
            this.simpleButton1 = new DevExpress.XtraEditors.SimpleButton();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl)).BeginInit();
            this.groupControl.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.memoEdit.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // groupControl
            // 
            this.groupControl.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(207)))), ((int)(((byte)(221)))), ((int)(((byte)(238)))));
            this.groupControl.Appearance.Options.UseBackColor = true;
            this.groupControl.Controls.Add(this.memoEdit);
            this.groupControl.Controls.Add(this.simpleButton1);
            this.groupControl.Location = new System.Drawing.Point(3, 7);
            this.groupControl.LookAndFeel.Style = DevExpress.LookAndFeel.LookAndFeelStyle.UltraFlat;
            this.groupControl.LookAndFeel.UseDefaultLookAndFeel = false;
            this.groupControl.Name = "groupControl";
            this.groupControl.Size = new System.Drawing.Size(215, 100);
            this.groupControl.TabIndex = 0;
            this.groupControl.Text = "groupControl1";
            // 
            // memoEdit
            // 
            this.memoEdit.Enabled = false;
            this.memoEdit.Location = new System.Drawing.Point(11, 41);
            this.memoEdit.Name = "memoEdit";
            this.memoEdit.Properties.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.memoEdit.Size = new System.Drawing.Size(192, 51);
            this.memoEdit.TabIndex = 3;
            // 
            // simpleButton1
            // 
            this.simpleButton1.Location = new System.Drawing.Point(144, 17);
            this.simpleButton1.Name = "simpleButton1";
            this.simpleButton1.Size = new System.Drawing.Size(59, 20);
            this.simpleButton1.TabIndex = 2;
            this.simpleButton1.Text = "浏览";
            this.simpleButton1.Click += new System.EventHandler(this.simpleButton1_Click);
            // 
            // CtrlSelectArg
            // 
            this.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(207)))), ((int)(((byte)(221)))), ((int)(((byte)(238)))));
            this.Appearance.Options.UseBackColor = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupControl);
            this.Name = "CtrlSelectArg";
            this.Size = new System.Drawing.Size(220, 115);
            ((System.ComponentModel.ISupportInitialize)(this.groupControl)).EndInit();
            this.groupControl.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.memoEdit.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.SimpleButton simpleButton1;
        public DevExpress.XtraEditors.MemoEdit memoEdit;
        public DevExpress.XtraEditors.GroupControl groupControl;

    }
}
