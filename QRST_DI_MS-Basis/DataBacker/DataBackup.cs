using System;
using System.Text;
using System.IO;

namespace QRST_DI_MS_Basis.DataBacker
{
    public class DataBackup
    {
        public static string serverIP = "172.16.0.185";
        public static string userName = "HJDATABASE_ADMIN";
        public static string pwd = "dbadmin_2011";
        //public static string serverIP = "localhost";
        //public static string userName = "root";
        //public static string pwd = "123";

        /// <summary>
        /// 备份数据 zxw 2013/1/22;Changed by Jiang Bin 2014/12/17
        /// </summary>
        /// <param name="dataBase">备份的数据库名称</param>
        /// <param name="backupPath">备份的数据文件路径</param>
        /// <param name="specificData">指定需要备份的项目</param>
        public static string BackupDatabase(string dataBase, string appDirecroty, string backupPath, string[] specificData = null)
        {
            try
            {
                StringBuilder sbcommand = new StringBuilder();
                StringBuilder sbfileName = new StringBuilder(dataBase);
                //sbfileName.AppendFormat("{0}", DateTime.Now.ToString()).Replace("-", "").Replace(":", "").Replace(" ", "").Replace("/", "");
                String fileName = backupPath + "\\" + sbfileName.ToString() + ".sql";
                string items = "";
                if (specificData != null)
                {
                    for (int i = 0 ; i < specificData.Length ; i++)
                    {
                        items = items + " " + specificData[i];
                    }
                }

                sbcommand.AppendFormat("mysqldump --quick --host={2} --default-character-set=utf8 --single-transaction --verbose  --force  --user={3} --password={4} --database {0} {5} > \"{1}\"", dataBase, fileName, serverIP, userName, pwd, items);

                appDirecroty = appDirecroty + "\\";
                if (!File.Exists(appDirecroty + "mysqldump.exe"))
                {
                    throw new Exception("数据库备份应用程序不存在!");
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
        /// 备份服务器上所有数据库
        /// </summary>
        /// <param name="backupPath">备份路径</param>
        public static string BackupAllDataBase(string backupPath, string appDirecroty)
        {
            try
            {
                StringBuilder sbcommand = new StringBuilder();
                StringBuilder sbfileName = new StringBuilder();
                sbfileName.AppendFormat("{0}", DateTime.Now.ToString()).Replace("-", "").Replace(":", "").Replace(" ", "").Replace("/", "");
                String fileName = backupPath + "\\" + sbfileName.ToString() + ".sql";

                sbcommand.AppendFormat("mysqldump --single-transaction --host={0} --user={1} --password={2} --flush-logs --master-data=2 --all-databases  -r \"{3}\"", serverIP, userName, pwd, fileName);

           //     String appDirecroty = System.Windows.Forms.Application.StartupPath + "\\";
                appDirecroty = appDirecroty + "\\";
                if (!File.Exists(appDirecroty + "mysqldump.exe"))
                {
                    throw new Exception("数据库备份应用程序不存在!");
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
        /// mysql数据库的增量备份
        /// </summary>
        /// <returns></returns>
        public static string IncrementalBackup(string appDirecroty)
        {
            try
            {
                string command = string.Format("mysqladmin --user={0} --password={1} flush-logs", userName, pwd);
                appDirecroty = appDirecroty + "\\";
                if (!File.Exists(appDirecroty + "mysqladmin.exe"))
                {
                    throw new Exception("数据库备份应用程序不存在!");
                }
                else
                {
                    return Cmd.StartCmd(appDirecroty, command);
                }
            }
            catch (System.Exception ex)
            {
                throw new Exception("数据库增量备份失败！" + ex.ToString());
            }
        }

        /// <summary>
        /// 备份单表，可通过循环来备份整个数据库，written by Jiang Bin
        /// </summary>
        /// <returns></returns>
        public static string BackupTables(string dataBase, string tablename, string appDirecroty, string backupPath, string[] specificData = null)
        {
            try
            {
                StringBuilder sbcommand = new StringBuilder();
                StringBuilder sbfileName = new StringBuilder(tablename);
                //sbfileName.AppendFormat("{0}", DateTime.Now.ToString()).Replace("-", "").Replace(":", "").Replace(" ", "").Replace("/", "");
                String fileName = backupPath + "\\" + sbfileName.ToString() + ".sql";
                string items = "";
                if (specificData != null)
                {
                    for (int i = 0; i < specificData.Length; i++)
                    {
                        items = items + " " + specificData[i];
                    }
                }



                sbcommand.AppendFormat("mysqldump --quick --host={2} --default-character-set=utf8 --lock-tables --verbose  --force  --user={3} --password={4} {0} {6} {5} > \"{1}\"", dataBase, fileName, serverIP, userName, pwd, items,tablename);

                appDirecroty = appDirecroty + "\\";
                if (!File.Exists(appDirecroty + "mysqldump.exe"))
                {
                    throw new Exception("数据库备份应用程序不存在!");
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
    }
}
