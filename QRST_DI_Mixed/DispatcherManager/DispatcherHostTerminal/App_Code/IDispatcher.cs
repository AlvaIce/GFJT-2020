/// <summary>
///IDispatcher 的摘要说明
/// </summary>
public interface IDispatcher
{
    /// <summary>
    /// 创建工作空间，为数据预处理入库作准备 
    /// </summary>
    /// <returns></returns>
    string CreateWorkSpace();

    /// <summary>
    ///更改订单状态,使订单进入等待状态，若返回"1"表示更改成功，返回"0"则更改失败，返回"-1"则该订单不存在
    /// </summary>
    /// <param name="orderCode"></param>
    /// <returns></returns>
    string UpdateOrderStatus(string orderCode);
}