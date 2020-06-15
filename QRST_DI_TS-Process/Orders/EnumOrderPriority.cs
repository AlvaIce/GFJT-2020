namespace QRST_DI_TS_Process.Orders
{
    public enum EnumOrderPriority
    {
        /// <summary>
        /// 应急模式
        /// </summary>
        Emergency = 0,
        /// <summary>
        /// 内部用户订单
        /// </summary>
        Ugent = 1,
        /// <summary>
        /// 常规订单
        /// </summary>
        High = 2,
        /// <summary>
        /// 公共用户订单
        /// </summary>
        Normal = 3,
        /// <summary>
        /// 非重要
        /// </summary>
        Low = 4,
    }
}
