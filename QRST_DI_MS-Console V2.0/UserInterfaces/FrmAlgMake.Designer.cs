namespace QRST_DI_MS_Desktop.UserInterfaces
{
    partial class FrmAlgMake
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
            this.txt_jarPath = new DevExpress.XtraEditors.TextEdit();
            this.simBtn_Browse = new DevExpress.XtraEditors.SimpleButton();
            this.groupControl1 = new DevExpress.XtraEditors.GroupControl();
            this.groupControl2 = new DevExpress.XtraEditors.GroupControl();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.memo_args = new DevExpress.XtraEditors.MemoEdit();
            this.simBtn_Run = new DevExpress.XtraEditors.SimpleButton();
            this.simBtn_Cancel = new DevExpress.XtraEditors.SimpleButton();
            ((System.ComponentModel.ISupportInitialize)(this.txt_jarPath.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).BeginInit();
            this.groupControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl2)).BeginInit();
            this.groupControl2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.memo_args.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // txt_jarPath
            // 
            this.txt_jarPath.Location = new System.Drawing.Point(5, 30);
            this.txt_jarPath.Name = "txt_jarPath";
            this.txt_jarPath.Size = new System.Drawing.Size(272, 21);
            this.txt_jarPath.TabIndex = 1;
            // 
            // simBtn_Browse
            // 
            this.simBtn_Browse.Location = new System.Drawing.Point(282, 28);
            this.simBtn_Browse.Name = "simBtn_Browse";
            this.simBtn_Browse.Size = new System.Drawing.Size(75, 23);
            this.simBtn_Browse.TabIndex = 2;
            this.simBtn_Browse.Text = "浏览";
            this.simBtn_Browse.Click += new System.EventHandler(this.simBtn_Browse_Click);
            // 
            // groupControl1
            // 
            this.groupControl1.Controls.Add(this.txt_jarPath);
            this.groupControl1.Controls.Add(this.simBtn_Browse);
            this.groupControl1.Location = new System.Drawing.Point(9, 0);
            this.groupControl1.LookAndFeel.Style = DevExpress.LookAndFeel.LookAndFeelStyle.UltraFlat;
            this.groupControl1.LookAndFeel.UseDefaultLookAndFeel = false;
            this.groupControl1.Name = "groupControl1";
            this.groupControl1.Size = new System.Drawing.Size(364, 66);
            this.groupControl1.TabIndex = 3;
            this.groupControl1.Text = "Jar包路径";
            // 
            // groupControl2
            // 
            this.groupControl2.Controls.Add(this.labelControl1);
            this.groupControl2.Controls.Add(this.memo_args);
            this.groupControl2.Location = new System.Drawing.Point(9, 72);
            this.groupControl2.LookAndFeel.Style = DevExpress.LookAndFeel.LookAndFeelStyle.UltraFlat;
            this.groupControl2.LookAndFeel.UseDefaultLookAndFeel = false;
            this.groupControl2.Name = "groupControl2";
            this.groupControl2.Size = new System.Drawing.Size(364, 269);
            this.groupControl2.TabIndex = 4;
            this.groupControl2.Text = "运行参数";
            // 
            // labelControl1
            // 
            this.labelControl1.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.Vertical;
            this.labelControl1.Location = new System.Drawing.Point(281, 35);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(77, 70);
            this.labelControl1.TabIndex = 6;
            this.labelControl1.Text = "*说明：运行参数每行输入一个，每个参数中不要带有空格";
            // 
            // memo_args
            // 
            this.memo_args.Location = new System.Drawing.Point(11, 33);
            this.memo_args.Name = "memo_args";
            this.memo_args.Size = new System.Drawing.Size(264, 219);
            this.memo_args.TabIndex = 0;
            // 
            // simBtn_Run
            // 
            this.simBtn_Run.Location = new System.Drawing.Point(208, 347);
            this.simBtn_Run.Name = "simBtn_Run";
            this.simBtn_Run.Size = new System.Drawing.Size(75, 23);
            this.simBtn_Run.TabIndex = 3;
            this.simBtn_Run.Text = "执行";
            this.simBtn_Run.Click += new System.EventHandler(this.simBtn_Run_Click);
            // 
            // simBtn_Cancel
            // 
            this.simBtn_Cancel.Location = new System.Drawing.Point(291, 347);
            this.simBtn_Cancel.Name = "simBtn_Cancel";
            this.simBtn_Cancel.Size = new System.Drawing.Size(75, 23);
            this.simBtn_Cancel.TabIndex = 5;
            this.simBtn_Cancel.Text = "取消";
            this.simBtn_Cancel.Click += new System.EventHandler(this.simBtn_Cancel_Click);
            // 
            // FrmAlgMake
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(380, 378);
            this.Controls.Add(this.simBtn_Cancel);
            this.Controls.Add(this.simBtn_Run);
            this.Controls.Add(this.groupControl2);
            this.Controls.Add(this.groupControl1);
            this.Name = "FrmAlgMake";
            this.Text = "算法自定义";
            ((System.ComponentModel.ISupportInitialize)(this.txt_jarPath.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).EndInit();
            this.groupControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.groupControl2)).EndInit();
            this.groupControl2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.memo_args.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.TextEdit txt_jarPath;
        private DevExpress.XtraEditors.SimpleButton simBtn_Browse;
        private DevExpress.XtraEditors.GroupControl groupControl1;
        private DevExpress.XtraEditors.GroupControl groupControl2;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.MemoEdit memo_args;
        private DevExpress.XtraEditors.SimpleButton simBtn_Run;
        private DevExpress.XtraEditors.SimpleButton simBtn_Cancel;
    }
}