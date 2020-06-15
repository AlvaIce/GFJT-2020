using System;
using System.Collections.Generic;
using QRST_DI_TS_Process.Tasks.InstalledTasks;
using System.IO;
using QRST_DI_DS_Metadata.MetaDataCls;
using QRST_DI_DS_Metadata;
using QRST_DI_DS_Metadata.Paths;

namespace QRST_DI_TS_Process.Orders.InstalledOrders
{
    public class IONoaaDataImport
    {
        public OrderClass CreateNoaaImportOrder(string SourceFilePath, string orderworkspace)
        {
            OrderClass noaaImportOrder = new OrderClass();
            noaaImportOrder.Type = EnumOrderType.Installed;
            noaaImportOrder.OrderCode = OrderManager.GetNewCode();

            //设置任务,任务赋参
            //  日志消息
            noaaImportOrder.Tasks.Add(new ITOutputLoginfo());
            noaaImportOrder.TaskParams.Add(new string[] { "开始订单任务！" });

            //参数：SourceFilePath
            noaaImportOrder.OrderParams = new string[] { SourceFilePath, orderworkspace };
            noaaImportOrder.SetOrderWorkspace = orderworkspace;   //重置工作空间 到数据准备订单位置

            //  元数据提取入库
            noaaImportOrder.Tasks.Add(new ITInsertNOAAMetaData());
            noaaImportOrder.TaskParams.Add(new string[] { SourceFilePath });

            //设置文件在数据阵列里面的路径,\\172.16.0.1\综合数据库\实验验证库\NOAA\NOAA-15(K)\AVHRR\20031203\数据名称\
            MetaDataReader mdReader = new MetaDataReader();
            MetaDataNOAA metNoaa = mdReader.ReadMetaDataNOAA(SourceFilePath);
            string dir = String.Format("{0}实验验证数据库\\NOAA\\{1}\\{2}\\{3}\\{4}\\{5}\\{6}\\", StoragePath.StoreBasePath, metNoaa.Satellite, metNoaa.Sensor, string.Format("{0:0000}", metNoaa.StartDate.Year), string.Format("{0:00}", metNoaa.StartDate.Month), string.Format("{0:00}", metNoaa.StartDate.Day), Path.GetFileNameWithoutExtension(SourceFilePath));
            
            //创建图片
            noaaImportOrder.Tasks.Add(new ITCreateNOAABitMap());
            noaaImportOrder.TaskParams.Add(new string[] { SourceFilePath, dir + Path.GetFileNameWithoutExtension(SourceFilePath)+".JPG" });
            //创建缩略图
            noaaImportOrder.Tasks.Add(new ITCreateThumbnail());
            noaaImportOrder.TaskParams.Add(new string[] { dir + Path.GetFileNameWithoutExtension(SourceFilePath) + ".JPG", dir + Path.GetFileNameWithoutExtension(SourceFilePath) + "-THUMB.JPG" });
            //  源数据文件入库
            noaaImportOrder.Tasks.Add(new ITCopyFiles());
            noaaImportOrder.TaskParams.Add(new string[] { SourceFilePath, dir + Path.GetFileName(SourceFilePath) });


            return noaaImportOrder;
        }

        /// <summary>
        /// 创建批量的noaa数据导入订单
        /// </summary>
        /// <param name="orderworkspaceDiractory">要倒入的文件夹所存放的路径</param>
        /// <status1>设置批量订单的状态，默认为挂起，以等待完成数据准备</status1>
        /// <returns></returns>
        public List<OrderClass> CreateBatchNoaaImportOrder(string orderworkspaceDiractory, EnumOrderStatusType status1 = EnumOrderStatusType.Suspended)
        {
            List<OrderClass> orderClass = new List<OrderClass>();
            string[] dataPaths = Directory.GetDirectories(orderworkspaceDiractory);
            for (int i = 0; i < dataPaths.Length; i++)
            {
                string[] datasources = Directory.GetFiles(dataPaths[i]);
                if ((datasources.Length > 0) && (datasources[0].EndsWith(".tar.gz") || datasources[0].EndsWith(".TAR.GZ") || datasources[0].EndsWith(".TAR.gz")))
                {
                    string str = dataPaths[i];
                    orderClass.Add(CreateNoaaImportOrder(str, status1));
                }
            }
            return orderClass;
        }

        /// <summary>
        /// Installed Order for noaa Data Preparing
        /// OrderParams描述
        /// orderworkspace 为待导入数据存放路径
        /// </summary>
        /// <param name="SourceFilePath"></param>
        /// <returns></returns>
        public OrderClass CreateNoaaImportOrder(string orderworkspace, EnumOrderStatusType status1)
        {
            string SourceFilePath = Directory.GetFiles(orderworkspace)[0];
            OrderClass noaaImportOrder = new OrderClass(status1);
            noaaImportOrder.Type = EnumOrderType.Installed;
            noaaImportOrder.OrderCode = OrderManager.GetNewCode();

            //设置任务,任务赋参
            //  日志消息
            noaaImportOrder.Tasks.Add(new ITOutputLoginfo());
            noaaImportOrder.TaskParams.Add(new string[] { "开始订单任务！" });
            //  创建文件夹工作空间
            noaaImportOrder.Tasks.Add(new ITCreateNoProcessWorkspace());
            noaaImportOrder.TaskParams.Add(new string[] { "" });

            //参数：SourceFilePath
            noaaImportOrder.OrderParams = new string[] { SourceFilePath, orderworkspace };
            noaaImportOrder.SetOrderWorkspace = orderworkspace;   //重置工作空间 到数据准备订单位置

            //  源数据文件入库
            noaaImportOrder.Tasks.Add(new ITCopyFiles());
            noaaImportOrder.TaskParams.Add(new string[] { noaaImportOrder.OrderWorkspace, SourceFilePath });

            //  元数据提取入库
            noaaImportOrder.Tasks.Add(new ITInsertNOAAMetaData());
            noaaImportOrder.TaskParams.Add(new string[] { SourceFilePath });

            return noaaImportOrder;
        }
    }
}
