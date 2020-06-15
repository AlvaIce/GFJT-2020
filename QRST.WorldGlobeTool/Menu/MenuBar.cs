using System;
using System.Drawing;
using System.Collections;
using System.Windows.Forms;
using Microsoft.DirectX.Direct3D;

namespace QRST.WorldGlobeTool.Menu
{
    /// <summary>
    /// 按钮菜单条
    /// </summary>
    public class MenuBar : IMenu
    {

        #region 私有字段

        /// <summary>
        /// 工具菜单按钮列表
        /// </summary>
        protected ArrayList m_toolsMenuButtons = new ArrayList();
        /// <summary>
        /// 图层菜单按钮列表
        /// </summary>
        protected ArrayList m_layersMenuButtons = new ArrayList();
        /// <summary>
        /// 可见状态
        /// </summary>
        protected VisibleState _visibleState = VisibleState.Visible;
        /// <summary>
        /// 上一次可见性改变时间
        /// </summary>
        protected DateTime _lastVisibleChange = DateTime.Now;
        /// <summary>
        /// 菜单内部控件间距
        /// </summary>
        protected float _outerPadding = 5;
        /// <summary>
        /// 菜单X位置
        /// </summary>
        protected int x;
        /// <summary>
        /// 菜单Y位置
        /// </summary>
        protected int y;
        /// <summary>
        /// 菜单隐藏时间（毫秒）
        /// </summary>
        protected int hideTimeMilliseconds = 100;
        /// <summary>
        /// 是否可隐藏
        /// </summary>
        protected bool _isHideable;
        /// <summary>
        /// 圆角比率
        /// </summary>
        protected const float padRatio = 1 / 9.0f;
        /// <summary>
        /// 鼠标光标类型
        /// </summary>
        protected CursorType mouseCursor;
        /// <summary>
        /// 尖头颜色
        /// </summary>
        protected int chevronColor = Color.Black.ToArgb();
        /// <summary>
        /// 开启状态下到尖头
        /// </summary>
        protected CustomVertex.TransformedColored[] enabledChevron = new CustomVertex.TransformedColored[3];
        /// <summary>
        /// DirectX3D小精灵
        /// </summary>
        protected Sprite m_sprite;
        /// <summary>
        /// 当前选中项索引
        /// </summary>
        int _curSelection = -1;

        #endregion

        #region 属性

        /// <summary>
        /// 获取或设置菜单到停靠位置
        /// </summary>
        public MenuAnchor Anchor
        {
            get { return m_anchor; }
            set { m_anchor = value; }
        }
        private MenuAnchor m_anchor = MenuAnchor.Top;

        /// <summary>
        /// Indicates whether the menu is "open". (user activity)
        /// 获取菜单是否打开，处于活动状态
        /// </summary>
        public bool IsActive
        {
            get
            {
                return (this._curSelection >= 0);
            }
        }

        /// <summary>
        /// 获取或设置图层菜单按钮列表
        /// </summary>
        public System.Collections.ArrayList LayersMenuButtons
        {
            get
            {
                return m_layersMenuButtons;
            }
            set
            {
                m_layersMenuButtons = value;
            }
        }

        /// <summary>
        /// 获取或设置工具菜单按钮列表
        /// </summary>
        public System.Collections.ArrayList ToolsMenuButtons
        {
            get
            {
                return m_toolsMenuButtons;
            }
            set
            {
                m_toolsMenuButtons = value;
            }
        }

        #endregion

        #region 公共方法

        /// <summary>
        /// 初始化一个MenuBar实例
        /// </summary>
        /// <param name="anchor">菜单停靠位置</param>
        /// <param name="iconSize">按钮图标尺寸</param>
        public MenuBar(MenuAnchor anchor, int iconSize)
        {
            m_anchor = anchor;
            MenuButton.SelectedSize = iconSize;
        }

