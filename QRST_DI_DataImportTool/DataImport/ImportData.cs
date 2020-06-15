using System;
using System.Collections.Generic;
using QRST_DI_DS_Metadata.Paths;
using log4net;

namespace QRST_DI_DataImportTool.DataImport
{
    public class ImportData
    {
        ILog log = LogManager.GetLogger(typeof(ImportData));

        public AbstractDataImportFactory dataImportFactory;
  
        List<SingleData> singleDataLst = new List<SingleData>();

        public ImportData(DataType dataType)
        {
            if (DataType.UserDocument == dataType)
            {
                dataImportFactory = new DocumentImportFactory();
            }
            else if (DataType.UserRaster == dataType)
            {
                dataImportFactory = new UserRasterImportFactory();
            }
            else if (DataType.UserToolKit == dataType)
            {
                dataImportFactory = new UserToolKitImportFactory();
            }
            else if (DataType.Vector == dataType)
            {
                dataImportFactory = new VectorImportFactory();
            }
            else
                throw new Exception("找不到指定的数据导入类型！");
        }

        public void Add(SingleData _singleData)
        {
            singleDataLst.Add(_singleData);
        }

        public void AddRange(List<SingleData> _singleDataLst)
        {
            singleDataLst.AddRange(_singleDataLst);
        }

        public void ClearAll()
        {
            singleDataLst.Clear();
        }

        public void DataImport()
        {
            foreach (var temp in singleDataLst)
            {
                try
                {
                    log.Info(string.Format("###########开始导入数据{0}###############", temp.fileData.GetFilePath()));
                    temp.DataImport();
                    log.Info(string.Format("数据导入成功：{0}！",temp.fileData.GetFilePath()));
                }
                catch(Exception ex)
                {
                    log.Error(string.Format("数据导入失败:{0}！", temp.fileData.GetFilePath()), ex);
                }
            }
        }

        public List<SingleData> GetImportDataLst()
        {
            return singleDataLst;
        }
        
    }

    public class SingleData
 {
        public SingleData(FileData.FileData _fileData, MetaData.MetaData _metaData)
        {
            fileData = _fileData;
            metaData = _metaData;
        }
        public FileData.FileData fileData;
        public MetaData.MetaData metaData;

        public static string GetDestPath(string qrstcode)
        {
            string tableCode = StoragePath.GetTableCodeByQrstCode(qrstcode);
            StoragePath storePath = new StoragePath(tableCode);
            return storePath.GetDataOldPathForTools(qrstcode);
        }

        /// <summary>
        /// 执行单文件数据入库
        /// </summary>
        public void DataImport()
        {
            metaData.ImportData();
            string destPath = SingleData.GetDestPath(metaData.QRST_CODE);
            fileData.FileCopy(destPath);
        }

        public override string ToString()
        {
            return fileData.GetFilePath();
        }
 }

    public enum DataType 
    {
         Vector = 0,              //矢量数据 
         UserDocument = 1,  //文档数据
         UserRaster = 2,       //栅格数据
         UserToolKit = 3,      //用户工具
       
    };
}
