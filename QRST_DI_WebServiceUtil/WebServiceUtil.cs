using System;
using System.Collections.Generic;
using System.Data;
using System.Web.Services.Description;
using DotSpatial.Topology;
using QRST_DI_Resources;
using QRST_DI_SS_Basis;
using QRST_DI_SS_Basis.MetaData;
using QRST_DI_SS_DBClient.TCP;
using QRST_DI_SS_DBInterfaces.IDBEngine;
using QRST_DI_SS_DBInterfaces.IDBService;
using QRST_DI_TS_Basis.DirectlyAddress;
using QRST_DI_TS_Basis.Search;
using QRST_DI_SS_Basis.TileSearch;
 
namespace QRST_DI_WebServiceUtil
{
    public class WebServiceUtil
    {
        public static List<DataSet> allDtCol = new List<DataSet>();
        public static List<ModIDSearchInfo> QRSTModInfo = new List<ModIDSearchInfo>();
        public int allrecordNum = 0;
        private Object obj = new object();
        public static Dictionary<string, ITCPService> _dicTSSTCPServ;

        public WebServiceUtil()
        {
            allDtCol = new List<DataSet>();
            QRSTModInfo = new List<ModIDSearchInfo>();
            allrecordNum = 0;
        }

        public static object InvokeWebservice(string url, string @namespace, string classname, string methodname, object[] args)
        {
            try
            {
                System.Net.WebClient wc = new System.Net.WebClient();
                System.IO.Stream stream = wc.OpenRead(url + "?WSDL");
                System.Web.Services.Description.ServiceDescription sd = System.Web.Services.Description.ServiceDescription.Read(stream);
                System.Web.Services.Description.ServiceDescriptionImporter sdi = new System.Web.Services.Description.ServiceDescriptionImporter();
                sdi.AddServiceDescription(sd, "", "");
                System.CodeDom.CodeNamespace cn = new System.CodeDom.CodeNamespace(@namespace);
                System.CodeDom.CodeCompileUnit ccu = new System.CodeDom.CodeCompileUnit();
                ccu.Namespaces.Add(cn);
                ServiceDescriptionImportWarnings warning = sdi.Import(cn, ccu);

                Microsoft.CSharp.CSharpCodeProvider csc = new Microsoft.CSharp.CSharpCodeProvider();
                System.CodeDom.Compiler.ICodeCompiler icc = csc.CreateCompiler();

                System.CodeDom.Compiler.CompilerParameters cplist = new System.CodeDom.Compiler.CompilerParameters();
                cplist.GenerateExecutable = false;
                cplist.GenerateInMemory = true;
                cplist.ReferencedAssemblies.Add("System.dll");
                cplist.ReferencedAssemblies.Add("System.XML.dll");
                cplist.ReferencedAssemblies.Add("System.Web.Services.dll");
                cplist.ReferencedAssemblies.Add("System.Data.dll");

                System.CodeDom.Compiler.CompilerResults cr = icc.CompileAssemblyFromDom(cplist, ccu);
                if (true == cr.Errors.HasErrors)
                {
                    System.Text.StringBuilder sb = new System.Text.StringBuilder();
                    foreach (System.CodeDom.Compiler.CompilerError ce in cr.Errors)
                    {
                        sb.Append(ce.ToString());
                        sb.Append(System.Environment.NewLine);
                    }
                    throw new Exception(sb.ToString());
                }
                System.Reflection.Assembly assembly = cr.CompiledAssembly;
                Type t = assembly.GetType(@namespace + "." + classname, true, true);
                object obj = Activator.CreateInstance(t);
                System.Reflection.MethodInfo mi = t.GetMethod(methodname);
                return mi.Invoke(obj, args);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.InnerException.Message, new Exception(ex.InnerException.StackTrace));
            }
        }

        public static ITCPService GetTServerSiteTCPService(string ip)
        {
            if (_dicTSSTCPServ == null)
            {
                _dicTSSTCPServ = new Dictionary<string, ITCPService>();
            }
            if (_dicTSSTCPServ.ContainsKey(ip) && _dicTSSTCPServ[ip] != null && _dicTSSTCPServ[ip].IsRunning)
            {
                return _dicTSSTCPServ[ip];
            }
            else
            {
                return TileSiteTCPServiceClient.InitTCPClient_StorageChl(ip, Constant.dbUtilityTcpPort);
            }

        }

