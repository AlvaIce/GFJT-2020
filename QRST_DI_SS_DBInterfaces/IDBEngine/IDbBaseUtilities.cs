using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;

namespace QRST_DI_SS_DBInterfaces.IDBEngine
{
    public interface IDbBaseUtilities
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="subdbname">BSDB MIDB EVDB...</param>
        /// <returns></returns>
        IDbBaseUtilities GetSubDBUtil(string subdbname);

        IDbBaseUtilities GetSubDbUtilByCon(string conStr);
        bool ConnectTest();
        /// <summary>
        /// 打开数据库连接
        /// </summary>
        void Open();
        void Close();
        /// <summary>
        /// 执行查询语句，返回OracleDataReader ( 注意：调用该方法后，一定要对OracleDataReader进行Close )
        /// </summary>
        /// <param name="sql">查询语句</param>
        /// <returns>OracleDataReader</returns>   
        DbDataReader ExecuteReader(string sql);

        /// <summary>
        /// 执行SQL语句，返回影响的记录数
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <returns>影响的记录数</returns>   
        int ExecuteSql(string sql, params DbParameter[] cmdParms);

        /// <summary>
        /// 执行NonQuery SQL语句
        /// </summary>
        /// <param name="sql">SQL语句</param>     
        int ExecuteSql(string sql);

        /// <summary>
        /// 执行SQL语句，返回数据到DataSet中
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <returns>返回DataSet</returns>
        DataSet GetDataSet(string sql);

        /// <summary>
        /// 执行SQL语句，返回数据到DataSet中
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <param name="DataSetName">自定义返回的DataSet表名</param>
        /// <returns>返回DataSet</returns>
        DataSet GetSpecNameDataSet(string sql, string dataSetName);

        DataSet GetDataSetByPath(string sql, string path);
        DataSet GetDataSet(string sql, string DataSourcePath, string tabielname, int currentIndex, int pageSize);
        /// <summary>
        /// 添加表到数据库中
        /// </summary>
        /// <param name="ds"></param>
        /// <returns></returns>
        void AddDataTableToDB(string tableName, DataTable dt);

        /// <summary>
        /// 执行Sql语句,返回带分页功能的自定义dataset
        /// </summary>
        /// <param name="sql">Sql语句</param>
        /// <param name="PageSize">每页显示记录数</param>
        /// <param name="CurrPageIndex">当前页</param>
        /// <param name="DataSetName">返回dataset表名</param>
        /// <returns>返回DataSet</returns>
        DataSet GetDataSet(string sql, int PageSize, int CurrPageIndex, string DataSetName);

        /// <summary>
        /// 执行SQL语句，返回记录总数
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <returns>返回记录总条数</returns>
        int GetRecordCount(string sql);

        /// <summary>
        /// 统计某表记录总数
        /// </summary>
        /// <param name="KeyField">主键/索引键</param>
        /// <param name="TableName">数据库.用户名.表名</param>
        /// <param name="Condition">查询条件</param>
        /// <returns>返回记录总数</returns> 
        int GetRecordCount(string keyField, string tableName, string condition);
        /// <summary>
        /// 统计某表记录总数
        /// </summary>
        /// <param name="Field">可重复的字段</param>
        /// <param name="tableName">数据库.用户名.表名</param>
        /// <param name="condition">查询条件</param>
        /// <param name="flag">字段是否主键</param>
        /// <returns>返回记录总数</returns> 
        int GetRecordCount(string Field, string tableName, string condition, bool flag);

        /// <summary>
        /// 统计某表分页总数
        /// </summary>
        /// <param name="keyField">主键/索引键</param>
        /// <param name="tableName">表名</param>
        /// <param name="condition">查询条件</param>
        /// <param name="pageSize">页宽</param>
        /// <param name="RecordCount">记录总数</param>
        /// <returns>返回分页总数</returns> 
        int GetPageCount(string keyField, string tableName, string condition, int pageSize, int RecordCount);
        /// <summary>
        /// 统计某表分页总数
        /// </summary>
        /// <param name="keyField">主键/索引键</param>
        /// <param name="tableName">表名</param>
        /// <param name="condition">查询条件</param>
        /// <param name="pageSize">页宽</param>
        /// <returns>返回页面总数</returns> 
        int GetPageCount(string keyField, string tableName, string condition, int pageSize, ref int RecordCount);
        /// <summary>
        /// 统计某表分页总数
        /// </summary>
        /// <param name="Field">可重复的字段</param>
        /// <param name="tableName">表名</param>
        /// <param name="condition">查询条件</param>
        /// <param name="pageSize">页宽</param>
        /// <param name="flag">是否主键</param>
        /// <returns>返回页页总数</returns> 
        int GetPageCount(string Field, string tableName, string condition, ref int RecordCount, int pageSize, bool flag);

