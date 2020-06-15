using System;
using System.Threading;
using System.IO;
using System.Collections;

namespace QRST.WorldGlobeTool.Utility
{
    /// <summary>
    /// Maintains the cached data on disk (staying within limits)
    /// 维护硬盘上的缓存数据（保持在限度以内）
    /// </summary>
    public class Cache : IDisposable
    {
        /// <summary>
        /// default value for maximum Cache size is 2 Gigabytes
        /// 默认最大缓存限度为2GB
        /// </summary>
        public long CacheUpperLimit = 2L * 1024L * 1024L * 1024L;
        /// <summary>
        /// default value for size where Cache cleanup stops is 1.5 Gigabytes(75% of max size)
        /// 默认缓存清理限度为1.5GB（最大值的75%）
        /// </summary>
        public long CacheLowerLimit = 1536L * 1024L * 1024L;
        /// <summary>
        /// 缓存目录
        /// </summary>
        public string CacheDirectory;
        /// <summary>
        /// 缓存清理频度
        /// </summary>
        public TimeSpan CleanupFrequency;
        /// <summary>
        /// 定时器
        /// </summary>
        Timer m_timer;

        /// <summary>
        /// Initializes a new instance of the <see cref= "T:Qrst.Cache"/> class.
        /// 初始化一个缓存类
        /// </summary>
        /// <param name="cacheDirectory">Location of the cache files.（本地缓存目录）</param>
        /// <param name="cleanupFrequencyInterval">Frequency of cache cleanup.（缓存清理频率）</param>
        /// <param name="totalRunTime">Total duration application has been running so far.（程序运行时间）</param>
        public Cache(
            string cacheDirectory,
            TimeSpan cleanupFrequencyInterval,
            TimeSpan totalRunTime)
        {
            this.CleanupFrequency = cleanupFrequencyInterval;
            this.CacheDirectory = cacheDirectory;
            Directory.CreateDirectory(this.CacheDirectory);

            // Start the timer
            double firstDueSeconds = cleanupFrequencyInterval.TotalSeconds -
                totalRunTime.TotalSeconds % cleanupFrequencyInterval.TotalSeconds;
            m_timer = new Timer(new TimerCallback(OnTimer), null,
                (long)(firstDueSeconds * 1000),
                (long)cleanupFrequencyInterval.TotalMilliseconds);
        }

        /// <summary>
        /// 初始化一个缓存类
        /// </summary>
        /// <param name="cacheDirectory">本地缓存目录</param>
        /// <param name="cacheLowerLimit">缓存下限</param>
        /// <param name="cacheUpperLimit">缓存上限</param>
        /// <param name="cleanupFrequencyInterval">缓存清理频率</param>
        /// <param name="totalRunTime">程序运行时间</param>
        public Cache(
            string cacheDirectory,
            long cacheLowerLimit,
            long cacheUpperLimit,
            TimeSpan cleanupFrequencyInterval,
            TimeSpan totalRunTime)
            : this(cacheDirectory, cleanupFrequencyInterval, totalRunTime)
        {
            this.CacheLowerLimit = cacheLowerLimit;
            this.CacheUpperLimit = cacheUpperLimit;
        }

        /// <summary>
        /// Monitors the cache, makes sure it stays within limits.
        /// 监视缓存，确保仍然在缓存限度内
        /// </summary>
        private void OnTimer(object state)
        {
            try
            {
                // We are are not in a hurry  当前程序的进程并不是一个很紧急的线程
                Thread.CurrentThread.Priority = ThreadPriority.BelowNormal;

                // dirSize is reported as the total of the file sizes, in bytes
                // use the on-disk filesize, not FileInfo.Length, to calculate dirSize
                // dirSize是以字节为单位的所有文件大小的和，
                long dirSize = GetDirectorySize(new DirectoryInfo(this.CacheDirectory));
                if (dirSize < this.CacheUpperLimit)
                    return;

                ArrayList fileInfoList = GetDirectoryFileInfoList(new DirectoryInfo(this.CacheDirectory));

                while (dirSize > this.CacheLowerLimit)
                {
                    if (fileInfoList.Count <= 100)  //如果文件个数小于等于100个时，直接退出
                        break;

                    //寻找日期最旧的文件
                    FileInfo oldestFile = null;
                    foreach (FileInfo curFile in fileInfoList)
                    {
                        if (oldestFile == null)
                        {
                            oldestFile = curFile;
                            continue;
                        }

                        if (curFile.LastAccessTimeUtc < oldestFile.LastAccessTimeUtc)
                        {
                            oldestFile = curFile;
                        }
                    }
                    //删除日期较旧的文件
                    fileInfoList.Remove(oldestFile);
                    dirSize -= oldestFile.Length;
                    try
                    {
                        File.Delete(oldestFile.FullName);

                        // Recursively remove empty directories
                        string directory = oldestFile.Directory.FullName;
                        while (Directory.GetFileSystemEntries(directory).Length == 0)
                        {
                            Directory.Delete(directory);
                            directory = Path.GetDirectoryName(directory);
                        }
                    }
                    catch (IOException caught)
                    {
                        Log.Write(Log.Levels.Error, "CACH", caught.Message);
                    }
                }
            }
            catch (Exception caught)
            {
                Log.Write(Log.Levels.Error, "CACH", caught.Message);
            }
        }

        /// <summary>
        /// 获取指定目录下的所有文件的文件信息
        /// </summary>
        /// <param name="inDir">文件目录</param>
        /// <returns>返回目录下文件的文件信息</returns>
        public static ArrayList GetDirectoryFileInfoList(DirectoryInfo inDir)
        {
            ArrayList returnList = new ArrayList();
            foreach (DirectoryInfo subDir in inDir.GetDirectories())
            {//递归获取子目录下的文件信息
                returnList.AddRange(GetDirectoryFileInfoList(subDir));
            }
            foreach (FileInfo fi in inDir.GetFiles())
            {
                returnList.Add(fi);
            }
            return returnList;
        }

        /// <summary>
        /// 获取目录的大小
        /// </summary>
        /// <param name="inDir">目录路径</param>
        /// <returns>返回目录的大小</returns>
        public static long GetDirectorySize(DirectoryInfo inDir)
        {
            long returnBytes = 0;
            foreach (DirectoryInfo subDir in inDir.GetDirectories())
            {
                returnBytes += GetDirectorySize(subDir);
            }
            foreach (FileInfo fi in inDir.GetFiles())
            {
                try
                {
                    returnBytes += fi.Length;
                }
                catch (IOException caught)
                {
                    // Ignore files that may have disappeared since we started scanning.
                    Log.Write(Log.Levels.Error, "CACH", caught.Message);
                }
            }
            return returnBytes;
        }

        public override string ToString()
        {
            return CacheDirectory;
        }

        #region IDisposable Members

        public void Dispose()
        {
            if (m_timer != null)
            {
                m_timer.Dispose();
                m_timer = null;
            }
        }

        #endregion
    }
}
