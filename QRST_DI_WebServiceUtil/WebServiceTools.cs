using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using System.Xml;
using QRST_DI_Resources;
using QRST_DI_SS_Basis;
using QRST_DI_SS_Basis.MetadataQuery;
using QRST_DI_TS_Basis.DirectlyAddress;
using QRST_DI_TS_Basis.Search;
 
namespace QRST_DI_WebServiceUtil
{
    /// <summary>
    ///WebServiceUtil 的摘要说明
    /// </summary>
    public class WebServiceTools
    {
        public static List<DataSet> DtCol = new List<DataSet>();

        public XmlElement xml = null;
        public string exceptions = string.Empty;
        public int allrecordNum = 0;
        SQLBaseTool sqltool;
        //static ConsoleServer_IPCService _ipcServ;
        //public static ConsoleServer_IPCService ipcServ
        //{
        //    get {

        //        if (ipcServ == null)
        //        {
        //            ipcServ = ConsoleServer_IPCService.InitIPCWebServiceClient_ConsoleChl();

        //        }
        //        return _ipcServ;
        //    }
        //}
    
        public WebServiceTools()
        {
            //
            //TODO: 在此处添加构造函数逻辑
       
            //
            //DbOperator = new MySqlBaseUtilities();
            sqltool = new SQLBaseTool();
        
        }
        //从Oralce中获取IP
        public string[] GetIPFromMySql()
        {
            DataSet IPDateSet = DirectlyAddressingIPMod.GetTileDsMod();
            int tbl = IPDateSet.Tables[0].Rows.Count;
            string[] s = new string[tbl];
            for (int i = 0; i < tbl; i++)
                s[i] = IPDateSet.Tables[0].Rows[i][0].ToString();
            return s;
        }
        //public string GettilepathBaseIP(string ip)
        //{
        //    string sql = string.Format("select CommonSharePath from db02.db02_sitesinfo where addressip = '{0}'", ip);
        //    DataSet ds = DbOperator.GetDataSet(sql);
        //    string tilepath = ds.Tables[0].Rows[0][0].ToString();
        //    return tilepath;
        //}

        public DataSet invokSearchMySQLWebService(List<string> ips, string _url, string nmspc, string classname, string methodname, List<object> args)
        { return invokSearchMySQLWebService(ips, _url, nmspc, classname, methodname, args, false); }
        public DataSet invokSearchMySQLWebService(List<string> ips,string _url, string nmspc, string classname, string methodname, List<object> args,bool isTile)
        {
            //ipcServ.UpdateOptimalHostList();

            //List<string> IPcol = new List<string>();
            //IPcol = ipcServ.GetHostIPList();
            List<string> IPcol = WebServiceTools.getLocalAndCenterIP();


            List<Task> tasks = new List<Task>();

            DataSet ds = new DataSet();
            foreach (string ip in ips)
            {
                if (IPcol.Contains(ip))
                {
                    string webserviceUR = string.Format(@"http://{0}/{1}", ip, _url);
                    //string ip2 = "172.16.0.185";
                    //webserviceUR = string.Format(@"http://{0}/WS_QDB_Searcher_Sqlite/Service.asmx", ip2);//仅调试主服务器
                    List<object> para = new List<object>();
                    //string url, string @namespace, string classname, string methodname, object[] args
                    para.Add(ip);
                    para.Add(webserviceUR);
                    para.Add(nmspc);
                    para.Add(classname);
                    para.Add(methodname);
                    foreach (object obj in args)
                    {
                        para.Add(obj);
                    }
                    //ExecusionWebService(para);
                    Task t = new Task(o => ExecusionWebService((List<object>)o), para);
                    tasks.Add(t);
                    t.Start();
                }
            }
            foreach (Task t in tasks)
            {
                t.Wait();
            }
            ds = GetAllDataSet(WebServiceTools.DtCol, isTile);
            return ds;
        }
        // DLF 20131125添加
        public DataSet invokSearchMySQLWSCustom(List<string> ips, string _url, string nmspc, string classname, string methodname, List<object> args)
        {
            allrecordNum = 0;
            //List<string> IPcol = new List<string>();
            //IPcol = ipcServ.GetHostIPList();
            List<string> IPcol = WebServiceTools.getLocalAndCenterIP();


            List<Task> tasks = new List<Task>();

            DataSet ds = new DataSet();
            foreach (string ip in ips)
            {
                if (IPcol.Contains(ip))
                {
                    string webserviceUR = string.Format(@"http://{0}/{1}", ip, _url);
                    //string ip2 = "172.16.0.185";
                    //webserviceUR = string.Format(@"http://{0}/WS_QDB_Searcher_Sqlite/Service.asmx", ip2);//仅调试主服务器
                    List<object> para = new List<object>();
                    //string url, string @namespace, string classname, string methodname, object[] args
                    para.Add(ip);
                    para.Add(webserviceUR);
                    para.Add(nmspc);
                    para.Add(classname);
                    para.Add(methodname);
                    foreach (object obj in args)
                    {
                        para.Add(obj);
                    }
                    //ExecusionWebService(para);
                    Task t = new Task(o => ExecusionWebService((List<object>)o), para);
                    tasks.Add(t);
                    t.Start();
                }
            }
            foreach (Task t in tasks)
            {
                t.Wait();
            }
            ds = GetAllDataSet(WebServiceTools.DtCol);
            return ds;
        }