        /// <summary>
        /// 构造分页查询SQL语句
        /// </summary>
        /// <param name="KeyField">主键</param>
        /// <param name="FieldStr">所有需要查询的字段(field1,field2...)</param>
        /// <param name="TableName">库名.拥有者.表名</param>
        /// <param name="where">查询条件1(where ...)</param>
        /// <param name="order">排序条件2(order by ...)</param>
        /// <param name="CurrentPage">当前页号</param>
        /// <param name="PageSize">页宽</param>
        /// <returns>SQL语句</returns> 
        string JoinPageSQL(string KeyField, string FieldStr, string TableName, string Where, string Order,
            int CurrentPage, int PageSize);

        /// <summary>
        /// 构造分页查询SQL语句
        /// </summary>
        /// <param name="Field">字段名(非主键)</param>
        /// <param name="TableName">库名.拥有者.表名</param>
        /// <param name="where">查询条件1(where ...)</param>
        /// <param name="order">排序条件2(order by ...)</param>
        /// <param name="CurrentPage">当前页号</param>
        /// <param name="PageSize">页宽</param>
        /// <returns>SQL语句</returns> 
        string JoinPageSQL(string Field, string TableName, string Where, string Order, int CurrentPage, int PageSize);

        /// <summary>
        /// 获取某表某个字段的最大值
        /// </summary>
        /// <param name="FieldName"></param>
        /// <param name="TableName"></param>
        /// <returns></returns>
        int GetMaxID(string FieldName, string TableName);

        string myExcuteScalar(string sql);

        string GetDbConnection();
        List<string> myExcuteReader(string sql);

        int ExecuteSqlTran(List<String> SQLStringList, out string errorMsg);

        /// <summary>
        /// 更新表
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="dt"></param>
        void UpdateTable(string sql, DataTable dt);
        void UpdateTable(string sql);
        #region 瓦片库操作接口

        #region 在sqlite中插入已上传文件的记录

        void CreateDBfile(string DataSourcePath);
        #endregion
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sqlstrs"></param>
        /// <param name="DataSourcePath"></param>
        void InSertData(List<string> sqlstrs, string DataSourcePath);

        void UpdateTableGFF(string DataSourcePath);

        int ExecuteTileQuery(string sql, string DataSourcePath);
        /// <summary>
        /// 效率低，每次更新要触发GFF表更新，多记录处理时建议使用
        /// InSertData(List<string> sqlstrs, string DataSourcePath)
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="DataSourcePath"></param>
        void InSertData(string sql, string DataSourcePath);

        #region 删除sqlite数据库中相应已删除文件记录

        void DeleteData(List<string> sqlstrs, string DataSourcePath);

        /// <summary>
        /// 效率低，每次更新要触发GFF表更新，
        /// 多记录处理时建议使用DeleteData(List<string> sqlstrs, string DataSourcePath)
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="DataSourcePath"></param>
        void DeleteData(string sql, string DataSourcePath);

        #endregion

        /// <summary>
        /// 返回分页查询结果的方法
        /// </summary>
        /// <param name="TableName">表名</param>
        /// <param name="returnFields">查询返回的字段列表，用“，”分割</param>
        /// <param name="strWhere">查询条件，where后边的内容，应包括括号</param>
        /// <param name="size">查询结果的总记录数</param>
        /// <param name="startIndex">查询的起始索引，从0开始</param>
        /// <param name="num">分页查询的每页记录数</param>
        /// <returns>查询结果数据集</returns>
        /// <returns></returns>
        DataSet GetList(string TableName, string returnFields, string strWhere, out int size, int startIndex = 0, int num = -1);

               /// <summary>
               /// 返回分页查询结果的方法
               /// </summary>
               /// <param name="TableName">表名</param>
               /// <param name="returnFields">查询返回的字段列表，用“，”分割</param>
               /// <param name="strWhere">查询条件，where后边的内容，应包括括号</param>
               /// <param name="orderByString">排序参数，示例“ ID DESC”，“ ID ASC”，“ ID”</param>
               /// <param name="size"></param>
               /// <param name="startIndex"></param>
               /// <param name="num"></param>
               /// <returns></returns>
        DataSet GetList(string TableName, string returnFields, string strWhere, string orderByString, out int size, int startIndex = 0, int num = -1);

        /// <summary>
        /// 获取获取不同数据库引擎的表结构
        /// </summary>
        /// <param name="tblName"></param>
        /// <returns></returns>
        DataSet GetTableStruct(string tblName);

        void UpdateDistinctTables(string DataSourcePath);

        /// <summary>
        /// 获取不同数据库引擎的当前时间
        /// </summary>
        /// <returns></returns>
        DateTime GetDBNowTime();
        #endregion
    }
}
