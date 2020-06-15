namespace QRST_DI_MS_Desktop.UserInterfaces
{
    partial class mucVirtualDirManager
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
            this.SuspendLayout();
            // 
            // virtualDirUC
            // 
            this.virtualDirUC.Dock = System.Windows.Forms.DockStyle.Fill;
            this.virtualDirUC.Location = new System.Drawing.Point(0, 0);
            this.virtualDirUC.Name = "virtualDirUC";
            this.virtualDirUC.Size = new System.Drawing.Size(931, 465);
            this.virtualDirUC.TabIndex = 0;
            // 
            // mucVirtualDirManager
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.virtualDirUC);
            this.Name = "mucVirtualDirManager";
            this.Size = new System.Drawing.Size(931, 465);
            this.ResumeLayout(false);

        }

        #endregion

        public QRST_DI_MS_Component.VirtualDirUI.VirtualDirUC virtualDirUC;

    }
}
