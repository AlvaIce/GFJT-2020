using System;
using System.Runtime.InteropServices;
using System.IO;
using System.Windows.Forms;
using System.Drawing;
using System.Collections;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;
using QRST.WorldGlobeTool.Renderable;
using QRST.WorldGlobeTool.Utility;

namespace QRST.WorldGlobeTool.Menu
{

    /// <summary>
    /// 图层管理按钮
    /// </summary>
    public class LayerManagerMenu : SideBarMenu
    {

        #region 字段

        public int DialogColor = System.Drawing.Color.Gray.ToArgb();
        public int TextColor = System.Drawing.Color.White.ToArgb();
        public LayerMenuItem MouseOverItem;
        public int ScrollBarSize = 20;
        public int ItemHeight = 20;
        World _parentWorld;
        MenuButton _parentButton;
        bool showScrollbar;
        int scrollBarPosition;
        float scrollSmoothPosition; // Current position of scroll when smooth scrolling (scrollBarPosition=target)
        int scrollGrabPositionY; // Location mouse grabbed scroll
        bool isResizing;
        bool isScrolling;
        int leftBorder = 2;
        int rightBorder = 1;
        int topBorder = 25;
        int bottomBorder = 1;
        Microsoft.DirectX.Direct3D.Font headerFont;
        Microsoft.DirectX.Direct3D.Font itemFont;
        Microsoft.DirectX.Direct3D.Font wingdingsFont;
        Microsoft.DirectX.Direct3D.Font worldwinddingsFont;
        ArrayList _itemList = new ArrayList();
        Microsoft.DirectX.Vector2[] scrollbarLine = new Vector2[2];
        public ContextMenu ContextMenu;

        DrawArgs m_DrawArgs = null;

        #endregion

        #region 属性

        /// <summary>
        /// Client area X position of left side
        /// </summary>
        public int ClientLeft
        {
            get
            {
                return Left + leftBorder;
            }
        }

        /// <summary>
        /// Client area X position of right side
        /// </summary>
        public int ClientRight
        {
            get
            {
                int res = Right - rightBorder;
                if (showScrollbar)
                    res -= ScrollBarSize;
                return res;
            }
        }

        /// <summary>
        /// Client area Y position of top side
        /// </summary>
        public int ClientTop
        {
            get
            {
                return Top + topBorder + 1;
            }
        }

        /// <summary>
        /// Client area Y position of bottom side
        /// </summary>
        public int ClientBottom
        {
            get
            {
                return Bottom - bottomBorder;
            }
        }

        /// <summary>
        /// Client area width
        /// </summary>
        public int ClientWidth
        {
            get
            {
                int res = Right - rightBorder - Left - leftBorder;
                if (showScrollbar)
                    res -= ScrollBarSize;
                return res;
            }
        }

        /// <summary>
        /// Client area height
        /// </summary>
        public int ClientHeight
        {
            get
            {
                int res = Bottom - bottomBorder - Top - topBorder - 1;
                return res;
            }
        }

        #endregion

        #region 构造函数

        /// <summary>
        /// Initializes a new instance of the <see cref= "T:WorldWind.Menu.LayerManagerMenu"/> class.
        /// </summary>
        /// <param name="parentWorld"></param>
        /// <param name="parentButton"></param>
        public LayerManagerMenu(World parentWorld, MenuButton parentButton)
        {
            this._parentWorld = parentWorld;
            this._parentButton = parentButton;
        }

        #endregion

        #region 接口成员

        public override void OnKeyDown(KeyEventArgs keyEvent)
        {
        }

        public override void OnKeyUp(KeyEventArgs keyEvent)
        {
        }

        public override bool OnMouseWheel(MouseEventArgs e)
        {
            if (e.X > this.Right || e.X < this.Left || e.Y < this.Top || e.Y > this.Bottom)
                // Outside
                return false;

            // Mouse wheel scroll
            this.scrollBarPosition -= (e.Delta / 6);
            return true;
        }

