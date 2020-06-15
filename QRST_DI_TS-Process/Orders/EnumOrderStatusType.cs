namespace QRST_DI_TS_Process.Orders
{
    public enum EnumOrderStatusType
    {
        Error = -1,     //异常
        Waiting = 0,        //等待
        Processing = 1,     //处理中
        Completed = 2,       //完成
        Suspending = 3,     //正在暂停中
        Suspended = 4,        //已暂停
        Cancelling = 5,       //正在取消
        Cancelled = 6      //取消
    }
}
