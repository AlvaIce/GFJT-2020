using System;
using System.Collections.Generic;
using System.IO;
using QRST_DI_DS_Metadata.MetaDataCls;
using QRST_DI_DS_Metadata.Paths;
using QRST_DI_SS_Basis;
using QRST_DI_SS_Basis.MetaData;
using QRST_DI_SS_DBInterfaces.IDBEngine;
using QRST_DI_Resources;

namespace QRST_DI_TS_Process.Tasks.InstalledTasks
{
    public class ITImportStandWfl : TaskClass
    {
        private static IDbOperating _liteOperating = Constant.IdbOperating;
        private IDbBaseUtilities _baseUtilities;
        /// <summary>
        /// 任务名,定义唯一标识，不可动态修改
        /// </summary>
        public override string TaskName
        {
            get { return "ITImportStandWfl"; }
            set { }
        }

        public override void Process()
        {
            string sourceFile = this.ProcessArgu[0];
            Exception exResult = null;    //用于记录任务是否发生异常
            string wflFile = "";
            string sourcefile = "";
            List<string> cmpFile = new List<string>();
            try
            {
                string[] fileArr = Directory.GetFiles(sourceFile);
                for (int i = 0; i < fileArr.Length;i++ )
                {
                    if (fileArr[i].EndsWith(".wfl") || fileArr[i].EndsWith(".WFL"))
                    {
                        wflFile = fileArr[i];
                    }
                    else if(fileArr[i].EndsWith(".tar")||fileArr[i].EndsWith(".TAR"))
                    {
                        cmpFile.Add(fileArr[i]);
                    }
                }
                if(string.IsNullOrEmpty(wflFile)||cmpFile.Count == 0)
                {
                    throw new Exception("数据文件不全！");
                }
                //获取数据流程压缩包，即与流程文件wfl同名的tar包
                sourcefile = string.Format(@"{0}\{1}.tar", sourceFile,Path.GetFileNameWithoutExtension(wflFile)); 
                if(!File.Exists(sourcefile))
                {
                    throw new Exception("源数据文件没找到！");
                }
                

                this.ParentOrder.Logs.Add("开始提取元数据！");
                MetaDataStandProWfl metadataStandWfl = new MetaDataStandProWfl();
                metadataStandWfl.ReadAttributes(wflFile);
                _baseUtilities = _liteOperating.GetSubDbUtilities(EnumDBType.MADB);
                metadataStandWfl.ImportData(_baseUtilities);
                this.ParentOrder.Logs.Add("元数据导入成功！");
                this.ParentOrder.Logs.Add("开始导入算法流程压缩包！");
                //获取数据应该存放的完整路径
                string tableCode = StoragePath.GetTableCodeByQrstCode(metadataStandWfl.QRST_CODE);
                StoragePath storePath = new StoragePath(tableCode);
                string destPath = storePath.GetDataPath(metadataStandWfl.QRST_CODE);
                //拷贝文件
                if (!Directory.Exists(destPath))
                {
                    Directory.CreateDirectory(destPath);
                }
                string tarFileName = new FileInfo(sourcefile).Name;
                destPath = string.Format(@"{0}\{1}", destPath, tarFileName);
                File.Copy(sourcefile, destPath);
                this.ParentOrder.Logs.Add("流程文件导入完成！");
                //将包含的组件归档
                this.ParentOrder.Logs.Add("开始组件归档！");
                for (int j = 0; j < metadataStandWfl.algCmpLst.Count;j++ )
                {
                    if (metadataStandWfl.algCmpLst[j] != null) 
                    {
                        string tableCode1 = StoragePath.GetTableCodeByQrstCode(metadataStandWfl.algCmpLst[j].QRST_CODE);
                        StoragePath storePath1 = new StoragePath(tableCode1);
                        string destPath1 = storePath1.GetDataPath(metadataStandWfl.algCmpLst[j].QRST_CODE);
                        if (!Directory.Exists(destPath))
                        {
                            Directory.CreateDirectory(destPath);
                        }
                        destPath1 = string.Format(@"{0}\{1}.tar", metadataStandWfl.algCmpLst[j].AlgorithmName);
                        string srcPath = string.Format(@"{0}\{1}.tar", sourceFile, metadataStandWfl.algCmpLst[j].AlgorithmName);
                        if (File.Exists(srcPath))
                        {
                            File.Copy(srcPath,destPath1);
                            this.ParentOrder.Logs.Add(string.Format("完成流程组件'{0}'入库！", metadataStandWfl.algCmpLst[j].AlgorithmName));
                        }
                        else
                        {
                            this.ParentOrder.Logs.Add(string.Format("流程组件'{0}'入库失败！找不到指定的组件文件。",metadataStandWfl.algCmpLst[j].AlgorithmName));
                        }
                    }
                }
                this.ParentOrder.Logs.Add("完成组件归档！");
            }
            catch (Exception ex)
            {
                exResult = ex;
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
