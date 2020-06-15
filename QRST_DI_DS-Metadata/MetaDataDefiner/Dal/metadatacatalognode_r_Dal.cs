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

    public class metadatacatalognode_r_Dal
    {
        private IDbBaseUtilities baseUtilities;

        private string tableCode;

        public metadatacatalognode_r_Dal(string  connectionStr)
        {
            //baseUtilities = new MySqlBaseUtilities(connectionStr);
            baseUtilities = Constant.IdbServerUtilities.GetSubDbUtilByCon(connectionStr);
            tableCode = GetTableCode();
        }

        public metadatacatalognode_r_Dal(IDbBaseUtilities baseutilities)
        {
            baseUtilities = baseutilities;
            tableCode = GetTableCode();
        }

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public void Add(metadatacatalognode_r_Mdl model)
        {
            //TableLocker dblock = new TableLocker(baseUtilities);
            Constant.IdbOperating.LockTable("metadatacatalognode_r",EnumDBType.MIDB);
            StringBuilder strSql = new StringBuilder();
            model.ID = baseUtilities.GetMaxID("ID", "metadatacatalognode_r");
            model.QRST_CODE = tableCode + "-" + model.ID.ToString();
            strSql.Append("insert into metadatacatalognode_r(");
            strSql.Append("ID,GROUP_CODE,NAME,CHILD_CODE,USER_ID,DATETIME,DESCRIPTION,QRST_CODE)");
            strSql.Append(" values (");
            strSql.Append(String.Format("{0},'{1}','{2}','{3}',{4},'{5}','{6}','{7}')", model.ID, model.GROUP_CODE,
                model.NAME, model.CHILD_CODE, model.USER_ID, model.DATETIME, model.DESCRIPTION, model.QRST_CODE));
     //       strSql.Append("@ID,@GROUP_CODE,@NAME,@CHILD_CODE,@USER_ID,@DATETIME,@DESCRIPTION,@QRST_CODE)");
     //       MySqlParameter[] parameters = {
     //new MySqlParameter("@ID", MySqlDbType.Decimal,10),
     //new MySqlParameter("@GROUP_CODE", MySqlDbType.VarChar,100),
     //new MySqlParameter("@NAME", MySqlDbType.VarChar,100),
     //new MySqlParameter("@CHILD_CODE", MySqlDbType.VarChar,100),
     //new MySqlParameter("@USER_ID", MySqlDbType.Decimal,10),
     //new MySqlParameter("@DATETIME", MySqlDbType.DateTime),
     //new MySqlParameter("@DESCRIPTION", MySqlDbType.Text),
     //new MySqlParameter("@QRST_CODE", MySqlDbType.Text)};
     //       parameters[0].Value = model.ID;
     //       parameters[1].Value = model.GROUP_CODE;
     //       parameters[2].Value = model.NAME;
     //       parameters[3].Value = model.CHILD_CODE;
     //       parameters[4].Value = model.USER_ID;
     //       parameters[5].Value = model.DATETIME;
     //       parameters[6].Value = model.DESCRIPTION;
     //       parameters[7].Value = model.QRST_CODE;
            baseUtilities.ExecuteSql(strSql.ToString());
            Constant.IdbOperating.UnlockTable("metadatacatalognode_r",EnumDBType.MIDB);
        }
        /// <summary>
        /// 更新一条数据
        /// </summary>
        public void Update(metadatacatalognode_r_Mdl model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(
                string.Format(
                    "update metadatacatalognode_r set ID={0},GROUP_CODE='{1}',NAME='{2}',CHILD_CODE='{3}',USER_ID={4},DATETIME='{5}',DESCRIPTION='{6}',QRST_CODE='{7}' where ID={8}",
                    model.ID, model.GROUP_CODE, model.NAME, model.CHILD_CODE, model.USER_ID, model.DATETIME,
                    model.DESCRIPTION, model.QRST_CODE, model.ID));
            //       strSql.Append("update metadatacatalognode_r set ");
            //       strSql.Append("ID=@ID,");
            //       strSql.Append("GROUP_CODE=@GROUP_CODE,");
            //       strSql.Append("NAME=@NAME,");
            //       strSql.Append("CHILD_CODE=@CHILD_CODE,");
            //       strSql.Append("USER_ID=@USER_ID,");
            //       strSql.Append("DATETIME=@DATETIME,");
            //       strSql.Append("DESCRIPTION=@DESCRIPTION,");
            //       strSql.Append("QRST_CODE=@QRST_CODE");
            //       strSql.Append(
            //           " where ID=@ID and GROUP_CODE=@GROUP_CODE and NAME=@NAME and CHILD_CODE=@CHILD_CODE and USER_ID=@USER_ID and DATETIME=@DATETIME 
            //and DESCRIPTION=@DESCRIPTION and QRST_CODE=@QRST_CODE ");
     //       MySqlParameter[] parameters = {
					//new MySqlParameter("@ID", MySqlDbType.Decimal,10),
					//new MySqlParameter("@GROUP_CODE", MySqlDbType.VarChar,100),
					//new MySqlParameter("@NAME", MySqlDbType.VarChar,100),
					//new MySqlParameter("@CHILD_CODE", MySqlDbType.VarChar,100),
					//new MySqlParameter("@USER_ID", MySqlDbType.Decimal,10),
					//new MySqlParameter("@DATETIME", MySqlDbType.DateTime),
					//new MySqlParameter("@DESCRIPTION", MySqlDbType.Text),
					//new MySqlParameter("@QRST_CODE", MySqlDbType.Text)};
     //       parameters[0].Value = model.ID;
     //       parameters[1].Value = model.GROUP_CODE;
     //       parameters[2].Value = model.NAME;
     //       parameters[3].Value = model.CHILD_CODE;
     //       parameters[4].Value = model.USER_ID;
     //       parameters[5].Value = model.DATETIME;
     //       parameters[6].Value = model.DESCRIPTION;
     //       parameters[7].Value = model.QRST_CODE;

            baseUtilities.ExecuteSql(strSql.ToString());
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public void Delete(string whereCondition)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from metadatacatalognode_r ");
            strSql.Append(" where  "+whereCondition);

            baseUtilities.ExecuteSql(strSql.ToString());
        }

        public string GetTableCode()
        {
            string strSql = "select QRST_CODE from tablecode where TABLE_NAME = 'metadatacatalognode_r'";
            DataSet ds = baseUtilities.GetDataSet(strSql);
            return ds.Tables[0].Rows[0][0].ToString();
        }

        /// <summary>
        /// 根据组编码获取该组的所有子类型的group_code
        /// </summary>
        /// <param name="groupCode"></param>
        /// <returns></returns>
        public List<string> GetGroupChild(string groupCode)
        {
            List<string> childGroupCode = new List<string>();
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select CHILD_CODE from metadatacatalognode_r ");
            strSql.AppendFormat(" where  GROUP_CODE = '{0}'", groupCode);

            DataSet ds = baseUtilities.GetDataSet(strSql.ToString());

            for (int i = 0; i <ds.Tables[0].Rows.Count; i++)
            {
                childGroupCode.Add(ds.Tables[0].Rows[i][0].ToString());
            }
            return childGroupCode;
        }

        /// <summary>
        ///  zxw 20131221 根据目录获取该目录的回溯父节点列表，例如基础空间库中的"1:5万线划图"回溯后得到的list为list[0]="1:5万线划图"，list[1]="线划图",list[2]="基础空间数据库"
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        public List<metadatacatalognode_Mdl> GetParent(metadatacatalognode_Mdl node)
        {
            List<metadatacatalognode_Mdl> parents = new List<metadatacatalognode_Mdl>();
            metadatacatalognode_Dal metadatalognode = new metadatacatalognode_Dal(baseUtilities);

            bool isroot = false;
            string parentgroup = node.GROUP_CODE;
            string childgroup;
            while(!isroot)
            {
              if(!string.IsNullOrEmpty(parentgroup))
              {
                  List<metadatacatalognode_Mdl> tempLst = metadatalognode.GetCatalogGroup(string.Format("GROUP_CODE='{0}'", parentgroup));
                  parents.Add(tempLst[0]);
                  childgroup = parentgroup;
                  StringBuilder strSql = new StringBuilder();
                  strSql.Append("select GROUP_CODE from metadatacatalognode_r ");
                  strSql.AppendFormat(" where  CHILD_CODE = '{0}'", childgroup);
                  DataSet ds = baseUtilities.GetDataSet(strSql.ToString());
                  if (ds != null && ds.Tables[0].Rows.Count > 0)
                  {
                      parentgroup = ds.Tables[0].Rows[0]["GROUP_CODE"].ToString();
                  }
                  else
                  {
                      parentgroup = "";
                  }
              }
                else
              {
                  isroot=true;
              }
            }

            return parents;
        }

    }
}
