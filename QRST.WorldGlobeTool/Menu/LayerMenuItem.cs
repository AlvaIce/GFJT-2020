using QRST.WorldGlobeTool.Renderable;
using System.Collections;
using System.Drawing;
using System.Windows.Forms;
using Microsoft.DirectX.Direct3D;
using System.Diagnostics;
using Microsoft.DirectX;

namespace QRST.WorldGlobeTool.Menu
{
    /// <summary>
    /// 图层菜单项
    /// </summary>
    public class LayerMenuItem
    {

        #region 字段

        /// <summary>
        /// 与当前菜单项相关联到可渲染对象
        /// </summary>
        RenderableObject m_renderableObject;
        /// <summary>
        /// 子菜单项列表
        /// </summary>
        ArrayList m_subItems = new ArrayList();
        /// <summary>
        /// 菜单项X位置
        /// </summary>
        private int _x;
        /// <summary>
        /// 菜单项Y位置
        /// </summary>
        private int _y;
        /// <summary>
        /// 菜单项的宽度
        /// </summary>
        private int _width;
        /// <summary>
        /// 子菜单项到X方向偏移
        /// </summary>
        private int _itemXOffset = 5;
        /// <summary>
        /// 展开箭头X方向尺寸
        /// </summary>
        private int _expandArrowXSize = 15;
        /// <summary>
        /// 复选框X方向偏移量
        /// </summary>
        private int _checkBoxXOffset = 15;
        /// <summary>
        /// 子菜单项X方向缩进
        /// </summary>
        private int _subItemXIndent = 15;
        /// <summary>
        /// 子菜单项开启时颜色
        /// </summary>
        int itemOnColor = Color.White.ToArgb();
        /// <summary>
        /// 子菜单项关闭时颜色
        /// </summary>
        int itemOffColor = Color.Gray.ToArgb();
        /// <summary>
        /// 菜单项是否展开
        /// </summary>
        private bool isExpanded;
        /// <summary>
        /// 父控件
        /// </summary>
        public Control ParentControl;
        /// <summary>
        /// 当前菜单所属到图层菜单管理器
        /// </summary>
        LayerManagerMenu m_parent;
        /// <summary>
        /// 上一次消耗的高度
        /// </summary>
        int lastConsumedHeight = 20;

        #endregion

        #region 属性

        /// <summary>
        /// 获取与当前菜单项相关联的可渲染对象
        /// </summary>
        public RenderableObject RenderableObject
        {
            get
            {
                return m_renderableObject;
            }
        } 

        #endregion

        #region 构造函数

        /// <summary>
        /// 初始化一个LayerMenuItem2实例
        /// </summary>
        /// <param name="parent">当前菜单项所属到父图层菜单管理对象</param>
        /// <param name="renderableObject">与当前菜单项相关联到可渲染对象</param>
        public LayerMenuItem(LayerManagerMenu parent, RenderableObject renderableObject)
        {
            m_renderableObject = renderableObject;
            m_parent = parent;
        }

        #endregion

        #region 公共方法

        /// <summary>
        /// Calculate the number of un-collapsed items in the tree.
        /// 计算树中未折叠到菜单项数量
        /// </summary>
        /// <returns>返回树中未折叠到菜单项数量</returns>
        public int GetNumberOfUncollapsedItems()
        {
            int numItems = 1;
            if (this.isExpanded)
            {
                foreach (LayerMenuItem subItem in m_subItems)
                    numItems += subItem.GetNumberOfUncollapsedItems();
            }

            return numItems;
        }

