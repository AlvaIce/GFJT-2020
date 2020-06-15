namespace QRST_DI_MS_Basis.Log
{
    public class OrderLog:translog
    {
        public string orderCode;   //订单号

        public OrderLog():base(EnumLogType.OrderLog)
        {

        }

        /// <summary>
        /// 在消息前面加上订单号
        /// </summary>
        /// <param name="message"></param>
        public override void Add(string message)
        {
            message = orderCode + ":" + message;
            base.Add(message);
        }

    }
}
