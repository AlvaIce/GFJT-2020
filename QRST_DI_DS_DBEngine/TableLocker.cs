using System;
using System.Data;
using System.Threading;
using QRST_DI_Resources;
using QRST_DI_SS_DBInterfaces.IDBEngine;
 
namespace QRST_DI_DS_DBEngine
{
    public class TableLocker
    {
        IDbBaseUtilities _dbu;
        string _ip;
        public TableLocker()
        {
            switch (Constant.QrstDbEngine)
            {
                case EnumDbEngine.SQLITE:
                    _dbu = new SQLiteBaseUtilities();
                    break;
                case EnumDbEngine.MYSQL:
                    _dbu = new MySqlBaseUtilities();
                    break;
                case EnumDbEngine.ClOUDDB:
                    break;
            }

            _ip = Constant.UsingIPAddress;
        }
        public TableLocker( IDbBaseUtilities dbu)
        {
            _dbu = dbu;
//            _dbu = dbu;
            _ip = Constant.UsingIPAddress;
        }

        public bool LockTable(string tablename)
        {
            return LockTable(tablename, 10);
        }

        public bool LockTable(string tablename,int maxSecond)
        {
            int effectrows = 0;
            string sql = string.Format("Select * from LockTable where TableName = '{0}';", tablename);
            DataSet ds = _dbu.GetDataSet(sql);
            if (ds == null || ds.Tables.Count == 0||ds.Tables[0].Rows.Count==0)
            {
                //如果表里无记录，新增记录并锁表
                sql =
                    string.Format(
                        @"insert into LockTable (TableName,ThreadID,TSSIP,LockTime,Status)values('{0}','{1}','{2}','{3}',1);",
                        tablename, Thread.CurrentThread.ManagedThreadId.ToString(), _ip,DateTime.Now);
                effectrows = _dbu.ExecuteSql(sql);
                
            }
            else if (ds.Tables[0].Rows.Count>0)
            {
                string status = ds.Tables[0].Rows[0]["Status"].ToString();
                //如果表里有记录，判断是否已锁表
                if (status == "1")
                {
                    //判断是否超时
                    string sql_outtime = null;
                    sql_outtime = string.Format("Select LockTime from LockTable where TableName = '{0}' and Status = 1;", tablename);
                    DataSet ds_outtime = _dbu.GetDataSet(sql_outtime);
                    if (ds_outtime != null && ds_outtime.Tables.Count > 0 && ds_outtime.Tables[0].Rows.Count > 0)
                    {
                        string lockTime = ds_outtime.Tables[0].Rows[0][0].ToString();
                        TimeSpan ts = DateTime.Now.Subtract(Convert.ToDateTime(lockTime));
                        int lockSecs = ts.Seconds;
                        if (lockSecs > maxSecond)
                        {
                            Thread.Sleep(200);
                            sql =
                                string.Format(
                                    @"update LockTable set Status = 0 where TableName = '{0}' and Status = 1 and ThreadID = '{1}' and TSSIP='{2}' ;",
                                    tablename, ds.Tables[0].Rows[0]["ThreadID"].ToString(),
                                    ds.Tables[0].Rows[0]["TSSIP"].ToString());
                            _dbu.ExecuteSql(sql);
                            GC.Collect();
                            return LockTable(tablename);
                        }
                    }
                    else
                    {
                        Thread.Sleep(200);
                        GC.Collect();
                        return LockTable(tablename);
                    }
                }
                else
                {
                    //如果表里有记录，但未锁表，锁表
                        sql =
                            string.Format(
                                @"update LockTable set ThreadID='{0}',TSSIP='{1}',LockTime='{2}',Status=1 where TableName = '{3}' and Status = 0 and ThreadID = '{4}' and TSSIP='{5}' ;",
                                Thread.CurrentThread.ManagedThreadId.ToString(), _ip, DateTime.Now,tablename,
                                ds.Tables[0].Rows[0]["ThreadID"].ToString(), ds.Tables[0].Rows[0]["TSSIP"].ToString());

                    effectrows = _dbu.ExecuteSql(sql);
                }
            }

            if (effectrows == 1)
            {
                //锁表成功
                return true;
            }
            else
            {
                //锁表失败
                Thread.Sleep(200);
                return LockTable(tablename);
            }
        }

        public void UnlockAnyTable()
        {
            //string sql = string.Format(@"update LockTable t set t.Status = 0 where Status = 1 and ThreadID = '{0}' and TSSIP='{1}' ;", System.Threading.Thread.CurrentThread.ManagedThreadId.ToString(), _ip);
            //SQLite @jianghua
            string sql = string.Format(@"update LockTable set Status = 0 where Status = 1 and ThreadID = '{0}' and TSSIP='{1}' ;", Thread.CurrentThread.ManagedThreadId.ToString(), _ip);
            _dbu.ExecuteSql(sql);
        }


        public bool UnlockTable(string tablename)
        {
            int effectrows = 0;
            //string sql = string.Format(@"update LockTable t set t.Status = 0 where TableName = '{0}' and Status = 1 and ThreadID = '{1}' and TSSIP='{2}' ;", tablename, System.Threading.Thread.CurrentThread.ManagedThreadId.ToString(), _ip);
            string sql = string.Format(@"update LockTable set Status = 0 where TableName = '{0}' and Status = 1 and ThreadID = '{1}' and TSSIP='{2}' ;", tablename, Thread.CurrentThread.ManagedThreadId.ToString(), _ip);
            effectrows = _dbu.ExecuteSql(sql);

            if (effectrows == 1)
            {
                return true;
            }
            else
            {
                sql = string.Format(@"Select * from LockTable where TableName = '{0}' and Status = 1 and ThreadID = '{1}' and TSSIP='{2}';", tablename, Thread.CurrentThread.ManagedThreadId.ToString(), _ip);
                DataSet ds = _dbu.GetDataSet(sql);
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    //如果表锁存在，重试
                    Thread.Sleep(200);
                    return UnlockTable(tablename);

                }
                else
                {
                    //如果表锁不存在，返回true
                    return true;
                }
            }

        }

    }
}
