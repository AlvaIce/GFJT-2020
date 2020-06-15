using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using QRST_DI_DS_Metadata.MetaDataDefiner.Mdl;
using QRST_DI_Resources;
using QRST_DI_SS_Basis;
using QRST_DI_SS_Basis.MetaData;
using QRST_DI_SS_DBInterfaces.IDBEngine;

namespace QRST_DI_DS_Metadata.MetaDataDefiner.Dal
{
    //public struct Constant
    //{
    //    static Constant()
    //    {
            
    //    }
    //}
    public class tablecode_Dal
    {
        private IDbBaseUtilities baseUtilities;

        public tablecode_Dal(string connectionStr)
        {
            baseUtilities = Constant.IdbServerUtilities.GetSubDbUtilByCon(connectionStr);
        }

        public tablecode_Dal(IDbBaseUtilities baseutilities)
        {
            baseUtilities = baseutilities;
        }

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public void Add(tablecode_Mdl model)
        {
            //int maxid = baseUtilities.GetMaxID("ID", "tablecode");
            // model.QRST_CODE = 
            //TableLocker dblock = new TableLocker(baseUtilities);
            Constant.IdbOperating.LockTable("tablecode",EnumDBType.MIDB);
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into tablecode(");
            strSql.Append("QRST_CODE,TABLE_NAME,DESCRIPTION)");
            strSql.Append(" values (");
            strSql.Append(string.Format("'{0}','{1}','{2}')", model.QRST_CODE, model.TABLE_NAME, model.DESCRIPTION));
     //       strSql.Append("@QRST_CODE,@TABLE_NAME,@DESCRIPTION)");
     //       MySqlParameter[] parameters = {
					//new MySqlParameter("@QRST_CODE", MySqlDbType.Text),
					//new MySqlParameter("@TABLE_NAME", MySqlDbType.Text),
					//new MySqlParameter("@DESCRIPTION", MySqlDbType.Text)};
     //       parameters[0].Value = model.QRST_CODE;
     //       parameters[1].Value = model.TABLE_NAME;
     //       parameters[2].Value = model.DESCRIPTION;

            baseUtilities.ExecuteSql(strSql.ToString());
            Constant.IdbOperating.UnlockTable("tablecode",EnumDBType.MIDB);
        }
        /// <summary>
        /// 更新一条数据
        /// </summary>
        public void Update(tablecode_Mdl  model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(string.Format(
                "update tablecode set QRST_CODE='{0}',TABLE_NAME='{1}',DESCRIPTION='{2}' where ID={3}", model.QRST_CODE,
                model.TABLE_NAME, model.DESCRIPTION, model.ID));
     //       strSql.Append("update tablecode set ");
     //       strSql.Append("QRST_CODE=@QRST_CODE,");
     //       strSql.Append("TABLE_NAME=@TABLE_NAME,");
     //       strSql.Append("DESCRIPTION=@DESCRIPTION");
     //       strSql.Append(" where ID=@ID ");
     //       MySqlParameter[] parameters = {
					//new MySqlParameter("@ID", MySqlDbType.Decimal,10),
					//new MySqlParameter("@QRST_CODE", MySqlDbType.Text),
					//new MySqlParameter("@TABLE_NAME", MySqlDbType.Text),
					//new MySqlParameter("@DESCRIPTION", MySqlDbType.Text)};
     //       parameters[0].Value = model.ID;
     //       parameters[1].Value = model.QRST_CODE;
     //       parameters[2].Value = model.TABLE_NAME;
     //       parameters[3].Value = model.DESCRIPTION;

            baseUtilities.ExecuteSql(strSql.ToString());
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public void Delete(long ID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append(string.Format("delete from tablecode where ID={0}",ID));
     //       strSql.Append("delete from tablecode ");
     //       strSql.Append(" where ID=@ID ");
     //       MySqlParameter[] parameters = {
					//new MySqlParameter("@ID", MySqlDbType.Decimal)};
     //       parameters[0].Value = ID;

            baseUtilities.ExecuteSql(strSql.ToString());
        }

        /// <summary>
        /// 条件删除记录
        /// </summary>
        /// <returns></returns>
        public void Delete(string whereCondition)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from tablecode ");
            if (!string.IsNullOrEmpty(whereCondition))
            { 
                strSql.Append(" where  "+whereCondition);
            }
            baseUtilities.ExecuteSql(strSql.ToString());
        }

        public int GetMaxID()
        {
            return baseUtilities.GetMaxID("ID", "tablecode");
        }

        /// <summary>
        /// 根据表编码获取表名
        /// </summary>
        /// <param name="tableCode"></param>
        /// <returns></returns>
        public  string  GetTableName(string tableCode)
        {
            string con = baseUtilities.GetDbConnection();
            DataSet ds = baseUtilities.GetDataSet(string.Format("select TABLE_NAME from tablecode where QRST_CODE = '{0}' ",tableCode));
            if (ds.Tables[0].Rows.Count == 0)
            {
                throw  new Exception("没有找到对应的表！");
            }
            else
            {
                return ds.Tables[0].Rows[0][0].ToString();
            }
        }

        /// <summary>
        /// 根据表名获取数据编码
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public string GetTableCode(string tableName)
        {
            DataSet ds = baseUtilities.GetDataSet(string.Format("select QRST_CODE from tablecode where TABLE_NAME  = '{0}' ", tableName));
            if (ds.Tables[0].Rows.Count == 0)
            {
                throw new Exception("没有找到对应的编码！");
            }
            else
            {
                return ds.Tables[0].Rows[0][0].ToString();
            }
        }

        /// <summary>
        /// 根据数据ID获取数据QRST_CODE编码
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="dataid"></param>
        /// <returns></returns>
        public string GetDataQRSTCode(string tableName,int dataid)
        {
            return string.Format("{0}-{1}-{2}", QRST_DI_Resources.Constant.INDUSTRYCODE, GetTableCode(tableName), dataid);
        }

        /// <summary>
        /// 判断在数据库中是否存在该表
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public bool ExistTable(string tableName)
        {
            int count = baseUtilities.GetRecordCount(string.Format("select QRST_CODE from tablecode where TABLE_NAME  = '{0}' ", tableName));
               if (count == 0)
               {
                   return false;
               }
            else
               {
                   return true;
               }
        }

        /// <summary>
        /// 获取该库中所有的数据格式模板以及表名
        /// </summary>
        /// <returns>返回一个数据字典，键值是模板表名，值是数据的格式(矢量、栅格、表格、文档)</returns>
        public Dictionary<string ,string > GetTableMoulds()
        {
            Dictionary<string, string> dicDataFormat = new Dictionary<string, string>();
            DataSet ds = baseUtilities.GetDataSet("select TABLE_NAME,DESCRIPTION from tablecode where TABLE_NAME like 'mould_%' ");

            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                dicDataFormat.Add(ds.Tables[0].Rows[i][1].ToString(),ds.Tables[0].Rows[i][0].ToString());
            }
            return dicDataFormat;
        }

        
    }
}
