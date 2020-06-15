using QRST_DI_Resources;
using QRST_DI_SS_Basis.MetaData;
using QRST_DI_SS_DBInterfaces.IDBEngine;
using QRST_DI_TS_Basis.DirectlyAddress;
using System.Data;
using System.IO;
using System.Threading;

namespace QRST_DI_TS_Process.Tasks.InstalledTasks
{
    /// <summary>
    /// 创建BCD数据入库订单工作空间任务
    /// </summary>
    public class ITCreateBCDDataWorkspace : TaskClass
    {
        //数据库操作对象
        //private IDbBaseUtilities postgresqlUtilities;
        //private IDbOperating postgresqlOperating;
        /// <summary>
        /// 创建BCD数据入库订单工作空间任务名称
        /// </summary>
        public override string TaskName
        {
            get { return "ITCreateBCDDataWorkspace"; }
            set { }
        }

        /// <summary>
        /// ITCreateBCDDataWorkspace任务执行过程逻辑
        /// </summary>
        public override void Process()
        {
            this.ParentOrder.Logs.Add(string.Format("建立工作空间。"));
            CreateBCDDataWorkspace();
            this.ParentOrder.Status = Orders.EnumOrderStatusType.Suspended;
        }

        /// <summary>
        /// 创建BCD数据入库订单工作空间
        /// </summary>
        protected void CreateBCDDataWorkspace()
        {
            //创建BCD数据入库订单工作空间
            if (!Directory.Exists(this.ParentOrder.OrderWorkspace))
            {
                Directory.CreateDirectory(this.ParentOrder.OrderWorkspace);
            }
            //创建BCD原始影像解压路径，存放影像解压后的文件
            if (!Directory.Exists(StorageBasePath.SharePath_OrignalData(this.ParentOrder.OrderWorkspace)))
            {
                Directory.CreateDirectory(StorageBasePath.SharePath_OrignalData(this.ParentOrder.OrderWorkspace));
            }

            if (!Directory.Exists(StorageBasePath.SharePath_CorrectedData(this.ParentOrder.OrderWorkspace)))
            {
                Directory.CreateDirectory(StorageBasePath.SharePath_CorrectedData(this.ParentOrder.OrderWorkspace));
            }

            if (!Directory.Exists(StorageBasePath.SharePath_TiledData(this.ParentOrder.OrderWorkspace)))
            {
                Directory.CreateDirectory(StorageBasePath.SharePath_TiledData(this.ParentOrder.OrderWorkspace));
            }

            if (!Directory.Exists(StorageBasePath.SharePath_Products(this.ParentOrder.OrderWorkspace)))
            {
                Directory.CreateDirectory(StorageBasePath.SharePath_Products(this.ParentOrder.OrderWorkspace));
            }
        }
    }
}
