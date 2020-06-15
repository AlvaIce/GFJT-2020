using Microsoft.DirectX;
using System.Windows.Forms;

namespace QRST.WorldGlobeTool.Menu
{

    /// <summary>
    /// 边侧条菜单抽象基类
    /// </summary>
    public abstract class SideBarMenu : IMenu
    {
        #region 字段

        /// <summary>
        /// 边侧条编号
        /// </summary>
        public long Id;
        /// <summary>
        /// 左侧位置
        /// </summary>
        public int Left;
        /// <summary>
        /// 顶部位置
        /// </summary>
        public int Top;
        /// <summary>
        /// 右侧位置
        /// </summary>
        public int Right = World.Settings.layerManagerWidth;
        /// <summary>
        /// 底部位置
        /// </summary>
        public int Bottom;
        /// <summary>
        /// 高度比例
        /// </summary>
        public readonly float HeightPercent = 0.9f;
        /// <summary>
        /// 外边框顶点列表
        /// </summary>
        private Vector2[] outlineVerts = new Vector2[5];
        /// <summary>
        /// 边侧条菜单栏的停靠位置
        /// </summary>
        private SideBarMenuAnchor m_anchor = SideBarMenuAnchor.Right;

        #endregion

        #region 属性

        /// <summary>
        /// 获取或设置边侧条宽度
        /// </summary>
        public int Width
        {
            get { return Right - Left; }
            set { Right = Left + value; }
        }

        /// <summary>
        /// 获取或设置边侧条高度
        /// </summary>
        public int Height
        {
            get { return Bottom - Top; }
            set { Bottom = Top + value; }
        }

        /// <summary>
        /// 获取或设置边侧条菜单栏到停靠位置
        /// </summary>
        public SideBarMenuAnchor Anchor
        {
            get { return m_anchor; }
            set { m_anchor = value; }
        }

        #endregion

        #region IMenu接口成员

        public abstract void OnKeyUp(KeyEventArgs keyEvent);
        public abstract void OnKeyDown(KeyEventArgs keyEvent);
        public abstract bool OnMouseUp(MouseEventArgs e);
        public abstract bool OnMouseDown(MouseEventArgs e);
        public abstract bool OnMouseMove(MouseEventArgs e);
        public abstract bool OnMouseWheel(MouseEventArgs e);
        public void Render(DrawArgs drawArgs)
        {
            //if ((DrawArgs.WorldWindow != null) && (DrawArgs.WorldWindow.MenuBar.Anchor == MenuAnchor.Bottom))
            //{
            //    this.Top = 0;
            //    this.Bottom = drawArgs.ScreenHeight - 120;
            //}
            //else
            //{
            this.Top = 60;
            this.Bottom = drawArgs.ScreenHeight - 1;
            //}

            if (m_anchor == SideBarMenuAnchor.Left)
            {
                this.Left = 0;
                this.Right = Width;
            }
            else
            {
                this.Left = drawArgs.ScreenWidth - 1 - Width;
                this.Right = drawArgs.ScreenWidth - 1;
            }

            MenuUtils.DrawBox(Left, Top, Right - Left, Bottom - Top, 0.0f,
                World.Settings.menuBackColor, drawArgs.device);

            RenderContents(drawArgs);

            outlineVerts[0].X = Left;
            outlineVerts[0].Y = Top;

            outlineVerts[1].X = Right;
            outlineVerts[1].Y = Top;

            outlineVerts[2].X = Right;
            outlineVerts[2].Y = Bottom;

            outlineVerts[3].X = Left;
            outlineVerts[3].Y = Bottom;

            outlineVerts[4].X = Left;
            outlineVerts[4].Y = Top;

            MenuUtils.DrawLine(outlineVerts, World.Settings.menuOutlineColor, drawArgs.device);
        }

        public abstract void RenderContents(DrawArgs drawArgs);
        public abstract void Dispose();

        #endregion

    }

    /// <summary>
    /// 边侧条菜单停靠位置
    /// </summary>
    public enum SideBarMenuAnchor
    {
        Left,
        Right
    }

}
