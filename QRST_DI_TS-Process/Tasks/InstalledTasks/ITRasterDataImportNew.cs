using System;
using QRST_DI_DS_Metadata.MetaDataCls;
using QRST_DI_DS_Metadata.Paths;
using System.IO;
using QRST_DI_SS_Basis;
using QRST_DI_SS_Basis.MetaData;
using QRST_DI_SS_DBInterfaces.IDBEngine;
using QRST_DI_Resources;

namespace QRST_DI_TS_Process.Tasks.InstalledTasks
{
    public class ITRasterDataImportNew:TaskClass
    {
        /// <summary>
        /// 任务名,定义唯一标识，不可动态修改
        /// </summary>
        public override string TaskName
        {
            get { return "ITRasterDataImportNew"; }
            set { }
        }

        public override void Process()
        {

            string dataType = this.ProcessArgu[0];
            string sourceFileAddress = this.ProcessArgu[1];
            IDbOperating liteOperating = Constant.IdbOperating;
            IDbBaseUtilities baseUtilities = liteOperating.GetSubDbUtilities(EnumDBType.EVDB);
            MetaData metaData = null;
            try
            {
              //  this.ParentOrder.Logs.Add("开始解析数据类型！");

                try
                {
                    dataType = dataType.ToLower();
                    switch(dataType)
                    {
                        case "modis":
                            {
                                metaData = new MetaDataModis();
                                break;
                            }
                    }
                    if (metaData == null)
                    {
                        throw new Exception(String.Format("没有找到对应的入库类{0}型！", dataType));
                    }
                    else
                    {
                      this.ParentOrder.Logs.Add("开始解析元数据！");
                        metaData.ReadAttributes(sourceFileAddress);
                        metaData.ImportData(baseUtilities);
                       this.ParentOrder.Logs.Add("元数据入库完成！");
                        //将数据入库
                       this.ParentOrder.Logs.Add("开始导入数据！");
                        string tableCode = StoragePath.GetTableCodeByQrstCode(metaData.QRST_CODE);
                        StoragePath storePath = new StoragePath(tableCode);
                        string relatvePath = storePath.GetAddressRelateParts(metaData.QRST_CODE);
                        string destpath = string.Format(@"{0}{1}", StoragePath.StoreBasePath, metaData.GetRelateDataPath());
                        if (!Directory.Exists(destpath))
                        {
                            Directory.CreateDirectory(destpath);
                        }
                        //开始拷贝数据
                        File.Copy(sourceFileAddress, string.Format("{0}{1}", destpath, Path.GetFileName(sourceFileAddress)),true);
                         this.ParentOrder.Logs.Add("数据导入完成！");
                    }
                }
                catch(Exception ex)
                {
                    throw new Exception("数据入库失败！");
                }
            }
            catch (Exception ex)
            {
                //删除没能导入成功的数据残余
                if (metaData != null)
                {
                    MetaData.DeleteData(metaData.QRST_CODE);
                }
                throw ex;
            }
        }
    }
}
