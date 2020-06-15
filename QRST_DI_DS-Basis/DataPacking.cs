using System;
using System.IO;
using ICSharpCode.SharpZipLib.Zip;
using ICSharpCode.SharpZipLib.Checksums;
using ICSharpCode.SharpZipLib.GZip;
using ICSharpCode.SharpZipLib.Tar;
 
namespace QRST_DI_DS_Basis
{
    //数据打包
    public class DataPacking
    {
        /// <summary>
        /// 压缩文件夹
        /// </summary>
        /// <param name="strFile">文件夹路径</param>
        /// <param name="strZip">压缩后的目标文件名称</param>
        public static void ZipFile(string strFile, string strZip)
        {
            if (strFile[strFile.Length - 1] != Path.DirectorySeparatorChar)
                strFile += Path.DirectorySeparatorChar;
            ZipOutputStream s = new ZipOutputStream(File.Create(strZip));
            s.SetLevel(6); // 0 - store only to 9 - means best compression
            zip(strFile, s, strFile);
            s.Finish();
            s.Close();
        }



        /// <summary>
        /// 压缩文件夹
        /// </summary>
        /// <param name="strFile">多个文件或文件夹</param>
        /// <param name="strZip">压缩后的目标文件名称</param>
        public static void ZipFile(string[] strFile, string strZip)
        {

            //zsm
            ZipOutputStream s = new ZipOutputStream(File.Create(strZip));
            s.SetLevel(6); // 0 - store only to 9 - means best compression
            Crc32 crc = new Crc32();

            foreach (string file in strFile)
            {

                if (File.Exists(file))
                {
                    //打开压缩文件
                    FileStream fs = File.OpenRead(file);

                    byte[] buffer = new byte[fs.Length];
                    fs.Read(buffer, 0, buffer.Length);
                    string tempfile = file.Substring(file.LastIndexOf("\\") + 1);
                    ZipEntry entry = new ZipEntry(tempfile) { DateTime = DateTime.Now, Size = fs.Length };
                    fs.Close();
                    crc.Reset();
                    crc.Update(buffer);
                    entry.Crc = crc.Value;
                    s.PutNextEntry(entry);

                    s.Write(buffer, 0, buffer.Length);
                }
                else
                {
                    string nfile = file;
                    if (nfile[nfile.Length - 1] != Path.DirectorySeparatorChar)
                        nfile += Path.DirectorySeparatorChar;

                    if (Directory.Exists(nfile))
                    {
                        zip(nfile, s, nfile.TrimEnd(Path.DirectorySeparatorChar));
                    }
                }

            }
            s.Finish();
            s.Close();
        }



        /// <summary>
        /// 压缩文件
        /// </summary>
        /// <param name="strFile"></param>
        /// <param name="s"></param>
        /// <param name="staticFile"></param>
        public static void zip(string strFile, ZipOutputStream s, string staticFile)
        {
            if (strFile[strFile.Length - 1] != Path.DirectorySeparatorChar) strFile += Path.DirectorySeparatorChar;
            Crc32 crc = new Crc32();
            string[] filenames = Directory.GetFileSystemEntries(strFile);
            foreach (string file in filenames)
            {
                if (Path.GetFileName(file) == "Thumbs.db")
                {
                    continue;
                }
                if (Directory.Exists(file))
                {
                    zip(file, s, staticFile);
                }

                else // 否则直接压缩文件
                {
                    //打开压缩文件
                    FileStream fs = File.OpenRead(file);

                    byte[] buffer = new byte[fs.Length];
                    fs.Read(buffer, 0, buffer.Length);
                    string tempfile = file.Substring(staticFile.LastIndexOf("\\") + 1);
                    ZipEntry entry = new ZipEntry(tempfile) { DateTime = DateTime.Now, Size = fs.Length };
                    fs.Close();
                    crc.Reset();
                    crc.Update(buffer);
                    entry.Crc = crc.Value;
                    s.PutNextEntry(entry);

                    s.Write(buffer, 0, buffer.Length);
                }
            }
        }

