using System;
using System.Collections.Generic;
using System.Data;
using QRST_DI_TS_Process.Tasks;
using QRST_DI_TS_Process.Site;
using QRST_DI_TS_Process.Orders.InstalledOrders;
using System.IO;
using QRST_DI_DS_Metadata.Paths;
using QRST_DI_Resources;
using QRST_DI_SS_DBInterfaces.IDBEngine;
using EnumDataType = QRST_DI_TS_Process.Orders.InstalledOrders.EnumDataType;
using QRST_DI_SS_Basis;
using QRST_DI_SS_Basis.MetaData;

namespace QRST_DI_TS_Process.Orders
{
    public class OrderManager
    {
        private static IDbOperating _sqLiteOperating =
            Constant.IdbOperating;

        private static IDbBaseUtilities _sqLiteUtilities =
            Constant.IdbOperating.GetSubDbUtilities(EnumDBType.MIDB);
        //written by Jiang Bin
        public struct tablesname
        {
            public List<string> midbtables;
            public List<string> bsdbtables;
            public List<string> evdbtables;
            public List<string> indbtables;
            public List<string> ipdbtables;
            public List<string> isdbtables;
            public List<string> madbtables;
            public List<string> rcdbtables;
        }
        #region static
        public static string GetNewCode()
        {
            Random random = new Random();
            string type = "P";

            string date = ((DateTime.Now.Year - 1900) * 365 + DateTime.Now.DayOfYear).ToString("00000");
            string time = ((int)DateTime.Now.TimeOfDay.TotalSeconds).ToString("00000");
            string msecond = DateTime.Now.Millisecond.ToString("000");
            System.Threading.Thread.Sleep(1);
            string randcode = random.Next(9).ToString();
            return string.Format("{0}{1}{2}{3}{4}", type, date.Substring(1), time, msecond, randcode);
        }

        public static void SubmitOrder(OrderClass order)
        {
            AddNewOrder2DB(order);
        }

        public static OrderClass CreateNewOrder(
            string ordername,
            EnumOrderPriority priority,
            string owner,
            List<TaskClass> tasks,
            List<string[]> taskparams,
            string tssiteIP)
        {
            return CreateNewOrder(ordername, GetNewCode(), priority, owner, tasks, taskparams, tssiteIP, 0, DateTime.Now);
        }

