using System;
using System.IO;
using QRST_DI_DS_Metadata.MetaDataCls;
using QRST_DI_DS_Metadata.Paths;
using QRST_DI_Resources;
using QRST_DI_SS_Basis;
using QRST_DI_SS_Basis.MetaData;
using QRST_DI_SS_DBInterfaces.IDBEngine;

namespace QRST_DI_TS_Process.Tasks.InstalledTasks
{
    public class ITImportUserProduct : TaskClass
    {
        /// <summary>
        /// 任务名,定义唯一标识，不可动态修改
        /// </summary>
        public override string TaskName
        {
            get { return "ITImportUserProduct"; }
            set { }
        }

        public override void Process()
        {
            string sourceFile = this.ProcessArgu[0];
            Exception exResult = null;    //用于记录任务是否发生异常

            MetaDataUserProduct metadataUserProduct = new MetaDataUserProduct();
            try
            {
                //检查产品文件是否存在
                this.ParentOrder.Logs.Add("开始解析产品文件夹！");
                string productDir = string.Format(@"{0}\{1}", sourceFile, StaticStrings.Products);
                if(!Directory.Exists(productDir))
                {
                    throw new Exception("产品文件夹不存在！");
                }
                //获取xml文件
                string []files = Directory.GetFiles(productDir);
                string xmlFile = "";
                for (int k = 0; k < files.Length;k++ )
                {
                    if (files[k].EndsWith(".xml") || files[k].EndsWith(".XML"))
                    {
                        xmlFile = files[k];
                        break;
                    }
                }
                if(string.IsNullOrEmpty(xmlFile))
                {
                    throw new Exception("找不到产品描述文件！");
                }

                //获取存放.tif的文件夹
                string []tifDir =  Directory.GetDirectories(productDir);
                string[] tifFile; 
                if (tifDir.Length == 0)
                {
                    throw new Exception("没能获取tif文件夹！");
                }
                tifFile = Directory.GetFiles(tifDir[0]);
                if(tifFile.Length == 0)
                {
                    throw new Exception("没能获得产品文件");
                }
                this.ParentOrder.Logs.Add("完成产品数据的解析！");
                //读取元数据信息
                this.ParentOrder.Logs.Add("开始提取产品详细信息！");
                metadataUserProduct.ReadAttributes(xmlFile);
                IDbOperating sqLiteOperating = Constant.IdbOperating;
                IDbBaseUtilities baseUtilities = sqLiteOperating.GetSubDbUtilities(EnumDBType.IPDB);
                metadataUserProduct.ImportData(baseUtilities);
                this.ParentOrder.Logs.Add("完成产品信息的提取！");

                //拷贝产品数据
                this.ParentOrder.Logs.Add("开始产品文件入库！");
                //获取数据应该存放的完整路径
                string tableCode = StoragePath.GetTableCodeByQrstCode(metadataUserProduct.QRST_CODE);
                StoragePath storePath = new StoragePath(tableCode);
                string destPath = storePath.GetDataPath(metadataUserProduct.QRST_CODE);

                if (!Directory.Exists(destPath))
                {
                    Directory.CreateDirectory(destPath);
                }
                for (int i = 0; i < tifFile.Length;i++ )
                {
                    string filename = Path.GetFileName(tifFile[i]);
                    string filePath = string.Format(@"{0}\{1}",destPath,filename );
                    File.Copy(tifFile[i],filePath);
                    this.ParentOrder.Logs.Add(string.Format("完成文件'{0}'的拷贝！",filename));
                }
                this.ParentOrder.Logs.Add("完成产品文件入库！");
            }
            catch (Exception ex)
            {
                exResult = ex;
                //删除未能导入成功的数据残余
                if (!string.IsNullOrEmpty(metadataUserProduct.QRST_CODE))
                {
                    MetaData.DeleteData(metadataUserProduct.QRST_CODE);
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
