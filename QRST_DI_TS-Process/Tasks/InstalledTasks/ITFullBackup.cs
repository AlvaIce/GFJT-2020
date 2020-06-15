using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using QRST_DI_SS_Basis;
using QRST_DI_SS_Basis.MetaData;
using QRST_DI_SS_DBInterfaces.IDBEngine;
using QRST_DI_Resources;

namespace QRST_DI_TS_Process.Tasks.InstalledTasks
{
    public class ITFullBackup : TaskClass
    {

        public static IDbBaseUtilities _baseUtilities =
            Constant.IdbOperating.GetSubDbUtilities(EnumDBType.MIDB);
        public struct autoTbName   //用于将自动备份的字符串进行拆分，处理成相应内容存储
        {
            public List<string> midbtables;
            public List<string> bsdbtables;
            public List<string> evdbtables;
            public List<string> indbtables;
            public List<string> ipdbtables;
            public List<string> isdbtables;
            public List<string> madbtables;
            public List<string> rcdbtables;
        }
        public autoTbName autoTableName = new autoTbName();
        public Dictionary<string, List<string>> autoTbName1 = new Dictionary<string, List<string>>();
        public static int TableCount = 0;//用于统计总共备份了多少张表

        public override string TaskName
        {
            get { return "ITFullBackup"; }
            set { }
        }
        public override void Process()
        {
            this.ParentOrder.Logs.Add(string.Format("开始数据库备份！"));
            //autoBackupAppointTables();
            this.ParentOrder.Logs.Add(string.Format("完成数据库备份，共备份{0}张表！",TableCount));
        }


        public void autoBackupAppointTables()
        {
            string backupaddress = Orders.OrderManager.getbackupaddress();
            string targetDir = string.Format(backupaddress);//获取备份地址
            targetDir = targetDir + "\\TableBackup";
            targetDir = targetDir + "\\" + DateTime.Now.ToString().Replace("-", "").Replace(":", "").Replace(" ", "").Replace("/", "") + "(auto)";

            string sql = string.Format(@"select value from appsettings where `key`='MysqlBackupTables'");
            string autoBackupTables = _baseUtilities.myExcuteScalar(sql);
            string[] str = autoBackupTables.TrimEnd(';').Split(";".ToCharArray());

            {
                autoTableName.midbtables = new List<string>();
                autoTableName.bsdbtables = new List<string>();
                autoTableName.evdbtables = new List<string>();
                autoTableName.rcdbtables = new List<string>();
                autoTableName.madbtables = new List<string>();
                autoTableName.isdbtables = new List<string>();
                autoTableName.ipdbtables = new List<string>();
                autoTableName.indbtables = new List<string>();
            }


            for (int i = 0; i < str.Length; i++)
            {
                string[] DbAndTable = str[i].Split(".".ToCharArray());
                string dbname = DbAndTable[0].Trim();
                string tablename = DbAndTable[1].Trim();
                switch (dbname)
                {
                    case "midb": { autoTableName.midbtables.Add(tablename); } break;
                    case "evdb": { autoTableName.evdbtables.Add(tablename); } break;
                    case "bsdb": { autoTableName.bsdbtables.Add(tablename); } break;
                    case "indb": { autoTableName.indbtables.Add(tablename); } break;
                    case "ipdb": { autoTableName.ipdbtables.Add(tablename); } break;
                    case "isdb": { autoTableName.isdbtables.Add(tablename); } break;
                    case "madb": { autoTableName.madbtables.Add(tablename); } break;
                    case "rcdb": { autoTableName.rcdbtables.Add(tablename); } break;
                    default: break;
                }
            }
            autoTbName1.Clear();
            autoTbName1.Add("midb", autoTableName.midbtables);
            autoTbName1.Add("bsdb", autoTableName.bsdbtables);
            autoTbName1.Add("evdb", autoTableName.evdbtables);
            autoTbName1.Add("rcdb", autoTableName.rcdbtables);
            autoTbName1.Add("madb", autoTableName.madbtables);
            autoTbName1.Add("isdb", autoTableName.isdbtables);
            autoTbName1.Add("ipdb", autoTableName.ipdbtables);
            autoTbName1.Add("indb", autoTableName.indbtables);

            TableCount = 0;

            foreach (KeyValuePair<string, List<string>> kvp in autoTbName1)
            {
                TableBackup(kvp.Key, kvp.Value, targetDir);
            }
        }

        public void TableBackup(string dbname, List<string> _tablesname, string _targetDir)
        {
            if (_tablesname.Count > 0)
            {
                string connectstr = Orders.OrderManager.getConnectStr(dbname);
                string[] str = connectstr.Split(";".ToCharArray());
                string ipAddress = str[0].Substring(str[0].IndexOf("=") + 1).Trim();
                string user = str[1].Substring(str[1].IndexOf("=") + 1).Trim();
                string password = str[2].Substring(str[2].IndexOf("=") + 1).Trim();
                string path = "";
                path = _targetDir + "\\" + dbname;
                CreateFile(path);
                Process p = new Process();
                for (int i = 0; i < _tablesname.Count; i++)
                {
                    p.StartInfo.FileName = "cmd.exe";
                    //p.StartInfo.WorkingDirectory = workingDirectory;
                    p.StartInfo.UseShellExecute = false;
                    p.StartInfo.RedirectStandardInput = true;
                    p.StartInfo.RedirectStandardOutput = true;
                    p.StartInfo.RedirectStandardError = true;
                    p.StartInfo.CreateNoWindow = true;
                    p.Start();
                    string command = "mysqldump --quick -h" + ipAddress + " -u" + user + " -p" + password + " --single-transaction --default-character-set=utf8  " + dbname + " " + _tablesname[i] + ">" + path + "\\" + _tablesname[i] + ".sql";
                    p.StandardInput.WriteLine(command);
                    p.StandardInput.WriteLine("exit");
                    TableCount++;
                }
            }
        }


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
                //throw e;
            }
        }
    }
}
