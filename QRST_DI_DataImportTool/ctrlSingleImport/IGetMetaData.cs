using QRST_DI_DataImportTool.DataImport.MetaData;
using QRST_DI_DataImportTool.DataImport.FileData;

namespace QRST_DI_DataImportTool.ctrlSingleImport
{
    public interface IGetMetaData
    {
        MetaData GetMetaData(string groupCode);

        void DisplayMetaData(MetaData metadata);

        void GetFileObj(FileData filedata);

         bool Check();
    }
}
