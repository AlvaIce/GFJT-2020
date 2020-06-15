using QRST_DI_DataImportTool.DataImport.MetaData;
using QRST_DI_DataImportTool.DataImport.FileData;

namespace QRST_DI_DataImportTool.DataImport
{
    public class DocumentImportFactory:AbstractDataImportFactory
    {
        public MetaData.MetaData CreateMetaData()
        {
            return new MetaDataUserDocument();
        }

        public FileData.FileData CreateFileData()
        {
            return new UserDocumentFileData();
        }
    }
}
