using System.Configuration;

namespace QDB_DBOperation_Dll.Common
{
    public class Constant
    {
        /// <summary>
        /// 获取App.config里配置项的hadoop数据阵列存储路径  
        /// </summary>
        public static string DataStorePath
        {
            get
            {
                return ConfigurationManager.AppSettings["DataStorePath"];
            }
        }

        /// <summary>
        /// 获取App.config里配置项的行业代号
        /// </summary>
        public static string INDUSTRYCODE
        {
            get
            {
                return ConfigurationManager.AppSettings["INDUSTRYCODE"];
            }
        }

        /// <summary>
        /// 获取App.config里配置项的数据库连接字符串  
        /// </summary>
        public static string ConnectionString
        {
            get
            {
                return ConfigurationManager.AppSettings["ConnectionString"];
            }
        }

        /// <summary>
        /// 获取App.config里配置项的数据库连接字符串(Oracle版)
        /// </summary>
        public static string ConnectionStringOracle
        {
            get
            {
                return ConfigurationManager.AppSettings["ConnectionStringOracle"];
            }
        }

        /// <summary>
        /// 获取App.config里配置项的数据库连接字符串(MySql版)
        /// </summary>
        public static string ConnectionStringMySql
        {
            get
            {
                return ConfigurationManager.AppSettings["ConnectionStringMySql"];
            }
        }

        /// <summary>
        /// 获取App.config里配置项的实验验证数据库连接字符串(MySql版)
        /// </summary>
        public static string ConnectionStringEVDB
        {
            get
            {
                return ConfigurationManager.AppSettings["ConnectionStringEVDB"];
            }
        }

        /// <summary>
        /// 获取App.config里配置项的基础空间数据库连接字符串(MySql版)
        /// </summary>
        public static string ConnectionStringBSDB
        {
            get
            {
                return ConfigurationManager.AppSettings["ConnectionStringBSDB"];
            }
        }


        /// <summary>
        /// 预处理系统所在IP地址
        /// </summary>
        public static string CorrectRecieveIP
        {
            get
            {
                return ConfigurationManager.AppSettings["CorrectRecieveIP"];
            }
        }

        /// <summary>
        /// 本机使用IP地址
        /// </summary>
        public static string UsingIPAddress
        {
            get
            {
                return ConfigurationManager.AppSettings["UsingIPAddress"];
            }
        }
        /// <summary>
        /// ftp用户名
        /// </summary>
        public static string FtpUserName
        {
            get
            {
                return ConfigurationManager.AppSettings["DB_FtpUserName"];
            }
        }
        /// <summary>
        /// ftp密码
        /// </summary>
        public static string FtpPassword
        {
            get
            {
                return ConfigurationManager.AppSettings["DB_FtpPassword"];
            }
        }

        public static string ListenPort
        {
            get
            {
                return ConfigurationManager.AppSettings["DB_ListenPort"];
            }
        }
        /// <summary>
        /// 消息监听端口
        /// </summary>
        public static string NoticeListenPort
        {
            get
            {
                return ConfigurationManager.AppSettings["NoticeListenPort"];
            }
        }
        /// <summary>
        /// 消息发送端口
        /// </summary>
        public static string NoticeSendPort
        {
            get
            {
                return ConfigurationManager.AppSettings["NoticeSendPort"];
            }
        }
        /// <summary>
        /// 存储站点（SS）TCP通道端口
        /// </summary>
        public static string TcpSSPort
        {
            get
            {
                return ConfigurationManager.AppSettings["TcpSSPort"];
            }
        }
        /// <summary>
        /// 服务台IP地址
        /// </summary>
        public static string ConsoleServerIP
        {
            get
            {
                return ConfigurationManager.AppSettings["ConsoleServerIP"];
            }
        }
        /// <summary>
        /// 服务台TCP通道端口
        /// </summary>
        public static string TCPConsolePort
        {
            get
            {
                return ConfigurationManager.AppSettings["TCPConsolePort"];
            }
        }
        /// <summary>
        /// 站点响应最低阈值（单位ms）
        /// </summary>
        public static string SSResponseThreshold
        {
            get
            {
                return ConfigurationManager.AppSettings["SSResponseThreshold"];
            }
        }
        /// <summary>
        /// 监控系统IP
        /// </summary>
        public static string MonitorIP
        {
            get
            {
                return ConfigurationManager.AppSettings["MonitorIP"];
            }
        }
        /// <summary>
        /// 切片程序所在路径
        /// </summary>
        public static string ImageCutExePath
        {
            get
            {
                return ConfigurationManager.AppSettings["ImageCutExePath"];
            }
        }

        /// <summary>
        /// 应用中心系统标识
        /// </summary>
        public static string ServerType_IsCenter
        {
            get
            {
                return ConfigurationManager.AppSettings["ServerType_IsCenter"];
            }
        }
        /// <summary>
        /// 切片程序所在路径
        /// </summary>
        public static string WS_QDB_GetData
        {
            get
            {
                return ConfigurationManager.AppSettings["WS_QDB_GetData"];
            }
        }

        /// <summary>
        /// 判断是否调用河大的通信的EXE程序
        /// </summary>

        public static string GetHenuEXE
        {
            get
            {
                return ConfigurationManager.AppSettings["HenuTel"];
            }
        }

        /// <summary>
        /// 返回部署好的hadoop云存储站点IP
        /// </summary>
        public static string DeployedHadoopIP
        {
            get
            {
                return ConfigurationManager.AppSettings["DeployedHadoopIP"];
            }
        }

        //添加日期：20130326 集成共享端发送消息终结点名称 
        public static string JcgxEndPointName
        {
            get
            {
                return ConfigurationManager.AppSettings["endPointName"];
            }
        }

        //添加日期：20130412 调用数据总线的url
        public static string DataBusServerUrl
        {
            get
            {
                return ConfigurationManager.AppSettings["DataBusServerUrl"];
            }
        }

        //添加日期：20130415 缩略图存放的文件服务器
        public static string FileServerIP
        {
            get
            {
                return ConfigurationManager.AppSettings["FileServerIP"];
            }
        }
    }
}
