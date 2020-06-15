using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;
using System.Runtime.Serialization.Formatters;
using QRST_DI_Resources;
using QRST_DI_SS_DBInterfaces.IDBService;
using QRST_DI_TS_Basis.DirectlyAddress;
using QRST_DI_SS_DBInterfaces.IDBEngine;
using QRST_DI_MS_Basis.Log;
using QRST_DI_DS_Metadata.MetaDataCls;
using QRST_DI_TS_Process.Orders;
using QRST_DI_DS_Metadata.Paths;
using System.IO;
using QRST_DI_TS_Process.Orders.InstalledOrders;
using QRST_DI_SS_Basis.MetaData;
using System.Data;
using QRST_DI_TS_Process;

namespace QRST_DI_SS_DBServer.DBService
{
    class QDB_GetDataTCPServer:MarshalByRefObject,IQDB_GetData
    {
        private static TcpServerChannel _chan = null;
        IDbOperating _mySQLOperator;
        string logStr = null;
        IDbBaseUtilities _dbBaseUti;

        public QDB_GetDataTCPServer()
        {
            if (!Constant.ServiceIsConnected)
            {
                Constant.InitializeTcpConnection();
            }
            _mySQLOperator= Constant.IdbOperating;
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
                props["name"] = "GetData_TCP";
                props["typeFilterLevel"] = TypeFilterLevel.Full;
                _chan = new TcpServerChannel(
                    props, serverProvider);
                ChannelServices.RegisterChannel(_chan);

                RemotingConfiguration.RegisterWellKnownServiceType(typeof(QDB_GetDataTCPServer),
                    "QDB_GetData_TCP",
                    WellKnownObjectMode.SingleCall);
            }
            catch (Exception e)
            {
                throw new Exception("注册QDB_GetDataTCPServer异常", e);
            }
        }

        public List<string> GetTilesList(string[] tileNames)
        {
            DirectlyAddressing da = new DirectlyAddressing(DirectlyAddressingIPMod.IPModDataSet);
            List<string> tilesPath = new List<string>();
            foreach (string tilename in tileNames)
            {
                string tilepath = da.GetPathByFileName(tilename);
                tilesPath.Add(tilepath);
            }
            return tilesPath;
        }

