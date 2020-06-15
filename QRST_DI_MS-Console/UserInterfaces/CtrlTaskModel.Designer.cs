namespace QRST_DI_MS_Console.UserInterfaces
{
    partial class CtrlTaskModel
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
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl3 = new DevExpress.XtraEditors.LabelControl();
            this.labelControlStep = new DevExpress.XtraEditors.LabelControl();
            this.labelControlTaskDescription = new DevExpress.XtraEditors.LabelControl();
            this.labelControlPara = new DevExpress.XtraEditors.LabelControl();
            this.SuspendLayout();
            // 
            // labelControl1
            // 
            this.labelControl1.Appearance.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelControl1.Appearance.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(255)))));
            this.labelControl1.Enabled = false;
            this.labelControl1.Location = new System.Drawing.Point(16, 14);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(36, 21);
            this.labelControl1.TabIndex = 0;
            this.labelControl1.Text = "步骤:";
            // 
            // labelControl2
            // 
            this.labelControl2.Appearance.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.labelControl2.Appearance.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(225)))));
            this.labelControl2.Enabled = false;
            this.labelControl2.Location = new System.Drawing.Point(23, 41);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(59, 20);
            this.labelControl2.TabIndex = 1;
            this.labelControl2.Text = "任务名称:";
            // 
            // labelControl3
            // 
            this.labelControl3.Appearance.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.labelControl3.Appearance.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(225)))));
            this.labelControl3.Enabled = false;
            this.labelControl3.Location = new System.Drawing.Point(23, 67);
            this.labelControl3.Name = "labelControl3";
            this.labelControl3.Size = new System.Drawing.Size(59, 20);
            this.labelControl3.TabIndex = 2;
            this.labelControl3.Text = "参数说明:";
            // 
            // labelControlStep
            // 
            this.labelControlStep.Appearance.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.labelControlStep.Appearance.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(225)))));
            this.labelControlStep.Enabled = false;
            this.labelControlStep.Location = new System.Drawing.Point(68, 15);
            this.labelControlStep.Name = "labelControlStep";
            this.labelControlStep.Size = new System.Drawing.Size(14, 20);
            this.labelControlStep.TabIndex = 3;
            this.labelControlStep.Text = "-1";
            // 
            // labelControlTaskDescription
            // 
            this.labelControlTaskDescription.Appearance.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.labelControlTaskDescription.Appearance.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(225)))));
            this.labelControlTaskDescription.Enabled = false;
            this.labelControlTaskDescription.Location = new System.Drawing.Point(88, 41);
            this.labelControlTaskDescription.Name = "labelControlTaskDescription";
            this.labelControlTaskDescription.Size = new System.Drawing.Size(0, 20);
            this.labelControlTaskDescription.TabIndex = 4;
            // 
            // labelControlPara
            // 
            this.labelControlPara.Appearance.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.labelControlPara.Appearance.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(225)))));
            this.labelControlPara.Enabled = false;
            this.labelControlPara.Location = new System.Drawing.Point(88, 67);
            this.labelControlPara.Name = "labelControlPara";
            this.labelControlPara.Size = new System.Drawing.Size(0, 20);
            this.labelControlPara.TabIndex = 5;
            // 
            // CtrlTaskModel
            // 
            this.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.Appearance.Options.UseBackColor = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.Controls.Add(this.labelControlPara);
            this.Controls.Add(this.labelControlTaskDescription);
            this.Controls.Add(this.labelControlStep);
            this.Controls.Add(this.labelControl3);
            this.Controls.Add(this.labelControl2);
            this.Controls.Add(this.labelControl1);
            this.Name = "CtrlTaskModel";
            this.Size = new System.Drawing.Size(325, 98);
            this.Load += new System.EventHandler(this.CtrlTaskModel_Load);
            this.Click += new System.EventHandler(this.CtrlTaskModel_Click);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraEditors.LabelControl labelControl3;
        public DevExpress.XtraEditors.LabelControl labelControlStep;
        private DevExpress.XtraEditors.LabelControl labelControlTaskDescription;
        private DevExpress.XtraEditors.LabelControl labelControlPara;
    }
}
