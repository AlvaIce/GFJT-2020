using QRST_DI_TS_Basis.DirectlyAddress;
using QRST_DI_TS_Process.Tasks.InstalledTasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QRST_DI_TS_Process.Orders.InstalledOrders
{
    public class IOGF6DataImport : OrderClass
    {

        public OrderClass Create()
        {
            OrderClass GF6DataImportOrder = new OrderClass();
            //参数：SourceFilePath
            GF6DataImportOrder.Type = EnumOrderType.Installed;
            GF6DataImportOrder.OrderCode = OrderManager.GetNewCode();

            //设置任务,任务赋参
            //  日志消息
            GF6DataImportOrder.Tasks.Add(new ITOutputLoginfo());
            GF6DataImportOrder.TaskParams.Add(new string[] { "开始订单任务！" });
            //  创建文件夹工作空间
            GF6DataImportOrder.Tasks.Add(new ITCreateGF6DataWorkspace());
            GF6DataImportOrder.TaskParams.Add(new string[] { "" });
            //由第三方拷贝数据到工作空间

            //  文件解压
            GF6DataImportOrder.Tasks.Add(new ITUnzipTargz4XmlJpg());
            GF6DataImportOrder.TaskParams.Add(new string[] { GF6DataImportOrder.OrderWorkspace, StorageBasePath.SharePath_OrignalData(GF6DataImportOrder.OrderWorkspace) });

            GF6DataImportOrder.Tasks.Add(new ITGF6DataQualityInspection());
            GF6DataImportOrder.TaskParams.Add(new string[] { GF6DataImportOrder.OrderWorkspace });


            //源数据文件入库
            GF6DataImportOrder.Tasks.Add(new ITGF6DataImport());
            GF6DataImportOrder.TaskParams.Add(new string[] { GF6DataImportOrder.OrderWorkspace, "false" });

            //姜斌插入的task，在入库数据的同时，向判定程序发送入库的影像信息
            //GF1dataImportOrder.Tasks.Add(new ITSendGF1Message());
            //GF1dataImportOrder.TaskParams.Add(new string[] { GF1dataImportOrder.OrderWorkspace });

            //  日志消息
            GF6DataImportOrder.Tasks.Add(new ITOutputLoginfo());
            GF6DataImportOrder.TaskParams.Add(new string[] { "完成数据入库" });

            return GF6DataImportOrder;
        }
    }
}
