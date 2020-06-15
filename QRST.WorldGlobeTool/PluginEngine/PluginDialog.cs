using System;
using System.Windows.Forms;
using System.IO;

namespace QRST.WorldGlobeTool.PluginEngine
{
    public partial class PluginDialog : Form
    {
        public PluginDialog(PluginCompiler compiler)
        {
            InitializeComponent();
            this.compiler = compiler;
        }

        private PluginCompiler compiler;

        /// <summary>
        /// 获取打开或关闭的图片列表
        /// </summary>
        public ImageList ImageList
        {
            get
            {
                return imageList;
            }
        }

        private void PluginDialog_Load(object sender, EventArgs e)
        {
            AddPluginList();

            //强制更新UI状态
            listView_SelectedIndexChanged(this, EventArgs.Empty);
            UpdateUIStates();
        }
        
        /// <summary>
        /// 双击列表项，加载或卸载插件
        /// </summary>
        private void listView_DoubleClick(object sender, EventArgs e)
        {
            foreach (PluginListItem pi in listView.SelectedItems)
            {
                if (pi.PluginInfo.IsCurrentlyLoaded)
                    PluginUnload(pi);
                else
                    PluginLoad(pi);
            }
            listView.Invalidate();
            UpdateUIStates();
        }

        /// <summary>
        /// 列表选中项改变时，更新界面信息
        /// </summary>
        private void listView_SelectedIndexChanged(object sender, EventArgs e)
        {
            //重置相关描述信息
            description.Text = "";
            developer.Text = "";
            webSite.Text = "";
            //更新UI状态
            UpdateUIStates();

            if (listView.SelectedItems.Count != 1)
                return;

            PluginListItem item = (PluginListItem)listView.SelectedItems[0];
            //设置相关描述信息
            description.Text = item.PluginInfo.Description;
            developer.Text = item.PluginInfo.Developer;
            webSite.Text = item.PluginInfo.WebSite;
        }

        /// <summary>
        /// 加载插件
        /// </summary>
        private void buttonLoad_Click(object sender, EventArgs e)
        {
            foreach (PluginListItem pi in listView.SelectedItems)
                PluginLoad(pi);
            listView.Invalidate();
            UpdateUIStates();
        }

        /// <summary>
        /// 卸载插件
        /// </summary>
        private void buttonUnLoad_Click(object sender, EventArgs e)
        {
            foreach (PluginListItem pi in listView.SelectedItems)
                PluginUnload(pi);
            listView.Invalidate();
            UpdateUIStates();
        }

        private void buttonInstall_Click(object sender, EventArgs e)
        {
            PluginInstallDialog installDialog = new PluginInstallDialog(compiler);
            installDialog.Icon = this.Icon;
            installDialog.ShowDialog();

            // Rescan for plugins
            compiler.FindPlugins();
            AddPluginList();
        }

        /// <summary>
        /// 卸载/移除插件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonUnInstall_Click(object sender, EventArgs e)
        {
            foreach (PluginListItem pi in listView.SelectedItems)
            {
                string fullPath = pi.PluginInfo.FullPath;
                if (!File.Exists(fullPath))
                {
                    // 忽略内部插件
                    MessageBox.Show("Plugin '" + pi.Name + "' is inside QRST.WorldGlobeTool.dll and cannot be uninstalled.",
                        "Uninstall",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                    continue;
                }

                // 显示卸载警告
                string msg = string.Format("Do you really want to uninstall {0}?", pi.Name);
                if (MessageBox.Show(msg, "Delete plugin", MessageBoxButtons.YesNo, MessageBoxIcon.Question,
                    MessageBoxDefaultButton.Button2) != DialogResult.Yes)
                    continue;

                try
                {
                    compiler.Uninstall(pi.PluginInfo);

                    // Remove if from the plugin list
                    listView.Items.Remove(pi);
                }
                catch (Exception caught)
                {
                    MessageBox.Show("Uninstall failed.  The error was:\n\n" + caught.Message, pi.Name +
                        " plugin failed to uninstall.",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                }
            }

            UpdateUIStates();
        }

        private void webSite_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (webSite.Text == null || webSite.Text.Length <= 0)
                return;

            System.Diagnostics.Process.Start(webSite.Text);
        }

        #region 私有方法

        /// <summary>
        /// 用当前已安装的插件填充列表视图
        /// </summary>
        private void AddPluginList()
        {
            listView.Items.Clear();
            foreach (PluginInfo pi in compiler.Plugins)
            {
                PluginListItem li = new PluginListItem(pi);
                listView.Items.Add(li);
            }
        }

        /// <summary>
        /// 更新控件使能状态来反映是否可选择
        /// </summary>
       private  void UpdateUIStates()
        {
            bool isItemSelected = listView.SelectedItems.Count > 0;
            buttonUnInstall.Enabled = isItemSelected;
            if (!isItemSelected)
            {
                buttonLoad.Enabled = false;
                buttonUnLoad.Enabled = false;
                return;
            }

            PluginListItem item = (PluginListItem)listView.SelectedItems[0];
            buttonLoad.Enabled = !item.PluginInfo.IsCurrentlyLoaded;
            buttonUnLoad.Enabled = item.PluginInfo.IsCurrentlyLoaded;
        }
        
       /// <summary>
       /// 加载插件，失败时显示消息
       /// </summary>
       private void PluginLoad(PluginListItem pi)
       {
           try
           {
               compiler.Load(pi.PluginInfo);
           }
           catch (Exception caught)
           {
               MessageBox.Show("错误:\n\n" + caught.Message, pi.Name + "插件加载失败！",
                   MessageBoxButtons.OK, MessageBoxIcon.Error);
           }
       }

       /// <summary>
       /// 卸载插件，失败时显示消息
       /// </summary>
       private void PluginUnload(PluginListItem pi)
       {
           try
           {
               compiler.Unload(pi.PluginInfo);
           }
           catch (Exception caught)
           {
               MessageBox.Show("错误:\n\n" + caught.Message, pi.Name + "插件卸载失败！",
                   MessageBoxButtons.OK, MessageBoxIcon.Error);
           }
       }

        
        #endregion

    }
}
