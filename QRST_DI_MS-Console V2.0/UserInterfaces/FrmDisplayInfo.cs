using System;
using System.Collections.Generic;
using System.Text;
using QRST_DI_DS_Metadata.MetaDataDefiner;
using System.Threading;
using QRST_DI_MS_Basis.DataBacker;
using System.IO;
using QRST_DI_Resources;
using QRST_DI_SS_Basis;
using QRST_DI_SS_Basis.MetaData;
using QRST_DI_SS_DBInterfaces.IDBEngine;
 
namespace QRST_DI_MS_Desktop.UserInterfaces
{
    public partial class FrmDisplayInfo : DevExpress.XtraEditors.XtraForm
    {
        Thread t;
        StringBuilder sb = new StringBuilder();
        bool isFinished = false;
        int errorNum = 0;
        int successNum;
        bool BackupDbORTable;
        bool BackupAddressBool;
        List<string> TableList;
        int BackupType;
        public static IDbOperating dbOperating = Constant.IdbOperating;

        public FrmDisplayInfo(string Path, List<SiteDb> data, bool i,bool j,int backuptype,List<string> tablelist)
        {
            InitializeComponent();
            backUpPath = Path;
            dataBases = data;
            BackupDbORTable = i;
            BackupAddressBool = j;
            TableList = tablelist;
            BackupType = backuptype;
        }

        public string backUpPath
        {
            get
            {
                return labelBackUpPath.Text;
            }
            set
            {
                labelBackUpPath.Text = value;
            }
        }

        public List<SiteDb> dataBases;

        private void btnCancle_Click(object sender, EventArgs e)
        {
            t.Abort();
            this.Close();
        }

        private void btnFinish_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        public void Start()
        {
            if (BackupDbORTable == true)
            {
                t = new Thread(StartBackupDatabase);
            }
            else
            {
                t = new Thread(StartBackupTables);
            }
            t.Start();
        }

        public void StartBackupDatabase()
        {
            errorNum = 0;
            string backupath1 = backUpPath;
            string backupath2 = "";
            if (BackupAddressBool == true)
            {
                backupath1 = backUpPath + "\\DatabaseBackup\\" + DateTime.Now.ToString().Replace("-", "").Replace(":", "").Replace(" ", "").Replace("/", "");
                CreateFile(backupath1);
            }
            backupath2 = backupath1;

            string appDirecroty = System.Windows.Forms.Application.StartupPath + "\\";
            sb.AppendLine("开始进行数据备份...");
            for (int i = 0; i < dataBases.Count; i++)
            {//  edit.Text = value.ToString() + "\r\n" + edit.Text;
                try
                {
                    sb.AppendLine(string.Format("正在备份'{0}'...", dataBases[i].DESCRIPTION));
                    string[] str = dataBases[i].ConnectStr.Split(";".ToCharArray());
                    string ipAddress = str[0].Substring(str[0].IndexOf("=") + 1).Trim();
                    string user = str[1].Substring(str[1].IndexOf("=") + 1).Trim();
                    string password = str[2].Substring(str[2].IndexOf("=") + 1).Trim();
                    DataBackup.serverIP = ipAddress;
                    DataBackup.userName = user;
                    DataBackup.pwd = password;
                    sb.Append(DataBackup.BackupDatabase(dataBases[i].NAME, appDirecroty, backupath2));
                    successNum++;
                }
                catch (System.Exception ex)
                {
                    errorNum++;
                    sb.AppendLine(ex.ToString());
                }
            }
            isFinished = true;
        }

