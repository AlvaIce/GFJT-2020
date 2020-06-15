using System;
using QRST_DI_TS_Process.Tasks.InstalledTasks;
using QRST_DI_DS_Metadata.Paths;
using System.Data;
using QRST_DI_DS_Basis;

namespace QRST_DI_TS_Process.Orders.InstalledOrders
{
    public class IOVectorImport
    {
        public OrderClass CreateVectorImportOrder(string metaStr, string orderworkspace,string sourceFilePath,string qrstCode)
        {
            OrderClass VectorImportOrder = new OrderClass();
            VectorImportOrder.Type = EnumOrderType.Installed;
            VectorImportOrder.OrderCode = OrderManager.GetNewCode();

            //设置任务,任务赋参
            //  日志消息
            VectorImportOrder.Tasks.Add(new ITOutputLoginfo());
            VectorImportOrder.TaskParams.Add(new string[] { "开始订单任务！" });

            //参数：SourceFilePath
            VectorImportOrder.OrderParams = new string[] { metaStr, orderworkspace, sourceFilePath };
            VectorImportOrder.SetOrderWorkspace = orderworkspace;

            //  元数据提取入库
            VectorImportOrder.Tasks.Add(new ITInsertVectorMeta());
            VectorImportOrder.TaskParams.Add(new string[] { metaStr });

            //  源数据文件入库
            VectorImportOrder.Tasks.Add(new ITImportVectorFiles());
            //  从xml中反序列化元数据
            DataTable dt = SerializerUtil.GetDsFormatXml(metaStr);
            string dataname = dt.Rows[0]["DATANAME"].ToString();
            //设置文件在数据阵列里面的路径,\\172.16.0.185\综合数据库\基础空间数据库\地形图\全国1：5万道路、水系、行政区划基础地理数据\数据名称\
            string dir = String.Format("{0}{1}\\", (new StoragePath(qrstCode)).getGroupAddress(), dataname);
            VectorImportOrder.TaskParams.Add(new string[] { sourceFilePath, dir });

            return VectorImportOrder;
        }
    }
}
