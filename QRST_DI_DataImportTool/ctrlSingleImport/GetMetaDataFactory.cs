using QRST_DI_DataImportTool.DataImport;

namespace QRST_DI_DataImportTool.ctrlSingleImport
{
    public class GetMetaDataFactory
    {
        public static IGetMetaData createCtrlGetMetadata(DataType dataType)
        {
            if (DataType.Vector == dataType)
            {
                return new ctrlVectorMetaData();
            }
            else if(DataType.UserToolKit== dataType)
            {
                return new ctrlUserToolkitMetaData();
            }
                else if(DataType.UserDocument == dataType)
            {
                return new ctrlDocumentMetaData();
                }
            else if (DataType.UserRaster== dataType)
            {
                return new ctrlRasterMetaData();
            }
            else
                return null;
        }
    }
}
