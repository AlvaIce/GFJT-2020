using System.Windows.Forms;

namespace QRST.WorldGlobeTool.Menu
{
    /// <summary>
    /// 菜单集合
    /// </summary>
    public class MenuCollection : IMenu
    {

        /// <summary>
        /// 菜单集合
        /// </summary>
        System.Collections.ArrayList _menus = new System.Collections.ArrayList();

        #region IMenu接口成员

        public void OnKeyUp(KeyEventArgs keyEvent)
        {
            foreach (IMenu m in this._menus)
                m.OnKeyUp(keyEvent);
        }

        public void OnKeyDown(KeyEventArgs keyEvent)
        {
            foreach (IMenu m in this._menus)
                m.OnKeyDown(keyEvent);
        }

        public bool OnMouseUp(MouseEventArgs e)
        {
            foreach (IMenu m in this._menus)
            {
                if (m.OnMouseUp(e))
                    return true;
            }
            return false;
        }

        public bool OnMouseDown(MouseEventArgs e)
        {
            foreach (IMenu m in this._menus)
            {
                if (m.OnMouseDown(e))
                    return true;
            }
            return false;
        }

        public bool OnMouseMove(MouseEventArgs e)
        {
            foreach (IMenu m in this._menus)
            {
                if (m.OnMouseMove(e))
                    return true;
            }
            return false;
        }

        public bool OnMouseWheel(MouseEventArgs e)
        {
            foreach (IMenu m in this._menus)
            {
                if (m.OnMouseWheel(e))
                    return true;
            }
            return false;
        }

        public void Render(DrawArgs drawArgs)
        {
            foreach (IMenu m in this._menus)
                m.Render(drawArgs);
        }

        public void Dispose()
        {
            foreach (IMenu m in this._menus)
                m.Dispose();
        }

        #endregion

        /// <summary>
        /// 向当前菜单集合中添加菜单
        /// </summary>
        /// <param name="menu">菜单项</param>
        public void AddMenu(IMenu menu)
        {
            lock (this._menus.SyncRoot)
            {
                this._menus.Add(menu);
            }
        }

        /// <summary>
        /// 从当前菜单集合中移除菜单
        /// </summary>
        /// <param name="menu">菜单项</param>
        public void RemoveMenu(IMenu menu)
        {
            lock (this._menus.SyncRoot)
            {
                this._menus.Remove(menu);
            }
        }
    }

}
