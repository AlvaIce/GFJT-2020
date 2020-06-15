using System;
using System.Configuration;
using MySql.Data.MySqlClient;

namespace DataPrepare
{
    public class Constant
    {
        public static MySqlConnection con = null;
        /// <summary>
        /// 获取App.config里配置项的MySql连接字符串
        /// </summary>
        public static string ConnectionStringMySql
        {
            get
            {
                return ConfigurationManager.AppSettings["ConnectionStringMySql"];
            }
        }

        public static string ConnectionMIDBStringMySql
        {
            get
            {
                return ConfigurationManager.AppSettings["ConnectionMIDBStringMySql"];
            }
        }
        //zsm 20151021
        //public static string ConnectionStringMySqlEVDB
        //{
        //    get
        //    {
        //        return ConfigurationManager.AppSettings["ConnectionStringMySqlEVDB"];
        //    }
        //}


        public static string GetFromAppSettingTable(string p)
        {
            string v = "";
            if (con == null)
            {
                con = new MySqlConnection(ConnectionMIDBStringMySql);
            }
            con.Open();
            string sql = string.Format(@"select value from appsettings where `key`='{0}'", p);
            MySqlCommand cmd = new MySqlCommand(sql, con);
            v = Convert.ToString(cmd.ExecuteScalar());
            con.Close();
            return v;
        }

        #region MessageCentorIP
        /// <summary>
        /// 获取App.config里配置项的MessageCentorIP消息中心IP
        /// </summary>
        public static string MessageCentorIP
        {
            get
            {
                return ConfigurationManager.AppSettings["MessageCentorIP"];
            }
        }
        #endregion


        /// <summary>
        /// 获取App.config里配置项的MessageCentorPort消息中心端口号
        /// </summary>
        public static string MessageCentorPort
        {
            get
            {
                return ConfigurationManager.AppSettings["MessageCentorPort"];
            }
        }


        /// <summary>
        /// 获取App.config里配置项的NeedOutput，内外环境（新疆），标识是否要求瓦片导出到外网
        /// </summary>
        public static string NeedOutput
        {
            get
            {
                return ConfigurationManager.AppSettings["NeedOutput"];
            }
        }

        /// <summary>
        /// 是否将App移动关注的瓦片推送过去
        /// </summary>
        public static string Need2App
        {
            get
            {
                string Need2App = "false";
                Need2App = GetFromAppSettingTable("IfSendMessage");

                return Need2App; //APPSetting 表获取
            }
        }

        /// <summary>
        /// App移动瓦片交互共享文件夹
        /// </summary>
        public static string AppTilePath
        {
            get
            {
                string AppTilePath = "";
                AppTilePath = GetFromAppSettingTable("AppTileSharedPath");

                return AppTilePath; //APPSetting 表获取
            }
        }

        /// <summary>
        /// 打包瓦片数据交互共享文件夹
        /// </summary>
        public static string zipdata
        {
            get
            {
                string zipdata = "";
                zipdata = GetFromAppSettingTable("websharefolder");

                return zipdata; //APPSetting 表获取
            }
        }

        /// <summary>
        /// 获取App.config里配置项的OutputTilePath，如果NeedOutput为true，设置导出的本地路径
        /// </summary>
        public static string OutputTilePath
        {
            get
            {
                return ConfigurationManager.AppSettings["OutputTilePath"];
            }
        }

        /// <summary>
        /// 获取App.config里配置中的EVDB的数据库连接字符串
        /// </summary>
        public static string ConnectionEVDBStringMySql
        {
            get
            {
                if (con == null)
                {
                    con = new MySqlConnection(ConnectionMIDBStringMySql);
                }
                con.Open();
                string sql = string.Format(@"select ConnectStr from subdbinfo where `name`='{0}'", "EVDB");
                MySqlCommand cmd = new MySqlCommand(sql, con);
                string result = Convert.ToString(cmd.ExecuteScalar());
                con.Close();
                return result;
            }
        }

        /// <summary>
        /// 获取App.config里配置中的ISDB的数据库连接字符串
        /// </summary>
        public static string ConnectionISDBStringMySql
        {
            get
            {
                if (con == null)
                {
                    con = new MySqlConnection(ConnectionMIDBStringMySql);
                }
                con.Open();
                string sql = string.Format(@"select ConnectStr from subdbinfo where `name`='{0}'", "ISDB");
                MySqlCommand cmd = new MySqlCommand(sql, con);
                string result = Convert.ToString(cmd.ExecuteScalar());
                con.Close();
                return result;
            }
        }


        /// <summary>
        /// 获取App.config里配置项的SystemName
        /// </summary>
        public static string SystemName
        {
            get
            {
                return ConfigurationManager.AppSettings["SystemName"];
            }
        }
    }
}
