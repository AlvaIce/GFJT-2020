namespace QRST.WorldGlobeTool
{
    partial class QRSTWorldToolbox
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(QRSTWorldToolbox));
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripButtonReset = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonLayerManager = new System.Windows.Forms.ToolStripButton();
            this.toolStripSplitButtonShowOrNot = new System.Windows.Forms.ToolStripSplitButton();
            this.ToolStripMenuItemGridLine = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItemSun = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItemPosition = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItemCopyright = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripDropDownButtonAddLayer = new System.Windows.Forms.ToolStripDropDownButton();
            this.ToolStripMenuItemAddImageLayer = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItemAddShapeLayer = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripDropDownButtonAddGCP = new System.Windows.Forms.ToolStripDropDownButton();
            this.ToolStripMenuItemAddGeoGCP = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItemAddATMGCP = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripDropDownButtonDraw = new System.Windows.Forms.ToolStripDropDownButton();
            this.ToolStripMenuItemDrawPolyLine = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItemDrawRectangle = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItemDrawPolygon = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripButtonSaveScreen = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonGoto = new System.Windows.Forms.ToolStripButton();
            this.toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButtonReset,
            this.toolStripButtonLayerManager,
            this.toolStripSplitButtonShowOrNot,
            this.toolStripDropDownButtonAddLayer,
            this.toolStripDropDownButtonAddGCP,
            this.toolStripDropDownButtonDraw,
            this.toolStripButtonSaveScreen,
            this.toolStripButtonGoto,
            this.toolStripLabel1});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(639, 25);
            this.toolStrip1.TabIndex = 1;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // toolStripButtonReset
            // 
            this.toolStripButtonReset.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonReset.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonReset.Image")));
            this.toolStripButtonReset.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonReset.Name = "toolStripButtonReset";
            this.toolStripButtonReset.Size = new System.Drawing.Size(23, 22);
            this.toolStripButtonReset.Text = "恢复地球到初始状态";
            this.toolStripButtonReset.Click += new System.EventHandler(this.toolStripButtonReset_Click);
            // 
            // toolStripButtonLayerManager
            // 
            this.toolStripButtonLayerManager.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonLayerManager.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonLayerManager.Image")));
            this.toolStripButtonLayerManager.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonLayerManager.Name = "toolStripButtonLayerManager";
            this.toolStripButtonLayerManager.Size = new System.Drawing.Size(23, 22);
            this.toolStripButtonLayerManager.Text = "显示/隐藏图层管理菜单";
            this.toolStripButtonLayerManager.Click += new System.EventHandler(this.toolStripButtonLayerManager_Click);
            // 
            // toolStripSplitButtonShowOrNot
            // 
            this.toolStripSplitButtonShowOrNot.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripSplitButtonShowOrNot.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ToolStripMenuItemGridLine,
            this.ToolStripMenuItemSun,
            this.ToolStripMenuItemPosition,
            this.ToolStripMenuItemCopyright});
            this.toolStripSplitButtonShowOrNot.Image = ((System.Drawing.Image)(resources.GetObject("toolStripSplitButtonShowOrNot.Image")));
            this.toolStripSplitButtonShowOrNot.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripSplitButtonShowOrNot.Name = "toolStripSplitButtonShowOrNot";
            this.toolStripSplitButtonShowOrNot.Size = new System.Drawing.Size(32, 22);
            this.toolStripSplitButtonShowOrNot.Text = "显示/隐藏内容";
            // 
            // ToolStripMenuItemGridLine
            // 
            this.ToolStripMenuItemGridLine.CheckOnClick = true;
            this.ToolStripMenuItemGridLine.Name = "ToolStripMenuItemGridLine";
            this.ToolStripMenuItemGridLine.Size = new System.Drawing.Size(124, 22);
            this.ToolStripMenuItemGridLine.Text = "经纬网格";
            this.ToolStripMenuItemGridLine.Click += new System.EventHandler(this.ToolStripMenuItemGridLine_Click);
            // 
            // ToolStripMenuItemSun
            // 
            this.ToolStripMenuItemSun.CheckOnClick = true;
            this.ToolStripMenuItemSun.Name = "ToolStripMenuItemSun";
            this.ToolStripMenuItemSun.Size = new System.Drawing.Size(124, 22);
            this.ToolStripMenuItemSun.Text = "太阳效果";
            this.ToolStripMenuItemSun.Click += new System.EventHandler(this.ToolStripMenuItemSun_Click);
            // 
            // ToolStripMenuItemPosition
            // 
            this.ToolStripMenuItemPosition.Checked = true;
            this.ToolStripMenuItemPosition.CheckOnClick = true;
            this.ToolStripMenuItemPosition.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ToolStripMenuItemPosition.Name = "ToolStripMenuItemPosition";
            this.ToolStripMenuItemPosition.Size = new System.Drawing.Size(124, 22);
            this.ToolStripMenuItemPosition.Text = "位置信息";
            this.ToolStripMenuItemPosition.Click += new System.EventHandler(this.ToolStripMenuItemPosition_Click);
            // 
            // ToolStripMenuItemCopyright
            // 
            this.ToolStripMenuItemCopyright.Checked = true;
            this.ToolStripMenuItemCopyright.CheckOnClick = true;
            this.ToolStripMenuItemCopyright.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ToolStripMenuItemCopyright.Name = "ToolStripMenuItemCopyright";
            this.ToolStripMenuItemCopyright.Size = new System.Drawing.Size(124, 22);
            this.ToolStripMenuItemCopyright.Text = "版权信息";
            this.ToolStripMenuItemCopyright.Click += new System.EventHandler(this.ToolStripMenuItemCopyright_Click);
            // 
            // toolStripDropDownButtonAddLayer
            // 
            this.toolStripDropDownButtonAddLayer.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripDropDownButtonAddLayer.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ToolStripMenuItemAddImageLayer,
            this.ToolStripMenuItemAddShapeLayer});
            this.toolStripDropDownButtonAddLayer.Image = ((System.Drawing.Image)(resources.GetObject("toolStripDropDownButtonAddLayer.Image")));
            this.toolStripDropDownButtonAddLayer.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripDropDownButtonAddLayer.Name = "toolStripDropDownButtonAddLayer";
            this.toolStripDropDownButtonAddLayer.Size = new System.Drawing.Size(29, 22);
            this.toolStripDropDownButtonAddLayer.Text = "添加图层";
            // 
            // ToolStripMenuItemAddImageLayer
            // 
            this.ToolStripMenuItemAddImageLayer.Name = "ToolStripMenuItemAddImageLayer";
            this.ToolStripMenuItemAddImageLayer.Size = new System.Drawing.Size(152, 22);
            this.ToolStripMenuItemAddImageLayer.Text = "添加栅格图层";
            this.ToolStripMenuItemAddImageLayer.Click += new System.EventHandler(this.ToolStripMenuItemAddImageLayer_Click);
            // 
            // ToolStripMenuItemAddShapeLayer
            // 
            this.ToolStripMenuItemAddShapeLayer.Name = "ToolStripMenuItemAddShapeLayer";
            this.ToolStripMenuItemAddShapeLayer.Size = new System.Drawing.Size(152, 22);
            this.ToolStripMenuItemAddShapeLayer.Text = "添加矢量图层";
            this.ToolStripMenuItemAddShapeLayer.Click += new System.EventHandler(this.ToolStripMenuItemAddShapeLayer_Click);
            // 
            // toolStripDropDownButtonAddGCP
            // 
            this.toolStripDropDownButtonAddGCP.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripDropDownButtonAddGCP.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ToolStripMenuItemAddGeoGCP,
            this.ToolStripMenuItemAddATMGCP});
            this.toolStripDropDownButtonAddGCP.Image = ((System.Drawing.Image)(resources.GetObject("toolStripDropDownButtonAddGCP.Image")));
            this.toolStripDropDownButtonAddGCP.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripDropDownButtonAddGCP.Name = "toolStripDropDownButtonAddGCP";
            this.toolStripDropDownButtonAddGCP.Size = new System.Drawing.Size(29, 22);
            this.toolStripDropDownButtonAddGCP.Text = "添加控制点";
            // 
            // ToolStripMenuItemAddGeoGCP
            // 
            this.ToolStripMenuItemAddGeoGCP.Image = ((System.Drawing.Image)(resources.GetObject("ToolStripMenuItemAddGeoGCP.Image")));
            this.ToolStripMenuItemAddGeoGCP.Name = "ToolStripMenuItemAddGeoGCP";
            this.ToolStripMenuItemAddGeoGCP.Size = new System.Drawing.Size(160, 22);
            this.ToolStripMenuItemAddGeoGCP.Text = "添加几何控制点";
            this.ToolStripMenuItemAddGeoGCP.Click += new System.EventHandler(this.ToolStripMenuItemAddGeoGCP_Click);
            // 
            // ToolStripMenuItemAddATMGCP
            // 
            this.ToolStripMenuItemAddATMGCP.Image = ((System.Drawing.Image)(resources.GetObject("ToolStripMenuItemAddATMGCP.Image")));
            this.ToolStripMenuItemAddATMGCP.Name = "ToolStripMenuItemAddATMGCP";
            this.ToolStripMenuItemAddATMGCP.Size = new System.Drawing.Size(160, 22);
            this.ToolStripMenuItemAddATMGCP.Text = "添加辐射控制点";
            this.ToolStripMenuItemAddATMGCP.Click += new System.EventHandler(this.ToolStripMenuItemAddATMGCP_Click);
            // 
            // toolStripDropDownButtonDraw
            // 
            this.toolStripDropDownButtonDraw.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripDropDownButtonDraw.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ToolStripMenuItemDrawPolyLine,
            this.ToolStripMenuItemDrawRectangle,
            this.ToolStripMenuItemDrawPolygon});
            this.toolStripDropDownButtonDraw.Image = ((System.Drawing.Image)(resources.GetObject("toolStripDropDownButtonDraw.Image")));
            this.toolStripDropDownButtonDraw.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripDropDownButtonDraw.Name = "toolStripDropDownButtonDraw";
            this.toolStripDropDownButtonDraw.Size = new System.Drawing.Size(29, 22);
            this.toolStripDropDownButtonDraw.Text = "绘制线、多边形";
            // 
            // ToolStripMenuItemDrawPolyLine
            // 
            this.ToolStripMenuItemDrawPolyLine.Image = ((System.Drawing.Image)(resources.GetObject("ToolStripMenuItemDrawPolyLine.Image")));
            this.ToolStripMenuItemDrawPolyLine.Name = "ToolStripMenuItemDrawPolyLine";
            this.ToolStripMenuItemDrawPolyLine.Size = new System.Drawing.Size(136, 22);
            this.ToolStripMenuItemDrawPolyLine.Text = "绘制线段";
            this.ToolStripMenuItemDrawPolyLine.Click += new System.EventHandler(this.ToolStripMenuItemDrawPolyLine_Click);
            // 
            // ToolStripMenuItemDrawRectangle
            // 
            this.ToolStripMenuItemDrawRectangle.Image = ((System.Drawing.Image)(resources.GetObject("ToolStripMenuItemDrawRectangle.Image")));
            this.ToolStripMenuItemDrawRectangle.Name = "ToolStripMenuItemDrawRectangle";
            this.ToolStripMenuItemDrawRectangle.Size = new System.Drawing.Size(136, 22);
            this.ToolStripMenuItemDrawRectangle.Text = "绘制矩形";
            this.ToolStripMenuItemDrawRectangle.Click += new System.EventHandler(this.ToolStripMenuItemDrawRectangle_Click);
            // 
            // ToolStripMenuItemDrawPolygon
            // 
            this.ToolStripMenuItemDrawPolygon.Image = ((System.Drawing.Image)(resources.GetObject("ToolStripMenuItemDrawPolygon.Image")));
            this.ToolStripMenuItemDrawPolygon.Name = "ToolStripMenuItemDrawPolygon";
            this.ToolStripMenuItemDrawPolygon.Size = new System.Drawing.Size(136, 22);
            this.ToolStripMenuItemDrawPolygon.Text = "绘制多边形";
            this.ToolStripMenuItemDrawPolygon.Click += new System.EventHandler(this.ToolStripMenuItemDrawPolygon_Click);
            // 
            // toolStripButtonSaveScreen
            // 
            this.toolStripButtonSaveScreen.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonSaveScreen.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonSaveScreen.Image")));
            this.toolStripButtonSaveScreen.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonSaveScreen.Name = "toolStripButtonSaveScreen";
            this.toolStripButtonSaveScreen.Size = new System.Drawing.Size(23, 22);
            this.toolStripButtonSaveScreen.Text = "保存屏幕截图";
            this.toolStripButtonSaveScreen.Click += new System.EventHandler(this.toolStripButtonSaveScreen_Click);
            // 
            // toolStripButtonGoto
            // 
            this.toolStripButtonGoto.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonGoto.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonGoto.Image")));
            this.toolStripButtonGoto.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonGoto.Name = "toolStripButtonGoto";
            this.toolStripButtonGoto.Size = new System.Drawing.Size(23, 22);
            this.toolStripButtonGoto.Text = "定位到地球上任何位置";
            this.toolStripButtonGoto.Click += new System.EventHandler(this.toolStripButtonGoto_Click);
            // 
            // toolStripLabel1
            // 
            this.toolStripLabel1.Name = "toolStripLabel1";
            this.toolStripLabel1.Size = new System.Drawing.Size(48, 22);
            this.toolStripLabel1.Text = "          ";
            // 
            // QRSTWorldToolbox
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.toolStrip1);
            this.Name = "QRSTWorldToolbox";
            this.Size = new System.Drawing.Size(639, 26);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton toolStripButtonReset;
        private System.Windows.Forms.ToolStripButton toolStripButtonLayerManager;
        private System.Windows.Forms.ToolStripSplitButton toolStripSplitButtonShowOrNot;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItemGridLine;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItemSun;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItemPosition;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItemCopyright;
        private System.Windows.Forms.ToolStripDropDownButton toolStripDropDownButtonAddLayer;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItemAddImageLayer;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItemAddShapeLayer;
        private System.Windows.Forms.ToolStripDropDownButton toolStripDropDownButtonAddGCP;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItemAddGeoGCP;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItemAddATMGCP;
        private System.Windows.Forms.ToolStripDropDownButton toolStripDropDownButtonDraw;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItemDrawPolyLine;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItemDrawRectangle;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItemDrawPolygon;
        private System.Windows.Forms.ToolStripButton toolStripButtonSaveScreen;
        private System.Windows.Forms.ToolStripButton toolStripButtonGoto;
        private System.Windows.Forms.ToolStripLabel toolStripLabel1;

    }
}
