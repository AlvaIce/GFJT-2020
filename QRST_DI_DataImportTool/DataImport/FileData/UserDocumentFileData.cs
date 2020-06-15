using System.IO;
using log4net;

namespace QRST_DI_DataImportTool.DataImport.FileData
{
    public class UserDocumentFileData:FileData
    {
        ILog log = LogManager.GetLogger(typeof(UserDocumentFileData));

        public UserDocumentFileData(string _filePath)
            : base(_filePath)
        {
            
        }

        public UserDocumentFileData() { }

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

        public override string GetFileFilter() { return "文档数据|*.doc;*.xls;*.ppt;.pdf"; }
     
    }
}
