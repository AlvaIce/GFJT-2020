using QRST_DI_Resources;
using QRST_DI_SS_DBClient.DBEngine;
using QRST_DI_SS_DBInterfaces.IDBEngine;

namespace QRST_DI_TS_Process
{
    //被抛弃，功能合并到constant 中   @jianghua 20170601
    //public class TSPCommonReference
    //{
    //    public static bool isCreated = false;
    //    //public static Dictionary<int, DBMySqlOperating> dboptdict;
    //    public static void Create()
    //    {
    //        //dboptdict = new Dictionary<int, DBMySqlOperating>();
    //        //dbOperating = new DBMySqlOperating();
    //        if (Constant.Created)
    //        {
    //            idbOperating = Constant.IdbOperating;
    //        }
    //        else
    //        {
    //            idbOperating = DbOperatingTCPClient.InitTCPClient_StorageChl(Constant.DbServerIp, Constant.dbUtilityTcpPort);
    //        }          
    //        isCreated = true;
    //    }

    //    //public static DBMySqlOperating dbOperating { };
    //    public static IDbOperating idbOperating;

    //    //{
    //    //    get
    //    //    {                
    //    //        if (!dboptdict.ContainsKey(Thread.CurrentThread.ManagedThreadId))
    //    //        {
    //    //            dboptdict.Add(Thread.CurrentThread.ManagedThreadId, new DBMySqlOperating());
    //    //        }
    //    //        DBMySqlOperating dbopt = dboptdict[Thread.CurrentThread.ManagedThreadId];
    //    //        if (dbopt == null)
    //    //        {
    //    //            dboptdict[Thread.CurrentThread.ManagedThreadId] = new DBMySqlOperating();
    //    //            dbopt = dboptdict[Thread.CurrentThread.ManagedThreadId];
    //    //        }
    //    //        return dbopt;
    //    //    }
    //    //}
    //}
}