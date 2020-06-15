using System;
using System.Windows.Forms;
using System.IO;
using QRST.WorldGlobeTool.Net;

namespace QRST.WorldGlobeTool.PluginEngine
{
    public partial class PluginInstallDialog : Form
    {
        public PluginInstallDialog(PluginCompiler compiler)
        {
            InitializeComponent();

            m_compiler = compiler;
        }

        /// <summary>
        /// 插件编译器
        /// </summary>
        private PluginCompiler m_compiler;

        /// <summary>
        /// 检查用户是否选择了一个文件
        /// </summary>
        bool IsFile
        {
            get
            {
                return File.Exists(url.Text);
            }
        }

        /// <summary>
        /// 检查用户是否指向了一个网址
        /// </summary>
        bool IsWeb
        {
            get
            {
                return url.Text.ToLower().StartsWith("http://");
            }
        }

        private void buttonBrowse_Click(object sender, EventArgs e)
        {
            OpenFileDialog of = new OpenFileDialog();
            if (IsFile)
                of.FileName = url.Text;
            if (of.ShowDialog() != DialogResult.OK)
                return;
            url.Text = of.FileName;
        }

        private void buttonInstall_Click(object sender, EventArgs e)
        {
            string warning = @"WARNING! You must trust the source from where you acquired this Plug-In. It
is NOT from NASA. There is always the possibility that by adding this
plug-in to World Wind, serious harm could come to your computer system. For
this reason, please verify the source for all Plug-Ins BEFORE installing so
that you may be assured of a safe and productive World Wind experience.";

            if (MessageBox.Show(warning, "Security Warning",
                MessageBoxButtons.OKCancel, MessageBoxIcon.Stop, MessageBoxDefaultButton.Button2) != DialogResult.OK)
                return;

            try
            {
                // Trim off any whitespace the user may have added 
                url.Text = url.Text.Trim();

                if (IsWeb)
                    InstallFromUrl(new Uri(url.Text));
                else if (IsFile)
                    InstallFromFile(url.Text);
                else
                {
                    MessageBox.Show("Please specify an existing filename or a web url starting with 'http://'.", "Not found", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    url.Focus();
                    return;
                }

                // Success, close this dialog
                Close();
            }
            catch (ApplicationException)
            {
                // User aborted
            }
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// 从一个文件安装插件
        /// </summary>
        /// <param name="pluginPath">插件路径</param>
        void InstallFromFile(string pluginPath)
        {
            string fileName = Path.GetFileName(pluginPath);
            string destPath = GetDestinationPath(fileName);
            if (destPath == null)
                return;

            File.Copy(pluginPath, destPath);

            ShowSuccessMessage(fileName);
        }

        /// <summary>
        /// 从一个网址安装插件
        /// </summary>
        /// <param name="uri">http:// URL</param>
        void InstallFromUrl(Uri uri)
        {
            string fileName = Path.GetFileName(uri.LocalPath);
            string destPath = GetDestinationPath(fileName);
            if (destPath == null)
                return;

            // Offline check
            if (!World.Settings.WorkOffline)
                using (WebDownload dl = new WebDownload(uri.ToString()))
                    dl.DownloadFile(destPath);
            else
                return;

            ShowSuccessMessage(fileName);
        }

        /// <summary>
        /// Calculates plugin destination directory based on name, and prepares it.
        /// 基于名称计算插件目标目录，并准备改目录
        /// </summary>
        /// <param name="fileName">只包含插件目录</param>
        string GetDestinationPath(string fileName)
        {
            string directory = Path.Combine(m_compiler.PluginRootDirectory, Path.GetFileNameWithoutExtension(fileName));
            Directory.CreateDirectory(directory);

            string fullPath = Path.Combine(directory, fileName);
            if (File.Exists(fullPath))
            {
                // Show overwrite warning
                string msg = string.Format("You already have {0} installed.  Do you wish to overwrite it?",
                    Path.GetFileNameWithoutExtension(fileName));
                if (MessageBox.Show(msg, "Overwrite?", MessageBoxButtons.YesNo, MessageBoxIcon.Question,
                    MessageBoxDefaultButton.Button2) != DialogResult.Yes)
                    throw new ApplicationException("Install aborted.");
            }

            return fullPath;
        }

        /// <summary>
        /// Display a message box with successful installation message.
        /// </summary>
        static void ShowSuccessMessage(string fileName)
        {
            string msg = string.Format("{0} was successfully installed.",
                Path.GetFileNameWithoutExtension(fileName));
            MessageBox.Show(msg, "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

    }
}
