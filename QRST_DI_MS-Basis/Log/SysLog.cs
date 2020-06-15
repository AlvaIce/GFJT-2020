using QRST_DI_MS_Basis.UserRole;

namespace QRST_DI_MS_Basis.Log
{
    public class SysLog : translog
    {
        public userInfo user ;   //订单号

        public SysLog(userInfo u)
            : base(EnumLogType.SystemLog)
        {
           user = u;
        }

        /// <summary>
        /// 在消息前面加上订单号
        /// </summary>
        /// <param name="message"></param>
        public override void Add(string message)
        {
            message = user.NAME + ":" + message;
            base.Add(message);
        }

    }
}
