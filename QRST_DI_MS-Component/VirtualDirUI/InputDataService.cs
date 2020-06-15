using QRST_DI_MS_Basis.UserRole;
using QRST_DI_Resources;
using QRST_DI_SS_DBInterfaces.IDBEngine;

namespace QRST_DI_MS_Component.VirtualDirUI
{ 
    public  class InputDataService
    { 
        public static IDbBaseUtilities BSDB_sqlUtilities
           { get { return Constant.IdbServerUtilities.GetSubDBUtil("BSDB"); } set{}}
        public static IDbBaseUtilities INDB_sqlUtilities
        { get { return Constant.IdbServerUtilities.GetSubDBUtil("INDB"); } set{} }
        public static IDbBaseUtilities ISDB_sqlUtilities
        { get { return Constant.IdbServerUtilities.GetSubDBUtil("ISDB"); } set { } }
        public static IDbBaseUtilities EVDB_sqlUtilities
        { get { return Constant.IdbServerUtilities.GetSubDBUtil("EVDB"); } set { } }
        public static IDbBaseUtilities MADB_sqlUtilities
        { get { return Constant.IdbServerUtilities.GetSubDBUtil("MADB"); } set { } }
        public static userInfo currentUser { get; set; }

        
    }
}
