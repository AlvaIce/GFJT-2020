using System;
using System.Collections.Generic;
using System.IO;

namespace QRST_DI_DataImportTool.DataImport.FileData
{
    public abstract class FileData
    {
        protected string fileDirectory;

        public FileData(string _fileDir)
        {
            fileDirectory = _fileDir;
        }

        public FileData() { }

        public void SetfilePath(string _filePath)
        {

            if (File.Exists(_filePath))
                fileDirectory = Path.GetDirectoryName(_filePath);
            else
                fileDirectory = _filePath;
        }

        public List<string> GetAttacheMents()
        {
            List<string> childs = new List<string>();
            childs.AddRange(Directory.GetFiles(fileDirectory));
            childs.AddRange(Directory.GetDirectories(fileDirectory));
            return childs;
        }


        public string GetFilePath() { return fileDirectory; }

        public abstract string GetFileFilter();

        public abstract bool FileCheck();

        public abstract void FileCopy(string destDir);

        public virtual string GetMetaDataFile()
        {
            //在数据目录下寻找元数据文件，先匹配与目录同名的xml文件，若不存在，在找到目录中的第一个xml文件
            string targetxml = string.Format(@"{0}\{1}.xml", fileDirectory, Path.GetFileName(fileDirectory));
            if (File.Exists(targetxml))
            {
                return targetxml;
            }

            string[] files = Directory.GetFiles(fileDirectory, "*.xml");
            if (0 == files.Length)
            {
                throw new Exception("没能找到元数据文件");
            }
            else
                return files[0];
        }
    }
}
