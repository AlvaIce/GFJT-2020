/*
 * 作者：zxw
 * 创建时间：2013-09-24
 * 描述：定义导入数据接口，主要提供导入数据时的元数据信息、数据缩略图、原始数据路径等,需要重新定义导入的插件需要继承此类
*/
using System.Collections.Generic;
 
namespace QRST_DI_DS_Metadata.DataImportPlugin
{
    public  interface IDataImport
    {
        /// <summary>
        /// 设置数据导入参数
        /// </summary>
        /// <param name="?"></param>
        void SetParameter(string[] paras);

       /// <summary>
       /// 数据准备
       /// </summary>
       /// <returns>数据准备好之后返回true,否则返回false</returns>
       bool DataPrepare();

        /// <summary>
        /// 关联父订单
        /// </summary>
        /// <param name="_parentOrder"></param>
        void SetParentOrder(IOrderInterface _parentOrder);

        /// <summary>
        /// 获取原始文件路径
        /// </summary>
        /// <returns>原始数据文件列表</returns>
        string[] GetSourceFilePath();

        /// <summary>
        /// 获取数据存储相对路径
        /// </summary>
        /// <returns></returns>
        string GetSourceRelatePath();

       /// <summary>
       /// 获取元数据字段与值对应关系
       /// </summary>
       /// <returns></returns>
        Dictionary<string, string> GetMetadata();

        /// <summary>
        /// 获取原始数据缩略图
        /// </summary>
        /// <returns>返回缩略图路径</returns>
        string[] GetThnmbnail();

       /// <summary>
       /// 获取键字段
       /// </summary>
       /// <returns></returns>
        string GetKeyField();
    }
}