        public string GetData(string DataType, string QRST_CODE)
        {
            logStr = string.Format("DataType='{0}' QRST_CODE='{1}'", DataType, QRST_CODE);
            InforLog<string>.inforLog.Info("文件:WS_QDB_GetData/App_Code/Service.cs 方法：GetData 输入:" + logStr);
            //如果是HJCorrectedData，则
            if (DataType == "AlgorithmCmp" || DataType == "ProductWFL" || DataType == "HJCorrectedData")
            {
                InforLog<string>.inforLog.Info("文件:WS_QDB_GetData/App_Code/Service.cs 方法：GetData 输出:" + MetaData.GetDataAddress(QRST_CODE));
                return MetaData.GetDataAddress(QRST_CODE);
            }
            else
            {
                InforLog<string>.inforLog.Info("文件:WS_QDB_GetData/App_Code/Service.cs 方法：GetData 输出:" + "-1");
                return "-1";
            }
        }
        public int DeleteData(string datacode)
        {
            InforLog<string>.inforLog.Info("文件:WS_QDB_GetData/App_Code/Service.cs 方法：DeleteData 输入:datacode=" + datacode);
            InforLog<string>.inforLog.Info("文件:WS_QDB_GetData/App_Code/Service.cs 方法：DeleteData 输出:" + MetaData.DeleteData(datacode));
            return MetaData.DeleteData(datacode);
        }

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
                    File.Copy(src, srcdestPath);

            }
            return "";

        }
        public string PushTileZipToDirByName(string tileName, string originalOrderCode)
        {
            logStr = string.Format("tileName='{0}' originalOrderCode='{1}'", tileName, originalOrderCode);
            InforLog<string>.inforLog.Info("文件:WS_QDB_GetData/App_Code/Service.cs 方法：PushTileZipToDirByName 输入:" + logStr);
            //创建并提交订单
            OrderClass orderClass = new IOPushTileFiles().Create(tileName, originalOrderCode);
            orderClass.Priority = EnumOrderPriority.High;
            OrderManager.SubmitOrder(orderClass);
            return "";
        }
        public string GetSourceDataPath(string DataCode)
        {
            logStr = string.Format("DataCode='{0}'", DataCode);
            InforLog<string>.inforLog.Info("文件:WS_QDB_GetData/App_Code/Service.cs 方法：GetSourceDataPath 输入:" + logStr);
            string tableCode = StoragePath.GetTableCodeByQrstCode(DataCode);
            StoragePath storePath = new StoragePath(tableCode);
            InforLog<string>.inforLog.Info("文件:WS_QDB_GetData/App_Code/Service.cs 方法：GetSourceDataPath 输出:" + storePath.GetDataPath(DataCode));
            return storePath.GetDataPath(DataCode);
        }
        public List<string> GetTilesList(List<string> tileNames)
        {
            logStr = string.Format("tileNames='{0}'", InforLog<string>.returnListStrElem(tileNames));
            InforLog<string>.inforLog.Info("文件:WS_QDB_GetData/App_Code/Service.cs 方法：GetTilesList 输入:" + logStr);
            DirectlyAddressing da = new DirectlyAddressing(DirectlyAddressingIPMod.IPModDataSet);
            List<string> tilesPath = new List<string>();
            foreach (string tilename in tileNames)
            {
                string tilepath = da.GetPathByFileName(tilename);
                tilesPath.Add(tilepath);
            }
            logStr = string.Format("tilesPath='{0}'", InforLog<string>.returnListStrElem(tilesPath));
            InforLog<string>.inforLog.Info("文件:WS_QDB_GetData/App_Code/Service.cs 方法：GetTilesList 输出:" + logStr);
            return tilesPath;
        }
        public string GetCorrectedDataAddress(string DataName)
        {
            InforLog<string>.inforLog.Info("文件:WS_QDB_GetData/App_Code/Service.cs 方法：GetCorrectedDataAddress 输入:DataName=" + DataName);
            //根据数据库名称获取库访问实例
            _dbBaseUti = _mySQLOperator.GetSubDbUtilities(EnumDBType.EVDB);
            //DataName = DataName.ToUpper();
            //DataName.TrimEnd(".TAR.GZ".ToCharArray());
            string ralativePath = GetRalativeAddByDataCode("EVDB-16");

            string[] strArr = GetAddressFields("EVDB-16");

            string sql = string.Format("select {0} from prod_hj_view where 数据名称 ='{1}'", string.Join(",", strArr), DataName);

            DataTable dt = _dbBaseUti.GetDataSet(sql).Tables[0];

            string AbsolutePath = string.Empty;
            if (dt.Rows.Count == 0)
            {
                return "-1";

            }
            else
            {
                List<string> subDictionaryValue = new List<string>();
                subDictionaryValue.Add(dt.Rows[0]["卫星"].ToString());
                subDictionaryValue.Add(dt.Rows[0]["传感器"].ToString());

                DateTime date = Convert.ToDateTime(dt.Rows[0]["日期"]);
                subDictionaryValue.Add(string.Format("{0:0000}", date.Year));
                subDictionaryValue.Add(string.Format("{0:00}", date.Month));
                subDictionaryValue.Add(string.Format("{0:00}", date.Day));

                subDictionaryValue.Add(dt.Rows[0]["数据名称"].ToString().ToUpper().TrimEnd(".TAR.GZ".ToCharArray()));

                AbsolutePath = string.Join(@"\", subDictionaryValue);
                InforLog<string>.inforLog.Info("文件:WS_QDB_GetData/App_Code/Service.cs 方法：GetCorrectedDataAddress 输出:ralativePath + AbsolutePath=" + ralativePath + AbsolutePath);
                return ralativePath + AbsolutePath;
            }
        }
        public string[] GetCorrectedDataAddressFields()
        {
            StoragePath storagePath = new StoragePath("EVDB-16");
            logStr = InforLog<string>.returnSArrStrElem(storagePath.DataStorePath());
            InforLog<string>.inforLog.Info("文件:WS_QDB_GetData/App_Code/Service.cs 方法：GetCorrectedDataAddressFields 输出:storagePath.DataStorePath()=" + logStr);
            return storagePath.DataStorePath();
        }
        public string[] GetAddressFields(string dataCode)
        {
            InforLog<string>.inforLog.Info("文件:WS_QDB_GetData/App_Code/Service.cs 方法：GetAddressFields 输入:dataCode=" + dataCode);
            StoragePath storagePath = new StoragePath(dataCode);
            logStr = InforLog<string>.returnSArrStrElem(storagePath.DataStorePath());
            InforLog<string>.inforLog.Info("文件:WS_QDB_GetData/App_Code/Service.cs 方法：GetAddressFields 输出:storagePath.DataStorePath()=" + logStr);
            return storagePath.DataStorePath();
        }
        public string GetRalativeAddByDataCode(string dataCode)
        {
            InforLog<string>.inforLog.Info("文件:WS_QDB_GetData/App_Code/Service.cs 方法：GetRalativeAddByDataCode 输入:dataCode=" + dataCode);
            StoragePath storagePath = new StoragePath(dataCode);

            string RalativeFilePath = storagePath.getRalativeAddress();
            InforLog<string>.inforLog.Info("文件:WS_QDB_GetData/App_Code/Service.cs 方法：GetRalativeAddByDataCode 输出:RalativeFilePath=" + RalativeFilePath);
            return RalativeFilePath;
        }
    
    }
}