        /// <summary>
        /// 获取菜单项高度
        /// </summary>
        /// <param name="drawArgs">绘制参数</param>
        /// <returns>返回当前菜单项高度</returns>
        public int GetItemsHeight(DrawArgs drawArgs)
        {
            System.Drawing.Rectangle rect = drawArgs.DefaultDrawingFont.MeasureString(
                null,
                this.m_renderableObject.Name, DrawTextFormat.None, System.Drawing.Color.White.ToArgb());

            int height = rect.Height;

            if (m_renderableObject.Description != null && m_renderableObject.Description.Length > 0)
            {
                System.Drawing.SizeF rectF = DrawArgs.Graphics.MeasureString(
                    m_renderableObject.Description,
                    drawArgs.DefaultSubTitleFont,
                    _width - (this._itemXOffset + this._expandArrowXSize + this._checkBoxXOffset)
                    );

                height += (int)rectF.Height + 15;
            }

            if (height < lastConsumedHeight)
                height = lastConsumedHeight;

            if (this.isExpanded)
            {
                foreach (LayerMenuItem subItem in m_subItems)
                    height += subItem.GetItemsHeight(drawArgs);
            }

            return height;
        }

        /// <summary>
        /// 探测鼠标是否移动到展开尖头上
        /// </summary>
        public bool OnMouseMove(MouseEventArgs e)
        {
            if (e.Y < this._y)
                // Over 
                return false;

            if (e.X < m_parent.Left || e.X > m_parent.Right)
                return false;

            if (e.Y < this._y + 20)
            {
                // Mouse is on item
                m_parent.MouseOverItem = this;

                if (e.X > this._x + this._itemXOffset &&
                    e.X < this._x + (this._itemXOffset + this._expandArrowXSize + this._checkBoxXOffset))
                {
                    if (m_renderableObject is RenderableObjectList)
                        DrawArgs.MouseCursor = CursorType.Hand;
                    return true;
                }
                return false;
            }

            foreach (LayerMenuItem lmi in m_subItems)
            {
                if (lmi.OnMouseMove(e))
                {
                    // Mouse is on current item
                    m_parent.MouseOverItem = lmi;
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// 鼠标按键弹起事件
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        public bool OnMouseUp(MouseEventArgs e)
        {
            if (e.Y < this._y)
                // Above 
                return false;

            if (e.Y <= this._y + 20)
            {
                if (e.X > this._x + this._itemXOffset &&
                    e.X < this._x + (this._itemXOffset + this._width) &&
                    e.Button == MouseButtons.Right)
                {
                    m_parent.ShowContextMenu(e.X, e.Y, this);
                }

                if (e.X > this._x + this._itemXOffset + this._expandArrowXSize + this._checkBoxXOffset &&
                    e.X < this._x + (this._itemXOffset + this._width) &&
                    e.Button == MouseButtons.Left &&
                    m_renderableObject != null &&
                    m_renderableObject.MetaData.Contains("InfoUri"))
                {
                    string infoUri = (string)m_renderableObject.MetaData["InfoUri"];

                    //if (World.Settings.UseInternalBrowser || infoUri.StartsWith(@"worldwind://"))
                    //{
                    //    SplitContainer sc = (SplitContainer)this.ParentControl.Parent.Parent;
                    //    InternalWebBrowserPanel browser = (InternalWebBrowserPanel)sc.Panel1.Controls[0];
                    //    browser.NavigateTo(infoUri);
                    //}
                    //else
                    //{
                    ProcessStartInfo psi = new ProcessStartInfo();
                    psi.FileName = infoUri;
                    psi.Verb = "open";
                    psi.UseShellExecute = true;
                    psi.CreateNoWindow = true;
                    Process.Start(psi);
                    //}
                }

                if (e.X > this._x + this._itemXOffset &&
                    e.X < this._x + (this._itemXOffset + this._expandArrowXSize) &&
                    m_renderableObject is RenderableObjectList)
                {
                    RenderableObjectList rol = (RenderableObjectList)m_renderableObject;
                    if (!rol.DisableExpansion)
                    {
                        this.isExpanded = !this.isExpanded;
                        return true;
                    }
                }

                if (e.X > this._x + this._itemXOffset + this._expandArrowXSize &&
                    e.X < this._x + (this._itemXOffset + this._expandArrowXSize + this._checkBoxXOffset))
                {
                    if (!m_renderableObject.IsOn && m_renderableObject.ParentList != null &&
                        m_renderableObject.ParentList.IsShowOnlyOneLayer)
                        m_renderableObject.ParentList.TurnOffAllChildren();

                    m_renderableObject.IsOn = !m_renderableObject.IsOn;
                    return true;
                }
            }

            if (isExpanded)
            {
                foreach (LayerMenuItem lmi in m_subItems)
                {
                    if (lmi.OnMouseUp(e))
                        return true;
                }
            }

            return false;
        }

        /// <summary>
        /// 渲染当前菜单项
        /// </summary>
        /// <param name="drawArgs">绘制参数</param>
        /// <param name="x">菜单项X位置</param>
        /// <param name="y">菜单项Y位置</param>
        /// <param name="yOffset">菜单项Y偏移</param>
        /// <param name="width">菜单项宽度</param>
        /// <param name="height">菜单项高度</param>
        /// <param name="drawingFont">绘制字体</param>
        /// <param name="wingdingsFont"></param>
        /// <param name="worldwinddingsFont"></param>
        /// <param name="mouseOverItem">鼠标覆盖的菜单项</param>
        /// <returns>返回本次渲染消耗的高度</returns>
        public int Render(DrawArgs drawArgs, int x, int y, int yOffset, int width, int height,
            Microsoft.DirectX.Direct3D.Font drawingFont,
            Microsoft.DirectX.Direct3D.Font wingdingsFont,
            Microsoft.DirectX.Direct3D.Font worldwinddingsFont,
            LayerMenuItem mouseOverItem)
        {
            if (ParentControl == null)
                ParentControl = drawArgs.parentControl;

            this._x = x;
            this._y = y + yOffset;
            this._width = width;

            int consumedHeight = 20;

            System.Drawing.Rectangle textRect = drawingFont.MeasureString(null,
                m_renderableObject.Name,
                DrawTextFormat.None,
                System.Drawing.Color.White.ToArgb());

            consumedHeight = textRect.Height;

            if (m_renderableObject.Description != null && m_renderableObject.Description.Length > 0 && !(m_renderableObject is QRST.WorldGlobeTool.Renderable.Icon))
            {
                System.Drawing.SizeF rectF = DrawArgs.Graphics.MeasureString(
                    m_renderableObject.Description,
                    drawArgs.DefaultSubTitleFont,
                    width - (this._itemXOffset + this._expandArrowXSize + this._checkBoxXOffset)
                    );

                consumedHeight += (int)rectF.Height + 15;
            }

            lastConsumedHeight = consumedHeight;
            // Layer manager client area height
            int totalHeight = height - y;

            updateList();

            if (yOffset >= -consumedHeight)
            {
                // Part of item or whole item visible
                int color = m_renderableObject.IsOn ? itemOnColor : itemOffColor;
                if (mouseOverItem == this)
                {
                    if (!m_renderableObject.IsOn)
                        // mouseover + inactive color (black)
                        color = 0xff << 24;
                    MenuUtils.DrawBox(m_parent.ClientLeft, _y, m_parent.ClientWidth, consumedHeight, 0,
                        World.Settings.menuOutlineColor, drawArgs.device);
                }

                if (m_renderableObject is RenderableObjectList)
                {
                    RenderableObjectList rol = (RenderableObjectList)m_renderableObject;
                    if (!rol.DisableExpansion)
                    {
                        worldwinddingsFont.DrawText(
                            null,
                            (this.isExpanded ? "L" : "A"),
                            new System.Drawing.Rectangle(x + this._itemXOffset, _y, this._expandArrowXSize, height),
                            DrawTextFormat.None,
                            color);
                    }
                }

                string checkSymbol = null;
                if (m_renderableObject.ParentList != null && m_renderableObject.ParentList.IsShowOnlyOneLayer)
                    // Radio check
                    checkSymbol = m_renderableObject.IsOn ? "O" : "P";
                else
                    // Normal check
                    checkSymbol = m_renderableObject.IsOn ? "N" : "F";

                worldwinddingsFont.DrawText(
                        null,
                        checkSymbol,
                        new System.Drawing.Rectangle(
                        x + this._itemXOffset + this._expandArrowXSize,
                        _y,
                        this._checkBoxXOffset,
                        height),
                        DrawTextFormat.NoClip,
                        color);


                drawingFont.DrawText(
                    null,
                    m_renderableObject.Name,
                    new System.Drawing.Rectangle(
                    x + this._itemXOffset + this._expandArrowXSize + this._checkBoxXOffset,
                    _y,
                    width - (this._itemXOffset + this._expandArrowXSize + this._checkBoxXOffset),
                    height),
                    DrawTextFormat.None,
                    color);

                if (m_renderableObject.Description != null && m_renderableObject.Description.Length > 0 && !(m_renderableObject is QRST.WorldGlobeTool.Renderable.Icon))
                {
                    drawArgs.DefaultSubTitleDrawingFont.DrawText(
                        null,
                        m_renderableObject.Description,
                        new System.Drawing.Rectangle(
                            x + this._itemXOffset + this._expandArrowXSize + this._checkBoxXOffset,
                            _y + textRect.Height,
                            width - (_itemXOffset + _expandArrowXSize + _checkBoxXOffset),
                            height),
                        DrawTextFormat.WordBreak,
                        System.Drawing.Color.Gray.ToArgb());
                }

                if (m_renderableObject.MetaData.Contains("InfoUri"))
                {
                    Vector2[] underlineVerts = new Vector2[2];
                    underlineVerts[0].X = x + this._itemXOffset + this._expandArrowXSize + this._checkBoxXOffset;
                    underlineVerts[0].Y = _y + textRect.Height;
                    underlineVerts[1].X = underlineVerts[0].X + textRect.Width;
                    underlineVerts[1].Y = _y + textRect.Height;

                    MenuUtils.DrawLine(underlineVerts, color, drawArgs.device);
                }
            }

            if (isExpanded)
            {
                for (int i = 0; i < m_subItems.Count; i++)
                {
                    int yRealOffset = yOffset + consumedHeight;
                    if (yRealOffset > totalHeight)
                        // No more space for items
                        break;
                    LayerMenuItem lmi = (LayerMenuItem)m_subItems[i];
                    consumedHeight += lmi.Render(
                        drawArgs,
                        x + _subItemXIndent,
                        y,
                        yRealOffset,
                        width - _subItemXIndent,
                        height,
                        drawingFont,
                        wingdingsFont,
                        worldwinddingsFont,
                        mouseOverItem);
                }
            }

            return consumedHeight;
        } 

        #endregion

        #region 私有方法

        /// <summary>
        /// 递归获取所有可渲染对象名称
        /// </summary>
        /// <param name="ro">可渲染对象</param>
        /// <param name="name">对象名称</param>
        /// <returns>返回所有可渲染对象名称</returns>
        private string getFullRenderableObjectName(RenderableObject ro, string name)
        {
            if (ro.ParentList == null)
                return "/" + name;
            else
            {
                if (name == null)
                    return getFullRenderableObjectName(ro.ParentList, ro.Name);
                else
                    return getFullRenderableObjectName(ro.ParentList, ro.Name + "/" + name);
            }
        }

        /// <summary>
        /// 更新列表
        /// </summary>
        private void updateList()
        {
            if (this.isExpanded)
            {
                RenderableObjectList rol = (RenderableObjectList)m_renderableObject;
                for (int i = 0; i < rol.ChildObjects.Count; i++)
                {
                    RenderableObject childObject = (RenderableObject)rol.ChildObjects[i];
                    if (i >= m_subItems.Count)
                    {
                        LayerMenuItem newItem = new LayerMenuItem(m_parent, childObject);
                        m_subItems.Add(newItem);
                    }
                    else
                    {
                        LayerMenuItem curItem = (LayerMenuItem)m_subItems[i];

                        if (curItem != null && curItem.RenderableObject != null &&
                            childObject != null &&
                            !curItem.RenderableObject.Name.Equals(childObject.Name))
                        {
                            m_subItems.Insert(i, new LayerMenuItem(m_parent, childObject));
                        }
                    }
                }

                int extraItems = m_subItems.Count - rol.ChildObjects.Count;
                if (extraItems > 0)
                    m_subItems.RemoveRange(rol.ChildObjects.Count, extraItems);
            }
        }

        #endregion
    }

}