        public static OrderClass CreateInstalledOrder(string orderName, string[] orderParas)
        {
            //新内置订单需提前注册
            OrderClass orderClass = null;
            switch (orderName)
            {
                case "IODOCDataImport":
                    //orderClass = new IODOCDataImport().Create(orderParas[0],orderParas[1],orderParas[2],orderParas[3],Convert.ToDateTime(orderParas[4]),orderParas[5],orderParas[6],orderParas[7],Convert.ToDateTime(orderParas[8]),Convert.ToInt32(orderParas[9]),orderParas[10]);//orderParas
                    orderClass = new IODOCDataImport().Create(orderParas[0], orderParas[1], orderParas[2], orderParas[3], orderParas[4], orderParas[5], orderParas[6], orderParas[7], orderParas[8], orderParas[9], orderParas[10]);
                    break;
                case "IODownLoadDOC":
                    orderClass = new IODownLoadDOC().CreateDownLoadDOC(orderParas[0], orderParas[1], orderParas[2]);
                    break;

                case "IOHJdataImport":
                    orderClass = new IOHJdataImport().CreateHJdataImportOrder(orderParas[0], orderParas[1]);
                    break;
                case "IOGF1dataPrepare":
                    orderClass = new IOGF1dataPrepare().CreateGF1dataPrepareOrder();
                    break;
                case "IOCbersDataImport":
                    orderClass = new IOCbersDataImport().CreateCbersImportOrder(orderParas[0], orderParas[1]);
                    break;
                case "IOModisDataImport":
                    orderClass = new IOModisDataImport().CreateModisImportOrder(orderParas[0], orderParas[1]);
                    break;
                case "IONoaaDataImport":
                    orderClass = new IONoaaDataImport().CreateNoaaImportOrder(orderParas[0], orderParas[1]);
                    break;
                case "IODEMDataImport":
                    orderClass = new IODEMDataImport().CreateDEMImportOrder(orderParas[0], orderParas[1], orderParas[2]);
                    break;
                case "IOVectorImport":
                    orderClass = new IOVectorImport().CreateVectorImportOrder(orderParas[0], orderParas[1], orderParas[2], orderParas[3]);
                    break;
                case "IONormalHJDataImport":
                    orderClass = new IONormalHJDataImport().CreateNormalHJdataImportOrder(orderParas[0], orderParas[1]);
                    break;
                case "IOSortedVectorImport":
                    orderClass = new IOSortedVectorImport().createSortedVectorDataImport(orderParas[0], orderParas[1], orderParas[2], orderParas[3], orderParas[4]);
                    break;
                case "IOTMDataImport":
                    orderClass = new IOTMDataImport().CreateTMImportOrder(orderParas[0], orderParas[1], orderParas[2]);
                    break;
                case "IOCopyAssistFiles":
                    orderClass = new IOCopyAssistFiles().createCopyAssistFiles(orderParas[0], orderParas[1], orderParas[2]);
                    break;
                case "IOTJQuickBirdDataImport":

                    orderClass = new IOTJQuickBirdDataImport().CreateTJQuickBirdImportOrder(orderParas[0], orderParas[1], orderParas[2]);
                    break;
                case "IOHHALOSDataImport":

                    orderClass = new IOHHALOSDataImport().CreateHHALOSImportOrder(orderParas[0], orderParas[1], orderParas[2]);
                    break;
                case "IOSZWorldView2DataImport":
                    orderClass = new IOSZWorldView2DataImport().CreateSZWorldView2ImportOrder(orderParas[0], orderParas[1], orderParas[2]);
                    break;
                case "IOZJCoastlandALOSDataImport":
                    orderClass = new IOZJCoastlandALOSDataImport().CreateZJCoastlandALOSImportOrder(orderParas[0], orderParas[1], orderParas[2]);
                    break;
                case "IOTJ5MRSImageDataImport":
                    orderClass = new IOTJ5MRSImageDataImport().CreateTJ5MRSImageImportOrder(orderParas[0], orderParas[1], orderParas[2]);
                    break;
                case "IOUserDataImport":
                    orderClass = new IOUserDataImport().CreateUserDataImport(orderParas[0], orderParas[1], orderParas[2]);
                    break;
                case "IODownLoadUserData":
                    orderClass = new IODownLoadUserData().CreateDownLoadUserData(orderParas[0], orderParas[1], orderParas[2]);
                    break;
                case "IOImportStandCmp":
                    orderClass = new IOImportStandCmp().CreateStandCmpImport(orderParas[0]);
                    break;
                case "IOImportStandWfl":
                    orderClass = new IOImportStandWfl().CreateStandWflImport(orderParas[0]);
                    break;
                case "IOImportUserProduct":
                    orderClass = new IOImportUserProduct().CreateUserProductImport(orderParas[0]);
                    break;
                case "IORasterDataImportNew":
                    orderClass = new IORasterDataImportNew().CreateRasterDataImport(orderParas[0], orderParas[1]);
                    break;
                case "IOImportZipCmp":
                    orderClass = new IOImportZipCmp().CreateZipCmpImportOrder(orderParas[0]);
                    break;
                case "IOGF1DataImport":
                    orderClass = new IOGF1DataImport().Create();
                    break;
                case "IOBCDDataImport":
                    orderClass = new IOBCDDataImport().Create();
                    break;
                case "IOGF5DataImport":
                    orderClass = new IOGF5DataImport().Create(orderParas[0]);
                    break;
                case "IOGF6DataImoort":
                    orderClass = new IOGF6DataImport().Create();
                    break;
                case "IOZY3DataImport":
                    orderClass = new IOZY3DataImport().Create();
                    break;
                case "IOHJDataImportStandard":
                    orderClass = new IOHJDataImportStandard().Create();
                    break;
                case "IOGF1CorrectedDataImport":
                    orderClass = new IOGF1CorrectedDataImport().CreateGF1CorrectedDataImportOrder(orderParas[0]);
                    break;
                case "IOZY02cDataImport":
                    orderClass = new IOZY02cDataImport().Create();
                    break;
                case "IONormalDataImport":
                    orderClass = new IONormalDataImport().Create(orderParas[0], orderParas[1]);
                    break;
                case "IOTileImport":
                    orderClass = new IOTileImport().CreateIOTileImportOrder(orderParas[0]);
                    break;
                case "IODownLoadTiles":
                    orderClass = new IODownLoadTiles().CreateDownLoadTiles(orderParas[0], orderParas[1], orderParas[2], orderParas[3]);
                    break;
                case "IOSJ9ADataImport":
                    orderClass = new IOSJ9ADataImport().Create();
                    break;
                case "IOHJ1CDataImport":
                    orderClass = new IOHJ1CDataImport().Create();
                    break;
                case "IOPushTileFiles":
                    orderClass = new IOPushTileFiles().Create(orderParas[0], orderParas[1]);
                    break;
                case "IOFullBackup":
                    orderClass = new IOFullBackup().Create();
                    break;
                case "IOGF3DataImport":
                    orderClass = new IOGF3DataImport().Create();
                    break;

                default:
                    break;
            }
            return orderClass;
        }

