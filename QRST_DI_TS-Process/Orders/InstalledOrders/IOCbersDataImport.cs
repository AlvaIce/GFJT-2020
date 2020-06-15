using System.Collections.Generic;
using QRST_DI_TS_Process.Tasks.InstalledTasks;
using System.IO;

namespace QRST_DI_TS_Process.Orders.InstalledOrders
{
    public class IOCbersDataImport
    {
        public OrderClass CreateCbersImportOrder(string SourceFilePath, string orderworkspace)
        {
            OrderClass cbersImportOrder = new OrderClass();
            cbersImportOrder.Type = EnumOrderType.Installed;
            cbersImportOrder.OrderCode = OrderManager.GetNewCode();

            //设置任务,任务赋参
            //  日志消息
            cbersImportOrder.Tasks.Add(new ITOutputLoginfo());
            cbersImportOrder.TaskParams.Add(new string[] { "开始订单任务！" });

            //参数：SourceFilePath
            cbersImportOrder.OrderParams = new string[] { SourceFilePath, orderworkspace };
            cbersImportOrder.SetOrderWorkspace = orderworkspace;   //重置工作空间 到数据准备订单位置

            //  元数据提取入库、源数据文件入库
            cbersImportOrder.Tasks.Add(new ITInsertCbersMetaData());
            cbersImportOrder.TaskParams.Add(new string[] { SourceFilePath });


            //  源数据文件入库
            //cbersImportOrder.Tasks.Add(new ITCopyFiles());
            //MetaDataReader mdReader = new MetaDataReader();
            //MetaDataCbers metCbers = mdReader.ReadMetaDataCbers(SourceFilePath);
            //设置文件在数据阵列里面的路径,\\172.16.0.1\综合数据库\实验验证库\CBERS\cbers2b\Asa\2003\12\03\数据名称\
          //  string dir = "";  //String.Format("{0}实验验证数据库\\CBERS\\{1}\\{2}\\{3}\\{4}\\{5}\\{6}\\", StoragePath.StoreBasePath, metCbers.Satellite, metCbers.Sensor, string.Format("{0:0000}", metCbers.SceneDate.Year), string.Format("{0:00}", metCbers.SceneDate.Month), string.Format("{0:00}", metCbers.SceneDate.Day), Path.GetFileNameWithoutExtension(Path.GetFileNameWithoutExtension(SourceFilePath)));
            //cbersImportOrder.TaskParams.Add(new string[] { SourceFilePath, dir + Path.GetFileName(SourceFilePath) });


            return cbersImportOrder;
        }

        /// <summary>
        /// 创建批量的环Cbers数据导入订单
        /// </summary>
        /// <param name="orderworkspaceDiractory">要倒入的文件夹所存放的路径</param>
        /// <status1>设置批量订单的状态，默认为挂起，以等待完成数据准备</status1>
        /// <returns></returns>
        public List<OrderClass> CreateBatchCbersImportOrder(string orderworkspaceDiractory, EnumOrderStatusType status1 = EnumOrderStatusType.Suspended)
        {
            List<OrderClass> orderClass = new List<OrderClass>();
            string[] dataPaths = Directory.GetDirectories(orderworkspaceDiractory);
            for (int i = 0; i < dataPaths.Length; i++)
            {
                string[] datasources = Directory.GetFiles(dataPaths[i]);
                if ((datasources.Length > 0) && (datasources[0].EndsWith(".tar.gz") || datasources[0].EndsWith(".TAR.GZ") || datasources[0].EndsWith(".TAR.gz")))
                {
                    string str = dataPaths[i];
                    orderClass.Add(CreateCbersImportOrder(str, status1));
                }
            }
            return orderClass;
        }

        /// <summary>
        /// Installed Order for Cbers Data Preparing
        /// OrderParams描述
        /// orderworkspace 为待导入数据存放路径
        /// </summary>
        /// <param name="SourceFilePath"></param>
        /// <returns></returns>
        public OrderClass CreateCbersImportOrder(string orderworkspace, EnumOrderStatusType status1)
        {
            string SourceFilePath = Directory.GetFiles(orderworkspace)[0];
            OrderClass cbersImportOrder = new OrderClass(status1);
            cbersImportOrder.Type = EnumOrderType.Installed;
            cbersImportOrder.OrderCode = OrderManager.GetNewCode();

            //设置任务,任务赋参
            //  日志消息
            cbersImportOrder.Tasks.Add(new ITOutputLoginfo());
            cbersImportOrder.TaskParams.Add(new string[] { "开始订单任务！" });
            //  创建文件夹工作空间
            cbersImportOrder.Tasks.Add(new ITCreateNoProcessWorkspace());
            cbersImportOrder.TaskParams.Add(new string[] { "" });

            //参数：SourceFilePath
            cbersImportOrder.OrderParams = new string[] { SourceFilePath, orderworkspace };
            cbersImportOrder.SetOrderWorkspace = orderworkspace;   //重置工作空间 到数据准备订单位置

            //  源数据文件入库
            cbersImportOrder.Tasks.Add(new ITCopyFiles());
            cbersImportOrder.TaskParams.Add(new string[] { cbersImportOrder.OrderWorkspace, SourceFilePath });

            //  元数据提取入库
            cbersImportOrder.Tasks.Add(new ITInsertCbersMetaData());
            cbersImportOrder.TaskParams.Add(new string[] { SourceFilePath });

            return cbersImportOrder;
        }
    }
}
