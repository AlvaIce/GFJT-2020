/***
 * 描述:SQLite操作基础类,包括瓦片元数据数据库和基础数据库
 * 作者：jianghua
 * 时间：20151111
 */
  
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SQLite;
using System.IO;
using System.Text;
using QRST_DI_Resources;
using QRST_DI_SS_DBInterfaces.IDBEngine;
using System.Threading;

namespace QRST_DI_DS_DBEngine
{
    [Serializable]
    public class SQLiteBaseUtilities : MarshalByRefObject, IDbBaseUtilities
    {
        public SQLiteConnection con;//      默认MIDB库连接对象
        public string connString = "";
        public static string _sqlitePWD = "987654";
        public SQLiteBaseUtilities(string constr)
        {
            con = new SQLiteConnection(constr);
            connString = constr;
            InitializeLifetimeService();
        }

        /// <summary>
        /// 线程锁对象
        /// </summary>
        private object lockObj = new object();
        public SQLiteBaseUtilities()
        {
            string SqliteCon = Constant.ConnectionStringSQLite;
            if (!string.IsNullOrEmpty(SqliteCon))
            {
                connString = SqliteCon;
                con = new SQLiteConnection(SqliteCon);
            }
        }

        public IDbBaseUtilities GetSubDBUtil(string subdbname)
        {
            SQLiteBaseUtilities MIDB = new SQLiteBaseUtilities();
            if (subdbname.ToUpper() == "MIDB")
            {
                return MIDB;
            }
            else
            {
                string sql = "select ConnectStr from subdbinfo where Name = '" + subdbname.ToUpper().Trim() + "';";
                DataSet ds = MIDB.GetDataSet(sql);
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    string connstr = ds.Tables[0].Rows[0][0].ToString();
                    return new SQLiteBaseUtilities(connstr);
                }
            }
            return null;
        }

        /// <summary>
        /// 重写远程对象生存周期。默认远程对象一段时间后删除，重写后永久保存。
        /// </summary>
        /// <returns></returns>
        public override object InitializeLifetimeService()
        {
            return null;
        }
        public IDbBaseUtilities GetSubDbUtilByCon(string conStr)
        {
            connString = conStr;
            SQLiteBaseUtilities utilities = new SQLiteBaseUtilities(conStr);
            return utilities;
        }

        /// <summary>
        /// 获取当前时间
        /// </summary>
        /// <returns></returns>
        public DateTime GetDBNowTime()
        {
            string sql = "select datetime('now','+8 hour')";      //sqlite当前时间 @jianghua 
            string dt = GetDataSet(sql).Tables[0].Rows[0][0].ToString();
            return Convert.ToDateTime(dt);      //2015-06-24 11:53:39
        }

        public string GetDbConnection()
        {
            return connString;
        }
        #region 打开数据库连接
        public bool ConnectTest()
        {
            bool suc = false;

            //判断连接的状态是否已经打开
            if (con.State == ConnectionState.Open)
            {
                con.Close();
            }

            //打开数据库连接
            if (con.State == ConnectionState.Closed)
            {
                try
                {
                    //打开数据库连接
                    con.Open();
                    suc = true;
                }
                catch (Exception e)
                {
                    throw new Exception("ConnectTest", e);
                }
                finally
                {
                    //判断连接的状态是否已经打开
                    if (con.State == ConnectionState.Open)
                    {
                        con.Close();
                    }
                }
            }

            return suc;
        }

        /// <summary>
        /// 打开数据库连接
        /// </summary>
        public void Open()
        {
            //打开数据库连接
            if (con.State == ConnectionState.Closed)
            {
                try
                {
                    //打开数据库连接
                    con.Open();
                }
                catch (Exception e)
                {
                    //con.Close();
                    throw new Exception("Open", e);
                }
            }
        }
        #endregion

        /// <summary>
        /// 获取表结构，sqlite库移除第一列。兼容MySQL库
        /// </summary>
        /// <param name="tblName"></param>
        /// <returns></returns>
        public DataSet GetTableStruct(string tblName)
        {
            string sql = string.Format("pragma table_info({0})", tblName);
            DataSet ds = null;
            try
            {
                ds = GetDataSet(sql);
                //只保留name和type列
                ds.Tables[0].Columns.RemoveAt(0);
                ds.Tables[0].Columns.RemoveAt(4);
                ds.Tables[0].Columns.RemoveAt(3);
                ds.Tables[0].Columns.RemoveAt(2);
            }
            catch (Exception e)
            {
                throw new Exception("获取表结构错误",e);
            }
            finally
            {

            }
            return ds;
        }

        #region 关闭数据库连接
        /// <summary>
        /// 关闭数据库连接
        /// </summary>
        public void Close()
        {
            //判断连接的状态是否已经打开
            if (con.State == ConnectionState.Open)
            {
                con.Close();
            }
        }
        #endregion

