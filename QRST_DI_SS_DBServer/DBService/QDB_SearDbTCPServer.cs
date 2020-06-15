using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;
using System.Runtime.Serialization.Formatters;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;
using Newtonsoft.Json;
using QRST_DI_DS_Metadata.MetaDataCls;
using QRST_DI_DS_Metadata.MetaDataDefiner.Dal;
using QRST_DI_DS_Metadata.Paths;
using QRST_DI_DS_MetadataQuery;
using QRST_DI_MS_Basis.Log;
using QRST_DI_MS_Basis.MySQLBase;
using QRST_DI_Resources;
using QRST_DI_SS_Basis.MetadataQuery;
using QRST_DI_SS_Basis.MetaData;
using QRST_DI_SS_DBInterfaces.IDBEngine;
using QRST_DI_SS_DBInterfaces.IDBService;
using QRST_DI_DS_MetadataQuery.JSONutilty;
using System.IO;
using QRST_DI_TS_Process.Orders;
using DotSpatial.Topology;
using System.Web.Services;
using QRST_DI_DS_MetadataQuery.QueryConditionParameter;
using System.Linq;
using QRST_DI_TS_Basis.DirectlyAddress;
using QRST_DI_TS_Basis.Search;
using DotSpatial.Data;

namespace QRST_DI_SS_DBServer.DBService
{
    class QDB_SearDbTCPServer : MarshalByRefObject, IQDB_Searcher_Db
    {
        private static TcpServerChannel _chan = null;
        IDbOperating _dbOperator;
        IDbBaseUtilities _dbBaseUti;
        private QRST_DI_MS_Basis.MySQLBase.SQLBaseTool sqlBaseTool;
        System.Data.DataSet logDS = null;
        private System.Data.DataSet returnDS = null;
        string logStr = "";
        DataTable dt = null;
        List<object> fileinfolist = null;
        GetClassify classify;
        gettype[] classifyfgw;
        string bpdbCon;
        string resuStr;
        string jsonStr;
        string[] strFields;

        public QDB_SearDbTCPServer()
        {
            if (!Constant.ServiceIsConnected)
            {
                Constant.InitializeTcpConnection();
            }

            _dbOperator = Constant.IdbOperating;
            _dbBaseUti = Constant.IdbServerUtilities;
            sqlBaseTool = new QRST_DI_MS_Basis.MySQLBase.SQLBaseTool();
        }

        /// <summary>
        /// 开启TCP服务
        /// </summary>
        public static void StartTCPService(string tcpPort)
        {
            try
            {
                BinaryServerFormatterSinkProvider serverProvider = new BinaryServerFormatterSinkProvider();
                serverProvider.TypeFilterLevel = TypeFilterLevel.Full;

                IDictionary props = new Hashtable();
                //props["port"] = tcpPort;
                props["name"] = "SearSQLite_TCP";
                props["typeFilterLevel"] = TypeFilterLevel.Full;
                _chan = new TcpServerChannel(
                    props, serverProvider);
                ChannelServices.RegisterChannel(_chan);

                RemotingConfiguration.RegisterWellKnownServiceType(typeof(QDB_SearDbTCPServer),
                    "QDB_SearSQLite_TCP",
                    WellKnownObjectMode.SingleCall);
            }
            catch (Exception e)
            {
                throw new Exception("注册QDB_SearDbTCPServer异常", e);
            }
        }

        #region 服务接口
        public XmlDocument GetTreeCatalogByXML()
        {
            TreeNode[] DBTreeNodes;
            XElement rootElement = new XElement("QRST_DB");

            DataTable subDbArr = _dbBaseUti.GetDataSet("select * from subdbinfo ").Tables[0];
            int DBCount = subDbArr.Rows.Count;
            DBTreeNodes = new TreeNode[DBCount];

            for (int i = 0; i < DBCount; i++)
            {
                string name = subDbArr.Rows[i]["NAME"].ToString();
                string qrst_code = subDbArr.Rows[i]["QRST_CODE"].ToString();
                string description = subDbArr.Rows[i]["DESCRIPTION"].ToString();
                string connectStr = subDbArr.Rows[i]["ConnectStr"].ToString();
                //SiteDb subdb = new SiteDb(name, qrst_code, connectStr, description);
                XElement elementDB = new XElement("DataBase",
                    new XAttribute("Name", description),
                    new XAttribute("Description", name)
                    //new XAttribute("CatalogCode", ""),
                    //new XAttribute("DataCode", ""),
                    //new XAttribute("DataType", ""),
                    //new XAttribute("PreCatalogCode", "")
                    );
                _dbBaseUti = _dbBaseUti.GetSubDbUtilByCon(connectStr);

                string sqlCatalog = @"select metadatacatalognode.GROUP_CODE as CatalogCode,
                              metadatacatalognode.NAME,metadatacatalognode.DATA_CODE as DataCode,
                              metadatacatalognode.GROUP_TYPE as DataType,metadatacatalognode.DESCRIPTION,
                              metadatacatalognode_r.GROUP_CODE as PreCatalogCode from metadatacatalognode 
                              left join metadatacatalognode_r on 
                              metadatacatalognode.GROUP_CODE = metadatacatalognode_r.CHILD_CODE";
                System.Data.DataSet dsCatalog = _dbBaseUti.GetDataSet(sqlCatalog);
                DataTable dtCatalog = dsCatalog.Tables[0];

                //添加数据库节点内容
                DataRow rowDB = null;
                for (int j = 0; j < dtCatalog.Rows.Count; j++)
                {
                    if (dtCatalog.Rows[j]["NAME"].ToString() == description)
                    {
                        rowDB = dtCatalog.Rows[j];
                        if (rowDB != null)
                        {
                            elementDB.Add(new XAttribute("CatalogCode", rowDB["CatalogCode"]));
                            elementDB.Add(new XAttribute("DataCode", rowDB["DataCode"]));
                            elementDB.Add(new XAttribute("DataType", rowDB["DataType"]));
                            elementDB.Add(new XAttribute("PreCatalogCode", rowDB["PreCatalogCode"]));
                        }
                        else
                        {
                            elementDB.Add(new XAttribute("CatalogCode", ""));
                            elementDB.Add(new XAttribute("DataCode", ""));
                            elementDB.Add(new XAttribute("DataType", ""));
                            elementDB.Add(new XAttribute("PreCatalogCode", ""));
                        }
                        dtCatalog.Rows.RemoveAt(j);
                        break;
                    }
                }

                //添加库中数据类型节点内容
                for (int j = 0; j < dtCatalog.Rows.Count; j++)
                {
                    XElement elementDataType = new XElement("CatalogData",
                        new XAttribute("Name", dtCatalog.Rows[j]["NAME"]),
                        new XAttribute("Description", dtCatalog.Rows[j]["DESCRIPTION"]),
                        new XAttribute("CatalogCode", dtCatalog.Rows[j]["CatalogCode"]),
                        new XAttribute("DataCode", dtCatalog.Rows[j]["DataCode"]),
                        new XAttribute("DataType", dtCatalog.Rows[j]["DataType"]),
                        new XAttribute("PreCatalogCode", dtCatalog.Rows[j]["PreCatalogCode"])
                        );
                    elementDB.Add(elementDataType);
                }

                rootElement.Add(elementDB);
            }

            XmlDocument returnXml = new XmlDocument();
            returnXml.LoadXml(rootElement.ToString());
            return returnXml;
        }

        public string[] GetFieldsByDataCode(string dataCode)
        {
            string[] arrFields = new string[] { };
            string tableName;
            string dbName;
            //根据表编码获取数据库名称
            dbName = dataCode.Substring(0, 4);

            //根据数据库名称获取库访问实例
            switch (dbName)
            {
                case "BSDB":
                    _dbBaseUti = _dbOperator.GetSubDbUtilities(EnumDBType.BSDB);
                    break;
                case "EVDB":
                    _dbBaseUti = _dbOperator.GetSubDbUtilities(EnumDBType.EVDB);
                    break;
                case "IPDB":
                    _dbBaseUti = _dbOperator.GetSubDbUtilities(EnumDBType.IPDB);
                    break;
                case "ISDB":
                    _dbBaseUti = _dbOperator.GetSubDbUtilities(EnumDBType.ISDB);
                    break;
                case "MADB":
                    _dbBaseUti = _dbOperator.GetSubDbUtilities(EnumDBType.MADB);
                    break;
                case "MIDB":
                    _dbBaseUti = _dbOperator.GetSubDbUtilities(EnumDBType.MIDB);
                    break;
                case "RCDB":
                    _dbBaseUti = _dbOperator.GetSubDbUtilities(EnumDBType.RCDB);
                    break;
                default:
                    break;
            }

            //根据库访问实例和表编码获取表名
            tablecode_Dal tablecodeDal = new tablecode_Dal(_dbBaseUti);
            tableName = tablecodeDal.GetTableName(dataCode);

            //根据表名获得表视图
            table_view_Dal tableviewDal = new table_view_Dal(_dbBaseUti);
            arrFields = tableviewDal.GetFields(tableName);
            return arrFields;
        }

        public System.Data.DataSet SearchAOD(List<string> datetime, string dataType)
        {
            System.Data.DataSet result = new System.Data.DataSet();
            if (datetime.Count != 2)
                return result;
            _dbBaseUti = _dbOperator.GetSubDbUtilities(EnumDBType.EVDB);
            string selectString = string.Format("select * from AOD_View where Date between '{0}' and '{1}' and Type='{2}'", datetime[0], datetime[1], dataType);
            result = _dbBaseUti.GetDataSet(selectString);
            return result;
        }

        public System.Data.DataSet SearchDOCByField(string TITLE, string DOCTYPE, string KEYWORD, string AUTHOR, string UPLOADER, string UPLOADTIMEStart, string UPLOADTIMEOver)//List<string> DOCDATE,, List<string> UPLOADTIME
        {
            System.Data.DataSet result = new System.Data.DataSet();
            //if (DOCDATE.Count != 2 || UPLOADTIME.Count != 2)
            //    return result;
            _dbBaseUti = _dbOperator.GetSubDbUtilities(EnumDBType.ISDB);
            string selectString = string.Format("select * from mould_doc where TITLE like '%{0}%' and DOCTYPE like '%{1}%' and KEYWORD like '%{2}%'   and AUTHOR like '%{3}%' and UPLOADER like '%{4}%'  and UPLOADTIME between '{5}' and '{6}' ", TITLE, DOCTYPE, KEYWORD, AUTHOR, UPLOADER, UPLOADTIMEStart, UPLOADTIMEOver);//or (DOCDATE between '{3}' and '{4}')  or (UPLOADTIME between '{7}' and '{8}')       
            result = _dbBaseUti.GetDataSet(selectString);
            return result;

        }

        public System.Data.DataSet SearchDOCByKeyWord(string KeyWord)
        {
            System.Data.DataSet result = new System.Data.DataSet();
            _dbBaseUti = _dbOperator.GetSubDbUtilities(EnumDBType.ISDB);
            string selectString = string.Format("select * from mould_doc where  TITLE like '%{0}%' or DOCTYPE like '%{0}%' or KEYWORD like '%{0}%' or ABSTRACT like '%{0}%' or DESCRIPTION like '%{0}%' or AUTHOR like '%{0}%' or UPLOADER like '%{0}%'  or FILESIZE like '%{0}%' or QRST_CODE like '%{0}%'", KeyWord);
            result = _dbBaseUti.GetDataSet(selectString);
            return result;
        }

        public System.Data.DataSet SearchDOCByFieldPage(string TITLE, string DOCTYPE, string KEYWORD, string AUTHOR, string UPLOADER, string UPLOADTIMEStart, string UPLOADTIMEOver, int pageNow, int pageSize)//List<string> DOCDATE,, List<string> UPLOADTIME
        {

            System.Data.DataSet result = new System.Data.DataSet();
            //if (DOCDATE.Count != 2 || UPLOADTIME.Count != 2)
            //    return result;
            //string dbConn = mySQLOperator.ISDB.connString;
            //MySqlBaseUtilities isdb = new MySqlBaseUtilities(dbConn);
            _dbBaseUti = _dbOperator.GetSubDbUtilities(EnumDBType.ISDB);
            int startSize = (pageNow - 1) * pageSize;
            string selectString = string.Format("select * from mould_doc where TITLE like '%{0}%' and DOCTYPE like '%{1}%'"
                + "and KEYWORD like '%{2}%'   and AUTHOR like '%{3}%' and UPLOADER like '%{4}%'  and UPLOADTIME between '{5}' and '{6}' limit " + startSize + "," + pageSize, TITLE, DOCTYPE, KEYWORD, AUTHOR, UPLOADER, UPLOADTIMEStart, UPLOADTIMEOver, startSize, pageSize);//or (DOCDATE between '{3}' and '{4}')  or (UPLOADTIME between '{7}' and '{8}')

            result = _dbBaseUti.GetDataSet(selectString);

            int TotalNumber = result.Tables[0].Rows.Count;
            if (result.Tables.Count > 0 && result.Tables[0].Rows.Count > 0)
            {
                InforLog<string>.inforLog.Info("文件:QDB_SearDbTCPServer/App_Code/Service.cs 方法：SearchDOCByField 输出:result不为空");
            }
            else
                InforLog<string>.inforLog.Info("文件:QDB_SearDbTCPServer/App_Code/Service.cs 方法：SearchDOCByField 输出:result为空");
            return result;

        }

        public System.Data.DataSet GetDataTableStruct(string tableCode)
        {
            InforLog<string>.inforLog.Info("文件:WS_QDB_Searcher_MySQL/App_Code/Service.cs 方法：GetDataTableStruct 输入:tableCode=" + tableCode);
            try
            {
                if (tableCode.Length < 6)
                {
                    throw new Exception("表编码不正确！");
                }
                string subDb = tableCode.Substring(0, 4);
                _dbBaseUti = _dbOperator.GetsqlBaseObj(subDb);
                if (_dbBaseUti == null)
                {
                    throw new Exception("表编码不正确！");
                }
                ViewBasedQuerySchema schema = new ViewBasedQuerySchema(new string[] { "*" }, tableCode, _dbBaseUti);
                logDS = schema.GetTableStruct();
                if (logDS != null && logDS.Tables.Count > 0 && logDS.Tables[0].Rows.Count > 0)
                {
                    InforLog<string>.inforLog.Info("文件:WS_QDB_Searcher_MySQL/App_Code/Service.cs 方法：GetDataTableStruct 输出:返回DataSet不为空");
                }
                else
                    InforLog<string>.inforLog.Info("文件:WS_QDB_Searcher_MySQL/App_Code/Service.cs 方法：GetDataTableStruct 输出:返回DataSet为空");
                return schema.GetTableStruct();
            }
            catch (Exception ex)
            {
                throw new Exception("获取视图表结构失败！");
            }
        }

        public QueryResponse GetMetadata(QueryRequest _request)
        {
            ViewBasedQuery queryObj = new ViewBasedQuery(_request);
            return queryObj.Query();
        }

        public System.Data.DataSet GetMetadataStr(string _xmlstrQueryRequest)
        {
            InforLog<string>.inforLog.Info("文件:WS_QDB_Searcher_MySQL/App_Code/Service.cs 方法：GetMetadataStr 输入:_xmlstrQueryRequest=" + _xmlstrQueryRequest);
            QueryRequest _request = new QueryRequest();
            System.Xml.Serialization.XmlSerializer xs = new System.Xml.Serialization.XmlSerializer(typeof(QueryRequest));
            using (System.IO.Stream ms = new System.IO.MemoryStream(Encoding.UTF8.GetBytes(_xmlstrQueryRequest)))
            {
                using (XmlReader xmlRead = XmlReader.Create(ms))
                {
                    object obj = xs.Deserialize(xmlRead);
                    _request = (QueryRequest)obj;
                }
            }

            ViewBasedQuery queryObj = new ViewBasedQuery(_request);
            logDS = queryObj.Query().recordSet;
            if (logDS != null && logDS.Tables.Count > 0 && logDS.Tables[0].Rows.Count > 0)
            {
                InforLog<string>.inforLog.Info("文件:WS_QDB_Searcher_MySQL/App_Code/Service.cs 方法：GetMetadataStr 输出:返回DataSet不为空");
            }
            else
                InforLog<string>.inforLog.Info("文件:WS_QDB_Searcher_MySQL/App_Code/Service.cs 方法：GetMetadataStr 输出:返回DataSet为空");
            return queryObj.Query().recordSet;
        }

        public int GetTotalRecord(QueryRequest _request)
        {
            ViewBasedQuery queryObj = new ViewBasedQuery(_request);
            return queryObj.GetRecordCount();
        }

        public System.Data.DataSet GetDetailInfo(string qrst_code)
        {
            InforLog<string>.inforLog.Info("文件:WS_QDB_Searcher_MySQL/App_Code/Service.cs 方法：GetDetailInfo 输入:qrst_code=" + qrst_code);
            try
            {
                string tableCode = StoragePath.GetTableCodeByQrstCode(qrst_code);
                QueryRequest qr = new QueryRequest();
                qr.dataBase = tableCode.Substring(0, 4);
                qr.elementSet = new string[] { "*" };
                qr.offset = 10;
                qr.recordSetStartPointSpecified = 0;
                qr.tableCode = tableCode;
                string[] idflag = new string[] { "QRST_CODE", "数据编码" };
                System.Data.DataSet tableStruct = GetDataTableStruct(tableCode);
                string fieldCode = null;

                for (int i = 0; i < tableStruct.Tables[0].Rows.Count; i++)
                {
                    for (int j = 0; j < idflag.Length; j++)
                    {
                        if (tableStruct.Tables[0].Rows[i][0].ToString() == idflag[j])
                        {
                            fieldCode = tableStruct.Tables[0].Rows[i]["FieldCode"].ToString();
                            break;
                        }
                    }
                    if (fieldCode != null)
                    {
                        break;
                    }
                }
                if (fieldCode == null)
                {
                    throw new Exception("没有找到ID字段");
                }
                //定义查询条件
                qr.complexCondition = new ComplexCondition();
                SimpleCondition sc = new SimpleCondition();
                sc.accessPointField = fieldCode;
                sc.comparisonOperatorField = "=";
                sc.valueField = qrst_code;

                qr.complexCondition.simpleCondition = new SimpleCondition[] { sc };
                ViewBasedQuery queryObj = new ViewBasedQuery(qr);
                logDS = queryObj.Query().recordSet;
            }
            catch (Exception ex)
            {
                logDS = null;
            }
            if (logDS != null && logDS.Tables.Count > 0 && logDS.Tables[0].Rows.Count > 0)
            {
                InforLog<string>.inforLog.Info("文件:WS_QDB_Searcher_MySQL/App_Code/Service.cs 方法：GetDetailInfo 输出:返回DataSet不为空");
            }
            else
                InforLog<string>.inforLog.Info("文件:WS_QDB_Searcher_MySQL/App_Code/Service.cs 方法：GetDetailInfo 输出:返回DataSet为空");
            return logDS;
        }

        #region 行政区划分级及 对应经纬度获取
        public string[] GetProvinceLst()
        {
            _dbBaseUti = _dbOperator.GetSubDbUtilities(EnumDBType.BSDB);

            List<string> provincesLst = new List<string>();
            string sql = "select distinct province from arealocation ";
           System.Data.DataSet ds = _dbBaseUti.GetDataSet(sql);
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                provincesLst.Add(ds.Tables[0].Rows[i][0].ToString());
            }
            InforLog<string>.inforLog.Info("文件:WS_QDB_Searcher_MySQL/App_Code/Service.cs 方法：GetProvinceLst 输出:provincesLst.ToArray()=" + InforLog<string>.returnSArrStrElem(provincesLst.ToArray()));
            return provincesLst.ToArray();
        }

