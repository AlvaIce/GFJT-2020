using QRST_DI_DataImportTool.DataImport.MetaData;
using QRST_DI_DataImportTool.DataImport.FileData;

namespace QRST_DI_DataImportTool.DataImport
{
    public class UserToolKitImportFactory : AbstractDataImportFactory
    {
        public MetaData.MetaData CreateMetaData()
        {
            return new MetaDataUserToolKit();
        }

        public FileData.FileData CreateFileData()
        {
            return new UserToolKitFileData();
        }
    }
}
