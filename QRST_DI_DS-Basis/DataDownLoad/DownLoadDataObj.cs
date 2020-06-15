/*
 * 作者：zxw
 * 创建时间：2013-09-02
 * 描述：数据下载对象
*/ 
using System;
using System.Collections.Generic;
using System.IO;

namespace QRST_DI_DS_Basis.DataDownLoad
{
    public class DownLoadDataObj
    {
        public bool isDir;          //判断下载的是否为文件

        public string srcPath;     //源数据路径，支持文件和文件夹下载
        public string destPath;    //目标数据路径,为目录

        public string status = "未下载";            //数据下载状态，分为未下载，下载完成，下载失败,正在下载
        public DateTime startTime;     //下载时间
        public DateTime lastTime;//已经下载的时间
        public long dataSize;                            //数据大小。单位为字节
        public long downloadedSize = 0;              //已经下载的大小

        public string downLoadMsg = "";                //记录下载信息

        public bool isTile = false;                    //针对多而小的切片数据批量下载的标记，默认为非切片 
        private List<string> tilesFilePath;            //存储切片数据地址的列表
   
        public DownLoadDataObj(string _srcPath,string _destPath)
        {
            srcPath = _srcPath;
            destPath = _destPath;

            if (Directory.Exists(srcPath))  //下载文件夹
            {
                dataSize = DirectoryUtil.GetDirectoryLength(srcPath);
                isDir = true;
            }
            else if (File.Exists(srcPath)) //下载文件
            {
                dataSize = new FileInfo(srcPath).Length;
                isDir = false;
            }
            //else
            //{
            //    status = "下载失败";
            //    downLoadMsg = "源文件不存在！";
            //}
        }

        public DownLoadDataObj(List<string> _tilesFilePath, string _destPath)
        {
            if (_tilesFilePath == null || _tilesFilePath.Count == 0)
            {
                //status = "下载失败";
                status = "下载失败";
                downLoadMsg = "源文件不存在！";
            }
            else
            {
                tilesFilePath = _tilesFilePath;
                srcPath = Path.GetFileName(_tilesFilePath[0]);      //???只下载第一个瓦片？ joki 170305
                destPath = _destPath;
                dataSize = (long)(1.45 * 1024 * 1024 * _tilesFilePath.Count);
                isTile = true;
                isDir = false;
            }
        }

        /// <summary>
        /// 将下载状态设置为正在下载
        /// </summary>
        public void setStatusDownloading()
        {
            //status = "正在下载";
            status = "正在下载";
        }

        public void Downdata()
        {
            status = "正在下载";
            if (isDir)  //下载文件夹
            {
                DownloadDirectory(destPath);
            }
            else  //下载文件
            {
                if(isTile)   //下载切片
                {
                    DownloadTiles();
                }
                else
                {
                    DownloadFile();
                }
            }
        }

        /// <summary>
        /// 下载切片
        /// </summary>
        private void DownloadTiles()
        {
            int failedTiles = 0;
            startTime = DateTime.Now;
            for (int i = 0; i < tilesFilePath.Count; i++)
            {
                srcPath = Path.GetFileName(tilesFilePath[i]);
                string tileDestPath = string.Format(@"{0}\{1}", destPath, Path.GetFileName(tilesFilePath[i]));
                try
                {
                    File.Copy(tilesFilePath[i], tileDestPath, true);
                }
                catch (Exception ex)
                {
                    failedTiles++;
                }
                downloadedSize += (long)(1.45 * 1024 * 1024);
                lastTime = DateTime.Now;
                //下载完成后，如果下载的是一次全覆盖的瓦片数据的GFF压缩包时，需要在共享目录下生成的gff包给删除掉
                FileInfo tempfi = new FileInfo(tilesFilePath[i]);
                if (tempfi.Name.ToUpper().EndsWith(".GFF"))
                {
                    File.Delete(tilesFilePath[i]);
                }
            }

            srcPath = string.Format("切片文件总数：{0}；下载成功数：{1}；下载失败数：{2}", tilesFilePath.Count, tilesFilePath.Count - failedTiles, failedTiles);//下载完成后在显示文件名的地方显示结果
            status = "下载完成";
        }