        public override bool OnMouseDown(MouseEventArgs e)
        {
            if (e.X > Right || e.X < Left || e.Y < Top || e.Y > Bottom)
                // Outside
                return false;

            if ((Anchor == SideBarMenuAnchor.Left && e.X > this.Right - 5 && e.X < this.Right + 5)
                || (Anchor == SideBarMenuAnchor.Right && e.X > this.Left - 5 && e.X < this.Left + 5))
            {
                this.isResizing = true;
                return true;
            }

            if (e.Y < ClientTop)
                return false;

            if (e.X > this.Right - ScrollBarSize)
            {
                int numItems = GetNumberOfUncollapsedItems();
                int totalHeight = GetItemsHeight(m_DrawArgs);
                if (totalHeight > ClientHeight)
                {
                    //int totalHeight = numItems * ItemHeight;
                    double percentHeight = (double)ClientHeight / totalHeight;
                    if (percentHeight > 1)
                        percentHeight = 1;

                    double scrollItemHeight = (double)percentHeight * ClientHeight;
                    int scrollPosition = ClientTop + (int)(scrollBarPosition * percentHeight);
                    if (e.Y < scrollPosition)
                        scrollBarPosition -= ClientHeight;
                    else if (e.Y > scrollPosition + scrollItemHeight)
                        scrollBarPosition += ClientHeight;
                    else
                    {
                        scrollGrabPositionY = e.Y - scrollPosition;
                        isScrolling = true;
                    }
                }
            }

            return true;
        }

        public override bool OnMouseMove(MouseEventArgs e)
        {
            // Reset mouse over effect since mouse moved.
            MouseOverItem = null;

            if (this.isResizing)
            {
                if (Anchor == SideBarMenuAnchor.Left)
                {
                    if (e.X > 100 && e.X < m_DrawArgs.ScreenWidth)
                        this.Width = e.X - this.Left;
                }
                else
                {
                    if (e.X < m_DrawArgs.ScreenWidth - 100)
                        this.Width = this.Right - e.X;
                }

                DrawArgs.MouseCursor = CursorType.SizeWE;
                return true;
            }

            if (this.isScrolling)
            {
                int totalHeight = GetItemsHeight(m_DrawArgs);//GetNumberOfUncollapsedItems() * ItemHeight;
                double percent = (double)totalHeight / ClientHeight;
                scrollBarPosition = (int)((e.Y - scrollGrabPositionY - ClientTop) * percent);
                return true;
            }

            if (e.X > this.Right || e.X < this.Left || e.Y < this.Top || e.Y > this.Bottom)
                // Outside
                return false;

            if ((Anchor == SideBarMenuAnchor.Left && Math.Abs(e.X - this.Right) < 5)
                || (Anchor == SideBarMenuAnchor.Right && Math.Abs(e.X - this.Left) < 5))
            {
                DrawArgs.MouseCursor = CursorType.SizeWE;
                return true;
            }

            if (e.X > ClientRight)
                return true;

            foreach (LayerMenuItem lmi in this._itemList)
                if (lmi.OnMouseMove(e))
                    return true;

            // Handled
            return true;
        }

        public override bool OnMouseUp(MouseEventArgs e)
        {
            if (this.isResizing)
            {
                this.isResizing = false;
                return true;
            }

            if (this.isScrolling)
            {
                this.isScrolling = false;
                return true;
            }

            foreach (LayerMenuItem lmi in this._itemList)
            {
                if (lmi.OnMouseUp(e))
                    return true;
            }

            if (e.X > this.Right - 20 && e.X < this.Right &&
                e.Y > this.Top && e.Y < this.Top + topBorder)
            {
                this._parentButton.SetPushed(false);
                return true;
            }
            //else if (e.X > 0 && e.X < this.Right && e.Y > 0 && e.Y < this.Bottom)
            //    return true;
            else
                return false;
        }

        public override void Dispose()
        {
        }

        #endregion

        #region 公共方法

        /// <summary>
        /// 显示右键菜单
        /// </summary>
        /// <param name="x">右键菜单起始X位置</param>
        /// <param name="y">右键菜单起始Y位置</param>
        /// <param name="item">图层菜单项</param>
        public void ShowContextMenu(int x, int y, LayerMenuItem item)
        {
            if (ContextMenu != null)
            {
                ContextMenu.Dispose();
                ContextMenu = null;
            }
            ContextMenu = new ContextMenu();
            item.RenderableObject.BuildContextMenu(ContextMenu);
            ContextMenu.Show(item.ParentControl, new System.Drawing.Point(x, y));
        }