        // DLF 20131202添加
        public XmlElement invokSearchMySQLWSXml(List<string> ips, string _url, string nmspc, string classname, string methodname, List<object> args)
        {
            //List<string> IPcol = new List<string>();
            //IPcol = ipcServ.GetHostIPList();
            List<string> IPcol = WebServiceTools.getLocalAndCenterIP();


            List<Task> tasks = new List<Task>();

            XmlElement xmlEle;
            foreach (string ip in ips)
            {
                if (IPcol.Contains(ip))
                {
                    string webserviceUR = string.Format(@"http://{0}/{1}", ip, _url);
                    //string ip2 = "172.16.0.185";
                    //webserviceUR = string.Format(@"http://{0}/WS_QDB_Searcher_Sqlite/Service.asmx", ip2);//仅调试主服务器
                    List<object> para = new List<object>();
                    //string url, string @namespace, string classname, string methodname, object[] args
                    para.Add(ip);
                    para.Add(webserviceUR);
                    para.Add(nmspc);
                    para.Add(classname);
                    para.Add(methodname);
                    foreach (object obj in args)
                    {
                        para.Add(obj);
                    }
                    //ExecusionWebService(para);
                    Task t = new Task(o => ExecusionWebServiceXML((List<object>)o), para);
                    tasks.Add(t);
                    t.Start();
                }
            }
            foreach (Task t in tasks)
            {
                t.Wait();
            }
            xmlEle = this.xml;
            return xmlEle;
        }

        private void ExecusionWebService(List<object> paraobjs)
        {
            DataSet ds = new DataSet();

            //public static object InvokeWebservice(string url, string @namespace, string classname, string methodname, object[] args)
            List<object> objs = new List<object>();
            for (int i = 5; i < paraobjs.Count; i++)
            {
                objs.Add(paraobjs[i]);
            }
            try
            {
                ds = WebServiceUtil.InvokeWebservice(paraobjs[1].ToString(), paraobjs[2].ToString(), paraobjs[3].ToString(), paraobjs[4].ToString(), objs.ToArray()) as DataSet;

                // ds = sqltool.AddDataHostInfo(ds, paraobjs[0].ToString());
            }
            catch (Exception)
            {
                return;
            }

            DtCol.Add(ds);
        }

        // DLF 20131125添加
        private void ExecusionWebServiceCustom(List<object> paraobjs)
        {
            QueryResponse queryResponse = new QueryResponse();

            //public static object InvokeWebservice(string url, string @namespace, string classname, string methodname, object[] args)
            List<object> objs = new List<object>();
            for (int i = 5; i < paraobjs.Count; i++)
            {
                objs.Add(paraobjs[i]);
            }
            try
            {
                object obj=WebServiceUtil.InvokeWebservice(paraobjs[1].ToString(), paraobjs[2].ToString(), paraobjs[3].ToString(), paraobjs[4].ToString(), objs.ToArray()) ;
                //queryResponse = obj as QueryResponse;

                //WSlocalMysql.QueryResponse response = obj as WSlocalMysql.QueryResponse;
                // ds = sqltool.AddDataHostInfo(ds, paraobjs[0].ToString());
            }
            catch (Exception)
            {
                return;
            }
            this.exceptions += queryResponse.exception;
            this.allrecordNum += queryResponse.totalRecordCount;
            DtCol.Add(queryResponse.recordSet);
        }
        //DLF 20131202添加
        private void ExecusionWebServiceXML(List<object> paraobjs)
        {
            List<object> objs = new List<object>();
            for (int i = 5; i < paraobjs.Count; i++)
            {
                objs.Add(paraobjs[i]);
            }
            try
            {
                object ob=WebServiceUtil.InvokeWebservice(paraobjs[1].ToString(), paraobjs[2].ToString(), paraobjs[3].ToString(), paraobjs[4].ToString(), objs.ToArray());
                this.xml = ob as XmlElement;
                // ds = sqltool.AddDataHostInfo(ds, paraobjs[0].ToString());
            }
            catch (Exception)
            {
                return;
            }
        
        }
        /// <summary>
        /// 将List融合成一个DataSet
        /// </summary>
        /// <param name="allDtCol">所有检索返回的List</param>
        /// <returns></returns>
        public DataSet GetAllDataSet(List<DataSet> allDtCol)
        {
            return GetAllDataSet(allDtCol,false);
        }
        public DataSet GetAllDataSet(List<DataSet> allDtCol,bool isTile)
        {
            int allrec = 0;
            DataSet allDS = new DataSet();
            if (allDtCol.Count != 0)
            {
                foreach (DataSet ds in allDtCol)
                {
                    if (isTile&&ds!=null&&ds.Tables.Count>0&&ds.Tables[0].Rows.Count>0)
                    {
                        allrec = allrec + int.Parse(ds.Tables[0].Rows[0]["ID"].ToString());
                        ds.Tables[0].Rows.RemoveAt(0);
                    }
                    allDS.Merge(ds);

                }

                if (isTile && allDS != null && allDS.Tables.Count > 0)
                {
                    DataRow dr = allDS.Tables[0].NewRow();

                    dr["ID"] = allrec;
                    //ds.Tables[0].Rows.Add(dr);
                    allDS.Tables[0].Rows.InsertAt(dr, 0);
                }
            }
            return allDS;

        }


