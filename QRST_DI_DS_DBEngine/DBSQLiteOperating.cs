using System;
using System.Data;
using QRST_DI_Resources;
using QRST_DI_SS_Basis;
using QRST_DI_SS_Basis.MetaData;
using System.Windows.Forms;
using QRST_DI_SS_DBInterfaces.IDBEngine;
 
namespace QRST_DI_DS_DBEngine
{
    [Serializable]
    public class DBSQLiteOperating:MarshalByRefObject,IDbOperating 
    {
        public
            DBSQLiteOperating()
        {
            MIDB = new SQLiteBaseUtilities();
            InitializeDBConnection();
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

        public IDbBaseUtilities GetSubDbUtilities(EnumDBType dbtype)
        {
            IDbBaseUtilities SQLiteOperating;
            switch (dbtype)
            {
                case EnumDBType.MIDB:
                    SQLiteOperating = MIDB;
                    break;
                case EnumDBType.BSDB:
                    SQLiteOperating = BSDB;
                    break;
                case EnumDBType.MADB:
                    SQLiteOperating = MADB;
                    break;
                case EnumDBType.RCDB:
                    SQLiteOperating = RCDB;
                    break;
                case EnumDBType.EVDB:
                    SQLiteOperating = EVDB;
                    break;
                case EnumDBType.IPDB:
                    SQLiteOperating = IPDB;
                    break;
                case EnumDBType.ISDB:
                    SQLiteOperating = ISDB;
                    break;
                default:
                    SQLiteOperating = MIDB;
                    break;
            }
            return SQLiteOperating;
        }

        /// <summary>
        /// 获取数据库服务地址IP
        /// </summary>
        /// <returns></returns>
       public string GetDbServerIp()
        {
            return Constant.DbServerIp;

        }

        /// <summary>
        /// 相对数据库地址转绝对数据库地址
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public string GetAbsoluteDbCon(string connectStr)
        {
            string aboConStr=null;
            string rootPath = Application.StartupPath.ToString().Trim();
                    string[] relativePathArr = connectStr.Split('=');
                    string tempConn = OperateFilePathFun.GetMixedAbsPath(rootPath, relativePathArr[1]);
                    aboConStr = string.Format("{0}={1}", relativePathArr[0], tempConn);
            return aboConStr;
        }

        /// <summary>
        /// SQLite版的单机模式，采用本地路径。多机采用网络共享，业务层控制
        /// </summary>
        /// <returns></returns>
        public string GetDbRootDataPath()
        {
            string rootPath = null;
            string sql =
            "select appsettings.value from appsettings where appsettings.key in ('DBServerAbsolutePath','DataRelativePath')";
            DataSet ds = null;
            string dbServerAbsolutePath = null;

            switch (Constant.DbStorage)
            {
                case EnumDbStorage.SINGLE:
                    ds = MIDB.GetDataSet(sql);
                    dbServerAbsolutePath = ds.Tables[0].Rows[0][0].ToString();
                    string tileDataRelativePath = ds.Tables[0].Rows[1][0].ToString();
                    rootPath = OperateFilePathFun.GetMixedAbsPath(dbServerAbsolutePath, tileDataRelativePath);
                    break;
                case EnumDbStorage.MULTIPLE:
                    break;
            }
            return rootPath;
        }

        #region 初始化数据库链接
        public SQLiteBaseUtilities MIDB
        {
            get;
            protected set;
        }

        public SQLiteBaseUtilities BSDB
        {
            get;
            protected set;
        }

        public SQLiteBaseUtilities EVDB
        {
            get;
            protected set;
        }

        public SQLiteBaseUtilities RCDB
        {
            get;
            protected set;
        }

        public SQLiteBaseUtilities MADB
        {
            get;
            protected set;
        }

        public SQLiteBaseUtilities ISDB
        {
            get;
            protected set;
        }

        public SQLiteBaseUtilities INDB
        {
            get;
            protected set;
        }
        public SQLiteBaseUtilities IPDB
        {
            get;
            protected set;
        }

        private void InitializeDBConnection()
        {
            DataSet ds = MIDB.GetDataSet(@"Select Name, ConnectStr from subdbinfo");

            if (ds.Tables.Count > 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    string name = dr["Name"].ToString();
                    string con = dr["ConnectStr"].ToString();
                    con = GetAbsoluteDbCon(con);
                    switch (name.ToUpper())
                    {
                        case "BSDB":
                            BSDB = new SQLiteBaseUtilities(con);
                            break;
                        case "EVDB":
                            EVDB = new SQLiteBaseUtilities(con);
                            break;
                        case "RCDB":
                            RCDB = new SQLiteBaseUtilities(con);
                            break;
                        case "ISDB":
                            ISDB = new SQLiteBaseUtilities(con);
                            break;
                        case "MADB":
                            MADB = new SQLiteBaseUtilities(con);
                            break;
                        case "IPDB":
                            IPDB = new SQLiteBaseUtilities(con);
                            break;
                        case "INDB":
                            INDB = new SQLiteBaseUtilities(con);
                            break;
                        default:
                            break;
                    }
                }
            }
        }
        #endregion


