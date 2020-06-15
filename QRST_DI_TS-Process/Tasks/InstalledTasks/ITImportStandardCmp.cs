using System;
using System.IO;
using QRST_DI_DS_Metadata.MetaDataCls;
using QRST_DI_DS_Metadata.Paths;
using QRST_DI_SS_Basis;
using QRST_DI_SS_Basis.MetaData;
using QRST_DI_SS_DBInterfaces.IDBEngine;
using QRST_DI_Resources;

namespace QRST_DI_TS_Process.Tasks.InstalledTasks
{
    /// <summary>
    /// 完成标准算法组件的入库
    /// </summary>
    public class ITImportStandardCmp : TaskClass
    {
        private static  IDbOperating _sqLiteOperating= Constant.IdbOperating;
        private IDbBaseUtilities _sqLiteBase;
        /// <summary>
        /// 任务名,定义唯一标识，不可动态修改
        /// </summary>
        public override string TaskName
        {
            get { return "ITImportStandardCmp"; }
            set { }
        }

        public override void Process()
        {
            string sourceFile = this.ProcessArgu[0];
            Exception exResult = null;    //用于记录任务是否发生异常
            string cmpFile = "";
            string soFile = "";
            string tarFile = "";
             MetaDataStandAlgCmp metadataStandCmp = new MetaDataStandAlgCmp();
            try
            {
   
                string[] fileArr = Directory.GetFiles(sourceFile);
                for (int i = 0; i < fileArr.Length;i++ )
                {
                    if(fileArr[i].EndsWith("so")||fileArr[i].EndsWith("SO"))
                    {
                        soFile = fileArr[i];
                    }
                    else if (fileArr[i].EndsWith("cmp") || fileArr[i].EndsWith("CMP"))
                    {
                        cmpFile = fileArr[i];
                    }
                    else if (fileArr[i].EndsWith("tar") || fileArr[i].EndsWith("TAR"))
                    {
                        tarFile = fileArr[i];
                    }
                }
                if(cmpFile == "" || tarFile == "")
                {
                    throw new Exception("数据文件不全！");
                }
                this.ParentOrder.Logs.Add("开始提取元数据！");
              
                metadataStandCmp.ReadAttributes(cmpFile);
                _sqLiteBase = _sqLiteOperating.GetSubDbUtilities(EnumDBType.MADB);
                if (metadataStandCmp.ExistInDb(_sqLiteBase))
                {
                    throw new Exception("该算法组件已经存在");
                }
                metadataStandCmp.ImportData(_sqLiteBase);
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
                string tarFileName = new FileInfo(tarFile).Name;

                //提取公共信息
                MetaDataStandardPublicInfo publicinfo = new MetaDataStandardPublicInfo();
                publicinfo.DataCode = metadataStandCmp.QRST_CODE;
                publicinfo.DataSize = new FileInfo(tarFile).Length / (1024 * 1024);
                MetaDataStandardPublicInfo.Add(publicinfo, _sqLiteBase);

                destPath = string.Format(@"{0}\{1}", destPath, tarFileName);
                File.Copy(tarFile, destPath);
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
                //是否发送消息
                //if(exResult == null)
                //{
                //}
            }
        }

    }
}
