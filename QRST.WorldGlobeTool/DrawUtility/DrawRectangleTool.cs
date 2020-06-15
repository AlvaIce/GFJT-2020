using System;
using System.Drawing;
using System.Windows.Forms;
using QRST.WorldGlobeTool.PluginEngine;

namespace QRST.WorldGlobeTool.DrawUtility
{
    /// <summary>
    /// 绘制矩形框工具
    /// </summary>
    public class DrawRectangleTool : Plugin
    {
        /// <summary>
        /// 要绘制的矩形框图层
        /// </summary>
        public DrawRectangleLayer drawLayer;
        /// <summary>
        /// 绘制完毕事件委托
        /// </summary>
        public event EventHandler OnCompleted;

        public DrawRectangleTool()
            : base()
        {

        }

        /// <summary>
        /// 加载绘制矩形框插件
        /// </summary>
        public override void Load()
        {
            drawLayer = new DrawRectangleLayer("矩形区域图层", Color.FromArgb(255, 255, 0, 0), this, ParentApplication.DrawArgs);
            drawLayer.IsOn = true;      //关闭WW自带响应事件
            ParentApplication.CurrentWorld.RenderableObjects.Add(drawLayer);        //添加图层 drawLayer

            // Subscribe events
            ParentApplication.MouseMove += new MouseEventHandler(drawLayer.MouseMove);
            ParentApplication.MouseDown += new MouseEventHandler(drawLayer.MouseDown);
            ParentApplication.MouseUp += new MouseEventHandler(drawLayer.MouseUp);
            ParentApplication.MouseDoubleClick += new MouseEventHandler(drawLayer.MouseDoubleClick);
            ParentApplication.KeyUp += new KeyEventHandler(drawLayer.KeyUp);
            drawLayer.OnCompleted += new EventHandler(drawLayer_OnCompleted);
        }

        /// <summary>
        /// 图层绘制完毕事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
        /// 插件加载
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="pluginDirectory"></param>
        public override void PluginLoad(QRSTWorldGlobeControl parent, string pluginDirectory)
        {
            base.PluginLoad(parent, pluginDirectory);
        }
    }
}
