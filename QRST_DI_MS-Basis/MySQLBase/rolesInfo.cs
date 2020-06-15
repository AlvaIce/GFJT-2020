using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using QRST_DI_Resources;
using QRST_DI_SS_Basis;
using QRST_DI_SS_Basis.MetaData;
using QRST_DI_SS_DBInterfaces.IDBEngine;

namespace QRST_DI_MS_Basis.UserRole
{
    [Serializable]
    public class rolesInfo
    {
        public rolesInfo()
		{}

        public static IDbBaseUtilities sqlUtilities; 
		#region Model
		private long _id;
		private string _name;
		private string _qrst_code;
		private string _description;

        public int[][] _rolearr;
		/// <summary>
		/// 
		/// </summary>
		public long ID
		{
			set{ _id=value;}
			get{return _id;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string NAME
		{
			set{ _name=value;}
			get{return _name;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string QRST_CODE
		{
			set{ _qrst_code=value;}
			get{return _qrst_code;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string DESCRIPTION
		{
			set{ _description=value;}
			get{return _description;}
		}

      public string ToString()
        {
            return NAME;
        }

        /// <summary>
        /// 
        /// </summary>
        public int[][] RoleArray
        {
            get { return GetRoleArray(); }
        }
		#endregion Model

        /// <summary>
        /// 根据权限描述，解析权限数组
        /// </summary>
        /// <returns></returns>
        public int[][] GetRoleArray()
        {
            int[][] roleState;
            String[] modules = DESCRIPTION.Split(';');

            roleState = new int[modules.Length - 1][];

            for (int i = 0 ; i < modules.Length - 1 ; i++)
            {
                roleState[i] = new int[modules[i].Length];

                for (int j = 0 ; j < modules[i].Length ; j++)
                {
                    roleState[i][j] = int.Parse(modules[i][j].ToString());
                }
            }
            return roleState;
        }

        
#region 一些静态函数
        /// <summary>
        /// 将两个角色数组合并成一个数组
        /// </summary>
        /// <param name="roleArr1"></param>
        /// <param name="roleArr2"></param>
        /// <returns></returns>
        public static int[][] MergeRoleArr(int [][]roleArr1,int [][]roleArr2)
        {
            int[][] mergedArr;
            int row = roleArr1.Length > roleArr2.Length ? roleArr1.Length : roleArr2.Length;
            mergedArr = new int[row][];
            for (int i = 0 ; i < row ;i++ )
            {
                if (roleArr1.Length >i && roleArr2.Length>i)
                {
                         //按列进行合并
                    int []maxCol;
                    int []minCol;
                    if (roleArr2[i].Length > roleArr1[i].Length)
                    {
                        maxCol = roleArr2[i];
                        minCol = roleArr1[i];
                    }
                    else
                    {
                         maxCol = roleArr1[i];
                         minCol = roleArr2[i];
                    }
                mergedArr[i] = new int[maxCol.Length];
                mergedArr[i][0] = 0;
                for (int j = 1 ; j < maxCol.Length ;j++ )
                 {
                    if (j < minCol.Length)
                    {
                        if (minCol[j] + maxCol[j] == 0)   //两个角色都不具有该权限
                        {
                            mergedArr[i][j] = 0;
                        }
                        else
                            mergedArr[i][j] = 1;
                    }
                    else
                        mergedArr[i][j] = maxCol[j];
                   
                     mergedArr[i][0] = mergedArr[i][0] + mergedArr[i][j];
                 }
                }
                else   //选取行较多的数组赋值
                {
                    int[] maxArr; 
                    if (roleArr1.Length > roleArr2.Length)
                    {
                        maxArr = roleArr1[i];
                    }
                    else
                    {
                        maxArr = roleArr2[i];
                    }
                    mergedArr[i] = new int[maxArr.Length];
                    for (int k=0;k<maxArr.Length;k++)
                    {
                        mergedArr[i][k] = maxArr[k];
                    }
                }
           
            }
            return mergedArr;
        }

        /// <summary>
        /// 将角色数组转换为字符串
        /// </summary>
        /// <returns></returns>
        public static string RoleArr2Str(int[][] roleArr1)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0 ; i < roleArr1.Length ;i++ )
            {
                StringBuilder sb1 = new StringBuilder();
                for (int j = 0 ; j < roleArr1[i].Length ;j++ )
                {
                    sb1.Append(roleArr1[i][j].ToString());
                }
                sb1.Append(";");
                sb.Append(sb1.ToString());
            }
            return sb.ToString();
        }

        public static bool ExistRole(string roleName)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.AppendFormat("select ID FROM rolesinfo where NAME = '{0}'",roleName);
            DataSet ds = sqlUtilities.GetDataSet(strSql.ToString());
            if (ds.Tables[0].Rows.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static int AddRole(rolesInfo model)
        {
            //TableLocker dblock = new TableLocker(sqlUtilities);
            Constant.IdbOperating.LockTable("rolesinfo",EnumDBType.MIDB);
            model.ID = sqlUtilities.GetMaxID("ID", "rolesinfo");
            model.QRST_CODE = "MIDB-11" + "-" + model.ID.ToString();
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into rolesinfo(");
            strSql.Append("ID,NAME,QRST_CODE,DESCRIPTION)");
            strSql.Append(" values (");
            strSql.Append(string.Format("{0},'{1}','{2}','{3}')", model.ID, model.NAME, model.QRST_CODE, model.DESCRIPTION));
            //strSql.Append("@ID,@NAME,@QRST_CODE,@DESCRIPTION)");
     //       MySqlParameter[] parameters = {
					//new MySqlParameter("@ID", MySqlDbType.Int32,7),
					//new MySqlParameter("@NAME", MySqlDbType.Text),
					//new MySqlParameter("@QRST_CODE", MySqlDbType.Text),
					//new MySqlParameter("@DESCRIPTION", MySqlDbType.Text)};
     //       parameters[0].Value = model.ID;
     //       parameters[1].Value = model.NAME;
     //       parameters[2].Value = model.QRST_CODE;
     //       parameters[3].Value = model.DESCRIPTION;
            int rs = sqlUtilities.ExecuteSql(strSql.ToString());
            Constant.IdbOperating.UnlockTable("rolesinfo",EnumDBType.MIDB);
            return rs;
        }

        public static int updateRole(rolesInfo model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(string.Format("update rolesinfo set NAME='{0}',DESCRIPTION='{1}' where ID={2}", model.NAME,
                model.DESCRIPTION, model.ID));
     //       strSql.Append("update rolesinfo set ");
     //       strSql.Append("NAME=@NAME,");
     //       strSql.Append("DESCRIPTION=@DESCRIPTION");
     //       strSql.Append(" where ID=@ID");
     //       MySqlParameter[] parameters = {
					//new MySqlParameter("@ID", MySqlDbType.Int32,7),
					//new MySqlParameter("@NAME", MySqlDbType.Text),
					//new MySqlParameter("@DESCRIPTION", MySqlDbType.Text)};
     //       parameters[0].Value = model.ID;
     //       parameters[1].Value = model.NAME;
     //       parameters[2].Value = model.DESCRIPTION;
            
            return sqlUtilities.ExecuteSql(strSql.ToString());
        }

        public static int DeleteRole(int id)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from rolesinfo ");
            strSql.AppendFormat(" where ID= {0}",id);

            return sqlUtilities.ExecuteSql(strSql.ToString());
        }

        public static List<rolesInfo> GetList(string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select ID,NAME,QRST_CODE,DESCRIPTION ");
            strSql.Append(" FROM rolesinfo ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            DataSet ds = sqlUtilities.GetDataSet(strSql.ToString());
            List<rolesInfo> roleLst = new List<rolesInfo>();

            for (int i = 0 ; i < ds.Tables[0].Rows.Count ; i++)
            {
                rolesInfo model = new rolesInfo();
                if (ds.Tables[0].Rows[i]["ID"].ToString() != "")
                {
                    model.ID = long.Parse(ds.Tables[0].Rows[i]["ID"].ToString());
                }
                model.NAME = ds.Tables[0].Rows[i]["NAME"].ToString();
                model.QRST_CODE = ds.Tables[0].Rows[i]["QRST_CODE"].ToString();
                model.DESCRIPTION = ds.Tables[0].Rows[i]["DESCRIPTION"].ToString();

                roleLst.Add(model);
            }
            return roleLst;
        }

#endregion
    }
}