        public void StartBackupTables()
        {
            errorNum = 0;
            string backupath1 = backUpPath;
            string backupath2 = "";
            if (BackupAddressBool == true)
            {
                if (BackupType == 3)
                {
                    backupath1 = backUpPath + "\\TableBackup\\" + DateTime.Now.ToString().Replace("-", "").Replace(":", "").Replace(" ", "").Replace("/", "");
                    CreateFile(backupath1);
                }

                if (BackupType == 2)
                {
                    backupath1 = backUpPath + "\\TableBackup\\" + DateTime.Now.ToString().Replace("-", "").Replace(":", "").Replace(" ", "").Replace("/", "");
                    CreateFile(backupath1);
                }
                if (BackupType == 1)
                {
                    backupath1 = backUpPath + "\\FullTableBackup\\" + DateTime.Now.ToString().Replace("-", "").Replace(":", "").Replace(" ", "").Replace("/", "");
                    CreateFile(backupath1);
                }
            }

            string appDirecroty = System.Windows.Forms.Application.StartupPath + "\\";
            sb.AppendLine("开始进行数据备份...");
            for (int i = 0; i < dataBases.Count; i++)
            {//  edit.Text = value.ToString() + "\r\n" + edit.Text;
                try
                {
                    if (BackupAddressBool == true)
                    {
                        backupath2 = backupath1 + "\\" + dataBases[i].NAME;
                        CreateFile(backupath2);
                    }
                    else
                    {
                        backupath2 = backupath1;
                    }
                    

                    sb.AppendLine(string.Format("正在备份'{0}'...", dataBases[i].DESCRIPTION));
                    string[] str = dataBases[i].ConnectStr.Split(";".ToCharArray());
                    string ipAddress = str[0].Substring(str[0].IndexOf("=") + 1).Trim();
                    string user = str[1].Substring(str[1].IndexOf("=") + 1).Trim();
                    string password = str[2].Substring(str[2].IndexOf("=") + 1).Trim();
                    DataBackup.serverIP = ipAddress;
                    DataBackup.userName = user;
                    DataBackup.pwd = password;

                    string sql = string.Format(@"select table_name from information_schema.tables where table_schema='" + dataBases[i].NAME + "' and table_type='base table'");
                    string sql2 = string.Format(@"select table_name from information_schema.tables where table_schema='" + dataBases[i].NAME + "' and table_type='view'");
                    List<string> tablename1, viewname1;



                    if (BackupType == 3)
                    {
                        tablename1 = TableList;
                        for (int j = 0; j < tablename1.Count; j++)
                        {
                            sb.Append(DataBackup.BackupTables(dataBases[i].NAME, tablename1[j], appDirecroty, backupath2));
                            successNum++;
                        }
                    }
                    else
                    {

                        tablename1 = dbOperating.GetSubDbUtilities(EnumDBType.MIDB).myExcuteReader(sql);
                        viewname1 = dbOperating.GetSubDbUtilities(EnumDBType.MIDB).myExcuteReader(sql2);
                        for (int j = 0; j < tablename1.Count; j++)
                        {
                            sb.Append(DataBackup.BackupTables(dataBases[i].NAME, tablename1[j], appDirecroty, backupath2));
                            successNum++;
                        }
                        for (int k = 0; k < viewname1.Count; k++)
                        {
                            sb.Append(DataBackup.BackupTables(dataBases[i].NAME, viewname1[k], appDirecroty, backupath2));
                            successNum++;
                        }
                    }

                }
                catch (System.Exception ex)
                {
                    errorNum++;
                    sb.AppendLine(ex.ToString());
                }
            }
            isFinished = true;
        }

        private void FrmDisplayInfo_Load(object sender, EventArgs e)
        {
            Start();
            timer1.Start();
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

        private void memoBackUpMsg_EditValueChanged(object sender, EventArgs e)
        {

        }

        //private void FrmDisplayInfo_Load(object sender, EventArgs e)
        //{
        //    StartBackUp();
        //}



        //在指定目录创建文件夹
        public static void CreateFile(string path)
        {
            try
            {
                // Determine whether the directory exists.
                if (!Directory.Exists(path))
                {
                    // Create the directory it does not exist.
                    Directory.CreateDirectory(path);
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}