        /// <summary>
        /// 计算树中没有折叠的选项数量
        /// </summary>
        /// <returns>返回树中没有折叠的选项数量</returns>
        public int GetNumberOfUncollapsedItems()
        {
            int numItems = 1;
            foreach (LayerMenuItem subItem in _itemList)
                numItems += subItem.GetNumberOfUncollapsedItems();

            return numItems;
        }

        /// <summary>
        /// 获取菜单项的高度
        /// </summary>
        /// <param name="drawArgs">绘制参数</param>
        /// <returns>返回菜单项的高度</returns>
        public int GetItemsHeight(DrawArgs drawArgs)
        {
            int height = 20;
            foreach (LayerMenuItem subItem in _itemList)
                height += subItem.GetItemsHeight(drawArgs);

            return height;
        }

        /// <summary>
        /// 渲染菜单内容
        /// </summary>
        /// <param name="drawArgs">绘制参数</param>
        public override void RenderContents(DrawArgs drawArgs)
        {
            m_DrawArgs = drawArgs;
            try
            {
                if (itemFont == null)
                {
                    itemFont = drawArgs.CreateFont(World.Settings.LayerManagerFontName,
                        World.Settings.LayerManagerFontSize, World.Settings.LayerManagerFontStyle);

                    // TODO: Fix wingdings menu problems
                    System.Drawing.Font localHeaderFont = new System.Drawing.Font("Arial", 12.0f, FontStyle.Bold);
                    headerFont = new Microsoft.DirectX.Direct3D.Font(drawArgs.device, localHeaderFont);

                    System.Drawing.Font wingdings = new System.Drawing.Font("Wingdings", 12.0f);
                    wingdingsFont = new Microsoft.DirectX.Direct3D.Font(drawArgs.device, wingdings);

                    string fontFile = Path.Combine(World.Settings.DataDirectory, "Earth\\World Wind Dings 1.04.ttf");
                    AddFontResource(fontFile);
                    System.Drawing.Text.PrivateFontCollection fpc = new System.Drawing.Text.PrivateFontCollection();
                    fpc.AddFontFile(fontFile);
                    System.Drawing.Font worldwinddings = new System.Drawing.Font(fpc.Families[0], 12.0f);
                    worldwinddingsFont = new Microsoft.DirectX.Direct3D.Font(drawArgs.device, worldwinddings);
                }

                this.updateList();

                this.worldwinddingsFont.DrawText(
                    null,
                    "E",
                    new System.Drawing.Rectangle(this.Right - 16, this.Top + 2, 20, topBorder),
                    DrawTextFormat.None,
                    TextColor);

                int numItems = GetNumberOfUncollapsedItems();
                int totalHeight = GetItemsHeight(drawArgs);//numItems * ItemHeight;
                showScrollbar = totalHeight > ClientHeight;
                if (showScrollbar)
                {
                    double percentHeight = (double)ClientHeight / totalHeight;
                    int scrollbarHeight = (int)(ClientHeight * percentHeight);

                    int maxScroll = totalHeight - ClientHeight;

                    if (scrollBarPosition < 0)
                        scrollBarPosition = 0;
                    else if (scrollBarPosition > maxScroll)
                        scrollBarPosition = maxScroll;

                    // Smooth scroll
                    const float scrollSpeed = 0.3f;
                    float smoothScrollDelta = (scrollBarPosition - scrollSmoothPosition) * scrollSpeed;
                    float absDelta = Math.Abs(smoothScrollDelta);
                    if (absDelta > 100f || absDelta < 3f)
                        // Scroll > 100 pixels and < 1.5 pixels faster
                        smoothScrollDelta = (scrollBarPosition - scrollSmoothPosition) * (float)Math.Sqrt(scrollSpeed);

                    scrollSmoothPosition += smoothScrollDelta;

                    if (scrollSmoothPosition > maxScroll)
                        scrollSmoothPosition = maxScroll;

                    int scrollPos = (int)((float)percentHeight * scrollBarPosition);

                    int color = isScrolling ? World.Settings.scrollbarHotColor : World.Settings.scrollbarColor;
                    MenuUtils.DrawBox(
                        Right - ScrollBarSize + 2,
                        ClientTop + scrollPos,
                        ScrollBarSize - 3,
                        scrollbarHeight + 1,
                        0.0f,
                        color,
                        drawArgs.device);

                    scrollbarLine[0].X = this.Right - ScrollBarSize;
                    scrollbarLine[0].Y = this.ClientTop;
                    scrollbarLine[1].X = this.Right - ScrollBarSize;
                    scrollbarLine[1].Y = this.Bottom;
                    MenuUtils.DrawLine(scrollbarLine,
                        DialogColor,
                        drawArgs.device);
                }

                this.headerFont.DrawText(
                    null, "图层管理",
                    new System.Drawing.Rectangle(Left + 5, Top + 1, Width, topBorder - 2),
                    DrawTextFormat.VerticalCenter, TextColor);

                Microsoft.DirectX.Vector2[] headerLinePoints = new Microsoft.DirectX.Vector2[2];
                headerLinePoints[0].X = this.Left;
                headerLinePoints[0].Y = this.Top + topBorder - 1;

                headerLinePoints[1].X = this.Right;
                headerLinePoints[1].Y = this.Top + topBorder - 1;

                MenuUtils.DrawLine(headerLinePoints, DialogColor, drawArgs.device);

                int runningItemHeight = 0;
                if (showScrollbar)
                    runningItemHeight = -(int)Math.Round(scrollSmoothPosition);

                // Set the Direct3D viewport to match the layer manager client area
                // to clip the text to the window when scrolling
                Viewport lmClientAreaViewPort = new Viewport();
                lmClientAreaViewPort.X = ClientLeft;
                lmClientAreaViewPort.Y = ClientTop;
                lmClientAreaViewPort.Width = ClientWidth;
                lmClientAreaViewPort.Height = ClientHeight;
                Viewport defaultViewPort = drawArgs.device.Viewport;
                drawArgs.device.Viewport = lmClientAreaViewPort;
                for (int i = 0; i < _itemList.Count; i++)
                {
                    if (runningItemHeight > ClientHeight)
                        // No more space for items
                        break;
                    LayerMenuItem lmi = (LayerMenuItem)_itemList[i];
                    runningItemHeight += lmi.Render(
                        drawArgs,
                        ClientLeft,
                        ClientTop,
                        runningItemHeight,
                        ClientWidth,
                        ClientBottom,
                        itemFont,
                        wingdingsFont,
                        worldwinddingsFont,
                        MouseOverItem);
                }
                drawArgs.device.Viewport = defaultViewPort;
            }
            catch (Exception caught)
            {
                MessageBox.Show(caught.ToString());
                Log.Write(caught);
            }
        }

