/// <summary>
///IWSSearcher4JCGX 的摘要说明
/// </summary>
public interface IWSSearcher4JCGX
{
    /// <summary>
    /// 检索预处理后未切片数据的
    /// </summary>
    /// <param name="position">空间范围 最小纬度 最小经度 最大纬度 最大经度</param>
    /// <param name="datetime">起时间 止时间</param>
    /// <param name="satellite">卫星 GF1 GF2 ...</param>
    /// <param name="sensor">传感器 CCD</param>
    /// <param name="DnMark">昼夜标识</param>
    /// <param name="PixelSpacing">分辨率</param>
    /// <param name="DataSizeRange">数据大小范围，单位为KB</param>
    /// <param name="CloudNumRange">云量范围</param>
    /// <param name="StartIndex">起始记录位置</param>
    /// <param name="RecordCount">结果集中的记录数目</param>
    /// <returns>返回的结果集</returns>
    //DataSet SearchCorrectedData(List<string> position, List<string> datetime, List<string> satellite, List<string> sensor,string DnMark,List<string> PixelSpacing,List<int> DataSizeRange,List<int> CloudNumRange,out int AllRecordsCount,int StartIndex,int ResultCount);

    /// <summary>
    /// 检索算法组件方法(JCGX调用)
    /// </summary>
    /// <param name="AlgEnName">算法组件英文名称</param>
    /// <param name="AlgCnName">算法组件中文名</param>
    /// <param name="ComponentVersion">算法组件版本号</param>
    /// <param name="IsOnCloud">是否在公有云中的信息</param>
    /// <param name="AllRecordsCount">输出参数，返回查询到的记录数</param>
    /// <param name="StartIndex">起始记录位置，开始于0</param>
    /// <param name="ResultCount">结果集中的记录数目</param>
    /// <param name="QueryRange">查询范围标记。0表示查询用户上传数据，1表示查询数据库中标准数据，2表示用户上传数据和数据库中标准数据联合查询。</param>
    /// <returns></returns>
    //DataSet SearchAlgorithmJCGX(string AlgEnName, string AlgCnName,  string ComponentVersion,string IsOnCloud, out int AllRecordsCount, int StartIndex, int ResultCount, int QueryRange);

    /// <summary>
    /// 检索文档Document方法(JCGX调用)
    /// </summary>
    /// <param name="DocumentName">文档名称</param>
    /// <param name="ProgramName">项目名称</param>
    /// <param name="Author">作者姓名</param>
    /// <param name="KeyWords">关键字</param>
    /// <param name="isOnCloud">是否在公有云中的信息</param>
    /// <param name="AllRecordsCount">输出参数，返回查询到的记录数</param>
    /// <param name="StartIndex">起始记录位置，开始于0</param>
    /// <param name="ResultCount">结果集中的记录数目</param>
    /// <param name="QueryRange">查询范围标记。0表示查询用户上传数据，1表示查询数据库中标准数据，2表示用户上传数据和数据库中标准数据联合查询。</param>
    /// <returns></returns>
    //DataSet SearchDocumentJCGX(List<string> DocumentName, List<string> ProgramName, List<string> Author, List<string> KeyWords, string isOnCloud, out int AllRecordsCount, int StartIndex, int ResultCount, int QueryRange, string strOrderBy, int OrderByType = -1);

    /// <summary>
    /// 检索工具包toolkit方法(JCGX调用)
    /// </summary>
    /// <param name="ToolkitName">工具包名称</param>
    /// <param name="OStype">操作系统类型</param>
    /// <param name="Author">作者姓名</param>
    /// <param name="KeyWords">关键字列表</param>
    /// <param name="isOnCloud">是否在公有云中的信息</param>
    /// <param name="AllRecordsCount">输出参数，返回查询到的记录数</param>
    /// <param name="StartIndex">起始记录位置，开始于0</param>
    /// <param name="ResultCount">结果集中的记录数目</param>
    /// <param name="QueryRange">查询范围标记。0表示查询用户上传数据，1表示查询数据库中标准数据，2表示用户上传数据和数据库中标准数据联合查询。</param>
    /// <returns></returns>
    //DataSet SearchToolkitJCGX(string ToolkitName, List<string> OStype, List<string> Author, List<string> KeyWords, string isOnCloud, out int AllRecordsCount, int StartIndex, int ResultCount, int QueryRange);

    /// <summary>
    /// 检索预处理后未切片数据的
    /// </summary>
    /// <param name="position">空间范围 最小纬度 最小经度 最大纬度 最大经度</param>
    /// <param name="datetime">起时间 止时间</param>
    /// <param name="satellite">卫星 GF1 GF2 ...</param>
    /// <param name="sensor">传感器 CCD</param>
    /// <param name="DnMark">昼夜标识</param>
    /// <param name="PixelSpacing">分辨率</param>
    /// <param name="DataSizeRange">数据大小范围，单位为KB</param>
    /// <param name="CloudNumRange">云量范围</param>
    /// <param name="StartIndex">起始记录位置</param>
    /// <param name="RecordCount">结果集中的记录数目</param>
    ///// <returns>返回的结果集</returns>
    //DataSet SearchCorrectedDataJCGX(List<string> position, List<string> datetime, List<string> satellite, List<string> sensor, List<string> PixelSpacing, List<int> DataSizeRange, string isOnCloud, out int AllRecordsCount, int StartIndex, int ResultCount, int QueryRange ,string strOrderBy, int OrderByType = -1);

    ///// <summary>
    ///// 基础矢量数据检索
    ///// </summary>
    ///// <param name="name">矢量数据名称</param>
    ///// <param name="position">空间范围 最小纬度 最小经度 最大纬度 最大经度</param>
    ///// <param name="datetime">起时间 止时间</param>
    ///// <param name="keyword">关键字</param>
    ///// <returns></returns>
    //DataSet SearchBaseVectors(string name, List<string> position, List<DateTime> datetime,string keyword);
    ///// <summary>
    ///// DEM检索
    ///// </summary>
    ///// <param name="type">类型 30m 90m</param>
    ///// <param name="position">空间范围 最小纬度 最小经度 最大纬度 最大经度</param>
    ///// <returns></returns>
    //DataSet SearchDEM(string type, List<string> position);
    ///// <summary>
    ///// 影像控制点检索
    ///// </summary>
    ///// <param name="position">空间范围 最小纬度 最小经度 最大纬度 最大经度</param>
    ///// <returns></returns>
    //DataSet SearchImageCP(List<string> position);
    /// <summary>
    /// 行政区划范围查询
    /// </summary>
    /// <param name="name">名称（省名、市名、县名）</param>
    /// <param name="type">类型（省、市、县）</param>
    /// <returns>空间范围 最小纬度 最小经度 最大纬度 最大经度</returns>
    //List<string> SearchRegion(string name, string type);
}