        public string[] GetIPFromMySql()
        {
            DataSet IPDateSet = DirectlyAddressingIPMod.GetTileDsMod();
            int tbl = IPDateSet.Tables[0].Rows.Count;
            string[] s = new string[tbl];
            for (int i = 0; i < tbl; i++)
                s[i] = IPDateSet.Tables[0].Rows[i][0].ToString();
            return s;
        }
        private DirectlyAddressing daUtil = null;
        public DirectlyAddressing _DAUtil
        {
            get
            {
                if (daUtil == null)
                {
                    daUtil = new DirectlyAddressing(DirectlyAddressingIPMod.IPModDataSet);
                }
                return daUtil;
            }
        }

        /// <summary>
        /// 查找目标配号的索引文件路径
        /// </summary>
        /// <param name="modId">8</param>
        /// <returns></returns>
        public string getSqlitedbfilepathByRowCol(string row, string col)
        {
            int mod = _DAUtil.GetStorageIPMod(Convert.ToInt16(row), Convert.ToInt16(col));
            return getSqlitedbfilepathByMod(mod.ToString());
        }
        /// <summary>
        /// 查找目标配号的索引文件路径
        /// </summary>
        /// <param name="modId">8</param>
        /// <returns></returns>
        public string getSqlitedbfilepathByMod(string modId)
        {
            string Sqlitedbfilepath = "-1";

            string ip = _DAUtil.GetIPbyMod(modId);
            if (ip == "-1")
            {
                return "-1";
            }
            //增加单机本地路径存储   jianghua
            switch (Constant.DbStorage)
            {
                case EnumDbStorage.MULTIPLE:
                    string commonsharepath = GetCommonSharePathBaseIP(ip);
                    Sqlitedbfilepath = string.Format(@"{0}\QRST_DB_Tile\{1}\QDB_IDX_{1}.db", commonsharepath, modId);
                    break;
                case EnumDbStorage.SINGLE:
                    Sqlitedbfilepath = string.Format(@"{0}\QRST_DB_Tile\{1}\QDB_IDX_{1}.db", Constant.PcDBRootPath,
                        modId);

                    break;
                case EnumDbStorage.CLUSTER:
                    break;
            }
            return Sqlitedbfilepath;
        }

        public string GetCommonSharePathBaseIP(string ip)
        {
            IDbBaseUtilities iBaseUtilities = Constant.IdbOperating.GetSubDbUtilities(EnumDBType.MIDB);
            string sql = string.Format("select CommonSharePath from tileserversitesinfo where addressip = '{0}'", ip);
            DataSet ds = iBaseUtilities.GetDataSet(sql);
            string tilepath = ds.Tables[0].Rows[0][0].ToString();
            return tilepath;
        }
        /// <summary>
        /// 获取各站点数据记录
        /// </summary>
        /// <param name="ipandsql"></param>
        public void InputAndOutput(List<string> ipandsql)
        {
            int recordNum = 0;
            List<DataSet> DtCol = new List<System.Data.DataSet>();
            try
            {
                ITCPService tcpService = WebServiceUtil.GetTServerSiteTCPService(ipandsql[1]);//ipandsql[1]为IP

                DtCol = tcpService.GetDataSetCol(ipandsql[0], Convert.ToInt32(ipandsql[2]), out recordNum, ipandsql[3], Convert.ToInt32(ipandsql[4]));//ipandsql[1]为sql

            }
            catch
            {
                recordNum = 0;
                DtCol.Clear();
            }
            lock (obj)
            {
                allDtCol.AddRange(DtCol);
                allrecordNum += recordNum;
            }
            //this.totalNum.Text = UC_DownloadSearch.allDtCol.Count.ToString();

        }

