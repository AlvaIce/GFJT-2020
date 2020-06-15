namespace QRST_DI_DataImportTool.DataImport
{
    public interface AbstractDataImportFactory
    {
        /// <summary>
        /// 创建元数据入库对象
        /// </summary>
        /// <returns></returns>
        MetaData.MetaData CreateMetaData();

        /// <summary>
        /// 创建文件数据入库对象
        /// </summary>
        /// <returns></returns>
         FileData.FileData CreateFileData();
    }
}
