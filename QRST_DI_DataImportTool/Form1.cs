using System;
using System.Windows.Forms;
using QRST_DI_DataImportTool.DataImport;
using System.IO;
using QRST_DI_Resources;
using System.Threading.Tasks;

namespace QRST_DI_DataImportTool
{
    public partial class Form1 : Form
    {
         bool isDataBaseConnect = false;
         bool isFileSystemConnect = false;


         public delegate void RefreshToolStripInfoDel(string information,int toolStripIndex,System.Drawing.Color fontColor);
         public delegate void SetTabControlStatusDel(bool status);

         public void RefreshToolStripInfo(string information, int toolStripIndex, System.Drawing.Color fontColor)
         {
             statusStrip1.Items[toolStripIndex].Text = information;
             statusStrip1.Items[toolStripIndex].ForeColor = fontColor;
         }

         public void SetTabControlStatus(bool status)
         {
             tabControl1.Enabled = status;
         }
        

        public Form1()
        {
            InitializeComponent();
            try
            {

                if (!Constant.ServiceIsConnected)
                {
                    Constant.InitializeTcpConnection();
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show("TCP初始化连接失败：" + ex);
            }
            Task.Factory.StartNew(() => {
                //检查连接状态
                try
                {
                    RefreshToolStripInfoDel del = new RefreshToolStripInfoDel(RefreshToolStripInfo);


                    Universial.dbOperating = Constant.IdbOperating;
                    isDataBaseConnect = true;
                    statusStrip1.Invoke(del, new object[] { "数据库连接状态：连接成功", 0, System.Drawing.Color.Green });
                    string ip = Constant.ConnectionStringMySql.Split(new char[] { ';' })[0].Substring(Constant.ConnectionStringMySql.IndexOf("=") + 1);
                    statusStrip1.Invoke(del, new object[] { string.Format("数据库服务器地址：{0}", ip), 4, System.Drawing.Color.Green });
                
                }
                catch (Exception ex)
                {
                    isDataBaseConnect = false;
                }
                if (Directory.Exists(string.Format(@"\\{0}\{1}", Constant.FileServerIP, StaticStrings.RootDir)))
                {
                    RefreshToolStripInfoDel del = new RefreshToolStripInfoDel(RefreshToolStripInfo);
                    statusStrip1.Invoke(del, new object[] { "文件系统连接状态:连接成功", 1, System.Drawing.Color.Green });
                    isFileSystemConnect = true;
                    statusStrip1.Invoke(del, new object[] { string.Format("文件服务器地址：{0}", Constant.FileServerIP), 3, System.Drawing.Color.Green });
                }
                else
                {
                    isFileSystemConnect = false;
                }
                SetTabControlStatusDel del1 = new SetTabControlStatusDel(SetTabControlStatus);
                if (!(isDataBaseConnect && isFileSystemConnect))
                {
                    MessageBox.Show("服务器未能连接成功，请检查连接状态！");
                    tabControl1.Invoke(del1, new object[] { false });
                }
                else
                    tabControl1.Invoke(del1, new object[] { true });
            
            });
            
            
            for (int i = 0; i < tabControl1.TabPages.Count; i++)
            {
                if(tabControl1.TabPages[i].Controls.Count>0)
                {
                    ((ctrlCommonDataImport)tabControl1.TabPages[i].Controls[0]).dataType = (DataType)Enum.Parse(typeof(DataType), i.ToString());
                }
            }
           
        }

     



    }
}