        #region  暂时无用
        /// <summary>
        /// 返回位置信息的SQL语句
        /// </summary>
        /// <param name="e"></param>
        /// <param name="w"></param>
        /// <param name="s"></param>
        /// <param name="n"></param>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public string getPosition(string e, string w, string s, string n, string tableName, string southColumn, string northColumn, string eastColumn, string westColumn)
        {
            string sqlString = "";
            Decimal west = Convert.ToDecimal(w);
            Decimal east = Convert.ToDecimal(e);
            Decimal south = Convert.ToDecimal(s);
            Decimal north = Convert.ToDecimal(n);
            sqlString = string.Format("NAME NOT IN (SELECT NAME FROM {0} WHERE {5} < {1}  or {6} >  {2} or {7} > {3}or {8} <  {4} ) ", tableName, south, north, east, west, southColumn, northColumn, eastColumn, westColumn);
            return sqlString;
        }
        /// <summary>
        /// 获取时间的SQL查询信息
        /// </summary>
        /// <param name="begintime"></param>
        /// <param name="endtime"></param>
        /// <param name="beginColumn"></param>
        /// <returns></returns>
        public string getTime(string begintime, string endtime, string beginColumn)
        {
            string sqlString = "";
            DateTime mintime = Convert.ToDateTime(begintime);
            DateTime maxtime = Convert.ToDateTime(endtime);

            sqlString = string.Format("{2} between to_date('{0}','yyyy/mm/dd hh24:mi:ss') and to_date('{1}','yyyy/mm/dd hh24:mi:ss')", mintime, maxtime, beginColumn);
            return sqlString;
        }
        /// <summary>
        /// 返回是否有时间信息
        /// </summary>
        /// <returns></returns>
        public bool Istime(string dateTimetext,string datetimeEndtext )
        {
            bool Time = false;
            if (dateTimetext != "" && datetimeEndtext != "")
            {
                Time = true;
            }
            else
            {
                Time = false;
            }
            return Time;
        }
        /// <summary>
        /// 判断是否有位置信息      
        /// </summary>
        /// <returns></returns>
        public bool IsPositionNull(string etext,string wtext,string stext,string ntext)
        {
            bool Position = false;
            if (etext != "" && wtext != "" && stext != "" && ntext != "")
            {
                Position = true;
            }
            else
            {
                Position = false;
            }
            return Position;
        }
        #endregion

        /// <summary>
        /// 获取本机及云中心IP组成的列表
        /// </summary>
        /// <returns></returns>
        public static List<string> getLocalAndCenterIP()
        {
            List<string> IpCol = new List<string>();

            string localIP = Constant.UsingIPAddress;
            string CCIP = Constant.CloudCenterIPAddress;

            if (string.IsNullOrEmpty(localIP) || string.IsNullOrEmpty(CCIP))
            {
                return IpCol;
            }
            else
            {
                IpCol.Add(localIP);
                IpCol.Add(CCIP);
            }
            return IpCol;
        }
        /// <summary>
        /// 获取云中心IP
        /// </summary>
        /// <returns></returns>
        public static List<string> getCenterIP()
        {
            List<string> IpCol = new List<string>();

            string CCIP = Constant.CloudCenterIPAddress;

            if (string.IsNullOrEmpty(CCIP))
            {
                return IpCol;
            }
            else
            {
                IpCol.Add(CCIP);
            }
            return IpCol;
        }
    }
}