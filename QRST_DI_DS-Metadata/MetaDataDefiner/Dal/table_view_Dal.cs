using System;
using System.Data;
using System.Text;
using QRST_DI_DS_Metadata.MetaDataDefiner.Mdl;
using QRST_DI_Resources;
using QRST_DI_SS_Basis;
using QRST_DI_SS_Basis.MetaData;
using QRST_DI_SS_DBInterfaces.IDBEngine;

namespace QRST_DI_DS_Metadata.MetaDataDefiner.Dal
{
    public class table_view_Dal
    {

        private IDbBaseUtilities baseUtilities;
        public table_view_Dal(string connectionStr)
        {
            //baseUtilities = new MySqlBaseUtilities(connectionStr);
            baseUtilities = Constant.IdbServerUtilities.GetSubDbUtilByCon(connectionStr);
        }

        public table_view_Dal(IDbBaseUtilities baseutilities)
        {
            baseUtilities = baseutilities;
        }


     



        /// <summary>
        /// 增加一条数据
        /// </summary>
        public void Add(table_view_Mdl model)
        {
            //TableLocker dblock = new TableLocker(baseUtilities);
            Constant.IdbOperating.LockTable("table_view",EnumDBType.MIDB);
            StringBuilder strSql = new StringBuilder();
            model.ID = baseUtilities.GetMaxID("ID", "table_view");
            strSql.Append("insert into table_view(");
            strSql.Append("ID,TABLE_NAME,VIEW_NAME,FIELD_NAME,VIEW_FIELD_NAME,MEMO)");
            strSql.Append(" values (");
            strSql.Append(String.Format("{0},'{1}','{2}','{3}','{4}','{5}')", model.ID, model.TABLE_NAME,
                model.VIEW_NAME, model.FIELD_NAME, model.VIEW_FIELD_NAME, model.MEMO));
     //       strSql.Append("@ID,@TABLE_NAME,@VIEW_NAME,@FIELD_NAME,@VIEW_FIELD_NAME,@MEMO)");
     //       MySqlParameter[] parameters = {
					//new MySqlParameter("@ID", MySqlDbType.Int32,7),
					//new MySqlParameter("@TABLE_NAME", MySqlDbType.VarChar,30),
					//new MySqlParameter("@VIEW_NAME", MySqlDbType.VarChar,30),
					//new MySqlParameter("@FIELD_NAME", MySqlDbType.VarChar,30),
					//new MySqlParameter("@VIEW_FIELD_NAME", MySqlDbType.VarChar,30),
					//new MySqlParameter("@MEMO", MySqlDbType.VarChar,100)};
     //       parameters[0].Value = model.ID;
     //       parameters[1].Value = model.TABLE_NAME;
     //       parameters[2].Value = model.VIEW_NAME;
     //       parameters[3].Value = model.FIELD_NAME;
     //       parameters[4].Value = model.VIEW_FIELD_NAME;
     //       parameters[5].Value = model.MEMO;

            baseUtilities.ExecuteSql(strSql.ToString());
            Constant.IdbOperating.UnlockTable("table_view",EnumDBType.MIDB);
        }

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public string GetAddScript(table_view_Mdl model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into table_view(");
            strSql.Append("ID,TABLE_NAME,VIEW_NAME,FIELD_NAME,VIEW_FIELD_NAME,MEMO)");
            strSql.Append(" values (");
            strSql.AppendFormat("{0},'{1}','{2}','{3}','{4}','{5}')",model.ID,model.TABLE_NAME,model.VIEW_NAME,model.FIELD_NAME,model.VIEW_FIELD_NAME,model.MEMO);

            return strSql.ToString();
        }

        public string GetDeleteScript(string whereCondition)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from table_view ");
            strSql.AppendFormat(" where {0}",whereCondition);
            return strSql.ToString();
        }
        /// <summary>
        /// 更新一条数据
        /// </summary>
        public void Update(table_view_Mdl model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(
                string.Format(
                    "update table_view set TABLE_NAME='{0}',VIEW_NAME='{1}',FIELD_NAME='{2}',VIEW_FIELD_NAME='{3}',MEMO='{4}' where ID={5}",
                    model.TABLE_NAME, model.VIEW_NAME, model.FIELD_NAME, model.VIEW_FIELD_NAME, model.MEMO, model.ID));
     //       strSql.Append("update table_view set ");
     //       strSql.Append("TABLE_NAME=@TABLE_NAME,");
     //       strSql.Append("VIEW_NAME=@VIEW_NAME,");
     //       strSql.Append("FIELD_NAME=@FIELD_NAME,");
     //       strSql.Append("VIEW_FIELD_NAME=@VIEW_FIELD_NAME,");
     //       strSql.Append("MEMO=@MEMO");
     //       strSql.Append(" where ID=@ID ");
     //       MySqlParameter[] parameters = {
					//new MySqlParameter("@ID", MySqlDbType.Int32,7),
					//new MySqlParameter("@TABLE_NAME", MySqlDbType.VarChar,30),
					//new MySqlParameter("@VIEW_NAME", MySqlDbType.VarChar,30),
					//new MySqlParameter("@FIELD_NAME", MySqlDbType.VarChar,30),
					//new MySqlParameter("@VIEW_FIELD_NAME", MySqlDbType.VarChar,30),
					//new MySqlParameter("@MEMO", MySqlDbType.VarChar,100)};
     //       parameters[0].Value = model.ID;
     //       parameters[1].Value = model.TABLE_NAME;
     //       parameters[2].Value = model.VIEW_NAME;
     //       parameters[3].Value = model.FIELD_NAME;
     //       parameters[4].Value = model.VIEW_FIELD_NAME;
     //       parameters[5].Value = model.MEMO;

            baseUtilities.ExecuteSql(strSql.ToString());
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public void Delete(int ID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append(String.Format("delete from table_view where ID={0}", ID));
     //       strSql.Append("delete from table_view ");
     //       strSql.Append(" where ID=@ID ");
     //       MySqlParameter[] parameters = {
					//new MySqlParameter("@ID", MySqlDbType.Int32)};
     //       parameters[0].Value = ID;

            baseUtilities.ExecuteSql(strSql.ToString());
        }


        public void Delete(string whereCondition)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from table_view ");
            if (!string.IsNullOrEmpty(whereCondition))
            {
                strSql.Append(" where  " + whereCondition);
            }
            baseUtilities.ExecuteSql(strSql.ToString());
        }

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        //public table_view_Mdl   GetModel(int ID)
        //{

