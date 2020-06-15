using QRST_DI_DataImportTool.DataImport.MetaData;
using QRST_DI_DataImportTool.DataImport.FileData;

namespace QRST_DI_DataImportTool.DataImport
{
    public class UserRasterImportFactory : AbstractDataImportFactory
    {
        public MetaData.MetaData CreateMetaData()
        {
            return new MetaDataUserRaster();
        }

        public FileData.FileData CreateFileData()
        {
            return new UserRasterFileData();
        }
    }
}
