using System;
using System.Collections.Generic;
using QRST_DI_TS_Process.Tasks.InstalledTasks;
using System.IO;
using QRST_DI_DS_Metadata;
using QRST_DI_DS_Metadata.MetaDataCls;
using QRST_DI_DS_Metadata.Paths;

namespace QRST_DI_TS_Process.Orders.InstalledOrders
{
    public class IOModisDataImport
    {
        public OrderClass CreateModisImportOrder(string SourceFilePath, string orderworkspace)
        {
            OrderClass modisImportOrder = new OrderClass();
            modisImportOrder.Type = EnumOrderType.Installed;
            modisImportOrder.OrderCode = OrderManager.GetNewCode();

            MetaDataReader mdReader = new MetaDataReader();
            MetaDataModis metModis = mdReader.ReadMetaDataModis(SourceFilePath);
            //设置任务,任务赋参
            //  日志消息
            modisImportOrder.Tasks.Add(new ITOutputLoginfo());
            modisImportOrder.TaskParams.Add(new string[] { "开始订单任务！" });

            //参数：SourceFilePath
            modisImportOrder.OrderParams = new string[] { SourceFilePath, orderworkspace };
            modisImportOrder.SetOrderWorkspace = orderworkspace;   //重置工作空间 到数据准备订单位置


            //缩略图和辅助文件下单
            string thumbPath;
            string baseDir = Path.GetDirectoryName(SourceFilePath);
            string ancillaryFile = "";
            string overViewFile = "";
            string[] temp = Path.GetFileNameWithoutExtension(SourceFilePath).Split('.');
            string filebase = "MOBRGB." + temp[1] + "." + temp[2] + "." + temp[3] + ".";
            string[] files = Directory.GetFiles(baseDir, filebase + "*.jpg");
            if (files.Length > 0) //同目录下存在缩略图文件
            {
                //插入元数据
                overViewFile = files[0];
                modisImportOrder.Tasks.Add(new ITInsertModisMetaData());
                modisImportOrder.TaskParams.Add(new string[] { overViewFile });

                //  源图片文件入库
                modisImportOrder.Tasks.Add(new ITCopyFiles());
                string dirthumb = String.Format("{0}实验验证数据库\\MODIS\\MOBRGB\\{1}\\{2}\\{3}\\{4}\\", StoragePath.StoreBasePath, string.Format("{0:0000}", metModis.BeginDate.Year), string.Format("{0:00}", metModis.BeginDate.Month), string.Format("{0:00}", metModis.BeginDate.Day), Path.GetFileNameWithoutExtension(overViewFile));
                modisImportOrder.TaskParams.Add(new string[] { overViewFile, dirthumb + Path.GetFileName(overViewFile) });

                //创建缩略图
                modisImportOrder.Tasks.Add(new ITCreateThumbnail());
                thumbPath = String.Format("{0}实验验证数据库\\MODIS\\MOBRGB\\{1}\\{2}\\{3}\\{4}\\{4}-THUMB.JPG", StoragePath.StoreBasePath, string.Format("{0:0000}", metModis.BeginDate.Year), string.Format("{0:00}", metModis.BeginDate.Month), string.Format("{0:00}", metModis.BeginDate.Day), Path.GetFileNameWithoutExtension(overViewFile));
                modisImportOrder.TaskParams.Add(new string[] { overViewFile, thumbPath });
            }
            string anciBase = "MOD03." + temp[1] + "." + temp[2] + "." + temp[3] + ".";
            string[] ancifiles = Directory.GetFiles(baseDir, anciBase + "*.hdf");
            if (ancifiles.Length > 0)  //同目录下存在辅助文件
            {
                ancillaryFile = ancifiles[0];
                //  元数据提取入库
                modisImportOrder.Tasks.Add(new ITInsertModisMetaData());
                modisImportOrder.TaskParams.Add(new string[] { ancillaryFile });

                //  源辅助数据文件入库
                modisImportOrder.Tasks.Add(new ITCopyFiles());
                //设置文件在数据阵列里面的路径,\\172.16.0.1\综合数据库\实验验证库\MODIS\MOD03\AVHRR\20031203\数据名称\
                string diranci = String.Format("{0}实验验证数据库\\MODIS\\{1}\\{2}\\{3}\\{4}\\{5}\\{6}\\", StoragePath.StoreBasePath, metModis.Satellite, metModis.Sensor, string.Format("{0:0000}", metModis.BeginDate.Year), string.Format("{0:00}", metModis.BeginDate.Month), string.Format("{0:00}", metModis.BeginDate.Day), Path.GetFileNameWithoutExtension(SourceFilePath));
                modisImportOrder.TaskParams.Add(new string[] { ancillaryFile, diranci + Path.GetFileName(ancillaryFile) });
            }

            //  元数据提取入库
            modisImportOrder.Tasks.Add(new ITInsertModisMetaData());
            modisImportOrder.TaskParams.Add(new string[] { SourceFilePath });

            //  源数据文件入库
            modisImportOrder.Tasks.Add(new ITCopyFiles());
            //设置文件在数据阵列里面的路径,\\172.16.0.1\综合数据库\实验验证库\MODIS\MOD03\AVHRR\2003\12\03\数据名称\
            string dir = String.Format("{0}实验验证数据库\\MODIS\\{1}\\{2}\\{3}\\{4}\\{5}\\{6}\\", StoragePath.StoreBasePath, metModis.Satellite, metModis.Sensor, string.Format("{0:0000}", metModis.BeginDate.Year), string.Format("{0:00}", metModis.BeginDate.Month), string.Format("{0:00}", metModis.BeginDate.Day), Path.GetFileNameWithoutExtension(SourceFilePath));
            modisImportOrder.TaskParams.Add(new string[] { SourceFilePath, dir + Path.GetFileName(SourceFilePath) });

            return modisImportOrder;
        }

