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
    public class metadatacatalognode_Dal
    {
        private IDbBaseUtilities baseUtilities;

        private string tablecode;

        public metadatacatalognode_Dal(string connectionStr)
        {
            baseUtilities = Constant.IdbServerUtilities.GetSubDbUtilByCon(connectionStr);
            tablecode = GetTableCode();

        }

        public metadatacatalognode_Dal(IDbBaseUtilities baseutilities)
        {
            baseUtilities = baseutilities;
            tablecode = GetTableCode();
        }
        /// <summary>
        /// 增加一条数据
        /// </summary>
        public metadatacatalognode_Mdl Add(metadatacatalognode_Mdl model)
        {
            //TableLocker dblock = new TableLocker(baseUtilities);
            Constant.IdbOperating.LockTable("metadatacatalognode",EnumDBType.MIDB);
            StringBuilder strSql = new StringBuilder();
            model.ID = baseUtilities.GetMaxID("ID", "metadatacatalognode");
            //定义编码
            model.GROUP_CODE = tablecode + "-" + model.ID.ToString();
            strSql.Append("insert into metadatacatalognode(");
            strSql.Append("ID,GROUP_CODE,NAME,DATA_CODE,GROUP_TYPE,DESCRIPTION)");
            strSql.Append(" values (");
            strSql.Append(string.Format("{0},'{1}','{2}','{3}','{4}','{5}')", model.ID, model.GROUP_CODE, model.NAME,
                model.DATA_CODE, model.GROUP_TYPE, model.DESCRIPTION));
     //       strSql.Append("@ID,@GROUP_CODE,@NAME,@DATA_CODE,@GROUP_TYPE,@DESCRIPTION)");
     //       MySqlParameter[] parameters = {
					//new MySqlParameter("@ID", MySqlDbType.Decimal,10),
					//new MySqlParameter("@GROUP_CODE", MySqlDbType.Text),
					//new MySqlParameter("@NAME", MySqlDbType.Text),
					//new MySqlParameter("@DATA_CODE", MySqlDbType.Text),
					//new MySqlParameter("@GROUP_TYPE", MySqlDbType.Text),
					//new MySqlParameter("@DESCRIPTION", MySqlDbType.Text)};
     //       parameters[0].Value = model.ID;
     //       parameters[1].Value = model.GROUP_CODE;
     //       parameters[2].Value = model.NAME;
     //       parameters[3].Value = model.DATA_CODE;
     //       parameters[4].Value = model.GROUP_TYPE;
     //       parameters[5].Value = model.DESCRIPTION;

            baseUtilities.ExecuteSql(strSql.ToString());
            Constant.IdbOperating.UnlockTable("metadatacatalognode",EnumDBType.MIDB);
            return model;
        }
        /// <summary>
        /// 更新一条数据
        /// </summary>
        public void Update(metadatacatalognode_Mdl model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(
                string.Format(
                    "update metadatacatalognode set GROUP_CODE='{0}',NAME='{1}',DATA_CODE='{2}',GROUP_TYPE='{3}',DESCRIPTION='{4}'",
                     model.GROUP_CODE, model.NAME, model.DATA_CODE, model.GROUP_TYPE, model.DESCRIPTION));
     //       strSql.Append("update metadatacatalognode set ");
     //       strSql.Append("ID=@ID,");
     //       strSql.Append("GROUP_CODE=@GROUP_CODE,");
     //       strSql.Append("NAME=@NAME,");
     //       strSql.Append("DATA_CODE=@DATA_CODE,");
     //       strSql.Append("GROUP_TYPE=@GROUP_TYPE,");
     //       strSql.Append("DESCRIPTION=@DESCRIPTION");
     //       strSql.Append(" where ID=@ID and GROUP_CODE=@GROUP_CODE and NAME=@NAME and DATA_CODE=@DATA_CODE and GROUP_TYPE=@GROUP_TYPE and DESCRIPTION=@DESCRIPTION ");
     //       MySqlParameter[] parameters = {
					//new MySqlParameter("@ID", MySqlDbType.Decimal,10),
					//new MySqlParameter("@GROUP_CODE", MySqlDbType.Text),
					//new MySqlParameter("@NAME", MySqlDbType.Text),
					//new MySqlParameter("@DATA_CODE", MySqlDbType.Text),
					//new MySqlParameter("@GROUP_TYPE", MySqlDbType.Text),
					//new MySqlParameter("@DESCRIPTION", MySqlDbType.Text)};
     //       parameters[0].Value = model.ID;
     //       parameters[1].Value = model.GROUP_CODE;
     //       parameters[2].Value = model.NAME;
     //       parameters[3].Value = model.DATA_CODE;
     //       parameters[4].Value = model.GROUP_TYPE;
     //       parameters[5].Value = model.DESCRIPTION;

            baseUtilities.ExecuteSql(strSql.ToString());
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public void Delete(string whereCondition)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from metadatacatalognode ");
            strSql.Append(" where  " + whereCondition);


            baseUtilities.ExecuteSql(strSql.ToString());
        }

        /// <summary>
        /// 获得数据列表
        /// </summary>
        public DataSet GetList(string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select ID,GROUP_CODE,NAME,DATA_CODE,GROUP_TYPE,DESCRIPTION ");
            strSql.Append(" FROM metadatacatalognode ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            return baseUtilities.GetDataSet(strSql.ToString());
        }

        public List<metadatacatalognode_Mdl> GetCatalogGroup(string whereClause)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select ID,GROUP_CODE,NAME,DATA_CODE,GROUP_TYPE,DESCRIPTION,ORDER_INDEX from metadatacatalognode ");
            strSql.Append(" where  " + whereClause);
            DataSet ds = baseUtilities.GetDataSet(strSql.ToString());
            List<metadatacatalognode_Mdl> groupLst = new List<metadatacatalognode_Mdl>();
            for (int i = 0 ; i < ds.Tables[0].Rows.Count ; i++)
            {
                metadatacatalognode_Mdl model = new metadatacatalognode_Mdl();

                if (ds.Tables[0].Rows[i]["ID"].ToString() != "")
                {
                    model.ID = decimal.Parse(ds.Tables[0].Rows[i]["ID"].ToString());
                }
                model.ORDER_INDEX = 0;
                try
                {
                    model.ORDER_INDEX = decimal.Parse(ds.Tables[0].Rows[i]["ORDER_INDEX"].ToString());
                }
                catch { }
                model.GROUP_CODE = ds.Tables[0].Rows[i]["GROUP_CODE"].ToString();
                model.NAME = ds.Tables[0].Rows[i]["NAME"].ToString();
                model.DATA_CODE = ds.Tables[0].Rows[i]["DATA_CODE"].ToString();
                model.GROUP_TYPE = ds.Tables[0].Rows[i]["GROUP_TYPE"].ToString();
                model.DESCRIPTION = ds.Tables[0].Rows[i]["DESCRIPTION"].ToString();
                model.IS_DATASET = model.GROUP_TYPE== EnumDataKind.System_DataSet.ToString()?true:false;
                groupLst.Add(model);
            }
            return groupLst;
        }

        public string  GetTableCode()
        {
            string strSql = "select QRST_CODE from tablecode where TABLE_NAME = 'metadatacatalognode'";
            DataSet ds =  baseUtilities.GetDataSet(strSql);
            return ds.Tables[0].Rows[0][0].ToString();
        }

    
    }
}
