using System.Windows.Forms;

namespace QRST.WorldGlobeTool.PluginEngine
{
    /// <summary>
    /// 插件窗口中的列表项
    /// </summary>
    public class PluginListItem : ListViewItem
    {
        PluginInfo pluginInfo;

        /// <summary>
        /// 获取插件信息内容
        /// </summary>
        public PluginInfo PluginInfo
        {
            get
            {
                return pluginInfo;
            }
        }

        /// <summary>
        /// 获取插件名称
        /// </summary>
        public new string Name
        {
            get
            {
                return pluginInfo.Name;
            }
        }

        /// <summary>
        /// 初始化一个PluginListItem类
        /// </summary>
        public PluginListItem(PluginInfo pi)
        {
            this.pluginInfo = pi;
            this.Text = pi.Name;
            this.Checked = pi.IsLoadedAtStartup;
        }
    }
}
