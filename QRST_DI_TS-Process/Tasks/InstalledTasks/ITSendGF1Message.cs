using System;
using QRST_DI_MS_Basis;
using System.Data;
using System.Data.Common;
using QRST_DI_SS_Basis;
using QRST_DI_SS_Basis.MetaData;
using QRST_DI_SS_DBInterfaces.IDBEngine;
using QRST_DI_Resources;

namespace QRST_DI_TS_Process.Tasks.InstalledTasks
{
    class ITSendGF1Message : TaskClass
    {
        //public DBMySqlOperating sqloperating = new DBMySqlOperating();
        private static IDbOperating slLiteOperating = Constant.IdbOperating;
        private IDbBaseUtilities sqLiteBaseUtilities;
        /// <summary>
        /// 任务名,定义唯一标识，不可动态修改
        /// </summary>
        public override string TaskName
        {
            get { return "ITSendGF1Message"; }
            set { }
        }

        public override void Process()
        {
            //System.Threading.Tasks.Task task = new System.Threading.Tasks.Task(new Action<object>(sendmessge),(object)ProcessArgu);
            //task.Start();
            if (ReferenceTheInstances.MessageNoticer == null)
            {
                ReferenceTheInstances.MessageNoticer = new NoticeMessager();
            }
            string msg1=msg();
            string ip1=SendMessageIp();
            if(IfSendMessage() && msg1!="" && ip1!="")
            {
                ReferenceTheInstances.MessageNoticer.SendMessage(ip1, msg1);
            }
        }


        public string msg()
        {
            string message = "";
            string sql = string.Format(@"select Name,DATALOWERLEFTLAT,DATALOWERLEFTLONG,DATALOWERRIGHTLAT,DATALOWERRIGHTLONG,DATAUPPERRIGHTLAT,DATAUPPERRIGHTLONG,DATAUPPERLEFTLAT,DATAUPPERLEFTLONG,QRST_CODE from prod_gf1 ORDER BY ID DESC LIMIT 1");
            string MySqlCon = QRST_DI_Resources.Constant.ConnectionStringEVDB;
            sqLiteBaseUtilities = slLiteOperating.GetSubDbUtilities(EnumDBType.EVDB);
            try
            {
                DbDataReader reader1;
                using (reader1 = sqLiteBaseUtilities.ExecuteReader(sql))
                {
                    while (reader1.Read())
                    {
                        message = string.Format("{0}|{1}|{2}|{3}|{4}|{5}|{6}|{7}|{8}|{9}", reader1[0].ToString(),
                                reader1[1].ToString(), reader1[2].ToString(), reader1[3].ToString(), reader1[4].ToString(),
                                reader1[5].ToString(), reader1[6].ToString(), reader1[7].ToString(), reader1[8].ToString(),
                                reader1[9].ToString());
                    }
                }
                reader1.Close();
                return message;
            }
            catch (Exception)
            {
                return "";
            }

            //MySqlConnection con = new MySqlConnection(MySqlCon);
            //try
            //{
            //    con.Open();
            //    MySqlCommand cmd = new MySqlCommand(sql, con);
            //    using (MySqlDataReader reader1 = cmd.ExecuteReader())
            //    {
            //        while (reader1.Read())
            //        {
            //            message = string.Format("{0}|{1}|{2}|{3}|{4}|{5}|{6}|{7}|{8}|{9}", reader1[0].ToString(),
            //                reader1[1].ToString(), reader1[2].ToString(), reader1[3].ToString(), reader1[4].ToString(),
            //                reader1[5].ToString(), reader1[6].ToString(), reader1[7].ToString(), reader1[8].ToString(),
            //                reader1[9].ToString());
            //        }
            //    }
            //    con.Close();
            //    return message;
            //}
            //catch
            //{
            //    if (con.State == ConnectionState.Open)
            //    {
            //        con.Close();
            //    }
            //    return "";
            //}
        }

        public bool IfSendMessage()
        {
            try
            {
                string sql = string.Format(@"select value from appsettings where key='IfSendMessage'");
                sqLiteBaseUtilities = slLiteOperating.GetSubDbUtilities(EnumDBType.MIDB);
                string ifsend = sqLiteBaseUtilities.myExcuteScalar(sql);
                if (ifsend == "true")
                    return true;
                else
                    return false;
            }
            catch
            {
                return false;
            }
        }

        public string SendMessageIp()
        {
            try
            {
                string sql = string.Format(@"select value from appsettings where key='SendMessageIP'");
                sqLiteBaseUtilities = slLiteOperating.GetSubDbUtilities(EnumDBType.MIDB);
                return sqLiteBaseUtilities.myExcuteScalar(sql);
            }
            catch
            {
                return "";
            }
        }
    }
}