        #region 执行查询语句，返回SQLiteDataReader ( 注意：调用该方法后，一定要对SQLiteDataReader进行Close )
        /// <summary>
        /// 执行查询语句，返回SQLiteDataReader ( 注意：调用该方法后，一定要对SQLiteDataReader进行Close )
        /// </summary>
        /// <param name="sql">查询语句</param>
        /// <returns>手动转 SQLiteDataReader对象</returns>   
        public DbDataReader ExecuteReader(string sql)
        {
            try
            {
                Monitor.Enter(lockObj);
                //添加了连接的开启与关闭操作 zxw 2013/04/21
                SQLiteDataReader myReader;
                Open();
                using (SQLiteCommand cmd = new SQLiteCommand(sql, con))
                {
                    myReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                    return myReader;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("ExecuteReader", ex);
            }
            finally
            {
                Close();
                Monitor.Exit(lockObj);
            }
        }
        #endregion


        #region 执行带参数的SQL语句
        /// <summary>
        /// 执行SQL语句，返回影响的记录数
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <param name="cmdParms">SQLiteParameter[] 对象</param>
        /// <returns>影响的记录数</returns>   
        public int ExecuteSql(string sql, params DbParameter[] cmdParms)
        {
            //添加了连接的开启与关闭操作 zxw 2013/04/21
            SQLiteCommand cmd = new SQLiteCommand();
            {
                try
                {
                    Monitor.Enter(lockObj);
                    cmd.Connection = con;
                    PrepareCommand(cmd, con, null, CommandType.Text, sql, cmdParms as SQLiteParameter[]);
                    int rows = cmd.ExecuteNonQuery();
                    cmd.Parameters.Clear();
                    return rows;
                }
                catch (SQLiteException e)
                {
                    throw new Exception("ExecuteSql", e);
                }
                finally
                {
                    Close();
                    Monitor.Exit(lockObj);
                }
            }
        }


        private void PrepareCommand(SQLiteCommand cmd, SQLiteConnection conn, SQLiteTransaction trans, CommandType cmdType, string cmdText, SQLiteParameter[] cmdParms)
        {

            //判断数据库连接状态
            if (conn.State != ConnectionState.Open)
                conn.Open();

            cmd.Connection = conn;
            cmd.CommandText = cmdText;

            //判断是否需要事物处理
            if (trans != null)
                cmd.Transaction = trans;

            cmd.CommandType = cmdType;

            if (cmdParms != null)
            {
                foreach (SQLiteParameter parm in cmdParms)
                    cmd.Parameters.Add(parm);
            }
        }

        #endregion
        #region 执行带参数的SQL语句
        /// <summary>
        /// 执行NonQuery SQL语句
        /// </summary>
        /// <param name="sql">SQL语句</param>     
        public int ExecuteSql(string sql)
        {
            SQLiteCommand cmd = new SQLiteCommand(sql, con);
            int resultrow = 0;
            try
            {
                Monitor.Enter(lockObj);
                Open();
                resultrow = cmd.ExecuteNonQuery();
                return resultrow;
            }
            catch (Exception e)
            {
                throw new Exception("ExecuteSql",e);
            }
            finally
            {
                Close();
                Monitor.Exit(lockObj);
            }
        }

        /// <summary>
        /// 执行NonQuery SQL语句
        /// </summary>
        /// <param name="sql">SQL语句</param>     
        public int ExecuteScript(string sql)
        {
            SQLiteCommand cmd = new SQLiteCommand(sql, con);
            int resultrow = 0;
            try
            {
                Monitor.Enter(lockObj);
                Open();
                resultrow = cmd.ExecuteNonQuery();
                Close();
                return resultrow;
            }
            catch (SQLiteException e)
            {
                throw;

            }
            finally
            {
                Monitor.Exit(lockObj);
            }

        }

        public int ExecutConSQL(string p_con, string p_sql)
        {

            SQLiteConnection sqlCon = new SQLiteConnection(p_con);
            SQLiteCommand sqlCmd;
            int resultrow = 0;
            try
            {
                Monitor.Enter(lockObj);
                if (sqlCon.State == ConnectionState.Closed)
                {
                    sqlCon.Open();
                }
                sqlCmd = new SQLiteCommand(p_sql, sqlCon);
                resultrow = sqlCmd.ExecuteNonQuery();
                return resultrow;
            }
            catch (SQLiteException e)
            {
                throw;

            }
            finally
            {
                Close();
                Monitor.Exit(lockObj);
            }
        }
        #endregion
        #region 执行SQL语句，返回数据到DataSet中
        int _outTimeEx = 0;
        /// <summary>
        /// 执行SQL语句，返回数据到DataSet中
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <returns>返回DataSet</returns>
        public DataSet GetDataSet(string sql)
        {
            DataSet ds = new DataSet();
            try
            {
                Monitor.Enter(lockObj);
                Open();//打开数据连接
                SQLiteDataAdapter adapter = new SQLiteDataAdapter(sql, con);
                int dd = adapter.Fill(ds);
            }
            catch (Exception ex)
            {
                if (ex.Message.StartsWith("Timeout expired") && _outTimeEx < 5)
                {
                    _outTimeEx++;
                    ds = GetDataSet(sql);
                }
                else
                {
                    throw;
                }
                //if (con.State == ConnectionState.Closed)
                //{
                //    Open();//打开数据连接
                //    SQLiteDataAdapter adapter = new SQLiteDataAdapter(sql, con);
                //    int dd = adapter.Fill(ds);

                //}
            }
            finally
            {
                Close();//关闭数据库连接
                _outTimeEx = 0;
                Monitor.Exit(lockObj);
            }
            return ds;
            //}
        }

        #endregion
        #region 执行SQL语句，返回数据到自定义DataSet中
        int _outTimeEx2 = 0;
        /// <summary>
        /// 执行SQL语句，返回数据到DataSet中
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <param name="DataSetName">自定义返回的DataSet表名</param>
        /// <returns>返回DataSet</returns>
        public DataSet GetSpecNameDataSet(string sql, string DataSetName)
        {
            DataSet ds = new DataSet();
            try
            {
                Monitor.Enter(lockObj);
                Open();//打开数据连接
                SQLiteDataAdapter adapter = new SQLiteDataAdapter(sql, con);
                adapter.Fill(ds, DataSetName);
            }
            catch (Exception ex)
            {
                if (ex.Message.StartsWith("Timeout expired") && _outTimeEx2 < 5)
                {
                    _outTimeEx2++;
                    ds = GetSpecNameDataSet(sql, DataSetName);
                }
                else
                {
                    throw;
                }
                //if (con.State == ConnectionState.Closed)
                //{
                //    Open();//打开数据连接
                //    SQLiteDataAdapter adapter = new SQLiteDataAdapter(sql, con);
                //    int dd = adapter.Fill(ds);

                //}
            }
            finally
            {
                Close();//关闭数据库连接
                _outTimeEx2 = 0;
                Monitor.Exit(lockObj);
            }
            return ds;
            //}
        }

        #endregion

        #region 执行Sql语句,返回带分页功能的自定义dataset
        int _outTimeEx3 = 0;
        /// <summary>
        /// 执行Sql语句,返回带分页功能的自定义dataset
        /// </summary>
        /// <param name="sql">Sql语句</param>
        /// <param name="PageSize">每页显示记录数</param>
        /// <param name="CurrPageIndex">当前页</param>
        /// <param name="DataSetName">返回dataset表名</param>
        /// <returns>返回DataSet</returns>
        public DataSet GetDataSet(string sql, int PageSize, int CurrPageIndex, string DataSetName)
        {
            DataSet ds = new DataSet();
            try
            {
                Monitor.Enter(lockObj);
                Open();//打开数据连接
                SQLiteDataAdapter adapter = new SQLiteDataAdapter(sql, con);
                adapter.Fill(ds, PageSize * (CurrPageIndex - 1), PageSize, DataSetName);
            }
            catch (Exception ex)
            {
                if (ex.Message.StartsWith("Timeout expired") && _outTimeEx3 < 5)
                {
                    _outTimeEx3++;
                    ds = GetDataSet(sql, PageSize, CurrPageIndex, DataSetName);
                }
                else
                {
                    throw;
                }
                //if (con.State == ConnectionState.Closed)
                //{
                //    Open();//打开数据连接
                //    SQLiteDataAdapter adapter = new SQLiteDataAdapter(sql, con);
                //    int dd = adapter.Fill(ds);

                //}
            }
            finally
            {
                Close();//关闭数据库连接
                _outTimeEx3 = 0;
                Monitor.Exit(lockObj);
            }
            return ds;

        }
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
        public DataSet GetList(string TableName, string returnFields, string strWhere, out int size, int startIndex = 0, int num = -1)
        {

            //lock (lockObj)
            //{
            StringBuilder strSql = new StringBuilder();
            StringBuilder strsqlCount = new StringBuilder();
            strSql.Append(string.Format("select {0} FROM ", returnFields));
            strSql.Append(TableName);

            strsqlCount.Append("select count(*) FROM ");
            strsqlCount.Append(TableName);
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
                strsqlCount.Append(" where " + strWhere);
            }
            DataSet dsCount = null, ds = null;
            size = 0;
            try
            {
                Monitor.Enter(lockObj);
                dsCount = GetDataSet(strsqlCount.ToString());

                size = int.Parse(dsCount.Tables[0].Rows[0][0].ToString());
                if (num > 0)
                {
                    strSql.Append(" limit " + startIndex + "," + num);
                }
                ds = GetDataSet(strSql.ToString());
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                Monitor.Exit(lockObj);
            }
            return ds;
            //}
        }
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
        public DataSet GetList(string TableName, string returnFields, string strWhere, string orderByString, out int size, int startIndex = 0, int num = -1)
        {

            //lock (lockObj)
            //{
            StringBuilder strSql = new StringBuilder();
            StringBuilder strsqlCount = new StringBuilder();
            strSql.Append(string.Format("select {0} FROM ", returnFields));
            strSql.Append(TableName);

            strsqlCount.Append("select count(*) FROM ");
            strsqlCount.Append(TableName);
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
                strsqlCount.Append(" where " + strWhere);
            }
            if (!string.IsNullOrEmpty(orderByString.Trim()))
            {
                strSql.Append(string.Format(" ORDER BY {0}", orderByString));
            }

