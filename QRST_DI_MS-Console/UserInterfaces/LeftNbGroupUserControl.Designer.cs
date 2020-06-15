namespace QRST_DI_MS_Console.UserInterfaces
{
	partial class LeftNbGroupUserControl
	{
		/// <summary> 
		/// 必需的设计器变量。
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary> 
		/// 清理所有正在使用的资源。
		/// </summary>
		/// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region 组件设计器生成的代码

		/// <summary> 
		/// 设计器支持所需的方法 - 不要
		/// 使用代码编辑器修改此方法的内容。
		/// </summary>
		private void InitializeComponent()
		{
			this.navBarControl1 = new DevExpress.XtraNavBar.NavBarControl();
			this.navBarGroup1 = new DevExpress.XtraNavBar.NavBarGroup();
			((System.ComponentModel.ISupportInitialize)(this.navBarControl1)).BeginInit();
			this.SuspendLayout();
			// 
			// navBarControl1
			// 
			this.navBarControl1.ActiveGroup = this.navBarGroup1;
			this.navBarControl1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.navBarControl1.Groups.AddRange(new DevExpress.XtraNavBar.NavBarGroup[] {
            this.navBarGroup1});
			this.navBarControl1.Location = new System.Drawing.Point(0, 0);
			this.navBarControl1.Name = "navBarControl1";
			this.navBarControl1.Size = new System.Drawing.Size(150, 150);
			this.navBarControl1.TabIndex = 0;
			this.navBarControl1.Text = "navBarControl1";
			this.navBarControl1.Click += new System.EventHandler(this.navBarControl1_Click);
			// 
			// navBarGroup1
			// 
			this.navBarGroup1.Caption = "navBarGroup1";
			this.navBarGroup1.Expanded = true;
			this.navBarGroup1.Name = "navBarGroup1";
			// 
			// LeftNbGroupUserControl
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.navBarControl1);
			this.Name = "LeftNbGroupUserControl";
			((System.ComponentModel.ISupportInitialize)(this.navBarControl1)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private DevExpress.XtraNavBar.NavBarControl navBarControl1;
		private DevExpress.XtraNavBar.NavBarGroup navBarGroup1;
	}
}
