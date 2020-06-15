using System.Collections;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;
using System.Runtime.Serialization.Formatters;
using QRST_DI_Resources;
using System;
using System.Collections.Generic;
using QRST_DI_MS_Basis.Log;
using QRST_DI_TS_Process.Site;
using QRST_DI_TS_Process.Orders;
using System.IO;
using QRST_DI_TS_Process;
using QRST_DI_TS_Process.Orders.InstalledOrders;
using QRST_DI_TS_Basis.DirectlyAddress;
using System.Data;
using QRST_DI_SS_DBInterfaces.IDBService;

namespace QRST_DI_SS_DBServer.DBService
{
    class QDB_SubmitOrderTCPServer: MarshalByRefObject,IQDB_SubmitOrder
    {
        private static TcpServerChannel _chan = null;
        private string logStr = "";
        private int lonInt = 0;
        public QDB_SubmitOrderTCPServer()
        {
            if (!Constant.ServiceIsConnected)
            {
                Constant.InitializeTcpConnection();
            }
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
                props["name"] = "SubmitOrder_TCP";
                props["typeFilterLevel"] = TypeFilterLevel.Full;
                _chan = new TcpServerChannel(
                    props, serverProvider);
                ChannelServices.RegisterChannel(_chan);

                RemotingConfiguration.RegisterWellKnownServiceType(typeof(QDB_SubmitOrderTCPServer),
                    "QDB_SubmitOrder_TCP",
                    WellKnownObjectMode.SingleCall);
            }
            catch (Exception e)
            {
                throw new Exception("注册QDB_SubmitOrderTCPServer异常", e);
            }
        }

        public string GetNewWorkSpace(string dataType)
        {
            logStr = string.Format("dataType='{0}'", dataType);
            InforLog<string>.inforLog.Info("文件:WS_QDB_SubmitOrder/App_Code/Service.cs 方法：GetNewWorkSpace 输入:" + logStr);
            string sharePath = "-1";
            TServerSiteManager.UpdateOptimalStorageSiteList();
            if (TServerSiteManager.optimalStorageSites.Count > 0)
            {
                string storeIP = TServerSiteManager.optimalStorageSites[0].IPAdress;
                string code = OrderManager.GetNewCode();
                sharePath = OrderManager.BuildWorkSpaceByOrderCode(storeIP, code, dataType);
            }
            InforLog<string>.inforLog.Info("文件:WS_QDB_SubmitOrder/App_Code/Service.cs 方法：GetNewWorkSpace 输出:sharePath=" + sharePath);
            return sharePath;
        }

