using System;
using System.Windows.Forms;
using Microsoft.DirectX.Direct3D;
using System.Drawing;
using QRST.WorldGlobeTool.Utility;
using Microsoft.DirectX;

namespace QRST.WorldGlobeTool.Menu
{
    /// <summary>
    /// 菜单按钮抽象基类
    /// </summary>
    public abstract class MenuButton : IMenu
    {

        #region 私有字段

        /// <summary>
        /// 按钮图标路径
        /// </summary>
        private string _iconTexturePath;
        /// <summary>
        /// 按钮图标纹理
        /// </summary>
        private Texture m_iconTexture;
        /// <summary>
        /// 按钮图标纹理尺寸
        /// </summary>
        private System.Drawing.Size _iconTextureSize;
        /// <summary>
        /// 按钮描述性信息
        /// </summary>
        string _description;
        /// <summary>
        /// 按钮当前尺寸
        /// </summary>
        float curSize;
        /// <summary>
        /// 白色
        /// </summary>
        static int white = System.Drawing.Color.White.ToArgb();
        /// <summary>
        /// 黑色
        /// </summary>
        static int black = System.Drawing.Color.Black.ToArgb();
        /// <summary>
        /// 透明色
        /// </summary>
        static int transparent = Color.FromArgb(140, 255, 255, 255).ToArgb();
        /// <summary>
        /// α通道
        /// </summary>
        int alpha;
        /// <summary>
        /// α步幅
        /// </summary>
        const int alphaStep = 30;
        /// <summary>
        /// 缩放比例
        /// </summary>
        const float zoomSpeed = 1.2f;
        /// <summary>
        /// 正常尺寸
        /// </summary>
        public static float NormalSize;
        /// <summary>
        /// 选中时的尺寸
        /// </summary>
        public static float SelectedSize;

        #endregion

        #region 属性

        /// <summary>
        /// 获取或设置按钮的描述信息
        /// </summary>
        public string Description
        {
            get
            {
                if (this._description == null)
                    return "N/A";
                else
                    return this._description;
            }
            set
            {
                this._description = value;
            }
        }

        /// <summary>
        /// 获取按钮的图标纹理
        /// </summary>
        public Texture IconTexture
        {
            get
            {
                return m_iconTexture;
            }
        }

        /// <summary>
        /// 获取按钮的图标尺寸
        /// </summary>
        public System.Drawing.Size IconTextureSize
        {
            get
            {
                return this._iconTextureSize;
            }
        }

        /// <summary>
        /// 获取按钮当前尺寸
        /// </summary>
        public float CurrentSize
        {
            get { return curSize; }
        }

        #endregion

        #region 构造函数

        /// <summary>
        /// 初始化一个MenuButton实例
        /// </summary>
        protected MenuButton()
        { }

        /// <summary>
        /// 初始化一个MenuButton实例
        /// </summary>
        /// <param name="iconTexturePath">图标纹理文件路径</param>
        public MenuButton(string iconTexturePath)
        {
            this._iconTexturePath = iconTexturePath;
        } 

        #endregion

        /// <summary>
        /// 初始化纹理
        /// </summary>
        /// <param name="device"></param>
        public void InitializeTexture(Device device)
        {
            try
            {
                m_iconTexture = ImageHelper.LoadIconTexture(this._iconTexturePath);

                using (Surface s = m_iconTexture.GetSurfaceLevel(0))
                {
                    SurfaceDescription desc = s.Description;
                    this._iconTextureSize = new Size(desc.Width, desc.Height);
                }
            }
            catch
            {
            }
        }

        /// <summary>
        /// 渲染标签
        /// </summary>
        /// <param name="drawArgs">绘制参数</param>
        /// <param name="x">标签X位置</param>
        /// <param name="y">标签Y位置</param>
        /// <param name="buttonHeight">按钮高度</param>
        /// <param name="selected">是否被选中</param>
        /// <param name="anchor">菜单位置</param>
        public void RenderLabel(DrawArgs drawArgs, int x, int y, int buttonHeight, bool selected, MenuAnchor anchor)
        {
            if (selected)
            {
                if (buttonHeight == curSize)
                {
                    alpha += alphaStep;
                    if (alpha > 255)
                        alpha = 255;
                }
            }
            else
            {
                alpha -= alphaStep;
                if (alpha < 0)
                {
                    alpha = 0;
                    return;
                }
            }

            int halfWidth = (int)(SelectedSize * 0.75);
            int label_x = x - halfWidth + 1;
            int label_y = (int)(y + SelectedSize) + 1;

            DrawTextFormat format = DrawTextFormat.NoClip | DrawTextFormat.Center | DrawTextFormat.WordBreak;

            if (anchor == MenuAnchor.Bottom)
            {
                format |= DrawTextFormat.Bottom;
                label_y = y - 202;
            }

            Rectangle rect = new System.Drawing.Rectangle(label_x, label_y, (int)halfWidth * 2, 200);

            if (rect.Right > drawArgs.ScreenWidth)
            {
                rect = Rectangle.FromLTRB(rect.Left, rect.Top, drawArgs.ScreenWidth, rect.Bottom);
            }

            drawArgs.ToolbarFont.DrawText(null, Description, rect, format, black & 0xffffff + (alpha << 24));

            rect.Offset(2, 0);
            drawArgs.ToolbarFont.DrawText(null, Description, rect, format, black & 0xffffff + (alpha << 24));

            rect.Offset(0, 2);
            drawArgs.ToolbarFont.DrawText(null, Description, rect, format, black & 0xffffff + (alpha << 24));

            rect.Offset(-2, 0);
            drawArgs.ToolbarFont.DrawText(null, Description, rect, format, black & 0xffffff + (alpha << 24));

            rect.Offset(1, -1);
            drawArgs.ToolbarFont.DrawText(null, Description, rect, format, white & 0xffffff + (alpha << 24));
        }