        public string[] GetCityLstByProvince(string province)
        {
            InforLog<string>.inforLog.Info("文件:WS_QDB_Searcher_MySQL/App_Code/Service.cs 方法：GetDetailInfo 输入:province=" + province);
            _dbBaseUti = _dbOperator.GetSubDbUtilities(EnumDBType.BSDB);

            List<string> cityLst = new List<string>();
            string sql = string.Format("select distinct city from arealocation where province = '{0}' ", province);
           System.Data.DataSet ds = _dbBaseUti.GetDataSet(sql);
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                cityLst.Add(ds.Tables[0].Rows[i][0].ToString());
            }
            InforLog<string>.inforLog.Info("文件:WS_QDB_Searcher_MySQL/App_Code/Service.cs 方法：GetCityLstByProvince 输出:cityLst.ToArray()=" + InforLog<string>.returnSArrStrElem(cityLst.ToArray()));
            return cityLst.ToArray();
        }

        public string[] GetCountyLstByProvinceCity(string province, string city)
        {
            logStr = string.Format("province='{0}' city='{1}'", province, city);
            InforLog<string>.inforLog.Info("文件:WS_QDB_Searcher_MySQL/App_Code/Service.cs 方法：GetCountyLstByProvinceCity 输入:" + logStr);
            _dbBaseUti = _dbOperator.GetSubDbUtilities(EnumDBType.BSDB);
            List<string> countyLst = new List<string>();
            string sql = string.Format("select distinct county from arealocation where province = '{0}' and city = '{1}' ", province, city);
           System.Data.DataSet ds = _dbBaseUti.GetDataSet(sql);
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                countyLst.Add(ds.Tables[0].Rows[i][0].ToString());
            }
            InforLog<string>.inforLog.Info("文件:WS_QDB_Searcher_MySQL/App_Code/Service.cs 方法：GetCountyLstByProvinceCity 输出:countyLst.ToArray()=" + InforLog<string>.returnSArrStrElem(countyLst.ToArray()));
            return countyLst.ToArray();
        }

        public double[] GetSpacialInfoByCounty(string province, string city, string county)
        {
            logStr = string.Format("province='{0}' city='{1}' county='{2}'", province, city, county);
            InforLog<string>.inforLog.Info("文件:WS_QDB_Searcher_MySQL/App_Code/Service.cs 方法：GetSpacialInfoByCounty 输入:" + logStr);
            _dbBaseUti = _dbOperator.GetSubDbUtilities(EnumDBType.BSDB);

            string querySql = string.Format(" select maxx, maxy, minx, miny from arealocation where province = '{0}' and city = '{1}' and county = '{2}'", province, city, county);
           System.Data.DataSet ds = _dbBaseUti.GetDataSet(querySql);
            if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count < 1)
            {
                InforLog<string>.inforLog.Info("文件:WS_QDB_Searcher_MySQL/App_Code/Service.cs 方法：GetSpacialInfoByCounty 输出:NULL");
                return null;
            }
            else
            {
                double[] extent = new double[4];
                extent[0] = double.Parse(ds.Tables[0].Rows[0]["maxx"].ToString());
                extent[1] = double.Parse(ds.Tables[0].Rows[0]["maxy"].ToString());
                extent[2] = double.Parse(ds.Tables[0].Rows[0]["minx"].ToString());
                extent[3] = double.Parse(ds.Tables[0].Rows[0]["miny"].ToString());
                InforLog<string>.inforLog.Info("文件:WS_QDB_Searcher_MySQL/App_Code/Service.cs 方法：GetSpacialInfoByCounty 输出:" + InforLog<double>.returnSArrStrElem(extent));
                return extent;
            }

        }

        public double[] GetSpacialInfoByCity(string province, string city)
        {
            logStr = string.Format("province='{0}' city='{1}'", province, city);
            InforLog<string>.inforLog.Info("文件:WS_QDB_Searcher_MySQL/App_Code/Service.cs 方法：GetSpacialInfoByCity 输入:" + logStr);
            _dbBaseUti = _dbOperator.GetSubDbUtilities(EnumDBType.BSDB);

            //select max(maxx) as maxx,max(maxy) as maxy,min(minx) as minx,min(miny) as miny from arealocation
            string querySql = string.Format("select max(maxx) as maxx,max(maxy) as maxy,min(minx) as minx,min(miny) as miny from arealocation where province = '{0}' and city = '{1}' ", province, city);
           System.Data.DataSet ds = _dbBaseUti.GetDataSet(querySql);
            if (ds == null || ds.Tables[0].Rows.Count < 1)
            {
                InforLog<string>.inforLog.Info("文件:WS_QDB_Searcher_MySQL/App_Code/Service.cs 方法：GetSpacialInfoByCity 输出:NULL");
                return null;
            }
            else
            {
                double[] extent = new double[4];
                extent[0] = double.Parse(ds.Tables[0].Rows[0]["maxx"].ToString());
                extent[1] = double.Parse(ds.Tables[0].Rows[0]["maxy"].ToString());
                extent[2] = double.Parse(ds.Tables[0].Rows[0]["minx"].ToString());
                extent[3] = double.Parse(ds.Tables[0].Rows[0]["miny"].ToString());
                InforLog<string>.inforLog.Info("文件:WS_QDB_Searcher_MySQL/App_Code/Service.cs 方法：GetSpacialInfoByCity 输出:" + InforLog<double>.returnSArrStrElem(extent));
                return extent;
            }
        }

        public double[] GetSpacialInfoByProvince(string province)
        {
            logStr = string.Format("province='{0}'", province);
            InforLog<string>.inforLog.Info("文件:WS_QDB_Searcher_MySQL/App_Code/Service.cs 方法：GetSpacialInfoByProvince 输入:" + logStr);
            _dbBaseUti = _dbOperator.GetSubDbUtilities(EnumDBType.BSDB);

            string querySql = string.Format("select max(maxx) as maxx,max(maxy) as maxy,min(minx) as minx,min(miny) as miny from arealocation where province = '{0}' ", province);
           System.Data.DataSet ds = _dbBaseUti.GetDataSet(querySql);
            if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count < 1)
            {
                InforLog<string>.inforLog.Info("文件:WS_QDB_Searcher_MySQL/App_Code/Service.cs 方法：GetSpacialInfoByProvince 输出:NULL");
                return null;
            }
            else
            {
                double[] extent = new double[4];
                extent[0] = double.Parse(ds.Tables[0].Rows[0]["maxx"].ToString());
                extent[1] = double.Parse(ds.Tables[0].Rows[0]["maxy"].ToString());
                extent[2] = double.Parse(ds.Tables[0].Rows[0]["minx"].ToString());
                extent[3] = double.Parse(ds.Tables[0].Rows[0]["miny"].ToString());
                InforLog<string>.inforLog.Info("文件:WS_QDB_Searcher_MySQL/App_Code/Service.cs 方法：GetSpacialInfoByProvince 输出:" + InforLog<double>.returnSArrStrElem(extent));
                return extent;
            }
        }

        public string GetSpacialDetailInfoByCounty(string province, string city, string county)
        {
            logStr = string.Format("province='{0}' city='{1}' county='{2}'", province, city, county);
            InforLog<string>.inforLog.Info("文件:WS_QDB_Searcher_MySQL/App_Code/Service.cs 方法：GetSpacialInfoByCounty 输入:" + logStr);
            _dbBaseUti = _dbOperator.GetSubDbUtilities(EnumDBType.BSDB);

            string querySql = string.Format(" select Points from arealocationall where province = '{0}' and city = '{1}' and county = '{2}'", province, city, county);
           System.Data.DataSet ds = _dbBaseUti.GetDataSet(querySql);
            if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count < 1)
            {
                InforLog<string>.inforLog.Info("文件:WS_QDB_Searcher_MySQL/App_Code/Service.cs 方法：GetSpacialInfoByCounty 输出:NULL");
                return null;
            }
            return ds.Tables[0].Rows[0][0].ToString();
        }

        public string GetSpaciaDetaillInfoByCity(string province, string city)
        {
            logStr = string.Format("province='{0}' city='{1}'", province, city);
            InforLog<string>.inforLog.Info("文件:WS_QDB_Searcher_MySQL/App_Code/Service.cs 方法：GetSpacialInfoByCity 输入:" + logStr);
            _dbBaseUti = _dbOperator.GetSubDbUtilities(EnumDBType.BSDB);

            //select max(maxx) as maxx,max(maxy) as maxy,min(minx) as minx,min(miny) as miny from arealocation
            string querySql = string.Format("select Points from arealocationcity where province = '{0}' and city = '{1}' ", province, city);
           System.Data.DataSet ds = _dbBaseUti.GetDataSet(querySql);
            if (ds == null || ds.Tables[0].Rows.Count < 1)
            {
                InforLog<string>.inforLog.Info("文件:WS_QDB_Searcher_MySQL/App_Code/Service.cs 方法：GetSpacialInfoByCity 输出:NULL");
                return null;
            }
            return ds.Tables[0].Rows[0][0].ToString();
        }

        public string GetSpacialDetailInfoByProvince(string province)
        {
            logStr = string.Format("province='{0}'", province);
            InforLog<string>.inforLog.Info("文件:WS_QDB_Searcher_MySQL/App_Code/Service.cs 方法：GetSpacialInfoByProvince 输入:" + logStr);
            _dbBaseUti = _dbOperator.GetSubDbUtilities(EnumDBType.BSDB);

            string querySql = string.Format("select Points from arealocationprovince where province = '{0}' ", province);
           System.Data.DataSet ds = _dbBaseUti.GetDataSet(querySql);
            if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count < 1)
            {
                InforLog<string>.inforLog.Info("文件:WS_QDB_Searcher_MySQL/App_Code/Service.cs 方法：GetSpacialInfoByProvince 输出:NULL");
                return null;
            }
            return ds.Tables[0].Rows[0][0].ToString();
        }
        #endregion

        #region 数据自我检验接口
        public bool GF1DataCertificate(string dataName)
        {
            logStr = string.Format("dataName='{0}'", dataName);
            InforLog<string>.inforLog.Info("文件:WS_QDB_Searcher_MySQL/App_Code/Service.cs 方法：GF1DataCertificate 输入:" + logStr);
            Boolean b = false;
            //根据数据库名称获取库访问实例
            _dbBaseUti = _dbOperator.GetSubDbUtilities(EnumDBType.EVDB);

            string sqlstring = string.Format("select * from prod_gf1 where name = '{0}'", dataName);

            returnDS = _dbBaseUti.GetDataSet(sqlstring);

            if (returnDS != null && returnDS.Tables.Count > 0 && returnDS.Tables[0].Rows.Count > 0)
            {
                b = true;
            }
            else
            {
                b = false;
            }
            InforLog<string>.inforLog.Info("文件:WS_QDB_Searcher_MySQL/App_Code/Service.cs 方法：GF1DataCertificate 输出:" + b);
            return b;
        }

        public List<string> GF1DataCertificate_Multi(List<string> dataNames)
        {
            logStr = string.Format("dataName='{0}'", InforLog<string>.returnListStrElem(dataNames));
            InforLog<string>.inforLog.Info("文件:WS_QDB_Searcher_MySQL/App_Code/Service.cs 方法：GF1DataCertificate_Multi 输入:" + logStr);
            List<string> result = new List<string>();
            if (dataNames == null || dataNames.Count == 0)
            {
                return result;
            }
            result.AddRange(dataNames.ToArray());

            string SQLString = string.Format("select name from prod_gf1 where name in (");
            for (int i = 0; i < dataNames.Count; i++)
            {
                SQLString = string.Format("{0}'{1}',", SQLString, dataNames[i]);
            }
            SQLString = SQLString.TrimEnd().TrimEnd(',');
            SQLString += ")";

            //根据数据库名称获取库访问实例
            _dbBaseUti = _dbOperator.GetSubDbUtilities(EnumDBType.EVDB);

            returnDS = _dbBaseUti.GetDataSet(SQLString);

            if (returnDS != null && returnDS.Tables.Count > 0)
            {
                foreach (DataRow dr in returnDS.Tables[0].Rows)
                {
                    result.Remove(dr["name"].ToString());
                }
            }
            logStr = string.Format("result='{0}'", InforLog<string>.returnListStrElem(result));
            InforLog<string>.inforLog.Info("文件:WS_QDB_Searcher_MySQL/App_Code/Service.cs 方法：GF1DataCertificate_Multi 输出:" + logStr);
            return result;
        }
        #endregion

        #region MS_波普查询接口
        #region 大气查询
        public String getQueryAtmospheres(String maxLat, String maxLng, String minLat, String minLng, String zdmc, String zdbh, String pagenum, String pagesize)
        {
            _dbBaseUti = _dbOperator.GetSubDbUtilities(EnumDBType.RCDB);
            String sql = "select fseq,zdmc,zdbh,zdsx,jcrq,zdjd,zdwd,sjsjdw,jcsj,zgqy,zdqy from atmosphere where 1=1 ";
            if ((maxLat != null) && (maxLat.Trim() != ""))
            {
                sql = sql + " AND zdwd<=" + maxLat;
            }
            if ((maxLng != null) && (maxLng.Trim() != ""))
            {
                sql = sql + " AND zdjd<=" + maxLng;
            }
            if ((minLat != null) && (minLat.Trim() != ""))
            {
                sql = sql + " AND zdwd>=" + minLat;
            }
            if ((minLng != null) && (minLng.Trim() != ""))
            {
                sql = sql + " AND zdjd>=" + minLng;
            }
            if ((zdmc != null) && (zdmc.Trim() != ""))
            {
                sql = sql + " AND zdmc like '%" + zdmc + "%'";
            }
            if ((zdbh != null) && (zdbh.Trim() != ""))
            {
                sql = sql + " AND zdbh like '%" + zdbh + "%'";
            }
            sql = sql + " ORDER BY fseq ";
            if ((pagesize != null) && (pagesize.Trim() != ""))
            {
                sql = sql + " limit " + pagenum + "," + pagesize;
            }
            try
            {
                dt = _dbBaseUti.GetDataSet(sql).Tables[0];
                jsonStr = JsonConvert.SerializeObject(dt);
                resuStr = "{\"atmospheres\":" + jsonStr + "}";
                return resuStr;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public String getAtmosZDBH()
        {
            _dbBaseUti = _dbOperator.GetSubDbUtilities(EnumDBType.RCDB);
            string sql = "SELECT DISTINCT zdbh FROM atmosphere ORDER BY fseq";
            try
            {
                dt = _dbBaseUti.GetDataSet(sql).Tables[0];
                DataTable destDT = new DataTable("zdbh");
                //建立新DataTable
                destDT.Columns.Add("type");
                foreach (DataRow sdr in dt.Rows)
                {
                    DataRow ddr = destDT.NewRow();
                    ddr["type"] = sdr["zdbh"] != null ? sdr["zdbh"] : "未知";
                    destDT.Rows.Add(ddr);

                }
                //序列化DataTable得到jsonString
                jsonStr = JsonConvert.SerializeObject(destDT);
                resuStr = "{\"types\":" + jsonStr + "}";
                return resuStr;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public String getAtmosZDMC()
        {
            _dbBaseUti = _dbOperator.GetSubDbUtilities(EnumDBType.RCDB);
            string sql = "SELECT DISTINCT zdmc FROM atmosphere ORDER BY fseq";
            try
            {
                dt = _dbBaseUti.GetDataSet(sql).Tables[0];
                DataTable destDT = new DataTable("zdmc");
                destDT.Columns.Add("type");
                //建立新DataTable
                foreach (DataRow sdr in dt.Rows)
                {
                    DataRow ddr = destDT.NewRow();
                    ddr["type"] = sdr["zdmc"] != null ? sdr["zdmc"] : "未知";
                    destDT.Rows.Add(ddr);

                }
                //序列化DataTable得到jsonString
                jsonStr = JsonConvert.SerializeObject(destDT);
                resuStr = "{\"types\":" + jsonStr + "}";
                return resuStr;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        #endregion

        #region 城市目标
        public String getQueryCityObjs(String dts, String dte, String csmbmc, String csmblb, String pagenum, String pagesize)
        {
            _dbBaseUti = _dbOperator.GetSubDbUtilities(EnumDBType.RCDB);
            String sql = "select fseq,csmbmc,csmblb,gpsj,cljd,clwd,cdgd,clrq from city_obj where 1=1 ";
            if (((dts != null) && (dts.Trim() != "")) || (
              (dte != null) && (dte.Trim() != "")))
            {
                sql = sql + " AND clrq IS NOT NULL ";
            }
            if ((dts != null) && (dts.Trim() != ""))
            {
                sql =
                  sql + " AND (clrq between " + dts;
            }
            if ((dte != null) && (dte.Trim() != ""))
            {
                sql =
                  sql + " and " + dte + ") ";
            }
            if ((csmbmc != null) && (csmbmc.Trim() != ""))
            {
                sql = sql + " AND csmbmc like '%" + csmbmc + "%'";
            }
            if ((csmblb != null) && (csmblb.Trim() != ""))
            {
                sql = sql + " AND csmblb like '%" + csmblb + "%'";
            }
            sql = sql + " ORDER BY fseq ";
            if ((pagesize != null) && (pagesize.Trim() != ""))
            {
                sql = sql + " limit " + pagenum + "," + pagesize;
            }
            try
            {
                dt = _dbBaseUti.GetDataSet(sql).Tables[0];
                jsonStr = JsonConvert.SerializeObject(dt);
                resuStr = "{\"city_objs\":" + jsonStr + "}";
                return resuStr;
            }
            catch (Exception e)
            {
                throw e;
            }

        }

        public String getCityCSMBMC()
        {
            _dbBaseUti = _dbOperator.GetSubDbUtilities(EnumDBType.RCDB);
            string sql = "SELECT DISTINCT csmbmc FROM city_obj ORDER BY fseq";
            try
            {
                dt = _dbBaseUti.GetDataSet(sql).Tables[0];
                DataTable destDT = new DataTable("city_obj");
                //建立新DataTable
                destDT.Columns.Add("type");
                foreach (DataRow sdr in dt.Rows)
                {
                    DataRow ddr = destDT.NewRow();
                    ddr["type"] = (sdr["csmbmc"] != null) && (sdr["csmbmc"].ToString().Trim() != "") ? sdr["csmbmc"] : "未知";
                    destDT.Rows.Add(ddr);
                }
                //序列化DataTable得到jsonString
                jsonStr = JsonConvert.SerializeObject(destDT);
                resuStr = "{\"types\":" + jsonStr + "}";
                return resuStr;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public String getCityTypes()
        {
            _dbBaseUti = _dbOperator.GetSubDbUtilities(EnumDBType.RCDB);
            string sql = "SELECT DISTINCT csmblb FROM city_obj ORDER BY fseq";
            try
            {
                dt = _dbBaseUti.GetDataSet(sql).Tables[0];
                DataTable destDT = new DataTable("city_obj");
                //建立新DataTable
                destDT.Columns.Add("type");
                foreach (DataRow sdr in dt.Rows)
                {
                    DataRow ddr = destDT.NewRow();
                    ddr["type"] = (sdr["csmblb"] != null) && (sdr["csmblb"].ToString().Trim() != "") ? sdr["csmblb"] : "未知";
                    destDT.Rows.Add(ddr);
                }
                //序列化DataTable得到jsonString
                jsonStr = JsonConvert.SerializeObject(destDT);
                resuStr = "{\"types\":" + jsonStr + "}";
                return resuStr;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public String getCityGPSJ(String fseq)
        {
            _dbBaseUti = _dbOperator.GetSubDbUtilities(EnumDBType.RCDB);
            string sql = "SELECT gpsjsz FROM city_obj where fseq=" + fseq +
              ";";
            try
            {
                dt = _dbBaseUti.GetDataSet(sql).Tables[0];
                DataTable destDT = new DataTable("city_obj");
                //建立新DataTable
                destDT.Columns.Add("type");
                foreach (DataRow sdr in dt.Rows)
                {
                    DataRow ddr = destDT.NewRow();
                    ddr["type"] = sdr["gpsjsz"];
                    destDT.Rows.Add(ddr);
                }
                //序列化DataTable得到jsonString
                jsonStr = JsonConvert.SerializeObject(destDT);
                resuStr = "{\"types\":" + jsonStr + "}";
                return resuStr;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        #endregion

        #region 岩石
        public String getRockSSLB()
        {
            _dbBaseUti = _dbOperator.GetSubDbUtilities(EnumDBType.RCDB);
            string sql = "SELECT DISTINCT sslb FROM rock_mineral ORDER BY fseq";
            try
            {
                dt = _dbBaseUti.GetDataSet(sql).Tables[0];
                DataTable destDT = new DataTable("rock_mineral");
                //建立新DataTable
                destDT.Columns.Add("type");
                foreach (DataRow sdr in dt.Rows)
                {
                    DataRow ddr = destDT.NewRow();
                    ddr["type"] = (sdr["sslb"] != null) && (sdr["sslb"].ToString().Trim() != "") ? sdr["sslb"] : "未知";
                    destDT.Rows.Add(ddr);
                }
                //序列化DataTable得到jsonString
                jsonStr = JsonConvert.SerializeObject(destDT);
                resuStr = "{\"types\":" + jsonStr + "}";
                return resuStr;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public String getRockSubTypes()
        {
            _dbBaseUti = _dbOperator.GetSubDbUtilities(EnumDBType.RCDB);
            string sql = "SELECT DISTINCT ykzl FROM rock_mineral ORDER BY fseq";
            try
            {
                dt = _dbBaseUti.GetDataSet(sql).Tables[0];
                DataTable destDT = new DataTable("rock_mineral");
                //建立新DataTable
                destDT.Columns.Add("type");
                foreach (DataRow sdr in dt.Rows)
                {
                    DataRow ddr = destDT.NewRow();
                    ddr["type"] = (sdr["ykzl"] != null) && (sdr["ykzl"].ToString().Trim() != "") ? sdr["ykzl"] : "未知";
                    destDT.Rows.Add(ddr);
                }
                //序列化DataTable得到jsonString
                jsonStr = JsonConvert.SerializeObject(destDT);
                resuStr = "{\"types\":" + jsonStr + "}";
                return resuStr;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public String getRockTypes()
        {
            _dbBaseUti = _dbOperator.GetSubDbUtilities(EnumDBType.RCDB);
            string sql = "SELECT DISTINCT yklb FROM rock_mineral ORDER BY fseq";
            try
            {
                dt = _dbBaseUti.GetDataSet(sql).Tables[0];
                DataTable destDT = new DataTable("rock_mineral");
                //建立新DataTable
                destDT.Columns.Add("type");
                foreach (DataRow sdr in dt.Rows)
                {
                    DataRow ddr = destDT.NewRow();
                    ddr["type"] = (sdr["yklb"] != null) && (sdr["yklb"].ToString().Trim() != "") ? sdr["yklb"] : "未知";
                    destDT.Rows.Add(ddr);
                }
                //序列化DataTable得到jsonString
                jsonStr = JsonConvert.SerializeObject(destDT);
                resuStr = "{\"types\":" + jsonStr + "}";
                return resuStr;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public String getQueryRocks(String dts, String dte, String maxLat, String maxLng, String minLat, String minLng, String ykmc, String yklb, String ykzl, String sslb, String pagenum, String pagesize)
        {
            _dbBaseUti = _dbOperator.GetSubDbUtilities(EnumDBType.RCDB);
            String sql = "select fseq,ykmc,yklb,ykzl,gpsj,sslb,cljd,clwd,clrq from rock_mineral where 1=1 ";
            if (((dts != null) && (dts.Trim() != "")) || (
              (dte != null) && (dte.Trim() != "")))
            {
                sql = sql + " AND clrq IS NOT NULL ";
            }
            if ((dts != null) && (dts.Trim() != ""))
            {
                sql =
                  sql + " AND (clrq between '" + dts + "'";
            }
            if ((dte != null) && (dte.Trim() != ""))
            {
                sql =
                  sql + " and '" + dte + "') ";
            }
            if ((maxLat != null) && (maxLat.Trim() != ""))
            {
                sql = sql + " AND clwd<=" + maxLat;
            }
            if ((maxLng != null) && (maxLng.Trim() != ""))
            {
                sql = sql + " AND cljd<=" + maxLng;
            }
            if ((minLat != null) && (minLat.Trim() != ""))
            {
                sql = sql + " AND clwd>=" + minLat;
            }
            if ((minLng != null) && (minLng.Trim() != ""))
            {
                sql = sql + " AND cljd>=" + minLng;
            }
            if ((ykmc != null) && (ykmc.Trim() != ""))
            {
                sql = sql + " AND ykmc like '%" + ykmc + "%'";
            }
            if ((yklb != null) && (yklb.Trim() != ""))
            {
                sql = sql + " AND yklb like '%" + yklb + "%'";
            }
            if ((ykzl != null) && (ykzl.Trim() != ""))
            {
                sql = sql + " AND ykzl like '%" + ykzl + "%'";
            }
            if ((sslb != null) && (sslb.Trim() != ""))
            {
                sql = sql + " AND sslb like '%" + sslb + "%'";
            }
            sql = sql + " ORDER BY fseq ";
            if ((pagesize != null) && (pagesize.Trim() != ""))
            {
                sql = sql + " limit " + pagenum + "," + pagesize;
            }
            try
            {
                dt = _dbBaseUti.GetDataSet(sql).Tables[0];
                jsonStr = JsonConvert.SerializeObject(dt);
                resuStr = "{\"rocks\":" + jsonStr + "}";
                return resuStr;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public String getRockGPSJ(String fseq)
        {
            _dbBaseUti = _dbOperator.GetSubDbUtilities(EnumDBType.RCDB);
            try
            {
                string sql = "SELECT gpsjsz FROM rock_mineral where fseq=" + fseq +
        ";";
                dt = _dbBaseUti.GetDataSet(sql).Tables[0];
                DataTable destDT = new DataTable("rock_mineral");
                //建立新DataTable
                destDT.Columns.Add("type");
                foreach (DataRow sdr in dt.Rows)
                {
                    DataRow ddr = destDT.NewRow();
                    ddr["type"] = sdr["gpsjsz"];
                    destDT.Rows.Add(ddr);
                }
                //序列化DataTable得到jsonString
                jsonStr = JsonConvert.SerializeObject(destDT);
                resuStr = "{\"types\":" + jsonStr + "}";
                return resuStr;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        #endregion

        #region 土壤
        public String getSoilSubTypes()
        {
            _dbBaseUti = _dbOperator.GetSubDbUtilities(EnumDBType.RCDB);
            string sql = "SELECT DISTINCT trzl FROM soil ORDER BY fseq";
            try
            {
                dt = _dbBaseUti.GetDataSet(sql).Tables[0];
                DataTable destDT = new DataTable("soil");
                //建立新DataTable
                destDT.Columns.Add("type");
                foreach (DataRow sdr in dt.Rows)
                {
                    DataRow ddr = destDT.NewRow();
                    ddr["type"] = (sdr["trzl"] != null) && (sdr["trzl"].ToString().Trim() != "") ? sdr["trzl"] : "未知";
                    destDT.Rows.Add(ddr);
                }
                //序列化DataTable得到jsonString
                jsonStr = JsonConvert.SerializeObject(destDT);
                resuStr = "{\"types\":" + jsonStr + "}";
                return resuStr;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public String getQuerySoils(String dts, String dte, String maxLat, String maxLng, String minLat, String minLng, String trmc, String trzl, String fgwmc, String pagenum, String pagesize)
        {
            _dbBaseUti = _dbOperator.GetSubDbUtilities(EnumDBType.RCDB);
            String sql = "select fseq,trmc,trbm,trzl,gpsj,cljd,clwd,clrq from soil where 1=1 ";
            if (((dts != null) && (dts.Trim() != "")) || (
              (dte != null) && (dte.Trim() != "")))
            {
                sql = sql + " AND clrq IS NOT NULL ";
            }
            if ((dts != null) && (dts.Trim() != ""))
            {
                sql =
                  sql + " AND (clrq between '" + dts + "'";
            }
            if ((dte != null) && (dte.Trim() != ""))
            {
                sql =
                  sql + " and '" + dte + "')";
            }
            if ((maxLat != null) && (maxLat.Trim() != ""))
            {
                sql = sql + " AND clwd<=" + maxLat;
            }
            if ((maxLng != null) && (maxLng.Trim() != ""))
            {
                sql = sql + " AND cljd<=" + maxLng;
            }
            if ((minLat != null) && (minLat.Trim() != ""))
            {
                sql = sql + " AND clwd>=" + minLat;
            }
            if ((minLng != null) && (minLng.Trim() != ""))
            {
                sql = sql + " AND cljd>=" + minLng;
            }
            if ((trmc != null) && (trmc.Trim() != ""))
            {
                sql = sql + " AND trmc like '%" + trmc + "%'";
            }
            if ((trzl != null) && (trzl.Trim() != ""))
            {
                sql = sql + " AND trzl like '%" + trzl + "%'";
            }
            if ((fgwmc != null) && (fgwmc.Trim() != ""))
            {
                sql = sql + " AND fgwmc like '%" + fgwmc + "%'";
            }
            sql = sql + " ORDER BY fseq ";
            if ((pagesize != null) && (pagesize.Trim() != ""))
            {
                sql = sql + " limit " + pagenum + "," + pagesize;
            }
            try
            {
                dt = _dbBaseUti.GetDataSet(sql).Tables[0];
                jsonStr = JsonConvert.SerializeObject(dt);
                resuStr = "{\"soils\":" + jsonStr + "}";
                return resuStr;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public String getSoilGPSJ(String fseq)
        {
            _dbBaseUti = _dbOperator.GetSubDbUtilities(EnumDBType.RCDB);
            try
            {
                string sql = "SELECT gpsjsz FROM soil where fseq=" + fseq +
            ";";
                dt = _dbBaseUti.GetDataSet(sql).Tables[0];
                DataTable destDT = new DataTable("soil");
                //建立新DataTable
                destDT.Columns.Add("type");
                foreach (DataRow sdr in dt.Rows)
                {
                    DataRow ddr = destDT.NewRow();
                    ddr["type"] = sdr["gpsjsz"];
                    destDT.Rows.Add(ddr);
                }
                //序列化DataTable得到jsonString
                jsonStr = JsonConvert.SerializeObject(destDT);
                resuStr = "{\"types\":" + jsonStr + "}";
                return resuStr;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        #endregion

        #region 北方植被
        public String getNorVegWHQ()
        {
            _dbBaseUti = _dbOperator.GetSubDbUtilities(EnumDBType.RCDB);
            string sql = "SELECT DISTINCT whq FROM vegetation_north ORDER BY fseq";
            try
            {
                dt = _dbBaseUti.GetDataSet(sql).Tables[0];
                DataTable destDT = new DataTable("vegetation_north");
                //建立新DataTable
                destDT.Columns.Add("type");
                foreach (DataRow sdr in dt.Rows)
                {
                    DataRow ddr = destDT.NewRow();
                    ddr["type"] = (sdr["whq"] != null) && (sdr["whq"].ToString().Trim() != "") ? sdr["whq"] : "未知";
                    destDT.Rows.Add(ddr);
                }
                //序列化DataTable得到jsonString
                jsonStr = JsonConvert.SerializeObject(destDT);
                resuStr = "{\"types\":" + jsonStr + "}";
                return resuStr;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public String getNorVegCLBW()
        {
            _dbBaseUti = _dbOperator.GetSubDbUtilities(EnumDBType.RCDB);
            string sql = "SELECT DISTINCT clbw FROM vegetation_north ORDER BY fseq";
            try
            {
                dt = _dbBaseUti.GetDataSet(sql).Tables[0];
                DataTable destDT = new DataTable("vegetation_north");
                //建立新DataTable
                destDT.Columns.Add("type");
                foreach (DataRow sdr in dt.Rows)
                {
                    DataRow ddr = destDT.NewRow();
                    ddr["type"] = (sdr["clbw"] != null) && (sdr["clbw"].ToString().Trim() != "") ? sdr["clbw"] : "未知";
                    destDT.Rows.Add(ddr);
                }
                //序列化DataTable得到jsonString
                jsonStr = JsonConvert.SerializeObject(destDT);
                resuStr = "{\"types\":" + jsonStr + "}";
                return resuStr;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public String getNorVegTypes()
        {
            _dbBaseUti = _dbOperator.GetSubDbUtilities(EnumDBType.RCDB);
            string sql = "SELECT DISTINCT zblb FROM vegetation_north ORDER BY fseq";
            try
            {
                dt = _dbBaseUti.GetDataSet(sql).Tables[0];
                DataTable destDT = new DataTable("vegetation_north");
                //建立新DataTable
                destDT.Columns.Add("type");
                foreach (DataRow sdr in dt.Rows)
                {
                    DataRow ddr = destDT.NewRow();
                    ddr["type"] = (sdr["zblb"] != null) && (sdr["zblb"].ToString().Trim() != "") ? sdr["zblb"] : "未知";
                    destDT.Rows.Add(ddr);
                }
                //序列化DataTable得到jsonString
                jsonStr = JsonConvert.SerializeObject(destDT);
                resuStr = "{\"types\":" + jsonStr + "}";
                return resuStr;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public String getQueryNorVegetations(String dts, String dte, String maxLat, String maxLng, String minLat, String minLng, String zbmc, String zblb, String clbw, String whq, String pagenum, String pagesize)
        {
            _dbBaseUti = _dbOperator.GetSubDbUtilities(EnumDBType.RCDB);
            String sql = "select fseq,zbmc,zblb,clbw,gpsj,cljd,clwd,clrq,whq from vegetation_north where 1=1 ";
            if (((dts != null) && (dts.Trim() != "")) || (
              (dte != null) && (dte.Trim() != "")))
            {
                sql = sql + " AND clrq IS NOT NULL ";
            }
            if ((dts != null) && (dts.Trim() != ""))
            {
                sql =
                  sql + " AND (clrq between " + dts;
            }
            if ((dte != null) && (dte.Trim() != ""))
            {
                sql =
                  sql + " and " + dte + ") ";
            }
            if ((maxLat != null) && (maxLat.Trim() != ""))
            {
                sql = sql + " AND clwd<=" + maxLat;
            }
            if ((maxLng != null) && (maxLng.Trim() != ""))
            {
                sql = sql + " AND cljd<=" + maxLng;
            }
            if ((minLat != null) && (minLat.Trim() != ""))
            {
                sql = sql + " AND clwd>=" + minLat;
            }
            if ((minLng != null) && (minLng.Trim() != ""))
            {
                sql = sql + " AND cljd>=" + minLng;
            }
            if ((zbmc != null) && (zbmc.Trim() != ""))
            {
                sql = sql + " AND zbmc like '%" + zbmc + "%'";
            }
            if ((zblb != null) && (zblb.Trim() != ""))
            {
                sql = sql + " AND zblb like '%" + zblb + "%'";
            }
            if ((clbw != null) && (clbw.Trim() != ""))
            {
                sql = sql + " AND clbw like '%" + clbw + "%'";
            }
            if ((whq != null) && (whq.Trim() != ""))
            {
                sql = sql + " AND whq like '%" + whq + "%'";
            }
            sql = sql + " ORDER BY fseq ";
            if ((pagesize != null) && (pagesize.Trim() != ""))
            {
                sql = sql + " limit " + pagenum + "," + pagesize;
            }
            try
            {
                dt = _dbBaseUti.GetDataSet(sql).Tables[0];
                jsonStr = JsonConvert.SerializeObject(dt);
                resuStr = "{\"vegetations\":" + jsonStr + "}";
                return resuStr;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public String getNorVegGPSJ(String fseq)
        {
            _dbBaseUti = _dbOperator.GetSubDbUtilities(EnumDBType.RCDB);
            try
            {
                string sql = "SELECT gpsjsz FROM vegetation_north where fseq=" + fseq +
            ";";
                dt = _dbBaseUti.GetDataSet(sql).Tables[0];
                DataTable destDT = new DataTable("vegetation_north");
                //建立新DataTable
                destDT.Columns.Add("type");
                foreach (DataRow sdr in dt.Rows)
                {
                    DataRow ddr = destDT.NewRow();
                    ddr["type"] = sdr["gpsjsz"];
                    destDT.Rows.Add(ddr);
                }
                //序列化DataTable得到jsonString
                jsonStr = JsonConvert.SerializeObject(destDT);
                resuStr = "{\"types\":" + jsonStr + "}";
                return resuStr;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        #endregion

        #region 南方植被
        public String getSouVegWHQ()
        {
            _dbBaseUti = _dbOperator.GetSubDbUtilities(EnumDBType.RCDB);
            string sql = "SELECT DISTINCT whq FROM vegetation_south ORDER BY fseq";
            try
            {
                dt = _dbBaseUti.GetDataSet(sql).Tables[0];
                DataTable destDT = new DataTable("vegetation_south");
                //建立新DataTable
                destDT.Columns.Add("type");
                foreach (DataRow sdr in dt.Rows)
                {
                    DataRow ddr = destDT.NewRow();
                    ddr["type"] = (sdr["whq"] != null) && (sdr["whq"].ToString().Trim() != "") ? sdr["whq"] : "未知";
                    destDT.Rows.Add(ddr);
                }
                //序列化DataTable得到jsonString
                jsonStr = JsonConvert.SerializeObject(destDT);
                resuStr = "{\"types\":" + jsonStr + "}";
                return resuStr;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public String getSouVegCLBW()
        {
            _dbBaseUti = _dbOperator.GetSubDbUtilities(EnumDBType.RCDB);
            string sql = "SELECT DISTINCT clbw FROM vegetation_south ORDER BY fseq";
            try
            {
                dt = _dbBaseUti.GetDataSet(sql).Tables[0];
                DataTable destDT = new DataTable("vegetation_south");
                //建立新DataTable
                destDT.Columns.Add("type");
                foreach (DataRow sdr in dt.Rows)
                {
                    DataRow ddr = destDT.NewRow();
                    ddr["type"] = (sdr["clbw"] != null) && (sdr["clbw"].ToString().Trim() != "") ? sdr["clbw"] : "未知";
                    destDT.Rows.Add(ddr);
                }
                //序列化DataTable得到jsonString
                jsonStr = JsonConvert.SerializeObject(destDT);
                resuStr = "{\"types\":" + jsonStr + "}";
                return resuStr;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public String getSouVegTypes()
        {
            _dbBaseUti = _dbOperator.GetSubDbUtilities(EnumDBType.RCDB);
            string sql = "SELECT DISTINCT zblb FROM vegetation_south ORDER BY fseq";
            try
            {
                dt = _dbBaseUti.GetDataSet(sql).Tables[0];
                DataTable destDT = new DataTable("vegetation_south");
                //建立新DataTable
                destDT.Columns.Add("type");
                foreach (DataRow sdr in dt.Rows)
                {
                    DataRow ddr = destDT.NewRow();
                    ddr["type"] = (sdr["zblb"] != null) && (sdr["zblb"].ToString().Trim() != "") ? sdr["zblb"] : "未知";
                    destDT.Rows.Add(ddr);
                }
                //序列化DataTable得到jsonString
                jsonStr = JsonConvert.SerializeObject(destDT);
                resuStr = "{\"types\":" + jsonStr + "}";
                return resuStr;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public String getQueryVegetations(String dts, String dte, String maxLat, String maxLng, String minLat, String minLng, String zbmc, String zblb, String clbw, String whq, String pagenum, String pagesize)
        {
            _dbBaseUti = _dbOperator.GetSubDbUtilities(EnumDBType.RCDB);
            String sql = "select fseq,zbmc,zblb,clbw,gpsj,cljd,clwd,clrq,whq from vegetation_south where 1=1 ";
            if (((dts != null) && (dts.Trim() != "")) || ((dte != null) && (dte.Trim() != "")))
            {
                sql = sql + " AND clrq IS NOT NULL ";
            }
            if ((dts != null) && (dts.Trim() != ""))
            {
                sql =
                  sql + " AND (clrq between " + dts;
            }
            if ((dte != null) && (dte.Trim() != ""))
            {
                sql =
                  sql + " and " + dte + ")";
            }
            if ((maxLat != null) && (maxLat.Trim() != ""))
            {
                sql = sql + " AND clwd<=" + maxLat;
            }
            if ((maxLng != null) && (maxLng.Trim() != ""))
            {
                sql = sql + " AND cljd<=" + maxLng;
            }
            if ((minLat != null) && (minLat.Trim() != ""))
            {
                sql = sql + " AND clwd>=" + minLat;
            }
            if ((minLng != null) && (minLng.Trim() != ""))
            {
                sql = sql + " AND cljd>=" + minLng;
            }
            if ((zbmc != null) && (zbmc.Trim() != ""))
            {
                sql = sql + " AND zbmc like '%" + zbmc + "%'";
            }
            if ((zblb != null) && (zblb.Trim() != ""))
            {
                sql = sql + " AND zblb like '%" + zblb + "%'";
            }
            if ((clbw != null) && (clbw.Trim() != ""))
            {
                sql = sql + " AND clbw like '%" + clbw + "%'";
            }
            if ((whq != null) && (whq.Trim() != ""))
            {
                sql = sql + " AND whq like '%" + whq + "%'";
            }
            sql = sql + " ORDER BY fseq ";
            if ((pagesize != null) && (pagesize.Trim() != ""))
            {
                sql = sql + " limit " + pagenum + "," + pagesize;
            }
            try
            {
                dt = _dbBaseUti.GetDataSet(sql).Tables[0];
                jsonStr = JsonConvert.SerializeObject(dt);
                resuStr = "{\"vegetations\":" + jsonStr + "}";
                return resuStr;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public String getSouVegGPSJ(String fseq)
        {
            _dbBaseUti = _dbOperator.GetSubDbUtilities(EnumDBType.RCDB);
            try
            {
                string sql = "SELECT gpsjsz FROM vegetation_south where fseq=" + fseq +
            ";";
                dt = _dbBaseUti.GetDataSet(sql).Tables[0];
                DataTable destDT = new DataTable("vegetation_south");
                //建立新DataTable
                destDT.Columns.Add("type");
                foreach (DataRow sdr in dt.Rows)
                {
                    DataRow ddr = destDT.NewRow();
                    ddr["type"] = sdr["gpsjsz"];
                    destDT.Rows.Add(ddr);
                }
                //序列化DataTable得到jsonString
                jsonStr = JsonConvert.SerializeObject(destDT);
                resuStr = "{\"types\":" + jsonStr + "}";
                return resuStr;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        #endregion

        #region 水
        public String getWaterGPYQ()
        {
            _dbBaseUti = _dbOperator.GetSubDbUtilities(EnumDBType.RCDB);
            string sql = "SELECT DISTINCT gpyq FROM water ORDER BY fseq";
            try
            {
                dt = _dbBaseUti.GetDataSet(sql).Tables[0];
                DataTable destDT = new DataTable("water");
                //建立新DataTable
                destDT.Columns.Add("type");
                foreach (DataRow sdr in dt.Rows)
                {
                    DataRow ddr = destDT.NewRow();
                    ddr["type"] = (sdr["gpyq"] != null) && (sdr["gpyq"].ToString().Trim() != "") ? sdr["gpyq"] : "未知";
                    destDT.Rows.Add(ddr);
                }
                //序列化DataTable得到jsonString
                jsonStr = JsonConvert.SerializeObject(destDT);
                resuStr = "{\"types\":" + jsonStr + "}";
                return resuStr;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public String getWaterSSLB()
        {
            _dbBaseUti = _dbOperator.GetSubDbUtilities(EnumDBType.RCDB);
            string sql = "SELECT DISTINCT sslb FROM water ORDER BY fseq";
            try
            {
                dt = _dbBaseUti.GetDataSet(sql).Tables[0];
                DataTable destDT = new DataTable("water");
                //建立新DataTable
                destDT.Columns.Add("type");
                foreach (DataRow sdr in dt.Rows)
                {
                    DataRow ddr = destDT.NewRow();
                    ddr["type"] = (sdr["sslb"] != null) && (sdr["sslb"].ToString().Trim() != "") ? sdr["sslb"] : "未知";
                    destDT.Rows.Add(ddr);
                }
                //序列化DataTable得到jsonString
                jsonStr = JsonConvert.SerializeObject(destDT);
                resuStr = "{\"types\":" + jsonStr + "}";
                return resuStr;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public String getWaterSYMC()
        {
            _dbBaseUti = _dbOperator.GetSubDbUtilities(EnumDBType.RCDB);
            string sql = "SELECT DISTINCT symc FROM water ORDER BY fseq";
            try
            {
                dt = _dbBaseUti.GetDataSet(sql).Tables[0];
                DataTable destDT = new DataTable("water");
                //建立新DataTable
                destDT.Columns.Add("type");
                foreach (DataRow sdr in dt.Rows)
                {
                    DataRow ddr = destDT.NewRow();
                    ddr["type"] = (sdr["symc"] != null) && (sdr["symc"].ToString().Trim() != "") ? sdr["symc"] : "未知";
                    destDT.Rows.Add(ddr);
                }
                //序列化DataTable得到jsonString
                jsonStr = JsonConvert.SerializeObject(destDT);
                resuStr = "{\"types\":" + jsonStr + "}";
                return resuStr;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public String getQueryWaters(String dts, String dte, String maxLat, String maxLng, String minLat, String minLng, String symc, String gpyq, String sslb, String pagenum, String pagesize)
        {
            _dbBaseUti = _dbOperator.GetSubDbUtilities(EnumDBType.RCDB);
            String sql = "select fseq,symc,sslb,gpsj,cljd,clwd,clrq from water where 1=1 ";
            if (((dts != null) && (dts.Trim() != "")) || (
              (dte != null) && (dte.Trim() != "")))
            {
                sql = sql + " AND clrq IS NOT NULL ";
            }
            if ((dts != null) && (dts.Trim() != ""))
            {
                sql =
                  sql + " AND (clrq between '" + dts + "'";
            }
            if ((dte != null) && (dte.Trim() != ""))
            {
                sql =
                  sql + " and '" + dte + "') ";
            }
            if ((maxLat != null) && (maxLat.Trim() != ""))
            {
                sql = sql + " AND clwd<=" + maxLat;
            }
            if ((maxLng != null) && (maxLng.Trim() != ""))
            {
                sql = sql + " AND cljd<=" + maxLng;
            }
            if ((minLat != null) && (minLat.Trim() != ""))
            {
                sql = sql + " AND clwd>=" + minLat;
            }
            if ((minLng != null) && (minLng.Trim() != ""))
            {
                sql = sql + " AND cljd>=" + minLng;
            }
            if ((symc != null) && (symc.Trim() != ""))
            {
                sql = sql + " AND symc like '%" + symc + "%'";
            }
            if ((gpyq != null) && (gpyq.Trim() != ""))
            {
                sql = sql + " AND gpyq like '%" + gpyq + "%'";
            }
            if ((sslb != null) && (sslb.Trim() != ""))
            {
                sql = sql + " AND sslb like '%" + sslb + "%'";
            }
            sql = sql + " ORDER BY fseq ";
            if ((pagesize != null) && (pagesize.Trim() != ""))
            {
                sql = sql + " limit " + pagenum + "," + pagesize;
            }
            try
            {
                dt = _dbBaseUti.GetDataSet(sql).Tables[0];
                jsonStr = JsonConvert.SerializeObject(dt);
                resuStr = "{\"waters\":" + jsonStr + "}";
                return resuStr;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public String getWaterGPSJ(String fseq)
        {
            _dbBaseUti = _dbOperator.GetSubDbUtilities(EnumDBType.RCDB);
            try
            {
                string sql = "SELECT gpsjsz FROM water where fseq=" + fseq +
            ";";
                dt = _dbBaseUti.GetDataSet(sql).Tables[0];
                DataTable destDT = new DataTable("water");
                //建立新DataTable
                destDT.Columns.Add("type");
                foreach (DataRow sdr in dt.Rows)
                {
                    DataRow ddr = destDT.NewRow();
                    ddr["type"] = sdr["gpsjsz"];
                    destDT.Rows.Add(ddr);
                }
                //序列化DataTable得到jsonString
                jsonStr = JsonConvert.SerializeObject(destDT);
                resuStr = "{\"types\":" + jsonStr + "}";
                return resuStr;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        #endregion
        #endregion

        #region 集成共享网站使用_波普特征数据查询
        public System.Data.DataSet GetBPNames()
        {
            string sql = "select NAME from metadatacatalognode where NAME !='遥感应用特征数据库'";
            //if (mySQLOperator == null)
            //{
            //    mySQLOperator = new DBMySqlOperating();
            //}
            //MySqlBaseUti = mySQLOperator.RCDB;
            _dbBaseUti = _dbOperator.GetSubDbUtilities(EnumDBType.RCDB);
            returnDS = _dbBaseUti.GetDataSet(sql);
            return returnDS;
        }

        public DataTable GetSoilBPRecord(string begindate, string enddate, string Maxlat, string MaxLon, string MinLat, string MinLon, string soilname, string soilzilei, string pagenum, string pagesize)
        {
            //注：暂未进行参数验证。参数测试

            dt = new DataTable();
            //        SoilService.SoilServicePortTypeClient soilclient;
            //        soilclient = new SoilService.SoilServicePortTypeClient("SoilServiceHttpSoap12Endpoint");
            fileinfolist = new List<object>();
            if (pagenum == null || pagenum.Equals(""))
            {
                pagenum = "0";
            }
            if (pagesize == null || pagesize.Equals(""))
            {
                pagesize = "50";
            }

            string jsonStr = getQuerySoils(begindate, enddate, Maxlat, MaxLon, MinLat, MinLon, soilname, soilzilei, "", pagenum, pagesize);
            Soil soil = JSON.parse<Soil>(jsonStr);
            foreach (SoilInfo info in soil.soils)
            {
                fileinfolist.Add(info);
            }
            dt = Utilties.ConvertObjPro2DataTable(fileinfolist);
            dt.TableName = "Soil";
            for (int i = 0; i < dt.Columns.Count; i++)
            {
                dt.Columns[i].ColumnName = Soil.soilattributenames[i];
            }
            return dt;
        }

        public string[] GetSoilTypes()
        {
            //        soilclient = new SoilService.SoilServicePortTypeClient("SoilServiceHttpSoap12Endpoint");
            jsonStr = getSoilSubTypes();
            classify = JSON.parse<GetClassify>(jsonStr);
            classifyfgw = classify.types;
            strFields = new string[classifyfgw.Length];
            for (int i = 0; i < classifyfgw.Length; i++)
            {
                strFields[i] = classifyfgw[i].type;
            }
            return strFields;
        }

        public DataTable GetRockBPRecord(string begindate, string enddate, string Maxlat, string MaxLon, string MinLat, string MinLon, string r_ykmc, string r_yklb, string r_ykzl, string r_sslb, string pagenum, string pagesize)
        {
            dt = new DataTable();
            //参数测试
            //        RockService.RockServicePortTypeClient rockclient;
            //        rockclient = new RockService.RockServicePortTypeClient("RockServiceHttpSoap12Endpoint");
            fileinfolist = new List<object>();
            if (pagenum == null || pagenum.Equals(""))
            {
                pagenum = "0";
            }
            if (pagesize == null || pagesize.Equals(""))
            {
                pagesize = "50";
            }
            jsonStr = getQueryRocks(begindate, enddate, Maxlat, MaxLon, MinLat, MinLon, r_ykmc, r_yklb, r_ykzl, r_sslb, pagenum, pagesize);
            RockMineral rockmineral = JSON.parse<RockMineral>(jsonStr);
            foreach (RockMineralInfo info in rockmineral.rocks)
            {
                fileinfolist.Add(info);
            }
            dt = Utilties.ConvertObjPro2DataTable(fileinfolist);
            dt.TableName = "Rock";
            for (int i = 0; i < dt.Columns.Count; i++)
            {
                dt.Columns[i].ColumnName = RockMineral.rockMineralattributenames[i];
            }
            return dt;
        }

        public string[] GetRockType()
        {
            //        rockclient = new RockService.RockServicePortTypeClient("RockServiceHttpSoap12Endpoint");
            jsonStr = getRockTypes();
            classify = JSON.parse<GetClassify>(jsonStr);
            classifyfgw = classify.types;
            strFields = new string[classifyfgw.Length];
            for (int i = 0; i < classifyfgw.Length; i++)
            {
                strFields[i] = classifyfgw[i].type;
            }
            return strFields;
        }

        public string[] GetRockSubType()
        {
            //        rockclient = new RockService.RockServicePortTypeClient("RockServiceHttpSoap12Endpoint");
            jsonStr = getRockSubTypes();
            classify = JSON.parse<GetClassify>(jsonStr);
            classifyfgw = classify.types;
            strFields = new string[classifyfgw.Length];
            for (int i = 0; i < classifyfgw.Length; i++)
            {
                strFields[i] = classifyfgw[i].type;
            }
            return strFields;
        }

        public string[] GetRockSSLR()
        {
            //        rockclient = new RockService.RockServicePortTypeClient("RockServiceHttpSoap12Endpoint");
            jsonStr = getRockSSLB();
            classify = JSON.parse<GetClassify>(jsonStr);
            classifyfgw = classify.types;
            strFields = new string[classifyfgw.Length];
            for (int i = 0; i < classifyfgw.Length; i++)
            {
                strFields[i] = classifyfgw[i].type;
            }
            return strFields;
        }

        public DataTable GetNorthVegBPRecord(string begindate, string enddate, string Maxlat, string MaxLon, string MinLat, string MinLon, string v_zbmc, string v_zblb, string v_clbw, string v_whq, string pagenum, string pagesize)
        {
            dt = new DataTable();
            fileinfolist = new List<object>();
            //        VegetationNorthService.VegetationNorthServicePortTypeClient vnorthclient;
            //        vnorthclient = new VegetationNorthService.VegetationNorthServicePortTypeClient("VegetationNorthServiceHttpSoap12Endpoint");
            if (pagenum == null || pagenum.Equals(""))
            {
                pagenum = "0";
            }
            if (pagesize == null || pagesize.Equals(""))
            {
                pagesize = "50";
            }
            jsonStr = getQueryNorVegetations(begindate, enddate, Maxlat, MaxLon, MinLat, MinLon, v_zbmc, v_zblb, v_clbw, v_whq, pagenum, pagesize);
            Vegetation vegetation = JSON.parse<Vegetation>(jsonStr);
            foreach (VegetationInfo info in vegetation.vegetations)
            {
                fileinfolist.Add(info);
            }
            dt = Utilties.ConvertObjPro2DataTable(fileinfolist);
            dt.TableName = "NorthVeg";
            for (int i = 0; i < dt.Columns.Count; i++)
            {
                dt.Columns[i].ColumnName = Vegetation.vegetationattributenames[i];
            }
            return dt;
        }

        public string[] GetNorthVegType()
        {
            //        vnorthclient = new VegetationNorthService.VegetationNorthServicePortTypeClient("VegetationNorthServiceHttpSoap11Endpoint");
            jsonStr = getNorVegTypes();
            classify = JSON.parse<GetClassify>(jsonStr);
            classifyfgw = classify.types;
            strFields = new string[classifyfgw.Length];
            for (int i = 0; i < classifyfgw.Length; i++)
            {
                strFields[i] = classifyfgw[i].type;
            }
            return strFields;
        }

        public string[] GetNorthVegBW()
        {
            //        vnorthclient = new VegetationNorthService.VegetationNorthServicePortTypeClient("VegetationNorthServiceHttpSoap11Endpoint");
            jsonStr = getNorVegCLBW();
            classify = JSON.parse<GetClassify>(jsonStr);
            classifyfgw = classify.types;
            strFields = new string[classifyfgw.Length];
            for (int i = 0; i < classifyfgw.Length; i++)
            {
                strFields[i] = classifyfgw[i].type;
            }
            return strFields;
        }

        public string[] GetNorthVegWHQ()
        {
            //        vnorthclient = new VegetationNorthService.VegetationNorthServicePortTypeClient("VegetationNorthServiceHttpSoap11Endpoint");
            jsonStr = getNorVegWHQ();
            classify = JSON.parse<GetClassify>(jsonStr);
            classifyfgw = classify.types;
            strFields = new string[classifyfgw.Length];
            for (int i = 0; i < classifyfgw.Length; i++)
            {
                strFields[i] = classifyfgw[i].type;
            }
            return strFields;
        }

        public DataTable GetSouthVegBPRecord(string begindate, string enddate, string Maxlat, string MaxLon, string MinLat, string MinLon, string v_zbmc, string v_zblb, string v_clbw, string v_whq, string pagenum, string pagesize)
        {
            dt = new DataTable();
            fileinfolist = new List<object>();
            //        VegetationSouthService.VegetationSouthServicePortTypeClient vsouthclient;
            //        vsouthclient = new VegetationSouthService.VegetationSouthServicePortTypeClient("VegetationSouthServiceHttpSoap12Endpoint");
            if (pagenum == null || pagenum.Equals(""))
            {
                pagenum = "0";
            }
            if (pagesize == null || pagesize.Equals(""))
            {
                pagesize = "50";
            }
            jsonStr = getQueryVegetations(begindate, enddate, Maxlat, MaxLon, MinLat, MinLon, v_zbmc, v_zblb, v_clbw, v_whq, pagenum, pagesize);
            Vegetation vegetation = JSON.parse<Vegetation>(jsonStr);
            foreach (VegetationInfo info in vegetation.vegetations)
            {
                fileinfolist.Add(info);
            }
            dt = Utilties.ConvertObjPro2DataTable(fileinfolist);
            dt.TableName = "SouthVeg";
            for (int i = 0; i < dt.Columns.Count; i++)
            {
                dt.Columns[i].ColumnName = Vegetation.vegetationattributenames[i];
            }
            return dt;
        }

        public string[] GetSouthVegType()
        {
            //        vsouthclient = new VegetationSouthService.VegetationSouthServicePortTypeClient("VegetationSouthServiceHttpSoap12Endpoint");
            jsonStr = getSouVegTypes();
            classify = JSON.parse<GetClassify>(jsonStr);
            classifyfgw = classify.types;
            strFields = new string[classifyfgw.Length];
            for (int i = 0; i < classifyfgw.Length; i++)
            {
                strFields[i] = classifyfgw[i].type;
            }
            return strFields;
        }

        public string[] GetSouthVegBW()
        {
            //        vsouthclient = new VegetationSouthService.VegetationSouthServicePortTypeClient("VegetationSouthServiceHttpSoap12Endpoint");
            jsonStr = getSouVegCLBW();
            classify = JSON.parse<GetClassify>(jsonStr);
            classifyfgw = classify.types;
            strFields = new string[classifyfgw.Length];
            for (int i = 0; i < classifyfgw.Length; i++)
            {
                strFields[i] = classifyfgw[i].type;
            }
            return strFields;
        }

        public string[] GetSouthVegWHQ()
        {
            //        vsouthclient = new VegetationSouthService.VegetationSouthServicePortTypeClient("VegetationSouthServiceHttpSoap12Endpoint");
            jsonStr = getSouVegWHQ();
            classify = JSON.parse<GetClassify>(jsonStr);
            classifyfgw = classify.types;
            strFields = new string[classifyfgw.Length];
            for (int i = 0; i < classifyfgw.Length; i++)
            {
                strFields[i] = classifyfgw[i].type;
            }
            return strFields;
        }

        public DataTable GetAtomspBPRecord(string Maxlat, string MaxLon, string MinLat, string MinLon, string a_zdmc, string a_zdbh, string pagenum, string pagesize)
        {
            dt = new DataTable();
            fileinfolist = new List<object>();
            if (pagenum == null || pagenum.Equals(""))
            {
                pagenum = "0";
            }
            if (pagesize == null || pagesize.Equals(""))
            {
                pagesize = "50";
            }
            jsonStr = getQueryAtmospheres(Maxlat, MaxLon, MinLat, MinLon, a_zdmc, a_zdbh, pagenum, pagesize);
            Atmosphere atmosp = JSON.parse<Atmosphere>(jsonStr);
            foreach (AtmosphereInfo info in atmosp.atmospheres)
            {
                fileinfolist.Add(info);
            }
            dt = Utilties.ConvertObjPro2DataTable(fileinfolist);
            dt.TableName = "Atmosp";
            for (int i = 0; i < dt.Columns.Count; i++)
            {
                dt.Columns[i].ColumnName = Atmosphere.atmosphereattributenames[i];
            }
            return dt;
        }

        public string[] GetAtomsDZMC()
        {
            //        atmosClient = new AtmosphereService.AtmosphereServicePortTypeClient("AtmosphereServiceHttpSoap12Endpoint");
            jsonStr = getAtmosZDMC();
            classify = JSON.parse<GetClassify>(jsonStr);
            classifyfgw = classify.types;
            strFields = new string[classifyfgw.Length];
            for (int i = 0; i < classifyfgw.Length; i++)
            {
                strFields[i] = classifyfgw[i].type;
            }
            return strFields;
        }

        public string[] GetAtomsDZBH()
        {
            //        atmosClient = new AtmosphereService.AtmosphereServicePortTypeClient("AtmosphereServiceHttpSoap12Endpoint");
            jsonStr = getAtmosZDBH();
            classify = JSON.parse<GetClassify>(jsonStr);
            classifyfgw = classify.types;
            strFields = new string[classifyfgw.Length];
            for (int i = 0; i < classifyfgw.Length; i++)
            {
                strFields[i] = classifyfgw[i].type;
            }
            return strFields;
        }

        public DataTable GetWaterBPRecord(string begindate, string enddate, string Maxlat, string MaxLon, string MinLat, string MinLon, string w_symc, string w_gpyq, string w_sslb, string pagenum, string pagesize)
        {
            dt = new DataTable();
            fileinfolist = new List<object>();
            //        WaterService.WaterServicePortTypeClient waterclient;
            //        waterclient = new WaterService.WaterServicePortTypeClient("WaterServiceHttpSoap12Endpoint");
            if (pagenum == null || pagenum.Equals(""))
            {
                pagenum = "0";
            }
            if (pagesize == null || pagesize.Equals(""))
            {
                pagesize = "50";
            }
            jsonStr = getQueryWaters(begindate, enddate, Maxlat, MaxLon, MinLat, MinLon, w_symc, w_gpyq, w_sslb, pagenum, pagesize);
            Water water = JSON.parse<Water>(jsonStr);
            foreach (WaterInfo info in water.waters)
            {
                fileinfolist.Add(info);
            }
            dt = Utilties.ConvertObjPro2DataTable(fileinfolist);
            dt.TableName = "Water";
            for (int i = 0; i < dt.Columns.Count; i++)
            {
                dt.Columns[i].ColumnName = Water.waterattributenames[i];
            }
            return dt;
        }

        public string[] GetWaterSYMC()
        {
            //        waterclient = new WaterService.WaterServicePortTypeClient("WaterServiceHttpSoap12Endpoint");
            jsonStr = getWaterSYMC();
            classify = JSON.parse<GetClassify>(jsonStr);
            classifyfgw = classify.types;
            strFields = new string[classifyfgw.Length];
            for (int i = 0; i < classifyfgw.Length; i++)
            {
                strFields[i] = classifyfgw[i].type;
            }
            return strFields;
        }

        public string[] GetWaterType()
        {
            //        waterclient = new WaterService.WaterServicePortTypeClient("WaterServiceHttpSoap12Endpoint");
            jsonStr = getWaterSSLB();
            classify = JSON.parse<GetClassify>(jsonStr);
            classifyfgw = classify.types;
            strFields = new string[classifyfgw.Length];
            for (int i = 0; i < classifyfgw.Length; i++)
            {
                strFields[i] = classifyfgw[i].type;
            }
            return strFields;
        }

        public string[] GetWaterGPYQ()
        {
            jsonStr = getWaterGPYQ();
            classify = JSON.parse<GetClassify>(jsonStr);
            classifyfgw = classify.types;
            strFields = new string[classifyfgw.Length];
            for (int i = 0; i < classifyfgw.Length; i++)
            {
                strFields[i] = classifyfgw[i].type;
            }
            return strFields;
        }

        public DataTable GetCityObjBPRecord(string begindate, string enddate, string c_csmc, string c_cslb, string pagenum, string pagesize)
        {
            dt = new DataTable();
            fileinfolist = new List<object>();
            //        CityObjService.CityObjServicePortTypeClient cityobjclient;
            //        cityobjclient = new CityObjService.CityObjServicePortTypeClient("CityObjServiceHttpSoap12Endpoint");
            if (pagenum == null || pagenum.Equals(""))
            {
                pagenum = "0";
            }
            if (pagesize == null || pagesize.Equals(""))
            {
                pagesize = "50";
            }
            jsonStr = getQueryCityObjs(begindate, enddate, c_csmc, c_cslb, pagenum, pagesize);
            CityObj cityObj = JSON.parse<CityObj>(jsonStr);
            foreach (CityObjInfo info in cityObj.city_objs)
            {
                fileinfolist.Add(info);
            }
            dt = Utilties.ConvertObjPro2DataTable(fileinfolist);
            dt.TableName = "CityObj";
            for (int i = 0; i < dt.Columns.Count; i++)
            {
                dt.Columns[i].ColumnName = CityObj.cityobjattributenames[i];
            }
            return dt;
        }

        public string[] GetCityObjName()
        {
            jsonStr = getCityCSMBMC();
            classify = JSON.parse<GetClassify>(jsonStr);
            classifyfgw = classify.types;
            strFields = new string[classifyfgw.Length];
            for (int i = 0; i < classifyfgw.Length; i++)
            {
                strFields[i] = classifyfgw[i].type;
            }
            return strFields;
        }

        public string[] GetCityObjType()
        {
            jsonStr = getCityTypes();
            classify = JSON.parse<GetClassify>(jsonStr);
            classifyfgw = classify.types;
            strFields = new string[classifyfgw.Length];
            for (int i = 0; i < classifyfgw.Length; i++)
            {
                strFields[i] = classifyfgw[i].type;
            }
            return strFields;
        }

        public System.Data.DataSet UpdateForJCGX(string startTime, string endTime, string satellite)
        {
            logStr = string.Format("dtStart='{0}' dtEnd='{1}' satellite='{2}'", startTime, endTime, satellite);
            InforLog<string>.inforLog.Info("文件:WS_QDB_Searcher_MySQL/App_Code/Service.cs 方法：UpdateForJCGX 输入:" + logStr);
           System.Data.DataSet ds = new System.Data.DataSet();
            DateTime dtStart = DateTime.Parse(startTime);
            DateTime dtEnd = DateTime.Parse(endTime);
            //string dbConn = mySQLOperator.EVDB.connString;
            //MySqlBaseUtilities evdb = new MySqlBaseUtilities(dbConn);
            _dbBaseUti = _dbOperator.GetSubDbUtilities(EnumDBType.EVDB);

            if (satellite == null || satellite == "")
            {
                string selectString = string.Format("select * from prod_gf1 where ImportTime between '{0}' and '{1}' and SatelliteID like'{2}'", dtStart, dtEnd, "GF%");
                ds = _dbBaseUti.GetDataSet(selectString);
            }
            else
            {
                string[] sSatellite = satellite.Split(',');
                if (sSatellite.Length == 1)
                {
                    string selectString = string.Format("select * from prod_gf1 where ImportTime between '{0}' and '{1}' and SatelliteID ='{2}'", dtStart, dtEnd, sSatellite[0]);
                    ds = _dbBaseUti.GetDataSet(selectString);
                }
                else if (sSatellite.Length == 2)
                {
                    string selectString = string.Format("select * from prod_gf1 where ImportTime between '{0}' and '{1}' and (SatelliteID ='{2}' or SatelliteID ='{3}')", dtStart, dtEnd, sSatellite[0], sSatellite[1]);
                    ds = _dbBaseUti.GetDataSet(selectString);
                }
                else
                {
                    string selectString = string.Format("select * from prod_gf1 where ImportTime between '{0}' and '{1}' and SatelliteID like'{2}'", dtStart, dtEnd, "GF%");
                    ds = _dbBaseUti.GetDataSet(selectString);
                }
            }

            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                InforLog<string>.inforLog.Info("文件:WS_QDB_Searcher_MySQL/App_Code/Service.cs 方法：UpdateForJCGX 输出:返回数据集不为空");
            }
            else
                InforLog<string>.inforLog.Info("文件:WS_QDB_Searcher_MySQL/App_Code/Service.cs 方法：UpdateForJCGX 输出:返回数据集为空");
            return ds;
        }

        #endregion
        #endregion

        #region "集成共享网站查询，模型算法查找"
        public String[] getAllMADBGroupNodeNames()
        {
            Dictionary<String, List<String>> BSDBNodeInfos = queryBSDBNodeInfos("MADB");
            Dictionary<String, String> metaDataMap = getBSDBMetaDataMap("MADB");
            List<string> list = new List<string>();
            foreach (String str in BSDBNodeInfos.Keys)
            {
                if (str != "MADB-2-1")
                {
                    String name = null;
                    metaDataMap.TryGetValue(str, out name);
                    list.Add(name);
                }
            }
            return list.ToArray();
        }

        public String[] getChildNamesByGroupNameForMADB(String groupName)
        {
            List<string> result = new List<string>();
            Dictionary<String, List<String>> BSDBNodeInfos = queryBSDBNodeInfos("MADB");
            Dictionary<String, String> metaDataMap = getBSDBMetaDataMap("MADB");
            List<String> list = new List<string>();
            String groupCode = getKeyByValue(metaDataMap, groupName);
            List<String> tempList = new List<string>();
            BSDBNodeInfos.TryGetValue(groupCode, out tempList);
            foreach (String str in tempList)
            {
                String strTemp = null;
                metaDataMap.TryGetValue(str, out strTemp);
                result.Add(strTemp);
            }
            return result.ToArray();
        }

        public System.Data.DataSet getMADBInfosByQueryName(String childNodeName, string startTime, string endTime, String keyWord, string fileName, Boolean searchType, int pageNum, int pageSize)
        {
            //if (mySQLOperator == null)
            //{
            //    mySQLOperator = new DBMySqlOperating();
            //}
            _dbBaseUti = _dbOperator.GetSubDbUtilities(EnumDBType.MADB);
            //MySqlBaseUti = mySQLOperator.MADB;
            string querySql = string.Format(" select group_code,data_code from metadatacatalognode where name ='{0}'", childNodeName);
           System.Data.DataSet ds = _dbBaseUti.GetDataSet(querySql);
            String groupCode = ds.Tables[0].Rows[0]["group_code"].ToString();
            String data_code = ds.Tables[0].Rows[0]["data_code"].ToString();

            string sql = string.Format("select table_name from tablecode where qrst_code ='{0}'", data_code);
            System.Data.DataSet dsTaleName = _dbBaseUti.GetDataSet(sql);
            String tableName = dsTaleName.Tables[0].Rows[0]["table_name"].ToString();
            string resultSql = null;
            string keywordSql = null;
            if (keyWord == null || keyWord.Equals(""))
            {
                keywordSql = null;
            }
            else
            {
                keywordSql = string.Format("and AlgorithmDesc like '%{0}%'", keyWord);
            }
            string fileNameSql = null;
            if (fileName == null || fileName.Equals(""))
            {
                fileNameSql = null;
            }
            else
            {
                fileNameSql = string.Format("and AlgorithmName = '{0}'", fileName);
            }
            resultSql = string.Format("select * from {0} {1} {2} limit {3},{4}", tableName, keywordSql, fileNameSql, (pageNum - 1) * pageSize, pageSize);
            System.Data.DataSet resultDs = _dbBaseUti.GetDataSet(resultSql);
            return resultDs;
        }

        #endregion


        #region
        private Dictionary<String, List<String>> queryBSDBNodeInfos(String dbName)
        {
            Dictionary<String, List<String>> result = new Dictionary<string, List<String>>();
            //if (mySQLOperator == null)
            //{
            //    mySQLOperator = new DBMySqlOperating();
            //}
            if (dbName.Equals("BSDB"))
            {
                //MySqlBaseUti = mySQLOperator.BSDB;
                _dbBaseUti = _dbOperator.GetSubDbUtilities(EnumDBType.BSDB);
            }
            else if (dbName.Equals("MADB"))
            {
                //MySqlBaseUti = mySQLOperator.MADB;
                _dbBaseUti = _dbOperator.GetSubDbUtilities(EnumDBType.MADB);
            }

            string querySql = string.Format(" select group_code,child_code from metadatacatalognode_r");
           System.Data.DataSet ds = _dbBaseUti.GetDataSet(querySql);
            foreach (DataRow mDr in ds.Tables[0].Rows)
            {
                String groupCode = mDr["group_code"].ToString();
                String childCode = mDr["child_code"].ToString();
                List<String> temp = new List<string>();
                if (result.TryGetValue(groupCode, out temp))
                {
                    result.Remove(groupCode);
                    temp.Add(childCode);
                    result.Add(groupCode, temp);
                }
                else
                {
                    temp = new List<string>();
                    temp.Add(childCode);
                    result.Add(groupCode, temp);
                }
            }
            return result;
        }

        private Dictionary<String, String> getBSDBMetaDataMap(String dbName)
        {
            Dictionary<String, String> result = new Dictionary<string, string>();
            if (dbName.Equals("BSDB"))
            {
                //MySqlBaseUti = mySQLOperator.BSDB;
                _dbBaseUti = _dbOperator.GetSubDbUtilities(EnumDBType.BSDB);
            }
            else if (dbName.Equals("MADB"))
            {
                //MySqlBaseUti = mySQLOperator.MADB;
                _dbBaseUti = _dbOperator.GetSubDbUtilities(EnumDBType.MADB);
            }
            string querySql = string.Format("select name,group_code from metadatacatalognode");
           System.Data.DataSet ds = _dbBaseUti.GetDataSet(querySql);
            foreach (DataRow mDr in ds.Tables[0].Rows)
            {
                String name = mDr["name"].ToString();
                String group_code = mDr["group_code"].ToString();
                result.Add(group_code, name);
            }
            return result;
        }

        public String getKeyByValue(Dictionary<String, String> map, String value)
        {
            String key = null;
            foreach (String str in map.Keys)
            {
                String name = null;
                map.TryGetValue(str, out name);
                if (name.Equals(value))
                {
                    key = str;
                    break;
                }
            }
            return key;
        }

        #endregion

        #region "集成共享网站查询，基础空间数据查找"
        public String[] getAllBSDBGroupNodeNames()
        {
            Dictionary<String, List<String>> BSDBNodeInfos = queryBSDBNodeInfos("BSDB");
            Dictionary<String, String> metaDataMap = getBSDBMetaDataMap("BSDB");
            List<string> list = new List<string>();
            foreach (String str in BSDBNodeInfos.Keys)
            {
                if (str != "BSDB-2-1")
                {
                    String name = null;
                    metaDataMap.TryGetValue(str, out name);
                    list.Add(name);
                }
            }
            return list.ToArray();
        }

        public String[] getChildNamesByGroupNameForBSDB(String groupName)
        {
            List<string> result = new List<string>();
            Dictionary<String, List<String>> BSDBNodeInfos = queryBSDBNodeInfos("BSDB");
            Dictionary<String, String> metaDataMap = getBSDBMetaDataMap("BSDB");
            List<String> list = new List<string>();
            String groupCode = getKeyByValue(metaDataMap, groupName);
            List<String> tempList = new List<string>();
            BSDBNodeInfos.TryGetValue(groupCode, out tempList);
            foreach (String str in tempList)
            {
                String strTemp = null;
                metaDataMap.TryGetValue(str, out strTemp);
                result.Add(strTemp);
            }
            return result.ToArray();
        }

        public System.Data.DataSet searchBSBDInfosByQueryName(String keyWord, double maxLon, double minLon, double maxLat, double minLat, String startTime, String endTime, int pageNum, int pageSize)
        {
            //if (mySQLOperator == null)
            //{
            //    mySQLOperator = new DBMySqlOperating();
            //}
            _dbBaseUti = _dbOperator.GetSubDbUtilities(EnumDBType.BSDB);
            //MySqlBaseUti = mySQLOperator.BSDB;

            String tableName = "prods_vector";
            string resultSql = null;
            string locationSql = string.Format("EXTENTUP <={0} and EXTENTDOWN>={1} and EXTENTLEFT >={2} and EXTENTRIGHT<={3}", maxLat, minLat, minLon, maxLon);
            string dateTimeSql = string.Format("and PRODUCEDATE>='{0}' and PRODUCEDATE<='{1}'", startTime, endTime);
            string keywordSql = null;
            if (keyWord == null || keyWord.Equals(""))
            {
                keywordSql = null;
            }
            else
            {
                keywordSql = string.Format("and PRODUCTNAME like '%{0}%'", keyWord);
            }
            resultSql = string.Format("select * from {0} where  {1} {2} {3} limit {4},{5}", tableName, locationSql, dateTimeSql, keywordSql, (pageNum - 1) * pageSize, pageSize);
            System.Data.DataSet resultDs = _dbBaseUti.GetDataSet(resultSql);
            return resultDs;
        }

        public System.Data.DataSet getBSBDInfosByQueryName(String childNodeName, double maxLon, double minLon, double maxLat, double minLat, String startTime, String endTime, String keyWord, Boolean searchType, int pageNum, int pageSize)
        {
            //if (mySQLOperator == null)
            //{
            //    mySQLOperator = new DBMySqlOperating();
            //}
            _dbBaseUti = _dbOperator.GetSubDbUtilities(EnumDBType.BSDB);
            string querySql = string.Format(" select group_code,data_code from metadatacatalognode where name ='{0}'", childNodeName);
           System.Data.DataSet ds = _dbBaseUti.GetDataSet(querySql);
            String groupCode = ds.Tables[0].Rows[0]["group_code"].ToString();
            String data_code = ds.Tables[0].Rows[0]["data_code"].ToString();

            string sql = string.Format("select table_name from tablecode where qrst_code ='{0}'", data_code);
            System.Data.DataSet dsTaleName = _dbBaseUti.GetDataSet(sql);
            String tableName = dsTaleName.Tables[0].Rows[0]["table_name"].ToString();
            string resultSql = null;
            string locationSql = string.Format("and EXTENTUP <={0} and EXTENTDOWN>={1} and EXTENTLEFT >={2} and EXTENTRIGHT<={3}", maxLat, minLat, minLon, maxLon);
            string dateTimeSql = string.Format("and PRODUCEDATE>={0} and PRODUCEDATE<={1}", startTime, endTime);
            string keywordSql = null;
            if (keyWord == null || keyWord.Equals(""))
            {
                keywordSql = null;
            }
            else
            {
                keywordSql = string.Format("and PRODUCTNAME like '%{0}%'", keyWord);
            }
            resultSql = string.Format("select * from {0} where groupcode='{1}' {2} {3} {4} limit {5},{6}", tableName, groupCode, locationSql, dateTimeSql, keywordSql, (pageNum - 1) * pageSize, pageSize);
            System.Data.DataSet resultDs = _dbBaseUti.GetDataSet(resultSql);
            return resultDs;
        }

        public String getKeyByValueForList(Dictionary<String, List<String>> map, String value)
        {
            String key = null;
            foreach (String str in map.Keys)
            {
                List<String> list = null;
                map.TryGetValue(str, out list);
                foreach (String strV in list)
                {
                    if (strV.Equals(value))
                    {
                        key = str;
                        break;
                    }
                }
            }
            return key;
        }

        public System.Data.DataSet Info973(string dataName, string searchFileName)
        {
            logStr = string.Format("dataName='{0}' searchFileName='{1}'", dataName, searchFileName);
            InforLog<string>.inforLog.Info("文件:WS_QDB_Searcher_MySQL/App_Code/Service.cs 方法：Info973 输入:" + logStr);
           System.Data.DataSet ds = new System.Data.DataSet();
            string tableName = "";

            if (dataName.ToLower().Contains("prod") && dataName.ToLower().Contains("omi"))
                tableName = "prod_aerosol_omi";

            else if (dataName.ToLower().Contains("prod") && dataName.ToLower().Contains("avhrr"))
                tableName = "prod_aerosol_avhrr";

            else if (dataName.ToLower().Contains("prod") && dataName.ToLower().Contains("modis"))
                tableName = "prod_aerosol_modis";

            else if (dataName.ToLower().Contains("prod") && dataName.ToLower().Contains("misr"))
                tableName = "prod_aerosol_misr";

            else if (dataName.ToLower().Contains("prod") && dataName.ToLower().Contains("toms"))
                tableName = "prod_aerosol_toms";

            else if (dataName.ToLower().Contains("prod") && dataName.ToLower().Contains("parasol"))
                tableName = "prod_aerosol_parasol";

            else if (dataName.ToLower().Contains("qrst") && dataName.ToLower().Contains("omi"))
                tableName = "qrst_standard_omi";

            else if (dataName.ToLower().Contains("qrst") && dataName.ToLower().Contains("modis"))
                tableName = "qrst_standard_modis";

            else if (dataName.ToLower().Contains("qrst") && dataName.ToLower().Contains("toms"))
                tableName = "qrst_standard_toms";

            else if (dataName.ToLower().Contains("qrst") && dataName.ToLower().Contains("misr"))
                tableName = "qrst_standard_misr";

            //string strCon = "Database='evdb';Data Source='192.168.10.104';User Id='HJDATABASE_ADMIN';Password='dbadmin_2011';charset='utf8'";
            //using (MySqlConnection mysqlConn = new MySqlConnection(strCon))
            //{
            //    mysqlConn.Open();
            //    using (MySqlCommand cmd = mysqlConn.CreateCommand())
            //    {
            _dbBaseUti = _dbOperator.GetSubDbUtilities(EnumDBType.EVDB);
            string sql = string.Format("select * from {0} where   (FileName='{1}')", tableName, searchFileName);
            //MySqlDataAdapter daTotalCount = new MySqlDataAdapter(cmd.CommandText, mysqlConn);
            //daTotalCount.SelectCommand = cmd;
            //daTotalCount.Fill(ds);
            ds = _dbBaseUti.GetDataSet(sql);
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                InforLog<string>.inforLog.Info("文件:WS_QDB_Searcher_MySQL/App_Code/Service.cs 方法：Info973 输出:返回数据集不为空");
            }
            else
                InforLog<string>.inforLog.Info("文件:WS_QDB_Searcher_MySQL/App_Code/Service.cs 方法：Info973 输出:返回数据集为空");
            return ds;
        }

        public string Download973Data(List<string> listPath, List<string> listFilename, string targetFolder)
        {
            logStr = string.Format("listPath='{0}' listFilename='{1}' targetFolder='{2}'", InforLog<string>.returnListStrElem(listPath), InforLog<string>.returnListStrElem(listFilename), targetFolder);
            InforLog<string>.inforLog.Info("文件:WS_QDB_Searcher_MySQL/App_Code/Service.cs 方法：Download973Data 输入:" + logStr);
            string result = "";
            int iresult = 0;
            for (int i = 0; i < listFilename.Count; i++)
            {
                File.Copy(listPath[i], targetFolder + listFilename[i], true);
                iresult++;
            }

            if (iresult == listFilename.Count)
                result = "Success";
            else result = "Failed";
            InforLog<string>.inforLog.Info("文件:WS_QDB_Searcher_MySQL/App_Code/Service.cs 方法：Download973Data 输出:" + result);
            return result;
        }

        public System.Data.DataSet Search973Data(string dataName, string prodType, string startTime, string endTime, double dStartLong, double dEndLong, double dStartLat, double dEndLat, int onPage, int countOfPerPage, out int totalCount)
        {
            logStr = string.Format("dataName='{0}' prodType='{1}' startTime='{2}' endTime='{3}' dStartLong='{4}' dEndLong='{5}' dStartLat='{6}' dEndLat='{7}' onPage='{8}' countOfPerPage='{9}' ", dataName, prodType, startTime, endTime, dStartLong, dEndLong, dStartLat, dEndLat, onPage, countOfPerPage);
            InforLog<string>.inforLog.Info("文件:WS_QDB_Searcher_MySQL/App_Code/Service.cs 方法：Search973Data 输入:" + logStr);
           System.Data.DataSet ds = new System.Data.DataSet();
            string tableName = "";
            string sql = "";
            if (dataName.ToLower().Contains("prod") && dataName.ToLower().Contains("omi"))
                tableName = "prod_aerosol_omi";

            else if (dataName.ToLower().Contains("prod") && dataName.ToLower().Contains("avhrr"))
                tableName = "prod_aerosol_avhrr";

            else if (dataName.ToLower().Contains("prod") && dataName.ToLower().Contains("modis"))
                tableName = "prod_aerosol_modis";

            else if (dataName.ToLower().Contains("prod") && dataName.ToLower().Contains("misr"))
                tableName = "prod_aerosol_misr";

            else if (dataName.ToLower().Contains("prod") && dataName.ToLower().Contains("toms"))
                tableName = "prod_aerosol_toms";

            else if (dataName.ToLower().Contains("prod") && dataName.ToLower().Contains("parasol"))
                tableName = "prod_aerosol_parasol";

            else if (dataName.ToLower().Contains("qrst") && dataName.ToLower().Contains("omi"))
                tableName = "qrst_standard_omi";

            else if (dataName.ToLower().Contains("qrst") && dataName.ToLower().Contains("modis"))
                tableName = "qrst_standard_modis";

            else if (dataName.ToLower().Contains("qrst") && dataName.ToLower().Contains("toms"))
                tableName = "qrst_standard_toms";

            else if (dataName.ToLower().Contains("qrst") && dataName.ToLower().Contains("misr"))
                tableName = "qrst_standard_misr";


            DateTime? dtStart;
            DateTime? dtEnd;
            if ((startTime == string.Empty) && (endTime == string.Empty))
            {
                dtStart = null;
                dtEnd = null;
            }
            else
            {
                dtStart = DateTime.Parse(startTime);
                dtEnd = DateTime.Parse(endTime);
            }
            _dbBaseUti = _dbOperator.GetSubDbUtilities(EnumDBType.EVDB);
            //string strCon = "Database='evdb';Data Source='192.168.10.104';User Id='HJDATABASE_ADMIN';Password='dbadmin_2011';charset='utf8'";
            //using (MySqlConnection mysqlConn = new MySqlConnection(strCon))
            //{
            //    mysqlConn.Open();
            //    using (MySqlCommand cmd = mysqlConn.CreateCommand())
            //    {

            int startRecord = onPage;//从第几条开始
            int RecordNum = countOfPerPage;//多少条

            if (dtStart == null && dtEnd == null)
            {
                sql = string.Format("select * from {0} where  ('{1}' >=DATAUPPERLEFTLONG)  and  '{2}'<=DATAUPPERRIGHTLONG and '{3}'>=DATALOWERRIGHTLAT  and  '{4}'<=DATAUPPERRIGHTLAT and ('{5}'=Type)", tableName, dStartLong, dEndLong, dStartLat, dEndLat, prodType);
                //cmd.CommandText = string.Format("select * from {0} where  ('{1}' >=DATAUPPERLEFTLONG)  and  '{2}'<=DATAUPPERRIGHTLONG and '{3}'>=DATALOWERRIGHTLAT  and  '{4}'<=DATAUPPERRIGHTLAT and ('{5}'=Type)", tableName, dStartLong, dEndLong, dStartLat, dEndLat, prodType);
                //MySqlDataAdapter daTotalCount = new MySqlDataAdapter(cmd.CommandText, mysqlConn);
                //daTotalCount.SelectCommand = cmd;
                //daTotalCount.Fill(ds);
                ds = _dbBaseUti.GetDataSet(sql);
                totalCount = ds.Tables[0].Rows.Count;//out 参数：获取总条数
                ds.Clear();

                sql = string.Format("select * from {0} where '{1}' >=DATAUPPERLEFTLONG  and  '{2}'<=DATAUPPERRIGHTLONG and '{3}'>=DATALOWERRIGHTLAT  and  '{4}'<=DATAUPPERRIGHTLAT and ('{5}'=Type) limit {6},{7}", tableName, dStartLong, dEndLong, dStartLat, dEndLat, prodType, startRecord, RecordNum);
                //MySqlDataAdapter da = new MySqlDataAdapter(cmd.CommandText, mysqlConn);
                //da.SelectCommand = cmd;
                //da.Fill(ds);
                ds = _dbBaseUti.GetDataSet(sql);

                //return ds;

            }

            else
            {

                sql = string.Format("select * from {0} where (SCENEDATE between '{1}' and '{2}')  and  '{3}' >=DATAUPPERLEFTLONG  and  '{4}'<=DATAUPPERRIGHTLONG and '{5}'>=DATALOWERRIGHTLAT  and  '{6}'<=DATAUPPERRIGHTLAT and ('{7}'=Type)", tableName, dtStart, dtEnd, dStartLong, dEndLong, dStartLat, dEndLat, prodType);

                //MySqlDataAdapter daTotalCount = new MySqlDataAdapter(cmd.CommandText, mysqlConn);
                //daTotalCount.SelectCommand = cmd;
                //daTotalCount.Fill(ds);
                ds = _dbBaseUti.GetDataSet(sql);
                totalCount = ds.Tables[0].Rows.Count;//out 参数：获取总条数
                ds.Clear();

                sql = string.Format("select * from {0} where (SCENEDATE between '{1}' and '{2}')  and  '{3}' >=DATAUPPERLEFTLONG  and  '{4}'<=DATAUPPERRIGHTLONG and '{5}'>=DATALOWERRIGHTLAT  and  '{6}'<=DATAUPPERRIGHTLAT and ('{7}'=Type) limit {8},{9}", tableName, dtStart, dtEnd, dStartLong, dEndLong, dStartLat, dEndLat, prodType, startRecord, RecordNum);
                //MySqlDataAdapter da = new MySqlDataAdapter(cmd.CommandText, mysqlConn);
                //da.SelectCommand = cmd;
                //da.Fill(ds);
                ds = _dbBaseUti.GetDataSet(sql);
                //return ds;
            }
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                InforLog<string>.inforLog.Info("文件:WS_QDB_Searcher_MySQL/App_Code/Service.cs 方法：Search973Data 输出:返回数据集不为空");
            }
            else
                InforLog<string>.inforLog.Info("文件:WS_QDB_Searcher_MySQL/App_Code/Service.cs 方法：Search973Data 输出:返回数据集为空");
            return ds;

        }

        public System.Data.DataSet Search973StdData(string stdName, string prodType, string startTime, string endTime, double dStartLong, double dEndLong, double dStartLat, double dEndLat, string filenameFilter)
        {
            logStr = string.Format("stdName='{0}' prodType='{1}' startTime='{2}' endTime='{3}' dStartLong='{4}' dEndLong='{5}' dStartLat='{6}' dEndLat='{7}' filenameFilter='{8}'", stdName, prodType, startTime, endTime, dStartLong, dEndLong, dStartLat, dEndLat, filenameFilter);
            InforLog<string>.inforLog.Info("文件:WS_QDB_Searcher_MySQL/App_Code/Service.cs 方法：Search973StdData 输入:" + logStr);
           System.Data.DataSet ds = new System.Data.DataSet();
            string tableName = "";


            if (stdName.ToLower().Contains("omi"))
                tableName = "qrst_standard_omi";
            else if (stdName.ToLower().Contains("modis"))
                tableName = "qrst_standard_modis";
            else if (stdName.ToLower().Contains("toms"))
                tableName = "qrst_standard_toms";
            else if (stdName.ToLower().Contains("misr"))
                tableName = "qrst_standard_misr";

            //DateTime myDate = DateTime.ParseExact(strDateYMD, "yyyyMMdd", System.Globalization.CultureInfo.InvariantCulture);
            // DateTime dtStart = DateTime.ParseExact(startTime, "yyyyMMdd", System.Globalization.CultureInfo.InvariantCulture);
            // DateTime dtEnd = DateTime.ParseExact(endTime, "yyyyMMdd", System.Globalization.CultureInfo.InvariantCulture);
            DateTime dtStart = DateTime.Parse(startTime);
            DateTime dtEnd = DateTime.Parse(endTime);

            _dbBaseUti = _dbOperator.GetSubDbUtilities(EnumDBType.EVDB);

            //string strCon = "Database='evdb';Data Source='192.168.10.104';User Id='HJDATABASE_ADMIN';Password='dbadmin_2011';charset='utf8'";
            //using (MySqlConnection mysqlConn = new MySqlConnection(strCon))
            //{
            //    mysqlConn.Open();
            //    using (MySqlCommand cmd = mysqlConn.CreateCommand())
            //    {

            //        //cmd.CommandText = "select * from '" + tableName + "' where (SCENEDATE between '" + dtStart + "' and '" + dtEnd + "') and (('" + dStartLong + "'>=DATAUPPERLEFTLONG  and  '" + dEndLong + "'<=DATAUPPERRIGHTLONG) and ('" + dStartLat + "'>=DATALOWERRIGHTLAT  and  '" + dEndLat + "'<=DATAUPPERRIGHTLAT)) and ('" + prodType + "'=Type)and (FileName like '%filenameFilter%')";

            //        //cmd.CommandText = "select * from '" + tableName + "' where  ('" + dStartLat + "'>=DATALOWERRIGHTLAT  and  '" + dEndLat + "'<=DATAUPPERRIGHTLAT)) and ('" + prodType + "'=Type)and (FileName like '%filenameFilter%')";

            //        cmd.CommandText = string.Format("select * from {0} where (SCENEDATE between '{1}' and '{2}')  and  '{3}' >=DATAUPPERLEFTLONG  and  '{4}'<=DATAUPPERRIGHTLONG and '{5}'>=DATALOWERRIGHTLAT  and  '{6}'<=DATAUPPERRIGHTLAT and ('{7}'=Type) and (FileName like '%{8}%')", tableName, dtStart, dtEnd, dStartLong, dEndLong, dStartLat, dEndLat, prodType, filenameFilter);

            //        //  cmd.CommandText = string.Format("select * from {0} where (Type='{1}') and (SCENEDATE between {2} and {3})", tableName, prodType,dtStart,dtEnd);

            //        MySqlDataAdapter da = new MySqlDataAdapter(cmd.CommandText, mysqlConn);
            //        da.SelectCommand = cmd;
            //        da.Fill(ds);
            string sql = string.Format("select * from {0} where (SCENEDATE between '{1}' and '{2}')  and  '{3}' >=DATAUPPERLEFTLONG  and  '{4}'<=DATAUPPERRIGHTLONG and '{5}'>=DATALOWERRIGHTLAT  and  '{6}'<=DATAUPPERRIGHTLAT and ('{7}'=Type) and (FileName like '%{8}%')", tableName, dtStart, dtEnd, dStartLong, dEndLong, dStartLat, dEndLat, prodType, filenameFilter);
            ds = _dbBaseUti.GetDataSet(sql);
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                InforLog<string>.inforLog.Info("文件:WS_QDB_Searcher_MySQL/App_Code/Service.cs 方法：Search973StdData 输出:返回数据集不为空");
            }
            else
                InforLog<string>.inforLog.Info("文件:WS_QDB_Searcher_MySQL/App_Code/Service.cs 方法：Search973StdData 输出:返回数据集为空");
            return ds;
            //    }
            //}
        }

        #region 仿真组
        public string GetBandLstBySensorID(string SensorID)
        {
            InforLog<string>.inforLog.Info("文件:WS_QDB_Searcher_MySQL/App_Code/Service.cs 方法：GetBandLstBySensorID 输入:SensorID=" + SensorID);
            List<string> sensorLst = new List<string>();
            //if (mySQLOperator == null)
            //{
            //    mySQLOperator = new DBMySqlOperating();
            //}
            _dbBaseUti = _dbOperator.GetSubDbUtilities(EnumDBType.EVDB);

            string sqlstring = string.Format("select * from evdb_bands where sensorID = '{0}'", SensorID);

           System.Data.DataSet ds = _dbBaseUti.GetDataSet(sqlstring);
            //using (DataSet ds = MySqlBaseUti.GetDataSet(sqlstring))
            //{
            //    if (ds != null && ds.Tables.Count > 0)
            //    {
            //        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            //        {
            //            sensorLst.Add(ds.Tables[0].Rows[i][0].ToString());
            //        }
            //    }
            //}

            InforLog<string>.inforLog.Info("文件:WS_QDB_Searcher_MySQL/App_Code/Service.cs 方法：GetBandLstBySensorID 输出:ds.GetXml()=" + ds.GetXml());
            return ds.GetXml();
        }

        public string GetSensorLstBySateID(string SateID)
        {
            InforLog<string>.inforLog.Info("文件:WS_QDB_Searcher_MySQL/App_Code/Service.cs 方法：GetSensorLstBySateID 输入:SateID=" + SateID);
            List<string> sensorLst = new List<string>();
            //if (mySQLOperator == null)
            //{
            //    mySQLOperator = new DBMySqlOperating();
            //}
            _dbBaseUti = _dbOperator.GetSubDbUtilities(EnumDBType.EVDB);

            string sqlstring = string.Format("select * from evdb_sensors where satelliteID = '{0}'", SateID);

           System.Data.DataSet ds = _dbBaseUti.GetDataSet(sqlstring);
            //using (DataSet ds = MySqlBaseUti.GetDataSet(sqlstring))
            //{
            //    if (ds != null && ds.Tables.Count > 0)
            //    {
            //        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            //        {
            //            sensorLst.Add(ds.Tables[0].Rows[i][0].ToString());
            //        }
            //    }
            //}
            InforLog<string>.inforLog.Info("文件:WS_QDB_Searcher_MySQL/App_Code/Service.cs 方法：GetSensorLstBySateID 输出:ds.GetXml()=" + ds.GetXml());
            return ds.GetXml();
        }

        public string GetSateInfo()
        {
            //List<string> sateLst = new List<string>();
            //if (mySQLOperator == null)
            //{
            //    mySQLOperator = new DBMySqlOperating();
            //}
            _dbBaseUti = _dbOperator.GetSubDbUtilities(EnumDBType.EVDB);

           System.Data.DataSet ds = _dbBaseUti.GetDataSet("select * from evdb_satellites order by ID");
            //using ()
            //{
            //    if (ds != null && ds.Tables.Count > 0)
            //    {
            //        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            //        {
            //            sateLst.Add(ds.Tables[0].Rows[i][0].ToString());
            //        }
            //    }
            //}
            InforLog<string>.inforLog.Info("文件:WS_QDB_Searcher_MySQL/App_Code/Service.cs 方法：GetSateInfo 输出:ds.GetXml()=" + ds.GetXml());
            return ds.GetXml();
        }

        public string GetSateAndSensorAndBandInfo()
        {
            //if (mySQLOperator == null)
            //{
            //    mySQLOperator = new DBMySqlOperating();
            //}
            _dbBaseUti = _dbOperator.GetSubDbUtilities(EnumDBType.EVDB);
            //MySqlBaseUti = mySQLOperator.EVDB;

            string sqlString = string.Format("select * from satellite_sensor_band_view order by sateID,sensorID,bandID");

           System.Data.DataSet ds = _dbBaseUti.GetDataSet(sqlString);
            InforLog<string>.inforLog.Info("文件:WS_QDB_Searcher_MySQL/App_Code/Service.cs 方法：GetSateAndSensorAndBandInfo 输出:ds.GetXml()=" + ds.GetXml());
            return ds.GetXml();
        }

        #endregion
        #endregion

        #region 查询、添加、修改用户评价、评分等公共信息
        public System.Data.DataSet SearchUserEvaluationInfoJCGX(string DataId, string UserId, out int AllRecordsCount, int StartIndex, int ResultCount)
        {
            logStr = string.Format("DataId='{0}' UserId='{1}' StartIndex='{2}' ResultCount='{3}'", DataId, UserId, StartIndex, ResultCount);
            InforLog<string>.inforLog.Info("文件:WS_QDB_Searcher_MySQL/App_Code/Service.cs 方法：SearchUserEvaluationInfoJCGX 输入:" + logStr);
            //if (mySQLOperator == null)
            //{
            //    mySQLOperator = new DBMySqlOperating();
            //}
            AllRecordsCount = 0;
            _dbBaseUti = _dbOperator.GetSubDbUtilities(EnumDBType.ISDB);
            string userTableName = "isdb_userevaluation";
            string UserTableReturnFields = "*";
            string strQueryCondition = sqlBaseTool.GetQueryCondition_UserEvaluation(DataId, UserId);
           System.Data.DataSet ds = _dbBaseUti.GetList(userTableName, UserTableReturnFields, strQueryCondition, out AllRecordsCount, StartIndex, ResultCount);
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                InforLog<string>.inforLog.Info("文件:WS_QDB_Searcher_MySQL/App_Code/Service.cs 方法：SearchUserEvaluationInfoJCGX 输出:returnDS不为空");
            }
            else
                InforLog<string>.inforLog.Info("文件:WS_QDB_Searcher_MySQL/App_Code/Service.cs 方法：SearchUserEvaluationInfoJCGX 输出:returnDS为空");
            return _dbBaseUti.GetList(userTableName, UserTableReturnFields, strQueryCondition, out AllRecordsCount, StartIndex, ResultCount);
        }

        public string AddUserEvaluationInfoJCGX(string dataID, string UserID, string Description, int Score, string BuyId)
        {
            logStr = string.Format("dataID='{0}' UserID='{1}' Description='{2}' Score='{3}' BuyId='{4}'", dataID, UserID, Description, Score, BuyId);
            InforLog<string>.inforLog.Info("文件:WS_QDB_Searcher_MySQL/App_Code/Service.cs 方法：AddUserEvaluationInfoJCGX 输入:" + logStr);
            try
            {
                //if (mySQLOperator == null)
                //{
                //    mySQLOperator = new DBMySqlOperating();
                //}
                _dbBaseUti = _dbOperator.GetSubDbUtilities(EnumDBType.ISDB);
                if (sqlBaseTool.AddUserEvaluation(dataID, UserID, Description, Score, BuyId, _dbBaseUti))
                {
                    logStr = "1";
                }
                else
                {
                    logStr = "0";
                }
            }
            catch (Exception ex)
            {
                logStr = "数据添加失败！" + ex.ToString();
            }
            InforLog<string>.inforLog.Info("文件:WS_QDB_Searcher_MySQL/App_Code/Service.cs 方法：AddUserEvaluationInfoJCGX 输出:" + logStr);
            return logStr;
        }

        public string DeleteUserEvaluationInfoJCGX(int id)
        {
            InforLog<string>.inforLog.Info("文件:WS_QDB_Searcher_MySQL/App_Code/Service.cs 方法：DeleteUserEvaluationInfoJCGX 输入:id=" + id);
            try
            {
                //if (mySQLOperator == null)
                //{
                //    mySQLOperator = new DBMySqlOperating();
                //}
                _dbBaseUti = _dbOperator.GetSubDbUtilities(EnumDBType.ISDB);
                if (sqlBaseTool.DeleteUserEvaluation(id, _dbBaseUti))
                {
                    logStr = "1";
                }
                else
                {
                    logStr = "0";
                }
            }
            catch (Exception ex)
            {
                logStr = "数据添加失败！" + ex.ToString();
            }
            InforLog<string>.inforLog.Info("文件:WS_QDB_Searcher_MySQL/App_Code/Service.cs 方法：DeleteUserEvaluationInfoJCGX 输出:" + logStr);
            return logStr;
        }

        public string UpDatePublicInfoJCGX(string DataID, string Price, int DownloadCount, double AverageScore)
        {
            logStr = string.Format("DataID='{0}' Price='{1}' DownloadCount='{2}' AverageScore='{3}'", DataID, Price, DownloadCount, AverageScore);
            InforLog<string>.inforLog.Info("文件:WS_QDB_Searcher_MySQL/App_Code/Service.cs 方法：UpDatePublicInfoJCGX 输入:" + logStr);
            try
            {
                //if (mySQLOperator == null)
                //{
                //    mySQLOperator = new DBMySqlOperating();
                //}
                _dbBaseUti = _dbOperator.GetSubDbUtilities(EnumDBType.ISDB);
                if (sqlBaseTool.UpdatePublicInfo(DataID, Price, DownloadCount, AverageScore, _dbBaseUti))
                {
                    logStr = "1";
                }
                else
                {
                    logStr = "0";
                }
            }
            catch (Exception ex)
            {
                logStr = "数据更新失败:" + ex.ToString();
            }
            InforLog<string>.inforLog.Info("文件:WS_QDB_Searcher_MySQL/App_Code/Service.cs 方法：UpDatePublicInfoJCGX 输出:" + logStr);
            return logStr;
        }

        public int GetEvaluationCount(string dataID)
        {
            InforLog<string>.inforLog.Info("文件:WS_QDB_Searcher_MySQL/App_Code/Service.cs 方法：GetEvaluationCount 输入:dataID=" + dataID);
            int rn = 0;
            try
            {
                //{
                //    if (mySQLOperator == null)
                //    {
                //        mySQLOperator = new DBMySqlOperating();
                //    }
                _dbBaseUti = _dbOperator.GetSubDbUtilities(EnumDBType.ISDB);
                rn = sqlBaseTool.GetEvaluationCount(dataID, _dbBaseUti);
            }
            catch (Exception)
            {
                rn = -1;
            }
            InforLog<string>.inforLog.Info("文件:WS_QDB_Searcher_MySQL/App_Code/Service.cs 方法：GetEvaluationCount 输出:" + rn);
            return rn;
        }







        #endregion

        #region 查询表记录

        public System.Data.DataSet SearchAlgorithmJCGX(List<string> UploadDate, List<string> ArtificialType, List<string> AlgEnName, List<string> DllName, List<string> AlgCnName, List<string> SuitableSatellites, List<string> SuitableSensors, string IsOnCloud, out int AllRecordsCount, int StartIndex, int ResultCount, int QueryRange, List<string> strOrderBy, List<int> OrderByType)
        {
            logStr = string.Format("UploadDate='{0}' ArtificialType='{1}' AlgEnName='{2}' DllName='{3}' AlgCnName='{4}' SuitableSatellites='{5}' SuitableSensors='{6}' IsOnCloud='{7}' StartIndex='{8}' ResultCount='{9}' QueryRange='{10}' strOrderBy='{11}' OrderByType='{12}'", InforLog<string>.returnListStrElem(UploadDate), InforLog<string>.returnListStrElem(ArtificialType), InforLog<string>.returnListStrElem(AlgEnName), InforLog<string>.returnListStrElem(DllName), InforLog<string>.returnListStrElem(AlgCnName), InforLog<string>.returnListStrElem(SuitableSatellites), InforLog<string>.returnListStrElem(SuitableSensors), IsOnCloud, StartIndex, ResultCount, QueryRange, InforLog<string>.returnListStrElem(strOrderBy), InforLog<int>.returnListStrElem(OrderByType));
            InforLog<string>.inforLog.Info("文件:WS_QDB_Searcher_MySQL/App_Code/Service.cs 方法：SearchAlgorithmJCGX 输入:" + logStr);
            AllRecordsCount = 0;

            #region 组织排序参数及 排序类型（升序，降序）
            //string sqlOrderBy = strOrderBy;
            //if (sqlOrderBy != "")
            //{
            //    switch (OrderByType)
            //    {
            //        case 0:
            //            sqlOrderBy += " ASC";
            //            break;
            //        case 1:
            //            sqlOrderBy += " DESC";
            //            break;
            //        default:
            //            break;
            //    }
            //}
            string sqlOrderBy = "";
            if (string.Join(" ", strOrderBy).Trim() != "")
            {
                if (strOrderBy.Count == OrderByType.Count)
                {
                    for (int i = 0; i < OrderByType.Count; i++)
                    {
                        switch (OrderByType[i])
                        {
                            case 0:
                                strOrderBy[i] += " ASC";
                                break;
                            case 1:
                                strOrderBy[i] += " DESC";
                                break;
                            default:
                                break;
                        }
                        sqlOrderBy = string.Join(",", strOrderBy);
                    }
                }
                else
                {
                    throw new Exception("输入的排序字段数与输入的排列顺序个数不一致！");

                }
            }
            #endregion

            #region 组织DateTime参数，从字符串类型转化为DateTime类型
            List<DateTime> listUploadDate = new List<DateTime>();
            try
            {
                foreach (string str in UploadDate)
                {
                    listUploadDate.Add(Convert.ToDateTime(str));
                }
            }
            catch
            {
                throw new Exception("输入的时间参数的格式不正确！");
            }
            #endregion

            //if (mySQLOperator == null)
            //{
            //    mySQLOperator = new DBMySqlOperating();
            //}
            string userTableName = "";
            string TableName = "";
            string UserTableReturnFields = "";
            string TableReturnFields = "";
            string strQueryCondition = "";
            string AddQueryTable_SuitSateSensor = string.Empty;

            switch (QueryRange)
            {
                case 0:
                    //查询用户上传算法组件
                    _dbBaseUti = _dbOperator.GetSubDbUtilities(EnumDBType.ISDB);
                    userTableName = "isdb_useralgorithm_isdb_publicinfo_view";
                    UserTableReturnFields = "*";
                    AddQueryTable_SuitSateSensor = "isdb_parautility_view";

                    strQueryCondition = sqlBaseTool.GetQueryCondition_AlgorithmJCGX(AddQueryTable_SuitSateSensor, listUploadDate, ArtificialType, AlgEnName, DllName, AlgCnName, SuitableSatellites, SuitableSensors, IsOnCloud);
                    if (string.IsNullOrEmpty(sqlOrderBy.Trim()))
                    {
                        returnDS = _dbBaseUti.GetList(userTableName, UserTableReturnFields, strQueryCondition, out AllRecordsCount, StartIndex, ResultCount);
                    }
                    else
                    {
                        returnDS = _dbBaseUti.GetList(userTableName, UserTableReturnFields, strQueryCondition, sqlOrderBy, out AllRecordsCount, StartIndex, ResultCount);
                    }

                    break;
                case 1:
                    //查询模型算法库标准算法组件
                    //MySqlBaseUti = mySQLOperator.MADB;
                    _dbBaseUti = _dbOperator.GetSubDbUtilities(EnumDBType.MADB);
                    TableName = "madb_algorithmcmp_view";
                    TableReturnFields = "*";
                    AddQueryTable_SuitSateSensor = "madb_parautility_view";

                    listUploadDate = new List<DateTime>() { };
                    //IsOnCloud = string.Empty;
                    strQueryCondition = sqlBaseTool.GetQueryCondition_AlgorithmJCGX(AddQueryTable_SuitSateSensor, listUploadDate, ArtificialType, AlgEnName, DllName, AlgCnName, SuitableSatellites, SuitableSensors, IsOnCloud);
                    if (string.IsNullOrEmpty(sqlOrderBy.Trim()))
                    {
                        returnDS = _dbBaseUti.GetList(TableName, TableReturnFields, strQueryCondition, out AllRecordsCount, StartIndex, ResultCount);
                    }
                    else
                    {
                        returnDS = _dbBaseUti.GetList(TableName, TableReturnFields, strQueryCondition, sqlOrderBy, out AllRecordsCount, StartIndex, ResultCount);
                    }
                    break;
                case 2:
                    //联合查询 模型算法库算法组件和信息服务库中的用户上传算法组件。
                    //MySqlBaseUti = mySQLOperator.ISDB;


                    break;
                default:
                    returnDS = null;
                    break;
            }
            if (returnDS != null && returnDS.Tables.Count > 0 && returnDS.Tables[0].Rows.Count > 0)
            {
                InforLog<string>.inforLog.Info("文件:WS_QDB_Searcher_MySQL/App_Code/Service.cs 方法：SearchAlgorithmJCGX 输出:returnDS不为空");
            }
            else
                InforLog<string>.inforLog.Info("文件:WS_QDB_Searcher_MySQL/App_Code/Service.cs 方法：SearchAlgorithmJCGX 输出:returnDS为空");
            return returnDS;
        }

        public System.Data.DataSet SearchDocumentJCGX(List<string> DocumentName, List<string> DocumentType, List<string> ProgramName, List<string> DocumentReleaseTime, List<string> Author, List<string> KeyWords, string IsOnCloud, out int AllRecordsCount, int StartIndex, int ResultCount, int QueryRange, List<string> strOrderBy, List<int> OrderByType)
        {
            logStr = string.Format("DocumentName='{0}' DocumentType='{1}' ProgramName='{2}' DocumentReleaseTime='{3}' Author='{4}' KeyWords='{5}' IsOnCloud='{6}' StartIndex='{7}' ResultCount='{8}' QueryRange='{9}' strOrderBy='{10}' OrderByType='{11}'", InforLog<string>.returnListStrElem(DocumentName), InforLog<string>.returnListStrElem(DocumentType), InforLog<string>.returnListStrElem(ProgramName), InforLog<string>.returnListStrElem(DocumentReleaseTime), InforLog<string>.returnListStrElem(Author), InforLog<string>.returnListStrElem(KeyWords), IsOnCloud, StartIndex, ResultCount, QueryRange, InforLog<string>.returnListStrElem(strOrderBy), InforLog<int>.returnListStrElem(OrderByType));
            InforLog<string>.inforLog.Info("文件:WS_QDB_Searcher_MySQL/App_Code/Service.cs 方法：SearchDocumentJCGX 输入:" + logStr);
            AllRecordsCount = 0;

            #region 组织排序参数及 排序类型（升序，降序）
            //string sqlOrderBy = strOrderBy;
            //if (sqlOrderBy != "")
            //{
            //    switch (OrderByType)
            //    {
            //        case 0:
            //            sqlOrderBy += " ASC";
            //            break;
            //        case 1:
            //            sqlOrderBy += " DESC";
            //            break;
            //        default:
            //            break;
            //    }
            //}
            string sqlOrderBy = "";
            if (string.Join(" ", strOrderBy).Trim() != "")
            {
                if (strOrderBy.Count == OrderByType.Count)
                {
                    for (int i = 0; i < OrderByType.Count; i++)
                    {
                        switch (OrderByType[i])
                        {
                            case 0:
                                strOrderBy[i] += " ASC";
                                break;
                            case 1:
                                strOrderBy[i] += " DESC";
                                break;
                            default:
                                break;
                        }
                        sqlOrderBy = string.Join(",", strOrderBy);
                    }
                }
                else
                {
                    throw new Exception("输入的排序字段数与输入的排列顺序个数不一致！");

                }
            }
            #endregion

            #region 组织DateTime参数，从字符串类型转化为DateTime类型
            List<DateTime> listDocumentReleaseTime = new List<DateTime>();
            try
            {
                foreach (string str in DocumentReleaseTime)
                {
                    listDocumentReleaseTime.Add(Convert.ToDateTime(str));
                }
            }
            catch
            {
                throw new Exception("输入的时间参数的格式不正确！");
            }
            #endregion

            //if (mySQLOperator == null)
            //{
            //    mySQLOperator = new DBMySqlOperating();
            //}
            string userTableName = "";
            string TableName = "";
            string UserTableReturnFields = "";
            string TableReturnFields = "";
            string strQueryCondition = "";

            switch (QueryRange)
            {
                case 0:
                    //查询用户上传文档
                    _dbBaseUti = _dbOperator.GetSubDbUtilities(EnumDBType.ISDB);
                    //MySqlBaseUti = mySQLOperator.ISDB;
                    userTableName = "isdb_document_isdb_publicinfo_view";
                    UserTableReturnFields = "*";

                    strQueryCondition = sqlBaseTool.GetQueryCondition_DocumentJCGX(DocumentName, DocumentType, ProgramName, listDocumentReleaseTime, Author, KeyWords, IsOnCloud);
                    if (string.IsNullOrEmpty(sqlOrderBy.Trim()))
                        returnDS = _dbBaseUti.GetList(userTableName, UserTableReturnFields, strQueryCondition, out AllRecordsCount, StartIndex, ResultCount);
                    else
                        returnDS = _dbBaseUti.GetList(userTableName, UserTableReturnFields, strQueryCondition, sqlOrderBy, out AllRecordsCount, StartIndex, ResultCount);
                    break;
                case 1:
                    //查询数据库标准文档
                    //MySqlBaseUti = mySQLOperator.MADB;
                    _dbBaseUti = _dbOperator.GetSubDbUtilities(EnumDBType.MADB);
                    TableName = "madb_document_view";
                    TableReturnFields = "*";

                    //IsOnCloud = string.Empty;
                    strQueryCondition = sqlBaseTool.GetQueryCondition_DocumentJCGX(DocumentName, DocumentType, ProgramName, listDocumentReleaseTime, Author, KeyWords, IsOnCloud);
                    if (string.IsNullOrEmpty(sqlOrderBy.Trim()))
                    {
                        returnDS = _dbBaseUti.GetList(TableName, TableReturnFields, strQueryCondition, out AllRecordsCount, StartIndex, ResultCount);
                    }
                    else
                    {
                        returnDS = _dbBaseUti.GetList(TableName, TableReturnFields, strQueryCondition, sqlOrderBy, out AllRecordsCount, StartIndex, ResultCount);
                    }
                    break;
                default:
                    returnDS = null;
                    break;
            }
            if (returnDS != null && returnDS.Tables.Count > 0 && returnDS.Tables[0].Rows.Count > 0)
            {
                InforLog<string>.inforLog.Info("文件:WS_QDB_Searcher_MySQL/App_Code/Service.cs 方法：SearchDocumentJCGX 输出:returnDS不为空");
            }
            else
                InforLog<string>.inforLog.Info("文件:WS_QDB_Searcher_MySQL/App_Code/Service.cs 方法：SearchDocumentJCGX 输出:returnDS为空");
            return returnDS;
        }

        public System.Data.DataSet SearchToolkitJCGX(List<string> ToolkitReleaseTime, List<string> ToolkitName, List<string> ToolkitType, List<string> SuitableSatellites, List<string> SuitableSensors, List<string> OSBits, List<string> OStype, List<string> Author, List<string> KeyWords, string IsOnCloud, out int AllRecordsCount, int StartIndex, int ResultCount, int QueryRange, List<string> strOrderBy, List<int> OrderByType)
        {
            logStr = string.Format("ToolkitReleaseTime='{0}' ToolkitName='{1}' ToolkitType='{2}' SuitableSatellites='{3}' SuitableSensors='{4}' OSBits='{5}' OStype='{6}' Author='{7}' KeyWords='{8}' IsOnCloud='{9}' StartIndex='{10}' ResultCount='{11}' QueryRange='{12}' strOrderBy='{13}' OrderByType='{14}'", InforLog<string>.returnListStrElem(ToolkitReleaseTime), InforLog<string>.returnListStrElem(ToolkitName), InforLog<string>.returnListStrElem(ToolkitType), InforLog<string>.returnListStrElem(SuitableSatellites), InforLog<string>.returnListStrElem(SuitableSensors), InforLog<string>.returnListStrElem(OSBits), InforLog<string>.returnListStrElem(OStype), InforLog<string>.returnListStrElem(Author), InforLog<string>.returnListStrElem(KeyWords), IsOnCloud, StartIndex, ResultCount, QueryRange, InforLog<string>.returnListStrElem(strOrderBy), InforLog<int>.returnListStrElem(OrderByType));
            InforLog<string>.inforLog.Info("文件:WS_QDB_Searcher_MySQL/App_Code/Service.cs 方法：SearchToolkitJCGX 输入:" + logStr);
            AllRecordsCount = 0;

            #region 组织排序参数及 排序类型（升序，降序）

            //string sqlOrderBy =string.Join(",", strOrderBy);
            string sqlOrderBy = "";
            if (string.Join(" ", strOrderBy).Trim() != "")
            {
                if (strOrderBy.Count == OrderByType.Count)
                {
                    for (int i = 0; i < OrderByType.Count; i++)
                    {
                        switch (OrderByType[i])
                        {
                            case 0:
                                strOrderBy[i] += " ASC";
                                break;
                            case 1:
                                strOrderBy[i] += " DESC";
                                break;
                            default:
                                break;
                        }
                        sqlOrderBy = string.Join(",", strOrderBy);
                    }
                }
                else
                {
                    throw new Exception("输入的排序字段数与输入的排列顺序个数不一致！");

                }
            }
            #endregion

            #region 组织DateTime参数，从字符串类型转化为DateTime类型
            List<DateTime> listReleaseTime = new List<DateTime>();
            try
            {
                foreach (string str in ToolkitReleaseTime)
                {
                    listReleaseTime.Add(Convert.ToDateTime(str));
                }
            }
            catch
            {
                throw new Exception("输入的时间参数的格式不正确！");
            }
            #endregion
            //if (mySQLOperator == null)
            //{
            //    mySQLOperator = new DBMySqlOperating();
            //}
            string userTableName = "";
            string TableName = "";
            string UserTableReturnFields = "";
            string TableReturnFields = "";
            string strQueryCondition = "";

            switch (QueryRange)
            {
                case 0:
                    //查询用户上传工具组件
                    //MySqlBaseUti = mySQLOperator.ISDB;
                    _dbBaseUti = _dbOperator.GetSubDbUtilities(EnumDBType.ISDB);
                    userTableName = "isdb_toolkit_isdb_publicinfo_view";
                    UserTableReturnFields = "*";

                    strQueryCondition = sqlBaseTool.GetQueryCondition_ToolkitJCGX(listReleaseTime, ToolkitName, ToolkitType, SuitableSatellites, SuitableSensors, OSBits, OStype, Author, KeyWords, IsOnCloud);
                    if (string.IsNullOrEmpty(sqlOrderBy.Trim()))
                    {
                        returnDS = _dbBaseUti.GetList(userTableName, UserTableReturnFields, strQueryCondition, out AllRecordsCount, StartIndex, ResultCount);
                    }
                    else
                    {
                        returnDS = _dbBaseUti.GetList(userTableName, UserTableReturnFields, strQueryCondition, sqlOrderBy, out AllRecordsCount, StartIndex, ResultCount);
                    }
                    break;
                case 1:
                    //查询标准工具
                    //MySqlBaseUti = mySQLOperator.MADB;
                    _dbBaseUti = _dbOperator.GetSubDbUtilities(EnumDBType.MADB);
                    TableName = "madb_toolkit_view";
                    TableReturnFields = "*";
                    //IsOnCloud = "";

                    strQueryCondition = sqlBaseTool.GetQueryCondition_ToolkitJCGX(listReleaseTime, ToolkitName, ToolkitType, SuitableSatellites, SuitableSensors, OSBits, OStype, Author, KeyWords, IsOnCloud);
                    if (string.IsNullOrEmpty(sqlOrderBy.Trim()))
                        returnDS = _dbBaseUti.GetList(TableName, TableReturnFields, strQueryCondition, out AllRecordsCount, StartIndex, ResultCount);
                    else
                        returnDS = _dbBaseUti.GetList(TableName, TableReturnFields, strQueryCondition, sqlOrderBy, out AllRecordsCount, StartIndex, ResultCount);
                    break;
                default:
                    returnDS = null;
                    break;
            }
            if (returnDS != null && returnDS.Tables.Count > 0 && returnDS.Tables[0].Rows.Count > 0)
            {
                InforLog<string>.inforLog.Info("文件:WS_QDB_Searcher_MySQL/App_Code/Service.cs 方法：SearchToolkitJCGX 输出:returnDS不为空");
            }
            else
                InforLog<string>.inforLog.Info("文件:WS_QDB_Searcher_MySQL/App_Code/Service.cs 方法：SearchToolkitJCGX 输出:returnDS为空");
            return returnDS;
        }

        public System.Data.DataSet SearchCorrectedDataJCGX(List<string> position, List<string> datetime, List<string> satellite, List<string> sensor, List<string> PixelSpacing, List<int> DataSizeRange, string IsOnCloud, out int AllRecordsCount, int StartIndex, int ResultCount, int QueryRange, List<string> strOrderBy, List<int> OrderByType)
        {
            logStr = string.Format("position='{0}' datetime='{1}' satellite='{2}' sensor='{3}' PixelSpacing='{4}' DataSizeRange='{5}' IsOnCloud='{6}' StartIndex='{7}' ResultCount='{8}' QueryRange='{9}' strOrderBy='{10}' OrderByType='{11}'", InforLog<string>.returnListStrElem(position), InforLog<string>.returnListStrElem(datetime), InforLog<string>.returnListStrElem(satellite), InforLog<string>.returnListStrElem(sensor), InforLog<string>.returnListStrElem(PixelSpacing), InforLog<int>.returnListStrElem(DataSizeRange), IsOnCloud, StartIndex, ResultCount, QueryRange, InforLog<string>.returnListStrElem(strOrderBy), InforLog<int>.returnListStrElem(OrderByType));
            InforLog<string>.inforLog.Info("文件:WS_QDB_Searcher_MySQL/App_Code/Service.cs 方法：SearchCorrectedDataJCGX 输入:" + logStr);
            AllRecordsCount = 0;

            #region 组织排序参数及 排序类型（升序，降序）
            //string sqlOrderBy =string.Join(",", strOrderBy);
            string sqlOrderBy = "";
            if (string.Join(" ", strOrderBy).Trim() != "")
            {
                if (strOrderBy.Count == OrderByType.Count)
                {
                    for (int i = 0; i < OrderByType.Count; i++)
                    {
                        switch (OrderByType[i])
                        {
                            case 0:
                                strOrderBy[i] += " ASC";
                                break;
                            case 1:
                                strOrderBy[i] += " DESC";
                                break;
                            default:
                                break;
                        }
                        sqlOrderBy = string.Join(",", strOrderBy);
                    }
                }
                else
                {
                    throw new Exception("输入的排序字段数与输入的排列顺序个数不一致！");

                }
            }
            #endregion

            #region 组织DateTime参数，从字符串类型转化为DateTime类型
            List<DateTime> listDatetime = new List<DateTime>();
            try
            {
                foreach (string str in datetime)
                {
                    listDatetime.Add(Convert.ToDateTime(str));
                }
            }
            catch
            {
                throw new Exception("输入的时间参数的格式不正确！");
            }
            #endregion

            //if (mySQLOperator == null)
            //{
            //    mySQLOperator = new DBMySqlOperating();
            //}
            string userTableName = "";
            string TableName = "";
            string UserTableReturnFields = "";
            string TableReturnFields = "";
            string strQueryCondition = "";

            //用户上传栅格数据不区分纠正后和产品切片，都存在同一表中。在组织sql时要添加查询语句，选出属于预处理的记录。

            string dataType = string.Empty;
            switch (QueryRange)
            {
                case 0:
                    //数据类型字段，只在查询用户上传栅格数据表的时候用到
                    dataType = "preProcessData";
                    //查询用户上传纠正后栅格数据
                    _dbBaseUti = _dbOperator.GetSubDbUtilities(EnumDBType.ISDB);
                    //MySqlBaseUti = mySQLOperator.ISDB;
                    userTableName = "isdb_userraster_isdb_publicinfo_view";
                    UserTableReturnFields = @"QRST_CODE,proName,proEngName,proLevel,prodataName,productDate,dataName,satelliteId,sensorId,sceneDate,pixelSpacing,sceneCenterLat,sceneCenterLong,
                                         dataUpperLeftLat,dataUpperLeftLong,dataUpperRightLat,dataUpperRightLong,dataLowerLeftLat,dataLowerLeftLong,dataLowerRightLat,dataLowerRightLong,
                                         dataFormatDes,SceneRow,ScenePath,mapProjection,UserName,UpLoadTime,Department,Price,PublicCloud,FileSize,DownloadCount,AverageScore";

                    strQueryCondition = sqlBaseTool.GetQueryCondition_CorrectedDataJCGX(position, listDatetime, satellite, sensor, PixelSpacing, DataSizeRange, IsOnCloud, dataType);
                    if (string.IsNullOrEmpty(sqlOrderBy.Trim()))
                    {
                        returnDS = _dbBaseUti.GetList(userTableName, UserTableReturnFields, strQueryCondition, out AllRecordsCount, StartIndex, ResultCount);
                    }
                    else
                    {
                        returnDS = _dbBaseUti.GetList(userTableName, UserTableReturnFields, strQueryCondition, sqlOrderBy, out AllRecordsCount, StartIndex, ResultCount);
                    }
                    break;
                case 1:
                    //查询标准纠正后栅格数据
                    //MySqlBaseUti = mySQLOperator.EVDB;
                    _dbBaseUti = _dbOperator.GetSubDbUtilities(EnumDBType.EVDB);
                    //要查询的数据对应的表名或者视图名
                    TableName = "prod_hj_view";
                    //要返回的字段名
                    TableReturnFields = "*";

                    //标准纠正后栅格数据中没有  数据大小字段和是否在公有云中的字段。标准数据表进行查询的时候  也不需要传入 DataType字段。
                    //dataType = string.Empty;
                    //DataSizeRange = new List<int>();
                    //IsOnCloud = "";
                    //查询条件语句
                    strQueryCondition = sqlBaseTool.GetQueryCondition_CorrectedDataJCGX_1(position, listDatetime, satellite, sensor, PixelSpacing, DataSizeRange, IsOnCloud, dataType);
                    if (string.IsNullOrEmpty(sqlOrderBy.Trim()))
                    {
                        returnDS = _dbBaseUti.GetList(TableName, TableReturnFields, strQueryCondition, out AllRecordsCount, StartIndex, ResultCount);
                    }
                    else
                    {
                        returnDS = _dbBaseUti.GetList(TableName, TableReturnFields, strQueryCondition, sqlOrderBy, out AllRecordsCount, StartIndex, ResultCount);
                    }

                    break;
                case 2:
                    //联合查询 标准纠正后栅格数据和信息服务库中的用户上传纠正后栅格数据。
                    //MySqlBaseUti = mySQLOperator.ISDB;
                    returnDS = null;
                    break;
                default:
                    returnDS = null;
                    break;
            }
            if (returnDS != null && returnDS.Tables.Count > 0 && returnDS.Tables[0].Rows.Count > 0)
            {
                InforLog<string>.inforLog.Info("文件:WS_QDB_Searcher_MySQL/App_Code/Service.cs 方法：SearchCorrectedDataJCGX 输出:returnDS不为空");
            }
            else
                InforLog<string>.inforLog.Info("文件:WS_QDB_Searcher_MySQL/App_Code/Service.cs 方法：SearchCorrectedDataJCGX 输出:returnDS为空");
            return returnDS;
        }

        public System.Data.DataSet SearchGF1DataJCGX(List<string> position, List<string> datetime, List<string> satellite, List<string> sensor, out int AllRecordsCount, int StartIndex, int ResultCount, List<string> strOrderBy, List<int> OrderByType)
        {
            //DBMySqlOperating mySQLOperator = new DBMySqlOperating();
            //配置基本查询信息

            QueryRequest qr = new QueryRequest();
            qr.dataBase = "EVDB";
            qr.elementSet = new string[] { "*" };
            qr.tableCode = "EVDB-32";
            qr.recordSetStartPointSpecified = StartIndex;
            qr.offset = ResultCount;

            _dbBaseUti = _dbOperator.GetSubDbUtilities(EnumDBType.EVDB);
            IGetQuerySchema querySchema = new FieldViewBasedQuerySchema(qr.elementSet, qr.tableCode, _dbBaseUti);
            //排序字段
            if (!(strOrderBy == null || OrderByType == null || strOrderBy.Count != OrderByType.Count))
            {
                OrderBy[] orderByArr = new OrderBy[strOrderBy.Count];
                for (int i = 0; i < orderByArr.Length; i++)
                {
                    orderByArr[i] = new OrderBy();
                    orderByArr[i].accessPointField = strOrderBy[i];
                    orderByArr[i].orderType = OrderByType[i] == 0 ? OrderType.ASC : OrderType.DESC;
                }
                qr.orderBy = orderByArr;
            }

            //设置查询条件
            RasterQueryPara _rasterQueryPara = new RasterQueryPara();
            _rasterQueryPara.dataCode = qr.tableCode;
            if (datetime != null && datetime[0] != null)
            {
                _rasterQueryPara.STARTTIME = datetime[0];
            }
            if (datetime != null && datetime[1] != null)
            {
                _rasterQueryPara.ENDTIME = datetime[1];
            }
            if (position != null && position.Count >= 4)
            {
                _rasterQueryPara.EXTENTDOWN = position[0];
                _rasterQueryPara.EXTENTLEFT = position[1];
                _rasterQueryPara.EXTENTUP = position[2];
                _rasterQueryPara.EXTENTRIGHT = position[3];
            }
            //卫星
            string _satellite = "";
            if (satellite != null)
            {
                for (int i = 0; i < satellite.Count; i++)
                {
                    _satellite = String.Format("{0}{1},", _satellite, satellite[i]);
                }
            }

            if (!string.IsNullOrEmpty(_satellite))
                _rasterQueryPara.SATELLITE = _satellite;

            //传感器
            string _sensor = "";
            if (sensor != null)
            {
                for (int i = 0; i < sensor.Count; i++)
                {
                    _sensor = String.Format("{0}{1},", _sensor, sensor[i]);
                }
            }

            if (!string.IsNullOrEmpty(_sensor))
                _rasterQueryPara.SENSOR = _sensor;
            string _ratio = "";
            //if (ratio != null)
            //{
            //    for (int i = 0; i < ratio.Count; i++)
            //    {
            //        _ratio = String.Format("{0}{1},", _ratio, ratio[i]);
            //    }
            //    _ratio = _ratio.TrimEnd(',');
            //}

            //if (!string.IsNullOrEmpty(_ratio))
            //    _rasterQueryPara.RATIO = _ratio;

            qr.complexCondition = _rasterQueryPara.GetSpecificCondition(querySchema);

            ViewBasedQuery query = new ViewBasedQuery(qr, querySchema);
            AllRecordsCount = query.GetRecordCount();
            QueryResponse queryResponse = query.Query();
            return queryResponse.recordSet;
        }

        public System.Data.DataSet SearchGFByRegion(string regionName, string category, string type, List<string> datetime, List<string> satellite, List<string> sensor, out int AllRecordsCount, int StartIndex, int ResultCount, List<string> strOrderBy, List<int> OrderByType)
        {
            List<string> position = new List<string>();
            DotSpatial.Data.IFeatureSet ProVftset, Cityftset, Countyftset;

            string shpPath = string.Format("{0}\\{1}", Application.StartupPath.ToString(), "map\\provincialBoundary.shp");
            ProVftset = DotSpatial.Data.Shapefile.Open(shpPath);
            string cityShpPath = string.Format("{0}\\{1}", Application.StartupPath.ToString(), "map\\cityBoundary.shp");
            //string cityShpPath = WebService.Server.MapPath("~/map/cityBoundary.shp");
            Cityftset = DotSpatial.Data.Shapefile.Open(cityShpPath);
            string countryShpPath = string.Format("{0}\\{1}", Application.StartupPath.ToString(), "map\\countyBoundary.shp");
            //string countryShpPath = WebService.Server.MapPath("~/map/countyBoundary.shp");
            Countyftset = DotSpatial.Data.Shapefile.Open(countryShpPath);
            string maxLon = null, minLon = null, maxLat = null, minLat = null;
            DotSpatial.Data.IFeature iFeature = null;
            if (category == "省")
            {
                foreach (DotSpatial.Data.Feature f in ProVftset.Features)
                {
                    if (f.DataRow["Name"].ToString() == regionName)
                    {
                        iFeature = f;
                        List<double> latList = new List<double>();
                        List<double> lonList = new List<double>();
                        foreach (var coordinate in f.Coordinates.ToList())
                        {
                            lonList.Add(coordinate[0]);
                            latList.Add(coordinate[1]);
                        }
                        maxLat = latList.Max().ToString();
                        minLat = latList.Min().ToString();
                        maxLon = lonList.Max().ToString();
                        minLon = lonList.Min().ToString();
                        break;
                    }
                }
            }
            else if (category == "市")
            {
                foreach (DotSpatial.Data.Feature f in Cityftset.Features)
                {
                    if (f.DataRow["Name"].ToString() == regionName)
                    {
                        iFeature = f;
                        List<double> latList = new List<double>();
                        List<double> lonList = new List<double>();
                        foreach (var coordinate in f.Coordinates.ToList())
                        {
                            lonList.Add(coordinate[0]);
                            latList.Add(coordinate[1]);
                        }
                        maxLat = latList.Max().ToString();
                        minLat = latList.Min().ToString();
                        maxLon = lonList.Max().ToString();
                        minLon = lonList.Min().ToString();
                        break;
                    }
                }
            }
            else
            {
                foreach (DotSpatial.Data.Feature f in Countyftset.Features)
                {
                    if (f.DataRow["Name"].ToString() == regionName)
                    {
                        iFeature = f;
                        List<double> latList = new List<double>();
                        List<double> lonList = new List<double>();
                        foreach (var coordinate in f.Coordinates.ToList())
                        {
                            lonList.Add(coordinate[0]);
                            latList.Add(coordinate[1]);
                        }
                        maxLat = latList.Max().ToString();
                        minLat = latList.Min().ToString();
                        maxLon = lonList.Max().ToString();
                        minLon = lonList.Min().ToString();
                        break;
                    }
                }
            }

            position.Add(minLon);
            position.Add(minLat);
            position.Add(maxLon);
            position.Add(maxLat);
            //DBMySqlOperating mySQLOperator = new DBMySqlOperating();
            //配置基本查询信息
            _dbBaseUti = _dbOperator.GetSubDbUtilities(EnumDBType.EVDB);
            QueryRequest qr = new QueryRequest();
            qr.dataBase = "EVDB";
            qr.elementSet = new string[] { "*" };
            qr.tableCode = "EVDB-32";
            qr.recordSetStartPointSpecified = StartIndex;
            qr.offset = ResultCount;


            IGetQuerySchema querySchema = new FieldViewBasedQuerySchema(qr.elementSet, qr.tableCode, _dbBaseUti);
            //排序字段
            if (!(strOrderBy == null || OrderByType == null || strOrderBy.Count != OrderByType.Count))
            {
                OrderBy[] orderByArr = new OrderBy[strOrderBy.Count];
                for (int i = 0; i < orderByArr.Length; i++)
                {
                    orderByArr[i] = new OrderBy();
                    orderByArr[i].accessPointField = strOrderBy[i];
                    orderByArr[i].orderType = OrderByType[i] == 0 ? OrderType.ASC : OrderType.DESC;
                }
                qr.orderBy = orderByArr;
            }

            //设置查询条件
            RasterQueryPara _rasterQueryPara = new RasterQueryPara();
            _rasterQueryPara.dataCode = qr.tableCode;
            if (datetime != null && datetime[0] != null)
            {
                _rasterQueryPara.STARTTIME = datetime[0];
            }
            if (datetime != null && datetime[1] != null)
            {
                _rasterQueryPara.ENDTIME = datetime[1];
            }
            if (position != null && position.Count >= 4)
            {
                _rasterQueryPara.EXTENTDOWN = position[0];
                _rasterQueryPara.EXTENTLEFT = position[1];
                _rasterQueryPara.EXTENTUP = position[2];
                _rasterQueryPara.EXTENTRIGHT = position[3];
            }
            //卫星
            string _satellite = "";
            if (satellite != null)
            {
                for (int i = 0; i < satellite.Count; i++)
                {
                    _satellite = String.Format("{0}{1},", _satellite, satellite[i]);
                }
            }

            if (!string.IsNullOrEmpty(_satellite))
                _rasterQueryPara.SATELLITE = _satellite;

            //传感器
            string _sensor = "";
            if (sensor != null)
            {
                for (int i = 0; i < sensor.Count; i++)
                {
                    _sensor = String.Format("{0}{1},", _sensor, sensor[i]);
                }
            }

            if (!string.IsNullOrEmpty(_sensor))
                _rasterQueryPara.SENSOR = _sensor;
            string _ratio = "";
            //if (ratio != null)
            //{
            //    for (int i = 0; i < ratio.Count; i++)
            //    {
            //        _ratio = String.Format("{0}{1},", _ratio, ratio[i]);
            //    }
            //    _ratio = _ratio.TrimEnd(',');
            //}

            //if (!string.IsNullOrEmpty(_ratio))
            //    _rasterQueryPara.RATIO = _ratio;

            qr.complexCondition = _rasterQueryPara.GetSpecificCondition(querySchema);

            ViewBasedQuery query = new ViewBasedQuery(qr, querySchema);
            //AllRecordsCount = query.GetRecordCount(iFeature);
            QueryResponse queryResponse = query.Query();
            if (queryResponse.recordSet != null && queryResponse.recordSet.Tables[0].Rows.Count > 0)
            {
                System.Data.DataTable tab = queryResponse.recordSet.Tables[0];
                for (int i = tab.Rows.Count - 1; i > -1; i--)
                {
                    IGeometry poly = getGeomFromRow(tab.Rows[i]);
                    if (!DotSpatial.Data.FeatureExt.Intersects(iFeature, poly))
                    {
                        tab.Rows.RemoveAt(i);
                    }
                }
                queryResponse.recordSet.Tables.Clear();
                queryResponse.recordSet.Tables.Add(tab);
            }
            AllRecordsCount = queryResponse.recordSet.Tables[0].Rows.Count;
            return queryResponse.recordSet;
        }

        public System.Data.DataSet SearchBatchGF1DataJCGX(List<string> position, List<string> datetime, List<string> satellite, List<string> sensor, out int AllRecordsCount, int StartIndex, int ResultCount, List<string> strOrderBy, List<int> OrderByType, string PointField, string KeyWords, string KeyValues)
        {
            logStr = string.Format("position='{0}' datetime='{1}' satellite='{2}' sensor='{3}' StartIndex='{4}' ResultCount='{5}' strOrderBy='{6}' OrderByType='{7}' PointField='{8}' KeyWords='{9}' KeyValues='{10}'", InforLog<string>.returnListStrElem(position), InforLog<string>.returnListStrElem(datetime), InforLog<string>.returnListStrElem(satellite), InforLog<string>.returnListStrElem(sensor), StartIndex, ResultCount, InforLog<string>.returnListStrElem(strOrderBy), InforLog<int>.returnListStrElem(OrderByType), PointField, KeyWords, KeyValues);
            InforLog<string>.inforLog.Info("文件:WS_QDB_Searcher_MySQL/App_Code/Service.cs 方法：SearchBatchGF1DataJCGX 输入:" + logStr);
            //DBMySqlOperating mySQLOperator = new DBMySqlOperating();
            //配置基本查询信息
            _dbBaseUti = _dbOperator.GetSubDbUtilities(EnumDBType.EVDB);
            QueryRequest qr = new QueryRequest();
            qr.dataBase = "EVDB";
            qr.elementSet = new string[] { "*" };
            qr.tableCode = "EVDB-32";
            qr.recordSetStartPointSpecified = StartIndex;
            qr.offset = ResultCount;


            IGetQuerySchema querySchema = new FieldViewBasedQuerySchema(qr.elementSet, qr.tableCode, _dbBaseUti);
            //排序字段
            if (!(strOrderBy == null || OrderByType == null || strOrderBy.Count != OrderByType.Count))
            {
                OrderBy[] orderByArr = new OrderBy[strOrderBy.Count];
                for (int i = 0; i < orderByArr.Length; i++)
                {
                    orderByArr[i] = new OrderBy();
                    orderByArr[i].accessPointField = strOrderBy[i];
                    orderByArr[i].orderType = OrderByType[i] == 0 ? OrderType.ASC : OrderType.DESC;
                }
                qr.orderBy = orderByArr;
            }

            //设置查询条件
            RasterQueryPara _rasterQueryPara = new RasterQueryPara();
            _rasterQueryPara.dataCode = qr.tableCode;
            if (datetime != null && datetime[0] != null)
            {
                _rasterQueryPara.STARTTIME = datetime[0];
            }
            if (datetime != null && datetime[1] != null)
            {
                _rasterQueryPara.ENDTIME = datetime[1];
            }
            if (position != null && position.Count >= 4)
            {
                _rasterQueryPara.EXTENTDOWN = position[0];
                _rasterQueryPara.EXTENTLEFT = position[1];
                _rasterQueryPara.EXTENTUP = position[2];
                _rasterQueryPara.EXTENTRIGHT = position[3];
            }
            //卫星
            string _satellite = "";
            if (satellite != null)
            {
                for (int i = 0; i < satellite.Count; i++)
                {
                    _satellite = String.Format("{0}{1},", _satellite, satellite[i]);
                }
            }

            if (!string.IsNullOrEmpty(_satellite))
                _rasterQueryPara.SATELLITE = _satellite;

            //传感器
            string _sensor = "";
            if (sensor != null)
            {
                for (int i = 0; i < sensor.Count; i++)
                {
                    _sensor = String.Format("{0}{1},", _sensor, sensor[i]);
                }
            }

            if (!string.IsNullOrEmpty(_sensor))
                _rasterQueryPara.SENSOR = _sensor;
            QRST_DI_SS_Basis.MetadataQuery.SimpleCondition sm = new SimpleCondition();
            sm.accessPointField = PointField;
            sm.comparisonOperatorField = KeyWords;
            sm.valueField = KeyValues;
            if (sm.comparisonOperatorField.Equals("like"))
            {
                sm.valueField = string.Format("%{0}%", sm.valueField);
            }
            if (sm.comparisonOperatorField.Equals("in"))
            {
                string[] values = sm.valueField.Split(',');
                string strvalue = "";
                foreach (var item in values)
                {
                    strvalue += "'" + item + "',";
                }
                strvalue = strvalue.TrimEnd(',');
                sm.valueField = string.Format(" ({0})", strvalue);
            }
            qr.complexCondition = _rasterQueryPara.GetSpecificCondition(querySchema);
            qr.complexCondition.simpleCondition = new SimpleCondition[] { sm };
            ViewBasedQuery query = new ViewBasedQuery(qr, querySchema);
            AllRecordsCount = query.GetRecordCount();
            QueryResponse queryResponse = query.Query();
            if (queryResponse.recordSet != null && queryResponse.recordSet.Tables.Count > 0 && queryResponse.recordSet.Tables[0].Rows.Count > 0)
            {
                InforLog<string>.inforLog.Info("文件:WS_QDB_Searcher_MySQL/App_Code/Service.cs 方法：SearchGF1DataJCGX 输出:queryResponse.recordSet不为空");
            }
            else
                InforLog<string>.inforLog.Info("文件:WS_QDB_Searcher_MySQL/App_Code/Service.cs 方法：SearchGF1DataJCGX 输出:queryResponse.recordSet为空");
            return queryResponse.recordSet;
        }

        public System.Data.DataSet SearchData()
        {

            //DBMySqlOperating mySQLOperator = new DBMySqlOperating();
            //配置基本查询信息
            _dbBaseUti = _dbOperator.GetSubDbUtilities(EnumDBType.EVDB);
            QueryRequest qr = new QueryRequest();
            qr.dataBase = "EVDB";
            qr.elementSet = new string[] { "*" };
            qr.tableCode = "EVDB-32";
            qr.recordSetStartPointSpecified = 0;
            qr.offset = 10;


            IGetQuerySchema querySchema = new FieldViewBasedQuerySchema(qr.elementSet, qr.tableCode, _dbBaseUti);



            //设置查询条件
            RasterQueryPara _rasterQueryPara = new RasterQueryPara();
            _rasterQueryPara.dataCode = qr.tableCode;

            _rasterQueryPara.EXTENTDOWN = "-90";
            _rasterQueryPara.EXTENTLEFT = "-180";
            _rasterQueryPara.EXTENTUP = "90";
            _rasterQueryPara.EXTENTRIGHT = "180";

            qr.complexCondition = _rasterQueryPara.GetSpecificCondition(querySchema);

            ViewBasedQuery query = new ViewBasedQuery(qr, querySchema);

            QueryResponse queryResponse = query.Query();
            return queryResponse.recordSet;
        }

        public System.Data.DataSet SearchGF1DataForJCGXByImportTime(String importStartTime, String importEndTime)
        {
            List<string> datetime = new List<string>();
            datetime.Add(importStartTime);
            datetime.Add(importEndTime);
            int AllRecordsCount;
            //DBMySqlOperating mySQLOperator = new DBMySqlOperating();
            //配置基本查询信息
            QueryRequest qr = new QueryRequest();
            qr.dataBase = "EVDB";
            qr.elementSet = new string[] { "*" };
            qr.tableCode = "EVDB-32";
            qr.recordSetStartPointSpecified = 0;
            qr.offset = int.MaxValue;

            List<string> position = null;
            List<string> strOrderBy = null;
            List<int> OrderByType = null;
            List<string> satellite = null;
            List<string> sensor = null;
            _dbBaseUti = _dbOperator.GetSubDbUtilities(EnumDBType.EVDB);
            IGetQuerySchema querySchema = new FieldViewBasedQuerySchema(qr.elementSet, qr.tableCode, _dbBaseUti);
            //排序字段
            if (!(strOrderBy == null || OrderByType == null || strOrderBy.Count != OrderByType.Count))
            {
                OrderBy[] orderByArr = new OrderBy[strOrderBy.Count];
                for (int i = 0; i < orderByArr.Length; i++)
                {
                    orderByArr[i] = new OrderBy();
                    orderByArr[i].accessPointField = strOrderBy[i];
                    orderByArr[i].orderType = OrderByType[i] == 0 ? OrderType.ASC : OrderType.DESC;
                }
                qr.orderBy = orderByArr;
            }

            //设置查询条件
            RasterQueryPara _rasterQueryPara = new RasterQueryPara();
            _rasterQueryPara.dataCode = qr.tableCode;
            if (datetime != null && datetime[0] != null)
            {
                _rasterQueryPara.IMPORTSTARTTIME = datetime[0];
            }
            if (datetime != null && datetime[1] != null)
            {
                _rasterQueryPara.IMPORTENDTIME = datetime[1];
            }
            if (position != null && position.Count >= 4)
            {
                _rasterQueryPara.EXTENTDOWN = position[0];
                _rasterQueryPara.EXTENTLEFT = position[1];
                _rasterQueryPara.EXTENTUP = position[2];
                _rasterQueryPara.EXTENTRIGHT = position[3];
            }
            //卫星
            string _satellite = "";
            if (satellite != null)
            {
                for (int i = 0; i < satellite.Count; i++)
                {
                    _satellite = String.Format("{0}{1},", _satellite, satellite[i]);
                }
            }

            if (!string.IsNullOrEmpty(_satellite))
                _rasterQueryPara.SATELLITE = _satellite;

            //传感器
            string _sensor = "";
            if (sensor != null)
            {
                for (int i = 0; i < sensor.Count; i++)
                {
                    _sensor = String.Format("{0}{1},", _sensor, sensor[i]);
                }
            }

            if (!string.IsNullOrEmpty(_sensor))
                _rasterQueryPara.SENSOR = _sensor;
            string _ratio = "";

            qr.complexCondition = _rasterQueryPara.GetSpecificCondition(querySchema);

            ViewBasedQuery query = new ViewBasedQuery(qr, querySchema);
            AllRecordsCount = query.GetRecordCount();
            QueryResponse queryResponse = query.Query();
            return queryResponse.recordSet;
        }

        public System.Data.DataSet SearchGFFDataJCGX(List<string> position, List<string> datetime, List<string> satellite, List<string> sensor, List<string> PixelSpacing, List<int> DataSizeRange, string IsOnCloud, out int AllRecordsCount, int StartIndex, int ResultCount, List<string> strOrderBy, List<int> OrderByType)
        {
            logStr = string.Format("position='{0}' datetime='{1}' satellite='{2}' sensor='{3}' PixelSpacing='{4}' DataSizeRange='{5}' IsOnCloud='{6}' StartIndex='{7}' ResultCount='{8}' strOrderBy='{9}' OrderByType='{10}'", InforLog<string>.returnListStrElem(position), InforLog<string>.returnListStrElem(datetime), InforLog<string>.returnListStrElem(satellite), InforLog<string>.returnListStrElem(sensor), InforLog<string>.returnListStrElem(PixelSpacing), InforLog<int>.returnListStrElem(DataSizeRange), IsOnCloud, StartIndex, ResultCount, InforLog<string>.returnListStrElem(strOrderBy), InforLog<int>.returnListStrElem(OrderByType));
            InforLog<string>.inforLog.Info("文件:WS_QDB_Searcher_MySQL/App_Code/Service.cs 方法：SearchGFFDataJCGX 输入:" + logStr);
            AllRecordsCount = 0;

            #region 组织排序参数及 排序类型（升序，降序）
            //string sqlOrderBy =string.Join(",", strOrderBy);
            string sqlOrderBy = "";
            if (string.Join(" ", strOrderBy).Trim() != "")
            {
                if (strOrderBy.Count == OrderByType.Count)
                {
                    for (int i = 0; i < OrderByType.Count; i++)
                    {
                        switch (OrderByType[i])
                        {
                            case 0:
                                strOrderBy[i] += " ASC";
                                break;
                            case 1:
                                strOrderBy[i] += " DESC";
                                break;
                            default:
                                break;
                        }
                        sqlOrderBy = string.Join(",", strOrderBy);
                    }
                }
                else
                {
                    throw new Exception("输入的排序字段数与输入的排列顺序个数不一致！");

                }
            }
            #endregion

            #region 组织DateTime参数，从字符串类型转化为DateTime类型
            List<DateTime> listDatetime = new List<DateTime>();
            try
            {
                foreach (string str in datetime)
                {
                    listDatetime.Add(Convert.ToDateTime(str));
                }
            }
            catch
            {
                throw new Exception("输入的时间参数的格式不正确！");
            }
            #endregion

            //if (mySQLOperator == null)
            //{
            //    mySQLOperator = new DBMySqlOperating();
            //}
            string userTableName = "";
            string TableName = "";
            string UserTableReturnFields = "";
            string TableReturnFields = "";
            string strQueryCondition = "";

            //查询标准纠正后栅格数据
            //MySqlBaseUti = mySQLOperator.EVDB;
            _dbBaseUti = _dbOperator.GetSubDbUtilities(EnumDBType.EVDB);
            //要查询的数据对应的表名或者视图名
            TableName = "evdb_gff_view";
            //要返回的字段名
            TableReturnFields = "*";

            //查询条件语句
            strQueryCondition = sqlBaseTool.GetQueryCondition_GFFDataJCGX(position, listDatetime, satellite, sensor, PixelSpacing, DataSizeRange, IsOnCloud);
            if (string.IsNullOrEmpty(sqlOrderBy.Trim()))
            {
                returnDS = _dbBaseUti.GetList(TableName, TableReturnFields, strQueryCondition, out AllRecordsCount, StartIndex, ResultCount);
            }
            else
            {
                returnDS = _dbBaseUti.GetList(TableName, TableReturnFields, strQueryCondition, sqlOrderBy, out AllRecordsCount, StartIndex, ResultCount);
            }
            //switch (QueryRange)
            //{
            //    case 0:
            //        break;
            //    case 1:
            //        break;
            //    default:
            //        returnDS = null;
            //        break;
            //}
            if (returnDS != null && returnDS.Tables.Count > 0 && returnDS.Tables[0].Rows.Count > 0)
            {
                InforLog<string>.inforLog.Info("文件:WS_QDB_Searcher_MySQL/App_Code/Service.cs 方法：SearchGFFDataJCGX 输出:returnDS不为空");
            }
            else
                InforLog<string>.inforLog.Info("文件:WS_QDB_Searcher_MySQL/App_Code/Service.cs 方法：SearchGFFDataJCGX 输出:returnDS为空");
            return returnDS;
        }


        #endregion

        #region 生产线接口
        public System.Data.DataSet SearchAlgorithmPL(string AlgEnName, string AlgCnName, string ComponentVersion, out int AllRecordsCount, int StartIndex, int ResultCount)
        {
            logStr = string.Format("AlgEnName='{0}' AlgCnName='{1}' ComponentVersion='{2}' StartIndex='{3}' ResultCount='{4}'", AlgEnName, AlgCnName, ComponentVersion, StartIndex, ResultCount);
            InforLog<string>.inforLog.Info("文件:WS_QDB_Searcher_MySQL/App_Code/Service.cs 方法：SearchAlgorithmPL 输入:" + logStr);
            AllRecordsCount = 0;

            //if (mySQLOperator == null)
            //{
            //    mySQLOperator = new DBMySqlOperating();
            //}
            string TableName = "";
            string TableReturnFields = "";
            string strQueryCondition = "";

            //查询用户上传算法组件
            //MySqlBaseUti = mySQLOperator.MADB;
            _dbBaseUti = _dbOperator.GetSubDbUtilities(EnumDBType.MADB);
            TableName = "madb_algorithmcmp_view";
            TableReturnFields = "*";

            strQueryCondition = sqlBaseTool.GetQueryCondition_AlgorithmPL(AlgEnName, AlgCnName, ComponentVersion);

            returnDS = _dbBaseUti.GetList(TableName, TableReturnFields, strQueryCondition, out AllRecordsCount, StartIndex, ResultCount);
            if (returnDS.Tables.Count > 0 && returnDS.Tables[0].Rows.Count > 0)
            {
                InforLog<string>.inforLog.Info("文件:WS_QDB_Searcher_MySQL/App_Code/Service.cs 方法：SearchAlgorithmPL 输出:returnDS不为空");
            }
            else
                InforLog<string>.inforLog.Info("文件:WS_QDB_Searcher_MySQL/App_Code/Service.cs 方法：SearchAlgorithmPL 输出:returnDS为空");
            return returnDS;
        }

        public System.Data.DataSet SearchGFFData(string queryCondition)
        {
            InforLog<string>.inforLog.Info("文件:WS_QDB_Searcher_MySQL/App_Code/Service.cs 方法：SearchGFFData 输入:queryCondition=" + queryCondition);
            try
            {
                //if (mySQLOperator == null)
                //{
                //    mySQLOperator = new DBMySqlOperating();
                //}
                _dbBaseUti = _dbOperator.GetSubDbUtilities(EnumDBType.EVDB);
                //MySqlBaseUti = mySQLOperator.EVDB;
                string sql = "select * from evdb_gff_view ";
                if (!string.IsNullOrEmpty(queryCondition))
                {
                    sql = string.Format(" {0} where {1}", sql, queryCondition);
                }
                if (_dbBaseUti.GetDataSet(sql).Tables.Count > 0 && _dbBaseUti.GetDataSet(sql).Tables[0].Rows.Count > 0)
                {
                    InforLog<string>.inforLog.Info("文件:WS_QDB_Searcher_MySQL/App_Code/Service.cs 方法：SearchGFFData 输出:returnDS不为空");
                }
                else
                    InforLog<string>.inforLog.Info("文件:WS_QDB_Searcher_MySQL/App_Code/Service.cs 方法：SearchGFFData 输出:returnDS为空");
                return _dbBaseUti.GetDataSet(sql);
            }
            catch (Exception ex)
            {
                InforLog<string>.inforLog.Info("文件:WS_QDB_Searcher_MySQL/App_Code/Service.cs 方法：SearchGFFData 输出错误:" + ex.Message);
                return null;
            }
        }

        public System.Data.DataSet SearchCorrectedDataPL(List<string> position, List<string> datetime, List<string> satellite, List<string> sensor, List<string> PixelSpacing, out int AllRecordsCount, int StartIndex, int ResultCount)
        {
            logStr = string.Format("position='{0}' datetime='{1}' satellite='{2}' sensor='{3}' PixelSpacing='{4}' StartIndex='{5}' ResultCount='{6}'",
                InforLog<string>.returnListStrElem(position), InforLog<string>.returnListStrElem(datetime), InforLog<string>.returnListStrElem(satellite), InforLog<string>.returnListStrElem(sensor), InforLog<string>.returnListStrElem(PixelSpacing), StartIndex, ResultCount);
            InforLog<string>.inforLog.Info("文件:WS_QDB_Searcher_MySQL/App_Code/Service.cs 方法：SearchCorrectedDataPL 输入:" + logStr);
            AllRecordsCount = 0;

            #region 组织DateTime参数，从字符串类型转化为DateTime类型
            List<DateTime> listDateTime = new List<DateTime>();
            try
            {
                foreach (string str in datetime)
                {
                    listDateTime.Add(Convert.ToDateTime(str));
                }
            }
            catch
            {
                throw new Exception("输入的时间参数的格式不正确！");
            }
            #endregion
            //if (mySQLOperator == null)
            //{
            //    mySQLOperator = new DBMySqlOperating();
            //}

            _dbBaseUti = _dbOperator.GetSubDbUtilities(EnumDBType.EVDB);

            //要查询的数据对应的表名或者视图名
            string TableName = "prod_hj_view";
            //要返回的字段名
            string returnFields = "*";

            string DnMark = string.Empty;
            List<int> DataSizeRange = new List<int> { };
            List<int> CloudNumRange = new List<int> { };

            //查询条件语句
            string strQueryCondition = sqlBaseTool.GetQueryCondition_CorrectedData(position, listDateTime, satellite, sensor, DnMark, PixelSpacing, DataSizeRange, CloudNumRange);
            //
            returnDS = _dbBaseUti.GetList(TableName, returnFields, strQueryCondition, out AllRecordsCount, StartIndex, ResultCount);
            if (returnDS.Tables.Count > 0 && returnDS.Tables[0].Rows.Count > 0)
            {
                InforLog<string>.inforLog.Info("文件:WS_QDB_Searcher_MySQL/App_Code/Service.cs 方法：SearchCorrectedDataPL 输出:returnDS不为空");
            }
            else
                InforLog<string>.inforLog.Info("文件:WS_QDB_Searcher_MySQL/App_Code/Service.cs 方法：SearchCorrectedDataPL 输出:returnDS为空");
            return returnDS;
        }

        public System.Data.DataSet SearchProductWFLPL(string ProductEnName, string ProductCnName, string ProductLevel, string version, out int AllRecordsCount, int StartIndex, int ResultCount)
        {
            logStr = string.Format("ProductEnName='{0}' ProductCnName='{1}' ProductLevel='{2}' version='{3}' StartIndex='{4}' ResultCount='{5}'", ProductEnName, ProductCnName, ProductLevel, version, StartIndex, ResultCount);
            InforLog<string>.inforLog.Info("文件:WS_QDB_Searcher_MySQL/App_Code/Service.cs 方法：SearchProductWFLPL 输入:" + logStr);
            AllRecordsCount = 0;

            //if (mySQLOperator == null)
            //{
            //    mySQLOperator = new DBMySqlOperating();
            //}
            _dbBaseUti = _dbOperator.GetSubDbUtilities(EnumDBType.MADB);

            //MySqlBaseUti = mySQLOperator.MADB;

            //要查询的数据对应的表名或者视图名
            string TableName = "madb_proworkflow_view";
            //要返回的字段名
            string returnFields = "*";

            //查询条件语句
            string strQueryCondition = sqlBaseTool.GetQueryCondition_ProductWFL(ProductEnName, ProductCnName, ProductLevel, version);
            //
            returnDS = _dbBaseUti.GetList(TableName, returnFields, strQueryCondition, out AllRecordsCount, StartIndex, ResultCount);
            if (returnDS.Tables.Count > 0 && returnDS.Tables[0].Rows.Count > 0)
            {
                InforLog<string>.inforLog.Info("文件:WS_QDB_Searcher_MySQL/App_Code/Service.cs 方法：SearchProductWFLPL 输出:returnDS不为空");
            }
            else
                InforLog<string>.inforLog.Info("文件:WS_QDB_Searcher_MySQL/App_Code/Service.cs 方法：SearchProductWFLPL 输出:returnDS为空");
            return returnDS;
        }




        #endregion



        public string CopyByQrstCodeOrderTask(string code, string sharePath, string mouid)
        {
            List<String> ps = new List<string>();
            ps.Add(code);
            ps.Add(sharePath);
            ps.Add(mouid);
            OrderClass orderClass = OrderManager.CreateInstalledOrder("IODownLoadDOC", ps.ToArray());
            orderClass.Priority = EnumOrderPriority.High;
            OrderManager.SubmitOrder(orderClass);
            return "1";

        }

        public string CopyByQrstCode(string code, string sharePath)
        {
            //导入原始数据和纠正后数据
            string tableCode = StoragePath.GetTableCodeByQrstCode(code);
            StoragePath storePath = new StoragePath(tableCode);
            string destPath = storePath.GetDataPath(code);
            Directory.CreateDirectory(destPath);             //需要判断路径是否存在我改天在该
            //拷贝源文件

            string[] srcFiles = Directory.GetFiles(destPath);
            foreach (string src in srcFiles)
            {
                string srcdestPath = string.Format(@"{0}\{1}", sharePath, Path.GetFileName(src));
                if (!File.Exists(src))
                {
                    return "";
                }

                if (!File.Exists(srcdestPath))
                    // File.Delete(srcdestPath);
                    //File.Move(src, srcdestPath);
                    File.Copy(src, srcdestPath);

            }
            return "";

        }

        public int SubmitDOCDataImportToString(string TITLE, string DOCTYPE, string KEYWORD, string ABSTRACT, string DOCDATE, string DESCRIPTION, string AUTHOR, string UPLOADER, string UPLOADTIME, string FILESIZE, string sharePath)  //, string sharePath
        {
            int q = Convert.ToInt32(FILESIZE);
            //logStr = string.Format("sharePath='{12}'", sharePath);
            //InforLog<string>.inforLog.Info("文件:WS_QDB_SubmitOrder/App_Code/Service.cs 方法：SubmitDOCDataImport 输入:" + logStr);
            List<String> ps = new List<string>();
            ps.Add(TITLE);
            ps.Add(DOCTYPE);
            ps.Add(KEYWORD);
            ps.Add(ABSTRACT);
            ps.Add(DOCDATE.ToString());
            ps.Add(DESCRIPTION);
            ps.Add(AUTHOR);
            ps.Add(UPLOADER);
            ps.Add(UPLOADTIME.ToString());
            ps.Add(FILESIZE.ToString());
            ps.Add(sharePath);
            OrderClass orderClass = OrderManager.CreateInstalledOrder("IODOCDataImport", ps.ToArray());
            OrderManager.AddNewOrder2DB(orderClass);
            return 1;
        }

        public int SubmitDOCDataImport(string TITLE, string DOCTYPE, string KEYWORD, string ABSTRACT, DateTime DOCDATE, string DESCRIPTION, string AUTHOR, string UPLOADER, DateTime UPLOADTIME, int FILESIZE, string sharePath)  //, string sharePath
        {
            //logStr = string.Format("sharePath='{12}'", sharePath);
            //InforLog<string>.inforLog.Info("文件:WS_QDB_SubmitOrder/App_Code/Service.cs 方法：SubmitDOCDataImport 输入:" + logStr);
            List<String> ps = new List<string>();
            ps.Add(TITLE);
            ps.Add(DOCTYPE);
            ps.Add(KEYWORD);
            ps.Add(ABSTRACT);
            ps.Add(DOCDATE.ToString());
            ps.Add(DESCRIPTION);
            ps.Add(AUTHOR);
            ps.Add(UPLOADER);
            ps.Add(UPLOADTIME.ToString());
            ps.Add(FILESIZE.ToString());
            ps.Add(sharePath);
            OrderClass orderClass = OrderManager.CreateInstalledOrder("IODOCDataImport", ps.ToArray());
            OrderManager.AddNewOrder2DB(orderClass);
            return 1;
        }

        public IGeometry getGeomFromRow(DataRow dr)
        {
            int flag = -1;
            foreach (DataColumn dc in dr.Table.Columns)
            {
                if (dc.Caption.Contains("经度") || dc.Caption.Contains("纬度"))
                {
                    flag = 1;
                    break;
                }
                else if (dc.Caption.Contains("数据范围"))
                {
                    flag = 0;
                    break;
                }
            }



            List<Coordinate> coords = new List<Coordinate>();

            if (flag == 1)
            {
                double lulat = Convert.ToDouble(dr["左上纬度"].ToString());
                double lulon = Convert.ToDouble(dr["左上经度"].ToString());
                double rulat = Convert.ToDouble(dr["右上纬度"].ToString());
                double rulon = Convert.ToDouble(dr["右上经度"].ToString());
                double rdlat = Convert.ToDouble(dr["右下纬度"].ToString());
                double rdlon = Convert.ToDouble(dr["右下经度"].ToString());
                double ldlat = Convert.ToDouble(dr["左下纬度"].ToString());
                double ldlon = Convert.ToDouble(dr["左下经度"].ToString());

                coords.Add(new Coordinate(lulon, lulat));
                coords.Add(new Coordinate(rulon, rulat));
                coords.Add(new Coordinate(rdlon, rdlat));
                coords.Add(new Coordinate(ldlon, ldlat));
                coords.Add(new Coordinate(lulon, lulat));

            }
            else if (flag == 0)
            {
                double maxlat = Convert.ToDouble(dr["数据范围上"].ToString());
                double minlat = Convert.ToDouble(dr["数据范围下"].ToString());
                double minlon = Convert.ToDouble(dr["数据范围左"].ToString());
                double maxlon = Convert.ToDouble(dr["数据范围右"].ToString());

                coords.Add(new Coordinate(minlon, maxlat));
                coords.Add(new Coordinate(maxlon, maxlat));
                coords.Add(new Coordinate(maxlon, minlat));
                coords.Add(new Coordinate(minlon, minlat));
                coords.Add(new Coordinate(minlon, maxlat));

            }
            IGeometry poly = new Polygon(coords);
            return poly;

        }

        public System.Data.DataSet GetParasInfoByAlgID(string AlgID, out int AllRecordsCount, int StartIndex, int ResultCount, List<string> strOrderBy, List<int> OrderByType)
        {
            logStr = string.Format("AlgID='{0}' StartIndex='{1}' ResultCount='{2}' strOrderBy='{3}' OrderByType='{4}'", AlgID, StartIndex, ResultCount, InforLog<string>.returnListStrElem(strOrderBy), InforLog<int>.returnListStrElem(OrderByType));
            InforLog<string>.inforLog.Info("文件:WS_QDB_Searcher_MySQL/App_Code/Service.cs 方法：GetParasInfoByAlgID 输入:" + logStr);
            AllRecordsCount = 0;

            #region 组织排序参数及 排序类型（升序，降序）
            //string sqlOrderBy =string.Join(",", strOrderBy);
            string sqlOrderBy = "";
            if (string.Join(" ", strOrderBy).Trim() != "")
            {
                if (strOrderBy.Count == OrderByType.Count)
                {
                    for (int i = 0; i < OrderByType.Count; i++)
                    {
                        switch (OrderByType[i])
                        {
                            case 0:
                                strOrderBy[i] += " ASC";
                                break;
                            case 1:
                                strOrderBy[i] += " DESC";
                                break;
                            default:
                                break;
                        }
                        sqlOrderBy = string.Join(",", strOrderBy);
                    }
                }
                else
                {
                    throw new Exception("输入的排序字段数与输入的排列顺序个数不一致！");

                }
            }
            #endregion
            string TableName = "";
            string TableReturnFields = "";
            string strQueryCondition = "";

            //查询标准纠正后栅格数据
            //MySqlBaseUti = mySQLOperator.MADB;
            _dbBaseUti = _dbOperator.GetSubDbUtilities(EnumDBType.MADB);
            //要查询的数据对应的表名或者视图名
            TableName = "madb_algorithmpara_view";
            //要返回的字段名
            TableReturnFields = "*";

            strQueryCondition = string.Format(" ParaID = '{0}' ", AlgID);
            if (string.IsNullOrEmpty(sqlOrderBy.Trim()))
            {
                returnDS = _dbBaseUti.GetList(TableName, TableReturnFields, strQueryCondition, out AllRecordsCount, StartIndex, ResultCount);
            }
            else
            {
                returnDS = _dbBaseUti.GetList(TableName, TableReturnFields, strQueryCondition, sqlOrderBy, out AllRecordsCount, StartIndex, ResultCount);
            }
            if (returnDS != null && returnDS.Tables.Count > 0 && returnDS.Tables[0].Rows.Count > 0)
            {
                InforLog<string>.inforLog.Info("文件:WS_QDB_Searcher_MySQL/App_Code/Service.cs 方法：GetParasInfoByAlgID 输出:returnDS不为空");
            }
            else
                InforLog<string>.inforLog.Info("文件:WS_QDB_Searcher_MySQL/App_Code/Service.cs 方法：GetParasInfoByAlgID 输出:returnDS为空");
            return returnDS;
        }

        public System.Data.DataSet GetProductInfo()
        {
            _dbBaseUti = _dbOperator.GetSubDbUtilities(EnumDBType.IPDB);
            logDS = _dbBaseUti.GetDataSet(" select * from productinfo");
            if (logDS != null && logDS.Tables.Count > 0 && logDS.Tables[0].Rows.Count > 0)
            {
                InforLog<string>.inforLog.Info("文件:WS_QDB_Searcher_MySQL/App_Code/Service.cs 方法：GetProductInfo 输出:返回DataSet不为空");
            }
            else
                InforLog<string>.inforLog.Info("文件:WS_QDB_Searcher_MySQL/App_Code/Service.cs 方法：GetProductInfo 输出:返回DataSet为空");
            return _dbBaseUti.GetDataSet(" select * from productinfo");
        }

        public System.Data.DataSet GetIndustryInfo()
        {
            _dbBaseUti = _dbOperator.GetSubDbUtilities(EnumDBType.MIDB);
            logDS = _dbBaseUti.GetDataSet(" select * from industryinfo");
            if (logDS != null && logDS.Tables.Count > 0 && logDS.Tables[0].Rows.Count > 0)
            {
                InforLog<string>.inforLog.Info("文件:WS_QDB_Searcher_MySQL/App_Code/Service.cs 方法：GetIndustryInfo 输出:返回DataSet不为空");
            }
            else
                InforLog<string>.inforLog.Info("文件:WS_QDB_Searcher_MySQL/App_Code/Service.cs 方法：GetIndustryInfo 输出:返回DataSet为空");
            return _dbBaseUti.GetDataSet(" select * from industryinfo");
        }

        public System.Data.DataSet SearchCountyByCity(string provinceName, string cityName)
        {
            logStr = string.Format("provinceName='{0}' cityName='{1}'", provinceName, cityName);
            InforLog<string>.inforLog.Info("文件:WS_QDB_Searcher_MySQL/App_Code/Service.cs 方法：SearchCountyByCity 输入:" + logStr);
            try
            {
                //mySQLOperator = new DBMySqlOperating();
                _dbBaseUti = _dbOperator.GetSubDbUtilities(EnumDBType.BSDB);
                logDS = sqlBaseTool.SearchCountyByCity(provinceName, cityName, _dbBaseUti);
            }
            catch (Exception)
            {
                logDS = null;
            }
            if (logDS != null && logDS.Tables.Count > 0 && logDS.Tables[0].Rows.Count > 0)
            {
                InforLog<string>.inforLog.Info("文件:WS_QDB_Searcher_MySQL/App_Code/Service.cs 方法：SearchCountyByCity 输出:返回DataSet不为空");
            }
            else
                InforLog<string>.inforLog.Info("文件:WS_QDB_Searcher_MySQL/App_Code/Service.cs 方法：SearchCountyByCity 输出:返回DataSet为空");
            return logDS;
        }

        public System.Data.DataSet SearchCityByProvince(string provinceName)
        {
            InforLog<string>.inforLog.Info("文件:WS_QDB_Searcher_MySQL/App_Code/Service.cs 方法：SearchCityByProvince 输入:provinceName=" + provinceName);
            try
            {
                _dbBaseUti = _dbOperator.GetSubDbUtilities(EnumDBType.BSDB);
                logDS = sqlBaseTool.SearchCityByProvince(provinceName, _dbBaseUti);
            }
            catch (Exception)
            {
                logDS = null;
            }
            if (logDS != null && logDS.Tables.Count > 0 && logDS.Tables[0].Rows.Count > 0)
            {
                InforLog<string>.inforLog.Info("文件:WS_QDB_Searcher_MySQL/App_Code/Service.cs 方法：SearchCityByProvince 输出:返回DataSet不为空");
            }
            else
                InforLog<string>.inforLog.Info("文件:WS_QDB_Searcher_MySQL/App_Code/Service.cs 方法：SearchCityByProvince 输出:返回DataSet为空");
            return logDS;
        }

        public System.Data.DataSet SearchProvince()
        {

            try
            {
                _dbBaseUti = _dbOperator.GetSubDbUtilities(EnumDBType.BSDB);
                logDS = sqlBaseTool.SearchProvinces(_dbBaseUti);
            }
            catch (Exception)
            {
                logDS = null;
            }
            if (logDS != null && logDS.Tables.Count > 0 && logDS.Tables[0].Rows.Count > 0)
            {
                InforLog<string>.inforLog.Info("文件:WS_QDB_Searcher_MySQL/App_Code/Service.cs 方法：SearchProvince 输出:返回DataSet不为空");
            }
            else
                InforLog<string>.inforLog.Info("文件:WS_QDB_Searcher_MySQL/App_Code/Service.cs 方法：SearchProvince 输出:返回DataSet为空");
            return logDS;
        }

        #region
        //所有格网
        List<string[]> AOITile = null;
        DotSpatial.Data.IFeature feature = null;
        public List<System.Data.DataSet> SerachDataByCoordsStrceshi1(string coordsStr, string tilelevel, int startdate, int enddate, out List<int> allcount)
        {
            List<System.Data.DataSet> listds = new List<System.Data.DataSet>();
            System.Data.DataSet ds = new System.Data.DataSet();
            AOITile = new List<string[]>();
            _dbBaseUti = _dbOperator.GetSubDbUtilities(EnumDBType.EVDB);
            DateTime dtStart = DateTime.Parse(Convert.ToString(startdate));
            DateTime dtEnd = DateTime.Parse(Convert.ToString(enddate));
            //tilelevel==8，则Resolution==25米
            string Resolution = DirectlyAddressing.GetResolutionByStrLv(tilelevel);
            string path = string.Format("~/map/{0}.shp", Resolution);
            //获取多边形最大最小经纬度minlat, minlon, maxlat, maxlon 
            double[] mmll = GridGeneration.GetMinMaxLatLonFormCoordsStr(coordsStr);
            List<Coordinate> coords1 = new List<Coordinate>();
            coords1.Add(new Coordinate(mmll[0], mmll[1]));
            coords1.Add(new Coordinate(mmll[0], mmll[3]));
            coords1.Add(new Coordinate(mmll[2], mmll[3]));
            coords1.Add(new Coordinate(mmll[2], mmll[1]));
            feature = new DotSpatial.Data.Feature(FeatureType.Polygon, coords1);
            GetAOITilesGrid(path, feature, tilelevel);
            int count = 0;
            allcount = null;
            foreach (var item in AOITile)
            {
                double[] minTileLB = DirectlyAddressing.GetLatAndLong(item, tilelevel);
                string selectString = string.Format("`DATAUPPERLEFTLAT` >= '{0}' AND `DATAUPPERLEFTLONG` <= '{1}' AND `DATAUPPERRIGHTLAT` >= '{0}' AND `DATAUPPERRIGHTLONG` >= '{2}' AND `DATALOWERRIGHTLAT` <= '{3}' AND `DATALOWERRIGHTLONG` >= '{2}' AND `DATALOWERLEFTLAT` <= '{3}' AND `DATALOWERLEFTLONG` <= '{1}' AND `StartTime`>='{4}' AND `EndTime`<='{5}'", minTileLB[2], minTileLB[1], minTileLB[3], minTileLB[0], startdate, enddate);
                ds = _dbBaseUti.GetDataSet(selectString);//如果为空 能添加到list里面？
                listds.Add(ds);
                count = ds.Tables[0].Rows.Count;
                allcount.Add(count);
            }

            return listds;

        }

        public void GetAOITilesGrid(string gridPath, DotSpatial.Data.IFeature feature, string tilelevel)
        {
            this.GetAOITilesCR(feature, AOITile, tilelevel);
        }

        private void GetAOITilesCR(DotSpatial.Data.IFeature feature, List<string[]> aoiTile, string tilelevel)
        {
            //DateTime beforDT = System.DateTime.Now;
            //得到外接矩形包含的行列号
            string[] feaEnvelope = new string[] {
                    feature.Envelope.Minimum.Y.ToString(),
                    feature.Envelope.Minimum.X.ToString() ,
                    feature.Envelope.Maximum.Y.ToString(),
                    feature.Envelope.Maximum.X.ToString() };
            int[] colRow = DirectlyAddressing.GetRowAndColum(feaEnvelope, tilelevel);//最小行，最小列，最大行，最大列
            int rownum = colRow[2] - colRow[0] + 1;
            int colnum = colRow[3] - colRow[1] + 1;
            //获得六参数
            double[] GT = new double[6];
            double resolution = double.Parse(DirectlyAddressing.GetDegreeByStrLv(tilelevel));
            GT[1] = GT[5] = resolution;
            string[] minTileRC = { colRow[0].ToString(), colRow[1].ToString() };
            double[] minTileLB = DirectlyAddressing.GetLatAndLong(minTileRC, tilelevel);//最小纬度，最小经度，最大纬度，最大经度
            GT[0] = minTileLB[0];//最小纬度
            GT[3] = minTileLB[1];//最小经度
            GT[2] = GT[4] = 0;

            //判断多边形和瓦片矩形是否相交，相交即为区域瓦片 注意point（x，y）x为精度，y为纬度
            for (int i = 0; i < rownum; i++)
                for (int j = 0; j < colnum; j++)
                {
                    try
                    {
                        List<double[]> Tile4Geolist = TileGeoTrans(i, j, GT);
                        Coordinate coord1 = new Coordinate(Tile4Geolist[0][0], Tile4Geolist[0][1]);
                        Coordinate coord2 = new Coordinate(Tile4Geolist[1][0], Tile4Geolist[1][1]);
                        Envelope enve = new Envelope(coord1, coord2);
                        bool overlap = feature.Intersects(enve);
                        if (overlap)
                        {
                            string[] tile = new string[2];
                            tile[0] = (i + colRow[0]).ToString();
                            tile[1] = (j + colRow[1]).ToString();
                            aoiTile.Add(tile);
                            //envList.Add(enve);
                        }
                    }
                    catch (Exception e)
                    {
                        throw e;
                        break;
                    }
                }
        }

        public System.Data.DataSet SerachDataCountByCoordsStrceshi2(string coordsStr, string tilelevel)
        {
            _dbBaseUti = _dbOperator.GetSubDbUtilities(EnumDBType.EVDB);
            System.Data.DataSet ds1 = new System.Data.DataSet();
            System.Data.DataSet ds2 = new System.Data.DataSet();
            DataTable dt = new DataTable();
            DataColumn col = new DataColumn("row", typeof(int));
            col.MaxLength = 1000;
            dt.Columns.Add(col);
            DataColumn col1 = new DataColumn("col", typeof(int));
            col1.MaxLength = 100;
            dt.Columns.Add(col1);
            DataColumn col2 = new DataColumn("count", typeof(int));//是否要指定长度呢？
            col2.MaxLength = 1000000;
            dt.Columns.Add(col2);
            //获取多边形最大最小经纬度minlat, minlon, maxlat, maxlon 
            double[] mmll = GridGeneration.GetMinMaxLatLonFormCoordsStr(coordsStr);
            string[] lonlat = new string[] { Convert.ToString(mmll[0]), Convert.ToString(mmll[1]), Convert.ToString(mmll[2]), Convert.ToString(mmll[3]) };
            //获取最大最小行列号
            int[] colRow = DirectlyAddressing.GetRowAndColum(lonlat, tilelevel);
            int count = 0;
            int rownum = colRow[2] - colRow[0] + 1;
            int colnum = colRow[3] - colRow[1] + 1;
            for (int i = 0; i < rownum; i++)
            {
                for (int j = 0; j < colnum; j++)
                {
                    string[] tile = new string[2];
                    tile[0] = (i + colRow[0]).ToString();
                    tile[1] = (j + colRow[1]).ToString();
                    DataRow dr = dt.NewRow();
                    dr["row"] = tile[0];
                    dr["col"] = tile[1];
                    dr["count"] = count;
                    dt.Rows.Add(dr);

                }
            }
            ds1.Tables.Add(dt);

            //一行一列得到T1  DATAUPPERLEFTLAT,DATAUPPERLEFTLONG,DATAUPPERRIGHTLAT,DATAUPPERRIGHTLONG,DATALOWERRIGHTLAT,DATALOWERRIGHTLONG,DATALOWERLEFTLAT,DATALOWERLEFTLONG
            string starttime = "20170101";
            string endtime = "20170201";
            string selectString = string.Format("select DATAUPPERLEFTLAT,DATAUPPERLEFTLONG,DATAUPPERRIGHTLAT,DATAUPPERRIGHTLONG,DATALOWERRIGHTLAT,DATALOWERRIGHTLONG,DATALOWERLEFTLAT,DATALOWERLEFTLONG from prod_gf1 where StartTime>='{0}' and EndTime<='{1}'", starttime, endtime);
            //"select 8个点的坐标 from gf where 东哥给的条件  得到T2

            ds2 = _dbBaseUti.GetDataSet(selectString);
            if (ds2 == null || ds2.Tables.Count == 0 || ds2.Tables[0].Rows.Count < 1)
            {
                return null;
            }
            else
            {
                foreach (DataRow dr2 in ds2.Tables[0].Rows)
                {
                    double leftuplat = Convert.ToDouble(dr2[0]);
                    double leftuplon = Convert.ToDouble(dr2[1]);
                    double rightuplat = Convert.ToDouble(dr2[2]);
                    double rightuplon = Convert.ToDouble(dr2[3]);
                    double rightdownlat = Convert.ToDouble(dr2[4]);
                    double rightdownlon = Convert.ToDouble(dr2[5]);
                    double leftdownlat = Convert.ToDouble(dr2[6]);
                    double leftdownlon = Convert.ToDouble(dr2[7]);

                    List<Coordinate> coords2 = new List<Coordinate>();
                    coords2.Add(new Coordinate(leftuplat, leftuplon));
                    coords2.Add(new Coordinate(rightuplat, rightuplon));
                    coords2.Add(new Coordinate(rightdownlat, rightdownlon));
                    coords2.Add(new Coordinate(leftdownlat, leftdownlon));
                    DotSpatial.Data.IFeature feature2 = new DotSpatial.Data.Feature(FeatureType.Polygon, coords2);

                    foreach (DataRow dr1 in ds1.Tables[0].Rows)
                    {
                        string row = dr1["row"].ToString();
                        string coll = dr1["col"].ToString();
                        string[] rowcol = { row, coll };
                        double[] minTileLB = DirectlyAddressing.GetLatAndLong(rowcol, tilelevel);//最小纬度，最小经度，最大纬度，最大经度
                                                                                                 //判断这个格网的经纬度是否在ds2中
                        List<Coordinate> coords1 = new List<Coordinate>();
                        coords1.Add(new Coordinate(minTileLB[0], minTileLB[1]));
                        coords1.Add(new Coordinate(minTileLB[0], minTileLB[3]));
                        coords1.Add(new Coordinate(minTileLB[2], minTileLB[3]));
                        coords1.Add(new Coordinate(minTileLB[2], minTileLB[1]));
                        DotSpatial.Data.IFeature feature1 = new DotSpatial.Data.Feature(FeatureType.Polygon, coords1);

                        bool overlap = feature1.Intersects(feature2);//判断这个格网和原始数据蓝色框框是否相交
                        if (overlap)
                        {
                            int countvlue = Convert.ToInt32(dr1["count"]);
                            countvlue = countvlue + 1;
                            dr1["count"] = countvlue;
                            continue;

                        }
                    }
                }
                for (int i = ds1.Tables[0].Rows.Count - 1; i >= 0; i--)
                {
                    if (Convert.ToInt32(ds1.Tables[0].Rows[i]["count"]) == 0)
                    {
                        ds1.Tables[0].Rows.Remove(ds1.Tables[0].Rows[i]);
                    }
                    else { }

                }
            }
            return ds1;
        }

        public System.Data.DataTable SerachDataCountByCoordsStr(string coordsStr, string tilelevel, string datalist)
        {
            System.Data.DataSet ds1 = new System.Data.DataSet();
            System.Data.DataSet ds2 = new System.Data.DataSet();
            DataTable dt = new DataTable();
            DataColumn col = new DataColumn("row", typeof(string));
            col.MaxLength = 1000;
            dt.Columns.Add(col);
            DataColumn col1 = new DataColumn("col", typeof(string));
            col1.MaxLength = 100;
            dt.Columns.Add(col1);
            DataColumn col2 = new DataColumn("count", typeof(string));//是否要指定长度呢？
            col2.MaxLength = 1000000;
            dt.Columns.Add(col2);
            //获取多边形最大最小经纬度minlat, minlon, maxlat, maxlon 
            double[] mmll = GridGeneration.GetMinMaxLatLonFormCoordsStr(coordsStr);
            string[] lonlat = new string[] { Convert.ToString(mmll[0]), Convert.ToString(mmll[1]), Convert.ToString(mmll[2]), Convert.ToString(mmll[3]) };
            //获取最大最小行列号
            int[] colRow = DirectlyAddressing.GetRowAndColum(lonlat, tilelevel);
            int count = 0;
            int rownum = colRow[2] - colRow[0] + 1;
            int colnum = colRow[3] - colRow[1] + 1;
            for (int i = 0; i < rownum; i++)
            {
                for (int j = 0; j < colnum; j++)
                {
                    string[] tile = new string[2];
                    tile[0] = (i + colRow[0]).ToString();
                    tile[1] = (j + colRow[1]).ToString();
                    DataRow dr = dt.NewRow();
                    dr["row"] = tile[0];
                    dr["col"] = tile[1];
                    dr["count"] = count;
                    dt.Rows.Add(dr);

                }
            }
            ds1.Tables.Add(dt);
            string[] everydata = datalist.Split("#".ToCharArray());
            foreach (string onedata in everydata)
            {
                string[] latlon = onedata.Split(",".ToCharArray());
                List<Coordinate> coords2 = new List<Coordinate>();
                coords2.Add(new Coordinate(Convert.ToDouble(latlon[2]), Convert.ToDouble(latlon[1])));
                coords2.Add(new Coordinate(Convert.ToDouble(latlon[4]), Convert.ToDouble(latlon[3])));
                coords2.Add(new Coordinate(Convert.ToDouble(latlon[6]), Convert.ToDouble(latlon[5])));
                coords2.Add(new Coordinate(Convert.ToDouble(latlon[8]), Convert.ToDouble(latlon[7])));
                DotSpatial.Data.IFeature feature2 = new DotSpatial.Data.Feature(FeatureType.Polygon, coords2);

                foreach (DataRow dr1 in ds1.Tables[0].Rows)
                {
                    string row = dr1["row"].ToString();
                    string coll = dr1["col"].ToString();
                    string[] rowcol = { row, coll };
                    double[] minTileLB = DirectlyAddressing.GetLatAndLong(rowcol, tilelevel);//最小纬度，最小经度，最大纬度，最大经度
                                                                                             //判断这个格网的经纬度是否在ds2中
                    List<Coordinate> coords1 = new List<Coordinate>();
                    coords1.Add(new Coordinate(minTileLB[0], minTileLB[1]));
                    coords1.Add(new Coordinate(minTileLB[0], minTileLB[3]));
                    coords1.Add(new Coordinate(minTileLB[2], minTileLB[3]));
                    coords1.Add(new Coordinate(minTileLB[2], minTileLB[1]));
                    DotSpatial.Data.IFeature feature1 = new DotSpatial.Data.Feature(FeatureType.Polygon, coords1);

                    bool overlap = feature1.Intersects(feature2);//判断这个格网和原始数据蓝色框框是否相交
                    if (overlap)
                    {
                        int countvlue = Convert.ToInt32(dr1["count"]);
                        countvlue = countvlue + 1;
                        dr1["count"] = countvlue;
                        continue;

                    }
                }
            }

            for (int i = ds1.Tables[0].Rows.Count - 1; i >= 0; i--)
            {
                if (Convert.ToInt32(ds1.Tables[0].Rows[i]["count"]) == 0)
                {
                    ds1.Tables[0].Rows.Remove(ds1.Tables[0].Rows[i]);
                }
                else { }

            }
            return ds1.Tables[0];
        }

        private List<double[]> TileGeoTrans(int a, int b, double[] gt)
        {
            List<double[]> tilegeolist = new List<double[]>();
            double[] geoxy1 = new double[2];//存放转换后的地理坐标
            double[] geoxy2 = new double[2];
            double[] geoxy3 = new double[2];
            double[] geoxy4 = new double[2];
            geoxy1[1] = gt[0] + gt[1] * a + gt[2] * b;//纬度y
            geoxy1[0] = gt[3] + gt[4] * a + gt[5] * b;//经度x
            tilegeolist.Add(geoxy1);//左下
                                    //geoxy2[1] =geoxy1[1]+gt[1];
                                    //geoxy2[0] = geoxy1[0];
                                    //tilegeolist.Add(geoxy2);//左上
            geoxy3[1] = geoxy1[1] + gt[1];
            geoxy3[0] = geoxy1[0] + gt[1];
            tilegeolist.Add(geoxy3);//右上
                                    //geoxy4[1] = geoxy1[1];
                                    //geoxy4[0] = geoxy1[0] + gt[1];
                                    //tilegeolist.Add(geoxy4);//右下
            return tilegeolist;
        }
        #endregion
        #region 预处理  
        [WebMethod(Description = "原始数据的查找（公开给预处理）,其中position:最小纬度、最小经度、最大纬度、最大经度组成的坐标点，datetime包括开始时间和结束时间；dataType表示要查找的数据类型，如“GF1、GF2、GF4号卫星用0来表示”“GF3号卫星用1来表示”")]
        public System.Data.DataSet SearchOriginalDataPR(List<string> position, List<string> datetime, List<string> satellite, List<string> sensor, int dataType, int StartIndex, int ResultCount)
        {
            //AllRecordsCount = 0;

            //DBMySqlOperating mySQLOperator = new DBMySqlOperating();
            //配置基本查询信息
            QueryRequest qr = new QueryRequest();
            qr.dataBase = "EVDB";
            qr.elementSet = new string[] { "*" };
            qr.tableCode = "";
            qr.recordSetStartPointSpecified = StartIndex;
            qr.offset = ResultCount;
            switch (dataType)
            {
                case 0:   //高分1号、2号、4号卫星
                    qr.tableCode = "EVDB-32";
                    break;
                case 1:  //高分3号卫星
                    qr.tableCode = "EVDB-39";
                    break;
            }
            _dbBaseUti = _dbOperator.GetSubDbUtilities(EnumDBType.EVDB);
            IGetQuerySchema querySchema = new FieldViewBasedQuerySchema(qr.elementSet, qr.tableCode, _dbBaseUti);
            //设置查询条件
            RasterQueryPara _rasterQueryPara = new RasterQueryPara();
            _rasterQueryPara.dataCode = qr.tableCode;
            if (datetime != null && datetime[0] != null)
            {
                _rasterQueryPara.STARTTIME = datetime[0];
            }
            if (datetime != null && datetime[1] != null)
            {
                _rasterQueryPara.ENDTIME = datetime[1];
            }
            if (position != null && position.Count >= 4)
            {
                _rasterQueryPara.EXTENTDOWN = position[0];
                _rasterQueryPara.EXTENTLEFT = position[1];
                _rasterQueryPara.EXTENTUP = position[2];
                _rasterQueryPara.EXTENTRIGHT = position[3];
            }
            //卫星
            string _satellite = "";
            if (satellite != null)
            {
                for (int i = 0; i < satellite.Count; i++)
                {
                    _satellite = String.Format("{0}{1},", _satellite, satellite[i]);
                }
            }

            if (!string.IsNullOrEmpty(_satellite))
                _rasterQueryPara.SATELLITE = _satellite;

            //传感器
            string _sensor = "";
            if (sensor != null)
            {
                for (int i = 0; i < sensor.Count; i++)
                {
                    _sensor = String.Format("{0}{1},", _sensor, sensor[i]);
                }
            }

            if (!string.IsNullOrEmpty(_sensor))
                _rasterQueryPara.SENSOR = _sensor;
            string _ratio = "";

            qr.complexCondition = _rasterQueryPara.GetSpecificCondition(querySchema);

            ViewBasedQuery query = new ViewBasedQuery(qr, querySchema);
            //AllRecordsCount = query.GetRecordCount();
            QueryResponse queryResponse = query.Query();
            return queryResponse.recordSet;

        }
        #endregion
    }
}
