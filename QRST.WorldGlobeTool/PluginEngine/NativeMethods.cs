using System;
using System.Runtime.InteropServices;
using System.Drawing;

namespace QRST.WorldGlobeTool.PluginEngine
{
    /// <summary>
    /// Interop functionality for Plugin namespace.
    /// 插件命名空间的互操作功能
    /// </summary>
    internal sealed class NativeMethods
    {
        private NativeMethods()
        {
        }

        [DllImport("User32.dll", CharSet = CharSet.Auto)]
        public static extern long SetWindowLong(IntPtr hwnd, int nIndex, long dwNewLong);

        [StructLayout(LayoutKind.Sequential)]
        public struct RECT
        {
            public int left;
            public int top;
            public int right;
            public int bottom;
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        public struct DRAWITEMSTRUCT
        {
            public int ctrlType;
            public int ctrlID;
            public int itemID;
            public int itemAction;
            public int itemState;
            public IntPtr hwnd;
            public IntPtr hdc;
            public RECT rcItem;
            public IntPtr itemData;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct LVHITTESTINFO
        {
            public Point pt;
            public int flags;
            public int iItem;
            public int iSubItem;
        }
    }
}
