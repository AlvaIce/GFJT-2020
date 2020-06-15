/*
 * 作者：zxw
 * 创建时间：2013-09-02
 * 描述：文件夹工具类
*/
using System;
using System.IO;
using QRST_DI_Resources;
 
namespace QRST_DI_DS_Basis
{
    public class DirectoryUtil
    {
        /// <summary>
        /// 获取文件加大小,单位为字节
        /// </summary>
        /// <param name="dirPath">文件夹路径</param>
        /// <returns>返回文件夹长度</returns>
        public static long GetDirectoryLength(string dirPath)
        {
            //判断给定的路径是否存在,如果不存在则退出
            if (!Directory.Exists(dirPath))
                return 0;
            long len = 0;

            //定义一个DirectoryInfo对象
            DirectoryInfo di = new DirectoryInfo(dirPath);

            //通过GetFiles方法,获取di目录中的所有文件的大小
            foreach (FileInfo fi in di.GetFiles())
            {
                len += fi.Length;
            }

            //获取di中所有的文件夹,并存到一个新的对象数组中,以进行递归
            DirectoryInfo[] dis = di.GetDirectories();
            if (dis.Length > 0)
            {
                for (int i = 0; i < dis.Length; i++)
                {
                    len += GetDirectoryLength(dis[i].FullName);
                }
            }
            return len;
        }


        /// <summary>
        /// 获取文件夹所在驱动剩余空间大小
        /// </summary>
        /// <param name="dirPath"></param>
        /// <returns></returns>
        public static long GetDirectoryDriverSize(string dirPath)
        {
            DirectoryInfo dr = new DirectoryInfo(dirPath);
            string driverName = dr.Root.FullName;
            DriveInfo[] allDrives = DriveInfo.GetDrives();
            for (int i = 0; i < allDrives.Length;i++ )
            {
                if(allDrives[i].Name == driverName)
                {
                    return allDrives[i].AvailableFreeSpace;
                }
            }
            throw new Exception("无法获取驱动信息！");
        }

      //  public delegate void ProgressReportDel();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="srcPath"></param>
        /// <param name="destPath"></param>
        /// <param name="reportDel">报告拷贝进度 </param>
        public static void FileCopy(string srcPath,string destPath,out double progressPercent)
        {
            if(!File.Exists(srcPath))
            {
                throw new Exception("源文件不存在！");
            }
            progressPercent = 0;
            FileStream openFs = File.Open(srcPath, FileMode.Open);
            FileStream outFs = File.Create(destPath);
            long totalByte = openFs.Length;//文件的字节数
            int bSize = 1024;
            byte[] b = new byte[bSize];
            long totalDownloadByte = 0;
            int readCount = openFs.Read(b, 0, bSize);
            string s;//当前下载进度信息
            while (readCount > 0)
            {

                outFs.Write(b, 0, readCount);
                totalDownloadByte += readCount;

                progressPercent = (double)totalDownloadByte / (double)totalByte * 100;
                readCount = openFs.Read(b, 0, bSize);
            }

            openFs.Close();
            outFs.Close();
        }


        ///// <summary>
        ///// 删除文件加
        ///// </summary>
        ///// <param name="path"></param>
        //public static void DeleteFolder(string path)
        //{
        //    if (!Directory.Exists(path))
        //    {
        //        return;
        //    }
        //    else
        //    {
        //        string[] files = Directory.GetFiles(path);
        //        for (int i = 0; i < files.Length; i++)
        //        {
        //            FileInfo fi = new FileInfo(files[i]);
        //            fi.IsReadOnly = false;
        //            File.Delete(files[i]);
        //        }
        //        string[] folders = Directory.GetDirectories(path);
        //        for (int j = 0; j < folders.Length; j++)
        //        {
        //            DeleteFolder(folders[j]);
        //        }
        //        Directory.Delete(path, true);
        //    }
        //}

        /// <summary>
        /// 删除文件夹，遍历删除文件，跳过无法删除的文件继续执行
        /// </summary>
        /// <param name="dir">目标文件夹</param>
        /// <returns>是否全部删除成功</returns>
        public static bool DeleteDirTraversal(string dir)
        {
            bool issuccessful = true;

            //如果文件夹不存在，默认视为删除成功。
            if (!Directory.Exists(dir))
            {
                return issuccessful;
            }

            DirectoryInfo di = new DirectoryInfo(dir);
            DirectoryInfo[] cdis = di.GetDirectories();
            FileInfo[] files = di.GetFiles();
            foreach (FileInfo fi in files)
            {
                try
                {
                    fi.Attributes = FileAttributes.Normal;
                    File.Delete(fi.FullName);
                }
                catch (Exception ex)
                {
                    issuccessful = false;
                    MyConsole.WriteLine(ex.Message);
                }
            }

            foreach (DirectoryInfo cdi in cdis)
            {
                try
                {
                    bool rst = DeleteDirTraversal(cdi.FullName);

                    issuccessful = (rst) ? issuccessful : rst;
                }
                catch (Exception ex)
                {
                    issuccessful = false;
                    MyConsole.WriteLine(ex.Message);
                }
            }

            try
            {
                Directory.Delete(dir, true);
            }
            catch (Exception ex)
            {
                issuccessful = false;
                MyConsole.WriteLine(ex.Message);
            }

            return issuccessful;
        }


        /// <summary>
        /// 拷贝文件夹，遍历拷贝文件夹文件，跳过无法拷贝的文件，继续执行
        /// </summary>
        /// <param name="sourcedir">源文件夹</param>
        /// <param name="targetdir">目标文件夹</param>
        /// <returns>是否有部分文件没拷贝成功</returns>
        public static bool CopyDirTraversal(string sourcedir, string targetdir)
        {
            bool issuccessful = true;
            DirectoryInfo di = new DirectoryInfo(sourcedir);
            DirectoryInfo[] cdis = di.GetDirectories();
            FileInfo[] files = di.GetFiles();
            foreach (FileInfo fi in files)
            {
                try
                {
                    string filepath = Path.Combine(targetdir, fi.FullName.Substring(sourcedir.Length + 1));
                    Directory.CreateDirectory(Path.GetDirectoryName(filepath));
                    File.Copy(fi.FullName, filepath, true);
                }
                catch (Exception ex)
                {
                    issuccessful = false;
                    MyConsole.WriteLine(ex.Message);
                }
            }

            foreach (DirectoryInfo cdi in cdis)
            {
                try
                {
                    bool rst = CopyDirTraversal(cdi.FullName, Path.Combine(targetdir, cdi.FullName.Substring(sourcedir.Length + 1)));
                    issuccessful = (rst) ? issuccessful : rst;
                }
                catch (Exception ex)
                {
                    issuccessful = false;
                    MyConsole.WriteLine(ex.Message);
                }
            }

            return issuccessful;
        }

    }
}
