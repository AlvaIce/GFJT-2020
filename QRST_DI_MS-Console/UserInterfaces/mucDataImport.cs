using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using QRST_DI_DS_Metadata.MetaDataDefiner;
using QRST_DI_DataImportTool.DataImport;
using System.IO;
using QRST_DI_Resources;
using System.Threading.Tasks;
using QRST_DI_DataImportTool;

namespace QRST_DI_MS_Console.UserInterfaces
{
    public partial class mucDataImport : DevExpress.XtraEditors.XtraUserControl
    {
        bool isDataBaseConnect = false;
        bool isFileSystemConnect = false;


        public delegate void RefreshToolStripInfoDel(string information, int toolStripIndex, System.Drawing.Color fontColor);
        public delegate void SetTabControlStatusDel(bool status);

        public void RefreshToolStripInfo(string information, int toolStripIndex, System.Drawing.Color fontColor)
        {
            //statusStrip1.Items[toolStripIndex].Text = information;
            //statusStrip1.Items[toolStripIndex].ForeColor = fontColor;
        }

        public void SetTabControlStatus(bool status)
        {
            //tabControl1.Enabled = status;
        }

        public mucDataImport()
        {
            InitializeComponent();
            Task.Factory.StartNew(() =>
            {
                //检查连接状态
                try
                {
                    RefreshToolStripInfoDel del = new RefreshToolStripInfoDel(RefreshToolStripInfo);


                    Universial.dbOperating = new QRST_DI_DS_Basis.DBEngine.DBMySqlOperating();
                    isDataBaseConnect = true;
                    //statusStrip1.Invoke(del, new object[] { "数据库连接状态：连接成功", 0, System.Drawing.Color.Green });
                    string ip = Constant.ConnectionStringMySql.Split(new char[] { ';' })[0].Substring(Constant.ConnectionStringMySql.IndexOf("=") + 1);
                    //statusStrip1.Invoke(del, new object[] { string.Format("数据库服务器地址：{0}", ip), 4, System.Drawing.Color.Green });

                }
                catch (Exception ex)
                {
                    isDataBaseConnect = false;
                }
                if (Directory.Exists(string.Format(@"\\{0}\{1}", Constant.DeployedHadoopIP, StaticStrings.RootDir)))
                {
                    RefreshToolStripInfoDel del = new RefreshToolStripInfoDel(RefreshToolStripInfo);
                    //statusStrip1.Invoke(del, new object[] { "文件系统连接状态:连接成功", 1, System.Drawing.Color.Green });
                    isFileSystemConnect = true;
                    //statusStrip1.Invoke(del, new object[] { string.Format("文件服务器地址：{0}", Constant.FileServerIP), 3, System.Drawing.Color.Green });
                }
                else
                {
                    isFileSystemConnect = false;
                }
                SetTabControlStatusDel del1 = new SetTabControlStatusDel(SetTabControlStatus);
                if (!(isDataBaseConnect && isFileSystemConnect))
                {
                    MessageBox.Show("服务器未能连接成功，请检查连接状态！");
                    //tabControl1.Invoke(del1, new object[] { false });
                }
                //else
                //tabControl1.Invoke(del1, new object[] { true });

            });


            for (int i = 0; i < xtraTabControlDataImport.TabPages.Count; i++)
            {
                if (xtraTabControlDataImport.TabPages[i].Controls.Count > 0)
                {
                    ((ctrlCommonDataImport)xtraTabControlDataImport.TabPages[i].Controls[0]).dataType = (DataType)Enum.Parse(typeof(DataType), i.ToString());
                }
            }

            

        }
        
    }
}
