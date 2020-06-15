using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text;
using MySql.Data.MySqlClient;
using QRST_DI_Resources;
using QRST_DI_SS_DBInterfaces.IDBEngine;
 
namespace QRST_DI_DS_DBEngine
{
    [Serializable]
    public class MySqlBaseUtilities : MarshalByRefObject, IDbBaseUtilities
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="subdbname">BSDB MIDB EVDB...</param>
        /// <returns></returns>
        public IDbBaseUtilities GetSubDBUtil(string subdbname)
        {
            MySqlBaseUtilities MIDB = new MySqlBaseUtilities();
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
                    return new MySqlBaseUtilities(connstr);
                }
            }
            return null;
        }


        public MySqlConnection con = new MySqlConnection(); //连接对象
        //
        public string connString = "";

        public MySqlBaseUtilities(string constr)
        {
            con = new MySqlConnection(constr);
            connString = constr;
            //变更 去掉打开连接
            // con.Open();
        }

        public MySqlBaseUtilities()
        {

            string MySqlCon = Constant.ConnectionStringMySql;
            if (!string.IsNullOrEmpty(MySqlCon))
            {
                connString = MySqlCon;
                con = new MySqlConnection(MySqlCon);
            }
            InitializeLifetimeService();
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
            MySqlBaseUtilities utilities = new MySqlBaseUtilities(conStr);
            return utilities;
        }

        public string GetDbConnection()
        {
            return connString;
        }

        /// <summary>
        /// 获取当前时间
        /// </summary>
        /// <returns></returns>
        public DateTime GetDBNowTime()
        {
            string sql = "select now()";
            string dt = GetDataSet(sql).Tables[0].Rows[0][0].ToString();
            return Convert.ToDateTime(dt);      //2015-06-24 11:53:39
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
                    throw e;
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
            if (con.State == ConnectionState.Closed || !con.Ping())
            {
                try
                {
                    //打开数据库连接
                    con.Open();
                }
                catch (Exception e)
                {
                    //con.Close();
                    throw e;
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
            string sql = string.Format("show columns from {0}", tblName);
            DataSet ds = null;
            try
            {
                ds = GetDataSet(sql);
                //移除“null”,"key","default","Extra"列
                ds.Tables[0].Columns.RemoveAt(5);
                ds.Tables[0].Columns.RemoveAt(4);
                ds.Tables[0].Columns.RemoveAt(3);
                ds.Tables[0].Columns.RemoveAt(2);
            }
            catch (Exception e)
            {
                throw new Exception("获取表结构错误", e);
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

        #region 执行查询语句，返回MySqlDataReader ( 注意：调用该方法后，一定要对MySqlDataReader进行Close )

        /// <summary>
        /// 执行查询语句，返回MySqlDataReader ( 注意：调用该方法后，一定要对MySqlDataReader进行Close )
        /// </summary>
        /// <param name="sql">查询语句</param>
        /// <returns>手动转 MySqlDataReader对象</returns>   
        public DbDataReader ExecuteReader(string sql)
        {
            try
            {
                //添加了连接的开启与关闭操作 zxw 2013/04/21
                MySqlDataReader myReader;
                Open();
                using (MySqlCommand cmd = new MySqlCommand(sql, con))
                {
                    myReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                    return myReader;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                Close();
            }
        }

        #endregion

        #region 执行带参数的SQL语句

        /// <summary>
        /// 执行SQL语句，返回影响的记录数
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <param name="cmdParms">MySqlParameter[] 对象</param>
        /// <returns>影响的记录数</returns>   
        public int ExecuteSql(string sql, params DbParameter[] cmdParms)
        {
            //添加了连接的开启与关闭操作 zxw 2013/04/21
            MySqlCommand cmd = new MySqlCommand();
            {
                try
                {
                    cmd.Connection = con;
                    PrepareCommand(cmd, con, null, CommandType.Text, sql, cmdParms as MySqlParameter[]);
                    int rows = cmd.ExecuteNonQuery();
                    cmd.Parameters.Clear();
                    return rows;
                }
                catch (MySqlException e)
                {
                    throw e;
                }
                finally
                {
                    Close();
                }
            }
        }


        private void PrepareCommand(MySqlCommand cmd, MySqlConnection conn, MySqlTransaction trans, CommandType cmdType,
            string cmdText, MySqlParameter[] cmdParms)
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
                foreach (MySqlParameter parm in cmdParms)
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
            lock (this)
            {
                MySqlCommand cmd = new MySqlCommand(sql, con);
                int resultrow = 0;
                try
                {
                    Open();
                    resultrow = cmd.ExecuteNonQuery();
                    Close();
                    return resultrow;
                }
                catch (Exception e)
                {
                    if (con.State == ConnectionState.Closed)
                    {
                        Open();
                        resultrow = cmd.ExecuteNonQuery();
                        Close();

                    }
                    throw e;
                    return resultrow;
                }
            }
        }

        /// <summary>
        /// 执行NonQuery SQL语句
        /// </summary>
        /// <param name="sql">SQL语句</param>     
        public int ExecuteScript(string sql)
        {
            lock (this)
            {
                MySqlCommand cmd = new MySqlCommand(sql, con);
                int resultrow = 0;
                try
                {
                    Open();
                    resultrow = cmd.ExecuteNonQuery();
                    Close();
                    return resultrow;
                }
                catch (MySqlException e)
                {
                    throw e;

                }
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

            ////joki 161220 测试效率
            //Console.WriteLine("######SQL测试效率");
            //Console.WriteLine(sql);
            //DateTime startdb = DateTime.Now;



            lock (this)
            {
                DataSet ds = new DataSet();
                int dd = 0;
                try
                {
                    Open(); //打开数据连接
                    MySqlDataAdapter adapter = new MySqlDataAdapter(sql, con);
                    //增加数据库连接时间,防止查询超时断开连接
                    adapter.SelectCommand.CommandTimeout = 999;
                    dd = adapter.Fill(ds);
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
                        throw ex;
                    }
                }
                finally
                {
                    Close(); //关闭数据库连接
                    _outTimeEx = 0;
                }



                ////joki 161220 测试效率
                //Console.WriteLine("######共计"+dd+"行，耗时:" + (DateTime.Now - startdb).TotalMilliseconds.ToString() + "ms");


                return ds;
            }
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

            lock (this)
            {
                DataSet ds = new DataSet();
                try
                {
                    Open(); //打开数据连接
                    MySqlDataAdapter adapter = new MySqlDataAdapter(sql, con);
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
                        throw ex;
                    }
                    //if (con.State == ConnectionState.Closed)
                    //{
                    //    Open();//打开数据连接
                    //    MySqlDataAdapter adapter = new MySqlDataAdapter(sql, con);
                    //    int dd = adapter.Fill(ds);

                    //}
                }
                finally
                {
                    Close(); //关闭数据库连接
                    _outTimeEx2 = 0;
                }
                return ds;
            }
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

            lock (this)
            {
                DataSet ds = new DataSet();
                try
                {
                    Open(); //打开数据连接
                    MySqlDataAdapter adapter = new MySqlDataAdapter(sql, con);
                    adapter.Fill(ds, PageSize*(CurrPageIndex - 1), PageSize, DataSetName);
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
                        throw ex;
                    }
                    //if (con.State == ConnectionState.Closed)
                    //{
                    //    Open();//打开数据连接
                    //    MySqlDataAdapter adapter = new MySqlDataAdapter(sql, con);
                    //    int dd = adapter.Fill(ds);

                    //}
                }
                finally
                {
                    Close(); //关闭数据库连接
                    _outTimeEx3 = 0;
                }
                return ds;
            }
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
        public DataSet GetList(string TableName, string returnFields, string strWhere, out int size, int startIndex = 0,
            int num = -1)
        {

            lock (this)
            {
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
                    throw ex;
                }
                return ds;
            }
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
        public DataSet GetList(string TableName, string returnFields, string strWhere, string orderByString,
            out int size, int startIndex = 0, int num = -1)
        {

            lock (this)
            {
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
                    throw ex;
                }
                return ds;
            }
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
        public DataSet GetList2(string TableName, string Fields_TableString, string strWhere, out int size,
            int startIndex = 0, int num = -1)
        {

            lock (this)
            {
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
                    throw ex;
                }
                return ds;
            }
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

            lock (this)
            {
                int recordCount = 0;
                try
                {
                    Open(); //打开数据连接
                    MySqlCommand command = new MySqlCommand(sql, con);
                    using (MySqlDataReader dataReader = command.ExecuteReader())
                    {
                        while (dataReader.Read())
                        {
                            recordCount++;
                        }
                    }
                }
                catch (Exception e)
                {
                    throw e;
                }
                finally
                {
                    Close();
                }
                return recordCount;
                //MySqlDataReader dataReader=
                //try
                //{
                //    MySqlDataReader dataReader = command.ExecuteReader();
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
            }
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
            PageCount = (RecordCount%pageSize) > 0 ? (RecordCount/pageSize) + 1 : RecordCount/pageSize;
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
        public int GetPageCount(string Field, string tableName, string condition, ref int RecordCount, int pageSize,
            bool flag)
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
        public string JoinPageSQL(string KeyField, string FieldStr, string TableName, string Where, string Order,
            int CurrentPage, int PageSize)
        {
            string sql = null;
            if (CurrentPage == 1)
            {
                sql = "select  " + CurrentPage*PageSize + " " + FieldStr + " from " + TableName + " " + Where + " " +
                      Order + " ";
            }
            else
            {
                sql = "select * from (";
                sql += "select  " + CurrentPage*PageSize + " " + FieldStr + " from " + TableName + " " + Where + " " +
                       Order + ") a ";
                sql += "where " + KeyField + " not in (";
                sql += "select  " + (CurrentPage - 1)*PageSize + " " + KeyField + " from " + TableName + " " + Where +
                       " " + Order + ")";
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
        public string JoinPageSQL(string Field, string TableName, string Where, string Order, int CurrentPage,
            int PageSize)
        {
            string sql = null;
            if (CurrentPage == 1)
            {
                sql = "select rownum " + CurrentPage*PageSize + " " + Field + " from " + TableName + " " + Where + " " +
                      Order + " group by " + Field;
            }
            else
            {
                sql = "select * from (";
                sql += "select rownum " + CurrentPage*PageSize + " " + Field + " from " + TableName + " " + Where + " " +
                       Order + " group by " + Field + " ) a ";
                sql += "where " + Field + " not in (";
                sql += "select rownum " + (CurrentPage - 1)*PageSize + " " + Field + " from " + TableName + " " + Where +
                       " " + Order + " group by " + Field + ")";
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


        //WYN 2012/04/17

        /// <summary>
        /// 添加数据集到数据库中
        /// </summary>
        /// <param name="ds"></param>
        /// <returns></returns>
        public void AddDatasetToDB(string tableName, DataSet ds)
        {
            string strsql = "select * from " + tableName;
            con.Open();
            using (MySqlDataAdapter myDataAdapter = new MySqlDataAdapter(strsql, con))
            {
                MySqlCommandBuilder comm = new MySqlCommandBuilder(myDataAdapter);
                try
                {
                    DataTable dt = ds.Tables[0];
                    foreach (DataRow dr in dt.Rows)
                    {
                        dr.SetAdded();
                    }
                    myDataAdapter.Update(dt);
                }
                catch (Exception e)
                {
                    throw e;
                }
                finally
                {
                    con.Close();
                }
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
            Open();
            using (MySqlDataAdapter myDataAdapter = new MySqlDataAdapter(strsql, con))
            {
                MySqlCommandBuilder comm = new MySqlCommandBuilder(myDataAdapter);
                try
                {
                    myDataAdapter.Update(dt);
                }
                catch (Exception e)
                {
                    throw e;
                }
                finally
                {
                    Close();
                }
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

                    throw ex;
                }
            }
        }

        /// <summary>
        /// 执行多条SQL语句，实现数据库事务。
        /// </summary>
        /// <param name="SQLStringList">多条SQL语句</param>		
        public int ExecuteSqlTran(List<String> SQLStringList, out string errorMsg)
        {
            if (con.State != ConnectionState.Open)
            {
                con.Open();
            }

            using (MySqlCommand cmd = new MySqlCommand {Connection = con})
            {
                MySqlTransaction tx = con.BeginTransaction();
                cmd.Transaction = tx;
                try
                {
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
                catch (Exception ex)
                {
                    tx.Rollback();
                    errorMsg = ex.ToString();
                    return -1;
                }
                finally
                {
                    Close();
                }

            }
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

            string sqlstring =
                string.Format("select VIEW_FIELD_NAME from table_view where TABLE_NAME = '{0}' and MEMO = 'keyWord'",
                    TableName);
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

            MySqlCommand cmd = new MySqlCommand(sql, con);
            try
            {
                Open();
                using (MySqlDataReader reader1 = cmd.ExecuteReader())
                {
                    while (reader1.Read())
                    {
                        tablesname.Add(reader1[0].ToString());
                    }
                }


            }
            catch (MySqlException e)
            {
                if (con.State == ConnectionState.Closed)
                {
                    Open();
                    using (MySqlDataReader reader1 = cmd.ExecuteReader())
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
            MySqlCommand cmd = new MySqlCommand(sql, con);
            try
            {

                Open();
                returnValue = cmd.ExecuteScalar().ToString();
                Close();
                return returnValue;
            }
            catch (MySqlException e)
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
        }

        #endregion

        #region//written by Jiang Bin, 检查备份的订单提交状态

        public int checkingstatus(string sql)
        {

            MySqlCommand mycmd = new MySqlCommand(sql, con);
            int value = -1;
            try
            {
                if (con.State == ConnectionState.Closed)
                    Open();
                value = mycmd.ExecuteNonQuery();
                return value;
            }
            catch (MySqlException e)
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

        /// <summary>
        /// 更新表
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="dt"></param>
        public void UpdateTable(string sql, DataTable dt)
        {
           Open();
           MySqlDataAdapter dataAdapter = new MySqlDataAdapter(sql, con);
           MySqlCommandBuilder builder = new MySqlCommandBuilder(dataAdapter);
           dataAdapter.Update(dt);


            Close();
        }

        public void UpdateTable(string sql)
        {
            Open();
            MySqlCommand mc = new MySqlCommand(sql, con);
            mc.ExecuteNonQuery();
            Close();
        }

        #region 瓦片库操作

        public DataSet GetDataSetByPath(string sql, string DataSourcePath)
        {
            SQLiteBaseUtilities sqLiteBase = new SQLiteBaseUtilities();
            return sqLiteBase.GetDataSetByPath(sql, DataSourcePath);
        }

        public DataSet GetDataSet(string sql, string DataSourcePath, string tablename, int currentIndex, int pageSize)
        {
            SQLiteBaseUtilities sqLiteBase = new SQLiteBaseUtilities();
            return sqLiteBase.GetDataSet(sql, DataSourcePath, tablename, currentIndex, pageSize);
        }

        public void CreateDBfile(string DataSourcePath)
        {
            SQLiteBaseUtilities sqLiteBase = new SQLiteBaseUtilities();
            sqLiteBase.CreateDBfile(DataSourcePath);
        }

        public void InSertData(string sql, string DataSourcePath)
        {
            SQLiteBaseUtilities sqLiteBase = new SQLiteBaseUtilities();
            sqLiteBase.InSertData(sql,DataSourcePath);
        }

        public void InSertData(List<string> sqlstrs, string DataSourcePath)
        {
            SQLiteBaseUtilities sqLiteBase = new SQLiteBaseUtilities();
            sqLiteBase.InSertData(sqlstrs, DataSourcePath);
        }

        public void UpdateTableGFF(string DataSourcePath)
        {
            SQLiteBaseUtilities sqLiteBase = new SQLiteBaseUtilities();
            sqLiteBase.UpdateTableGFF(DataSourcePath);
        }

        public int ExecuteTileQuery(string sql, string DataSourcePath)
        {
            SQLiteBaseUtilities sqLiteBase = new SQLiteBaseUtilities();
           return  sqLiteBase.ExecuteTileQuery(sql, DataSourcePath);
        }

        public void DeleteData(string sql, string DataSourcePath)
        {
            SQLiteBaseUtilities sqLiteBase = new SQLiteBaseUtilities();
            sqLiteBase.DeleteData(sql, DataSourcePath);
        }

        public void DeleteData(List<string> sqlstrs, string DataSourcePath)
        {
            SQLiteBaseUtilities sqLiteBase = new SQLiteBaseUtilities();
            sqLiteBase.DeleteData(sqlstrs, DataSourcePath);
        }

        public void UpdateDistinctTables(string DataSourcePath)
        {
            SQLiteBaseUtilities sqLiteBase = new SQLiteBaseUtilities();
            sqLiteBase.UpdateDistinctTables( DataSourcePath);
        }
            #endregion
        }
}
