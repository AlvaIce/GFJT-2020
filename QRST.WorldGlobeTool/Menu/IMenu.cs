using System.Windows.Forms;

namespace QRST.WorldGlobeTool.Menu
{
    /// <summary>
    /// 菜单接口
    /// </summary>
    public interface IMenu
    {
        void OnKeyUp(KeyEventArgs keyEvent);
        void OnKeyDown(KeyEventArgs keyEvent);
        bool OnMouseUp(MouseEventArgs e);
        bool OnMouseDown(MouseEventArgs e);
        bool OnMouseMove(MouseEventArgs e);
        bool OnMouseWheel(MouseEventArgs e);
        void Render(DrawArgs drawArgs);
        void Dispose();
    }
}