        /// <summary>
        /// 压缩为tar包
        /// </summary>
        /// <param name="fileName">压缩后的文件名</param>
        /// <param name="directory">压缩的文件夹</param>
        public static void CompressTarFile(string fileName, string directory)
        {
            Stream outStream = File.OpenWrite(fileName);
            outStream = new GZipOutputStream(outStream);
            TarArchive archive = TarArchive.CreateOutputTarArchive(outStream, TarBuffer.DefaultBlockFactor);
            String[] files = Directory.GetFiles(directory);
            foreach (String name in files)
            {
                TarEntry entry = TarEntry.CreateEntryFromFile(name);
                archive.WriteEntry(entry, true);
            }
            if (archive != null)
            {
                archive.CloseArchive();
            }
        }

        /// <summary>
        /// 解压文件
        /// </summary>
        /// <param name="TargetFile">待解压的文件："D:\\zip\\b.zip"</param>
        /// <param name="fileDir">解压后文件的放置目录： "D:\\unzipped\\"</param>
        /// <returns></returns>
        public static string unZipFile(string TargetFile, string fileDir)
        {
            string rootFile = " ";
            try
            {
                //读取压缩文件(zip文件)，准备解压缩
                ZipInputStream s = new ZipInputStream(File.OpenRead(TargetFile.Trim()));
                ZipEntry theEntry;
                string path = fileDir;
                //解压出来的文件保存的路径

                string rootDir = " ";
                //根目录下的第一个子文件夹的名称
                while ((theEntry = s.GetNextEntry()) != null)
                {
                    rootDir = Path.GetDirectoryName(theEntry.Name);
                    //得到根目录下的第一级子文件夹的名称
                    if (rootDir.IndexOf("\\") >= 0)
                    {
                        rootDir = rootDir.Substring(0, rootDir.IndexOf("\\") + 1);
                    }
                    string dir = Path.GetDirectoryName(theEntry.Name);
                    //根目录下的第一级子文件夹的下的文件夹的名称
                    string fileName = Path.GetFileName(theEntry.Name);
                    //根目录下的文件名称
                    if (dir != " ")
                    //创建根目录下的子文件夹,不限制级别
                    {
                        if (!Directory.Exists(fileDir + "\\" + dir))
                        {
                            path = fileDir + "\\" + dir;
                            //在指定的路径创建文件夹
                            Directory.CreateDirectory(path);
                        }
                    }
                    else if (dir == " " && fileName != "")
                    //根目录下的文件
                    {
                        path = fileDir;
                        rootFile = fileName;
                    }
                    else if (dir != " " && fileName != "")
                    //根目录下的第一级子文件夹下的文件
                    {
                        if (dir.IndexOf("\\") > 0)
                        //指定文件保存的路径
                        {
                            path = fileDir + "\\" + dir;
                        }
                    }

                    if (dir == rootDir)
                    //判断是不是需要保存在根目录下的文件
                    {
                        path = fileDir + "\\" + rootDir;
                    }

                    //以下为解压缩zip文件的基本步骤
                    //基本思路就是遍历压缩文件里的所有文件，创建一个相同的文件。
                    if (fileName != String.Empty)
                    {
                        FileStream streamWriter = File.Create(path + "\\" + fileName);

                        int size = 2048;
                        byte[] data = new byte[2048];
                        while (true)
                        {
                            size = s.Read(data, 0, data.Length);
                            if (size > 0)
                            {
                                streamWriter.Write(data, 0, size);
                            }
                            else
                            {
                                break;
                            }
                        }

                        streamWriter.Close();
                    }
                }
                s.Close();

                return rootFile;
            }
            catch (Exception ex)
            {
                return "1; " + ex.Message;
            }
        }
    }
}
