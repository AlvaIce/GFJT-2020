using System.Windows.Forms;
using QRST.WorldGlobeTool.Renderable;

namespace QRST.WorldGlobeTool.Menu
{
    /// <summary>
    /// 图层快捷菜单按钮
    /// </summary>
    public class LayerShortcutMenuButton : MenuButton
    {
        #region 私有字段

        /// <summary>
        /// 是否按下
        /// </summary>
        protected bool m_isPushed = false;
        /// <summary>
        /// 渲染对象
        /// </summary>
        protected RenderableObject m_ro;
        /// <summary>
        /// 描述信息
        /// </summary>
        public string Description = "";

        #endregion

        /// <summary>
        /// 初始化一个LayerShortcutMenuButton对象
        /// </summary>
        /// <param name="imageFilePath">图像文件路径</param>
        /// <param name="ro">渲染对象</param>
        public LayerShortcutMenuButton(
            string imageFilePath, RenderableObject ro)
        {
            this.Description = ro.Name;
            this.m_ro = ro;
            this.m_isPushed = ro.IsOn;
        }

        #region 接口成员

        public override void Dispose()
        {
            base.Dispose();
        }

        public override bool IsPushed()
        {
            return this.m_isPushed;
        }

        public override void SetPushed(bool isPushed)
        {
            this.m_isPushed = isPushed;
            if (!this.m_ro.IsOn && this.m_ro.ParentList != null && this.m_ro.ParentList.IsShowOnlyOneLayer)
                this.m_ro.ParentList.TurnOffAllChildren();

            this.m_ro.IsOn = this.m_isPushed;

        }

        public override void OnKeyDown(KeyEventArgs keyEvent)
        {
        }

        public override void OnKeyUp(KeyEventArgs keyEvent)
        {

        }

        public override void Update(DrawArgs drawArgs)
        {
            if (this.m_ro.IsOn != this.m_isPushed)
                this.m_isPushed = this.m_ro.IsOn;
        }

        public override void Render(DrawArgs drawArgs)
        {
        }

        public override bool OnMouseDown(MouseEventArgs e)
        {
            return false;
        }

        public override bool OnMouseMove(MouseEventArgs e)
        {
            return false;
        }

        public override bool OnMouseUp(MouseEventArgs e)
        {
            return false;
        }

        public override bool OnMouseWheel(MouseEventArgs e)
        {
            return false;
        }
        #endregion

    }

}
