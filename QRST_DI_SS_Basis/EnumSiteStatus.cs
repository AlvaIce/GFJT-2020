using System;

namespace QRST_DI_SS_Basis
{
    [Serializable]
    public enum EnumSiteStatus
    {
        /// <summary>
        /// 正在运行
        /// </summary>
        Running = 0,
        /// <summary>
        /// 停止运行
        /// </summary>
        Stopped = 1,
        /// <summary>
        ///运行出错
        /// </summary>
        Error = 2,
        /// <summary>
        ///暂停接收新订单
        ////// </summary>
        Suspended = 3

    }
}