        public int SubmitHJDataImport(string sharePath)
        {
            logStr = string.Format("sharePath='{0}'", sharePath);
            InforLog<string>.inforLog.Info("文件:WS_QDB_SubmitOrder/App_Code/Service.cs 方法：SubmitHJDataImport 输入:" + logStr);
            //检核文件是否存在
            if (!Directory.Exists(sharePath))
            {
                InforLog<string>.inforLog.Info("文件:WS_QDB_SubmitOrder/App_Code/Service.cs 方法：SubmitHJDataImport 输出:" + "-2");
                return -2;                    //共享的工作空间不存在
            }
            string[] files = Directory.GetFiles(sharePath, "*.tar.gz");
            if (files.Length == 0)
            {
                InforLog<string>.inforLog.Info("文件:WS_QDB_SubmitOrder/App_Code/Service.cs 方法：SubmitHJDataImport 输出:" + "-1");
                return -1;                 //文件未找到
            }
            try
            {
                List<OrderClass> orderclassLst = OrderManager.CreateInstalledBatchOrder("IONormalHJBatchImport", new string[] { sharePath });
                foreach (OrderClass orderclass in orderclassLst)
                {
                    orderclass.Priority = EnumOrderPriority.High;
                    OrderManager.AddNewOrder2DB(orderclass);
                }
                InforLog<string>.inforLog.Info("文件:WS_QDB_SubmitOrder/App_Code/Service.cs 方法：SubmitHJDataImport 输出:" + "1");
                return 1;   //成功触发入库
            }
            catch (Exception)
            {
                InforLog<string>.inforLog.Info("文件:WS_QDB_SubmitOrder/App_Code/Service.cs 方法：SubmitHJDataImport 输出:" + "0");
                return 0;                      //未知原因
            }
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

        public int SubmitDataImport(string datatype, string sharePath)
        {
            logStr = string.Format("datatype='{0}' sharePath='{1}'", datatype, sharePath);
            InforLog<string>.inforLog.Info("文件:WS_QDB_SubmitOrder/App_Code/Service.cs 方法：SubmitDataImport 输入:" + logStr);
            //检核文件是否存在
            if (!Directory.Exists(sharePath))
            {
                InforLog<string>.inforLog.Info("文件:WS_QDB_SubmitOrder/App_Code/Service.cs 方法：SubmitDataImport 输出:" + "-2");
                return -2;                    //共享的工作空间不存在
            }

            try
            {
                //TSPCommonReference.Create();
                if (datatype.ToLower() == "modis")
                {
                    string[] files = Directory.GetFiles(sharePath, "*.hdf");
                    if (files.Length == 0)
                    {
                        InforLog<string>.inforLog.Info("文件:WS_QDB_SubmitOrder/App_Code/Service.cs 方法：SubmitDataImport 输出:" + "-1");
                        return -1;                 //文件未找到
                    }
                    else
                    {
                        List<OrderClass> orderLst = new IORasterDataImportNew().CreateRasterDataBatchImport(datatype, files);
                        foreach (OrderClass orderclass in orderLst)
                        {
                            orderclass.Priority = EnumOrderPriority.High;
                            OrderManager.AddNewOrder2DB(orderclass);
                        }
                        InforLog<string>.inforLog.Info("文件:WS_QDB_SubmitOrder/App_Code/Service.cs 方法：SubmitDataImport 输出:" + "1");
                        return 1;
                    }
                }
                else if (datatype.ToLower() == "noaa")
                {
                    List<OrderClass> orderLst = new IONoaaBatchImport().CreateNoaaBatchImport(sharePath);
                    foreach (OrderClass orderclass in orderLst)
                    {
                        orderclass.Priority = EnumOrderPriority.High;
                        OrderManager.AddNewOrder2DB(orderclass);
                    }
                    InforLog<string>.inforLog.Info("文件:WS_QDB_SubmitOrder/App_Code/Service.cs 方法：SubmitDataImport 输出:" + "1");
                    return 1;
                }
                else
                {
                    InforLog<string>.inforLog.Info("文件:WS_QDB_SubmitOrder/App_Code/Service.cs 方法：SubmitDataImport 输出:" + "-3");
                    return -3;
                }

            }
            catch (Exception)
            {
                InforLog<string>.inforLog.Info("文件:WS_QDB_SubmitOrder/App_Code/Service.cs 方法：SubmitDataImport 输出:" + "0");
                return 0;                      //未知原因
            }
        }

        public int SubmitImportOrder(string dataType, string sharePath)
        {
            logStr = string.Format("dataType='{0}' sharePath='{1}'", dataType, sharePath);
            InforLog<string>.inforLog.Info("文件:WS_QDB_SubmitOrder/App_Code/Service.cs 方法：SubmitImportOrder 输入:" + logStr);
            //检核文件是否存在
            if (!Directory.Exists(sharePath))
            {
                InforLog<string>.inforLog.Info("文件:WS_QDB_SubmitOrder/App_Code/Service.cs 方法：SubmitImportOrder 输出:" + "-2");
                return -2;                    //共享的工作空间不存在
            }

            try
            {
                //TSPCommonReference.Create();
                switch (dataType)
                {
                    case "HJCorrectedData":
                        {
                            string[] files = Directory.GetFiles(sharePath, "*.tar.gz");
                            if (files.Length == 0)
                            {
                                InforLog<string>.inforLog.Info("文件:WS_QDB_SubmitOrder/App_Code/Service.cs 方法：SubmitImportOrder 输出:" + "-1");
                                return -1;                 //文件未找到
                            }
                            List<OrderClass> orderclassLst = OrderManager.CreateInstalledBatchOrder("IONormalHJBatchImport", new string[] { sharePath });
                            foreach (OrderClass orderclass in orderclassLst)
                            {
                                orderclass.Priority = EnumOrderPriority.High;
                                OrderManager.AddNewOrder2DB(orderclass);
                            }
                            InforLog<string>.inforLog.Info("文件:WS_QDB_SubmitOrder/App_Code/Service.cs 方法：SubmitImportOrder 输出:" + "1");
                            return 1;   //成功触发入库
                        }
                    case "AlgorithmCmp":
                        {
                            string[] files = Directory.GetFiles(sharePath, "*.tar");
                            if (files.Length == 0)
                            {
                                InforLog<string>.inforLog.Info("文件:WS_QDB_SubmitOrder/App_Code/Service.cs 方法：SubmitImportOrder 输出:" + "-1");
                                return -1;
                            }
                            OrderClass orderclass = new IOImportStandCmp().CreateStandCmpImport(sharePath);
                            orderclass.Priority = EnumOrderPriority.High;
                            OrderManager.AddNewOrder2DB(orderclass);
                            InforLog<string>.inforLog.Info("文件:WS_QDB_SubmitOrder/App_Code/Service.cs 方法：SubmitImportOrder 输出:" + "1");
                            return 1;
                        }
                    case "ProductWFL":
                        {
                            string[] files = Directory.GetFiles(sharePath, "*.tar");
                            if (files.Length == 0)
                            {
                                InforLog<string>.inforLog.Info("文件:WS_QDB_SubmitOrder/App_Code/Service.cs 方法：SubmitImportOrder 输出:" + "-1");
                                return -1;
                            }
                            OrderClass orderclass = new IOImportStandWfl().CreateStandWflImport(sharePath);
                            orderclass.Priority = EnumOrderPriority.High;
                            OrderManager.AddNewOrder2DB(orderclass);
                            InforLog<string>.inforLog.Info("文件:WS_QDB_SubmitOrder/App_Code/Service.cs 方法：SubmitImportOrder 输出:" + "1");
                            return 1;
                        }
                    case "UserProduct":
                        {
                            OrderClass orderclass = new IOImportUserProduct().CreateUserProductImport(sharePath);
                            orderclass.Priority = EnumOrderPriority.High;
                            OrderManager.AddNewOrder2DB(orderclass);
                            InforLog<string>.inforLog.Info("文件:WS_QDB_SubmitOrder/App_Code/Service.cs 方法：SubmitImportOrder 输出:" + "1");
                            return 1;
                        }
                    default:
                        InforLog<string>.inforLog.Info("文件:WS_QDB_SubmitOrder/App_Code/Service.cs 方法：SubmitImportOrder 输出:" + "-3");
                        return -3;     //没有对应的数据入库类型
                }
            }
            catch (Exception)
            {
                InforLog<string>.inforLog.Info("文件:WS_QDB_SubmitOrder/App_Code/Service.cs 方法：SubmitImportOrder 输出:0");
                return 0;                      //未知原因
            }
        }

        public int UploadData(string dataType, string pathID, string opID)
        {
            logStr = string.Format("dataType='{0}' pathID='{1}' opID='{2}'", dataType, pathID, opID);
            InforLog<string>.inforLog.Info("文件:WS_QDB_SubmitOrder/App_Code/Service.cs 方法：UploadData 输入:" + logStr);
            try
            {
                OrderClass orderclass = new IOUserDataImport().CreateUserDataImport(dataType, pathID, opID);
                orderclass.Priority = EnumOrderPriority.High;
                OrderManager.AddNewOrder2DB(orderclass);
                InforLog<string>.inforLog.Info("文件:WS_QDB_SubmitOrder/App_Code/Service.cs 方法：UploadData 输出:" + "1");
                return 1;
            }
            catch (Exception)
            {
                InforLog<string>.inforLog.Info("文件:WS_QDB_SubmitOrder/App_Code/Service.cs 方法：UploadData 输出:" + "0");
                return 0;
            }
        }

        public int DownLoad_P2P(string dataName, string dataSharePath, string opID, string webserviceIp)
        {
            logStr = string.Format("dataName='{0}' dataSharePath='{1}' opID='{2}'", dataName, dataSharePath, opID);
            InforLog<string>.inforLog.Info("文件:WS_QDB_SubmitOrder/App_Code/Service.cs 方法：DownLoad_P2P 输入:" + logStr);
            try
            {
                OrderClass orderclass = new IODownLoadByP2P().CreateIODownLoadByP2P(dataName, dataSharePath, opID, webserviceIp);
                orderclass.Priority = EnumOrderPriority.High;
                OrderManager.AddNewOrder2DB(orderclass);
                InforLog<string>.inforLog.Info("文件:WS_QDB_SubmitOrder/App_Code/Service.cs 方法：DownLoad 输出:" + "1");
                return 1;
            }
            catch (Exception)
            {
                InforLog<string>.inforLog.Info("文件:WS_QDB_SubmitOrder/App_Code/Service.cs 方法：DownLoad 输出:" + "0");
                return 0;
            }

        }

        public string SubmitInstalledOrder(string orderName, string[] orderParas)
        {
            logStr = string.Format("orderName='{0}' orderParas='{1}'", orderName, InforLog<string>.returnSArrStrElem(orderParas));
            InforLog<string>.inforLog.Info("文件:WS_QDB_SubmitOrder/App_Code/Service.cs 方法：SubmitInstalledOrder 输入:" + logStr);
            //王栋工作流 任务驱动入库 joki 131127
            OrderClass neworder = OrderManager.CreateInstalledOrder(orderName, orderParas);
            neworder.Priority = EnumOrderPriority.High;
            OrderManager.AddNewOrder2DB(neworder);
            InforLog<string>.inforLog.Info("文件:WS_QDB_SubmitOrder/App_Code/Service.cs 方法：SubmitInstalledOrder 输出: neworder.OrderCode=" + neworder.OrderCode);
            return neworder.OrderCode;
        }

        public List<string> SubmitTilesInstalledOrder(string orderName, string[] orderParas)
        {
            logStr = string.Format("orderName='{0}' orderParas='{1}'", orderName, InforLog<string>.returnSArrStrElem(orderParas));
            InforLog<string>.inforLog.Info("文件:WS_QDB_SubmitOrder/App_Code/Service.cs 方法：SubmitTilesInstalledOrder 输入:" + logStr);
            //谢毅工作流 任务驱动入库 ksk 131127
            List<string> orderCode = new List<string>();
            string[] tiles = orderParas[0].Split('#');
            DirectlyAddressing da = new DirectlyAddressing(DirectlyAddressingIPMod.IPModDataSet);
            Dictionary<string, string> tileInfo = new Dictionary<string, string>();
            List<TileInfo> tilesInfo = new List<TileInfo>();
            string failTiles = "";
            foreach (var item in tiles)
            {
                bool Contain = false;
                string ip = "-1";
                string desPath = da.GetPathByFileName(item + ".jpg", out ip);
                if (ip != "-1")
                {
                    foreach (var info in tilesInfo)
                    {
                        if (info.IPaddress == ip)
                        {
                            info.CreateTileList(item);
                            Contain = true;
                            break;
                        }
                    }
                    if (Contain)
                        continue;
                    TileInfo tile = new TileInfo();
                    tile.IPaddress = ip;
                    tile.CreateTileList(item);
                    tilesInfo.Add(tile);
                    //if (!tileInfo.ContainsKey(ip))
                    //{
                    //    tileInfo.Add(ip, item);
                    //}
                    //else
                    //{
                    //    string strTemp = tileInfo[ip];
                    //    strTemp += "#" + item;
                    //    tileInfo[ip] = strTemp;
                    //}
                    //tileDesPath tile = new tileDesPath();
                    //tile.Ip = ip;
                    //tile.Despath = desPath;
                }
                else
                {
                    failTiles += item + "#";
                    //tilefileNames.Add(failedpath);
                }
            }
            foreach (var item in tilesInfo)
            {
                foreach (var tileNameStr in item.tilesNameList)
                {
                    string[] orderParasTemp = { tileNameStr, orderParas[1], orderParas[2], item.IPaddress };
                    OrderClass neworder = OrderManager.CreateInstalledOrder(orderName, orderParasTemp);
                    neworder.Priority = EnumOrderPriority.High;
                    OrderManager.AddNewOrder2DB(neworder);
                    orderCode.Add(neworder.OrderCode);
                }
            }
            //foreach (var item in tileInfo)
            //{
            //    string[] orderParasTemp = { item.Value, orderParas[1], orderParas[2], item.Key };
            //    OrderClass neworder = OrderManager.CreateInstalledOrder(orderName, orderParasTemp);
            //    OrderManager.AddNewOrder2DB(neworder);
            //    orderCode.Add(neworder.OrderCode);
            //}
            failTiles = failTiles.TrimEnd('#');
            if (failTiles != "")
            {
                string[] orderParasTemp = { failTiles, orderParas[1], orderParas[2], TServerSiteManager.GetCenterSiteIP() };
                OrderClass neworder = OrderManager.CreateInstalledOrder(orderName, orderParasTemp);
                neworder.Priority = EnumOrderPriority.High;
                OrderManager.AddNewOrder2DB(neworder);
                orderCode.Add(neworder.OrderCode);
            }
            logStr = string.Format("orderCode='{0}'", InforLog<string>.returnListStrElem(orderCode));
            InforLog<string>.inforLog.Info("文件:WS_QDB_SubmitOrder/App_Code/Service.cs 方法：SubmitTilesInstalledOrder 输出:" + logStr);
            return orderCode;
            //OrderClass neworder = OrderManager.CreateInstalledOrder(orderName, orderParas);
            //OrderManager.AddNewOrder2DB(neworder);
            //return neworder.OrderCode;
        }

        public int DownLoad(string dataID, string pathID, string opID, string GFdataName, string webservice)
        {

            logStr = string.Format("dataID='{0}' pathID='{1}' opID='{2}'", dataID, pathID, opID);
            InforLog<string>.inforLog.Info("文件:WS_QDB_SubmitOrder/App_Code/Service.cs 方法：DownLoad 输入:" + logStr);
            try
            {
                OrderClass orderclass = new IODownLoadUserData().CreateDownLoadUserData(dataID, pathID, opID, GFdataName, webservice);
                orderclass.Priority = EnumOrderPriority.High;
                OrderManager.AddNewOrder2DB(orderclass);
                InforLog<string>.inforLog.Info("文件:WS_QDB_SubmitOrder/App_Code/Service.cs 方法：DownLoad 输出:" + "1");
                return 1;
            }
            catch (Exception)
            {
                InforLog<string>.inforLog.Info("文件:WS_QDB_SubmitOrder/App_Code/Service.cs 方法：DownLoad 输出:" + "0");
                return 0;
            }
        }


        public int DownLoad(string dataID, string pathID, string opID)
        {
            return DownLoad(dataID, pathID, opID, "","");
        }

        public string CreateWorkSpaceForPreProcess()
        {
            string orderPath = "-1";
            OrderClass orderClass = new IOGF1dataPrepare().CreateGF1dataPrepareOrder();
            orderClass.Priority = EnumOrderPriority.High;
            OrderManager.AddNewOrder2DB(orderClass);
            //循环监控订单是否被执行
            string condition = string.Format("s.OrderCode = '{0}' and s.Status = 'Suspended'", orderClass.OrderCode);
            DateTime dtNow = DateTime.Now;
            DateTime dtSpan = DateTime.Now;
            while ((dtSpan - dtNow).TotalMilliseconds < 10000) //三秒内没有创建好，则创建工作空间失败
            {
                DataTable orderLst = OrderManager.GetOrderList(condition);
                if (orderLst.Rows.Count > 0)
                {
                    //  orderClass = OrderManager.DBRow2OrderCls(orderLst.Rows[0]);
                    string addressip = orderLst.Rows[0]["addressip"].ToString();
                    //找到工作空间位置，例如 \\127.0.0.1\QRST_DB_Share\P1475369832501
                    orderPath = string.Format(@"{1}:\\{0}\QRST_DB_Share\{1}", addressip, orderClass.OrderCode);
                    return orderPath;
                }
                dtSpan = DateTime.Now;
            }
            InforLog<string>.inforLog.Info("文件:WS_QDB_SubmitOrder/App_Code/Service.cs 方法：CreateWorkSpaceForPreProcess 输出:orderPath=" + orderPath);
            //超时 
            return orderPath;
        }

        public string ApplyNewGF1DataImportOrder()
        {
            string orderPath = "-1";
            OrderClass orderClass = new IOGF1DataImport().Create();
            orderClass.Priority = EnumOrderPriority.High;
            OrderManager.SubmitOrder(orderClass);
            InforLog<string>.inforLog.Info("文件:WS_QDB_SubmitOrder/App_Code/Service.cs 方法：ApplyNewGF1DataImportOrder 输出:orderClass.OrderCode=" + orderClass.OrderCode);
            return orderClass.OrderCode;
        }

        public string GetOrderWorkspace(string ordercode)
        {
            logStr = string.Format("ordercode='{0}'", ordercode);
            InforLog<string>.inforLog.Info("文件:WS_QDB_SubmitOrder/App_Code/Service.cs 方法：GetOrderWorkspace 输入:" + logStr);
            ////循环监控订单是否被执行
            OrderClass order = OrderManager.GetOrderByCode(ordercode);
            logStr = (order == null) ? "-1" : (order.OrderWorkspace);
            InforLog<string>.inforLog.Info("文件:WS_QDB_SubmitOrder/App_Code/Service.cs 方法：GetOrderWorkspace 输出:" + logStr);
            return (order == null) ? "-1" : (order.OrderWorkspace);
        }

        public bool ResumeGF1DataImportOrder(string ordercode)
        {
            ////循环监控订单是否被执行
            logStr = string.Format("ordercode='{0}'", ordercode);
            InforLog<string>.inforLog.Info("文件:WS_QDB_SubmitOrder/App_Code/Service.cs 方法：ResumeGF1DataImportOrder 输入:" + logStr);
            OrderClass order = OrderManager.GetOrderByCode(ordercode);
            if (order != null && order.Status == EnumOrderStatusType.Suspended)
            {
                OrderManager.UpdateOrderStatus(ordercode, EnumOrderStatusType.Waiting);
                InforLog<string>.inforLog.Info("文件:WS_QDB_SubmitOrder/App_Code/Service.cs 方法：ResumeGF1DataImportOrder 输出:True");
                return true;
            }
            InforLog<string>.inforLog.Info("文件:WS_QDB_SubmitOrder/App_Code/Service.cs 方法：ResumeGF1DataImportOrder 输出:False");
            return false;
        }

        public void SetOrderStatus2Waiting(string orderCode)
        {
            logStr = string.Format("orderCode='{0}'", orderCode);
            InforLog<string>.inforLog.Info("文件:WS_QDB_SubmitOrder/App_Code/Service.cs 方法：SetOrderStatus2Waiting 输入:" + logStr);
            OrderManager.UpdateOrderStatus(orderCode, EnumOrderStatusType.Waiting);
        }

        public string GetOrderStatus(string ordercode)
        {
            string s = "";
            logStr = string.Format("ordercode='{0}'", ordercode);
            InforLog<string>.inforLog.Info("文件:WS_QDB_SubmitOrder/App_Code/Service.cs 方法：GetOrderStatus 输入:" + logStr);
            OrderClass order = OrderManager.GetOrderByCode(ordercode);
            if (order != null)
            {
                s = order.Status.ToString();
            }
            else
                s = "Missing";
            InforLog<string>.inforLog.Info("文件:WS_QDB_SubmitOrder/App_Code/Service.cs 方法：GetOrderStatus 输出:" + s);
            return s;
        }

        private string GetFailedTilePath()
        {
            string s = "";
            string CenterIP = TServerSiteManager.GetCenterSiteIP();
            string pattern = @"^(0|[1-9][0-9]?|1[0-9]{2}|2[0-4][0-9]|25[0-5])\.(0|[1-9][0-9]?|1[0-9]{2}|2[0-4][0-9]|25[0-5])\.(0|[1-9][0-9]?|1[0-9]{2}|2[0-4][0-9]|25[0-5])\.(0|[1-9][0-9]?|1[0-9]{2}|2[0-4][0-9]|25[0-5])$";

            if (System.Text.RegularExpressions.Regex.IsMatch(CenterIP, pattern))
            {
                s = string.Format(@"\\{0}\{1}\{2}\", CenterIP, StorageBasePath.QRST_DB_Tile, StorageBasePath.FailedTile);
            }
            else
            {
                s = "";
            }
            InforLog<string>.inforLog.Info("文件:WS_QDB_SubmitOrder/App_Code/Service.cs 方法：GetFailedTilePath 输出:" + s);
            return s;

        }
    
    
    
    }
    public class TileInfo
    {
        public string IPaddress
        {
            get;
            set;
        }
        public List<string> tilesNameList = new List<string>();
        private int tilesNum = 0;
        string tempName = "";
        public void CreateTileList(string tileName)
        {
            int index = tilesNum / 100;
            if (tilesNameList.Count < (index + 1))
                tilesNameList.Add(tileName);
            else
            {
                tilesNameList[index] += "#" + tileName;
            }
            tilesNum++;
        }
    }
}