        /// <summary>
        /// 查询privew记录，删除不存在.png;.pgw;-1.tif;-2.tif;-3.tif;-4.tif文件的privew记录
        /// </summary>
        /// <param name="sqlIpTilepath">长度为3，存放sql语句、IP地址、切片路径</param>
        public void DeletePngRecordWithGFFfileMissing(List<string> ipTilepath)
        {
            int recordNum = 0;
            DataSet DtCol = new DataSet();
            try
            {
                ITCPService sm = WebServiceUtil.GetTServerSiteTCPService(ipTilepath[0]);//ipandsql[1]为IP

                DtCol = sm.DeletePngRecordWithGFFfileMissing(ipTilepath[1]);//ipandsql[1]为sql

            }
            catch
            {
                recordNum = 0;
            }
            if (DtCol != null)
            {
                lock (obj)
                {
                    allDtCol.Add(DtCol);
                }
            }

        }
        /// <summary>
        /// 查询并删除有记录但文件缺失的瓦片记录，遍历每个配号下的DataSet，不分页返回全部结果。
        /// </summary>
        /// <param name="sqlIpTilepath">长度为3，存放sql语句、IP地址、切片路径</param>
        public void DeleteTileFileMissingRecord(List<string> ipTilepath)
        {
            int recordNum = 0;
            DataSet DtCol = new DataSet();
            try
            {
                ITCPService sm = WebServiceUtil.GetTServerSiteTCPService(ipTilepath[0]);//ipandsql[1]为IP

                DtCol = sm.DeleteTileFileMissingRecord(ipTilepath[1]);//ipandsql[1]为sql

            }
            catch
            {
                recordNum = 0;
            }
            if (DtCol != null)
            {
                lock (obj)
                {
                    allDtCol.Add(DtCol);
                }
            }

        }

        /// <summary>
        /// 获取各站点数据记录，不执行分页操作。获取全部数据。目前用于获取数据库中所有的切片等级、DEM
        /// </summary>
        /// <param name="sqlIpTilepath">长度为3，存放sql语句、IP地址、切片路径</param>
        public void GetDataSetColAll(List<string> sqlIpTilepath)
        {
            int recordNum = 0;
            List<DataSet> DtCol = new List<System.Data.DataSet>();
            try
            {       //测试站点未开时的情况
                ITCPService sm = WebServiceUtil.GetTServerSiteTCPService(sqlIpTilepath[1]);//ipandsql[1]为IP

                DtCol = sm.GetDataSetCol(sqlIpTilepath[0], sqlIpTilepath[2]);//ipandsql[1]为sql

            }
            catch(Exception e)
            {
                recordNum = 0;
                DtCol.Clear();
            }
            if (DtCol != null)
            {
                lock (obj)
                {
                    allDtCol.AddRange(DtCol);
                }
            }
            lock (obj)
            {
                allrecordNum += recordNum;
            }

        }

        /// <summary>
        /// 获取各站点数据记录，不执行分页操作。获取全部数据。目前用于获取数据库中所有的切片等级、DEM
        /// </summary>
        /// <param name="sqlCoordsIpTilepath">长度为3，存放sql语句、IP地址、切片路径</param>
        public void GetDataSetColAll_CoordsFilter(List<string> sqlCoordsIpTilepath)
        {
            int recordNum = 0;
            List<DataSet> DtCol = new List<System.Data.DataSet>();
            try
            {
                ITCPService sm = WebServiceUtil.GetTServerSiteTCPService(sqlCoordsIpTilepath[2]);//ipandsql[1]为IP

                DtCol = sm.GetDataSetCol_CoordsFilter(sqlCoordsIpTilepath[0], sqlCoordsIpTilepath[1], sqlCoordsIpTilepath[3]);//ipandsql[1]为sql

            }
            catch
            {
                recordNum = 0;
                DtCol.Clear();
            }
            if (DtCol != null)
            {
                lock (obj)
                {
                    allDtCol.AddRange(DtCol);
                }
            }
            lock (obj)
            {
                allrecordNum += recordNum;
            }

        }

