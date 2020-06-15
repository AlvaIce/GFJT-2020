using System;

namespace QRST.WorldGlobeTool.Utility
{
    /// <summary>
    /// 日志事件参数
    /// </summary>
    public class LogEventArgs : EventArgs
    {
        public int level;
        public string category;
        public string message;

        /// <summary>
        /// 日志事件参数
        /// </summary>
        /// <param name="_l">日志等级</param>
        /// <param name="_c">日志种类</param>
        /// <param name="_m">日志消息</param>
        public LogEventArgs(int _l, string _c, string _m)
        {
            level = _l;
            category = _c;
            message = _m;
        }
    }
}
