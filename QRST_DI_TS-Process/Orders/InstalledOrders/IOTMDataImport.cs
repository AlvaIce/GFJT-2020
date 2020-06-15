using System;
using QRST_DI_TS_Process.Tasks.InstalledTasks;
using System.Data;
using QRST_DI_DS_Basis;
using QRST_DI_DS_Metadata.Paths;

namespace QRST_DI_TS_Process.Orders.InstalledOrders
{
    public class IOTMDataImport
    {
        public OrderClass CreateTMImportOrder(string metaStr, string orderworkspace, string sourceFilePath)
        {
            OrderClass TMImportOrder = new OrderClass();
            TMImportOrder.Type = EnumOrderType.Installed;
            TMImportOrder.OrderCode = OrderManager.GetNewCode();

            //设置任务,任务赋参
            //  日志消息
            TMImportOrder.Tasks.Add(new ITOutputLoginfo());
            TMImportOrder.TaskParams.Add(new string[] { "开始订单任务！" });

            //参数：SourceFilePath
            TMImportOrder.OrderParams = new string[] { metaStr, orderworkspace, sourceFilePath };
            TMImportOrder.SetOrderWorkspace = orderworkspace;   //重置工作空间 到数据准备订单位置

            //  元数据提取入库
            TMImportOrder.Tasks.Add(new ITInsertRasterMetaData());
            TMImportOrder.TaskParams.Add(new string[] { metaStr });

            //  源数据文件入库
            TMImportOrder.Tasks.Add(new ITImportRasterFiles());
            //  从xml中反序列化元数据
            DataTable dt = SerializerUtil.GetDsFormatXml(metaStr);
            string dataname = dt.Rows[0]["DATANAME"].ToString();
            //设置文件在数据阵列里面的路径,\\172.16.0.185\综合数据库\基础空间数据库\DEM\全国范围30米分辨率DEM数据\数据名称\
            string dir = String.Format("{0}{1}\\", (new StoragePath("BSDB-2-14")).getGroupAddress(), dataname);
            TMImportOrder.TaskParams.Add(new string[] { sourceFilePath, dir });

            return TMImportOrder;
        }
    }
}
