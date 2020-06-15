using System;
using System.Drawing;
using System.Windows.Forms;
using QRST.WorldGlobeTool.PluginEngine;

namespace QRST.WorldGlobeTool.DrawUtility
{
    /// <summary>
    /// 绘制多边形区域
    /// </summary>
    public class DrawPolygonTool : Plugin
    {
        /// <summary>
        /// 要绘制的多边形区域图层
        /// </summary>
        public DrawPolygonLayer drawLayer;
        /// <summary>
        /// 绘制完毕事件委托
        /// </summary>
        public event EventHandler OnCompleted;
        public event EventHandler OnUp;

        public DrawPolygonTool()
            : base()
        {

        }

        /// <summary>
        /// 加载插件
        /// </summary>
        public override void Load()
        {
            drawLayer = new DrawPolygonLayer("多边形区域图层", Color.FromArgb(255, 255, 0, 0), this, ParentApplication.DrawArgs);
            drawLayer.IsOn = true;      //关闭WW自带响应事件
            ParentApplication.CurrentWorld.RenderableObjects.Add(drawLayer);

            // Subscribe events
            ParentApplication.MouseMove += new MouseEventHandler(drawLayer.MouseMove);
            ParentApplication.MouseDown += new MouseEventHandler(drawLayer.MouseDown);
            ParentApplication.MouseUp += new MouseEventHandler(drawLayer.MouseUp);
            ParentApplication.MouseDoubleClick += new MouseEventHandler(drawLayer.MouseDoubleClick);
            ParentApplication.KeyUp += new KeyEventHandler(drawLayer.KeyUp);
            drawLayer.OnCompleted += new EventHandler(drawLayer_OnCompleted);
            drawLayer.OnUp += new EventHandler(drawLayer_OnUp);
        }

        void drawLayer_OnCompleted(object sender, EventArgs e)
        {
            if (OnCompleted != null)
            {
                OnCompleted(this, e);
            }
        }

        /// <summary>
        /// 卸载插件
        /// </summary>
        public override void Unload()
        {
            ParentApplication.MouseMove -= new MouseEventHandler(drawLayer.MouseMove);
            ParentApplication.MouseDown -= new MouseEventHandler(drawLayer.MouseDown);
            ParentApplication.MouseUp -= new MouseEventHandler(drawLayer.MouseUp);
            ParentApplication.MouseDoubleClick -= new MouseEventHandler(drawLayer.MouseDoubleClick);
            ParentApplication.KeyUp -= new KeyEventHandler(drawLayer.KeyUp);

            ParentApplication.CurrentWorld.RenderableObjects.Remove(drawLayer);
            m_isLoaded = false;
            drawLayer.OnCompleted -= new EventHandler(drawLayer_OnCompleted);
            drawLayer.Dispose();
            drawLayer = null;
        }

        /// <summary>
        /// 加载插件
        /// </summary>
        /// <param name="parent">插件所属的球体控件</param>
        /// <param name="pluginDirectory">插件所在的目录</param>
        public override void PluginLoad(QRSTWorldGlobeControl parent, string pluginDirectory)
        {
            base.PluginLoad(parent, pluginDirectory);
        }
        void drawLayer_OnUp(object sender, EventArgs e)
        {
            if (OnUp != null)
            {
                OnUp(sender, e);
            }
        }
    }
}
