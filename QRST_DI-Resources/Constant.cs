using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Management;
using QRST_DI_SS_DBClient.DBEngine;
using QRST_DI_SS_DBClient.DBService;
using QRST_DI_SS_DBClient.TCP;
using QRST_DI_SS_DBInterfaces.IDBEngine;
using QRST_DI_SS_DBInterfaces.IDBService;
using System.IO;
using System.Windows.Forms;
using QRST_DI_SS_Basis.MetaData;
using QRST_DI_SS_Basis;

namespace QRST_DI_Resources
{
    public class Constant
    {
        #region TCP远程对象
        /// <summary>
        /// 数据库远程底层操作接口
        /// </summary>
        public static IDbBaseUtilities IdbServerUtilities { get; set; }

        /// <summary>
        /// 瓦片库远程基础操作接口
        /// </summary>
        public static IDbBaseUtilities ITileDbUtilities { get; set; }
        public static int a =1;

        /// <summary>
        /// 八大库顶层远程操作接口
        /// </summary>
        public static IDbOperating IdbOperating { get; set; }

        /// <summary>
        /// WS_QDB_GetData远程服务操作接口
        /// </summary>
        public static IQDB_GetData IGetDataService { get; set; }

        /// <summary>
        /// WS_Searcher__Sqlite远程服务操作接口
        /// </summary>
        public static IQDB_Searcher_Tile ISearcherTileServ { get; set; }

        /// <summary>
        /// WS_Searcher__MySQL远程服务操作接口
        /// </summary>
        public static IQDB_Searcher_Db ISearcherDbServ { get; set; }

        /// <summary>
        /// TCP远程服务操作接口
        /// </summary>
        public static ITCPService ITcpService { get; set; }

        /// <summary>
        /// WS_QDB__SubmitOrder远程服务操作接口
        /// </summary>
        public static IQDB_SubmitOrder ISubmitOrderSer { get; set; }
        #endregion
       
        /// <summary>
        /// 不通过读取配置文件，需要手工输入的数据库连接字符
        /// </summary>
        public static string MyConnectionStringMysql
        {
            get;
            set;
        }

        /// <summary>
        /// 不通过读取配置文件，需要手工输入的数据库连接字符
        /// </summary>
        public static string MyConnectionStringSQLite
        {
            get;
            set;
        }

        /// <summary>
        /// 获取App.config里配置项的hadoop数据阵列存储路径  
        /// </summary>
        public static string DataStorePath
        {
            get;
            set;
        }

        /// <summary>
        /// 获取App.config里配置项的行业代号
        /// </summary>
        public static string INDUSTRYCODE
        {
            get;
            set;
        }

        /// <summary>
        /// 获取App.config里配置项的数据库连接字符串  
        /// </summary>
        public static string ConnectionString
        {
            get;
            set;
        }

        /// <summary>
        /// 获取App.config里配置项的数据库连接字符串(Oracle版)
        /// </summary>
        public static string ConnectionStringOracle
        {
            get;
            set;
        }

        /// <summary>
        /// 获取App.config里配置项的数据库连接字符串(MySql版)
        /// </summary>
        public static string ConnectionStringMySql
        {
            get
            {
                #region 废弃，在各程序初始时Create()连接
                //{
                //    if (!Created)
                //    {
                //        if (MyConnectionStringMysql!= null && !MyConnectionStringMysql.Equals(""))
                //        {
                //            Create(MyConnectionStringMysql);
                //        }
                //        else {
                //            Create();
                //        }
                //    }
                //    if (MyConnectionStringMysql != null && !MyConnectionStringMysql.Equals(""))
                //    {
                //        return MyConnectionStringMysql;
                //    }
                //    else
                //    {
                //        if (ConfigKeyValue.ContainsKey("ConnectionStringMySql"))
                //        {
                //            return ConfigKeyValue["ConnectionStringMySql"];
                //        }
                //        else
                //        {
                //            return "";
                //        }
                //    }
                #endregion

                try
                {
                    string relativeCon = ConfigurationSettings.AppSettings["ConnectionStringMySql"];
                    return relativeCon;
                }
                catch (Exception)
                {
                    return null;
                }

            }
        }

