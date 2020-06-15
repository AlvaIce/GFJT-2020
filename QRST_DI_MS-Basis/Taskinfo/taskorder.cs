using QRST_DI_SS_DBInterfaces.IDBEngine;
using QRST_DI_Resources;
using QRST_DI_SS_Basis.MetaData;
using System.Data;

namespace QRST_DI_MS_Basis.Taskinfo
{
    public class taskorder
    {
        private static IDbOperating _sqLiteOperating =
           Constant.IdbOperating;

        private static IDbBaseUtilities _sqLiteUtilities =
            Constant.IdbOperating.GetSubDbUtilities(EnumDBType.MIDB);

        //根据状态获取所需任务
        public static DataTable GetTaskTable(string status)
        {
            string sql =  string.Format(@"Select * from taskmonitor_view where Status='{0}';", status);
            DataSet ds = _sqLiteUtilities.GetDataSet(sql);
            return ds.Tables[0];
        }

        //根据查询条件获取任务列表
        public static DataTable GetTaskList(string whereCondition)
        {
            string sql = "select * from taskmonitor_view";
            if (!string.IsNullOrEmpty(whereCondition))
            {
                sql = sql + " where " + whereCondition;
            }
            sql = sql + " order by AssignTime desc";
            DataSet ds = _sqLiteUtilities.GetDataSet(sql);

            return ds.Tables[0];
        }
    }
}
