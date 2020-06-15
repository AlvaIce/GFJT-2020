using System.IO;
using log4net;

namespace QRST_DI_DataImportTool.DataImport.FileData
{
    public static class FilesCopy
    {
        static ILog log = LogManager.GetLogger(typeof(FilesCopy));

        public static void CopyFile(string srcPath,string destPath)
        {
            log.Info(string.Format("开始导入文件：{0}",srcPath));
            File.Copy(srcPath, destPath, true);
            log.Info(string.Format("完成导入文件：{0}", srcPath));
        }

        public static void CopyFiles(string srcDir,string destDir)
        {
            if(Directory.Exists(destDir))
            {
                Directory.Delete(destDir, true);
            }
            Directory.CreateDirectory(destDir);
            string[] files = Directory.GetFiles(srcDir);
            for (int i = 0; i < files.Length;i++ )
            {
                CopyFile(files[i], string.Format(@"{0}\{1}", destDir, Path.GetFileName(files[i])));
            }
        }
    }
}
