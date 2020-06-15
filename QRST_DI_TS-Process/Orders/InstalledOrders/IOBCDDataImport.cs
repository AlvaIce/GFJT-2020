using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using QRST_DI_TS_Process.Tasks.InstalledTasks;
using QRST_DI_TS_Basis.DirectlyAddress;

namespace QRST_DI_TS_Process.Orders.InstalledOrders
{
    /// <summary>
    /// BCD数据入库订单类
    /// </summary>
    public class IOBCDDataImport
    {
        /// <summary>
        /// 创建BCD数据入库订单
        /// </summary>
        /// <returns>BCD数据入库订单</returns>
        public OrderClass Create()
        {
            //创建BCD数据入库订单
            OrderClass BCDdataImportOrder = new OrderClass();

            //设置BCD数据入库订单类型为Installed
            BCDdataImportOrder.Type = EnumOrderType.Installed;

            //随机生成BCD数据入库订单编号
            BCDdataImportOrder.OrderCode = OrderManager.GetNewCode();

            //设置BCD数据入库订单任务，并给每个任务赋参
            //添加日志记录任务
            BCDdataImportOrder.Tasks.Add(new ITOutputLoginfo());
            BCDdataImportOrder.TaskParams.Add(new string[] { "开始订单任务！" });

            //添加创建BCD数据入库订单工作空间任务
            //待入库的BCD数据由用户拷贝到创建的订单工作空间中
            BCDdataImportOrder.Tasks.Add(new ITCreateBCDDataWorkspace());
            BCDdataImportOrder.TaskParams.Add(new string[] { "" });

            //添加解压tar.gz格式的压缩包的任务
            //参数1为待解压文件的路径；参数2为解压后文件的存储路径
            BCDdataImportOrder.Tasks.Add(new ITUnzipTargz4XmlJpg());
            BCDdataImportOrder.TaskParams.Add(new string[] { BCDdataImportOrder.OrderWorkspace, StorageBasePath.SharePath_OrignalData(BCDdataImportOrder.OrderWorkspace) });

            //添加BCD数据质量检测的任务
            //参数为BCD数据入库订单工作空间
            BCDdataImportOrder.Tasks.Add(new ITBCDDataQualityInspection());
            BCDdataImportOrder.TaskParams.Add(new string[] { BCDdataImportOrder.OrderWorkspace });

            //添加BCD数据入库的任务
            //参数为BCD数据入库订单工作空间
            BCDdataImportOrder.Tasks.Add(new ITBCDDataImport());
            BCDdataImportOrder.TaskParams.Add(new string[] { BCDdataImportOrder.OrderWorkspace, "false" });

            //添加日志记录任务
            BCDdataImportOrder.Tasks.Add(new ITOutputLoginfo());
            BCDdataImportOrder.TaskParams.Add(new string[] { "完成数据入库" });

            return BCDdataImportOrder;
        }


    }
}