        /// <summary>
        /// 渲染启用的图标
        /// </summary>
        /// <param name="sprite"></param>
        /// <param name="drawArgs">绘制参数</param>
        /// <param name="centerX">中心点X坐标</param>
        /// <param name="topY">顶部Y坐标</param>
        /// <param name="selected">是否选中</param>
        [Obsolete]
        public void RenderEnabledIcon(Sprite sprite, DrawArgs drawArgs, float centerX, float topY,
                                      bool selected)
        {
            RenderEnabledIcon(sprite, drawArgs, centerX, topY, selected, MenuAnchor.Top);
        }

        /// <summary>
        /// 渲染启用的图标
        /// </summary>
        /// <param name="sprite"></param>
        /// <param name="drawArgs">绘制参数</param>
        /// <param name="centerX">中心点X坐标</param>
        /// <param name="topY">顶部Y坐标</param>
        /// <param name="selected">是否选中</param>
        /// <param name="anchor">菜单位置</param>
        public void RenderEnabledIcon(Sprite sprite, DrawArgs drawArgs, float centerX, float topY,
            bool selected, MenuAnchor anchor)
        {
            float width = selected ? MenuButton.SelectedSize : width = MenuButton.NormalSize;

            RenderLabel(drawArgs, (int)centerX, (int)topY, (int)width, selected, anchor);

            int color = selected ? white : transparent;

            float centerY = topY + curSize * 0.5f;
            this.RenderIcon(sprite, (int)centerX, (int)centerY, (int)curSize, (int)curSize, color, m_iconTexture);

            if (curSize == 0)
                curSize = width;
            if (width > curSize)
            {
                curSize = (int)(curSize * zoomSpeed);
                if (width < curSize)
                    curSize = width;
            }
            else if (width < curSize)
            {
                curSize = (int)(curSize / zoomSpeed);
                if (width > curSize)
                    curSize = width;
            }
        }

        /// <summary>
        /// 渲染图标
        /// </summary>
        /// <param name="sprite"></param>
        /// <param name="centerX">中心点X坐标</param>
        /// <param name="centerY">中心点Y坐标</param>
        /// <param name="buttonWidth">按钮宽度</param>
        /// <param name="buttonHeight">按钮高度</param>
        /// <param name="color">颜色</param>
        /// <param name="t">纹理</param>
        private void RenderIcon(Sprite sprite, float centerX, float centerY,
            int buttonWidth, int buttonHeight, int color, Texture t)
        {
            int halfIconWidth = (int)(0.5f * buttonWidth);
            int halfIconHeight = (int)(0.5f * buttonHeight);

            float scaleWidth = (float)buttonWidth / this._iconTextureSize.Width;
            float scaleHeight = (float)buttonHeight / this._iconTextureSize.Height;

            sprite.Transform = Matrix.Transformation2D(
                Vector2.Empty, 0.0f,
                new Vector2(scaleWidth, scaleHeight),
                Vector2.Empty,
                0.0f,
                new Vector2(centerX, centerY));

            sprite.Draw(t,
                new Vector3(this._iconTextureSize.Width / 2.0f, this._iconTextureSize.Height / 2.0f, 0),
                Vector3.Empty,
                color);
        }


        public abstract bool IsPushed();
        public abstract void SetPushed(bool isPushed);
        public abstract void Update(DrawArgs drawArgs);
        public abstract void Render(DrawArgs drawArgs);
        public abstract bool OnMouseUp(MouseEventArgs e);
        public abstract bool OnMouseMove(MouseEventArgs e);
        public abstract bool OnMouseDown(MouseEventArgs e);
        public abstract bool OnMouseWheel(MouseEventArgs e);
        public abstract void OnKeyUp(KeyEventArgs keyEvent);
        public abstract void OnKeyDown(KeyEventArgs keyEvent);
        public virtual void Dispose()
        {
            if (m_iconTexture != null)
            {
                m_iconTexture.Dispose();
                m_iconTexture = null;
            }
        }
    }
}