        /// <summary>
        /// 获取App.config里配置项的数据库连接字符串(SQLite版)
        /// </summary>
        public static string ConnectionStringSQLite
        {
            get
            {
                //if (!Created)
                //{
                //    if (MyConnectionStringSQLite != null && !MyConnectionStringSQLite.Equals(""))
                //    {
                //        Create(MyConnectionStringSQLite);
                //    }
                //    else
                //    {
                //        Create();
                //    }
                //}
                //if (MyConnectionStringSQLite != null && !MyConnectionStringSQLite.Equals(""))
                //{
                //    return MyConnectionStringSQLite;
                //}
                //else
                //{
                //    return ConfigurationSettings.AppSettings["ConnectionStringSQLite"];
                //}
                try
                {
                    string relativeCon = ConfigurationSettings.AppSettings["ConnectionStringSQLite"];
                    //SQLiteBaseUtilities初始化时不用生成，所有元数据生成在MS、TS启动时完成。 jianghua 20170405
                    //Create();  
                    string absoluteCon = null;
                    if (!string.IsNullOrEmpty(relativeCon))
                    {
                        string[] relativePathArr = relativeCon.Split('=');
                        string tmpRootDir = Application.StartupPath.ToString().Trim();
                        string absolutePath = OperateFilePathFun.GetMixedAbsPath(tmpRootDir, relativePathArr[1]);
                        absoluteCon = string.Format("{0}={1}", relativePathArr[0], absolutePath);
                    }

                    return absoluteCon;

                }
                catch (Exception)
                {
                    return null;
                }


            }
        }

        /// <summary>
        /// 根据MIDB库的DbServer绝对路径和瓦片根目录的相对路径，
        /// 获得瓦片根目录的绝对路径
        /// </summary>
        public static string PcDBRootPath { get;
            set;
        }
        /// <summary>
        /// 存储站点（SS）TCP通道端口
        /// </summary>
        //public static string TcpSSPort { get;
        //    set;
        //}

        public static string DbServerIp
        {
            get
            {

                string serverIp= ConfigurationSettings.AppSettings["DbServerIp"];
                if ((String.IsNullOrEmpty(serverIp)))
                {
                    return null;
                }
                return serverIp;
            }
        }

        public static string dbUtilityTcpPort
        {
            get
            {
                string tcpPort=null;
                switch (DbStorage)
                {
                    case EnumDbStorage.SINGLE:
                        try
                        {
                            tcpPort = XmlUtility.GetServerUtilityPort("dbUtilityTcpPort");
                            if (string.IsNullOrEmpty(tcpPort))
                            {
                                tcpPort = "51112";
                            }

                        }
                        catch (Exception e)
                        {
                            throw new Exception("获取dbUtilityTcpPort异常：", e);
                        }
                        break;
                    case EnumDbStorage.MULTIPLE:
                        tcpPort = ConfigurationSettings.AppSettings["BaseUtilityTcpPort"];
                        if (String.IsNullOrEmpty(tcpPort))
                        {
                            return null;
                        }

                        break;
                    case EnumDbStorage.CLUSTER:
                        break;
                }
                return tcpPort;
            }
        }

        #region 换成单端口通信模式 jianghua 20170515
        //public static string dbOperatingTcpPort { get; set; }

        //public static string SubmitOrderTcpPort { get; set; }

        //public static string TileTcpPort { get; set; }

        //public static string SearDbTcpPort { get; set; }

        //public static string SearAppTcpPort { get; set; }

        //public static string GetDataTcpPort { get; set; }
        #endregion
        
            /// <summary>
        /// 获取系统语言
        /// </summary>
        public static EnumLanguage SystemLanguage
        {
            get
            {
                EnumLanguage selectedLang= EnumLanguage.en;
                string language = ConfigurationSettings.AppSettings["SystemLanguage"];
                if (string.IsNullOrEmpty(language))
                {
                    language = "ch";
                }
                switch(language.ToLower())
                {
                    case "ch":
                        selectedLang= EnumLanguage.ch;
                        break;
                    case "en":
                        selectedLang = EnumLanguage.en;
                        break;
                }
                return selectedLang;
            }
        }