        /// <summary>
        /// 向菜单条添加一个工具菜单按钮
        /// </summary>
        /// <param name="button">工具菜单按钮</param>
        public void AddToolsMenuButton(MenuButton button)
        {
            lock (m_toolsMenuButtons.SyncRoot)
            {
                m_toolsMenuButtons.Add(button);
            }
        }

        /// <summary>
        /// 向菜单条指定位置添加一个工具菜单按钮
        /// </summary>
        /// <param name="button">工具菜单按钮</param>
        /// <param name="index">按钮编号</param>
        public void AddToolsMenuButton(MenuButton button, int index)
        {
            lock (m_toolsMenuButtons.SyncRoot)
            {
                if (index < 0)
                    m_toolsMenuButtons.Insert(0, button);
                else if (index >= m_toolsMenuButtons.Count)
                    m_toolsMenuButtons.Add(button);
                else
                    m_toolsMenuButtons.Insert(index, button);
            }
        }

        /// <summary>
        /// 如果找到相应的工具菜单按钮，则从菜单条中移除它
        /// </summary>
        /// <param name="button">要移除的菜单按钮</param>
        public void RemoveToolsMenuButton(MenuButton button)
        {
            lock (m_toolsMenuButtons.SyncRoot)
            {
                m_toolsMenuButtons.Remove(button);
            }
        }

        /// <summary>
        /// 添加一个图层按钮到当前菜单条中
        /// </summary>
        /// <param name="button">图层菜单按钮</param>
        public void AddLayersMenuButton(MenuButton button)
        {
            lock (m_layersMenuButtons.SyncRoot)
            {
                m_layersMenuButtons.Add(button);
            }
        }

        /// <summary>
        /// 添加一个图层按钮到当前菜单条中的指定位置
        /// </summary>
        /// <param name="button">图层菜单按钮</param>
        /// <param name="index">按钮编号</param>
        public void AddLayersMenuButton(MenuButton button, int index)
        {
            lock (m_layersMenuButtons.SyncRoot)
            {
                if (index < m_layersMenuButtons.Count)
                    m_layersMenuButtons.Insert(0, button);
                else if (index >= m_layersMenuButtons.Count)
                    m_layersMenuButtons.Add(button);
                else
                    m_layersMenuButtons.Insert(index, button);
            }
        }

        /// <summary>
        /// 如果找到相应的图层菜单按钮，则从菜单条中移除它
        /// </summary>
        /// <param name="button">要移除的图层菜单按钮</param>
        public void RemoveLayersMenuButton(MenuButton button)
        {
            lock (m_layersMenuButtons.SyncRoot)
            {
                m_layersMenuButtons.Remove(button);
            }
        } 

        #endregion

        #region IMenu接口成员

        public void OnKeyUp(KeyEventArgs keyEvent)
        {
            // TODO:  Add ToolsMenuBar.OnKeyUp implementation
        }

        public void OnKeyDown(KeyEventArgs keyEvent)
        {
            // TODO:  Add ToolsMenuBar.OnKeyDown implementation
        }

        public bool OnMouseUp(MouseEventArgs e)
        {
            if (World.Settings.ShowToolbar)
            {
                if (this._curSelection != -1 && e.Button == MouseButtons.Left)
                {
                    if (this._curSelection < m_toolsMenuButtons.Count)
                    {
                        MenuButton button = (MenuButton)m_toolsMenuButtons[this._curSelection];
                        button.SetPushed(!button.IsPushed());
                    }
                    else
                    {
                        MenuButton button = (MenuButton)m_layersMenuButtons[this._curSelection - m_toolsMenuButtons.Count];
                        button.SetPushed(!button.IsPushed());
                    }

                    return true;
                }
            }

            // Pass message on to the "tools"
            foreach (MenuButton button in m_toolsMenuButtons)
                if (button.IsPushed())
                    if (button.OnMouseUp(e))
                        return true;

            return false;
        }

        public bool OnMouseDown(MouseEventArgs e)
        {
            // Trigger "tool" update
            foreach (MenuButton button in m_toolsMenuButtons)
                if (button.IsPushed())
                    if (button.OnMouseDown(e))
                        return true;

            return false;
        }

