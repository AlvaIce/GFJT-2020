using System;
using System.Windows.Forms;
using System.Security.Permissions;
using System.Runtime.InteropServices;
using System.Drawing;

namespace QRST.WorldGlobeTool.PluginEngine
{
    /// <summary>
    /// 插件窗口中的插件列表项
    /// </summary>
    public partial class PluginListView : ListView
    {

        /// <summary>
        /// 图像列表
        /// </summary>
        ImageList imageList;

        /// <summary>
        /// 初始化一个PluginListView实例
        /// </summary>
        public PluginListView()
        {
            this.View = View.Details;
            this.ResizeRedraw = true;
        }

        protected override CreateParams CreateParams
        {
            get
            {
                const int LVS_OWNERDRAWFIXED = 0x0400;
                CreateParams cp = base.CreateParams;
                cp.Style |= LVS_OWNERDRAWFIXED;
                return cp;
            }
        }


        [SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true), SecurityPermission(SecurityAction.InheritanceDemand, UnmanagedCode = true)]
        protected override void WndProc(ref Message m)
        {
            const int WM_DRAWITEM = 0x002B;
            const int WM_REFLECT = 0x2000;
            const int ODS_SELECTED = 0x0001;

            switch (m.Msg)
            {
                case WM_REFLECT | WM_DRAWITEM:
                    {
                        //从消息参数中获取绘制选项结构体
                        NativeMethods.DRAWITEMSTRUCT dis = (NativeMethods.DRAWITEMSTRUCT)Marshal.PtrToStructure(
                            m.LParam, typeof(NativeMethods.DRAWITEMSTRUCT));
                        // 根据RECT结构体创建一个矩形
                        Rectangle r = new Rectangle(dis.rcItem.left, dis.rcItem.top,
                            dis.rcItem.right - dis.rcItem.left, dis.rcItem.bottom - dis.rcItem.top);

                        //从绘制选项结构体的hdc字段中获取画笔
                        using (Graphics g = Graphics.FromHdc(dis.hdc))
                        {	
			                //采用默认状态创建一个新的绘制选项状态
                            DrawItemState d = DrawItemState.Default;
                            //设置绘制的正确状态
                            if ((dis.itemState & ODS_SELECTED) > 0)
                                d = DrawItemState.Selected;
                            //创建绘制选项事件参数对象
                            PluginListItem item = (PluginListItem)Items[dis.itemID];
                            DrawItemEventArgs e = new DrawItemEventArgs(g, this.Font, r, dis.itemID, d);
                            OnDrawItem(e, item);
                            //处理消息
                            m.Result = (IntPtr)1;
                        }
                        break;
                    }

                default:
                    base.WndProc(ref m);
                    break;
            }
        }

        /// <summary>
        /// 重载鼠标弹起事件
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseUp(MouseEventArgs e)
        {
            //探测哪一列被单击并快速处理
            const int LVM_FIRST = 0x1000;
            const int LVM_SUBITEMHITTEST = LVM_FIRST + 57;

            NativeMethods.LVHITTESTINFO hitInfo = new NativeMethods.LVHITTESTINFO();
            hitInfo.pt = new Point(e.X, e.Y);
            IntPtr pointer = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(NativeMethods.LVHITTESTINFO)));
            Marshal.StructureToPtr(hitInfo, pointer, true);
            Message message = Message.Create(Handle, LVM_SUBITEMHITTEST, IntPtr.Zero, pointer);
            DefWndProc(ref message);
            hitInfo = (NativeMethods.LVHITTESTINFO)Marshal.PtrToStructure(
                pointer, typeof(NativeMethods.LVHITTESTINFO));
            Marshal.FreeHGlobal(pointer);

            if (hitInfo.iItem >= 0 && hitInfo.iSubItem == 1)
            {
                //翻转加载启动标志
                PluginListItem item = (PluginListItem)Items[hitInfo.iItem];
                item.PluginInfo.IsLoadedAtStartup = !item.PluginInfo.IsLoadedAtStartup;
                Invalidate();
                return;
            }

            base.OnMouseUp(e);
        }

        /// <summary>
        /// 绘制列表项
        /// </summary>
        protected void OnDrawItem(DrawItemEventArgs e, PluginListItem item)
        {
            e.DrawBackground();

            //正在运行状态的位图
            const int imageWidth = 16 + 3;
            if (imageList == null)
            {
                if (this.Parent is PluginDialog)
                {
                    imageList = ((PluginDialog)Parent).ImageList;
                }
                else if (this.Parent.Parent is PluginDialog)
                {
                    imageList = ((PluginDialog)Parent.Parent).ImageList;
                }
                else
                {
                    return;
                }
            }
            if (imageList != null)
            {
                int imageIndex = item.PluginInfo.IsCurrentlyLoaded ? 0 : 1;
                imageList.Draw(e.Graphics, e.Bounds.Left + 2, e.Bounds.Top + 1, imageIndex);
            }

            //名称
            Rectangle bounds = Rectangle.FromLTRB(e.Bounds.Left + imageWidth,
                e.Bounds.Top, e.Bounds.Left + Columns[0].Width, e.Bounds.Bottom);
            using (Brush brush = new SolidBrush(e.ForeColor))
                e.Graphics.DrawString(item.Name, e.Font, brush, bounds);

            //复选框
            bounds = Rectangle.FromLTRB(bounds.Right + 1,
                bounds.Top, bounds.Right + Columns[1].Width + 1, bounds.Bottom - 1);
            ButtonState state = item.PluginInfo.IsLoadedAtStartup ? ButtonState.Checked : ButtonState.Normal;
            ControlPaint.DrawCheckBox(e.Graphics, bounds, state);
        }

    }
}
