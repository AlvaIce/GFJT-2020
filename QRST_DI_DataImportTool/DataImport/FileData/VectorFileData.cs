using System;
using System.IO;

namespace QRST_DI_DataImportTool.DataImport.FileData
{
    public class VectorFileData:FileData
    {
        public VectorFileData(string _filePath)
            : base(_filePath)
        {
            
        }

        public VectorFileData() { }
        public override bool FileCheck()
        {
            return true;
        }

        public override void FileCopy(string destDir)
        {
            FilesCopy.CopyFiles(fileDirectory, destDir);
        }

        public override string GetMetaDataFile()
        {

            string[] files = Directory.GetFiles(fileDirectory,"*.shp");
            if (0 == files.Length)
            {
                throw new Exception("没能找到元数据文件");
            }
            else
                return files[0];
        }

        public override string GetFileFilter() { return "矢量数据|*.shp"; }
    }
}