        /// <summary>
        /// 创建批量的modis数据导入订单
        /// </summary>
        /// <param name="orderworkspaceDiractory">要倒入的文件夹所存放的路径</param>
        /// <status1>设置批量订单的状态，默认为挂起，以等待完成数据准备</status1>
        /// <returns></returns>
        public List<OrderClass> CreateBatchModisImportOrder(string orderworkspaceDiractory, EnumOrderStatusType status1 = EnumOrderStatusType.Suspended)
        {
            List<OrderClass> orderClass = new List<OrderClass>();
            string[] dataPaths = Directory.GetDirectories(orderworkspaceDiractory);
            for (int i = 0; i < dataPaths.Length; i++)
            {
                string[] datasources = Directory.GetFiles(dataPaths[i]);
                if ((datasources.Length > 0) && (datasources[0].EndsWith(".tar.gz") || datasources[0].EndsWith(".TAR.GZ") || datasources[0].EndsWith(".TAR.gz")))
                {
                    string str = dataPaths[i];
                    orderClass.Add(CreateModisImportOrder(str, status1));
                }
            }
            return orderClass;
        }

        /// <summary>
        /// Installed Order for Modis Data Preparing
        /// OrderParams描述
        /// orderworkspace 为待导入数据存放路径
        /// </summary>
        /// <param name="SourceFilePath"></param>
        /// <returns></returns>
        public OrderClass CreateModisImportOrder(string orderworkspace, EnumOrderStatusType status1)
        {
            string SourceFilePath = Directory.GetFiles(orderworkspace)[0];
            OrderClass modisImportOrder = new OrderClass(status1);
            modisImportOrder.Type = EnumOrderType.Installed;
            modisImportOrder.OrderCode = OrderManager.GetNewCode();

            //设置任务,任务赋参
            //  日志消息
            modisImportOrder.Tasks.Add(new ITOutputLoginfo());
            modisImportOrder.TaskParams.Add(new string[] { "开始订单任务！" });
            //  创建文件夹工作空间
            modisImportOrder.Tasks.Add(new ITCreateNoProcessWorkspace());
            modisImportOrder.TaskParams.Add(new string[] { "" });

            //参数：SourceFilePath
            modisImportOrder.OrderParams = new string[] { SourceFilePath, orderworkspace };
            modisImportOrder.SetOrderWorkspace = orderworkspace;   //重置工作空间 到数据准备订单位置

            //  源数据文件入库
            modisImportOrder.Tasks.Add(new ITCopyFiles());
            modisImportOrder.TaskParams.Add(new string[] { modisImportOrder.OrderWorkspace, SourceFilePath });

            //  元数据提取入库
            modisImportOrder.Tasks.Add(new ITInsertModisMetaData());
            modisImportOrder.TaskParams.Add(new string[] { SourceFilePath });

            return modisImportOrder;
        }
    }
}