        public bool OnMouseMove(MouseEventArgs e)
        {
            // Default to arrow cursor every time mouse moves
            mouseCursor = CursorType.Arrow;

            // Trigger "tools" update
            foreach (MenuButton button in m_toolsMenuButtons)
                if (button.IsPushed())
                    if (button.OnMouseMove(e))
                        return true;

            if (!World.Settings.ShowToolbar)
                return false;

            if (this._visibleState == VisibleState.Visible)
            {
                float width, height;

                int buttonCount;

                int sel = -1;

                switch (m_anchor)
                {
                    case MenuAnchor.Top:
                        buttonCount = m_toolsMenuButtons.Count + m_layersMenuButtons.Count;
                        width = buttonCount * (_outerPadding + MenuButton.NormalSize) + _outerPadding;
                        height = _outerPadding * 2 + MenuButton.NormalSize;

                        if (e.Y >= y && e.Y <= y + height + 2 * _outerPadding)
                        {
                            sel = (int)((e.X - _outerPadding) / (MenuButton.NormalSize + _outerPadding));
                            if (sel < buttonCount)
                                mouseCursor = CursorType.Hand;
                            else
                                sel = -1;
                        }
                        _curSelection = sel;

                        break;

                    case MenuAnchor.Bottom:
                        buttonCount = m_toolsMenuButtons.Count + m_layersMenuButtons.Count;
                        width = buttonCount * (_outerPadding + MenuButton.NormalSize) + _outerPadding;
                        height = _outerPadding * 2 + MenuButton.NormalSize;

                        if (e.Y >= y && e.Y <= y + (height + 2 * _outerPadding))
                        {
                            sel = (int)((e.X - _outerPadding) / (MenuButton.NormalSize + _outerPadding));
                            if (sel < buttonCount)
                                mouseCursor = CursorType.Hand;
                            else
                                sel = -1;
                        }
                        _curSelection = sel;

                        break;

                    case MenuAnchor.Right:
                        width = _outerPadding * 2 + MenuButton.SelectedSize;
                        height = _outerPadding * 2 + (m_toolsMenuButtons.Count * m_layersMenuButtons.Count) * MenuButton.SelectedSize;

                        if (e.X >= x + _outerPadding && e.X <= x + width + _outerPadding &&
                            e.Y >= y + _outerPadding && e.Y <= y + height + _outerPadding)
                        {
                            int dx = (int)(e.Y - (y + _outerPadding));
                            _curSelection = (int)(dx / MenuButton.SelectedSize);
                        }
                        else
                        {
                            _curSelection = -1;
                        }
                        break;
                }
            }

            return false;
        }

        public bool OnMouseWheel(MouseEventArgs e)
        {
            // Trigger "tool" update
            foreach (MenuButton button in m_toolsMenuButtons)
                if (button.IsPushed())
                    if (button.OnMouseWheel(e))
                        return true;

            return false;
        }

