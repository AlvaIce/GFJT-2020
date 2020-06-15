namespace QRST_DI_TS_Process.Service
{
    /// <summary>
    /// 用户可以上传的数据类型枚举 zxw 2013/3/10
    /// </summary>
    public enum UserUploaderDataType
    {
        DocData = 6,          //文档数据
        ServerData = 5,
        AlgData = 4,          //算法数据
        VectorData = 3,   //矢量数据
        RasterData = 2,  //栅格数据
        Unknown = -1,  //未知数据
    } 
}
