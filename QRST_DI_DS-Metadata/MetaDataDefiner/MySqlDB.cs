using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using QRST_DI_DS_Metadata.MetaDataDefiner.Mdl;
using QRST_DI_Resources;
using QRST_DI_SS_DBInterfaces.IDBEngine;

namespace QRST_DI_DS_Metadata.MetaDataDefiner
{
    public class MySqlDB
    {
        private string _dbPath;
        private string _password;
        private string _ip;

        private IDbBaseUtilities infoBaseUtilities;  //information_schema的数据库连接对象

        public MySqlDB(string password, string ip,string dbPath)
        {
            _dbPath = dbPath;
            _password = password;
            _ip = ip;

            string connectionStr = string.Format("Data Source={0};Password={1};", _dbPath, _password);
            infoBaseUtilities = Constant.IdbServerUtilities.GetSubDbUtilByCon(connectionStr);
            if (!infoBaseUtilities.ConnectTest())
            {
                throw new Exception("无法连接到服务器");
            }
        }

        public MySqlDB()
        {
            infoBaseUtilities = Constant.IdbServerUtilities;
            if (!infoBaseUtilities.ConnectTest())
            {
                throw new Exception("无法连接到服务器");
            }
        }

        public MySqlDB(IDbBaseUtilities utilities)
        {
            infoBaseUtilities = utilities;
            if (!infoBaseUtilities.ConnectTest())
            {
                throw new Exception("无法连接到服务器");
            }
        }

        /// <summary>
        /// 获取mysql数据库中使用的数据集
        /// </summary>
        /// <returns></returns>
        public List<string> GetCharSet()
        {
            string sqlString = "select CHARACTER_SET_NAME from information_schema.CHARACTER_SETS";
            DataSet ds = infoBaseUtilities.GetDataSet(sqlString);

            List<string> charSetLst = new List<string>();

            if (ds != null && ds.Tables.Count > 0)
            {
                for (int i = 0 ; i < ds.Tables[0].Rows.Count ; i++)
                {
                    charSetLst.Add(ds.Tables[0].Rows[i][0].ToString());
                }
            }
            return charSetLst;
        }

        /// <summary>
        /// 获取数据库引擎列表
        /// </summary>
        /// <returns></returns>
        public List<string> GetEngineLst()
        {
            string sqlString = "select ENGINE from ENGINES";
            DataSet ds = infoBaseUtilities.GetDataSet(sqlString);

            List<string> engineLst = new List<string>();

            if (ds != null && ds.Tables.Count > 0)
            {
                for (int i = 0 ; i < ds.Tables[0].Rows.Count ; i++)
                {
                    engineLst.Add(ds.Tables[0].Rows[i][0].ToString());
                }
            }
            return engineLst;
        }