        #region 判断表或编码是否存在
        /// <summary>
        /// 判断该表是否存在
        /// </summary>
        /// <param name="TableName">表名称</param>
        /// <param name="dbtype">目标数据库</param>
        public bool JudgeTableExist(string TableName, EnumDBType dbtype)
        {
            bool isExist = false;
            IDbBaseUtilities SQLiteOperating = GetSubDbUtilities(dbtype);
            try
            {
                int JustbleNum = SQLiteOperating.GetRecordCount("select QRST_CODE from tablecode where TABLE_NAME = '" + TableName + "'");
                int t = JustbleNum;
                if (JustbleNum != 0)
                {
                    isExist = true;
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            return isExist;
        }

        /// <summary>
        /// 根据子库名称，获取数据库连接对象，
        /// </summary>
        /// <param name="dbName">子库名称如：evdb</param>
        /// <returns></returns>
        public IDbBaseUtilities GetsqlBaseObj(string dbName)
        {
            string subdb = dbName.ToUpper();
            switch (subdb)
            {
                case "MIDB":
                    return MIDB;
                case "BSDB":
                    return BSDB;
                case "MADB":
                    return MADB;
                case "RCDB":
                    return RCDB;
                case "EVDB":
                    return EVDB;
                case "IPDB":
                    return IPDB;
                case "ISDB":
                    return ISDB;
                case "INDB":
                    return INDB;
                default:
                    return null;
            }

        }

        /// <summary>
        /// 判断编码是否为表的编码
        /// </summary>
        /// <param name="TableCode">表编码</param>
        public bool JudgeTableCodeExist(string TableCode, EnumDBType dbtype)
        {
            bool isExist = false;
             IDbBaseUtilities SQLiteOperating = GetSubDbUtilities(dbtype);
            try
            {
                int JustbleNum = SQLiteOperating.GetRecordCount("select TABLE_NAME from tablecode where QRST_CODE ='" + TableCode + "'");
                if (JustbleNum != 0)
                {
                    isExist = true;
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            return isExist;
        }
        #endregion

        #region 固定的表名与表编码转换

        /// <summary>
        ///依据表名获取表的编码 
        /// </summary>
        /// <param name="TableName">表名称</param>
        /// <returns>表编码</returns>
        public string GetTableCodeFromTableName(string TableName, EnumDBType dbtype)
        {
            //判断该表是否存在
            bool isTableExist = JudgeTableExist(TableName, dbtype);
            IDbBaseUtilities SQLiteOperating = GetSubDbUtilities(dbtype);
            if (isTableExist == true)
            {
                DataSet allTableInfo = SQLiteOperating.GetDataSet("select QRST_CODE from tablecode where TABLE_NAME = '" + TableName + "'");
                DataTable tableInfo = allTableInfo.Tables[0];
                string tableCode = tableInfo.Rows[0][0].ToString();
                return tableCode;
            }
            else
            {
                throw new Exception("该表不存在！");
            }
        }

        /// <summary>
        /// 依据编码获取表的名称
        /// </summary>
        /// <param name="TableCode">表编码</param>
        /// <returns>表名</returns>
        //依据表编码获取表名称
        public string GetTableNameFromTableCode(string TableCode, EnumDBType dbtype)
        {
            //判断该编码是否是表的编码
            bool isTableCodeExist = JudgeTableCodeExist(TableCode, dbtype);
            IDbBaseUtilities sqliteOperating = GetSubDbUtilities(dbtype);
            if (isTableCodeExist == true)
            {
                DataSet allCodeInfo = sqliteOperating.GetDataSet("select TABLE_NAME from tablecode where QRST_CODE ='" + TableCode + "'");
                DataTable CodeInfo = allCodeInfo.Tables[0];
                string tableName = CodeInfo.Rows[0][0].ToString();
                return tableName;
            }
            else
            {
                throw new Exception("该编码不是表编码！");
            }
        }

        /// <summary>
        /// 依据记录获取所有表名称
        /// </summary>
        /// <param name="TableCode">记录编码</param>
        /// <returns>所有表名</returns>
        public string GetTableNameFromColumnCode(string TableCode, EnumDBType dbtype)
        {
            string tableName = "";
            //判断该编码是否是记录编码
            bool isTableCodeExist = JudgeTableCodeExist(TableCode, dbtype);
            if (isTableCodeExist == false)
            {
                string[] CodeField = TableCode.Split('-');
                //获取初始最上层的表名称
                string[] allTableName = new string[100];
                tableName = GetTableNameFromTableCode(CodeField[0], dbtype);
                //获得记录的所有表名称
                string[] secTableCode = new string[100];
                secTableCode[0] = CodeField[0];
                for (int ii = 1; ii < CodeField.Length; ii++)
                {
                    secTableCode[ii] = secTableCode[ii - 1] + "-" + Convert.ToString(CodeField[ii]);
                    bool isExist = JudgeTableCodeExist(secTableCode[ii], dbtype);
                    if (isExist == true)
                    {
                        allTableName[ii - 1] = GetTableNameFromTableCode(secTableCode[ii], dbtype);
                        tableName = tableName + ";" + allTableName[ii - 1];
                    }
                    else
                    {
                        break;
                    }
                }

            }
            return tableName;
        }

        /// <summary>
        /// 依据记录获取当前表名称
        /// </summary>
        /// <returns></returns>
        public string GetCurrentTableNameFromColumnCode(string TableCode, EnumDBType dbtype)
        {
            string tableName = "";
            //判断该编码是否是记录编码
            bool isTableCodeExist = JudgeTableCodeExist(TableCode, dbtype);
            if (isTableCodeExist == false)
            {
                string[] CodeField = TableCode.Split('-');
                //获取初始最上层的表名称
                string[] allTableName = new string[100];
                tableName = GetTableNameFromTableCode(CodeField[0], dbtype);
                //获得记录的所有表名称
                string[] secTableCode = new string[100];
                secTableCode[0] = CodeField[0];
                for (int ii = 1; ii < CodeField.Length; ii++)
                {
                    secTableCode[ii] = secTableCode[ii - 1] + "-" + Convert.ToString(CodeField[ii]);
                    bool isExist = JudgeTableCodeExist(secTableCode[ii], dbtype);
                    if (isExist == true)
                    {
                        allTableName[ii - 1] = GetTableNameFromTableCode(secTableCode[ii], dbtype);
                        tableName = allTableName[ii - 1];
                    }
                    else
                    {
                        break;
                    }
                }
            }
            return tableName;
        }

        #endregion 固定的表名与表编码转换

        #region 根据表名或者编码获取新的表记录编码
        /// <summary>
        /// 依据表名插入表的新纪录编码
        /// </summary>
        /// <param name="TableName">表名</param>
        /// <returns>新纪录编码</returns>
        public string GetNewColumnCodeFromTableName(string TableName, EnumDBType dbtype)
        {
            string tableCode = "";
            tableCode = GetTableCodeFromTableName(TableName, dbtype);
            int maxIDDS = GetNewID(TableName, dbtype);
            string NewColumnCode = tableCode + "-" + Convert.ToString(maxIDDS + 1);
            return NewColumnCode;
        }
        ////////////////////////////////////////////////////////////////   lxl 15:01
        /// <summary>
        /// 依据表名插入表的新纪录编码
        /// </summary>
        /// <param name="TableName">表名</param>
        /// <returns>新纪录编码</returns>
        public string GetNewQrstCodeFromTableName(string TableName, EnumDBType dbtype)
        {
            string tableCode = "";
            tableCode = GetTableCodeFromTableName(TableName, dbtype);
            int newIDDS = GetNewID(TableName, dbtype);
            string NewColumnCode = tableCode + "-" + Convert.ToString(newIDDS);
            return NewColumnCode;
        }
        /// <summary>
        /// 依据表名插入表的新纪录编码的前缀
        /// </summary>
        /// <param name="TableName">表名</param>
        /// <returns>新纪录编码的前缀</returns>
        public string GetPreNewColumnCodeFromTableName(string TableName, EnumDBType dbtype)
        {
            int id = 0;
            return GetPreNewColumnCodeFromTableName(TableName, dbtype, out id);
        }

        ////////////////////////////////////////////////////////////////   lxl 15:01
        /// <summary>
        /// 依据表名插入表的新纪录编码的前缀
        /// </summary>
        /// <param name="TableName">表名</param>
        /// <returns>新纪录编码的前缀</returns>
        public string GetPreNewColumnCodeFromTableName(string TableName, EnumDBType dbtype, out int newID)
        {
            string tableCode = "";
            tableCode = GetTableCodeFromTableName(TableName, dbtype);
            newID = GetNewID(TableName, dbtype);
            string NewColumnCode = tableCode + "-";
            return NewColumnCode;
        }
        public int GetNewID(string TableName, EnumDBType dbtype)
        {
            //判断该表是否存在
            bool isTableExist = JudgeTableExist(TableName, dbtype);
            IDbBaseUtilities sqliteOperating = GetSubDbUtilities(dbtype);
            if (isTableExist == true)
            {
                int maxIDDS = 0;
                int tempflag = sqliteOperating.GetRecordCount("select * from " + TableName);
                if (tempflag != 0)
                {

                    DataTable temp = sqliteOperating.GetDataSet("select max(id) from " + TableName).Tables[0];
                    string maxID = temp.Rows[0][0].ToString();
                    if (maxID != "")
                    {
                        maxIDDS = Convert.ToInt32(maxID);
                    }

                }
                return maxIDDS + 1;
            }
            else
            {
                throw new Exception("该表不存在！");
            }
        }

        /// <summary>
        /// 依据编码插入表的新纪录编码
        /// </summary>
        /// <param name="TableCode">表编码</param>
        /// <returns>表的新纪录编码</returns>
        public string GetNewColumnCodeFromTableCode(string TableCode, EnumDBType dbtype)
        {
            string tableName = null;
            string newCode = null;
            //判断该编码是否为表名
            bool isExist = JudgeTableCodeExist(TableCode, dbtype);
            if (isExist == true)
            {
                tableName = GetTableNameFromTableCode(TableCode, dbtype);
                newCode = GetNewColumnCodeFromTableName(tableName, dbtype);
            }
            else
            {
                tableName = GetTableNameFromColumnCode(TableCode, dbtype);
                string[] perTableName = tableName.Split(';');
                newCode = GetNewColumnCodeFromTableName(perTableName[perTableName.Length - 1], dbtype);
            }
            return newCode;
        }
        #endregion 根据表名或者编码获取新的表记录编码

        #region 生成数据表或者辅助表的编码


        /// <summary>
        /// 生成数据表的code
        /// </summary>
        /// <param name="DataType">数据表类型</param>
        /// <param name="tableName">表名</param>
        /// <returns>表编码</returns>
        public string GetNewTableCode(EnumDataTypes DataType, string tableName, EnumDBType dbtype)
        {
            string newTableCode = null;
            //判断该表是否存在
            bool isTableExist = JudgeTableExist(tableName, dbtype);
            IDbBaseUtilities sqliteOperating = GetSubDbUtilities(dbtype);
            if (isTableExist == false)
            {
                switch (DataType)
                {
                    case EnumDataTypes.other:
                        break;
                    //获取栅格数据表的表编码
                    case EnumDataTypes.Raster:
                        newTableCode = GetNewColumnCodeFromTableName("PRODS_RASTER", dbtype);
                        break;
                    //获取统计数据表的表编码
                    case EnumDataTypes.Static:
                        newTableCode = GetNewColumnCodeFromTableName("PRODS_TABLE", dbtype);
                        break;
                }
            }
            else
            {
                throw new Exception("该表已存在！");
            }
            return newTableCode;
        }

        /// <summary>
        /// 生成辅助表的code
        /// </summary>
        /// <param name="hypTableName">辅助表表名</param>
        /// <returns>辅助表表编码</returns>
        public string GetNewHypTableCode(string hypTableName, EnumDBType dbtype)
        {
            string NewTableCode = null;
            //判断该表是否存在
            bool isTableExist = JudgeTableExist(hypTableName, dbtype);
            IDbBaseUtilities sqlOperating = GetSubDbUtilities(dbtype);
            if (isTableExist == false)
            {
                string[] fieldName = hypTableName.Split('_');
                string midTableName = fieldName[0] + '_' + fieldName[1] + '_' + fieldName[2];
                bool ismidTableExist = JudgeTableExist(midTableName, dbtype);
                if (ismidTableExist == true)
                {
                    string midCode = GetTableCodeFromTableName(midTableName, dbtype);
                    int allCulumnNum = sqlOperating.GetRecordCount("select * from tablecode where regexp_like (TABLE_NAME, '^" + midTableName + "_')");
                    NewTableCode = midCode + 'A' + '0' + Convert.ToString(allCulumnNum + 1);
                }
                return NewTableCode;
            }
            else
            {
                throw new Exception("该表已存在！");
            }
        }
        #endregion 生成数据表或者辅助表的编码

        #region 锁表
        public bool LockTable(string tablename, EnumDBType dbtype)
        {
            IDbBaseUtilities sqlOperating = GetSubDbUtilities(dbtype);
            TableLocker tblLocker=new TableLocker(sqlOperating);
            return tblLocker.LockTable(tablename);
        }

        public void UnlockAnyTable(EnumDBType dbtype)
        {
            IDbBaseUtilities sqlOperating = GetSubDbUtilities(dbtype);
            TableLocker tblLocker = new TableLocker(sqlOperating);
            tblLocker.UnlockAnyTable();
        }

        public bool UnlockTable(string tablename, EnumDBType dbtype)
        {
            IDbBaseUtilities sqlOperating = GetSubDbUtilities(dbtype);
            TableLocker tblLocker = new TableLocker(sqlOperating);
            return tblLocker.UnlockTable(tablename);
        }
        #endregion


        #region 表时间同步到本地
        public void SynDBTimeToLocal()
        {
            SynDBTimeToLocalClass sdbt = new SynDBTimeToLocalClass();
            sdbt.SynDBTimeToLocal();
        }
        #endregion


    }
}
