using QRST_DI_TS_Basis.DirectlyAddress;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace QRST_DI_TS_Process.Tasks.InstalledTasks
{

    public class ITCreateGF6DataWorkspace : TaskClass
    {
        //数据库操作对象
        /*private IDbBaseUtilities postgresqlUtilities;
        private IDbOperating postgresqlOperating;*/

        /// <summary>
        /// 创建GF6数据入库订单工作空间任务名称
        /// </summary>
        public override string TaskName
        {
            get { return "ITCreateGF6DataWorkspace"; }
            set { }
        }

        /// <summary>
        /// ITCreateGFDataWorkspace任务执行过程逻辑
        /// </summary>
        public override void Process()
        {
            this.ParentOrder.Logs.Add(string.Format("建立工作空间。"));
            CreateGF6DataWorkspace();
            this.ParentOrder.Status = Orders.EnumOrderStatusType.Suspended;
        }

        /// <summary>
        /// 创建GF5数据入库订单工作空间
        /// </summary>
        protected void CreateGF6DataWorkspace()
        {
            //创建GF5数据入库订单工作空间
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
