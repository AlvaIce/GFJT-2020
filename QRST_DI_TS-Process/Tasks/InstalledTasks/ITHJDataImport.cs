using System;
using System.IO;
using QRST_DI_TS_Basis.DirectlyAddress;
using QRST_DI_DS_Metadata.MetaDataCls;
using QRST_DI_DS_Metadata.Paths;
using QRST_DI_SS_Basis;
using QRST_DI_SS_Basis.MetaData;
using QRST_DI_SS_DBInterfaces.IDBEngine;
using QRST_DI_Resources;

namespace QRST_DI_TS_Process.Tasks.InstalledTasks
{
    /// <summary>
    /// 环境星数据导入 20130903
    /// </summary>
    public class ITHJDataImport : TaskClass
    {
        private static IDbOperating _sqLiteOperating = Constant.IdbOperating;
        /// <summary>
        /// 任务名,定义唯一标识，不可动态修改
        /// </summary>
        public override string TaskName
        {
            get { return "ITHJDataImport"; }
            set { }
        }

        public override void Process()
        {

            //数据阵列 数据归档
            //  迁移数据至归档目录
            this.ParentOrder.Logs.Add(string.Format("开始源数据归档。"));
            string orderworkspace = ProcessArgu[0];                               //订单工作空间

            try
            {
                //提取元数据信息
                string[] xmlFiles = Directory.GetFiles(StorageBasePath.SharePath_OrignalData(orderworkspace), "*.xml");
                string[] thumbnailFiles = Directory.GetFiles(StorageBasePath.SharePath_OrignalData(orderworkspace), "*.jpg");
                string[] srcFiles = Directory.GetFiles(orderworkspace);
                string srcFile = "";
                if (srcFiles.Length == 0)
                {
                    throw new Exception("没有找到源文件！");
                }
                else
                {
                    srcFile = srcFiles[0];

                }
                if (xmlFiles.Length > 0)
                {
                    ParentOrder.Logs.Add("开始元数据入库！");
                    MetaDataHj metadataHJ = new MetaDataHj();
                    metadataHJ.ReadAttributes(xmlFiles[0]);
                    metadataHJ.NAME = Path.GetFileName(srcFile);
                    if (!metadataHJ.NAME.Contains("HJ"))
					{
						ParentOrder.Logs.Add("数据类型不匹配！");
						throw new Exception("数据类型不匹配！");
					}
                    IDbBaseUtilities _sqLiteBase = _sqLiteOperating.GetSubDbUtilities(EnumDBType.EVDB);
                    metadataHJ.ImportData(_sqLiteBase);
                    ParentOrder.Logs.Add("完成元数据入库！");

                    //导入原始数据和纠正后数据
                    string tableCode = StoragePath.GetTableCodeByQrstCode(metadataHJ.QRST_CODE);
                    StoragePath storePath = new StoragePath(tableCode);
                    string destPath = storePath.GetDataPath(metadataHJ.QRST_CODE);
                    if (Directory.Exists(destPath)) //删除旧的文件
                    {
                        Directory.Delete(destPath, true);
                    }
                    Directory.CreateDirectory(destPath);
                    //拷贝源文件
                    ParentOrder.Logs.Add("开始拷贝源文件！");
                    string srcdestPath = string.Format(@"{0}\{1}", destPath, metadataHJ.NAME);
                    if (!File.Exists(srcFile))
                    {
                        ParentOrder.Logs.Add("没有找到源文件！");

                    }
                    File.Copy(srcFile, srcdestPath, true);
                    ParentOrder.Logs.Add("完成拷贝源文件！");

                    ParentOrder.Logs.Add("开始拷贝缩略图！");
                    //拷贝缩略图
                    for (int i = 0; i < thumbnailFiles.Length; i++)
                    {
                        //ksk 添加if语句，功能是生成以QRST_CODE命名的jpg，集成共享使用。
                        if (destPath.Contains("zhsjk"))
                        {
                            if (thumbnailFiles[i].Contains("THUMB"))
                            {
                                string thumbbasePath = Path.Combine(StoragePath.StoreBasePath, "Thumb");
                                string thumbfilename = metadataHJ.QRST_CODE + ".jpg";   //默认值
                                string thumbFullPath = StoragePath.GetThumbPathByFileName(thumbbasePath, thumbfilename);
                                if (!Directory.Exists(thumbFullPath)) Directory.CreateDirectory(thumbFullPath);
                                try
                                {       //当快视图正被加载中时是无法覆盖的
                                    File.Copy(thumbnailFiles[i], Path.Combine(thumbFullPath, thumbfilename), true);
                                }
                                catch { }
                            }
                        }
                        string thumbnailDestPath = string.Format(@"{0}\{1}", destPath, Path.GetFileName(thumbnailFiles[i]));
                        try
                        {       //当快视图正被加载中时是无法覆盖的
                            File.Copy(thumbnailFiles[i], thumbnailDestPath, true);
                        }
                        catch { }
                    }
                    ParentOrder.Logs.Add("完成拷贝缩略图！");

                    this.ParentOrder.Tag = metadataHJ;
                    //拷贝校正后数据
                    //ParentOrder.Logs.Add("开始拷贝拷贝校正后数据！");
                    //string destCorrectedData = metadataGf1.GetCorrectedDataPath();
                    //if (Directory.Exists(destCorrectedData))
                    //{
                    //    Directory.Delete(destCorrectedData, true);
                    //}
                    //CopyFolder(StorageBasePath.SharePath_CorrectedData(orderworkspace), destCorrectedData);
                    //ParentOrder.Logs.Add("完成拷贝校正后数据！");
                }
                else
                {
                    throw new Exception("没有找到元数据.xml文件！");
                }

                //是否要删除订单文件夹
                // DeleteFolder(orderworkspace);
            }
            catch (Exception ex)
            {
                ParentOrder.Logs.Add(ex.ToString());
                throw ex;
            }

        }
    }
}
