using QRST_DI_DataImportTool.DataImport.MetaData;
using QRST_DI_DataImportTool.DataImport.FileData;

namespace QRST_DI_DataImportTool.DataImport
{
    public class VectorImportFactory : AbstractDataImportFactory
    {
          public MetaData.MetaData CreateMetaData()
        {
            return new MetaDataVector();
        }

        public FileData.FileData CreateFileData()
        {
            return new VectorFileData();
        }
    }
}