        /// <summary>
        /// 下载文件夹
        /// </summary>
        private void DownloadDirectory(string currentDownloadPath)
        {
            try
            {
                List<string> dirsList = new List<string>();
                List<string> srcList = new List<string>();
                startTime = DateTime.Now;
                string[] subFiles = Directory.GetFiles(srcPath);
                string[] subDirs = Directory.GetDirectories(srcPath);
                DirectoryInfo drinfo = new DirectoryInfo(srcPath);
                currentDownloadPath = string.Format(@"{0}\{1}", currentDownloadPath, drinfo.Name);
                if(Directory.Exists(currentDownloadPath))  //覆盖存在的同名文件加
                {
                    DirectoryUtil.DeleteDirTraversal(currentDownloadPath);
                }
                Directory.CreateDirectory(currentDownloadPath);

                //下载目录下的文件
                for (int i = 0; i < subFiles.Length;i++)
                {
                    FileInfo fileInfo = new FileInfo(subFiles[i]);
                    srcList.Add(fileInfo.Length.ToString());
                    string destpath = string.Format(@"{0}\{1}", currentDownloadPath, Path.GetFileName(subFiles[i]));
                   FileStream openFs = File.Open(subFiles[i], FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                    FileStream outFs = File.Create(destpath);
                    long totalByte = openFs.Length;//文件的字节数
                    int bSize = 1024;
                    byte[] b = new byte[bSize];
                    int readCount = openFs.Read(b, 0, bSize);
                    while (readCount > 0)
                    {

                        outFs.Write(b, 0, readCount);
                        downloadedSize += readCount;

                        readCount = openFs.Read(b, 0, bSize);
                        lastTime = DateTime.Now;
                    }
                    openFs.Close();
                    outFs.Close();
                    FileInfo  f = new FileInfo(destpath);
                    dirsList.Add(f.Length.ToString());
                }
                
                //递归下载子目录
                for (int j = 0; j < subDirs.Length;j++ )
                {
                    string downloadPath = currentDownloadPath;
                    DownloadDirectory(downloadPath);
                }
                int count=0;
                if (dirsList.Count == srcList.Count)
                {
                    for (int i = 0; i < dirsList.Count; i++)
                    {
                        if (dirsList[i] == srcList[i])
                        {
                            count++;
                        }
                    }
                }
                else
                {
                    //status = "下载失败";
                    status = "下载失败";
                    downLoadMsg = "下载数据不完整！";
                }
                if (count == dirsList.Count)
                {
                    status = "下载完成";
                }
                else
                {
                    status = "下载失败";
                    //status = "下载失败";
                    downLoadMsg = "下载数据不完整！";
                }
            }
            catch(Exception ex)
            {
                status = "下载失败";
                //status = "下载失败";
                downLoadMsg = ex.ToString(); 
            }

        }

        /// <summary>
        /// 获取下载进度,范围在0到100之间 
        /// </summary>
        /// <returns></returns>
        public double GetProgress()
        {
            return ((double)downloadedSize/(double)dataSize) * 100;
        }

        /// <summary>
        /// 获取下载速度  kb/s
        /// </summary>
        public double DownloadSpeed()
        {
            TimeSpan ts = lastTime - startTime;
            if (ts.Seconds == 0)
                return 0;
            return (downloadedSize/1024)/ts.Seconds;
        }

        /// <summary>
        /// 获取下载数据的名称
        /// </summary>
        /// <returns></returns>
        public string GetSrcName()
        {
            if(isTile)
            {
                return srcPath;
            }
            if (File.Exists(srcPath))
            {
                return Path.GetFileName(srcPath);
            }
            else
            {
                return new DirectoryInfo(srcPath).Name;
            }
        }

        /// <summary>
        /// 下载文件
        /// </summary>
        private void DownloadFile()
        {
            try
            {
                FileStream openFs = File.Open(srcPath, FileMode.Open);
                string destFilePath = string.Format(@"{0}\{1}", destPath, Path.GetFileName(srcPath));
                if (Path.GetExtension(srcPath) == ".gnf" && !Directory.Exists(destPath))
                {
                    destFilePath = destPath;
                    destPath = Path.GetDirectoryName(destPath);
                }
                FileStream outFs = File.Create(destFilePath);
                long totalByte = openFs.Length;//文件的字节数
                int bSize = 1024;
                byte[] b = new byte[bSize];
                int readCount = openFs.Read(b, 0, bSize);
                while (readCount > 0)
                {

                    outFs.Write(b, 0, readCount);
                    downloadedSize += readCount;

                    readCount = openFs.Read(b, 0, bSize);
                }
                lastTime = DateTime.Now;
                openFs.Close();
                outFs.Close();
                status = "下载完成";
            }
            catch(Exception ex)
            {
                status = "下载失败";
                //status = "下载失败";
                downLoadMsg = ex.ToString(); 
            }
          
        }

    }
}