        public void Render(DrawArgs drawArgs)
        {
            if (m_sprite == null)
                m_sprite = new Sprite(drawArgs.device);

            if (mouseCursor != CursorType.Arrow)
                DrawArgs.MouseCursor = mouseCursor;


            foreach (MenuButton button in m_toolsMenuButtons)
                if (button.IsPushed())
                    // Does not render the button, but the functionality behind the button
                    button.Render(drawArgs);

            foreach (MenuButton button in m_toolsMenuButtons)
                button.Update(drawArgs);

            foreach (MenuButton button in m_layersMenuButtons)
                button.Update(drawArgs);

            if (!World.Settings.ShowToolbar)
                return;

            if (this._isHideable)
            {
                if (this._visibleState == VisibleState.NotVisible)
                {
                    if (
                        (m_anchor == MenuAnchor.Top && DrawArgs.LastMousePosition.Y < MenuButton.NormalSize) ||
                        (m_anchor == MenuAnchor.Bottom && DrawArgs.LastMousePosition.Y > drawArgs.ScreenHeight - MenuButton.NormalSize) ||
                        (m_anchor == MenuAnchor.Right && DrawArgs.LastMousePosition.X > drawArgs.ScreenWidth - MenuButton.NormalSize)
                        )
                    {
                        this._visibleState = VisibleState.Ascending;
                        this._lastVisibleChange = System.DateTime.Now;
                    }
                }
                else if (
                    (m_anchor == MenuAnchor.Top && DrawArgs.LastMousePosition.Y > 2 * this._outerPadding + MenuButton.NormalSize) ||
                    (m_anchor == MenuAnchor.Bottom && DrawArgs.LastMousePosition.Y < drawArgs.ScreenHeight - 2 * this._outerPadding - MenuButton.NormalSize) ||
                    (m_anchor == MenuAnchor.Right && DrawArgs.LastMousePosition.X < drawArgs.ScreenWidth - MenuButton.NormalSize)
                    )
                {
                    if (this._visibleState == VisibleState.Visible)
                    {
                        this._visibleState = VisibleState.Descending;
                        this._lastVisibleChange = System.DateTime.Now;
                    }
                    else if (this._visibleState == VisibleState.Descending)
                    {
                        if (System.DateTime.Now.Subtract(this._lastVisibleChange) > System.TimeSpan.FromMilliseconds(hideTimeMilliseconds))
                        {
                            this._visibleState = VisibleState.NotVisible;
                            this._lastVisibleChange = System.DateTime.Now;
                        }
                    }
                }
                else if (this._visibleState == VisibleState.Ascending)
                {
                    if (System.DateTime.Now.Subtract(this._lastVisibleChange) > System.TimeSpan.FromMilliseconds(hideTimeMilliseconds))
                    {
                        this._visibleState = VisibleState.Visible;
                        this._lastVisibleChange = System.DateTime.Now;
                    }
                }
                else if (this._visibleState == VisibleState.Descending)
                {
                    if (System.DateTime.Now.Subtract(this._lastVisibleChange) > System.TimeSpan.FromMilliseconds(hideTimeMilliseconds))
                    {
                        this._visibleState = VisibleState.NotVisible;
                        this._lastVisibleChange = System.DateTime.Now;
                    }
                }
            }
            else
            {
                this._visibleState = VisibleState.Visible;
            }

            int totalNumberButtons = m_toolsMenuButtons.Count + m_layersMenuButtons.Count;
            MenuButton.NormalSize = MenuButton.SelectedSize / 2;
            _outerPadding = MenuButton.NormalSize * padRatio;

            float menuWidth = (MenuButton.NormalSize + _outerPadding) * totalNumberButtons + _outerPadding;
            if (menuWidth > drawArgs.ScreenWidth)
            {
                MenuButton.NormalSize = (drawArgs.ScreenWidth) / ((padRatio + 1) * totalNumberButtons + padRatio);
                _outerPadding = MenuButton.NormalSize * padRatio;

                // recalc menuWidth if we want to center the toolbar
                menuWidth = (MenuButton.NormalSize + _outerPadding) * totalNumberButtons + _outerPadding;
            }

            if (m_anchor == MenuAnchor.Left)
            {
                x = 0;
                y = (int)MenuButton.NormalSize;
            }
            else if (m_anchor == MenuAnchor.Right)
            {
                x = (int)(drawArgs.ScreenWidth - 2 * _outerPadding - MenuButton.NormalSize);
                y = (int)MenuButton.NormalSize;
            }
            else if (m_anchor == MenuAnchor.Top)
            {
                x = (int)(drawArgs.ScreenWidth / 2 - totalNumberButtons * MenuButton.NormalSize / 2 - _outerPadding);
                y = 0;
            }
            else if (m_anchor == MenuAnchor.Bottom)
            {
                x = (int)(drawArgs.ScreenWidth / 2 - totalNumberButtons * MenuButton.NormalSize / 2 - _outerPadding);
                y = (int)(drawArgs.ScreenHeight - 2 * _outerPadding - MenuButton.NormalSize);
            }

            if (this._visibleState == VisibleState.Ascending)
            {
                TimeSpan t = System.DateTime.Now.Subtract(this._lastVisibleChange);
                if (t.Milliseconds < hideTimeMilliseconds)
                {
                    double percent = (double)t.Milliseconds / hideTimeMilliseconds;
                    int dx = (int)((MenuButton.NormalSize + 5) - (percent * (MenuButton.NormalSize + 5)));

                    if (m_anchor == MenuAnchor.Left)
                    {
                        x -= dx;
                    }
                    else if (m_anchor == MenuAnchor.Right)
                    {
                        x += dx;
                    }
                    else if (m_anchor == MenuAnchor.Top)
                    {
                        y -= dx;

                    }
                    else if (m_anchor == MenuAnchor.Bottom)
                    {
                        y += dx;
                    }
                }
            }
            else if (this._visibleState == VisibleState.Descending)
            {
                TimeSpan t = System.DateTime.Now.Subtract(this._lastVisibleChange);
                if (t.Milliseconds < hideTimeMilliseconds)
                {
                    double percent = (double)t.Milliseconds / hideTimeMilliseconds;
                    int dx = (int)((percent * (MenuButton.NormalSize + 5)));

                    if (m_anchor == MenuAnchor.Left)
                    {
                        x -= dx;
                    }
                    else if (m_anchor == MenuAnchor.Right)
                    {
                        x += dx;
                    }
                    else if (m_anchor == MenuAnchor.Top)
                    {
                        y -= dx;
                    }
                    else if (m_anchor == MenuAnchor.Bottom)
                    {
                        y += dx;
                    }
                }
            }

            lock (m_toolsMenuButtons.SyncRoot)
            {
                MenuButton selectedButton = null;
                if (_curSelection >= 0 & _curSelection < totalNumberButtons)
                {
                    if (_curSelection < m_toolsMenuButtons.Count)
                        selectedButton = (MenuButton)m_toolsMenuButtons[_curSelection];
                    else
                        selectedButton = (MenuButton)m_layersMenuButtons[_curSelection - m_toolsMenuButtons.Count];
                }

                //_outerPadding = MenuButton.NormalSize*padRatio;
                //float menuWidth = (MenuButton.NormalSize+_outerPadding)*totalNumberButtons+_outerPadding;
                //if(menuWidth>drawArgs.screenWidth)
                //{
                //    //MessageBox.Show(drawArgs.screenWidth.ToString());
                //    MenuButton.NormalSize = (drawArgs.screenWidth)/((padRatio+1)*totalNumberButtons+padRatio);
                //    //MessageBox.Show(MenuButton.NormalSize.ToString());
                //    _outerPadding = MenuButton.NormalSize*padRatio;
                //}

                if (this._visibleState != VisibleState.NotVisible)
                {
                    if (m_anchor == MenuAnchor.Top)
                    {
                        MenuUtils.DrawBox(0, 0, drawArgs.ScreenWidth, (int)(MenuButton.NormalSize + 2 * _outerPadding), 0.0f,
                            World.Settings.toolBarBackColor, drawArgs.device);
                    }
                    else if (m_anchor == MenuAnchor.Bottom)
                    {
                        MenuUtils.DrawBox(0, (int)(y - _outerPadding), drawArgs.ScreenWidth, (int)(MenuButton.NormalSize + 4 * _outerPadding), 0.0f,
                            World.Settings.toolBarBackColor, drawArgs.device);
                    }
                }

                float total = 0;
                float extra = 0;
                for (int i = 0; i < totalNumberButtons; i++)
                {
                    MenuButton button;
                    if (i < m_toolsMenuButtons.Count)
                        button = (MenuButton)m_toolsMenuButtons[i];
                    else
                        button = (MenuButton)m_layersMenuButtons[i - m_toolsMenuButtons.Count];
                    total += button.CurrentSize;
                    extra += button.CurrentSize - MenuButton.NormalSize;
                }

                float pad = ((float)_outerPadding * (totalNumberButtons + 1) - extra) / (totalNumberButtons + 1);
                float buttonX = pad;

                // TODO - to center the menubar set the buttonX to center-half toolbar width
                // float buttonX = (drawArgs.screenWidth - menuWidth) / 2; 

                m_sprite.Begin(SpriteFlags.AlphaBlend);
                for (int i = 0; i < totalNumberButtons; i++)
                {
                    MenuButton button;
                    if (i < m_toolsMenuButtons.Count)
                        button = (MenuButton)m_toolsMenuButtons[i];
                    else
                        button = (MenuButton)m_layersMenuButtons[i - m_toolsMenuButtons.Count];

                    if (button.IconTexture == null)
                        button.InitializeTexture(drawArgs.device);

                    if (this._visibleState != VisibleState.NotVisible)
                    {
                        int centerX = (int)(buttonX + button.CurrentSize * 0.5f);
                        buttonX += button.CurrentSize + pad;
                        float buttonTopY = y + _outerPadding;

                        if (m_anchor == MenuAnchor.Bottom)
                            buttonTopY = (int)(drawArgs.ScreenHeight - _outerPadding - button.CurrentSize);

                        if (button.IsPushed())
                        {
                            // Draw the chevron
                            float chevronSize = button.CurrentSize * padRatio;

                            enabledChevron[0].Color = chevronColor;
                            enabledChevron[1].Color = chevronColor;
                            enabledChevron[2].Color = chevronColor;

                            if (m_anchor == MenuAnchor.Bottom)
                            {
                                enabledChevron[2].X = centerX - chevronSize;
                                enabledChevron[2].Y = y - 2;
                                enabledChevron[2].Z = 0.0f;

                                enabledChevron[0].X = centerX;
                                enabledChevron[0].Y = y - 2 + chevronSize;
                                enabledChevron[0].Z = 0.0f;

                                enabledChevron[1].X = centerX + chevronSize;
                                enabledChevron[1].Y = y - 2;
                                enabledChevron[1].Z = 0.0f;
                            }
                            else
                            {
                                enabledChevron[2].X = centerX - chevronSize;
                                enabledChevron[2].Y = y + 2;
                                enabledChevron[2].Z = 0.0f;

                                enabledChevron[0].X = centerX;
                                enabledChevron[0].Y = y + 2 + chevronSize;
                                enabledChevron[0].Z = 0.0f;

                                enabledChevron[1].X = centerX + chevronSize;
                                enabledChevron[1].Y = y + 2;
                                enabledChevron[1].Z = 0.0f;
                            }

                            drawArgs.device.VertexFormat = CustomVertex.TransformedColored.Format;
                            drawArgs.device.TextureState[0].ColorOperation = TextureOperation.Disable;
                            drawArgs.device.DrawUserPrimitives(PrimitiveType.TriangleList, 1, enabledChevron);
                            drawArgs.device.TextureState[0].ColorOperation = TextureOperation.SelectArg1;
                        }

                        button.RenderEnabledIcon(
                            m_sprite,
                            drawArgs,
                            centerX,
                            buttonTopY,
                            i == this._curSelection,
                            m_anchor);
                    }
                }
                m_sprite.End();

            }
        }

        public void Dispose()
        {
            foreach (MenuButton button in m_toolsMenuButtons)
                button.Dispose();

            if (m_sprite != null)
            {
                m_sprite.Dispose();
                m_sprite = null;
            }
        }

        #endregion

        /// <summary>
        /// 可见状态
        /// </summary>
        protected enum VisibleState
        {
            /// <summary>
            /// 不可见
            /// </summary>
            NotVisible,
            /// <summary>
            /// 降序排列
            /// </summary>
            Descending,
            /// <summary>
            /// 升序排列
            /// </summary>
            Ascending,
            /// <summary>
            /// 可见
            /// </summary>
            Visible
        }
    }
}