        /// <summary>
        /// 瓦片SQLite数据库通信方式
        /// </summary>
        public static EnumDbStorage DbStorage
        {
            get
            {
                try
                {
                    string communication = ConfigurationSettings.AppSettings["DbCommunication"];
                    EnumDbStorage dbCommuni = EnumDbStorage.NULL;
                    if(!string.IsNullOrEmpty(communication))
                    {
                        switch (communication.ToUpper())
                        {
                            case "SINGLE":
                                dbCommuni = EnumDbStorage.SINGLE;
                                break;
                            case "MULTIPLE":
                                dbCommuni = EnumDbStorage.MULTIPLE;
                                break;
                            case "CLUSTER":
                                dbCommuni = EnumDbStorage.CLUSTER;
                                break;
                        }
                       
                    }
                    return dbCommuni;
                }
                catch
                {
                    throw new Exception();
                }
            }
        }

        /// <summary>
        /// 底层数据库引擎
        /// </summary>
        public static EnumDbEngine QrstDbEngine
        {
            get
            {
                try
                {
                    string engine = ConfigurationSettings.AppSettings["QrstDbEngine"];
                    EnumDbEngine dbEngine = EnumDbEngine.NULL;
                    if (!string.IsNullOrEmpty(engine))
                    {
                        switch (engine.ToUpper())
                        {
                            case "MYSQL":
                                dbEngine = EnumDbEngine.MYSQL;
                                break;
                            case "SQLITE":
                                dbEngine = EnumDbEngine.SQLITE;
                                break;
                            case "ClOUDDB":
                                dbEngine = EnumDbEngine.ClOUDDB;
                                break;
                        }
                    }
                    return dbEngine;
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }

            }
        }
        /// <summary>
        /// 获取App.config里配置项的实验验证数据库连接字符串(MySql版)
        /// </summary>
        public static string ConnectionStringEVDB
        {
            get;
            set;
        }
        public static string ConnectionStringISDB
        {
            get;
            set;
        }
        public static string ConnectionStringINDB
        {
            get;
            set;
        }
        public static string ConnectionStringIPDB
        {
            get;
            set;
        }
        public static string ConnectionStringMADB
        {
            get;
            set;
        }
        public static string ConnectionStringRCDB
        {
            get;
            set;
        }
        /// <summary>
        /// 获取App.config里配置项的基础空间数据库连接字符串(MySql版)
        /// </summary>
        public static string ConnectionStringBSDB
        {
            get;
            set;
        }


        /// <summary>
        /// 预处理系统所在IP地址
        /// </summary>
        public static string CorrectRecieveIP
        {
            get;
            set;
        }

        /// <summary>
        /// 本机使用IP地址
        /// </summary>
        public static string UsingIPAddress
        {
            get;
            set;
        }

        /// <summary>
        /// 云中心结点IP地址
        /// </summary>
        public static string CloudCenterIPAddress
        {
            get;
            set;
        }
		/// <summary>
		/// ftp服务地址
		/// </summary>
		public static string FtpServerIP
		{
			get;
			set;
		}
        /// <summary>
        /// ftp用户名
        /// </summary>
        public static string FtpUserName
        {
            get;
            set;
        }
        /// <summary>
        /// ftp密码
        /// </summary>
        public static string FtpPassword
        {
            get;
            set;
        }
        /// <summary>
        /// ftp端口号
        /// </summary>
        public static string FtpServerPort
        {
            get;
            set;
        }

        public static string ListenPort
        {
            get;
            set;
        }
        /// <summary>
        /// 消息监听端口
        /// </summary>
        public static string NoticeListenPort
        {
            get;
            set;
        }
        /// <summary>
        /// 消息发送端口
        /// </summary>
        public static string NoticeSendPort
        {
            get;
            set;
        }

        /// <summary>
        /// 服务台IP地址
        /// </summary>
        public static string ConsoleServerIP
        {
            get;
            set;
        }
        /// <summary>
        /// 服务台TCP通道端口
        /// </summary>
        public static string TCPConsolePort
        {
            get;
            set;
        }
        /// <summary>
        /// 站点响应最低阈值（单位ms）
        /// </summary>
        public static string SSResponseThreshold
        {
            get;
            set;
        }
        /// <summary>
        /// 监控系统IP
        /// </summary>
        public static string MonitorIP
        {
            get;
            set;
        }
        /// <summary>
        /// 切片程序所在路径
        /// </summary>
        public static string ImageCutExePath
        {
            get;
            set;
        }

