using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using QRST_DI_Resources;
using QRST_DI_SS_DBInterfaces.IDBEngine;

namespace QRST_DI_MS_Basis.Log
{
    public class translog
    {
        //public static MySqlBaseUtilities sqlUtilities; 
         public static IDbBaseUtilities sqlUtilities;
        public translog(EnumLogType logtype)
        {
            TYPE = logtype.ToString();
        }
        public translog()
        {
        }
        #region Model
        private int _id;
        private string _type;
        private DateTime  _logtime;
        private string _ipaddress;
        private string _message;
        /// <summary>
        /// 
        /// </summary>
        public int ID
        {
            set { _id = value; }
            get { return _id; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string TYPE
        {
            set { _type = value; }
            get { return _type; }
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime LOGTIME
        {
            set { _logtime = value; }
            get { return _logtime; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string IPADDRESS
        {
            set { _ipaddress = value; }
            get { return _ipaddress; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string MESSAGE
        {
            set { _message = value; }
            get { return _message; }
        }

         public string ToString()
        {
          //  return string.Format("[{0}] {1} {2}",IPADDRESS,LOGTIME.ToString,MESSAGE);
            return string.Format("[{0}]", IPADDRESS) + " " + LOGTIME.ToString() +" " +MESSAGE;
        }

         /// <summary>
         /// 添加日志信息
         /// </summary>
         public virtual void Add(string message)
         {
             //TableLocker dblock = new TableLocker(sqlUtilities);
             //dblock.LockTable("translog");


             string sql = "";
             try
             {
                 message = message.Replace(@"\", @"//");
                 message = message.Replace(@"'", @"~");
                 string ip = Constant.UsingIPAddress;
                string nowTime = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss");
                 if (TYPE == EnumLogType.OrderLog.ToString())
                 {
                    sql = string.Format("insert into translog (TYPE,LOGTIME,IPADDRESS,MESSAGE,OrderCode)values ('{0}','{1}','{2}','{3}','{4}')", TYPE, DateTime.Now,ip, message, message.Substring(0, message.IndexOf(':')));
                    //sql = string.Format("insert into translog (TYPE,LOGTIME,IPADDRESS,MESSAGE,OrderCode)values ('{0}',datetime('now','+8 hour'),'{1}','{2}','{3}')", TYPE, ip, message, message.Substring(0, message.IndexOf(':')));
                     sqlUtilities.ExecuteSql(sql);
                 }
                 else
                 {
                    sql = string.Format("insert into translog (TYPE,LOGTIME,IPADDRESS,MESSAGE)values ('{0}','{1}','{2}','{3}')", TYPE,DateTime.Now, ip, message);
                    //sql = string.Format("insert into translog (TYPE,LOGTIME,IPADDRESS,MESSAGE)values ('{0}',datetime('now','+8 hour'),'{1}','{2}')", TYPE, ip, message);
                     sqlUtilities.ExecuteSql(sql);
                 }

             }
             catch (Exception ex)
             {
                 MyConsole.WriteLine("[translog Add Error]sql:" + sql + "###Exception:" + ex.Message);
             }



             //ID = sqlUtilities.GetMaxID("ID", "translog");
             ////获取本机IP
             //string hostname = Dns.GetHostName();
             //IPHostEntry hostEntry = Dns.GetHostByName(hostname);
             //IPADDRESS = hostEntry.AddressList[0].ToString();

             ////日志时间
             //_logtime = DateTime.Now;

             //StringBuilder strSql = new StringBuilder();
             //strSql.Append("insert into translog(");
             //strSql.Append("ID,TYPE,LOGTIME,IPADDRESS,MESSAGE)");
             //strSql.Append(" values (");
             //strSql.Append("@ID,@TYPE,@LOGTIME,@IPADDRESS,@MESSAGE)");
             //MySqlParameter[] parameters = {
             //       new MySqlParameter("@ID", MySqlDbType.Int32,7),
             //       new MySqlParameter("@TYPE", MySqlDbType.Text),
             //       new MySqlParameter("@LOGTIME", MySqlDbType.DateTime),
             //       new MySqlParameter("@IPADDRESS", MySqlDbType.Text),
             //       new MySqlParameter("@MESSAGE", MySqlDbType.Text)};
             //parameters[0].Value = ID;
             //parameters[1].Value = TYPE;
             //parameters[2].Value = LOGTIME;
             //parameters[3].Value = IPADDRESS;
             //parameters[4].Value = message;

             //sqlUtilities.ExecuteSql(strSql.ToString(), parameters);
             //dblock.UnlockTable("translog");
         }
        #endregion Model

#region  一些静态方法
         /// <summary>
         /// 根据条件获取相关日志信息
         /// </summary>
         /// <returns></returns>
         public static List<translog> GetList(string strWhere)
         {
             //int offset = int.Parse(sqlUtilities.GetDataSet(" select count(*) from translog").Tables[0].Rows[0][0].ToString())-100;
             //if (offset < 0)
             //    offset = 0;

             StringBuilder strSql = new StringBuilder();
             strSql.Append("select ID,TYPE,LOGTIME,IPADDRESS,MESSAGE ");
             strSql.Append(" FROM translog ");
             if (strWhere.Trim() != "")
             {
                 strSql.Append(" where " + strWhere);
             }
             strSql.Append(" order by LOGTIME DESC");
            strSql.AppendFormat(" limit {0},100",0);
             DataSet ds = sqlUtilities.GetDataSet(strSql.ToString());

             List<translog> logLst = new List<translog>();
             for (int i = 0 ; i < ds.Tables[0].Rows.Count ;i++ )
             {
                 translog model = new translog();

                 if (ds.Tables[0].Rows[i]["ID"].ToString() != "")
                 {
                     model.ID = int.Parse(ds.Tables[0].Rows[i]["ID"].ToString());
                 }
                 model.TYPE = ds.Tables[0].Rows[i]["TYPE"].ToString();
                 if (ds.Tables[0].Rows[i]["LOGTIME"].ToString() != "")
                 {
                     model.LOGTIME = DateTime.Parse(ds.Tables[0].Rows[i]["LOGTIME"].ToString());
                 }
                 model.IPADDRESS = ds.Tables[0].Rows[i]["IPADDRESS"].ToString();
                 model.MESSAGE = ds.Tables[0].Rows[i]["MESSAGE"].ToString();
                 logLst.Add(model);
             }
             return logLst;
         }

         public static void DeleteLogs(string strWhere)
         {
             StringBuilder strSql = new StringBuilder();
             strSql.Append("delete FROM translog ");
             if (strWhere.Trim() != "")
             {
                 strSql.Append(" where " + strWhere);
             }
             sqlUtilities.ExecuteSql(strSql.ToString());
         }

#endregion

    }
}
