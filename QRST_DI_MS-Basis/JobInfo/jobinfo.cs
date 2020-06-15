using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using QRST_DI_SS_DBInterfaces.IDBEngine;
using QRST_DI_Resources;
using QRST_DI_SS_Basis.MetaData;
using System.Data;

namespace QRST_DI_MS_Basis.JobInfo
{
    public class jobinfo
    {
        //private static IDbOperating _sqLiteOperating =
        //    Constant.IdbOperating;

        private static IDbBaseUtilities _mySQlUtilities =
            Constant.IdbOperating.GetSubDbUtilities(EnumDBType.MIDB);

        public static DataTable getJobInfo()
        {
            string sql = "select * from job_info";
            DataSet ds = _mySQlUtilities.GetDataSet(sql);
            return ds.Tables[0];
        }

        public static int insertNewJob(string values)
        {
            string sql = "insert into job_info (ID,ComputeID,name,state,finalStatus,progress,startedTime,finishedTime,elapsedTime) values ";
            return _mySQlUtilities.ExecuteSql(sql + values);
        }

        public static DataTable getNewsestJob()
        {
            string sql = "select * from job_info order by ID desc";
            DataSet ds = _mySQlUtilities.GetDataSet(sql);
            return ds.Tables[0];
        }

        public static void updateJob(string set,string where)
        {   
            string sql = string.Format("update job_info set {0} where {1}", set, where);
            //_sqLiteOperating.LockTable("job_info", EnumDBType.MIDB);
             _mySQlUtilities.ExecuteSql(sql);

           // _sqLiteOperating.UnlockTable("job_info", EnumDBType.MIDB);
           // DataTable dtn = dt;
            //return _mySQlUtilities.ExecuteSql(sql);
        }
    }
}
