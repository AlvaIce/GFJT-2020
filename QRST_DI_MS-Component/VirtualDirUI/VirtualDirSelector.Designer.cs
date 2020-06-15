namespace QRST_DI_MS_Component.VirtualDirUI
{
    partial class VirtualDirSelector
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
            this.virtualDirUC1 = new QRST_DI_MS_Component.VirtualDirUI.VirtualDirUC();
            this.btn_ok = new DevComponents.DotNetBar.ButtonX();
            this.btn_cancel = new DevComponents.DotNetBar.ButtonX();
            this.SuspendLayout();
            // 
            // virtualDirUC1
            // 
            this.virtualDirUC1.Dock = System.Windows.Forms.DockStyle.Top;
            this.virtualDirUC1.Location = new System.Drawing.Point(0, 0);
            this.virtualDirUC1.Name = "virtualDirUC1";
            this.virtualDirUC1.Size = new System.Drawing.Size(568, 443);
            this.virtualDirUC1.TabIndex = 0;
            // 
            // btn_ok
            // 
            this.btn_ok.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btn_ok.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btn_ok.Location = new System.Drawing.Point(182, 449);
            this.btn_ok.Name = "btn_ok";
            this.btn_ok.Size = new System.Drawing.Size(75, 23);
            this.btn_ok.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.btn_ok.TabIndex = 1;
            this.btn_ok.Text = "确定";
            this.btn_ok.Click += new System.EventHandler(this.btn_ok_Click);
            // 
            // btn_cancel
            // 
            this.btn_cancel.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btn_cancel.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btn_cancel.Location = new System.Drawing.Point(330, 449);
            this.btn_cancel.Name = "btn_cancel";
            this.btn_cancel.Size = new System.Drawing.Size(75, 23);
            this.btn_cancel.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.btn_cancel.TabIndex = 1;
            this.btn_cancel.Text = "取消";
            this.btn_cancel.Click += new System.EventHandler(this.btn_cancel_Click);
            // 
            // VirtualDirSelector
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(568, 477);
            this.Controls.Add(this.btn_cancel);
            this.Controls.Add(this.btn_ok);
            this.Controls.Add(this.virtualDirUC1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "VirtualDirSelector";
            this.Text = "选择目标虚拟目录";
            this.ResumeLayout(false);

        }

        #endregion

        public VirtualDirUC virtualDirUC1;
        private DevComponents.DotNetBar.ButtonX btn_ok;
        private DevComponents.DotNetBar.ButtonX btn_cancel;

    }
}