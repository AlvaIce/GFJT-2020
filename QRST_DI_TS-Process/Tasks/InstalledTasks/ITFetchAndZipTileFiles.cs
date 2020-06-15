using System;
using System.IO;
using ICSharpCode.SharpZipLib.Zip;
using ICSharpCode.SharpZipLib.Checksums;
using QRST_DI_TS_Basis.DirectlyAddress;
 
namespace QRST_DI_TS_Process.Tasks.InstalledTasks
{
    /// <summary>
    /// 将查询到的瓦片数据进行打包操作
    /// @zhangfeilong
    /// </summary>
    public class ITFetchAndZipTileFiles : TaskClass
    {
        /// <summary>
        /// 任务名,定义唯一标识，不可动态修改
        /// </summary>
        public override string TaskName
        {
            get { return "ITFetchAndZipTileFiles"; }
            set { }
        }

        public override void Process()
        {
            this.ParentOrder.Logs.Add(string.Format("开始打包瓦片数据。"));
            string tileFileNames = this.ProcessArgu[0];
            string originalOrderCode = this.ProcessArgu[1];
            string resultZipFile = StorageBasePath.StorePath_ResultTileZipPath();
            string strZip = Path.Combine(resultZipFile, originalOrderCode + ".gff_temp");//zip_temp
            //string strZip2 = Path.Combine(resultZipFile, originalOrderCode + ".zip");
            ZipOutputStream s = new ZipOutputStream(File.Create(strZip));
            string[] tiles = tileFileNames.Split('$');
            DirectlyAddressing da = new DirectlyAddressing(DirectlyAddressingIPMod.IPModDataSet);
            foreach (string tile in tiles)
            {
                if (tile != "")
                {
                    zip(da.GetPathByFileName(tile + ".png"), s, strZip);
                    string pgwpath = da.GetPathByFileName(tile + ".pgw");
                    if (File.Exists(pgwpath))
                    {
                        zip(pgwpath, s, strZip);
                    }
                    for (int i = 1; i < 5;i++ )
                    {
                        zip(da.GetPathByFileName(string.Format("{0}-{1}.tif",tile,i)), s, strZip);
                    }
                }
            }
            s.Close();
            //rename
            renameTileFile(strZip);
            this.ParentOrder.Logs.Add(string.Format("完成打包瓦片数据。"));
        }

        public void renameTileFile(string path)
        {
            FileInfo file = new FileInfo(path);
            string resultPath = path.Substring(0, path.LastIndexOf("_"));
            file.MoveTo(resultPath);
        }

        public void ZipFile(string strFile, string strZip)
        {
            if (strFile[strFile.Length - 1] != Path.DirectorySeparatorChar)
                strFile += Path.DirectorySeparatorChar;
            ZipOutputStream s = new ZipOutputStream(File.Create(strZip));
            s.SetLevel(6); // 0 - store only to 9 - means best compression
            zip(strFile, s, strFile);
            s.Finish();
            s.Close();
        }


        private void zip(string strFile, ZipOutputStream s, string staticFile)
        {
            if(File.Exists(strFile))
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
