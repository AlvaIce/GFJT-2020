using System;
using System.Collections.Generic;
using System.Text;
using QRST_DI_MS_Basis.DataBacker;
using QRST_DI_DS_Metadata.MetaDataDefiner;
using System.IO;
using System.Threading;
using QRST_DI_Resources;
using QRST_DI_SS_Basis.MetaData;
using QRST_DI_SS_DBInterfaces.IDBEngine;
 
namespace QRST_DI_MS_Desktop.UserInterfaces
{
    public partial class FrmRecover : DevExpress.XtraEditors.XtraForm
    {
        Thread t;
        StringBuilder sb = new StringBuilder();
        bool isFinished = false;
        int errorNum = 0;
        int successNum;
        public List<SiteDb> dataBases;
        public static IDbOperating dbOperating = Constant.IdbOperating;
        public string dbname, appDirectory;
        public List<string> dbaddress;
        public FrmRecover(List<string> DbAddress, string DbName)
        {
            InitializeComponent();
            dbaddress = DbAddress;
            dbname = DbName;
           //appDirectory = AppDirecroty;
        }



        /// <summary>
        /// 恢复数据库
        /// </summary>
        public string DatabaseRecover(string dbaddress, string dbname, string appDirecroty)
        {
            try
            {
                StringBuilder sbcommand = new StringBuilder();
                StringBuilder sbfileName = new StringBuilder(dbaddress);
                string connectstr = getConnectStr(dbname);
                string[] str = connectstr.Split(";".ToCharArray());
                string ipAddress = str[0].Substring(str[0].IndexOf("=") + 1).Trim();
                string user = str[1].Substring(str[1].IndexOf("=") + 1).Trim();
                string password = str[2].Substring(str[2].IndexOf("=") + 1).Trim();
                sbcommand.AppendFormat("mysql -h{0} -u{1} -r{2} < \"{3}\"", ipAddress, user, password, dbaddress);
                appDirecroty = appDirecroty + "\\";
                if (!File.Exists(appDirecroty + "mysql.exe"))
                {
                    throw new Exception("数据库还原程序不存在!");
                }
                else
                {
                    String command = sbcommand.ToString();
                    return Cmd.StartCmd(appDirecroty, command);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("数据库全备份失败！" + ex.ToString());
            }
        }

        /// <summary>
        /// 恢复数据表
        /// </summary>
        public string TableRecover(string dbaddress, string dbname, string appDirecroty)
        {
            errorNum = 0;
            try
            {
                StringBuilder sbcommand = new StringBuilder();
                StringBuilder sbfileName = new StringBuilder(dbaddress);
                string connectstr = getConnectStr(dbname);
                string[] str = connectstr.Split(";".ToCharArray());
                string ipAddress = str[0].Substring(str[0].IndexOf("=") + 1).Trim();
                string user = str[1].Substring(str[1].IndexOf("=") + 1).Trim();
                string password = str[2].Substring(str[2].IndexOf("=") + 1).Trim();
                sb.AppendLine(string.Format("正在还原'{0}'...", dbaddress));
                sbcommand.AppendFormat("mysql -h{0} -u{1} -p{2} {4}< {3}", ipAddress, user, password, dbaddress,dbname);
                appDirecroty = appDirecroty + "\\";
                if (!File.Exists(appDirecroty + "mysql.exe"))
                {
                    throw new Exception("数据库还原程序不存在!");
                }
                else
                {
                    
                    String command = sbcommand.ToString();
                    return Cmd.StartCmd(appDirecroty, command);
                }
            }
            catch (Exception ex)
            {
                errorNum++;
                sb.AppendLine(ex.ToString());
                throw new Exception("数据库还原失败！" + ex.ToString());
            }
        }


        public static string getConnectStr(string dbname)
        {
            string str1 = string.Format(@"select ConnectStr from subdbinfo where `NAME`=" + "'" + dbname + "'");
            string connectstr = dbOperating.GetSubDbUtilities(EnumDBType.MIDB).myExcuteScalar(str1);
            return connectstr;
        }

        private void FrmRecover_Load(object sender, EventArgs e)
        {
            start();
            timer1.Start();

        }

        public void startTableRecover()
        {
            appDirectory = System.Windows.Forms.Application.StartupPath + "\\";
            for (int i = 0; i < dbaddress.Count; i++)
            {
                sb.AppendLine("开始进行数据恢复...");
                TableRecover(dbaddress[i], dbname, appDirectory);
                successNum++;
            }
            isFinished = true;
        }

        public void start()
        {
            t = new Thread(startTableRecover);
            t.Start();
        }

        private void btnFinish_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            memoBackUpMsg.Text = memoBackUpMsg.Text + sb.ToString();
            labelError.Text = errorNum.ToString();            
            labelFinished.Text = successNum.ToString();
            sb.Clear();
            if (isFinished)
            {
                btnFinish.Enabled = true;
                btnCancle.Enabled = false;
                marqueeProgressBarControl1.Properties.Stopped = true;
                marqueeProgressBarControl1.Text = "";
                timer1.Stop();
            }
        }

        private void btnCancle_Click(object sender, EventArgs e)
        {
            t.Abort();
            this.Close();
        }
    }
}
