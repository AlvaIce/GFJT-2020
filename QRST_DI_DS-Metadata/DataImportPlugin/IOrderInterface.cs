/*
 * 作者：zxw
 * 创建时间：2013-09-24
 * 描述：开放部分订单信息接口，便于导入数据的插件获取部分订单信息
*/
 
namespace QRST_DI_DS_Metadata.DataImportPlugin
{
    public interface IOrderInterface
    {
        /// <summary>
        /// 添加订单日志
        /// </summary>
        /// <param name="logMessage"></param>
        void Addlog(string logMessage);

        /// <summary>
        /// 获取订单执行站点
        /// </summary>
        /// <returns></returns>
        string GetExecuteSite();

        /// <summary>
        /// 获取订单工作空间路径
        /// </summary>
        /// <returns></returns>
        string GetOrderWorkspace();
    }
}
