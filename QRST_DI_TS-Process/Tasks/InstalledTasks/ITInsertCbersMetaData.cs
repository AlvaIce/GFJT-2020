using System;
using QRST_DI_DS_Metadata;
using QRST_DI_DS_Metadata.MetaDataCls;
using QRST_DI_TS_Process.Site;
using QRST_DI_DS_Metadata.Paths;
using System.IO;

namespace QRST_DI_TS_Process.Tasks.InstalledTasks
{
    public class ITInsertCbersMetaData:TaskClass
    {
        /// <summary>
        /// 任务名,定义唯一标识，不可动态修改
        /// </summary>
        public override string TaskName
        {
            get { return "ITInsertCbersMetaData"; }
            set { }
        }

        public override void Process()
        {
            string sourceFilePath = ProcessArgu[0];
            //  读xml 元数据
            this.ParentOrder.Logs.Add(string.Format("开始元数据入库。"));
            MetaDataReader mdReader = new MetaDataReader();
            MetaDataCbers metCbers = mdReader.ReadMetaDataCbers(sourceFilePath);
            //元数据入库
            string insertSql = "";
            MetaDataDBImporter mdDBImporter = new MetaDataDBImporter();
            mdDBImporter.ImportData(EnumMetadataTypes.CBERS, metCbers, sourceFilePath, TServerSiteManager.ConvertTSSiteIP2TSSiteCode(this.ParentOrder.TSSiteIP), out insertSql);

            //组成入库路径、导入源数据文件
            string dir = String.Format("{0}实验验证数据库\\CBERS\\{1}\\{2}\\{3}\\{4}\\{5}\\{6}\\", StoragePath.StoreBasePath, metCbers.Satellite, metCbers.Sensor, string.Format("{0:0000}", metCbers.SceneDate.Year), string.Format("{0:00}", metCbers.SceneDate.Month), string.Format("{0:00}", metCbers.SceneDate.Day), Path.GetFileNameWithoutExtension(Path.GetFileNameWithoutExtension(sourceFilePath)));
            string srcFilePath = sourceFilePath;
            string destFilePath = dir + Path.GetFileName(sourceFilePath);

            try
            {
                if (System.IO.File.Exists(srcFilePath))
                {

                    if (destFilePath != srcFilePath)
                    {
                        this.ParentOrder.Logs.Add(string.Format("正在拷贝数据..."));
                        string destDir = Path.GetDirectoryName(destFilePath);
                        if (!Directory.Exists(destDir))
                        {
                            Directory.CreateDirectory(destDir);
                        }
                        System.IO.File.Copy(srcFilePath, destFilePath, true);
                        this.ParentOrder.Logs.Add(string.Format("完成数据拷贝。"));
                    }
                }
            }
            catch (Exception ex)
            {
                this.ParentOrder.Logs.Add(string.Format("拷贝数据出现异常{0}", ex.Message));
                throw ex;
            }
            
            this.ParentOrder.Logs.Add(string.Format("完成数据入库。"));
        }
    }
}