        //    StringBuilder strSql = new StringBuilder();
        //    strSql.Append("select ID,TABLE_NAME,VIEW_NAME,FIELD_NAME,VIEW_FIELD_NAME,MEMO from table_view ");
        //    strSql.Append(" where ID=@ID ");
        //    MySqlParameter[] parameters = {
        //            new MySqlParameter("@ID", MySqlDbType.Int32)};
        //    parameters[0].Value = ID;

        //    table_view_Mdl model = new table_view_Mdl();
        //    DataSet ds = baseUtilities.GetDataSet(strSql.ToString(), parameters);
        //    if (ds.Tables[0].Rows.Count > 0)
        //    {
        //        if (ds.Tables[0].Rows[0]["ID"].ToString() != "")
        //        {
        //            model.ID = int.Parse(ds.Tables[0].Rows[0]["ID"].ToString());
        //        }
        //        model.TABLE_NAME = ds.Tables[0].Rows[0]["TABLE_NAME"].ToString();
        //        model.VIEW_NAME = ds.Tables[0].Rows[0]["VIEW_NAME"].ToString();
        //        model.FIELD_NAME = ds.Tables[0].Rows[0]["FIELD_NAME"].ToString();
        //        model.VIEW_FIELD_NAME = ds.Tables[0].Rows[0]["VIEW_FIELD_NAME"].ToString();
        //        model.MEMO = ds.Tables[0].Rows[0]["MEMO"].ToString();
        //        return model;
        //    }
        //    else
        //    {
        //        return null;
        //    }
        //}

