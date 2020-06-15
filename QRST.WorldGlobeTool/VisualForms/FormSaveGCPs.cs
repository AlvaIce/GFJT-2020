using System;
using System.IO;
using System.Windows.Forms;
using QRST.WorldGlobeTool.Renderable;

namespace QRST.WorldGlobeTool.VisualForms
{
    public partial class FormSaveGCPs : Form
    {
        /// <summary>
        /// 要保存的控制点
        /// </summary>
        GCPs saveGCPs;

        /// <summary>
        /// 实例化一个保存控制点对话框
        /// </summary>
        /// <param name="gcps">要保存的控制点</param>
        public FormSaveGCPs(GCPs gcps)
        {
            InitializeComponent();
            this.saveGCPs = gcps;
        }

        /// <summary>
        /// 保存文件窗口，选择要保存的控制点文件的位置和名称
        /// </summary>
        private void buttonSavePathBrower_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "TXT文件|*.txt";
            sfd.FileName = "*.txt";
            sfd.Title = string.Format("选择{0}的位置", this.Text);
            if (sfd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                this.textBoxSavePath.Text = sfd.FileName;
            }
        }

        /// <summary>
        /// 保存控制点到指定文件中
        /// </summary>
        private void buttonSave_Click(object sender, EventArgs e)
        {
            try
            {
                using (StreamWriter sw = new StreamWriter(this.textBoxSavePath.Text))
                {
                    if (((GCP)saveGCPs.ChildObjects[0]).GcpType == GCPType.ATMGCP)
                        sw.WriteLine(saveGCPs.SourceImageWidthAndHeight);
                    GCP tempGCP;
                    for (int i = 0; i < saveGCPs.Count; i++)
                    {
                        tempGCP = saveGCPs.ChildObjects[i] as GCP;
                        if (tempGCP.GcpType == GCPType.GeoGCP)
                        {
                            sw.WriteLine(string.Format("{0} {1:f6} {2:f6} {3:f6} {4:f6}", tempGCP.Name, tempGCP.X, tempGCP.Y, tempGCP.Longitude, tempGCP.Latitude));
                        }
                        else if (tempGCP.GcpType == GCPType.ATMGCP)
                        {
                            sw.WriteLine(string.Format("{0},{1},{2},{3},{4}", tempGCP.Name, tempGCP.X, tempGCP.Y, tempGCP.Longitude, tempGCP.Latitude));
                        }
                    }
                }
                MessageBox.Show("保存成功");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Close();
            }
        }

        /// <summary>
        /// 取消保存控制点信息
        /// </summary>
        private void buttonCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
