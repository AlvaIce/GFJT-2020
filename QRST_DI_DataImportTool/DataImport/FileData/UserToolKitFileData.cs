using System.IO;

namespace QRST_DI_DataImportTool.DataImport.FileData
{
    public class UserToolKitFileData:FileData
    {
        public UserToolKitFileData(string _filePath)
            : base(_filePath)
        {
            
        }
        public UserToolKitFileData() { }

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



        public override string GetFileFilter() { return "用户工具|*.tar.gz;*.rar"; }
    }
}