        /// <summary>
        /// 应用中心系统标识
        /// </summary>
        public static string ServerType_IsCenter
        {
            get;
            set;
        }
        /// <summary>
        /// 切片程序所在路径
        /// </summary>
        public static string WS_QDB_GetData
        {
            get;
            set;
        }
        public static string WS_QDB_Searcher_APP
        {
            get;
            set;
        }
        public static string WS_QDB_Searcher_MySQL
        {
            get;
            set;
        }
        public static string WS_QDB_Searcher_Sqlite
        {
            get;
            set;
        }
        public static string WS_QDB_SubmitOrder
        {
            get;
            set;
        }
        /// <summary>
        /// 判断是否调用河大的通信的EXE程序
        /// </summary>

        public static string GetHenuEXE
        {
            get;
            set;
        }

        /// <summary>
        /// 返回部署好的hadoop云存储站点IP
        /// </summary>
        public static string DeployedHadoopIP
        {
            get;
            set;
        }

        /// <summary>
        /// 历史hadoop地址
        /// </summary>
        public static string DeployedHadoopIP_History
        {
            get;
            set;
        }
        //添加日期：20130326 集成共享端发送消息终结点名称 
        public static string JcgxEndPointName
        {
            get;
            set;
        }

        //添加日期：20130412 调用数据总线的url
        public static string DataBusServerUrl
        {
            get;
            set;
        }

        //添加日期：20130415 缩略图存放的文件服务器
        public static string FileServerIP
        {
            get;
            set;
        }
        //添加日期：20131209 HJStorageSite存储路径
        public static string HJStorageSitesPath
        {
            get;
            set;
        }
        public static string DataDownLoadPath
        {
            get;
            set;
        }
		public static string JcgxEndPointAddress
		{
			get;
			set;
		}
		public static string SnffEndPointAddress
		{
			get;
			set;
		}
        public static string NEWFTPEndPointAddress
        {
            get;
            set;
        }
        public static string OLDNEWFTPEndPointAddress
        {
            get;
            set;
        }
		public static string WPFFEndPointAddress
		{
			get;
			set;
		}

        //查询到的瓦片数据压缩包推送地址
        public static string ResultTileZipPath
        {
            set;
            get;
        }
        /// <summary>
        ///  快视图存放位置
        /// </summary>
        public static string PreviewPath
        {
            set;
            get;
        }

        /// <summary>
        /// 获取root数据的绝对路径  等价于QRST_DB_Prototype
        /// </summary>
        private static void GetDataAbsRootPath()
        {
            PcDBRootPath = IdbOperating.GetDbRootDataPath();
        }

        public static string releasePath(string path)
        {

            string tmpRootDir = Directory.GetCurrentDirectory().ToString();
            if (path.StartsWith("..\\"))
            {
                int sum = substringCount(path, "..\\");
                path = path.Replace("..\\", "");
                string[] arr = tmpRootDir.Split(new char[] { '\\' });
                int startIndex = tmpRootDir.LastIndexOf(arr[arr.Length - sum]);
                tmpRootDir = tmpRootDir.Remove(startIndex, tmpRootDir.Length - startIndex);
                tmpRootDir = string.Format("{0}{1}", tmpRootDir, path);
            }
            else if (path.StartsWith(".\\"))
            {

                path = path.Remove(0, 2);
                tmpRootDir = string.Format("{0}\\{1}", tmpRootDir, path);

            }
            else tmpRootDir = path;

            return tmpRootDir;
        }
        private static int substringCount(string str, string substring)
        {
            if (str.Contains(substring))
            {
                string strReplaced = str.Replace(substring, "");
                return (str.Length - strReplaced.Length) / substring.Length;
            }
            return 0;
        }
        public static bool Created = false;
        public static bool ServiceIsConnected = false;
        static Dictionary<string, string> ConfigKeyValue = null;

