using System;
using System.Collections;

namespace QRST.WorldGlobeTool.Net
{
    /// <summary>
    /// Download queue with priorities
    /// 带优先级的下载队列
    /// </summary>
    public class DownloadQueue : IDisposable
    {
        #region  字段

        /// <summary>
        /// 最大下载队列长度
        /// </summary>
        public static int MaxQueueLength = 200;
        /// <summary>
        /// 最大当前正在下载长度
        /// </summary>
        public static int MaxConcurrentDownloads = 2;
        /// <summary>
        /// 下载请求列表
        /// </summary>
        private ArrayList m_requests = new ArrayList();
        /// <summary>
        /// 活动下载列表
        /// </summary>
        private ArrayList m_activeDownloads = new ArrayList();

        #endregion

        /// <summary>
        /// 初始化一个DownloadQueue示例
        /// </summary>
        public DownloadQueue()
        {
            DownloadRequest.Queue = this;
        }

        #region  属性

        /// <summary>
        /// 获取下载请求列表
        /// </summary>
        public ArrayList Requests
        {
            get
            {
                return m_requests;
            }
        }

        /// <summary>
        /// 获取当前活动的下载列表
        /// </summary>
        public ArrayList ActiveDownloads
        {
            get
            {
                return m_activeDownloads;
            }
        }

        #endregion

        /// <summary>
        /// 清空所有下载请求
        /// </summary>
        /// <param name="owner"></param>
        public virtual void Clear(object owner)
        {
            lock (m_requests.SyncRoot)
            {
                for (int i = m_requests.Count - 1; i >= 0; i--)
                {
                    DownloadRequest request = (DownloadRequest)m_requests[i];
                    if (request.Owner == owner)
                    {
                        m_requests.RemoveAt(i);
                        request.Dispose();
                    }
                }
            }
            ServiceDownloadQueue();
        }

        /// <summary>
        /// 招到下一个下载请求
        /// </summary>
        protected virtual DownloadRequest GetNextDownloadRequest()
        {
            DownloadRequest bestRequest = null;
            float highestScore = float.MinValue;

            lock (m_requests.SyncRoot)
            {
                for (int i = m_requests.Count - 1; i >= 0; i--)
                {
                    DownloadRequest request = (DownloadRequest)m_requests[i];
                    if (request.IsDownloading)
                        continue;

                    float score = request.CalculateScore();
                    if (float.IsNegativeInfinity(score))
                    {
                        // Request is of no interest anymore, remove it
                        m_requests.RemoveAt(i);
                        request.Dispose();
                        continue;
                    }

                    if (score > highestScore)
                    {
                        highestScore = score;
                        bestRequest = request;
                    }
                }
            }

            return bestRequest;
        }

        /// <summary>
        /// 添加一个下载请求到当前下载队列
        /// </summary>
        public virtual void Add(DownloadRequest newRequest)
        {
            if (newRequest == null)
                throw new NullReferenceException();

            lock (m_requests.SyncRoot)
            {
                foreach (DownloadRequest request in m_requests)
                {
                    if (request.Key == newRequest.Key)
                    {
                        newRequest.Dispose();
                        return;
                    }
                }

                m_requests.Add(newRequest);

                if (m_requests.Count > MaxQueueLength)
                {
                    // Remove lowest scoring queued request
                    DownloadRequest leastImportantRequest = null;
                    float lowestScore = float.MinValue;

                    for (int i = m_requests.Count - 1; i >= 0; i--)
                    {
                        DownloadRequest request = (DownloadRequest)m_requests[i];
                        if (request.IsDownloading)
                            continue;

                        float score = request.CalculateScore();
                        if (score == float.MinValue)
                        {
                            // Request is of no interest anymore, remove it
                            m_requests.Remove(request);
                            request.Dispose();
                            return;
                        }

                        if (score < lowestScore)
                        {
                            lowestScore = score;
                            leastImportantRequest = request;
                        }
                    }

                    if (leastImportantRequest != null)
                    {
                        m_requests.Remove(leastImportantRequest);
                        leastImportantRequest.Dispose();
                    }
                }
            }

            ServiceDownloadQueue();
        }

        /// <summary>
        /// 从下载队列中移除一个下载请求
        /// </summary>
        public virtual void Remove(DownloadRequest request)
        {
            lock (m_requests.SyncRoot)
            {
                for (int i = m_activeDownloads.Count - 1; i >= 0; i--)
                    if (request == m_activeDownloads[i])
                        // Already downloading, let it finish
                        return;

                m_requests.Remove(request);
            }
            request.Dispose();

            ServiceDownloadQueue();
        }

        /// <summary>
        /// 当有可用线程时启动下载请求
        /// </summary>
        protected virtual void ServiceDownloadQueue()
        {
            lock (m_requests.SyncRoot)
            {
                // Remove finished downloads
                for (int i = m_activeDownloads.Count - 1; i >= 0; i--)
                {
                    DownloadRequest request = (DownloadRequest)m_activeDownloads[i];
                    if (!request.IsDownloading)
                        m_activeDownloads.RemoveAt(i);
                }

                // Start new downloads
                while (m_activeDownloads.Count < MaxConcurrentDownloads)
                {
                    DownloadRequest request = GetNextDownloadRequest();
                    if (request == null)
                        break;

                    m_activeDownloads.Add(request);
                    request.Start();
                }
            }
        }

        /// <summary>
        /// 在下载完成时的回调函数
        /// </summary>
        internal void OnComplete(DownloadRequest request)
        {
            lock (m_requests.SyncRoot)
            {
                // Remove the finished item from queue
                m_requests.Remove(request);
                request.Dispose();
            }

            // Start next download
            ServiceDownloadQueue();
        }

        #region IDisposable Members

        /// <summary>
        /// 释放下载队列
        /// </summary>
        public void Dispose()
        {
            lock (m_requests.SyncRoot)
            {
                foreach (DownloadRequest request in m_requests)
                    request.Dispose();
                m_requests.Clear();
                m_activeDownloads.Clear();
            }

            GC.SuppressFinalize(this);
        }

        #endregion
    }
}
