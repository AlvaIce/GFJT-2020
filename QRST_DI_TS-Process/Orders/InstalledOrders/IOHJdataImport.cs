using System.Collections.Generic;
using QRST_DI_TS_Process.Tasks.InstalledTasks;
using System.IO;

namespace QRST_DI_TS_Process.Orders.InstalledOrders
{
    public class IOHJdataImport
    {

        /// <summary>
        /// Installed Order for HJ Data Preparing
        /// OrderParams描述
        /// OrderParams[0]为待导入数据存放路径
        /// </summary>
        /// <param name="SourceFilePath"></param>
        /// <returns></returns>
        public OrderClass CreateHJdataImportOrder(string SourceFilePath, string orderworkspace)
        {
            OrderClass HJdataImportOrder = new OrderClass();
            HJdataImportOrder.Type = EnumOrderType.Installed;
            HJdataImportOrder.OrderCode = OrderManager.GetNewCode();

            //参数：SourceFilePath
            HJdataImportOrder.OrderParams = new string[] { SourceFilePath, orderworkspace };
            HJdataImportOrder.SetOrderWorkspace = orderworkspace;   //重置工作空间 到数据准备订单位置

            //  切片入库(建索引)
            HJdataImportOrder.Tasks.Add(new ITStoreTilesNew());
            HJdataImportOrder.TaskParams.Add(new string[] { HJdataImportOrder.OrderWorkspace });
            //  源数据及校正文件入库
            HJdataImportOrder.Tasks.Add(new ITStoreHJData());
            HJdataImportOrder.TaskParams.Add(new string[] { HJdataImportOrder.OrderWorkspace, SourceFilePath });

            //  元数据提取入库
            HJdataImportOrder.Tasks.Add(new ITInsertHJMetaData());
            HJdataImportOrder.TaskParams.Add(new string[] { SourceFilePath });

            return HJdataImportOrder;
        }

        /// <summary>
        /// Installed Order for HJ Data Preparing
        /// OrderParams描述
        /// orderworkspace 为待导入数据存放路径
        /// </summary>
        /// <param name="SourceFilePath"></param>
        /// <returns></returns>
        public OrderClass CreateHJdataImportOrder(string orderworkspace,EnumOrderStatusType status1)
        {
            string SourceFilePath = Directory.GetFiles(orderworkspace)[0];
            OrderClass HJdataImportOrder = new OrderClass(status1);
            HJdataImportOrder.Type = EnumOrderType.Installed;
            HJdataImportOrder.OrderCode = OrderManager.GetNewCode();

            //参数：SourceFilePath
            HJdataImportOrder.OrderParams = new string[] { SourceFilePath, orderworkspace };
            HJdataImportOrder.SetOrderWorkspace = orderworkspace;   //重置工作空间 到数据准备订单位置

            //  切片入库(建索引)
            HJdataImportOrder.Tasks.Add(new ITStoreTiles());
            HJdataImportOrder.TaskParams.Add(new string[] { HJdataImportOrder.OrderWorkspace });
            //  源数据及校正文件入库
            HJdataImportOrder.Tasks.Add(new ITStoreHJData());
            HJdataImportOrder.TaskParams.Add(new string[] { HJdataImportOrder.OrderWorkspace, SourceFilePath });

            //  元数据提取入库
            HJdataImportOrder.Tasks.Add(new ITInsertHJMetaData());
            HJdataImportOrder.TaskParams.Add(new string[] { SourceFilePath });

            return HJdataImportOrder;
        }


        /// <summary>
        /// 创建批量的环境星数据导入订单
        /// </summary>
        /// <param name="orderworkspaceDiractory">要倒入的文件夹所存放的路径</param>
        /// <status1>设置批量订单的状态，默认为挂起，以等待完成数据准备</status1>
        /// <returns></returns>
        public List<OrderClass> CreateBatchHJdataImportOrder(string orderworkspaceDiractory,EnumOrderStatusType status1 = EnumOrderStatusType.Suspended)
        {
            List<OrderClass> orderClass = new List<OrderClass>();
            string[] dataPaths = Directory.GetDirectories(orderworkspaceDiractory);
            for (int i = 0 ; i < dataPaths.Length ;i++ )
            {
                string []datasources  = Directory.GetFiles(dataPaths[i]);
                if ((datasources.Length > 0) && (datasources[0].EndsWith(".tar.gz") || datasources[0].EndsWith(".TAR.GZ") || datasources[0].EndsWith(".TAR.gz")))
                {
                    string str = dataPaths[i];
                    orderClass.Add(CreateHJdataImportOrder(str, status1));
                }
            }
            return orderClass;
        }
    }
}
