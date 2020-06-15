using System;

namespace QRST.WorldGlobeTool.Net
{
    /// <summary>
    /// Base class for various types of download requests (protocol independent)
    /// 各种类型下载请求的基类（独立协议）
    /// </summary>
    public abstract class DownloadRequest : IDisposable
    {
        /// <summary>
        /// 下载队列
        /// </summary>
        internal static DownloadQueue Queue;
        /// <summary>
        /// 所有者
        /// </summary>
        private object m_owner;

        /// <summary>
        /// 初始化一个DownloadRequest实例
        /// </summary>
        /// <param name="owner"></param>
        protected DownloadRequest(object owner)
        {
            m_owner = owner;
        }

        /// <summary>
        /// A unique key identifying this request
        /// 获取描述当前请求的唯一关键词
        /// </summary>
        public abstract string Key
        {
            get;
        }

        /// <summary>
        /// The object that created this request
        /// 创建当前请求的对象
        /// </summary>
        public object Owner
        {
            get
            {
                return m_owner;
            }
            set
            {
                m_owner = value;
            }
        }

        /// <summary>
        /// Value (0-1) indicating how far the download has progressed.
        /// 描述下载进度的0~1之间的值
        /// </summary>
        public abstract float Progress
        {
            get;
        }

        /// <summary>
        /// Whether the request is currently being downloaded
        /// 当前请求是否正在下载
        /// </summary>
        public abstract bool IsDownloading
        {
            get;
        }

        /// <summary>
        /// Starts processing this request
        /// 启动请求进程
        /// </summary>
        public abstract void Start();

        /// <summary>
        /// Calculates the score of this request.  Used to prioritize downloads.  
        /// Override in derived class to allow prioritization.
        /// 计算当前请求的得分评价，用来按照优先级下载。
        /// 在继承类中重载实现允许优先级的使用。
        /// </summary>
        /// <returns>Relative score or float.MinValue if request is no longer of interest.</returns>
        public virtual float CalculateScore()
        {
            return 0;
        }

        /// <summary>
        /// Derived classes should call this method to signal processing complete.
        /// 继承类应该调用这个方法来上报进程结束的信号。
        /// </summary>
        public virtual void OnComplete()
        {
            Queue.OnComplete(this);
        }

        #region IDisposable Members

        public virtual void Dispose()
        {
        }

        #endregion
    }
}
