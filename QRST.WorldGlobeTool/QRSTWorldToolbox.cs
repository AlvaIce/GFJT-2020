using System;
using System.Windows.Forms;
using QRST.WorldGlobeTool.Renderable;
using QRST.WorldGlobeTool.VisualForms;
using QRST.WorldGlobeTool.PluginEngine;

namespace QRST.WorldGlobeTool
{
    public partial class QRSTWorldToolbox : UserControl
    {
        public QRSTWorldToolbox()
        {
            InitializeComponent();
        }

        private QRSTWorldGlobeControl m_BuddyGlobeControl;
        /// <summary>
        /// 获取或设置与当前工具条绑定的球体控件
        /// </summary>
        public QRSTWorldGlobeControl BuddyGlobeControl
        {
            get { return m_BuddyGlobeControl; }
            set
            {
                m_BuddyGlobeControl = value;
            }
        }
        
        /// <summary>
        /// 重置球体到初始状态
        /// </summary>
        private void toolStripButtonReset_Click(object sender, EventArgs e)
        {
            if (m_BuddyGlobeControl != null)
                this.m_BuddyGlobeControl.ResetGlobe();
        }

        /// <summary>
        /// 打开/关闭图层管理器
        /// </summary>
        private void toolStripButtonLayerManager_Click(object sender, EventArgs e)
        {
            if (m_BuddyGlobeControl != null)
            {
                World.Settings.ShowLayerManager = !World.Settings.ShowLayerManager;
                m_BuddyGlobeControl.LayerManagerButton.SetPushed(World.Settings.ShowLayerManager);
            }
        }

        /// <summary>
        /// 显示/隐藏经纬网格线
        /// </summary>
        private void ToolStripMenuItemGridLine_Click(object sender, EventArgs e)
        {
            if (m_BuddyGlobeControl != null)
                this.m_BuddyGlobeControl.QrstGlobe.ShowLatLonLines = !this.m_BuddyGlobeControl.QrstGlobe.ShowLatLonLines;
        }

        /// <summary>
        /// 开启/关闭太阳效果
        /// </summary>
        private void ToolStripMenuItemSun_Click(object sender, EventArgs e)
        {
            if (m_BuddyGlobeControl != null)
                this.m_BuddyGlobeControl.QrstGlobe.ShowSunShading = !this.m_BuddyGlobeControl.QrstGlobe.ShowSunShading;
        }

        /// <summary>
        /// 显示/隐藏位置信息
        /// </summary>
        private void ToolStripMenuItemPosition_Click(object sender, EventArgs e)
        {
            if (m_BuddyGlobeControl != null)
                this.m_BuddyGlobeControl.QrstGlobe.ShowPosition = !this.m_BuddyGlobeControl.QrstGlobe.ShowPosition;
        }

        /// <summary>
        /// 显示/隐藏版权信息
        /// </summary>
        private void ToolStripMenuItemCopyright_Click(object sender, EventArgs e)
        {
            if (m_BuddyGlobeControl != null)
                this.m_BuddyGlobeControl.QrstGlobe.ShowCopyright = !this.m_BuddyGlobeControl.QrstGlobe.ShowCopyright;
        }

        /// <summary>
        /// 添加栅格图层
        /// </summary>
        private void ToolStripMenuItemAddImageLayer_Click(object sender, EventArgs e)
        {
            foreach (Form f in Application.OpenForms)
            {
                if (f is AddImagesLayer)
                {
                    return;
                }
            }
            AddImagesLayer form = new AddImagesLayer(this.m_BuddyGlobeControl);
            form.ShowInTaskbar = false;
            form.Show();
        }

        /// <summary>
        /// 添加矢量图层
        /// </summary>
        private void ToolStripMenuItemAddShapeLayer_Click(object sender, EventArgs e)
        {
            foreach (Form f in Application.OpenForms)
            {
                if (f is AddShapeLayer)
                {
                    return;
                }
            }
            AddShapeLayer form = new AddShapeLayer(this.m_BuddyGlobeControl);
            form.ShowInTaskbar = false;
            form.Show();
        }

        /// <summary>
        /// 添加几何控制点
        /// </summary>
        private void ToolStripMenuItemAddGeoGCP_Click(object sender, EventArgs e)
        {
            if (m_BuddyGlobeControl != null)
                this.m_BuddyGlobeControl.QrstGlobe.AddGCPLayerBySelectFile(GCPType.GeoGCP);
        }

        /// <summary>
        /// 添加辐射控制点
        /// </summary>
        private void ToolStripMenuItemAddATMGCP_Click(object sender, EventArgs e)
        {
            if (m_BuddyGlobeControl != null)
                this.m_BuddyGlobeControl.QrstGlobe.AddGCPLayerBySelectFile(GCPType.ATMGCP);
        }

        /// <summary>
        /// 绘制多线段
        /// </summary>
        private void ToolStripMenuItemDrawPolyLine_Click(object sender, EventArgs e)
        {
            if (m_BuddyGlobeControl != null)
                this.m_BuddyGlobeControl.UsingDrawPolyLineTool();
        }

        /// <summary>
        /// 绘制矩形区域
        /// </summary>
        private void ToolStripMenuItemDrawRectangle_Click(object sender, EventArgs e)
        {
            if (m_BuddyGlobeControl != null)
                this.m_BuddyGlobeControl.UsingDrawRectangleTool();
        }

        /// <summary>
        /// 绘制多边形区域
        /// </summary>
        private void ToolStripMenuItemDrawPolygon_Click(object sender, EventArgs e)
        {
            if (m_BuddyGlobeControl != null)
                this.m_BuddyGlobeControl.UsingDrawPloygonTool();
        }

        /// <summary>
        /// 保存屏幕截图
        /// </summary>
        private void toolStripButtonSaveScreen_Click(object sender, EventArgs e)
        {
            if (m_BuddyGlobeControl != null)
            {
                SaveFileDialog sfd = new SaveFileDialog();
                sfd.Title = "保存当前三维球体屏幕截图";
                sfd.Filter = "BMP文件|*.bmp|JPG文件|*.jpg|PNG文件|*.png";
                sfd.FileName = "三维球体截屏";
                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    this.m_BuddyGlobeControl.SaveScreenshot(sfd.FileName);
                }
            }
        }

        /// <summary>
        /// 位置搜索定位
        /// </summary>
        private void toolStripButtonGoto_Click(object sender, EventArgs e)
        {
            foreach (Form f in Application.OpenForms)
            {
                if (f is GotoPlace)
                {
                    return;
                }
            }
            GotoPlace form = new GotoPlace(this.m_BuddyGlobeControl);
            form.ShowInTaskbar = false;
            form.TopMost = true;
            form.StartPosition = FormStartPosition.WindowsDefaultBounds;
            form.Show();
        }

        /// <summary>
        /// 插件管理
        /// </summary>
        private void toolStripButtonPluginManage_Click(object sender, EventArgs e)
        {
            foreach (Form f in Application.OpenForms)
            {
                if (f is PluginDialog)
                {
                    return;
                }
            }
            PluginDialog form = new PluginDialog(this.m_BuddyGlobeControl.PCompiler);
            form.ShowInTaskbar = false;
            form.TopMost = false;
            form.StartPosition = FormStartPosition.CenterParent;
            form.Show();
        }

    }
}
