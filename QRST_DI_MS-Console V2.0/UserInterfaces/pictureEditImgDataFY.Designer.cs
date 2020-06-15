using System.Drawing;
namespace QRST_DI_MS_Desktop.UserInterfaces
{
    partial class pictureEditImgDataFY
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
            this.pictureEditFY = new DevExpress.XtraEditors.PictureEdit();
            this.PageCounter = new DevExpress.XtraEditors.LabelControl();
            this.NextPage = new System.Windows.Forms.Label();
            this.PrePage = new System.Windows.Forms.Label();
            this.midline = new System.Windows.Forms.Label();
            this.comboBoxEdit1 = new DevExpress.XtraEditors.ComboBoxEdit();
            this.buttonX1 = new DevComponents.DotNetBar.ButtonX();
            this.simpleButton2 = new DevExpress.XtraEditors.SimpleButton();
            this.simpleButton1 = new DevExpress.XtraEditors.SimpleButton();
            this.simpleButtonZoomOut = new DevExpress.XtraEditors.SimpleButton();
            this.simpleButtonZoomIn = new DevExpress.XtraEditors.SimpleButton();
            ((System.ComponentModel.ISupportInitialize)(this.pictureEditFY.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.comboBoxEdit1.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureEditFY
            // 
            this.pictureEditFY.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureEditFY.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.pictureEditFY.Location = new System.Drawing.Point(0, 0);
            this.pictureEditFY.Name = "pictureEditFY";
            this.pictureEditFY.Size = new System.Drawing.Size(343, 354);
            this.pictureEditFY.TabIndex = 0;
            this.pictureEditFY.Click += new System.EventHandler(this.pictureEditFY_Click);
            this.pictureEditFY.Paint += new System.Windows.Forms.PaintEventHandler(this.pictureEditFY_Paint);
            this.pictureEditFY.MouseClick += new System.Windows.Forms.MouseEventHandler(this.pictureEditFY_MouseClick);
            this.pictureEditFY.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.pictureEditFY_MouseDoubleClick);
            this.pictureEditFY.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pictureEditFY_MouseDown);
            this.pictureEditFY.MouseEnter += new System.EventHandler(this.pictureEditFY_MouseEnter);
            this.pictureEditFY.MouseLeave += new System.EventHandler(this.pictureEditFY_MouseLeave);
            this.pictureEditFY.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pictureEditFY_MouseMove);
            // 
            // PageCounter
            // 
            this.PageCounter.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.PageCounter.Appearance.BackColor = System.Drawing.SystemColors.Control;
            this.PageCounter.Appearance.Font = new System.Drawing.Font("Verdana", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.PageCounter.Appearance.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.PageCounter.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.PageCounter.CausesValidation = false;
            this.PageCounter.Location = new System.Drawing.Point(146, 189);
            this.PageCounter.Name = "PageCounter";
            this.PageCounter.Size = new System.Drawing.Size(40, 18);
            this.PageCounter.TabIndex = 3;
            this.PageCounter.Text = "(1/x)";
            this.PageCounter.Visible = false;
            // 
            // NextPage
            // 
            this.NextPage.Dock = System.Windows.Forms.DockStyle.Right;
            this.NextPage.Font = new System.Drawing.Font("黑体", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.NextPage.ForeColor = System.Drawing.Color.DimGray;
            this.NextPage.Location = new System.Drawing.Point(316, 0);
            this.NextPage.Name = "NextPage";
            this.NextPage.Size = new System.Drawing.Size(27, 354);
            this.NextPage.TabIndex = 4;
            this.NextPage.Text = "下一张";
            this.NextPage.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.NextPage.Visible = false;
            // 
            // PrePage
            // 
            this.PrePage.Dock = System.Windows.Forms.DockStyle.Left;
            this.PrePage.Font = new System.Drawing.Font("黑体", 14.25F);
            this.PrePage.ForeColor = System.Drawing.Color.DimGray;
            this.PrePage.Location = new System.Drawing.Point(0, 0);
            this.PrePage.Name = "PrePage";
            this.PrePage.Size = new System.Drawing.Size(29, 354);
            this.PrePage.TabIndex = 5;
            this.PrePage.Text = "上一张";
            this.PrePage.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.PrePage.Visible = false;
            // 
            // midline
            // 
            this.midline.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this.midline.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.midline.ForeColor = System.Drawing.SystemColors.AppWorkspace;
            this.midline.Location = new System.Drawing.Point(166, 0);
            this.midline.Name = "midline";
            this.midline.Size = new System.Drawing.Size(1, 355);
            this.midline.TabIndex = 6;
            this.midline.Visible = false;
            // 
            // comboBoxEdit1
            // 
            this.comboBoxEdit1.Location = new System.Drawing.Point(95, 3);
            this.comboBoxEdit1.Name = "comboBoxEdit1";
            this.comboBoxEdit1.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.comboBoxEdit1.Size = new System.Drawing.Size(136, 21);
            this.comboBoxEdit1.TabIndex = 10;
            this.comboBoxEdit1.Visible = false;
            this.comboBoxEdit1.SelectedIndexChanged += new System.EventHandler(this.comboBoxEdit1_SelectedIndexChanged);
            // 
            // buttonX1
            // 
            this.buttonX1.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.buttonX1.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.buttonX1.Location = new System.Drawing.Point(208, 38);
            this.buttonX1.Name = "buttonX1";
            this.buttonX1.Size = new System.Drawing.Size(47, 23);
            this.buttonX1.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.buttonX1.TabIndex = 11;
            this.buttonX1.Text = "下一张";
            this.buttonX1.Visible = false;
            this.buttonX1.Click += new System.EventHandler(this.buttonX1_Click);
            // 
            // simpleButton2
            // 
            this.simpleButton2.Image = global::QRST_DI_MS_Desktop.Properties.Resources.fuyuan;
            this.simpleButton2.ImageLocation = DevExpress.XtraEditors.ImageLocation.TopCenter;
            this.simpleButton2.Location = new System.Drawing.Point(0, 0);
            this.simpleButton2.Name = "simpleButton2";
            this.simpleButton2.Size = new System.Drawing.Size(29, 24);
            this.simpleButton2.TabIndex = 13;
            this.simpleButton2.ToolTip = "复原";
            this.simpleButton2.Click += new System.EventHandler(this.simpleButton2_Click);
            // 
            // simpleButton1
            // 
            this.simpleButton1.Image = global::QRST_DI_MS_Desktop.Properties.Resources.lrgenghuan;
            this.simpleButton1.ImageLocation = DevExpress.XtraEditors.ImageLocation.TopCenter;
            this.simpleButton1.Location = new System.Drawing.Point(26, 0);
            this.simpleButton1.Name = "simpleButton1";
            this.simpleButton1.Size = new System.Drawing.Size(29, 24);
            this.simpleButton1.TabIndex = 12;
            this.simpleButton1.ToolTip = "更换传感器";
            this.simpleButton1.Click += new System.EventHandler(this.simpleButton1_Click);
            // 
            // simpleButtonZoomOut
            // 
            this.simpleButtonZoomOut.ButtonStyle = DevExpress.XtraEditors.Controls.BorderStyles.Flat;
            this.simpleButtonZoomOut.Image = global::QRST_DI_MS_Desktop.Properties.Resources.缩小;
            this.simpleButtonZoomOut.Location = new System.Drawing.Point(49, 3);
            this.simpleButtonZoomOut.Name = "simpleButtonZoomOut";
            this.simpleButtonZoomOut.Size = new System.Drawing.Size(40, 36);
            this.simpleButtonZoomOut.TabIndex = 8;
            this.simpleButtonZoomOut.Visible = false;
            this.simpleButtonZoomOut.Click += new System.EventHandler(this.simpleButtonZoomOut_Click);
            // 
            // simpleButtonZoomIn
            // 
            this.simpleButtonZoomIn.ButtonStyle = DevExpress.XtraEditors.Controls.BorderStyles.Flat;
            this.simpleButtonZoomIn.Image = global::QRST_DI_MS_Desktop.Properties.Resources.放大;
            this.simpleButtonZoomIn.Location = new System.Drawing.Point(3, 3);
            this.simpleButtonZoomIn.Name = "simpleButtonZoomIn";
            this.simpleButtonZoomIn.Size = new System.Drawing.Size(40, 36);
            this.simpleButtonZoomIn.TabIndex = 7;
            this.simpleButtonZoomIn.Visible = false;
            this.simpleButtonZoomIn.Click += new System.EventHandler(this.simpleButtonZoomIn_Click);
            // 
            // pictureEditImgDataFY
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.Controls.Add(this.simpleButton2);
            this.Controls.Add(this.simpleButton1);
            this.Controls.Add(this.buttonX1);
            this.Controls.Add(this.comboBoxEdit1);
            this.Controls.Add(this.simpleButtonZoomOut);
            this.Controls.Add(this.simpleButtonZoomIn);
            this.Controls.Add(this.midline);
            this.Controls.Add(this.PrePage);
            this.Controls.Add(this.NextPage);
            this.Controls.Add(this.PageCounter);
            this.Controls.Add(this.pictureEditFY);
            this.Name = "pictureEditImgDataFY";
            this.Size = new System.Drawing.Size(343, 354);
            this.SizeChanged += new System.EventHandler(this.pictureEditImgDataFY_SizeChanged);
            ((System.ComponentModel.ISupportInitialize)(this.pictureEditFY.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.comboBoxEdit1.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.LabelControl PageCounter;
        public DevExpress.XtraEditors.PictureEdit pictureEditFY;
        private System.Windows.Forms.Label NextPage;
        private System.Windows.Forms.Label PrePage;
        private System.Windows.Forms.Label midline;
        public DevExpress.XtraEditors.ComboBoxEdit comboBoxEdit1;
        private DevExpress.XtraEditors.SimpleButton simpleButtonZoomIn;
        private DevExpress.XtraEditors.SimpleButton simpleButtonZoomOut;
        private DevComponents.DotNetBar.ButtonX buttonX1;
        private DevExpress.XtraEditors.SimpleButton simpleButton1;
        private DevExpress.XtraEditors.SimpleButton simpleButton2;
    }
}