            DataSet dsCount = null, ds = null;
            size = 0;
            try
            {
                Monitor.Enter(lockObj);
                dsCount = GetDataSet(strsqlCount.ToString());

                size = int.Parse(dsCount.Tables[0].Rows[0][0].ToString());
                if (num > 0)
                {
                    strSql.Append(" limit " + startIndex + "," + num);
                }
                ds = GetDataSet(strSql.ToString());
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                Monitor.Exit(lockObj);
            }
            return ds;
            //}
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="TableName"></param>
        /// <param name="Fields_TableString">多表联合查询时，查询语句前半段  不能由returnFields和TableName组合</param>
        /// <param name="strWhere"></param>
        /// <param name="size"></param>
        /// <param name="startIndex"></param>
        /// <param name="num"></param>
        /// <returns></returns>
        public DataSet GetList2(string TableName, string Fields_TableString, string strWhere, out int size, int startIndex = 0, int num = -1)
        {

            //lock (lockObj)
            //{
            StringBuilder strSql = new StringBuilder();
            StringBuilder strsqlCount = new StringBuilder();
            //strSql.Append(string.Format("select {0} FROM ", returnFields));
            strSql.Append(Fields_TableString);

            strsqlCount.Append("select count(*) FROM ");
            strsqlCount.Append(TableName);
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
                strsqlCount.Append(" where " + strWhere);
            }
            DataSet dsCount = null, ds = null;
            size = 0;
            try
            {
                Monitor.Enter(lockObj);
                dsCount = GetDataSet(strsqlCount.ToString());

                size = int.Parse(dsCount.Tables[0].Rows[0][0].ToString());
                if (num > 0)
                {
                    strSql.Append(" limit " + startIndex + "," + num);
                }
                ds = GetDataSet(strSql.ToString());
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                Monitor.Exit(lockObj);
            }
            return ds;
            //}
        }
        #endregion
        #region 执行SQL语句，返回记录总数
        /// <summary>
        /// 执行SQL语句，返回记录总数
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <returns>返回记录总条数</returns>
        public int GetRecordCount(string sql)
        {

            //lock (lockObj)
            //{
            int recordCount = 0;
            try
            {
                Monitor.Enter(lockObj);
                Open();//打开数据连接
                SQLiteCommand command = new SQLiteCommand(sql, con);
                using (SQLiteDataReader dataReader = command.ExecuteReader())
                {
                    while (dataReader.Read())
                    {
                        recordCount++;
                    }
                }
            }
            catch (Exception e)
            {
                throw;
            }
            finally
            {
                Close();
                Monitor.Exit(lockObj);
            }
            return recordCount;
            //SQLiteDataReader dataReader=
            //try
            //{
            //    SQLiteDataReader dataReader = command.ExecuteReader();
            //    while (dataReader.Read())
            //    {
            //        recordCount++;
            //    }
            //    return recordCount;
            //}
            //catch(Exception ex)
            //{
            //    throw ex;
            //    return recordCount;
            //}
            //finally
            //{
            //    dataReader.Close();
            //    Close();//关闭数据库连接				
            //}
            //}
        }
        #endregion
        #region 统计某表记录总数
        /// <summary>
        /// 统计某表记录总数
        /// </summary>
        /// <param name="KeyField">主键/索引键</param>
        /// <param name="TableName">数据库.用户名.表名</param>
        /// <param name="Condition">查询条件</param>
        /// <returns>返回记录总数</returns> 
        public int GetRecordCount(string keyField, string tableName, string condition)
        {
            int RecordCount = 0;
            string sql = "select count(" + keyField + ") as count from " + tableName + " " + condition;
            DataSet ds = GetDataSet(sql);
            if (ds.Tables[0].Rows.Count > 0)
            {
                RecordCount = Convert.ToInt32(ds.Tables[0].Rows[0][0]);
            }
            ds.Clear();
            ds.Dispose();
            return RecordCount;
        }
        /// <summary>
        /// 统计某表记录总数
        /// </summary>
        /// <param name="Field">可重复的字段</param>
        /// <param name="tableName">数据库.用户名.表名</param>
        /// <param name="condition">查询条件</param>
        /// <param name="flag">字段是否主键</param>
        /// <returns>返回记录总数</returns> 
        public int GetRecordCount(string Field, string tableName, string condition, bool flag)
        {
            int RecordCount = 0;
            if (flag)
            {
                RecordCount = GetRecordCount(Field, tableName, condition);
            }
            else
            {
                string sql = "select count(distinct(" + Field + ")) as count from " + tableName + " " + condition;
                DataSet ds = GetDataSet(sql);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    RecordCount = Convert.ToInt32(ds.Tables[0].Rows[0][0]);
                }
                ds.Clear();
                ds.Dispose();
            }
            return RecordCount;
        }
        #endregion
        #region 统计某表分页总数
        /// <summary>
        /// 统计某表分页总数
        /// </summary>
        /// <param name="keyField">主键/索引键</param>
        /// <param name="tableName">表名</param>
        /// <param name="condition">查询条件</param>
        /// <param name="pageSize">页宽</param>
        /// <param name="RecordCount">记录总数</param>
        /// <returns>返回分页总数</returns> 
        public int GetPageCount(string keyField, string tableName, string condition, int pageSize, int RecordCount)
        {
            int PageCount = 0;
            PageCount = (RecordCount % pageSize) > 0 ? (RecordCount / pageSize) + 1 : RecordCount / pageSize;
            if (PageCount < 1) PageCount = 1;
            return PageCount;
        }
        /// <summary>
        /// 统计某表分页总数
        /// </summary>
        /// <param name="keyField">主键/索引键</param>
        /// <param name="tableName">表名</param>
        /// <param name="condition">查询条件</param>
        /// <param name="pageSize">页宽</param>
        /// <returns>返回页面总数</returns> 
        public int GetPageCount(string keyField, string tableName, string condition, int pageSize, ref int RecordCount)
        {
            RecordCount = GetRecordCount(keyField, tableName, condition);
            return GetPageCount(keyField, tableName, condition, pageSize, RecordCount);
        }
        /// <summary>
        /// 统计某表分页总数
        /// </summary>
        /// <param name="Field">可重复的字段</param>
        /// <param name="tableName">表名</param>
        /// <param name="condition">查询条件</param>
        /// <param name="pageSize">页宽</param>
        /// <param name="flag">是否主键</param>
        /// <returns>返回页页总数</returns> 
        public int GetPageCount(string Field, string tableName, string condition, ref int RecordCount, int pageSize, bool flag)
        {
            RecordCount = GetRecordCount(Field, tableName, condition, flag);
            return GetPageCount(Field, tableName, condition, pageSize, ref RecordCount);
        }
        #endregion
        #region Sql分页函数
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
        public string JoinPageSQL(string KeyField, string FieldStr, string TableName, string Where, string Order, int CurrentPage, int PageSize)
        {
            string sql = null;
            if (CurrentPage == 1)
            {
                sql = "select  " + CurrentPage * PageSize + " " + FieldStr + " from " + TableName + " " + Where + " " + Order + " ";
            }
            else
            {
                sql = "select * from (";
                sql += "select  " + CurrentPage * PageSize + " " + FieldStr + " from " + TableName + " " + Where + " " + Order + ") a ";
                sql += "where " + KeyField + " not in (";
                sql += "select  " + (CurrentPage - 1) * PageSize + " " + KeyField + " from " + TableName + " " + Where + " " + Order + ")";
            }
            return sql;
        }
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
        public string JoinPageSQL(string Field, string TableName, string Where, string Order, int CurrentPage, int PageSize)
        {
            string sql = null;
            if (CurrentPage == 1)
            {
                sql = "select rownum " + CurrentPage * PageSize + " " + Field + " from " + TableName + " " + Where + " " + Order + " group by " + Field;
            }
            else
            {
                sql = "select * from (";
                sql += "select rownum " + CurrentPage * PageSize + " " + Field + " from " + TableName + " " + Where + " " + Order + " group by " + Field + " ) a ";
                sql += "where " + Field + " not in (";
                sql += "select rownum " + (CurrentPage - 1) * PageSize + " " + Field + " from " + TableName + " " + Where + " " + Order + " group by " + Field + ")";
            }
            return sql;
        }
        #endregion 
        #region 根据系统时间动态生成各种查询语句(现已经注释掉，以备以后使用)
        //  #region 根据查询时间的条件，动态生成查询语句
        //  /// <summary>
        //  /// 根据查询时间的条件，动态生成查询语句
        //  /// </summary>
        //  /// <param name="starttime">开始时间</param>
        //  /// <param name="endtime">结束时间</param>
        //  /// <param name="dw">单位</param>
        //  /// <param name="startxsl">开始线损率</param>
        //  /// <param name="endxsl">结束线损率</param>
        //  /// <param name="danwei">单位字段</param>
        //  /// <param name="xiansunlv">线损率字段</param>
        //  /// <param name="tablehz">表后缀</param>
        //  /// <returns>SQL语句</returns>
        //  public  string SQL(DateTime starttime,DateTime endtime,string dw,float startxsl,float endxsl,string danwei,string xiansunlv,string tablehz)
        //  {
        //
        //   string sql=null;
        //   //将输入的时间格式转换成固定的格式"yyyy-mm-dd"
        //   string zstarttime=starttime.GetDateTimeFormats('D')[1].ToString();
        //   string zendtime=endtime.GetDateTimeFormats('D')[1].ToString();
        //   string nTime=DateTime.Now.GetDateTimeFormats('D')[1].ToString();
        //
        //
        //   //取日期值的前六位，及年月值
        //   string sTime=zstarttime.Substring(0,4)+zstarttime.Substring(5,2);
        //   string eTime=zendtime.Substring(0,4)+zendtime.Substring(5,2);
        //   string nowTime=nTime.Substring(0,4)+nTime.Substring(5,2);
        //   //分别取日期的年和月
        //   int sy=Convert.ToInt32(zstarttime.Substring(0,4));
        //   int ey=Convert.ToInt32(zendtime.Substring(0,4));
        //   int sm=Convert.ToInt32(zstarttime.Substring(5,2));
        //   int em=Convert.ToInt32(zendtime.Substring(5,2));
        //   //相关变量定义
        //   int s;
        //   int e;
        //   int i;
        //   int j;
        //   int js;
        //   int nz;
        //   string x;
        //   //一，取当前表生成SQL语句
        //   if(sTime==nowTime&&eTime==nowTime)
        //   {
        //    sql="select  * from "+tablehz+" "+"where"+" "+danwei+"="+dw+" "+" "+"and"+" "+xiansunlv+">="+startxsl+" "+"and"+" "+xiansunlv+"<="+endxsl+" ";
        //   }
        //    //二，取当前表和其他表生成SQL语句
        //   else if(sTime==nowTime&&eTime!=nowTime)
        //   {
        //    sql="select  * from "+tablehz+" "+"where"+" "+danwei+"="+dw+" "+" "+"and"+" "+xiansunlv+">="+startxsl+" "+"and"+" "+xiansunlv+"<="+endxsl+" "+"union"+" ";
        //    //如果年份相等
        //    if(sy==ey)
        //    {
        //     s=Convert.ToInt32(sTime);
        //     e=Convert.ToInt32(eTime);
        //     for(i=s+1;i<e;i++)
        //     {
        //      i=i++;
        //      sql+="select  * from "+i.ToString()+'_'+tablehz+" "+"where"+" "+danwei+"="+dw+" "+" "+"and"+" "+xiansunlv+">="+startxsl+" "+"and"+" "+xiansunlv+"<="+endxsl+" "+"union"+" ";
        //     }
        //     sql+="select  * from "+e.ToString()+'_'+tablehz+" "+"where"+" "+danwei+"="+dw+" "+" "+"and"+" "+xiansunlv+">="+startxsl+" "+"and"+" "+xiansunlv+"<="+endxsl+" ";
        //    }
        //     //结束年份大于开始年份
        //    else
        //    {
        //     //1，先循环到起始时间和起始时间的12月
        //     s=Convert.ToInt32(sTime);
        //     x=zstarttime.Substring(0,4)+"12";
        //     nz=Convert.ToInt32(x);
        //     for(i=s+1;i<=nz;i++)
        //     {
        //      i=i++;
        //      sql+="select  * from "+i.ToString()+'_'+tablehz+" "+"where"+" "+danwei+"="+dw+" "+" "+"and"+" "+xiansunlv+">="+startxsl+" "+"and"+" "+xiansunlv+"<="+endxsl+" "+"union"+" ";
        //     }
        //     //2，循环两者相差年份
        //     for(i=sy+1;i<ey;i++)
        //     {
        //
        //      for(j=1;j<=12;j++)
        //      {
        //       if(j<10)
        //       {
        //        sql+="select  * from "+i.ToString()+"0"+j.ToString()+'_'+tablehz+" "+"where"+" "+danwei+"="+dw+" "+" "+"and"+" "+xiansunlv+">="+startxsl+" "+"and"+" "+xiansunlv+"<="+endxsl+" "+"union"+" ";
        //       }
        //       else
        //       {
        //        sql+="select  * from "+i.ToString()+j.ToString()+'_'+tablehz+" "+"where"+" "+danwei+"="+dw+" "+" "+"and"+" "+xiansunlv+">="+startxsl+" "+"and"+" "+xiansunlv+"<="+endxsl+" "+"union"+" ";
        //       }
        //      }
        //     }
        //     //3，循环到结束的月份
        //     js=Convert.ToInt32(zendtime.Substring(0,4)+"00");
        //     for(i=js;i<Convert.ToInt32(eTime);i++)
        //     {
        //      i++;
        //      sql+="select  * from "+i.ToString()+'_'+tablehz+" "+"where"+" "+danwei+"="+dw+" "+" "+"and"+" "+xiansunlv+">="+startxsl+" "+"and"+" "+xiansunlv+"<="+endxsl+" "+"union"+" ";
        //
        //     }
        //     sql+="select  * from "+eTime.ToString()+'_'+tablehz+" "+"where"+" "+danwei+"="+dw+" "+" "+"and"+" "+xiansunlv+">="+startxsl+" "+"and"+" "+xiansunlv+"<="+endxsl+" ";
        //
        //    }
        //   }
        //    //三，取其他表生成生成SQL语句
        //   else
        //   {
        //    //1，先循环到起始时间和起始时间的12月
        //    s=Convert.ToInt32(sTime);
        //    x=zstarttime.Substring(0,4)+"12";
        //    nz=Convert.ToInt32(x);
        //    for(i=s;i<=nz;i++)
        //    {
        //     i=i++;
        //     sql+="select  * from "+i.ToString()+'_'+tablehz+" "+"where"+" "+danwei+"="+dw+" "+" "+"and"+" "+xiansunlv+">="+startxsl+" "+"and"+" "+xiansunlv+"<="+endxsl+" "+"union"+" ";
        //    }
        //    //2，循环两者相差年份
        //    for(i=sy+1;i<ey;i++)
        //    {
        //
        //     for(j=1;j<=12;j++)
        //     {
        //      if(j<10)
        //      {
        //       sql+="select  * from "+i.ToString()+"0"+j.ToString()+'_'+tablehz+" "+"where"+" "+danwei+"="+dw+" "+" "+"and"+" "+xiansunlv+">="+startxsl+" "+"and"+" "+xiansunlv+"<="+endxsl+" "+"union"+" ";
        //      }
        //      else
        //      {
        //       sql+="select  * from "+i.ToString()+j.ToString()+'_'+tablehz+" "+"where"+" "+danwei+"="+dw+" "+" "+"and"+" "+xiansunlv+">="+startxsl+" "+"and"+" "+xiansunlv+"<="+endxsl+" "+"union"+" ";
        //      }
        //     }
        //    }
        //    //3，循环到结束的月份
        //    js=Convert.ToInt32(zendtime.Substring(0,4)+"00");
        //    for(i=js;i<Convert.ToInt32(eTime);i++)
        //    {
        //     i++;
        //     sql+="select  * from "+i.ToString()+'_'+tablehz+" "+"where"+" "+danwei+"="+dw+" "+" "+"and"+" "+xiansunlv+">="+startxsl+" "+"and"+" "+xiansunlv+"<="+endxsl+" "+"union"+" ";
        //
        //    }
        //    sql+="select  * from "+eTime.ToString()+'_'+tablehz+" "+"where"+" "+danwei+"="+dw+" "+" "+"and"+" "+xiansunlv+">="+startxsl+" "+"and"+" "+xiansunlv+"<="+endxsl+" ";
        //
        //   }
        //   return sql;
        //  }
        //  #endregion
        //
        //  #region 根据查询时间的条件，动态生成查询语句
        //  /// <summary>
        //  /// 根据查询时间的条件，动态生成查询语句
        //  /// </summary>
        //  /// <param name="starttime">开始时间</param>
        //  /// <param name="endtime">结束时间</param>
        //  /// <param name="zhiduan">查询字段</param>
        //  /// <param name="tiaojiao">查询条件</param>
        //  /// <param name="tablehz">表后缀</param>
        //  /// <returns>SQL语句</returns>
        //  public  string SQL(DateTime starttime,DateTime endtime,string zhiduan,string tiaojiao,string tablehz)
        //  {
        //
        //   string sql=null;
        //   //将输入的时间格式转换成固定的格式"yyyy-mm-dd"
        //   string zstarttime=starttime.GetDateTimeFormats('D')[1].ToString();
        //   string zendtime=endtime.GetDateTimeFormats('D')[1].ToString();
        //   string nTime=DateTime.Now.GetDateTimeFormats('D')[1].ToString();
        //
        //
        //   //取日期值的前六位，及年月值
        //   string sTime=zstarttime.Substring(0,4)+zstarttime.Substring(5,2);
        //   string eTime=zendtime.Substring(0,4)+zendtime.Substring(5,2);
        //   string nowTime=nTime.Substring(0,4)+nTime.Substring(5,2);
        //   //分别取日期的年和月
        //   int sy=Convert.ToInt32(zstarttime.Substring(0,4));
        //   int ey=Convert.ToInt32(zendtime.Substring(0,4));
        //   int sm=Convert.ToInt32(zstarttime.Substring(5,2));
        //   int em=Convert.ToInt32(zendtime.Substring(5,2));
        //   //相关变量定义
        //   int s;
        //   int e;
        //   int i;
        //   int j;
        //   int js;
        //   int nz;
        //   string x;
        //   //一，取当前表生成SQL语句
        //   if(sTime==nowTime&&eTime==nowTime)
        //   {
        //    sql="select"+" "+zhiduan+" "+"from"+" "+tablehz+" "+"where"+" "+tiaojiao+" ";
        //
        //   }
        //    //二，取当前表和其他表生成SQL语句
        //   else if(sTime==nowTime&&eTime!=nowTime)
        //   {
        //    sql="select"+" "+zhiduan+" "+"from"+" "+tablehz+" "+"where"+" "+tiaojiao+" "+"union"+" ";
        //
        //    //如果年份相等
        //    if(sy==ey)
        //    {
        //     s=Convert.ToInt32(sTime);
        //     e=Convert.ToInt32(eTime);
        //     for(i=s+1;i<e;i++)
        //     {
        //      i=i++;
        //      sql+="select"+" "+zhiduan+" "+"from"+" "+i.ToString()+'_'+tablehz+" "+"where"+" "+tiaojiao+" "+"union"+" ";
        //
        //     }
        //     sql+="select"+" "+zhiduan+" "+"from"+" "+e.ToString()+'_'+tablehz+" "+"where"+" "+tiaojiao+" ";
        //
        //    }
        //     //结束年份大于开始年份
        //    else
        //    {
        //     //1，先循环到起始时间和起始时间的12月
        //     s=Convert.ToInt32(sTime);
        //     x=zstarttime.Substring(0,4)+"12";
        //     nz=Convert.ToInt32(x);
        //     for(i=s+1;i<=nz;i++)
        //     {
        //      i=i++;
        //      sql+="select"+" "+zhiduan+" "+"from"+" "+i.ToString()+'_'+tablehz+" "+"where"+" "+tiaojiao+" "+"union"+" ";
        //
        //     }
        //     //2，循环两者相差年份
        //     for(i=sy+1;i<ey;i++)
        //     {
        //
        //      for(j=1;j<=12;j++)
        //      {
        //       if(j<10)
        //       {
        //        sql+="select"+" "+zhiduan+" "+"from"+" "+i.ToString()+"0"+j.ToString()+'_'+tablehz+" "+"where"+" "+tiaojiao+" "+"union"+" ";
        //
        //       }
        //       else
        //       {
        //        sql+="select"+" "+zhiduan+" "+"from"+" "+i.ToString()+j.ToString()+'_'+tablehz+" "+"where"+" "+tiaojiao+" "+"union"+" ";
        //       }
        //      }
        //     }
        //     //3，循环到结束的月份
        //     js=Convert.ToInt32(zendtime.Substring(0,4)+"00");
        //     for(i=js;i<Convert.ToInt32(eTime);i++)
        //     {
        //      i++;
        //      sql+="select"+" "+zhiduan+" "+"from"+" "+i.ToString()+'_'+tablehz+" "+"where"+" "+tiaojiao+" "+"union"+" ";
        //
        //     }
        //     sql+="select"+" "+zhiduan+" "+"from"+" "+eTime.ToString()+'_'+tablehz+" "+"where"+" "+tiaojiao+" ";
        //
        //    }
        //   }
        //    //三，取其他表生成生成SQL语句
        //   else
        //   {
        //    //1，先循环到起始时间和起始时间的12月
        //    s=Convert.ToInt32(sTime);
        //    x=zstarttime.Substring(0,4)+"12";
        //    nz=Convert.ToInt32(x);
        //    for(i=s;i<=nz;i++)
        //    {
        //     i=i++;
        //     sql+="select"+" "+zhiduan+" "+"from"+" "+i.ToString()+'_'+tablehz+" "+"where"+" "+tiaojiao+" "+"union"+" ";
        //
        //    }
        //    //2，循环两者相差年份
        //    for(i=sy+1;i<ey;i++)
        //    {
        //
        //     for(j=1;j<=12;j++)
        //     {
        //      if(j<10)
        //      {
        //       sql+="select"+" "+zhiduan+" "+"from"+" "+i.ToString()+"0"+j.ToString()+'_'+tablehz+" "+"where"+" "+tiaojiao+" "+"union"+" ";
        //
        //      }
        //      else
        //      {
        //       sql+="select"+" "+zhiduan+" "+"from"+" "+i.ToString()+j.ToString()+'_'+tablehz+" "+"where"+" "+tiaojiao+" "+"union"+" ";
        //      }
        //     }
        //    }
        //    //3，循环到结束的月份
        //    js=Convert.ToInt32(zendtime.Substring(0,4)+"00");
        //    for(i=js;i<Convert.ToInt32(eTime);i++)
        //    {
        //     i++;
        //     sql+="select"+" "+zhiduan+" "+"from"+" "+i.ToString()+'_'+tablehz+" "+"where"+" "+tiaojiao+" "+"union"+" ";
        //
        //    }
        //    sql+="select"+" "+zhiduan+" "+"from"+" "+eTime.ToString()+'_'+tablehz+" "+"where"+" "+tiaojiao+" ";
        //
        //   }
        //   return sql;
        //  }
        //  #endregion

        #endregion

        #region 更新表
        /// <summary>
        /// 更新表
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="dt"></param>
        public void UpdateTable(string sql, DataTable dt)
        {
            try
            {
                Monitor.Enter(lockObj);
                SQLiteDataAdapter sqlDtAdp = new SQLiteDataAdapter(sql, con);
                SQLiteCommandBuilder sqLiteCommand = new SQLiteCommandBuilder(sqlDtAdp);
                Open();
                sqlDtAdp.Update(dt);
            }
            catch (Exception ex)
            {
                throw new Exception("UpdateTable Error", ex);
            }
            finally
            {
                Close();
                Monitor.Exit(lockObj);
            }
        }

        public void UpdateTable(string sql)
        {
            try
            {
                Monitor.Enter(lockObj);
                SQLiteCommand sc = new SQLiteCommand(sql, con);
                Open();
                sc.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception("UpdateTable Error", ex);
            }
            finally
            {
                Close();
                Monitor.Exit(lockObj);
            }
        }
        #endregion

        //WYN 2012/04/17

        /// <summary>
        /// 添加数据集到数据库中
        /// </summary>
        /// <param name="ds"></param>
        /// <returns></returns>
        public void AddDatasetToDB(string tableName, DataSet ds)
        {
            string strsql = "select * from " + tableName;
            try
            {
                Monitor.Enter(lockObj);
                using (SQLiteDataAdapter myDataAdapter = new SQLiteDataAdapter(strsql, con))
                {
                    SQLiteCommandBuilder comm = new SQLiteCommandBuilder(myDataAdapter);

                    con.Open();
                    DataTable dt = ds.Tables[0];
                    foreach (DataRow dr in dt.Rows)
                    {
                        dr.SetAdded();
                    }
                    myDataAdapter.Update(dt);
                }
            }
            catch (Exception e)
            {
                throw;
            }
            finally
            {
                con.Close();
                Monitor.Exit(lockObj);
            }
            
        }


        /// <summary>
        /// 添加表到数据库中
        /// </summary>
        /// <param name="ds"></param>
        /// <returns></returns>
        public void AddDataTableToDB(string tableName, DataTable dt)
        {
            string strsql = "select * from " + tableName;
            try
            {

                Monitor.Enter(lockObj);
                using (SQLiteDataAdapter myDataAdapter = new SQLiteDataAdapter(strsql, con))
                {
                    SQLiteCommandBuilder comm = new SQLiteCommandBuilder(myDataAdapter);

                    Open();
                    myDataAdapter.Update(dt);
                }
            }
            catch (Exception e)
            {
                throw;
            }
            finally
            {
                Close();
                Monitor.Exit(lockObj);

            }
            
        }

        /// <summary>
        /// 获取某表某个字段的最大值
        /// </summary>
        /// <param name="FieldName"></param>
        /// <param name="TableName"></param>
        /// <returns></returns>
        public int GetMaxID(string FieldName, string TableName)
        {
            string strsql = String.Format("select max({0})+1 from {1}", FieldName, TableName);
            DataSet ds = GetDataSet(strsql);
            if (ds == null)
            {
                return 1;
            }
            else
            {
                try
                {
                    if (string.IsNullOrEmpty(ds.Tables[0].Rows[0][0].ToString()))
                    {
                        return 1;
                    }

                    string str = ds.Tables[0].Rows[0][0].ToString();
                    return int.Parse(ds.Tables[0].Rows[0][0].ToString());
                }
                catch (Exception ex)
                {

                    throw;
                }
            }
        }

        /// <summary>
        /// 执行多条SQL语句，实现数据库事务。
        /// </summary>
        /// <param name="SQLStringList">多条SQL语句</param>		
        public int ExecuteSqlTran(List<String> SQLStringList, out string errorMsg)
        {
            SQLiteTransaction tx = null;
            try
            {
                Monitor.Enter(lockObj);
                if (con.State != ConnectionState.Open)
                {
                    con.Open();
                }

                using (SQLiteCommand cmd = new SQLiteCommand { Connection = con })
                {
                    tx = con.BeginTransaction();
                    cmd.Transaction = tx;
                    int count = 0;
                    for (int n = 0; n < SQLStringList.Count; n++)
                    {
                        string strsql = SQLStringList[n];
                        if (strsql.Trim().Length > 1)
                        {
                            cmd.CommandText = strsql;
                            count += cmd.ExecuteNonQuery();
                        }
                    }
                    tx.Commit();
                    errorMsg = "";
                    return count;

                }
            }
            catch (Exception ex)
            {
                tx.Rollback();
                errorMsg = ex.ToString();
                return -1;
            }
            finally
            {
                Close();
                Monitor.Exit(lockObj);
            }

        }
    

        /// <summary>
        /// 获取SQLite表结构 @jh
        /// </summary>
        /// <param name="tName"></param>
        /// <returns></returns>
        public DataSet GetLiteTableSchema(string tName)
        {
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            string sql = string.Format("select * from {0}", tName);
            try
            {
                Monitor.Enter(lockObj);

                Open();//打开数据连接
                SQLiteCommand sCmd = new SQLiteCommand(sql, con);
                using (IDataReader dr = sCmd.ExecuteReader(CommandBehavior.KeyInfo | CommandBehavior.SchemaOnly))
                {
                    dt = dr.GetSchemaTable();
                    ds.Tables.Add(dt);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                con.Close();
                Monitor.Exit(lockObj);
            }
            return ds;


        }
        #region 数据库查询方法 （DLF 130618）
        /// <summary>
        /// 获取数据库中的某种类型的表中的所有卫星类型
        /// </summary>
        /// <param name="TableName"></param>
        /// <returns></returns>
        public List<string> getSatellites(string TableName)
        {
            List<string> listSatellites = new List<string>();

            string sqlstring = string.Format("select distinct SATELLITE  from {0} ", TableName);
            DataSet ds = GetDataSet(sqlstring);
            if (ds == null || ds.Tables.Count == 0)
            {
                return listSatellites;
            }
            else
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    string sate = ds.Tables[0].Rows[i]["SATELLITE"].ToString();

                    if (!string.IsNullOrEmpty(sate))
                    {
                        listSatellites.Add(sate);
                    }
                }
                return listSatellites;
            }
        }

        /// <summary>
        /// 获取数据库中的某种类型的表中的所有传感器类型
        /// </summary>
        /// <param name="TableName"></param>
        /// <returns></returns>
        public List<string> getSensors(string TableName)
        {
            List<string> listSatellites = new List<string>();

            string sqlstring = string.Format("select distinct SENSOR  from {0} ", TableName);
            DataSet ds = GetDataSet(sqlstring);
            if (ds == null || ds.Tables.Count == 0)
            {
                return listSatellites;
            }
            else
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    string sate = ds.Tables[0].Rows[i]["SENSOR"].ToString();

                    if (!string.IsNullOrEmpty(sate))
                    {
                        listSatellites.Add(sate);
                    }
                }
                return listSatellites;
            }
        }
        /// <summary>
        /// 在table_view表中查找 每张表对应的视图中作为关键字查询的字段列表
        /// </summary>
        /// <param name="TableName"></param>
        /// <returns></returns>
        public List<string> getKeyWordZiduan(string TableName)
        {
            List<string> listKeyWordZiduan = new List<string>();

            string sqlstring = string.Format("select VIEW_FIELD_NAME from table_view where TABLE_NAME = '{0}' and MEMO = 'keyWord'", TableName);
            DataSet ds = GetDataSet(sqlstring);
            if (ds == null || ds.Tables.Count == 0)
            {
                return listKeyWordZiduan;
            }
            else
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    string keyZiduan = ds.Tables[0].Rows[i]["VIEW_FIELD_NAME"].ToString();

                    if (!string.IsNullOrEmpty(keyZiduan.Trim()))
                    {
                        listKeyWordZiduan.Add(keyZiduan);
                    }
                }
                return listKeyWordZiduan;
            }
        }
        #endregion
        #region //written by Jiang Bin, 执行excutereader，并返回第一列的数据
        public List<string> myExcuteReader(string sql)
        {
            List<string> tablesname = new List<string>();

            SQLiteCommand cmd = new SQLiteCommand(sql, con);
            try
            {
                Open();
                using (SQLiteDataReader reader1 = cmd.ExecuteReader())
                {
                    while (reader1.Read())
                    {
                        tablesname.Add(reader1[0].ToString());
                    }
                }


            }
            catch (SQLiteException e)
            {
                if (con.State == ConnectionState.Closed)
                {
                    Open();
                    using (SQLiteDataReader reader1 = cmd.ExecuteReader())
                    {
                        while (reader1.Read())
                        {
                            tablesname.Add(reader1[0].ToString());
                        }

                    }

                }
            }
            finally
            {
                Close();
            }
            return tablesname;
        }
        #endregion

        #region//written by Jiang Bin;获取检索数据的第一个值
        public string myExcuteScalar(string sql)
        {
            string returnValue = "-1";
            SQLiteCommand cmd = new SQLiteCommand(sql, con);
            try
            {
                Monitor.Enter(lockObj);
                Open();
                returnValue = cmd.ExecuteScalar().ToString();
                Close();
                return returnValue;
            }
            catch (SQLiteException e)
            {

                if (con.State == ConnectionState.Closed)
                {
                    Open();
                    returnValue = cmd.ExecuteScalar().ToString();
                    Close();

                }
                //throw e;
                return returnValue;
            }
            finally
            {
                Monitor.Exit(lockObj);
            }
        }
        #endregion

        #region//written by Jiang Bin, 检查备份的订单提交状态
        public int checkingstatus(string sql)
        {

            SQLiteCommand mycmd = new SQLiteCommand(sql, con);
            int value = -1;
            try
            {
                if (con.State == ConnectionState.Closed)
                    Open();
                value = mycmd.ExecuteNonQuery();
                return value;
            }
            catch (SQLiteException e)
            {
                if (con.State == ConnectionState.Closed)
                {
                    Open();
                    value = mycmd.ExecuteNonQuery();
                }
                return value;
            }
            finally
            {
                Close();
            }

        }
        #endregion

        #region 返回瓦片DataSet
        /// <summary>
        /// 执行SQL语句，返回数据到DataSet中
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <returns>返回DataSet</returns>
        public DataSet GetDataSetByPath(string sql, string DataSourcePath)
        {
            lock (lockObj)
            {
                DataSet ds = new DataSet();
                //conStr.DataSource = DataSourcePath;//先获取一个测试的sqlite文件testSqliteDB.db
                //conStr.Password = _sqlitePWD;
                string conStr = string.Format("Data Source={0};Password={1}", DataSourcePath, _sqlitePWD);
                con = new SQLiteConnection(conStr);
                try
                {
                    this.Open();
                    SQLiteDataAdapter adapter = new SQLiteDataAdapter(sql, con);

                    adapter.Fill(ds);
                }
                catch (Exception ex)
                {
                    //throw ex;
                    if (con.State == ConnectionState.Closed)
                    {
                        Open();//打开数据连接
                        SQLiteDataAdapter adapter = new SQLiteDataAdapter(sql, con);
                        adapter.Fill(ds);
                        con.Close();
                    }
                }
                finally
                {
                    Close();//关闭数据库连接
                }
                return ds;
            }

        }

        public DataSet GetDataSet(string sql, string DataSourcePath, string tablename, int currentIndex, int pageSize)
        {

            DataSet ds = new DataSet();
            try
            {
                Monitor.Enter(lockObj);

                //conStr.DataSource = DataSourcePath;//先获取一个测试的sqlite文件testSqliteDB.db
                //conStr.Password = _sqlitePWD;
                string conStr = string.Format("Data Source={0};Password={1}", DataSourcePath, _sqlitePWD);
                con = new SQLiteConnection(conStr);
                this.Open();
                SQLiteDataAdapter adapter = new SQLiteDataAdapter(sql, con);
                adapter.Fill(ds, currentIndex, pageSize, tablename);
            }
            catch (Exception ex)
            {
                //throw ex;
                if (con.State == ConnectionState.Closed)
                {
                    Open();//打开数据连接
                    SQLiteDataAdapter adapter = new SQLiteDataAdapter(sql, con);
                    adapter.Fill(ds, currentIndex, pageSize, tablename);
                    con.Close();
                }
            }
            finally
            {
                Close();//关闭数据库连接
                Monitor.Exit(lockObj);
            }
            return ds;
        }
        #endregion 

        #region 在切片sqlite中插入已上传文件的记录
        public void CreateDBfile(string DataSourcePath)
        {

            System.Data.SQLite.SQLiteConnection.CreateFile(DataSourcePath);
            //conStr.DataSource = DataSourcePath;
            //conStr.Password = _sqlitePWD;
            string conStr = string.Format("Data Source={0};Password={1}", DataSourcePath, _sqlitePWD);

            try
            {
                Monitor.Enter(lockObj);
                con = new SQLiteConnection(conStr);
                this.Open();
                System.Data.SQLite.SQLiteCommand cmd = new System.Data.SQLite.SQLiteCommand();
                //去掉ext，添加时间
                //增加供应商Provider字段，说明数据来源终端  //JOKI 20131214
                const string CreatCorrectedTiles =
                    "create table correctedTiles([ID] integer primary key autoincrement,[DataSourceID] nchar,[Satellite] nchar,[Sensor] nchar,[Date] integer,[Level] tinyint,[Row] integer,[Col] integer,[type] nchar,[Time] timestamp,[Provider] nchar,[Availability] integer,[Cloud] integer)";
                string CreatProductTiles = "create table productTiles([ID] integer primary key autoincrement,[ProdType] nchar,[DataSourceID] nchar,[Date] integer,[Level] tinyint,[Row] integer,[Col] integer,[Time] timestamp,[Provider] nchar,[Availability] integer,[Cloud] integer)";
                string CreatIdxCorrected = "CREATE  INDEX Idx_Corrected On [correctedTiles] ([ID] ASC,[DataSourceID] ASC,[Satellite] ASC,[Sensor] ASC,[Date] DESC,[Level] ASC,[Row] ASC,[Col] ASC,[type] ASC,[Time] DESC,[Provider] ASC)";
                string CreatIdxProd = "CREATE  INDEX Idx_Prod On [productTiles] ([ID] ASC,[ProdType] ASC,[DataSourceID] ASC,[Date] DESC,[Level] ASC,[Col] ASC,[Row] ASC,[Time] DESC,[Provider] ASC)";

                cmd.Connection = con;
                cmd.CommandText = CreatCorrectedTiles;
                cmd.ExecuteNonQuery();
                cmd.CommandText = CreatProductTiles;
                cmd.ExecuteNonQuery();
                cmd.CommandText = CreatIdxCorrected;
                cmd.ExecuteNonQuery();
                cmd.CommandText = CreatIdxProd;
                cmd.ExecuteNonQuery();
            }
            catch(Exception ex)
            {
                throw new Exception("CreateDBfile", ex);
            }
            finally
            {
                this.Close();
                Monitor.Exit(lockObj);
            }

        }

        public void InSertData(string sql, string DataSourcePath)
        {
            try
            {
                Monitor.Enter(lockObj);
                string conStr = string.Format("Data Source={0};Password={1}", DataSourcePath, _sqlitePWD);
                con = new SQLiteConnection(conStr);
                if (!File.Exists(DataSourcePath))  //源路径DB文件不存在，新建db文件及表
                {
                    CreateDBfile(DataSourcePath);
                    //conStr.DataSource = DataSourcePath;
                    //conStr.Password = _sqlitePWD;
                    this.Open();
                    System.Data.SQLite.SQLiteCommand cmd = new System.Data.SQLite.SQLiteCommand();

                    cmd.Connection = con;
                    cmd.CommandText = sql;
                    cmd.ExecuteNonQuery();

                    this.Close();

                }
                else
                {
                    //conStr.DataSource = DataSourcePath;//先获取一个测试的sqlite文件testSqliteDB.db
                    //conStr.Password = _sqlitePWD;
                    this.Open();
                    System.Data.SQLite.SQLiteCommand cmd = new System.Data.SQLite.SQLiteCommand();
                    string selectCountSql = CountSql(sql);
                    cmd.Connection = con;
                    cmd.CommandText = selectCountSql;
                    System.Data.SQLite.SQLiteDataReader SqlDr = cmd.ExecuteReader();
                    int count = 0;

                    if (SqlDr.Read())
                    {
                        count = SqlDr.GetInt32(0);
                    }
                    SqlDr.Close();
                    cmd.Connection = con;
                    if (count > 0)  //如果原来的记录在表中已经存在，则删除，重新插入该记录，以更新入库时间,2013/2/27 zxw
                    {
                        string deleteSql = DeleteSql(sql);
                        cmd.CommandText = deleteSql;
                        cmd.ExecuteNonQuery();
                    }
                    cmd.CommandText = sql;
                    cmd.ExecuteNonQuery();

                    con.Close();
                }

            }

            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                this.Close();//关闭数据库连接
                Monitor.Exit(lockObj);
            }

        }

        public void InSertData(List<string> sqlstrs, string DataSourcePath)
        {
            DbTransaction trans = null;
            try
            {
                Monitor.Enter(lockObj);
                string conStr = string.Format("Data Source={0};Password={1}", DataSourcePath, _sqlitePWD);
                con = new SQLiteConnection(conStr);
  
                if (!File.Exists(DataSourcePath))  //源路径DB文件不存在，新建db文件及表
                {
                    CreateDBfile(DataSourcePath);
                    this.Open();

                    System.Data.SQLite.SQLiteCommand cmd = new System.Data.SQLite.SQLiteCommand();

                    cmd.Connection = con;

                    trans = con.BeginTransaction();
                    foreach (string sql in sqlstrs)
                    {
                        cmd.CommandText = sql;
                        cmd.ExecuteNonQuery();
                    }

                    updateTableGFF(cmd);

                    trans.Commit();
                }
                else
                {
                    this.Open();
                    System.Data.SQLite.SQLiteCommand cmd = new System.Data.SQLite.SQLiteCommand();
                    cmd.Connection = con;

                    trans = con.BeginTransaction();
                    foreach (string sql in sqlstrs)
                    {
                        //如果原来的记录在表中已经存在，则删除，重新插入该记录，以更新入库时间,2013/2/27 zxw

                        //try   //单句sql调试使用
                        //{

                        string deleteSql = getDeleteSqlStr(sql);
                        cmd.CommandText = deleteSql;
                        cmd.ExecuteNonQuery();
                        cmd.CommandText = sql;
                        cmd.ExecuteNonQuery();
                        //}
                        //catch (Exception ex)
                        //{ }
                    }

                    updateTableGFF(cmd);
                    trans.Commit();
                }

            }
            catch (Exception e)
            {
                trans.Rollback();
                throw;
            }
            finally
            {
                this.Close();
                Monitor.Exit(lockObj);
            }
        }

        public void UpdateDistinctTables(string DataSourcePath)
        {
            if (!File.Exists(DataSourcePath))  //
            {
                throw new Exception("the databasefile  is not exists");
            }
            string conStr = string.Format("Data Source={0};Password={1}", DataSourcePath, _sqlitePWD);
            con = new SQLiteConnection(conStr);
            this.Open();
            System.Data.SQLite.SQLiteCommand cmd = new System.Data.SQLite.SQLiteCommand();
            cmd.Connection = con;

            updateDistinctAttrbutes(cmd);

            this.Close();
        }

        /// <summary>
        /// 每次增删改correctedTiles，需要刷新Distinct相关表。
        /// </summary>
        /// <param name="cmd"></param>
        private void updateDistinctAttrbutes(SQLiteCommand cmd)
        {
            string DropDistinctSatellite = "DROP TABLE IF EXISTS `distinctSatellite`;";
            string CreatDistinctSatellite = "create table `distinctSatellite` as select distinct Satellite from correctedTiles";

            string DropDistinctSensor = "DROP TABLE IF EXISTS `distinctSensor`;";
            string CreatDistinctSensor = "create table `distinctSensor` as select distinct Sensor from correctedTiles";

            string DropDistinctCTtype = "DROP TABLE IF EXISTS `distinctCTtype`;";
            string CreatDistinctCTtype = "create table `distinctCTtype` as select distinct type from correctedTiles";

            string DropDistinctCTLevel = "DROP TABLE IF EXISTS `distinctCTLevel`;";
            string CreatDistinctCTLevel = "create table `distinctCTLevel` as select distinct Level from correctedTiles";

            string DropDistinctPTLevel = "DROP TABLE IF EXISTS `distinctPTLevel`;";
            string CreatDistinctPTLevel = "create table `distinctPTLevel` as select distinct Level from productTiles";

            string DropDistinctPTProdType = "DROP TABLE IF EXISTS `distinctPTProdType`;";
            string CreatDistinctPTProdType = "create table `distinctPTProdType` as select distinct ProdType from productTiles";

            try
            {
                Monitor.Enter(lockObj);
                cmd.CommandText = DropDistinctSatellite;
                cmd.ExecuteNonQuery();
                cmd.CommandText = CreatDistinctSatellite;
                cmd.ExecuteNonQuery();
                cmd.CommandText = DropDistinctSensor;
                cmd.ExecuteNonQuery();
                cmd.CommandText = CreatDistinctSensor;
                cmd.ExecuteNonQuery();
                cmd.CommandText = DropDistinctCTtype;
                cmd.ExecuteNonQuery();
                cmd.CommandText = CreatDistinctCTtype;
                cmd.ExecuteNonQuery();
                cmd.CommandText = DropDistinctCTLevel;
                cmd.ExecuteNonQuery();
                cmd.CommandText = CreatDistinctCTLevel;
                cmd.ExecuteNonQuery();
                cmd.CommandText = DropDistinctPTLevel;
                cmd.ExecuteNonQuery();
                cmd.CommandText = CreatDistinctPTLevel;
                cmd.ExecuteNonQuery();
                cmd.CommandText = DropDistinctPTProdType;
                cmd.ExecuteNonQuery();
                cmd.CommandText = CreatDistinctPTProdType;
                cmd.ExecuteNonQuery();
            }
            catch
            {
                throw;
            }
            finally
            {
                Monitor.Exit(lockObj);
            }
        }

        /// <summary>
        /// 插入记录前，已有相同记录则删除
        /// </summary>
        /// <param name="insertsql">瓦片索引插入Sql</param>
        /// <returns></returns>
        private string getDeleteSqlStr(string insertsql)
        {       //增加了Availability和Cloud字段，需要更新
            string[] selectCountStr;
            char[] splitArr = new char[] { ',', '(', ')' };
            selectCountStr = insertsql.Split(splitArr);
            string selectCountSql;
            string istsql = insertsql.Trim().ToLower();
            if (istsql.StartsWith("insert into correctedtiles"))
            {
                //"INSERT INTO correctedTiles(DataSourceID,Satellite,Sensor,Date,Level,Row,Col,type,Time,Provider,Availability,Cloud) values('{0}','{1}','{2}',{3},'{4}',{5},{6},'{7}','{8}','{9}',{10},{11})"
                selectCountSql = string.Format("delete from correctedTiles" + " where" + " " + selectCountStr[1] + " = " + selectCountStr[14] + " and " + selectCountStr[2] + " = " + selectCountStr[15] + " and " + selectCountStr[3] + " = " + selectCountStr[16] + " and " + selectCountStr[4] + " = " + selectCountStr[17] + " and " + selectCountStr[5] + " = " + selectCountStr[18] + " and " + selectCountStr[6] + " = " + selectCountStr[19] + " and " + selectCountStr[7] + " = " + selectCountStr[20] + " and " + selectCountStr[8] + " = " + selectCountStr[21] + " and " + selectCountStr[10] + " = " + selectCountStr[23]);
                return selectCountSql;
            }
            else if (istsql.StartsWith("insert into producttiles"))
            {
                //"INSERT INTO productTiles(ProdType,DataSourceID,Date,Level,Row,Col,Time,Provider,Availability,Cloud) values('{0}','{1}',{2},'{3}',{4},{5},'{6}','{7}',{8},{9})"
                selectCountSql = string.Format("delete from productTiles" + " where" + " " + selectCountStr[1] + " = " + selectCountStr[12] + " and " + selectCountStr[2] + " = " + selectCountStr[13] + " and " + selectCountStr[3] + " = " + selectCountStr[14] + " and " + selectCountStr[4] + " = " + selectCountStr[14] + " and " + selectCountStr[5] + " = " + selectCountStr[16] + " and " + selectCountStr[6] + " = " + selectCountStr[17] + " and " + selectCountStr[8] + " = " + selectCountStr[19]);
                return selectCountSql;
            }
            else if (istsql.StartsWith("insert into classifysampletiles"))
            {
                //"INSERT INTO classifySampleTiles(ID,CategoryCode,ShootTime,DataSource,Level,Row,Col) values({0},'{1}',{2},'{3}','{4}',{5},{6})"
                selectCountSql = string.Format("delete from classifySampleTiles" + " where" + " " + selectCountStr[1] + " = " + selectCountStr[9] + " and " + selectCountStr[2] + " = " + selectCountStr[10] + " and " + selectCountStr[3] + " = " + selectCountStr[11] + " and " + selectCountStr[4] + " = " + selectCountStr[12] + " and " + selectCountStr[5] + " = " + selectCountStr[13] + " and " + selectCountStr[6] + " = " + selectCountStr[14] + " and " + selectCountStr[7] + " = " + selectCountStr[15]);
                return selectCountSql;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 每次增删改correctedTiles，需要刷新GFF表，找1.2.3.4.png都存在的数据，目前采用全表刷新，后期瓦片数据达到一定量的时候，效率有待提升，后期建议修改为触发器模式。
        /// </summary>
        /// <param name="cmd"></param>
        private void updateTableGFF(SQLiteCommand cmd)
        {
            string DropGFF = "DROP TABLE IF EXISTS `gff`;";
            string CreatGFF = "create table `gff` as select * from (select *,group_concat(type) as types from MAIN.[correctedTiles] where type in ('1','2','3','4','Preview') group by DataSourceID,Satellite,Sensor,Date,Level,Row,Col order by type) where types = '1,2,3,4,Preview'";
            string CreatIdxGff = "CREATE  INDEX Idx_gff On [gff] ([ID] ASC,[DataSourceID] ASC,[Satellite] ASC,[Sensor] ASC,[Date] DESC,[Level] ASC,[Row] ASC,[Col] ASC,[type] ASC,[Time] DESC,[Provider] ASC,[Availability] DESC,[Cloud] ASC)";
            try
            {
                Monitor.Enter(lockObj);
                cmd.CommandText = DropGFF;
                cmd.ExecuteNonQuery();
                cmd.CommandText = CreatGFF;
                cmd.ExecuteNonQuery();
                cmd.CommandText = CreatIdxGff;
                cmd.ExecuteNonQuery();
            }
            catch
            {
                throw;
            }
            finally
            {
                Monitor.Exit(lockObj);
            }

        }

        public void UpdateTableGFF(string DataSourcePath)
        {
            string conStr = string.Format("Data Source={0};Password={1}", DataSourcePath, _sqlitePWD);
            if (!File.Exists(DataSourcePath))  //
            {
                throw new Exception("the databasefile  is not exists");
            }
            try
            {
                Monitor.Enter(lockObj);
                //conStr.DataSource = DataSourcePath;
                //conStr.Password = _sqlitePWD;
                con = new SQLiteConnection(conStr);
                this.Open();
                System.Data.SQLite.SQLiteCommand cmd = new System.Data.SQLite.SQLiteCommand();
                cmd.Connection = con;

                updateTableGFF(cmd);
            }
            catch(Exception ex)
            {
                throw new Exception("UpdateTableGFF Error", ex);
            }

            finally
            {
                this.Close();
                Monitor.Exit(lockObj);
            }

        }
        #endregion

        #region 删除sqlite数据库中相应已删除文件记录
        public void DeleteData(string sql, string DataSourcePath)
        {
            try
            {

                Monitor.Enter(lockObj);
                string conStr = string.Format("Data Source={0};Password={1}", DataSourcePath, _sqlitePWD);
                con = new SQLiteConnection(conStr);
                if (!File.Exists(DataSourcePath))  //
                {
                    throw new Exception("the databasefile  is not exists");
                }
                else
                {
                    //conStr.DataSource = DataSourcePath;//先获取一个测试的sqlite文件testSqliteDB.db
                    //conStr.Password = _sqlitePWD;
                    this.Open();
                    System.Data.SQLite.SQLiteCommand cmd = new System.Data.SQLite.SQLiteCommand();
                    cmd.Connection = con;
                    cmd.CommandText = sql;
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                this.Close();//关闭数据库连接
                Monitor.Exit(lockObj);
            }

        }

        public void DeleteData(List<string> sqlstrs, string DataSourcePath)
        {
            DbTransaction trans = null;
            try
            {

                Monitor.Enter(lockObj);
                string conStr = string.Format("Data Source={0};Password={1}", DataSourcePath, _sqlitePWD);
                con = new SQLiteConnection(conStr);
                if (!File.Exists(DataSourcePath))  //
                {
                    throw new Exception("the databasefile  is not exists");
                }
                else
                {
                    //conStr.DataSource = DataSourcePath;//先获取一个测试的sqlite文件testSqliteDB.db
                    //conStr.Password = _sqlitePWD;

                    this.Open();
                    System.Data.SQLite.SQLiteCommand cmd = new System.Data.SQLite.SQLiteCommand();
                    cmd.Connection = con;

                    trans = con.BeginTransaction();

                    foreach (string sql in sqlstrs)
                    {
                        //如果原来的记录在表中已经存在，则删除，重新插入该记录，以更新入库时间,2013/2/27 zxw

                        //单句sql调试使用


                        cmd.CommandText = sql;
                        cmd.ExecuteNonQuery();

                    }
                    updateTableGFF(cmd);
                    trans.Commit();


                }
            }
            catch (Exception ex)
            {
                trans.Rollback();
                throw;
            }
            finally
            {
                this.Close();//关闭数据库连接
                Monitor.Exit(lockObj);

            }

            
        }
        #endregion

        #region 返回查找已插入的count查询语句
        //修改时间：2013/2/27 zxw 添加Time字段以后，将字符数组selectCountStr中元素的index调整了
        public string CountSql(string insertsql)
        {
            string[] selectCountStr;
            char[] splitArr = new char[] { ',', '(', ')' };
            selectCountStr = insertsql.Split(splitArr);
            string selectCountSql;
            if (selectCountStr.Length == 23)
            {
                selectCountSql = string.Format("select count (*) from correctedTiles" + " where" + " " + selectCountStr[1] + " = " + selectCountStr[12] + " and " + selectCountStr[2] + " = " + selectCountStr[13] + " and " + selectCountStr[3] + " = " + selectCountStr[14] + " and " + selectCountStr[4] + " = " + selectCountStr[15] + " and " + selectCountStr[5] + " = " + selectCountStr[16] + " and " + selectCountStr[6] + " = " + selectCountStr[17] + " and " + selectCountStr[7] + " = " + selectCountStr[18] + " and " + selectCountStr[8] + " = " + selectCountStr[19] + " and " + selectCountStr[10] + " = " + selectCountStr[21]);
                return selectCountSql;
            }
            else if (selectCountStr.Length == 19)
            {
                selectCountSql = string.Format("select count (*) from productTiles" + " where" + " " + selectCountStr[1] + " = " + selectCountStr[10] + " and " + selectCountStr[2] + " = " + selectCountStr[11] + " and " + selectCountStr[3] + " = " + selectCountStr[12] + " and " + selectCountStr[4] + " = " + selectCountStr[13] + " and " + selectCountStr[5] + " = " + selectCountStr[14] + " and " + selectCountStr[6] + " = " + selectCountStr[15] + " and " + selectCountStr[7] + " = " + selectCountStr[16]);

                return selectCountSql;
            }
            else
            {
                return null;
            }
        }
        #endregion

        public int ExecuteTileQuery(string sql, string DataSourcePath)
        {
            int effectrows = 0;
            try
            {
                //bool needupdategff=false;
                Monitor.Enter(lockObj);
                string conStr = string.Format("Data Source={0};Password={1}", DataSourcePath, _sqlitePWD);
                con = new SQLiteConnection(conStr);
                if (!File.Exists(DataSourcePath))  //
                {
                    throw new Exception("the databasefile  is not exists");
                }
                else
                {

                    //conStr.DataSource = DataSourcePath;//先获取一个测试的sqlite文件testSqliteDB.db
                    //conStr.Password = _sqlitePWD;

                    this.Open();
                    System.Data.SQLite.SQLiteCommand cmd = new System.Data.SQLite.SQLiteCommand();
                    cmd.Connection = con;
                    cmd.CommandText = sql;
                    effectrows = cmd.ExecuteNonQuery();

                    //if (sql.ToLower().Contains("table")&&sql.ToLower().Contains("correctedtiles"))
                    //{
                    //    //如果修改了表correctedtiles，则更新GFF表
                    //    needupdategff = true;
                    //}

                    //if (needupdategff)
                    //{
                    //    updateTableGFF(cmd);
                    //}

                    con.Close();
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                this.Close();//关闭数据库连接
                Monitor.Exit(lockObj);
            }
                return effectrows;
            
        }
        public string DeleteSql(string insertsql)
        {
            string[] selectCountStr;
            char[] splitArr = new char[] { ',', '(', ')' };
            selectCountStr = insertsql.Split(splitArr);
            string selectCountSql;
            if (selectCountStr.Length == 23)
            {
                selectCountSql = string.Format("delete from correctedTiles" + " where" + " " + selectCountStr[1] + " = " + selectCountStr[12] + " and " + selectCountStr[2] + " = " + selectCountStr[13] + " and " + selectCountStr[3] + " = " + selectCountStr[14] + " and " + selectCountStr[4] + " = " + selectCountStr[15] + " and " + selectCountStr[5] + " = " + selectCountStr[16] + " and " + selectCountStr[6] + " = " + selectCountStr[17] + " and " + selectCountStr[7] + " = " + selectCountStr[18] + " and " + selectCountStr[8] + " = " + selectCountStr[19] + " and " + selectCountStr[10] + " = " + selectCountStr[21]);
                return selectCountSql;
            }
            else if (selectCountStr.Length == 19)
            {
                selectCountSql = string.Format("delete from productTiles" + " where" + " " + selectCountStr[1] + " = " + selectCountStr[11] + " and " + selectCountStr[2] + " = " + selectCountStr[12] + " and " + selectCountStr[3] + " = " + selectCountStr[13] + " and " + selectCountStr[4] + " = " + selectCountStr[14] + " and " + selectCountStr[5] + " = " + selectCountStr[15] + " and " + selectCountStr[6] + " = " + selectCountStr[16] + " and " + selectCountStr[9] + " = " + selectCountStr[17]);
                return selectCountSql;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 根据插入属性和相应值构成SQL，带参数执行
        /// </summary>
        /// <param name="sqlBase"></param>
        /// <param name="attrNames"></param>
        /// <param name="attrValues"></param>
        public void InsertDataByParamSql(string tableName, List<string> attrNames, List<object> attrValues)
        {
            try
            {
                Monitor.Enter(lockObj);
                StringBuilder strSql = new StringBuilder();
            SQLiteParameter[] parameters = new SQLiteParameter[attrNames.Count];
            SQLiteParameter sqlParam;
            strSql.Append(string.Format("insert into '{0}'(", tableName));
            foreach (string elem in attrNames)
            {
                strSql.Append(elem + ",");
            }
            strSql.Remove(strSql.Length - 1, 1);
            strSql.Append(" )values (");
            for (int i = 0; i < attrNames.Count; i++)
            {
                strSql.Append("@" + attrNames[i] + ",");
                sqlParam = new SQLiteParameter("@" + attrNames[i]);
                //SQL语句参数赋值
                sqlParam.Value = attrValues[i];
                parameters[i] = sqlParam;

            }
            strSql.Remove(strSql.Length - 1, 1);        //移除最后一个逗号
            strSql.Append(")");

                ExecuteSql(strSql.ToString(), parameters);
            }
            catch (Exception ex)
            {
                throw new Exception("SQLite InsertDataByParamSql:", ex);
            }
            finally
            {
                Monitor.Exit(lockObj);
            }
        }

        /// <summary>
        /// 得到标准日期的时间格式 @jh 
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        private string GetStandardDateFormateSQL(string sql)
        {
            return "a";
        }
    }
}
