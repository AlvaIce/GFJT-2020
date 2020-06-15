using System;
using System.Windows.Forms;
using System.Drawing;
using QRST.WorldGlobeTool.PluginEngine;

namespace QRST.WorldGlobeTool.DrawUtility
{
    /// <summary>
    /// 绘制多线段工具
    /// </summary>
    public class DrawPolyLineTool : Plugin
    {
        /// <summary>
        /// 与工具箱关联的绘制多线段图层
        /// </summary>
        public DrawPolylineLayer drawLayer;
        /// <summary>
        /// 绘制完毕事件委托
        /// </summary>
        public event EventHandler OnCompleted;
        public event EventHandler OnPolyUp;
        /// <summary>
        /// 初始化一个DrawPolyLineTool实例
        /// </summary>
        public DrawPolyLineTool()
            : base()
        {

        }

        /// <summary>
        /// 加载工具
        /// </summary>
        public override void Load()
        {
            drawLayer = new DrawPolylineLayer("多线段图层", Color.FromArgb(255, 255, 0, 0), this, ParentApplication.DrawArgs);
            drawLayer.IsOn = true;      //关闭WW自带响应事件
            ParentApplication.CurrentWorld.RenderableObjects.Add(drawLayer);

            // Subscribe events
            ParentApplication.MouseMove += new MouseEventHandler(drawLayer.MouseMove);
            ParentApplication.MouseDown += new MouseEventHandler(drawLayer.MouseDown);
            ParentApplication.MouseUp += new MouseEventHandler(drawLayer.MouseUp);
            ParentApplication.MouseDoubleClick += new MouseEventHandler(drawLayer.MouseDoubleClick);
            ParentApplication.KeyUp += new KeyEventHandler(drawLayer.KeyUp);
            drawLayer.OnCompleted += new EventHandler(drawLayer_OnCompleted);
            drawLayer.OnPolyUp += new EventHandler(drawLayer_OnPolyUp);
        }

        void drawLayer_OnCompleted(object sender, EventArgs e)
        {
            if (OnCompleted != null)
            {
                OnCompleted(this, e);
            }
        }

        void drawLayer_OnPolyUp(object sender, EventArgs e)
        {
            if (OnPolyUp != null)
            {
                OnPolyUp(this, e);
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
            drawLayer.OnPolyUp -= new EventHandler(drawLayer_OnPolyUp);
            drawLayer.Dispose();
            drawLayer = null;
        }

        /// <summary>
        /// 加载插件
        /// </summary>
        /// <param name="parent">插件所属的球体控件</param>
        /// <param name="pluginDirectory">插件目录</param>
        public override void PluginLoad(QRSTWorldGlobeControl parent, string pluginDirectory)
        {
            base.PluginLoad(parent, pluginDirectory);
        }
    }
}