        #endregion

        #region 私有方法

        /// <summary>
        /// 更新列表
        /// </summary>
        private void updateList()
        {
            if (this._parentWorld != null && this._parentWorld.RenderableObjects != null)
            {
                for (int i = 0; i < this._parentWorld.RenderableObjects.ChildObjects.Count; i++)
                {
                    RenderableObject curObject = (RenderableObject)this._parentWorld.RenderableObjects.ChildObjects[i];

                    if (i >= this._itemList.Count)
                    {
                        LayerMenuItem newItem = new LayerMenuItem(this, curObject);
                        this._itemList.Add(newItem);
                    }
                    else
                    {
                        LayerMenuItem curItem = (LayerMenuItem)this._itemList[i];
                        if (!curItem.RenderableObject.Name.Equals(curObject.Name))
                        {
                            this._itemList.Insert(i, new LayerMenuItem(this, curObject));
                        }
                    }
                }

                int extraItems = this._itemList.Count - this._parentWorld.RenderableObjects.ChildObjects.Count;
                this._itemList.RemoveRange(this._parentWorld.RenderableObjects.ChildObjects.Count, extraItems);
            }
            else
            {
                this._itemList.Clear();
            }
        }

        #endregion

        #region DLL引用

        [DllImport("gdi32.dll")]
        static extern int AddFontResource(string lpszFilename);

        #endregion
    }

}
