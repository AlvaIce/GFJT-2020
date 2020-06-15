using System.Windows.Forms;

namespace QRST.WorldGlobeTool.Menu
{

    /// <summary>
    /// 图层管理按钮，控制图层管理菜单的显示与关闭
    /// </summary>
    public class LayerManagerButton : MenuButton
    {

        #region 字段

        /// <summary>
        /// 地球对象
        /// </summary>
        private World m_parentWorld;
        /// <summary>
        /// 图层管理菜单对象
        /// </summary>
        private LayerManagerMenu lmm; 

        #endregion

        #region 构造函数

        /// <summary>
        /// 初始化一个LayerManagerButton对象
        /// </summary>
        /// <param name="iconImagePath">图标文件路径</param>
        /// <param name="parentWorld">所属的地球对象</param>
        public LayerManagerButton(
            string iconImagePath,
            World parentWorld)
            : base(iconImagePath)
        {
            this.m_parentWorld = parentWorld;
            this.Description = "图层管理";
        } 

        #endregion

        #region 接口成员

        public override void Dispose()
        {
            base.Dispose();
        }

        /// <summary>
        /// 菜单按钮是否被按下
        /// </summary>
        /// <returns></returns>
        public override bool IsPushed()
        {
            return World.Settings.showLayerManager;
        }

        public override void Update(DrawArgs drawArgs)
        {
        }

        public override void OnKeyDown(KeyEventArgs keyEvent)
        {
        }

        public override void OnKeyUp(KeyEventArgs keyEvent)
        {
        }

        /// <summary>
        /// 鼠标按下事件
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        public override bool OnMouseDown(MouseEventArgs e)
        {
            if (IsPushed())
                return this.lmm.OnMouseDown(e);
            else
                return false;
        }

        public override bool OnMouseMove(MouseEventArgs e)
        {
            if (lmm != null && IsPushed())
                return this.lmm.OnMouseMove(e);
            else
                return false;
        }

        public override bool OnMouseUp(MouseEventArgs e)
        {
            if (this.IsPushed())
                return this.lmm.OnMouseUp(e);
            else
                return false;
        }

        public override bool OnMouseWheel(MouseEventArgs e)
        {
            if (this.IsPushed())
                return this.lmm.OnMouseWheel(e);
            else
                return false;
        }

        /// <summary>
        /// 渲染控件
        /// </summary>
        /// <param name="drawArgs"></param>
        public override void Render(DrawArgs drawArgs)
        {
            if (IsPushed())
            {
                if (lmm == null)
                    lmm = new LayerManagerMenu(m_parentWorld, this);

                lmm.Render(drawArgs);
            }
        }

        /// <summary>
        /// 设置按钮按下状态
        /// </summary>
        /// <param name="isPushed"></param>
        public override void SetPushed(bool isPushed)
        {
            World.Settings.showLayerManager = isPushed;
        } 

        #endregion

    }

}