        private static void readConfig(Configuration cfg)
        {
            if (ConfigKeyValue == null)
            {
                ConfigKeyValue = new Dictionary<string, string>();
            }
            ConfigKeyValue.Clear();
            for (int i = 0; i < cfg.AppSettings.Settings.Count; i++)
            {
                string key = cfg.AppSettings.Settings.AllKeys[i];
                ConfigKeyValue.Add(key, cfg.AppSettings.Settings[key].Value.ToString());
            }
        }

        private static void readConfig(string mysqlconn)
        {
            if (ConfigKeyValue == null)
            {
                ConfigKeyValue = new Dictionary<string, string>();
            }
            ConfigKeyValue.Clear();

            ConfigKeyValue.Add("ConnectionStringMySql", mysqlconn);
        }

        private static void readConfig()
        {
            if (ConfigKeyValue == null)
            {
                ConfigKeyValue = new Dictionary<string, string>();
            }
            ConfigKeyValue.Clear();
            for (int i = 0; i < ConfigurationManager.AppSettings.Count; i++)
            {
                string key = ConfigurationManager.AppSettings.AllKeys[i];
                ConfigKeyValue.Add(key, ConfigurationManager.AppSettings[key].ToString());
            }
        }

        private static void create()
        {
            try
            {
                UsingIPAddress = GetIPAddress();

                #region 迁移到服务进程 @jianghua 20170415

                //MySqlConnection con = (MySqlCon != "") ? new MySqlConnection(MySqlCon) : new MySqlConnection();
                //DataTable dt = new DataTable();
                //try
                //{
                //    con.Open();//打开数据连接
                //    MySqlDataAdapter adapter = new MySqlDataAdapter(sql, con);
                //    int dd = adapter.Fill(dt);
                //}
                //catch (Exception ex)
                //{
                //    //throw ex;
                //    if (con.State == ConnectionState.Closed)
                //    {
                //        con.Open();//打开数据连接
                //        MySqlDataAdapter adapter = new MySqlDataAdapter(sql, con);
                //        int dd = adapter.Fill(dt);
                //    }
                //}
                //finally
                //{
                //    con.Close();//关闭数据库连接
                //}

                #endregion

                string sql = "select appSettings.key,appSettings.value from appSettings";
                if(IdbOperating==null)
                {
                    IdbOperating = DbOperatingTCPClient.InitTCPClient_StorageChl(DbServerIp, dbUtilityTcpPort);
                }

                IdbServerUtilities = IdbOperating.GetSubDbUtilities(EnumDBType.MIDB);
                DataSet ds = IdbServerUtilities.GetDataSet(sql);
                string tempConn = null;
                foreach (DataRow rows in ds.Tables[0].Rows)
                {
                    Object[] row = rows.ItemArray;
                    if (row[0].ToString() == "DataStorePath")
                        DataStorePath = row[1].ToString();
                    else if (row[0].ToString() == "INDUSTRYCODE")
                        INDUSTRYCODE = row[1].ToString();
                    else if (row[0].ToString() == "ConnectionString")
                        ConnectionString = row[1].ToString();
                    else if (row[0].ToString() == "ConnectionStringOracle")
                        ConnectionStringOracle = row[1].ToString();
                    else if (row[0].ToString() == "ConnectionStringEVDB")
                     {
                        tempConn = row[1].ToString();
                        ConnectionStringEVDB = IdbOperating.GetAbsoluteDbCon(tempConn);
                    }
                    else if (row[0].ToString() == "ConnectionStringBSDB")
                    {
                        tempConn = row[1].ToString();
                        ConnectionStringBSDB = IdbOperating.GetAbsoluteDbCon(tempConn);
                    }
                    else if (row[0].ToString() == "ConnectionStringIPDB")
                    {
                        tempConn = row[1].ToString();
                        ConnectionStringIPDB = IdbOperating.GetAbsoluteDbCon(tempConn);
                    }
                    else if (row[0].ToString() == "ConnectionStringINDB")
                    {
                        tempConn = row[1].ToString();
                        ConnectionStringINDB = IdbOperating.GetAbsoluteDbCon(tempConn);
                    }
                    else if (row[0].ToString() == "ConnectionStringISDB")
                    {
                        tempConn = row[1].ToString();
                        ConnectionStringISDB = IdbOperating.GetAbsoluteDbCon(tempConn);
                    }
                    else if (row[0].ToString() == "ConnectionStringMADB")
                    {
                        tempConn = row[1].ToString();
                        ConnectionStringMADB = IdbOperating.GetAbsoluteDbCon(tempConn);
                    }
                    else if (row[0].ToString() == "ConnectionStringRCDB")
                    {
                        tempConn = row[1].ToString();
                        ConnectionStringRCDB = IdbOperating.GetAbsoluteDbCon(tempConn);
                    }
                    else if (row[0].ToString() == "CorrectRecieveIP")
                    {
                        CorrectRecieveIP = row[1].ToString();
                    }
                    else if (row[0].ToString() == "CloudCenterIPAddress")
                        CloudCenterIPAddress = row[1].ToString();
                    else if (row[0].ToString() == "FtpServerName")
                        FtpUserName = row[1].ToString();
                    else if (row[0].ToString() == "FtpServerPassword")
                        FtpPassword = row[1].ToString();
                    else if (row[0].ToString() == "FtpServerPort")
                        FtpServerPort = row[1].ToString();
                    else if (row[0].ToString() == "ListenPort")
                        ListenPort = row[1].ToString();
                    else if (row[0].ToString() == "NoticeListenPort")
                        NoticeListenPort = row[1].ToString();
                    else if (row[0].ToString() == "NoticeSendPort")
                        NoticeSendPort = row[1].ToString();
                    else if (row[0].ToString() == "ConsoleServerIP")
                        ConsoleServerIP = row[1].ToString();
                    else if (row[0].ToString() == "TCPConsolePort")
                        TCPConsolePort = row[1].ToString();
                    else if (row[0].ToString() == "SSResponseThreshold")
                        SSResponseThreshold = row[1].ToString();
                    else if (row[0].ToString() == "MonitorIP")
                        MonitorIP = row[1].ToString();
                    else if (row[0].ToString() == "ImageCutExePath")
                        ImageCutExePath = row[1].ToString();
                    else if (row[0].ToString() == "ServerType_IsCenter")
                        ServerType_IsCenter = row[1].ToString();
                    else if (row[0].ToString() == "WS_QDB_GetData")
                        WS_QDB_GetData = row[1].ToString();
                    else if (row[0].ToString() == "GetHenuEXE")
                        GetHenuEXE = row[1].ToString();
                    else if (row[0].ToString() == "DeployedHadoopIP")
                        DeployedHadoopIP = row[1].ToString();
                    else if (row[0].ToString() == "JcgxEndPointName")
                        JcgxEndPointName = row[1].ToString();
                    else if (row[0].ToString() == "DataBusServerUrl")
                        DataBusServerUrl = row[1].ToString();
                    else if (row[0].ToString() == "FileServerIP")
                        FileServerIP = row[1].ToString();
                    else if (row[0].ToString() == "HJStorageSitesPath")
                        HJStorageSitesPath = row[1].ToString();
                    else if (row[0].ToString() == "DataDownLoadPath")
                        DataDownLoadPath = row[1].ToString();
                    else if (row[0].ToString() == "WS_QDB_Searcher_APP")
                        WS_QDB_Searcher_APP = row[1].ToString();
                    else if (row[0].ToString() == "WS_QDB_Searcher_MySQL")
                        WS_QDB_Searcher_MySQL = row[1].ToString();
                    else if (row[0].ToString() == "WS_QDB_Searcher_Sqlite")
                        WS_QDB_Searcher_Sqlite = row[1].ToString();
                    else if (row[0].ToString() == "WS_QDB_SubmitOrder")
                        WS_QDB_SubmitOrder = row[1].ToString();
                    else if (row[0].ToString() == "DeployedHadoopIP_History")
                        DeployedHadoopIP_History = row[1].ToString();
                    else if (row[0].ToString() == "JcgxEndPointAddress")
                        JcgxEndPointAddress = row[1].ToString();
                    else if (row[0].ToString() == "SnffEndPointAddress")
                        SnffEndPointAddress = row[1].ToString();
                    else if (row[0].ToString() == "WPFFEndPointAddress")
                        WPFFEndPointAddress = row[1].ToString();
                    else if (row[0].ToString() == "NEWFTPEndPointAddress")
                        NEWFTPEndPointAddress = row[1].ToString();

                    else if (row[0].ToString() == "OLDNEWFTPEndPointAddress")
                        OLDNEWFTPEndPointAddress = row[1].ToString();

                    else if (row[0].ToString() == "FtpServerIP")
                        FtpServerIP = row[1].ToString();
                    else if (row[0].ToString() == "ResultTileZipPath")
                        ResultTileZipPath = row[1].ToString();
                    else if (row[0].ToString() == "PreviewPath")
                        PreviewPath = releasePath(row[1].ToString());

                }
                Created = true;
            }
            catch (Exception e)
            {
                throw new Exception("create:获取元数据异常", e);
            }

        }

