using System.IO;

namespace QRST_DI_DataImportTool.DataImport.FileData
{
    public class UserRasterFileData:FileData
    {
        public UserRasterFileData(string _filePath)
            : base(_filePath)
        {
            
        }

        public UserRasterFileData() { }

        public override bool FileCheck()
        {
            bool checkResult;
            if (!Directory.Exists(fileDirectory) || Directory.GetFiles(fileDirectory).Length == 0)
                checkResult = false;
            else
                checkResult = true;
            return checkResult;
        }

        public override void FileCopy(string destDir)
        {
            FilesCopy.CopyFiles(fileDirectory, destDir);
        }



        public override string GetFileFilter() { return "栅格数据|*.tar.gz"; }
    }
}
