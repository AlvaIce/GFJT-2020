using System;
using QRST_DI_DS_Metadata.MetaDataCls;
using System.IO;
using QRST_DI_DS_Metadata.Paths;
using QRST_DI_SS_Basis;
using QRST_DI_SS_Basis.MetaData;
using QRST_DI_SS_DBInterfaces.IDBEngine;
using QRST_DI_Resources;

namespace QRST_DI_TS_Process.Tasks.InstalledTasks
{
    //导入zip压缩包的模型算法文件  zxw 20130618
    public class ITImportZipCmp : TaskClass
    {
        /// <summary>
        /// 任务名,定义唯一标识，不可动态修改
        /// </summary>
        public override string TaskName
        {
            get { return "ITImportZipCmp"; }
            set { }
        }


        public override void Process()
        {
            string sourceFile = this.ProcessArgu[0];
            Exception exResult = null;    //用于记录任务是否发生异常
            string cmpFile = "";
            MetaDataStandAlgCmp metadataStandCmp = new MetaDataStandAlgCmp();
            IDbOperating sqLiteOperating = Constant.IdbOperating;
            IDbBaseUtilities baseUtilities = sqLiteOperating.GetSubDbUtilities(EnumDBType.MADB);
            try
            {
                string [] files = Directory.GetFiles(this.ParentOrder.OrderWorkspace,"*.cmp",SearchOption.AllDirectories);
                if (files.Length == 0 )
                {
                    throw new Exception("没有找到元数据文件！");
                }
                cmpFile = files[0];
                this.ParentOrder.Logs.Add("开始提取元数据！");

                metadataStandCmp.ReadAttributes(cmpFile);
                if (metadataStandCmp.ExistInDb(baseUtilities))
                {
                    metadataStandCmp.DeleteMetData(baseUtilities);
                }
                metadataStandCmp.ImportData(baseUtilities);
                this.ParentOrder.Logs.Add("元数据导入成功！");
                this.ParentOrder.Logs.Add("开始导入算法组件压缩包！");
                //获取数据应该存放的完整路径
                string tableCode = StoragePath.GetTableCodeByQrstCode(metadataStandCmp.QRST_CODE);
                StoragePath storePath = new StoragePath(tableCode);
                string destPath = storePath.GetDataPath(metadataStandCmp.QRST_CODE);
                //拷贝文件
                if (!Directory.Exists(destPath))
                {
                    Directory.CreateDirectory(destPath);
                }
                string tarFileName = new FileInfo(sourceFile).Name;

                //提取公共信息
                MetaDataStandardPublicInfo publicinfo = new MetaDataStandardPublicInfo();
                publicinfo.DataCode = metadataStandCmp.QRST_CODE;
                publicinfo.DataSize = new FileInfo(sourceFile).Length / (1024 * 1024);
                MetaDataStandardPublicInfo.Add(publicinfo, baseUtilities);

                destPath = string.Format(@"{0}\{1}", destPath, tarFileName);
                File.Copy(sourceFile, destPath,true);
                this.ParentOrder.Logs.Add("数据导入完成！");
            }
            catch (Exception ex)
            {
                exResult = ex;
                //删除未能导入成功的数据残余
                if (!string.IsNullOrEmpty(metadataStandCmp.QRST_CODE))
                {
                    MetaData.DeleteData(metadataStandCmp.QRST_CODE);
                }
                throw ex;
            }
            finally
            {
                Directory.Delete(this.ParentOrder.OrderWorkspace,true);
            }
        }
    }
}
