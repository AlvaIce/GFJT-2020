using System.Collections.Generic;
using System.IO;

namespace QRST_DI_TS_Process.Orders.InstalledOrders
{
    public class IOModisBatchImport
    {
        public List<OrderClass> CreateModisBatchImport(string SourceFileDir)
        {
            List<OrderClass> ModisBatchImportList = new List<OrderClass>();
            string[] files = Directory.GetFiles(SourceFileDir, "*.hdf");
            for (int i = 0; i < files.Length;i++ )
            {
                ModisBatchImportList.Add(new IORasterDataImportNew().CreateRasterDataImport("modis",files[i]));
            }
            return ModisBatchImportList;
        }

        /// <summary>
        /// 导入标准产品流程
        /// </summary>
        /// <returns></returns>
        //public OrderClass CreateRasterDataImport(string dataType, string dataPath)
        //{
        //    OrderClass orderClass = new OrderClass() { Type = EnumOrderType.Installed, OrderCode = OrderManager.GetNewCode(), /*订单赋参*/OrderParams = new string[] { dataType, dataPath } };

        //    //解压算法组件包
        //    orderClass.Tasks.Add(new ITRasterDataImportNew());
        //    orderClass.TaskParams.Add(new string[] { dataType, dataPath });

        //    return orderClass;
        //}

        //public List<OrderClass> CreateRasterDataBatchImport(string dataType, string[] dataPathArr)
        //{
        //    List<OrderClass> orderLst = new List<OrderClass>();
        //    for (int i = 0; i < dataPathArr.Length; i++)
        //    {
        //        orderLst.Add(CreateRasterDataImport(dataType, dataPathArr[i]));
        //    }
        //    return orderLst;
        //}
    }
}
