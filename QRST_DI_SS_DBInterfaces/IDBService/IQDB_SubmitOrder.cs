using System;
using System.Collections.Generic;

namespace QRST_DI_SS_DBInterfaces.IDBService
{
    public interface IQDB_SubmitOrder
    {
        /// <summary>
        /// 创建一个共享的工作空间，用户将数据临时放入该空间下，触发入库，若为'-1'，则申请失败！
        /// dataType为数据类型
        /// </summary>
        /// <param name="dataType"></param>
        /// <returns></returns>
        string GetNewWorkSpace(string dataType);

        /// <summary>
        /// 触发数据入库
        /// </summary>
        /// <param name="sharePath"></param>
        /// <returns></returns>
        int SubmitHJDataImport(string sharePath);

        /// <summary>
        /// 触发DOC数据入库！(日期转为字符串)
        /// </summary>
        /// <param name="TITLE"></param>
        /// <param name="DOCTYPE"></param>
        /// <param name="KEYWORD"></param>
        /// <param name="ABSTRACT"></param>
        /// <param name="DOCDATE"></param>
        /// <param name="DESCRIPTION"></param>
        /// <param name="AUTHOR"></param>
        /// <param name="UPLOADER"></param>
        /// <param name="UPLOADTIME"></param>
        /// <param name="FILESIZE"></param>
        /// <param name="sharePath"></param>
        /// <returns></returns>
        int SubmitDOCDataImportToString(string TITLE, string DOCTYPE, string KEYWORD, string ABSTRACT, string DOCDATE,
            string DESCRIPTION, string AUTHOR, string UPLOADER, string UPLOADTIME, string FILESIZE, string sharePath);

        /// <summary>
        /// 触发DOC数据入库
        /// </summary>
        /// <param name="TITLE"></param>
        /// <param name="DOCTYPE"></param>
        /// <param name="KEYWORD"></param>
        /// <param name="ABSTRACT"></param>
        /// <param name="DOCDATE"></param>
        /// <param name="DESCRIPTION"></param>
        /// <param name="AUTHOR"></param>
        /// <param name="UPLOADER"></param>
        /// <param name="UPLOADTIME"></param>
        /// <param name="FILESIZE"></param>
        /// <param name="sharePath"></param>
        /// <returns></returns>
        int SubmitDOCDataImport(string TITLE, string DOCTYPE, string KEYWORD, string ABSTRACT, DateTime DOCDATE,
            string DESCRIPTION, string AUTHOR, string UPLOADER, DateTime UPLOADTIME, int FILESIZE, string sharePath);

        /// <summary>
        /// 触发数据批量入库！datatype为数据类型，dirPath为数据目录
        /// </summary>
        /// <param name="datatype"></param>
        /// <param name="sharePath"></param>
        /// <returns></returns>
        int SubmitDataImport(string datatype, string sharePath);

        /// <summary>
        /// 触发数据入库！dataType为需要上传的数据类型，sharePath为数据所在的网络路径,生产线
        /// </summary>
        /// <param name="dataType"></param>
        /// <param name="sharePath"></param>
        /// <returns></returns>
        int SubmitImportOrder(string dataType, string sharePath);

        /// <summary>
        /// 用户数据上传统一接口，dataType为上传类型，pathID源数据url,opID为操作号
        /// </summary>
        /// <param name="dataType"></param>
        /// <param name="pathID"></param>
        /// <param name="opID"></param>
        /// <returns></returns>
        int UploadData(string dataType, string pathID, string opID);

        /// <summary>
        /// 提交内置指定订单
        /// </summary>
        /// <param name="orderName"></param>
        /// <param name="orderParas"></param>
        /// <returns></returns>
        string SubmitInstalledOrder(string orderName, string[] orderParas);

        /// <summary>
        /// 提交内置指定订单,瓦片下载使用
        /// </summary>
        /// <param name="orderName"></param>
        /// <param name="orderParas"></param>
        /// <returns></returns>
        List<string> SubmitTilesInstalledOrder(string orderName, string[] orderParas);

        /// <summary>
        /// 为预处理准备数据生产工作空间！-1表示创建失败,否则传送[订单号：工作空间路径]，
        /// 例如：'P1475369832501:\\127.0.0.1\\QRST_DB_Share\\P1475369832501'！ zxw 20130818
        /// </summary>
        /// <returns></returns>
        string CreateWorkSpaceForPreProcess();

        /// <summary>
        /// 为原始数据及元数据入库订单创建工作空间。
        /// -1表示创建失败,否则传送[订单号]，例如：'P1475369832501'
        /// </summary>
        /// <returns></returns>
        string ApplyNewGF1DataImportOrder();

        /// <summary>
        /// 为原始数据及元数据入库订单创建工作空间。
        /// -1表示创建失败,否则传送[工作空间路径]，例如：'\\127.0.0.1\\QRST_DB_Share\\P1475369832501'！
        /// </summary>
        /// <param name="ordercode"></param>
        /// <returns></returns>
        string GetOrderWorkspace(string ordercode);

        /// <summary>
        /// 继续执行下一步订单
        /// </summary>
        /// <param name="ordercode"></param>
        /// <returns></returns>
        bool ResumeGF1DataImportOrder(string ordercode);

        /// <summary>
        /// 更改订单状态,使订单进入等待状态 zxw 20130818
        /// </summary>
        /// <param name="orderCode"></param>
        void SetOrderStatus2Waiting(string orderCode);

        /// <summary>
        /// 更改订单状态,使订单进入等待状态 joki 131128
        /// </summary>
        /// <param name="ordercode"></param>
        /// <returns></returns>
        string GetOrderStatus(string ordercode);

        int DownLoad_P2P(string dataName, string dataSharePath, string opID, string webserviceIp);

        int DownLoad(string dataID, string pathID, string opID, string GFdataName, string webservice);

        int DownLoad(string dataID, string pathID, string opID);


    }
}