        public static void Create(String connStringMysql)
        {
            readConfig(connStringMysql);
            //create(connStringMysql);
            create();
        }

        /// <summary>
        /// 通过TCP服务获取midb库里的元数据信息
        /// </summary>
        public static void Create()
        {
            try
            {
            readConfig();
            //if (ConfigKeyValue.ContainsKey("ConnectionStringMySql"))
            //{
            //    create(ConfigKeyValue["ConnectionStringMySql"]);
            //}
            //else
            //{
            //    create("");
            //}
            create();
            //根据不同数据库引擎获取数据根路径
               GetDataAbsRootPath();
            }
            catch(Exception ex)
            {
                throw new Exception("Create:获取元数据异常", ex);
            }


        }


        public static void Create(Configuration cfg)
        {
            readConfig(cfg);
            //if (ConfigKeyValue.ContainsKey("ConnectionStringMySql"))
            //{
            //    create(ConfigKeyValue["ConnectionStringMySql"]);
            //}
            //else
            //{
            //    create("");
            //}
            create();
       
        }

        /// <summary>
        /// 获取所有TCP服务的远程对象
        /// </summary>
        public static void InitializeTcpConnection()
        {
            try
            {
                if(IdbOperating==null)
                {
                    IdbOperating = DbOperatingTCPClient.InitTCPClient_StorageChl(DbServerIp, dbUtilityTcpPort);
                }
                ITileDbUtilities = DbUtilitiesTCPClient.InitTCPClient_StorageChl("127.0.0.1", dbUtilityTcpPort);
                IGetDataService = QDB_GetDataTCPClient.InitTCPClient_StorageChl(DbServerIp, dbUtilityTcpPort);
                ISearcherDbServ = QDB_SearDbTCPClient.InitTCPClient_StorageChl(DbServerIp, dbUtilityTcpPort);
                ISearcherTileServ = QDB_SearTileTCPClient.InitTCPClient_StorageChl(DbServerIp, dbUtilityTcpPort);
                IdbServerUtilities = IdbOperating.GetSubDbUtilities(EnumDBType.MIDB);
                ServiceIsConnected = true;
            }
            catch (Exception e)
            {
                
                throw new Exception("InitializeTcpConnection：获取远程TCP服务异常", e);
            }

        }
        public static string GetIPAddress()
        {
            try
            {
                string st = "";
                try
                {
                    if (ConfigKeyValue.ContainsKey("UsingIP"))
                    {
                        st = ConfigKeyValue["UsingIP"].Trim();
                        if (st != "")
                        {
                            return st;
                        }
                    }
                }
                catch 
                {
                }
          
                ManagementClass mc = new ManagementClass("Win32_NetworkAdapterConfiguration");
                ManagementObjectCollection moc = mc.GetInstances();
                foreach (ManagementObject mo in moc)
                {
                    if ((bool)mo["IPEnabled"] == true)
                    {
                        //st=mo["IpAddress"].ToString(); 
                        System.Array ar;
                        ar = (System.Array)(mo.Properties["IpAddress"].Value);
                        st = ar.GetValue(0).ToString();
                        break;
                    }
                }
                moc = null;
                mc = null;
                if(st=="")
                {
                    throw new Exception("IP为null");
                }
                return st;
            }
            catch(Exception ex)
            {
                throw new Exception("unknow IP",ex);
            }
            finally
            {
            }

        }
    }
}
