using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using QRST_DI_DS_Metadata.MetaDataDefiner.Dal;
using System.IO;
using QRST_DI_SS_Basis;
using QRST_DI_SS_Basis.MetaData;
using QRST_DI_SS_DBInterfaces.IDBEngine;
using QRST_DI_Resources;

namespace QRST_DI_TS_Process.Tasks
{
    public class taskdef
    {
        private static IDbOperating _sqLiteOperating = Constant.IdbOperating;
        private static IDbBaseUtilities _baseUtilities;
        public taskdef()
        { }
        #region Model
        private long _id;
        private string _name;
        private string _qrst_code;
        private string _description;
        private string _type;
        private string _params;
        private int _suspendable;
        private string _processexec;
        private DateTime createTime = DateTime.Now;
        /// <summary>
        /// auto_increment
        /// </summary>
        public long ID
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
        public string QRST_CODE
        {
            set { _qrst_code = value; }
            get { return _qrst_code; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string DESCRIPTION
        {
            set { _description = value; }
            get { return _description; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Type
        {
            set { _type = value; }
            get { return _type; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Params
        {
            set { _params = value; }
            get { return _params; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int Suspendable
        {
            set { _suspendable = value; }
            get { return _suspendable; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string ProcessExec
        {
            set { _processexec = value; }
            get { return _processexec; }
        }

        public DateTime CreateTime
        {
            set { createTime = value; }
            get { return createTime; }
        }
        #endregion Model

        public static List<taskdef> GetTaskDefsByStr(string dbstr)
        {
            List<taskdef> tasks = new List<taskdef>();
            dbstr.Trim();
            string[] strs0 = dbstr.Split(",".ToCharArray());

            foreach (string str1 in strs0)
            {
                taskdef task = CreateTaskDefByName(str1);
                tasks.Add(task);
            }

            return tasks;
        }

        public static taskdef CreateTaskDefByName(string taskname)
        {
            taskdef task = null;
            string sql = string.Format(@"Select * from taskdef where name='{0}'", taskname);
            _baseUtilities = _sqLiteOperating.GetSubDbUtilities(EnumDBType.MIDB);
            DataSet ds = _baseUtilities.GetDataSet(sql);

            if (ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    task = DBRow2TaskDefCls(ds.Tables[0].Rows[0]);
                }
            }

            return task;
        }

        public static void AddtaskDef(taskdef _taskdef)
        {
            _baseUtilities = _sqLiteOperating.GetSubDbUtilities(EnumDBType.MIDB);
            _sqLiteOperating.LockTable("taskdef", EnumDBType.MIDB);
            //TableLocker dblock = new TableLocker(TSPCommonReference.dbOperating.MIDB);
            //dblock.LockTable("taskdef");
            tablecode_Dal tablecode = new tablecode_Dal(_baseUtilities);
            _taskdef.ID = _baseUtilities.GetMaxID("ID", "taskdef");
            _taskdef.QRST_CODE = tablecode.GetDataQRSTCode("taskdef", (int)_taskdef.ID);
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into taskdef(");
            strSql.Append("NAME,QRST_CODE,DESCRIPTION,Type,Params,Suspendable,ProcessExec,CreateTime)");
            strSql.Append(" values (");
            strSql.AppendFormat("'{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}')", _taskdef.NAME, _taskdef.QRST_CODE, _taskdef.DESCRIPTION, _taskdef.Type, _taskdef.Params, _taskdef.Suspendable, _taskdef.ProcessExec, _taskdef.createTime.ToString());
            _baseUtilities.ExecuteSql(strSql.ToString());
            _sqLiteOperating.UnlockTable("taskdef",EnumDBType.MIDB);
        }

        public static void UpdatetaskDef(taskdef _taskdef)
        {
            _baseUtilities = _sqLiteOperating.GetSubDbUtilities(EnumDBType.MIDB);
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update taskdef set ");
            strSql.AppendFormat("NAME='{0}',", _taskdef.NAME);
            strSql.AppendFormat("DESCRIPTION='{0}',", _taskdef.DESCRIPTION);
            strSql.AppendFormat("Type='{0}',", _taskdef.Type);
            strSql.AppendFormat("Params='{0}',", _taskdef.Params);
            strSql.AppendFormat("Suspendable='{0}',", _taskdef.Suspendable);
            strSql.AppendFormat("ProcessExec='{0}'", _taskdef.ProcessExec);
            strSql.AppendFormat("CreateTime = '{0}'", _taskdef.createTime.ToString());
            strSql.AppendFormat(" where QRST_CODE={0} ", _taskdef.QRST_CODE);
            _baseUtilities.ExecuteSql(strSql.ToString());
        }

        public static taskdef DBRow2TaskDefCls(DataRow dr)
        {
            taskdef task = new taskdef();
            task.NAME = dr["Name"].ToString();
            task.Type = dr["Type"].ToString();
            task.DESCRIPTION = dr["Description"].ToString();
            task.ProcessExec = dr["Processexec"].ToString();
            task.QRST_CODE = dr["QRST_CODE"].ToString();
            task.Params = dr["Params"].ToString();
            task.CreateTime = DateTime.Parse(dr["CreateTime"].ToString());
            return task;
        }

        public static DataSet GetDataSet(string whereCondition)
        {
            _baseUtilities = _sqLiteOperating.GetSubDbUtilities(EnumDBType.MIDB);
            StringBuilder strSql = new StringBuilder("select * from taskdef ");
            if (!string.IsNullOrEmpty(whereCondition))
            {
                strSql.AppendFormat("where {0}", whereCondition);
            }
            DataSet ds = _baseUtilities.GetDataSet(strSql.ToString());
            return ds;
        }

        public static List<taskdef> GetTaskdefLst(string whereCondition)
        {
            List<taskdef> lst = new List<taskdef>();
            DataSet ds = GetDataSet(whereCondition);
            if (ds != null && ds.Tables.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    lst.Add(DBRow2TaskDefCls(ds.Tables[0].Rows[i]));
                }

            }
            return lst;

        }

        public override bool Equals(object obj)
        {
            try
            {
                taskdef _taskdef = (taskdef)obj;
                if (this.NAME != _taskdef.NAME)
                {
                    return false;
                }
                if (this.DESCRIPTION != _taskdef.DESCRIPTION)
                {
                    return false;
                }
                if (this.Type != _taskdef.Type)
                {
                    return false;
                }
                if (this.ProcessExec != _taskdef.ProcessExec)
                {
                    return false;
                }
                if (this.Params != _taskdef.Params)
                {
                    return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        /// <summary>
        ///生成组件描述文件,
        /// </summary>
        /// <param name="dllFilePath"></param>
        public void CreateDllDesFile(string dllFilePath)
        {
            try
            {
                string filename = string.Format(@"{0}\{1}.txt", Path.GetDirectoryName(dllFilePath), Path.GetFileNameWithoutExtension(dllFilePath));
                FileStream fs = new FileStream(filename, FileMode.OpenOrCreate, FileAccess.ReadWrite);
                StreamWriter sw = new StreamWriter(fs);
                sw.WriteLine(CreateTime.ToString());
                string[] files = Directory.GetFiles(Path.GetDirectoryName(dllFilePath));
                for (int i = 0; i < files.Length; i++)
                {
                    sw.WriteLine(Path.GetFileName(files[i]));
                }
                sw.Close();
                fs.Close();
            }
            catch (Exception ex)
            {
                throw new Exception("组件文件创建失败：" + ex.ToString());
            }
        }

        /// <summary>
        /// 获取本地组件中的创建时间
        /// </summary>
        /// <param name="dllDir"></param>
        /// <returns></returns>
        public static DateTime GetDllCreateTime(string dllFilePath)
        {
            string desFileName = string.Format(@"{0}\{1}.txt", Path.GetDirectoryName(dllFilePath), Path.GetFileNameWithoutExtension(dllFilePath));
            if (!File.Exists(desFileName))
            {
                //  throw new Exception("未能找到组件描述文件！");
                return DateTime.MinValue;
            }
            try
            {
                FileStream fs = new FileStream(desFileName, FileMode.Open, FileAccess.Read);
                StreamReader sr = new StreamReader(fs);
                DateTime createTime = DateTime.Parse(sr.ReadLine());
                sr.Close();
                fs.Close();
                return createTime;
            }
            catch (Exception ex)
            {
                return DateTime.MinValue;
            }
        }

    }
}