        /// <summary>
        /// 获得数据列表
        /// </summary>
        public DataSet GetList(string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select ID,TABLE_NAME,VIEW_NAME,FIELD_NAME,VIEW_FIELD_NAME,MEMO ");
            strSql.Append(" FROM table_view ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            return baseUtilities.GetDataSet(strSql.ToString());
        }
        /// <summary>
        /// 获取
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public string[] GetFields(string tableName)
        {
            string[] fields=new string[]{};

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select VIEW_FIELD_NAME ");
            strSql.Append("FROM table_view " );
            strSql.Append(string.Format("where TABLE_NAME = '{0}' and VIEW_FIELD_NAME != ''", tableName));

            DataSet ds=baseUtilities.GetDataSet(strSql.ToString());
            if (ds != null && ds.Tables.Count > 0)
            {
                fields = new string[ds.Tables[0].Rows.Count];
                for (int i = 0; i < fields.Length; i++)
                {
                    fields[i] = ds.Tables[0].Rows[i]["VIEW_FIELD_NAME"].ToString();
                }
            }
            else
            {
                throw new Exception("查询当前数据类型的可选字段时出错！");
            }
            return fields;
        }

        /// <summary>
        /// 根据英文字段获取视图字段
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public string GetField(string tableName,string fieldname)
        {
            string field ="";

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select VIEW_FIELD_NAME ");
            strSql.Append("FROM table_view ");
            strSql.Append(String.Format("where TABLE_NAME = '{0}' and VIEW_FIELD_NAME != '' and FIELD_NAME = '{1}'", tableName, fieldname));

            DataSet ds = baseUtilities.GetDataSet(strSql.ToString());
            if (ds != null && ds.Tables.Count > 0)
            {
                 field = ds.Tables[0].Rows[0]["VIEW_FIELD_NAME"].ToString();
            }
            else
            {
                MyConsole.WriteLine("查询当前数据类型的可选字段时出错！");
            }
            return field;
        }

        /// <summary>
        /// 获取基于特定表的视图信息
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public DataTable GetTableViewLstByTableName(string tableName)
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendFormat("select FIELD_NAME as COLUMN_NAME,VIEW_FIELD_NAME as VIEWNAME,MEMO  FROM table_view where TABLE_NAME = '{0}' and   VIEW_FIELD_NAME!='' and VIEW_FIELD_NAME is not null", tableName);

            DataTable dt = baseUtilities.GetDataSet(sb.ToString()).Tables[0];
            dt.Columns.Add(new DataColumn() { ColumnName = "keyWord",DataType = Type.GetType("System.Boolean") });

            for (int i=0;i<dt.Rows.Count;i++)
            {
                if (dt.Rows[i]["MEMO"].ToString().Contains("keyWord"))
                {
                    dt.Rows[i]["keyWord"] = true;
                }
                else
                {
                    dt.Rows[i]["keyWord"] = false;
                }
            }

            return dt;
        }

        /// <summary>
        /// 根据视图名获取视图信息
        /// </summary>
        /// <param name="viewName"></param>
        /// <returns></returns>
        public DataTable GetTableViewLstByViewName(string viewName)
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendFormat("select FIELD_NAME as COLUMN_NAME,VIEW_FIELD_NAME as VIEWNAME,MEMO FROM table_view where VIEW_NAME = '{0}' and   VIEW_FIELD_NAME!='' and VIEW_FIELD_NAME is not null", viewName);

            DataTable dt = baseUtilities.GetDataSet(sb.ToString()).Tables[0];
            dt.Columns.Add(new DataColumn() { ColumnName = "keyWord", DataType = Type.GetType("System.Boolean") });

            for (int i = 0 ; i < dt.Rows.Count ; i++)
            {
                if (dt.Rows[i]["MEMO"].ToString().Contains("keyWord"))
                {
                    dt.Rows[i]["keyWord"] = true;
                }
                else
                {
                    dt.Rows[i]["keyWord"] = false;
                }
            }

            return dt;
        }
        

    }
}
