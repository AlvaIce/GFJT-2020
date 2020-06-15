using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using QRST_DI_TS_Process.Orders;
using QRST_DI_TS_Process.Tasks;
using System.IO;
using QRST_DI_DS_Basis.DBEngine;
using QRST_DI_TS_Basis.DirectlyAddress;
using QRST_DI_TS_Process.Tasks.InstalledTasks;
using QRST_DI_Resources;

namespace QRST_DI_TS_Process.Orders.InstalledOrders
{
    public class IOHJdataPrepare 
    {
        /// <summary>
        /// Installed Order for HJ Data Preparing
        /// OrderParams描述
        /// OrderParams[0]为待导入数据存放路径
        /// </summary>
        /// <param name="SourceFilePath"></param>
        /// <returns></returns>
        public OrderClass CreateHJdataPrepareOrder(string SourceFilePath)
        {
            OrderClass HJdataPrepareOrder = new OrderClass();
            //参数：SourceFilePath
            HJdataPrepareOrder.OrderParams = new string[] { SourceFilePath };
            HJdataPrepareOrder.Type = EnumOrderType.Installed;
            HJdataPrepareOrder.OrderCode = OrderManager.GetNewCode();

            //设置任务,任务赋参
            //  日志消息
            HJdataPrepareOrder.Tasks.Add(new ITOutputLoginfo());
            HJdataPrepareOrder.TaskParams.Add(new string[] { "开始订单任务！" });
            //  创建文件夹工作空间
            HJdataPrepareOrder.Tasks.Add(new ITCreateHJDataInputWSP());
            HJdataPrepareOrder.TaskParams.Add(new string[] {""});
            
            //  源数据文件拷贝
            HJdataPrepareOrder.Tasks.Add(new ITCopyFiles());
            HJdataPrepareOrder.TaskParams.Add(new string[] { SourceFilePath,
                string.Format(@"{0}{1}", HJdataPrepareOrder.OrderWorkspace, System.IO.Path.GetFileName(SourceFilePath)) });
            //  文件解压
            HJdataPrepareOrder.Tasks.Add(new ITUnzipHJtargz());
            HJdataPrepareOrder.TaskParams.Add(new string[] { SourceFilePath, StorageBasePath.SharePath_OrignalData(HJdataPrepareOrder.OrderWorkspace)});
            //  元数据文件拷贝
            HJdataPrepareOrder.Tasks.Add(new ITCopyFiles());
            string sourceFileNameWithoutExt = Path.GetFileNameWithoutExtension(Path.GetFileNameWithoutExtension(SourceFilePath));    //.tar.gz 去两次
            HJdataPrepareOrder.TaskParams.Add(new string[] { string.Format(@"{0}{1}.XML", StorageBasePath.SharePath_OrignalData(HJdataPrepareOrder.OrderWorkspace), sourceFileNameWithoutExt),
            string.Format(@"{0}{1}.XML", StorageBasePath.SharePath_CorrectedData(HJdataPrepareOrder.OrderWorkspace), sourceFileNameWithoutExt)});
            //图片压缩,不需要啊，本身已经包含缩略图了,需要查找路径的话，根据命名规则可以找到图片的啊。。。所以先注释了，顾昱骅
            //HJdataPrepareOrder.Tasks.Add(new ITCreateThumbnail());
            //HJdataPrepareOrder.TaskParams.Add(new string[] { string.Format(@"{0}{1}.JPG", StorageBasePath.SharePath_OrignalData(HJdataPrepareOrder.OrderWorkspace), sourceFileNameWithoutExt),
            //string.Format(@"{0}{1}.JPG", StorageBasePath.SharePath_OrignalData(HJdataPrepareOrder.OrderWorkspace), sourceFileNameWithoutExt+"-THUMB")});
            //  告知预处理
            HJdataPrepareOrder.Tasks.Add(new ITSendMessage());
            HJdataPrepareOrder.TaskParams.Add(new string[] { Constant.CorrectRecieveIP,
               string.Format("{0}?{1}?@NoticePreprocess", HJdataPrepareOrder.OrderCode, StorageBasePath.SharePath_OrignalData(HJdataPrepareOrder.OrderWorkspace))});
            //  日志消息
            HJdataPrepareOrder.Tasks.Add(new ITOutputLoginfo());
            HJdataPrepareOrder.TaskParams.Add(new string[] { "已完成数据准备，开始产品生产。" });

            return HJdataPrepareOrder;

        }

        /// <summary>
        /// 创建批量环境星数据的准备订单
        /// </summary>
        /// <param name="sourceFileDirectory">环境星数据的存放文件夹</param>
        /// <returns></returns>
        public List<OrderClass> CreateBatchHJdataPrepareOrder(string sourceFileDirectory)
        {
            List<OrderClass> orderClassLst = new List<OrderClass>();
            string [] sourceFiles = Directory.GetFiles(sourceFileDirectory);
            for (int i = 0 ; i < sourceFiles.Length ;i++ )
            {
                if (sourceFiles[i].EndsWith(".tar.gz") || sourceFiles[i].EndsWith(".TAR.GZ") || sourceFiles[i].EndsWith(".TAR.gz"))
                  orderClassLst.Add(CreateHJdataPrepareOrder(sourceFiles[i]));
            }
            return orderClassLst;
        }

    }
}
