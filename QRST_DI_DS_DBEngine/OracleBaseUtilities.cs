using System;
using System.Data;
using System.Data.Common;
using System.Data.OracleClient;
using QRST_DI_Resources;
 
namespace QRST_DI_DS_DBEngine
{
    public class OracleBaseUtilities 
    {
        protected OracleConnection con=new OracleConnection ();//连接对象
        //

        public OracleBaseUtilities(string strServiceAliasName, string strUserName, string strPwd)
        {
            string strConn = String.Format("Data Source={0};User ID={1};Password={2};Unicode=True", strServiceAliasName, strUserName, strPwd);
            con = new OracleConnection(strConn);
        }
        public OracleBaseUtilities(string constr)
        {
            con = new OracleConnection(constr);
            con.Open();
        }
        public OracleBaseUtilities()
        {           
           string  OracleCon = Constant.ConnectionStringOracle;
           con = new OracleConnection(OracleCon);
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
            if (con.State == ConnectionState.Closed)
            {
                try
                {
                    //打开数据库连接
                    con.Open();
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
        }
        #endregion
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
        #region 执行查询语句，返回OracleDataReader ( 注意：调用该方法后，一定要对OracleDataReader进行Close )
        /// <summary>
        /// 执行查询语句，返回OracleDataReader ( 注意：调用该方法后，一定要对OracleDataReader进行Close )
        /// </summary>
        /// <param name="sql">查询语句</param>
        /// <returns>手动转 OracleDataReader 对象</returns>   
        public DbDataReader ExecuteReader(string sql)
        {
            OracleDataReader myReader;
            Open();
            OracleCommand cmd = new OracleCommand(sql, con);
            myReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
            return myReader;
        }
        #endregion
        #region 执行带参数的SQL语句
        /// <summary>
        /// 执行SQL语句，返回影响的记录数
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <param name="cmdParms">OracleParameter[] 对象</param>
        /// <returns>影响的记录数</returns>   
        public int ExecuteSql(string sql, params DbParameter[] cmdParms)
        {
            OracleCommand cmd = new OracleCommand();
            {
                try
                {
                    PrepareCommand(cmd, con, null, CommandType.Text, sql, cmdParms as OracleParameter[]);
                    int rows = cmd.ExecuteNonQuery();
                    cmd.Parameters.Clear();
                    return rows;
                }
                catch (System.Data.OracleClient.OracleException e)
                {
                    throw e;
                }
            }
        }

        private void PrepareCommand(OracleCommand cmd, OracleConnection conn, OracleTransaction trans, CommandType cmdType, string cmdText, OracleParameter[] cmdParms)
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
                foreach (OracleParameter parm in cmdParms)
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
            OracleCommand cmd = new OracleCommand(sql, con);
            int resultrow = 0;
            try
            {
                Open();
                resultrow = cmd.ExecuteNonQuery();
                Close();
                return resultrow;
            }
            catch (System.Data.OracleClient.OracleException e)
            {
                if (con.State == ConnectionState.Closed)
                {
                    Open();
                    resultrow = cmd.ExecuteNonQuery();
                    Close();
                    
                }
                //throw e;
                return resultrow;
            }
            
        }
        #endregion
        #region 执行SQL语句，返回数据到DataSet中
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
                Open();//打开数据连接
                OracleDataAdapter adapter = new OracleDataAdapter(sql, con);
                 int dd=adapter.Fill(ds);
            }           
            catch(Exception ex)
            {
                //throw ex;
                if (con.State == ConnectionState.Closed)
                {
                    Open();//打开数据连接
                    OracleDataAdapter adapter = new OracleDataAdapter(sql, con);
                    int dd = adapter.Fill(ds);

                }
            }
            finally
            {                  
                Close();//关闭数据库连接
            }
            return ds;
        }
        #endregion
        #region 执行SQL语句，返回数据到自定义DataSet中
        /// <summary>
        /// 执行SQL语句，返回数据到DataSet中
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <param name="DataSetName">自定义返回的DataSet表名</param>
        /// <returns>返回DataSet</returns>
        public DataSet GetDataSet(string sql, string DataSetName)
        {
            DataSet ds = new DataSet();
            Open();//打开数据连接
            OracleDataAdapter adapter = new OracleDataAdapter(sql, con);
            adapter.Fill(ds, DataSetName);
            Close();//关闭数据库连接
            return ds;
        }
        #endregion
        #region 执行Sql语句,返回带分页功能的自定义dataset
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
            Open();//打开数据连接
            OracleDataAdapter adapter = new OracleDataAdapter(sql, con);
            adapter.Fill(ds, PageSize * (CurrPageIndex - 1), PageSize, DataSetName);
            Close();//关闭数据库连接
            return ds;
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
            int recordCount = 0;
            Open();//打开数据连接
            OracleCommand command = new OracleCommand(sql, con);
            OracleDataReader dataReader = command.ExecuteReader();
            while (dataReader.Read())
            {
                recordCount++;
            }
            dataReader.Close();
            Close();//关闭数据库连接
            return recordCount;
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


        /// <summary>
        /// 添加表到数据库中
        /// </summary>
        /// <param name="ds"></param>
        /// <returns></returns>
        public void AddDataTableToDB(string tableName, DataTable dt)
        {
            //string strsql = "select * from " + tableName;
            ////con.Open();
            //using (MySqlDataAdapter myDataAdapter = new MySqlDataAdapter(strsql, con))
            //{
            //    MySqlCommandBuilder comm = new MySqlCommandBuilder(myDataAdapter);
            //    try
            //    {
            //        myDataAdapter.Update(dt);
            //    }
            //    catch (Exception e)
            //    {
            //        throw e;
            //    }
            //    finally
            //    {
            //        con.Close();
            //    }
            //}
        }
    }
}
