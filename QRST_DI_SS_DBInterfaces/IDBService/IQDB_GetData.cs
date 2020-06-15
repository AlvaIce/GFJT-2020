using System.Collections.Generic;

namespace QRST_DI_SS_DBInterfaces.IDBService
{
    public interface IQDB_GetData
    {
        /// <summary>
        /// 获取切片数据路径
        /// </summary>
        /// <param name="tileNames"></param>
        /// <returns></returns>
        List<string> GetTilesList(string[] tileNames);

        List<string> GetTilesList(List<string> tileNames);

        /// <summary>
        /// 根据数据类型编码获得数据存放相对路径
        /// </summary>
        /// <param name="dataCode"></param>
        /// <returns></returns>
        string GetRalativeAddByDataCode(string dataCode);

        /// <summary>
        /// 根据数据类型编码获得构成数据存放相对路径2的字段
        /// </summary>
        /// <param name="dataCode"></param>
        /// <returns></returns>
        string[] GetAddressFields(string dataCode);

        /// <summary>
        /// 获得纠正后未切片数据的构成存放相对路径的字段
        /// </summary>
        /// <returns></returns>
        string[] GetCorrectedDataAddressFields();

        /// <summary>
        /// 根据查询得到的数据名称，获得纠正后未切片数据的存储路径
        /// </summary>
        /// <param name="DataName"></param>
        /// <returns></returns>
        string GetCorrectedDataAddress(string DataName);

        /// <summary>
        /// 根据查询得到的原数据编码，获得数据所在文件夹的存储路径
        /// </summary>
        /// <param name="DataCode"></param>
        /// <returns></returns>
        string GetSourceDataPath(string DataCode);

        /// <summary>
        /// 根据切片文件名，找到瓦片数据后打包推送到指定目录下
        /// </summary>
        /// <param name="tileName"></param>
        /// <param name="originalOrderCode"></param>
        /// <returns></returns>
        string PushTileZipToDirByName(string tileName, string originalOrderCode);

        /// <summary>
        /// 根据QRST_CODE获取路径拷贝文件(本地直接复制方法)
        /// </summary>
        /// <param name="code"></param>
        /// <param name="sharePath"></param>
        /// <returns></returns>
        string CopyByQrstCode(string code, string sharePath);

        /// <summary>
        /// 根据QRST_CODE获取路径拷贝文件（创建order并提交订单task）
        /// </summary>
        /// <param name="code"></param>
        /// <param name="sharePath"></param>
        /// <param name="mouid"></param>
        /// <returns></returns>
        string CopyByQrstCodeOrderTask(string code, string sharePath, string mouid);

        #region 生产线接口
        /// <summary>
        /// 生产线接口：根据数据的datacode删除对应的元数据记录与文件
        /// </summary>
        /// <param name="datacode"></param>
        /// <returns></returns>
        int DeleteData(string datacode);

        /// <summary>
        /// 生产线接口：根据数据的datacode删除对应的元数据记录与文件
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        //string test(string s);

        /// <summary>
        /// 生产线接口：根据查询数据名称以及数据编码获取数据存放路径,目前支持AlgorithmCmp和ProductWFL查询
        /// </summary>
        /// <param name="DataType"></param>
        /// <param name="QRST_CODE"></param>
        /// <returns></returns>
        string GetData(string DataType, string QRST_CODE);


        #endregion
    }
}
