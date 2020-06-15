using System;
using QRST_DI_DS_Metadata.Paths;
using System.IO;
using ICSharpCode.SharpZipLib.Zip;
using ICSharpCode.SharpZipLib.Checksums;
using QRST_DI_DS_Basis;

 
namespace QRST_DI_TS_Process.Tasks.InstalledTasks
{
    public class ITDownLoadDOC:TaskClass
    {
        public override string TaskName
        {
            get { return "ITDownLoadDOC"; }
            set { }
        }

        public override void Process()
        {
            string qrst_code = this.ProcessArgu[0];    //要下载数据的数据编码
            string destPath = this.ProcessArgu[1];     //目的地址
            string mouid = this.ProcessArgu[2];       //操作号ID
            Exception exResult = null;
            //string[] isFTPUPLoad = opID.Split('#');
            try
            {
                //解析数据编码
                this.ParentOrder.Logs.Add("开始解析数据编码！");
                //int index = qrst_code.IndexOf('-');
                //string industrycode = qrst_code.Substring(0, index);
                //if (industrycode != Constant.INDUSTRYCODE)
                //{
                //    throw new Exception("数据编码错误");0001-ISDB-10-13", @"\\192.168.10.17\sharefolder\1010010001", "334444
                //}
                //根据数据编码获取数据
                string tableCode = StoragePath.GetTableCodeByQrstCode(qrst_code);
                StoragePath storPath = new StoragePath(tableCode);
                         
                string sourcePath = storPath.GetDataPath(qrst_code);
                if (Directory.Exists(sourcePath))
                {
                    if (Directory.Exists(destPath))//destPath表示目的路径\\192.168.10.17\sharefolder\1010010005
                    {
                        DataPacking.ZipFile(sourcePath, string.Format(@"{0}\{1}.zip", destPath, mouid));
                    }
                    else
                    {
                        Directory.CreateDirectory(destPath);
                        DataPacking.ZipFile(sourcePath, string.Format(@"{0}\{1}.zip", destPath, mouid));                    
                    }
                }
                else
                {
                    throw new Exception("数据文档不存在！");
                }
               
                #region
                //string tableCode = StoragePath.GetTableCodeByQrstCode(code);
                //StoragePath storePath = new StoragePath(tableCode);
                //string destPath = storePath.GetDataPath(code);
                //Directory.CreateDirectory(destPath);             //需要判断路径是否存在我改天在该
                ////拷贝源文件

                //string[] srcFiles = Directory.GetFiles(destPath);
                //foreach (string src in srcFiles)
                //{
                //    string srcdestPath = string.Format(@"{0}\{1}", sharePath, Path.GetFileName(src));
                //    if (!File.Exists(src))
                //    {
                //        return "";
                //    }

                //    if (!File.Exists(srcdestPath))
                //        File.Copy(src, srcdestPath);

                //}
                //return "";
                #endregion

                #region   原来的数据下载
                //if (sourcePath != "" && Directory.Exists(sourcePath))      //数据目录存在
                //{
                //    //获取数据文件
                //    DirectoryInfo dirInfo = new DirectoryInfo(sourcePath);
                //    FileInfo[] fileinfo = dirInfo.GetFiles();
                //    if (fileinfo.Length <= 0)
                //    {
                //        throw new Exception("数据不存在！");
                //    }
                //    else
                //    {
                //sourcePath = string.Format(@"{0}\{1}", sourcePath, fileinfo[0].ToString());
                //destPath = string.Format(@"{0}\{1}", destPath, qrst_code);
                //Directory.CreateDirectory(destPath);
                //destPath = string.Format(@"{0}\{1}", destPath, fileinfo[0].ToString());
                //this.ParentOrder.Logs.Add("已经获取到源数据路径！");
                //this.ParentOrder.Logs.Add("开始下载数据！");
                //File.Copy(sourcePath, destPath);
                //this.ParentOrder.Logs.Add("完成数据下载！");                                           
                //    }
                //}
                //else
                //{
                //    throw new Exception("数据不存在！");
                //}
                #endregion
            }
            catch (Exception ex)
            {
                exResult = ex;
                this.ParentOrder.Logs.Add(ex.Message);                
            }
           
        }


        private void zip(string strFile, ZipOutputStream s, string staticFile)
        {
            if (File.Exists(strFile))
            {
                Crc32 crc = new Crc32();
                //string[] filenames = Directory.GetFileSystemEntries(strFile);
                FileStream fs = File.OpenRead(strFile);

                byte[] buffer = new byte[fs.Length];
                fs.Read(buffer, 0, buffer.Length);
                string tempfile = strFile.Substring(strFile.LastIndexOf("\\") + 1);
                ZipEntry entry = new ZipEntry(tempfile);

                entry.DateTime = DateTime.Now;
                entry.Size = fs.Length;
                fs.Close();
                crc.Reset();
                crc.Update(buffer);
                entry.Crc = crc.Value;
                s.PutNextEntry(entry);
                s.Write(buffer, 0, buffer.Length);
            }
        }
    }
}