        public void ExecuteNonQuery(List<string> ipandsql)
        {
            int recordNum = 0;
            List<DataSet> DtCol = new List<System.Data.DataSet>();
            try
            {
                ITCPService sm = WebServiceUtil.GetTServerSiteTCPService(ipandsql[1]);//ipandsql[1]为IP

                sm.ExecuteNonQuery(ipandsql[0], ipandsql[2]);//ipandsql[1]为sql

            }
            catch
            {
                recordNum = 0;
                DtCol.Clear();
            }
            if (DtCol != null)
            {
                lock (obj)
                {
                    allDtCol.AddRange(DtCol);
                }
            }
            lock (obj)
            {
                allrecordNum += recordNum;
            }

        }
        /// <summary>
        /// 获取分页查询结果,分页信息已知，（先分页后查询）
        /// </summary>
        /// <param name="ipandsql"></param>
        public void GetDataSetColPaged2(QuerySearchInfo siteQueryInfo)
        {
            int recordNum = 0;
            List<DataSet> DtCol = new List<System.Data.DataSet>();
            string ipAddress = string.Empty;
            if (siteQueryInfo.QRST_ModsList != null && siteQueryInfo.QRST_ModsList.Count != 0)
            {
                ipAddress = siteQueryInfo.QRST_ModsList[0].IPaddress;
            }
            try
            {

                ITCPService sm = WebServiceUtil.GetTServerSiteTCPService(ipAddress);//ipandsql[1]为IP

                DtCol = sm.GetDataSetColPaged2(siteQueryInfo.SQLString, out recordNum, siteQueryInfo.QRST_ModsList);//ipandsql[1]为sql

            }
            catch(Exception e)
            {
                recordNum = 0;
                DtCol.Clear();
            }
            if (DtCol != null)
            {
                lock (obj)
                {
                    allDtCol.AddRange(DtCol);
                }
            }

            //allrecordNum += recordNum;
            //this.totalNum.Text = UC_DownloadSearch.allDtCol.Count.ToString();

        }
        /// <summary>
        /// 获取分页查询结果,分页信息已知，（先分页后查询）
        /// </summary>
        /// <param name="ipandsql"></param>
        public void GetDataSetColPaged3(QuerySearchInfo siteQueryInfo)
        {
            int recordNum = 0;
            List<DataSet> DtCol = new List<System.Data.DataSet>();
            string ipAddress = string.Empty;
            if (siteQueryInfo.QRST_ModsList != null && siteQueryInfo.QRST_ModsList.Count != 0)
            {
                ipAddress = siteQueryInfo.QRST_ModsList[0].IPaddress;
            }
            try
            {

                ITCPService sm = WebServiceUtil.GetTServerSiteTCPService(ipAddress);//ipandsql[1]为IP

                //检查下siteQueryInfo.SQLString和siteQueryInfo.QRST_ModsList.ModSqlstring有何区别，如无区别，则GetDataSetColPaged2==GetDataSetColPaged3
                DtCol = sm.GetDataSetColPaged2(out recordNum, siteQueryInfo.QRST_ModsList);

            }
            catch
            {
                recordNum = 0;
                DtCol.Clear();
            }
            if (DtCol != null)
            {
                lock (obj)
                {
                    allDtCol.AddRange(DtCol);
                }
            }

            //allrecordNum += recordNum;
            //this.totalNum.Text = UC_DownloadSearch.allDtCol.Count.ToString();

        }
        /// <summary>
        /// 汇总结果记录信息，记录各站点各mod上符合记录的个数，以便用以计算分页检索时各配号分配检索个数
        /// </summary>
        /// <param name="ipandsql"></param>
        public void GetResultInfo_SiteModRecordCount(List<string> ipandsql)
        {
            int recordNum = 0;
            List<ModIDSearchInfo> MICol = new List<ModIDSearchInfo>();
            try
            {

                ITCPService sm = WebServiceUtil.GetTServerSiteTCPService(ipandsql[1]);//ipandsql[1]为IP

                MICol = sm.GetResultInfo_SiteModRecordCount(ipandsql[0], out recordNum, ipandsql[2]);//ipandsql[1]为sql
                //添加IP地址标记
                if (MICol.Count != 0)
                {
                    foreach (ModIDSearchInfo modinfo in MICol)
                    {
                        modinfo.IPaddress = ipandsql[1];
                    }
                }
            }
            catch
            {
                recordNum = 0;
                MICol.Clear();
            }
            lock (obj)
            {
                QRSTModInfo.AddRange(MICol);
                allrecordNum += recordNum;
            }
            //this.totalNum.Text = UC_DownloadSearch.allDtCol.Count.ToString();

        }