        /// <summary>
        /// 根据表名获取表的基本信息
        /// </summary>
        /// <returns></returns>
        public TABLES_Mdl GetTableModel(string database, string tablename)
        {
            TABLES_Mdl model = new TABLES_Mdl();

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select TABLE_CATALOG,TABLE_SCHEMA,TABLE_NAME,TABLE_TYPE,ENGINE,VERSION,ROW_FORMAT,TABLE_ROWS,AVG_ROW_LENGTH,DATA_LENGTH,MAX_DATA_LENGTH,INDEX_LENGTH,DATA_FREE,AUTO_INCREMENT,CREATE_TIME,UPDATE_TIME,CHECK_TIME,TABLE_COLLATION,CHECKSUM,CREATE_OPTIONS,TABLE_COMMENT from information_schema.TABLES ");
            strSql.Append(string.Format(" where  TABLE_SCHEMA='{0}' and TABLE_NAME='{1}'  ", database, tablename));
            DataSet ds = infoBaseUtilities.GetDataSet(strSql.ToString());

            if (ds.Tables[0].Rows.Count > 0)
            {
                model.TABLE_CATALOG = ds.Tables[0].Rows[0]["TABLE_CATALOG"].ToString();
                model.TABLE_SCHEMA = ds.Tables[0].Rows[0]["TABLE_SCHEMA"].ToString();
                model.TABLE_NAME = ds.Tables[0].Rows[0]["TABLE_NAME"].ToString();
                model.TABLE_TYPE = ds.Tables[0].Rows[0]["TABLE_TYPE"].ToString();
                model.ENGINE = ds.Tables[0].Rows[0]["ENGINE"].ToString();
                if (ds.Tables[0].Rows[0]["VERSION"].ToString() != "")
                {
                    model.VERSION = long.Parse(ds.Tables[0].Rows[0]["VERSION"].ToString());
                }
                model.ROW_FORMAT = ds.Tables[0].Rows[0]["ROW_FORMAT"].ToString();
                if (ds.Tables[0].Rows[0]["TABLE_ROWS"].ToString() != "")
                {
                    model.TABLE_ROWS = long.Parse(ds.Tables[0].Rows[0]["TABLE_ROWS"].ToString());
                }
                if (ds.Tables[0].Rows[0]["AVG_ROW_LENGTH"].ToString() != "")
                {
                    model.AVG_ROW_LENGTH = long.Parse(ds.Tables[0].Rows[0]["AVG_ROW_LENGTH"].ToString());
                }
                if (ds.Tables[0].Rows[0]["DATA_LENGTH"].ToString() != "")
                {
                    model.DATA_LENGTH = long.Parse(ds.Tables[0].Rows[0]["DATA_LENGTH"].ToString());
                }
                if (ds.Tables[0].Rows[0]["MAX_DATA_LENGTH"].ToString() != "")
                {
                    model.MAX_DATA_LENGTH = long.Parse(ds.Tables[0].Rows[0]["MAX_DATA_LENGTH"].ToString());
                }
                if (ds.Tables[0].Rows[0]["INDEX_LENGTH"].ToString() != "")
                {
                    model.INDEX_LENGTH = long.Parse(ds.Tables[0].Rows[0]["INDEX_LENGTH"].ToString());
                }
                if (ds.Tables[0].Rows[0]["DATA_FREE"].ToString() != "")
                {
                    model.DATA_FREE = long.Parse(ds.Tables[0].Rows[0]["DATA_FREE"].ToString());
                }
                if (ds.Tables[0].Rows[0]["AUTO_INCREMENT"].ToString() != "")
                {
                    model.AUTO_INCREMENT = long.Parse(ds.Tables[0].Rows[0]["AUTO_INCREMENT"].ToString());
                }
                if (ds.Tables[0].Rows[0]["CREATE_TIME"].ToString() != "")
                {
                    model.CREATE_TIME = DateTime.Parse(ds.Tables[0].Rows[0]["CREATE_TIME"].ToString());
                }
                if (ds.Tables[0].Rows[0]["UPDATE_TIME"].ToString() != "")
                {
                    model.UPDATE_TIME = DateTime.Parse(ds.Tables[0].Rows[0]["UPDATE_TIME"].ToString());
                }
                if (ds.Tables[0].Rows[0]["CHECK_TIME"].ToString() != "")
                {
                    model.CHECK_TIME = DateTime.Parse(ds.Tables[0].Rows[0]["CHECK_TIME"].ToString());
                }
                model.TABLE_COLLATION = ds.Tables[0].Rows[0]["TABLE_COLLATION"].ToString();
                if (ds.Tables[0].Rows[0]["CHECKSUM"].ToString() != "")
                {
                    model.CHECKSUM = long.Parse(ds.Tables[0].Rows[0]["CHECKSUM"].ToString());
                }
                model.CREATE_OPTIONS = ds.Tables[0].Rows[0]["CREATE_OPTIONS"].ToString();
                model.TABLE_COMMENT = ds.Tables[0].Rows[0]["TABLE_COMMENT"].ToString();
                //获取默认字符集
                DataSet ds1 = infoBaseUtilities.GetDataSet(string.Format("select CHARACTER_SET_NAME from information_schema.COLLATION_CHARACTER_SET_APPLICABILITY where COLLATION_NAME = '{0}'",model.TABLE_COLLATION));
                model.DEFAULTCHARSET = ds1.Tables[0].Rows[0][0].ToString();
                //获取该表的列对象
                model.columns = GetColumns(database, tablename);
                model.keyColumns = GetKeyColumns(database, tablename);

                return model;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 根据
        /// </summary>
        /// <param name="database">数据库中</param>
        /// <param name="tableName">表</param>
        /// <returns>获取该表的列信息</returns>
        public List<COLUMNS_Mdl> GetColumns(string database, string tablename)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select t.TABLE_CATALOG,t.TABLE_SCHEMA,t.TABLE_NAME,t.COLUMN_NAME,t.ORDINAL_POSITION,t.COLUMN_DEFAULT,t.IS_NULLABLE,t.DATA_TYPE,t.CHARACTER_MAXIMUM_LENGTH,t.CHARACTER_OCTET_LENGTH,t.NUMERIC_PRECISION,t.NUMERIC_SCALE,t.CHARACTER_SET_NAME,t.COLLATION_NAME,t.COLUMN_TYPE,t.COLUMN_KEY,t.EXTRA,t.PRIVILEGES,t.COLUMN_COMMENT from information_schema.COLUMNS t  ");
            strSql.Append(string.Format(" where  t.TABLE_SCHEMA='{0}' and t.TABLE_NAME='{1}' ", database, tablename));

            List<COLUMNS_Mdl> columnsLst = new List<COLUMNS_Mdl>();

            DataSet ds = infoBaseUtilities.GetDataSet(strSql.ToString());

            for (int i = 0 ; i < ds.Tables[0].Rows.Count ; i++)
            {
                COLUMNS_Mdl model = new COLUMNS_Mdl();

                model.TABLE_CATALOG = ds.Tables[0].Rows[i]["TABLE_CATALOG"].ToString();
                model.TABLE_SCHEMA = ds.Tables[0].Rows[i]["TABLE_SCHEMA"].ToString();
                model.TABLE_NAME = ds.Tables[0].Rows[i]["TABLE_NAME"].ToString();
                model.COLUMN_NAME = ds.Tables[0].Rows[i]["COLUMN_NAME"].ToString();
                if (ds.Tables[0].Rows[i]["ORDINAL_POSITION"].ToString() != "")
                {
                    model.ORDINAL_POSITION = long.Parse(ds.Tables[0].Rows[i]["ORDINAL_POSITION"].ToString());
                }
                model.COLUMN_DEFAULT = ds.Tables[0].Rows[i]["COLUMN_DEFAULT"].ToString();
                model.IS_NULLABLE = ds.Tables[0].Rows[i]["IS_NULLABLE"].ToString();
                model.DATA_TYPE = ds.Tables[0].Rows[i]["DATA_TYPE"].ToString();
                if (ds.Tables[0].Rows[i]["CHARACTER_MAXIMUM_LENGTH"].ToString() != "")
                {
                    model.CHARACTER_MAXIMUM_LENGTH = long.Parse(ds.Tables[0].Rows[i]["CHARACTER_MAXIMUM_LENGTH"].ToString());
                }
                if (ds.Tables[0].Rows[i]["CHARACTER_OCTET_LENGTH"].ToString() != "")
                {
                    model.CHARACTER_OCTET_LENGTH = long.Parse(ds.Tables[0].Rows[i]["CHARACTER_OCTET_LENGTH"].ToString());
                }
                if (ds.Tables[0].Rows[i]["NUMERIC_PRECISION"].ToString() != "")
                {
                    model.NUMERIC_PRECISION = long.Parse(ds.Tables[0].Rows[i]["NUMERIC_PRECISION"].ToString());
                }
                if (ds.Tables[0].Rows[i]["NUMERIC_SCALE"].ToString() != "")
                {
                    model.NUMERIC_SCALE = long.Parse(ds.Tables[0].Rows[i]["NUMERIC_SCALE"].ToString());
                }
                model.CHARACTER_SET_NAME = ds.Tables[0].Rows[i]["CHARACTER_SET_NAME"].ToString();
                model.COLLATION_NAME = ds.Tables[0].Rows[i]["COLLATION_NAME"].ToString();
                model.COLUMN_TYPE = ds.Tables[0].Rows[i]["COLUMN_TYPE"].ToString();
                model.COLUMN_KEY = ds.Tables[0].Rows[i]["COLUMN_KEY"].ToString();
                model.EXTRA = ds.Tables[0].Rows[i]["EXTRA"].ToString();
                model.PRIVILEGES = ds.Tables[0].Rows[i]["PRIVILEGES"].ToString();
                model.COLUMN_COMMENT = ds.Tables[0].Rows[i]["COLUMN_COMMENT"].ToString();
                model.COLID = i;
               // model.VIEW_NAME = ds.Tables[0].Rows[i]["VIEW_FIELD_NAME"].ToString();
                columnsLst.Add(model);
            }

            return columnsLst;
        }

        /// <summary>
        /// 获取mysql中的所有sql
        /// </summary>
        /// <returns></returns>
        public List<string> GetAllTableSchema()
        {
            List<string> schemaLst = new List<string>();

            string sqlStr = "select distinct table_schema from information_schema.tables";
            DataSet ds = infoBaseUtilities.GetDataSet(sqlStr);

            for (int i = 0 ; i < ds.Tables[0].Rows.Count ; i++)
            {
                schemaLst.Add(ds.Tables[0].Rows[i][0].ToString());
            }
            return schemaLst;
        }

        /// <summary>
        /// 获取schema下的所有表
        /// </summary>
        /// <param name="schema"></param>
        /// <returns></returns>
        public List<string> GetTablesWithSchema(string schema)
        {
            List<string> tableLst = new List<string>();

            string sqlStr = string.Format("select table_name from information_schema.tables where table_schema = '{0}' and table_type = 'BASE TABLE'", schema);
            DataSet ds = infoBaseUtilities.GetDataSet(sqlStr);

            for (int i = 0 ; i < ds.Tables[0].Rows.Count ; i++)
            {
                tableLst.Add(ds.Tables[0].Rows[i][0].ToString());
            }
            return tableLst;
        }

        /// <summary>
        /// 获取主键信息
        /// </summary>
        /// <param name="database"></param>
        /// <param name="tablename"></param>
        /// <returns></returns>
        public List<string> GetKeyColumns(string database, string tablename)
        {
            //SELECT
            //  t.TABLE_NAME,
            //  t.CONSTRAINT_TYPE,
            //  c.COLUMN_NAME,
            //  c.ORDINAL_POSITION
            //FROM
            //  INFORMATION_SCHEMA.TABLE_CONSTRAINTS AS t,
            //  INFORMATION_SCHEMA.KEY_COLUMN_USAGE AS c
            //WHERE
            //  t.TABLE_NAME = c.TABLE_NAME
            //  AND t.TABLE_SCHEMA = 'test'
            //  AND t.CONSTRAINT_TYPE = 'PRIMARY KEY';

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select c.COLUMN_NAME from  INFORMATION_SCHEMA.TABLE_CONSTRAINTS AS t, INFORMATION_SCHEMA.KEY_COLUMN_USAGE AS c ");
            strSql.Append(string.Format(" where   t.TABLE_NAME = c.TABLE_NAME and   t.TABLE_SCHEMA = '{0}' and  t.TABLE_NAME = '{1}' and  t.CONSTRAINT_TYPE = 'PRIMARY KEY'  and c.CONSTRAINT_NAME = 'PRIMARY' ", database, tablename));

            List<string> keyStr = new List<string>();
            DataSet ds = infoBaseUtilities.GetDataSet(strSql.ToString());

            for (int i = 0 ; i < ds.Tables[0].Rows.Count ; i++)
            {
                keyStr.Add(ds.Tables[0].Rows[i][0].ToString());
            }
            return keyStr;
        }

        public void ExecuteSqlScript(string sqlScript)
        {
            try
            {
                infoBaseUtilities.ExecuteSql(sqlScript);
            }
            catch (Exception e)
            {

                throw e;
            }

        }

        //public List<db02_catalog_group_Mdl> GetCatalogGroup(string whereClause)
        //{
        //    StringBuilder strSql = new StringBuilder();
        //    strSql.Append("select ID,GROUP_CODE,NAME,DATA_CODE,GROUP_TYPE,DESCRIPTION from db02.db02_catalog_group ");
        //    strSql.Append(" where  "+whereClause);

        //    DataSet ds = infoBaseUtilities.GetDataSet(strSql.ToString());
        //    List<db02_catalog_group_Mdl> groupLst = new List<db02_catalog_group_Mdl>();
        //    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
        //    {
        //        db02_catalog_group_Mdl model = new db02_catalog_group_Mdl();

        //        if (ds.Tables[0].Rows[i]["ID"].ToString() != "")
        //        {
        //            model.ID = decimal.Parse(ds.Tables[0].Rows[i]["ID"].ToString());
        //        }
        //        model.GROUP_CODE = ds.Tables[0].Rows[i]["GROUP_CODE"].ToString();
        //        model.NAME = ds.Tables[0].Rows[i]["NAME"].ToString();
        //        model.DATA_CODE = ds.Tables[0].Rows[i]["DATA_CODE"].ToString();
        //        model.GROUP_TYPE = ds.Tables[0].Rows[i]["GROUP_TYPE"].ToString();
        //        model.DESCRIPTION = ds.Tables[0].Rows[i]["DESCRIPTION"].ToString();

        //        groupLst.Add(model);
        //    }
        //    return groupLst;
        //}


        /// <summary>
        /// 获取元数据子节点
        /// </summary>
        /// <param name="whereClause"></param>
        /// <returns></returns>
        //public List<db02_catalog_group_Mdl> GetChild(string groupCode)
        //{
        //    List<db02_catalog_group_Mdl> groupLst = new List<db02_catalog_group_Mdl>();

        //    StringBuilder strSql = new StringBuilder();
        //    strSql.Append("select CHILD_CODE from db02.db02_catalog_index ");
        //    strSql.AppendFormat(" where  GROUP_CODE = '{0}'",groupCode );

        //    DataSet ds = infoBaseUtilities.GetDataSet(strSql.ToString());
        //    for (int i = 0 ; i < ds.Tables[0].Rows.Count ; i++)
        //    {
        //        string chileCode = ds.Tables[0].Rows[i]["CHILD_CODE"].ToString();
        //        groupLst.AddRange(GetCatalogGroup(string.Format("GROUP_CODE = '{0}'", chileCode)));
        //    }
        //    return groupLst;
        //}

      
        /// <summary>
        /// 定义一种元数据类型，
        /// </summary>
        /// <param name="child"></param>
        /// <param name="parent">该元数据的父类</param>
        //public void AddMetadata(db02_catalog_group_Mdl child,db02_catalog_group_Mdl parent)
        //{
        //    //在db02.db02_catalog_group中添加记录
        //    AddCatalogGroup(child);
        //    //将类型映射添加到db02_catalog_index
        //    int id = GetMaxID("ID", "db02.db02_catalog_index");
        //    string str =string.Format("INSERT INTO  db02.db02_catalog_index  VALUES ('{0}', '{1}', null, '{2}', null, null, null, null)",id,parent.GROUP_CODE,child.GROUP_CODE) ;
        //    infoBaseUtilities.ExecuteSql(str);
        //}

        /// <summary>
        /// 获取某个表的某个字段最大值，表前面需要加方案名前缀
        /// </summary>
        /// <param name="FieldName"></param>
        /// <param name="TableName"></param>
        /// <returns></returns>
        public int GetMaxID(string FieldName,string TableName)
        {
            string strsql = String.Format("select max({0})+1 from {1}", FieldName, TableName);
            DataSet ds = infoBaseUtilities.GetDataSet(strsql);

            return Convert.ToInt32(ds.Tables[0].Rows[0][0].ToString());
        }

        /// <summary>
        /// 添加一个新的数据类型
        /// </summary>
        /// <param name="data"></param>
        //public void AddCatalogGroup(db02_catalog_group_Mdl model)
        //{
        //    int id = GetMaxID("ID", "db02.db02_catalog_group");
        //    model.ID = id;
        //    model.GROUP_CODE = string.Format("DB02G-{0}", id);

        //    StringBuilder strSql = new StringBuilder();
        //    strSql.Append("insert into db02.db02_catalog_group(");
        //    strSql.Append("ID,GROUP_CODE,NAME,DATA_CODE,GROUP_TYPE,DESCRIPTION)");
        //    strSql.Append(" values (");
        //    strSql.Append("@ID,@GROUP_CODE,@NAME,@DATA_CODE,@GROUP_TYPE,@DESCRIPTION)");
        //    MySqlParameter[] parameters = {
        //            new MySqlParameter("@ID", MySqlDbType.Decimal,10),
        //            new MySqlParameter("@GROUP_CODE", MySqlDbType.Text),
        //            new MySqlParameter("@NAME", MySqlDbType.Text),
        //            new MySqlParameter("@DATA_CODE", MySqlDbType.Text),
        //            new MySqlParameter("@GROUP_TYPE", MySqlDbType.Text),
        //            new MySqlParameter("@DESCRIPTION", MySqlDbType.Text)};
        //    parameters[0].Value = model.ID;
        //    parameters[1].Value = model.GROUP_CODE;
        //    parameters[2].Value = model.NAME;
        //    parameters[3].Value = model.DATA_CODE;
        //    parameters[4].Value = model.GROUP_TYPE;
        //    parameters[5].Value = model.DESCRIPTION;

        //    infoBaseUtilities.ExecuteSql(strSql.ToString(), parameters);
        //}

        

    }
}