        public static List<OrderClass> CreateInstalledBatchOrder(string orderName, string[] orderParas)
        {
            List<OrderClass> orderClasses = new List<OrderClass>();
            switch (orderName)
            {
                case "IONormalHJBatchImport":
                    orderClasses = new IONormalHJBatchImport().CreateNormalHJBatchImport(orderParas[0]);
                    break;
                case "IOVectorBatchImport":
                    orderClasses = new IOVectorBatchImport().CreateVectorBatchImport(orderParas[0], orderParas[1], orderParas[2]);
                    break;
                case "IONoaaBatchImport":
                    orderClasses = new IONoaaBatchImport().CreateNoaaBatchImport(orderParas[0]);
                    break;
                case "IOModisBatchImport":
                    orderClasses = new IOModisBatchImport().CreateModisBatchImport(orderParas[0]);
                    break;
                case "IOCbersBatchImport":
                    orderClasses = new IOCbersBatchImport().CreateCbersBatchImport(orderParas[0]);
                    break;
                case "IOAlgCmpBatchImport":
                    orderClasses = new IOAlgCmpBatchImport().CreateAlgCmpBatchImport(orderParas[0]);
                    break;
                case "IOGF1dataBatchImport":
                    orderClasses = new IOGF1dataBatchImport().CreateGF1BatchImport(orderParas[0]);
                    break;
                default:
                    break;
            }
            return orderClasses;
        }


        public static OrderClass CreateNewOrder(
          string ordername,
          string ordercode,
          EnumOrderPriority priority,
          string owner,
          List<TaskClass> tasks,
          List<string[]> taskparams,
          string tssiteIP
                )
        {
            return CreateNewOrder(ordername, ordercode, priority, owner, tasks, taskparams, tssiteIP, 0, DateTime.Now);
        }

        public static OrderClass CreateNewOrder(
          string ordername,
          string ordercode,
          EnumOrderPriority priority,
          string owner,
          List<TaskClass> tasks,
          List<string[]> taskparams,
          string tssiteIP,
            int phase,
            DateTime submitdt
                )
        {
            OrderClass order = new OrderClass();

            order.Priority = priority;
            order.OrderName = ordername;
            order.OrderCode = ordercode;
            order.Owner = owner;
            order.Tasks = tasks;

            order.TaskParams = taskparams;

            order.SubmitTime = submitdt;
            order.TaskPhase = phase;
            order.TSSiteIP = tssiteIP;

            return order;
        }

        public static OrderClass GetMissingProcessingOrderFromDB(string tssiteCode, List<string> processingOrderCodes)
        {
            OrderClass neworder = null;
            if (tssiteCode == "")
            {
                return neworder;
            }

            string sql = string.Format(@"select * from orders where TServerSite='{0}' and Status='Processing' order by submittime;", tssiteCode);

            DataSet ds = _sqLiteUtilities.GetDataSet(sql);

            if (ds.Tables.Count > 0)
            {
                DataRow firstRow = null;
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    string ordercode = dr["OrderCode"].ToString();
                    if (processingOrderCodes.Contains(ordercode))
                    {
                        continue;
                    }
                    else
                    {
                        firstRow = dr;
                        break;
                    }
                }

                if (firstRow != null)
                {
                    neworder = DBRow2OrderCls(firstRow);
                }
            }


            return neworder;
        }