        /// <summary>
        /// 汇总结果记录信息，记录各站点各mod上符合记录的个数，以便用以计算分页检索时各配号分配检索个数
        /// </summary>
        /// <param name="dic">根据多边形坐标点串为空间范围</param>
        public void GetResultInfo_SiteModRecordCount(Dictionary<List<string>, List<Coordinate>> dic)
        {
            int recordNum = 0;
            List<ModIDSearchInfo> MICol = new List<ModIDSearchInfo>();
            try
            {
                List<string> ipandsql = new List<string>();
                List<Coordinate> coordinate = new List<Coordinate>();
                foreach (var item in dic)
                {
                    ipandsql = item.Key;
                    coordinate = item.Value;
                }
                ITCPService sm = WebServiceUtil.GetTServerSiteTCPService(ipandsql[2]);//ipandsql[1]为IP

                MICol = sm.GetResultInfo_SiteModRecordCount(ipandsql[0], ipandsql[1], out recordNum, ipandsql[3], coordinate);//ipandsql[1]为sql
                //添加IP地址标记
                if (MICol.Count != 0)
                {
                    foreach (ModIDSearchInfo modinfo in MICol)
                    {
                        modinfo.IPaddress = ipandsql[2];
                    }
                }
            }
            catch
            {
                recordNum = 0;
                MICol.Clear();
            }
            lock (obj)
            {
                QRSTModInfo.AddRange(MICol);
                allrecordNum += recordNum;
            }
            //this.totalNum.Text = UC_DownloadSearch.allDtCol.Count.ToString();

        }

        /// <summary>
        /// 汇总结果记录信息，记录各站点各mod上符合记录的个数，以便用以计算分页检索时各配号分配检索个数
        /// </summary>
        /// <param name="dic">根据行政区名称定义空间范围</param>
        public void GetResultInfo_SiteModRecordCount(Dictionary<List<string>, List<string>> dic)
        {
            int recordNum = 0;
            List<ModIDSearchInfo> MICol = new List<ModIDSearchInfo>();
            List<string> ipandsql = new List<string>();
            List<string> regionLst = new List<string>();
            foreach (var item in dic)
            {
                ipandsql = item.Key;
                regionLst = item.Value;
            }
            try
            {
                ITCPService sm = WebServiceUtil.GetTServerSiteTCPService(ipandsql[1]);//ipandsql[1]为IP

                //MICol = sm.GetResultInfo_Site(ipandsql[0], out recordNum, ipandsql[2]);//ipandsql[1]为sql
                MICol = sm.GetResultInfo_SiteModRecordCount(regionLst[2], ipandsql[0], out recordNum, ipandsql[2], regionLst[0], regionLst[1]);//ipandsql[1]为sql
                //添加IP地址标记
                if (MICol.Count != 0)
                {
                    foreach (ModIDSearchInfo modinfo in MICol)
                    {
                        modinfo.IPaddress = ipandsql[1];
                    }
                }
            }
            catch
            {
                recordNum = 0;
                MICol.Clear();
            }
            lock (obj)
            {
                QRSTModInfo.AddRange(MICol);
                allrecordNum += recordNum;
            }
            //this.totalNum.Text = UC_DownloadSearch.allDtCol.Count.ToString();

        }

        /// <summary>
        /// 将List融合成一个DataSet
        /// </summary>
        /// <param name="allDtCol">所有检索返回的List</param>
        /// <returns></returns>
        public DataSet MergeAllDataSet()
        {
            DataSet DS = new DataSet();
            if (allDtCol.Count != 0)
            {
                foreach (DataSet ds in allDtCol)
                {
                    if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        if (DS.Tables.Count == 0)
                        {
                            DS.Tables.Add(ds.Tables[0].Copy());
                        }
                        else
                        {
                            //DataRow[] drs=new DataRow[ds.Tables[0].Rows.Count];
                            //ds.Tables[0].Rows.CopyTo(drs, 0);
                            //DS.Tables[0].LoadDataRow(drs,  LoadOption.OverwriteChanges);

                            DS.Merge(ds, false, MissingSchemaAction.Add);
                        }
                    }

                }


            }
            return DS;

        }


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
        public bool Istime(string dateTimetext, string datetimeEndtext)
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
        public bool IsPositionNull(string etext, string wtext, string stext, string ntext)
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

        /// <summary>
        /// 查询缺少满幅度云量的瓦片（需要1.2.3.4波段）,计算更新瓦片的满幅度云量信息，重命名，更新记录。
        /// 建议调用本方法前执行下DeleteTileFileMissingRecord方法，对缺失数据的记录进行清理
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="commonSharePath"></param>
        /// <returns></returns>
        public void UpdateDistinctTables(List<string> ipandpath)
        {
            try
            {
                ITCPService sm = GetTServerSiteTCPService(ipandpath[0]);//ipandsql[0]为IP

                sm.UpdateDistinctTables(ipandpath[1]);//ipandsql[1]为commonSharePath

            }
            catch
            {
            }

        }
    }
}
