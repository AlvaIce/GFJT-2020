using System;
using QRST_DI_SS_DBInterfaces.IDBEngine;
using QRST_DI_Resources;
using System.Data;

namespace QRST_DI_MS_Basis.Taskinfo
{
    public class taskinfo
    {
        public static IDbBaseUtilities sqlUtilities;

        public taskinfo()
        {
        }
        #region Model
        private int _id;
        private string _name;
        private int _subsystemid;
        private int _methodid;
        private string _sourcename;
        private string _sharepath;
        private string _status;
        private DateTime _assigntime;
        private string _priority;
        private string _comment;
        private string _qrst_code;
        /// <summary>
        /// 
        /// </summary>
        public int ID
        {
            set { _id = value; }
            get { return _id; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string NAME
        {
            set { _name = value; }
            get { return _name; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int SUBSYSTEMID
        {
            set { _subsystemid = value; }
            get { return _subsystemid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int METHODID
        {
            set { _methodid = value; }
            get { return _methodid; }
        }
        
        /// /// <summary>
        /// 
        /// </summary>
        public string SOURCENAME
        {
            set { _sourcename = value; }
            get { return _sourcename; }
        }
        /// /// <summary>
        /// 
        /// </summary>
        public string SHAREPATH
        {
            set { _sharepath = value; }
            get { return _sharepath; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string STATUS
        {
            set { _status = value; }
            get { return _status; }
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime ASSIGNTIME
        {
            set { _assigntime = value; }
            get { return _assigntime; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string PRIORITY
        {
            set { _priority = value; }
            get { return _priority; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string COMMENT
        {
            set { _comment = value; }
            get { return _comment; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string QRST_CODE
        {
            set { _qrst_code = value; }
            get { return _qrst_code; }
        }

        //添加任务分发信息表
        public string Add(string name,int subsystemid, int methodid,string sourcename,string tempPath,string comment)
        {
            string sql = "";
            try
            {
                sqlUtilities = Constant.IdbServerUtilities.GetSubDBUtil("midb");
                int id = sqlUtilities.GetMaxID("ID", "taskinfo");
                string sid = Convert.ToString(id);
                tempPath = tempPath.Replace(@"\", @"/");
                string qrst_code = "MIDB-26-"+sid;
                string assigntime= DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss");
                sql = string.Format("insert into taskinfo (ID,NAME,SubsystemID,MethodID,SourceName,SharePath,Status,Assigntime,Priority,Comment,QRST_CODE)values ('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}')", id, name, subsystemid, methodid, sourcename, tempPath + sid + name, "waiting", assigntime, "Normal", comment, qrst_code);
                sqlUtilities.ExecuteSql(sql);
                return sid;
            }
            catch (Exception ex)
            {
                MyConsole.WriteLine("[taskinfo Add Error]sql:" + sql + "###Exception:" + ex.Message);
                return null;
            }
            
        }

        //获得子系统服务方法的ID
        public string MethodID(string mName)
        {
            try
            {
                sqlUtilities = Constant.IdbServerUtilities.GetSubDBUtil("midb");
                DataSet allMethodInfo = sqlUtilities.GetDataSet("select MethodID from methodcode where Name ='" + mName + "'");
                DataTable MethodInfo = allMethodInfo.Tables[0];
                string mid = MethodInfo.Rows[0][0].ToString();
                return mid;
            }
            catch (Exception ex)
            {
                MyConsole.WriteLine("[Get method Error]sql:"  + "###Exception:" + ex.Message);
                return null;
            }
        }
    }
}
        #endregion