        /// <summary>
        /// 将订单的状态从processing改为waiting
        /// </summary>
        /// <param name="order"></param>
        public static void ResignOrderProcessing(OrderClass order)
        {
            string sql = string.Format(@"update orders set Status='Waiting' where ordercode='{0}';", order.OrderCode);
            int effectrows = _sqLiteUtilities.ExecuteSql(sql);
        }

        /// <summary>
        /// 站点领取新订单
        /// </summary>
        /// <param name="tssiteCode"></param>
        /// <returns></returns>
        public static OrderClass GetNewOrderFromDB(string tssiteCode, OrderProcessBy processBy)
        {
            OrderClass neworder = null;
            //增加判断语句，判断是通过时间排序取订单还是通过优先级加时间排序取订单，@张飞龙
            string sql = null;
            if (processBy == OrderProcessBy.ONLY_BY_TIME)
            {
                sql = (tssiteCode.Trim() != "") ?
                string.Format(@"select * from orders where (TServerSite='null' or TServerSite='{0}') and Status='Waiting' order by submittime;", tssiteCode)
                : "select * from orders where TServerSite='null' and Status='Waiting' order by submittime;";
            }
            else
            {
                sql = (tssiteCode.Trim() != "") ?
                string.Format(@"select * from orders where (TServerSite='null' or TServerSite='{0}') and Status='Waiting' group by Priority order by submittime;", tssiteCode)
                   : "select * from orders where TServerSite='null' and Status='Waiting' group by Priority order by submittime;";
            }
            //string sql = (tssiteCode.Trim() != "") ?
            //    string.Format(@"select * from orders where (TServerSite='null' or TServerSite='{0}') and Status='Waiting' group by Priority order by submittime;", tssiteCode)
            //    : "select * from orders where TServerSite='null' and Status='Waiting' group by Priority order by submittime;";
            DataSet ds = _sqLiteUtilities.GetDataSet(sql);

            if (ds.Tables.Count > 0)
            {
                DataRow firstRow = null;
                EnumOrderPriority curPriority = EnumOrderPriority.Low;
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    string priority = dr["Priority"].ToString();
                    EnumOrderPriority epriority = EnumOrderPriority.Normal;
                    Enum.TryParse(priority, out epriority);

                    if (epriority < curPriority || firstRow == null)
                    {
                        curPriority = epriority;
                        firstRow = dr;
                    }
                }

                if (firstRow != null)
                {
                    neworder = DBRow2OrderCls(firstRow);
                }
            }


            return neworder;

        }

        public static OrderClass DBRow2OrderCls(DataRow dr)
        {
            //
            OrderClass order = new OrderClass();
            order.OrderName = dr["name"].ToString();
            order.OrderCode = dr["OrderCode"].ToString();
            order.TSSiteIP = TServerSiteManager.ConvertTSSiteCode2TSSiteIP(dr["TServerSite"].ToString());
            EnumOrderStatusType statustype = EnumOrderStatusType.Waiting;
            Enum.TryParse(dr["Status"].ToString(), out statustype);
            order.Status = statustype;
            EnumOrderType ordertype = EnumOrderType.UnKnown;
            Enum.TryParse(dr["Type"].ToString(), out ordertype);
            order.Type = ordertype;

            order.Owner = dr["Owner"].ToString();
            order.SubmitTime = DateTime.Parse(dr["SubmitTime"].ToString());

            order.OrderParams = Str2OrderParams(dr["OrderParams"].ToString());

            order.Tasks = Str2TaskList(dr["Tasks"].ToString());
            order.TaskParams = Str2TaskParams(dr["TaskParams"].ToString());
            order.TaskPhase = int.Parse(dr["Phase"].ToString());

            EnumOrderPriority orderpriority = EnumOrderPriority.Normal;
            Enum.TryParse(dr["Priority"].ToString(), out orderpriority);
            order.Priority = orderpriority;

            order.SetOrderWorkspace = (dr["WorkSpace"].ToString() != "%OrderWorkspace%") ? dr["WorkSpace"].ToString() : "";
            order.Create();
            return order;
        }
        //static TableLocker dblock;

        public static void AddNewOrder2DB(OrderClass order)
        {
            //if (dblock == null)
            //{
            //    //string ss = TSPCommonReference.dbOperating.MIDB.ToString(); //QRST_DI_DS_Basis.DBEngine.MySqlBaseUtilities
            //    dblock = new TableLocker(TSPCommonReference.dbOperating.MIDB);
            //}
            try
            {

                _sqLiteOperating.LockTable("orders", EnumDBType.MIDB);
                string newqrstcode = Constant.IdbOperating.GetNewQrstCodeFromTableName("orders",
                    EnumDBType.MIDB);
                //int newid = TSPCommonReference.dbOperating.GetNewID("orders", EnumDBType.MIDB);

                //string deleteSql = string.Format(@"Delete from orders where qrst_code='{0}'", order.OrderCode);
                //TSPCommonReference.dbOperating.MIDB.ExecuteSql(deleteSql);

                string timeNow = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                string sql =
                    string.Format(
                        @"insert into orders(Name,TServerSite,Status,Type,Owner,SubmitTime,Tasks,TaskParams,Phase,OrderCode,qrst_code,Priority,OrderParams,WorkSpace) values ('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}',{8},'{9}','{10}','{11}','{12}','{13}');",
                        order.OrderName,
                        (order.TSSiteIP == null)
                            ? "null"
                            : TServerSiteManager.ConvertTSSiteIP2TSSiteCode(order.TSSiteIP), order.Status,
                        Enum.GetName(typeof (EnumOrderType), order.Type), order.Owner, timeNow, TaskList2Str(order),
                        TaskParams2Str(order), order.TaskPhase, order.OrderCode, newqrstcode,
                        Enum.GetName(typeof (EnumOrderPriority), order.Priority), OrderParams2Str(order),
                        order.OrderWorkspace);
                _sqLiteUtilities.ExecuteSql(sql);
                _sqLiteOperating.UnlockTable("orders", EnumDBType.MIDB);

            }
            catch (Exception)
            {

                throw;
            }
        }

        public static bool IsOrderUnsigned(OrderClass order)
        {
            string sql = string.Format(@"Select Status from orders where ordercode='{0}';", order.OrderCode);


            DataSet ds = _sqLiteUtilities.GetDataSet(sql);

            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                DataRow dr = ds.Tables[0].Rows[0];
                EnumOrderStatusType statustype = EnumOrderStatusType.Error;
                bool sucParse = Enum.TryParse(dr["Status"].ToString(), out statustype);

                if (!sucParse)
                {
                    return false;
                }
                if (statustype == EnumOrderStatusType.Waiting)
                {
                    return true;
                }
            }

            return false;
        }

        public static bool UpdateOrder2DB(OrderClass order)
        {
            string sql =
                string.Format(
                    @"update orders set Name='{0}',TServerSite='{1}',Status='{2}',Type='{3}',Owner='{4}',SubmitTime='{5}',Tasks='{6}',TaskParams='{7}',Phase={8},Priority='{9}' where ordercode='{10}';",
                    order.OrderName,
                    (order.TSSiteIP == null) ? "null" : TServerSiteManager.ConvertTSSiteIP2TSSiteCode(order.TSSiteIP),
                    order.Status, Enum.GetName(typeof (EnumOrderType), order.Type), order.Owner, order.SubmitTime,
                    TaskList2Str(order), TaskParams2Str(order), order.TaskPhase,
                    Enum.GetName(typeof (EnumOrderPriority), order.Priority), order.OrderCode);
            int effectrows = _sqLiteUtilities.ExecuteSql(sql);

            return (effectrows == 1) ? true : false;
        }

        public static bool UpdateOrderStatus(string orderCode, EnumOrderStatusType status)
        {
            string sql = string.Format(@"update orders set Status='{0}'where ordercode='{1}'", status.ToString(), orderCode);
            int effectrows = _sqLiteUtilities.ExecuteSql(sql);

            return (effectrows == 1) ? true : false;
        }

        private static string TaskList2Str(OrderClass order)
        {
            string str = "";

            foreach (TaskClass task in order.Tasks)
            {
                str += string.Format(@"{0},", task.TaskName);

            }
            return str.TrimEnd(",".ToCharArray());
        }

        public static string OrderParams2Str(OrderClass order)
        {
            string str = "";
            if (order.OrderParams == null)
            {
                return "";
            }
            foreach (string strp in order.OrderParams)
            {
                string para = strp;
                if (string.IsNullOrEmpty(para))
                {
                    para = "";
                }
                str += string.Format(@"{0},", para.Trim());
            }
            return str.Replace(@"\", @"\\");
        }

        public static string TaskParams2Str(OrderClass order)
        {
            string str = "";
            foreach (string[] taskparams in order.TaskParams)
            {
                string strp = "";
                foreach (string p in taskparams)
                {
                    strp += string.Format(@"{0},", p);
                }
                strp = strp.TrimEnd(",".ToCharArray());
                strp = strp.Replace("<", "$&$");
                strp = strp.Replace(">", "$^$");
                str += string.Format(@"<{0}>", strp);
            }
            return str.Replace(@"\", @"\\");
        }

        public static List<TaskClass> Str2TaskList(string dbstr)
        {
            List<TaskClass> tasks = new List<TaskClass>();
            dbstr.Trim();
            string[] strs0 = dbstr.Split(",".ToCharArray());

            foreach (string str1 in strs0)
            {
                TaskClass task = TaskClass.CreateTaskClassByName(str1);
                tasks.Add(task);
            }

            return tasks;
        }
        private static Object thisLock = new Object();
        public static bool SignOrderProcessing(OrderClass order, string tssip)
        {
            lock (thisLock)
            {
                if (order.Status != EnumOrderStatusType.Waiting)
                {
                    return false;
                }

                order.TSSiteIP = tssip;
                order.Status = EnumOrderStatusType.Processing;

                string sql = string.Format(@"update orders set TServerSite='{0}',Status='{1}'where ordercode='{2}' and  Status='Waiting';",
                    (order.TSSiteIP == null) ? "null" : TServerSiteManager.ConvertTSSiteIP2TSSiteCode(order.TSSiteIP), order.Status, order.OrderCode);
                int effectrows = _sqLiteUtilities.ExecuteSql(sql);
                System.Threading.Thread.Sleep(200);
                return (effectrows > 0) ? true : false;
            }
        }

        public static string[] Str2OrderParams(string dbstr)
        {

            dbstr = dbstr.Trim();
            string[] orderparams = dbstr.Split(",".ToCharArray());

            for (int i = 0; i < orderparams.Length; i++)
            {
                orderparams[i] = orderparams[i].Trim();
            }

            return orderparams;
        }
        //将order中的TaskParams转换为Task的参数列表
        private static List<string[]> Str2TaskParams(string dbstr)
        {
            List<string[]> taskparams = new List<string[]>();
            dbstr = dbstr.Trim();
            dbstr = dbstr.TrimStart("<".ToCharArray());
            dbstr = dbstr.TrimEnd(">".ToCharArray());
            string[] strs0 = dbstr.Split(new string[] { "><" }, StringSplitOptions.None);

            foreach (string str1 in strs0)
            {
                //因为向数据库中加入xml元数据，有<>号，因此替换掉<>
                string str2;
                str2 = str1.Replace("$&$", "<");
                str2 = str2.Replace("$^$", ">");
                string[] strs1 = str2.Split(",".ToCharArray());
                taskparams.Add(strs1);
            }

            return taskparams;
        }

        public static List<OrderClass> GetOrderObjLst(string whereCondition)
        {
            string sql = "select * from orders ";
            if (!string.IsNullOrEmpty(whereCondition))
            {
                sql = sql + " where " + whereCondition;
            }
            DataSet ds = _sqLiteUtilities.GetDataSet(sql);
            if (ds != null && ds.Tables.Count > 0)
            {
                List<OrderClass> ordersLst = new List<OrderClass>();
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    ordersLst.Add(DBRow2OrderCls(ds.Tables[0].Rows[i]));
                }
                return ordersLst;
            }
            else
                return null;
        }

        public static DataTable GetOrderList(string whereCondition)
        {
            string sql = "select s.* ,t.addressip from orders s left join tileserversitesinfo t on  s.tserversite = t.qrst_code  ";
            if (!string.IsNullOrEmpty(whereCondition))
            {
                sql = sql + " where " + whereCondition;
            }
            sql = sql + " order by SubmitTime desc";
            DataSet ds = _sqLiteUtilities.GetDataSet(sql);

            return ds.Tables[0];
        }

        //获取正在执行的订单
        public static DataTable GetProcessingTable()
        {
            string sql = "select * from order_processing_view";
            DataSet ds = _sqLiteUtilities.GetDataSet(sql);
            return ds.Tables[0];
        }

        /// <summary>
        /// 根据订单号删除订单
        /// </summary>
        /// <param name="orderCode"></param>
        public static void DeleteOrderByCode(string orderCode)
        {
            string delSql = string.Format("delete from orders where OrderCode = '{0}'", orderCode);
            _sqLiteUtilities.ExecuteSql(delSql);
        }

        /// <summary>
        /// 更改订单状态，站点接收到status变化后进行相应的后续操作
        /// </summary>
        /// <param name="orderCode"></param>
        public static void ChangeOrderStatus(string orderCode, EnumOrderStatusType type)
        {
            string updateSql = string.Format("update orders set Status = '{0}'  where OrderCode = '{1}'", type.ToString(), orderCode);
            _sqLiteUtilities.ExecuteSql(updateSql);
        }

        /// <summary>
        /// 创建工作空间
        /// </summary>
        /// <param name="IPAddress">共享文件夹所在站点</param>
        /// <param name="code">订单编号</param>
        /// <returns>工作空间路径</returns>
        public static string BuildWorkSpaceByOrderCode(string IPAddress, string code)
        {
            try
            {
                Directory.CreateDirectory(StoragePath.TempStoragePath(IPAddress, code));
                return StoragePath.TempStoragePath(IPAddress, code);
            }
            catch (Exception ex)
            {
                return "-1";
            }
        }

        /// <summary>
        /// 根据数据的不同导入类型创建工作空间 zxw
        /// </summary>
        /// <param name="IPAddress">共享文件夹所在站点</param>
        /// <param name="code">订单编号</param>
        /// <param name="dataType">数据类型</param>
        /// <returns>工作空间路径</returns>
        public static string BuildWorkSpaceByOrderCode(string IPAddress, string code, string dataType)
        {
            try
            {
                string path = StoragePath.TempStoragePath(IPAddress, code);
                Directory.CreateDirectory(path);
                if (dataType == EnumDataType.RegularProduct.ToString() || dataType == EnumDataType.UserProduct.ToString())
                {
                    Directory.CreateDirectory(string.Format(@"{0}//{1}", path, StaticStrings.OrignalData));
                    Directory.CreateDirectory(string.Format(@"{0}//{1}", path, StaticStrings.Products));
                    Directory.CreateDirectory(string.Format(@"{0}//{1}", path, StaticStrings.TiledData));
                    Directory.CreateDirectory(string.Format(@"{0}//{1}", path, StaticStrings.CorrectedData));
                }
                return path;
            }
            catch (Exception ex)
            {
                return "-1";
            }
        }

        public static OrderClass GetOrderByCode(string ordercode)
        {
            string sql = string.Format("select * from orders where OrderCode = '{0}'", ordercode);
            DataSet ds = _sqLiteUtilities.GetDataSet(sql);

            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                return OrderManager.DBRow2OrderCls(ds.Tables[0].Rows[0]);
            }
            else
                return null;
        }

        //written by Jiang Bin, 用于检查自动备份是否已产生自动备份订单
        public static string CheckOrder2DB(OrderClass order)
        {
            string sql = string.Format(@"select count(*) from orders where SubmitTime like CONCAT((select date_format(now(),'%Y-%m-%d')),'%');");
            string checknum = _sqLiteUtilities.myExcuteScalar(sql);
            return checknum;
        }

        //written by Jiang Bin,检查数据备份的订单提交状态
        public static int checkorderstatus()
        {
            string sql = string.Format(@"update appsettings set `value`='processing' where `key`='MysqlBackupStatus' and `value`='waiting'");
            int checkstatus1 = _sqLiteUtilities.ExecuteSql(sql);
            return checkstatus1;
        }

        //written by Jiang Bin, 将数据库备份时间写入appsettting
        public static void updatebackuptime()
        {
            string sql = string.Format(@"update appsettings set `value`=now() where `key`='MysqlBackupDatetime'");
            _sqLiteUtilities.ExecuteSql(sql);
        }

        //written by Jiang Bin,获取appsetting 中写的备份地址
        public static string getbackupaddress()
        {
            string sql = string.Format(@"select value from appsettings where `key`='BackupAddress'");
            string backupaddress = _sqLiteUtilities.myExcuteScalar(sql);
            return backupaddress;
        }

        //Written by Jiang Bin,获取subinfo中的数据库连接信息
        public static string getConnectStr(string dbname)
        {
            string str1 = string.Format(@"select ConnectStr from subdbinfo where `NAME`=" + "'" + dbname + "'");
            string connectstr = _sqLiteUtilities.myExcuteScalar(str1);
            return connectstr;
        }

        //written by Jiang Bin, 获取数据库中所有表的名称
        public static tablesname getalltablesname()
        {

            tablesname _tablesname;

            //List<string> tablesname=new List<string>();
            //string[] _dbname ={"bsdb","evdb","indb","ipdb","isdb","madb","midb","rcdb"};
            //dbname.AddRange(_dbname);
            //string[] dbsql=new string[8];
            string dbsqlmidb, dbsqlbsdb, dbsqlevdb, dbsqlindb, dbsqlipdb, dbsqlisdb, dbsqlmadb, dbsqlrcdb;


            dbsqlmidb = string.Format(@"select table_name from information_schema.tables where table_schema='midb' and table_type='base table'");
            _tablesname.midbtables = _sqLiteUtilities.myExcuteReader(dbsqlmidb);

            dbsqlbsdb = string.Format(@"select table_name from information_schema.tables where table_schema='bsdb' and table_type='base table'");
            _tablesname.bsdbtables = _sqLiteUtilities.myExcuteReader(dbsqlbsdb);

            dbsqlevdb = string.Format(@"select table_name from information_schema.tables where table_schema='evdb' and table_type='base table'");
            _tablesname.evdbtables = _sqLiteUtilities.myExcuteReader(dbsqlevdb);

            dbsqlindb = string.Format(@"select table_name from information_schema.tables where table_schema='indb' and table_type='base table'");
            _tablesname.indbtables = _sqLiteUtilities.myExcuteReader(dbsqlindb);

            dbsqlipdb = string.Format(@"select table_name from information_schema.tables where table_schema='ipdb' and table_type='base table'");
            _tablesname.ipdbtables = _sqLiteUtilities.myExcuteReader(dbsqlipdb);

            dbsqlisdb = string.Format(@"select table_name from information_schema.tables where table_schema='isdb' and table_type='base table'");
            _tablesname.isdbtables = _sqLiteUtilities.myExcuteReader(dbsqlisdb);

            dbsqlmadb = string.Format(@"select table_name from information_schema.tables where table_schema='madb' and table_type='base table'");
            _tablesname.madbtables = _sqLiteUtilities.myExcuteReader(dbsqlmadb);

            dbsqlrcdb = string.Format(@"select table_name from information_schema.tables where table_schema='rcdb' and table_type='base table'");
            _tablesname.rcdbtables = _sqLiteUtilities.myExcuteReader(dbsqlrcdb);
            return _tablesname;

        }

        //written by Jiang Bin, 将备份状态从processing改为waiting
        public static void changestatus()
        {
            string sql = string.Format(@"update appsettings set `value`='waiting' where `key`='MysqlBackupStatus' and `value`='processing'");
            _sqLiteUtilities.ExecuteSql(sql);
        }

        //written by Jiang Bin, 获取上次的备份时间，用来计算两次全备份的时间间隔
        public static double gettimespan()
        {
            string lasttime;
            string sql = string.Format(@"select value from appsettings where `key`='MysqlBackupDatetime'");
            try
            {
                lasttime = _sqLiteUtilities.myExcuteScalar(sql);
            }
            catch
            {
                lasttime = "2014-10-30 21:00:00";
            }
            DateTime t1 = DateTime.ParseExact(lasttime, "yyyy-MM-dd HH:mm:ss", null);
            DateTime t2 = DateTime.Now;
            TimeSpan t3 = t2 - t1;
            double ts = t3.TotalDays;
            return ts;

        }

        //written by Jiang Bin, 获取要求的备份间隔
        public static double getMysqlBackupSpan()
        {
            string sql = string.Format(@"select value from appsettings where `key`='MysqlBackupSpan'");
            string span = _sqLiteUtilities.myExcuteScalar(sql);
            double span1 = Convert.ToDouble(span);
            return span1;
        }

        #endregion
    }

    //取订单时通过时间或者时间加优先级
    public enum OrderProcessBy
    {
        ONLY_BY_TIME,
        BY_TIME_AND_PRIORITY
    }
}
