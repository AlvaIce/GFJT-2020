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
    /// <summary>
    /// 实体类userinfo 。(属性说明自动提取数据库字段的描述信息)
    /// </summary>
    [Serializable]
    public class userInfo
    {
        public userInfo()
        { }
        public static IDbBaseUtilities sqlUtilities=Constant.IdbServerUtilities; 

        #region Model
        private long _id;
        private string _name;
        private string _password;
        private long _levels;
        private long _logintimes;
        private long _lastsite;
        private DateTime? _lastdatetime;
        private string _email;
        private string _address;
        private string _realname;
        private string _cellphone;
        private long _available;
        private string _qrst_code;
        private string _description;
        private string _roles;

        public string[] ROLESARR
        {
            get { return Roles2RolesArr(); }
        }
        /// <summary>
        /// 
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
        public string PASSWORD
        {
            set { _password = value; }
            get { return _password; }
        }
        /// <summary>
        /// 
        /// </summary>
        public long LEVELS
        {
            set { _levels = value; }
            get { return _levels; }
        }
        /// <summary>
        /// 
        /// </summary>
        public long LOGINTIMES
        {
            set { _logintimes = value; }
            get { return _logintimes; }
        }
        /// <summary>
        /// 
        /// </summary>
        public long LASTSITE
        {
            set { _lastsite = value; }
            get { return _lastsite; }
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime? LASTDATETIME
        {
            set { _lastdatetime = value; }
            get { return _lastdatetime; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string EMAIL
        {
            set { _email = value; }
            get { return _email; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string ADDRESS
        {
            set { _address = value; }
            get { return _address; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string REALNAME
        {
            set { _realname = value; }
            get { return _realname; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string CELLPHONE
        {
            set { _cellphone = value; }
            get { return _cellphone; }
        }
        /// <summary>
        /// 
        /// </summary>
        public long AVAILABLE
        {
            set { _available = value; }
            get { return _available; }
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

        public string ROLES
        {
            set { _roles = value; }
            get { return _roles; }
        }

        #endregion Model

        /// <summary>
        /// 将角色字符串转换为角色数组
        /// </summary>
        /// <returns></returns>
        public string[] Roles2RolesArr()
        {
            String[] modules = ROLES.Split(";".ToCharArray(),StringSplitOptions.RemoveEmptyEntries);
            return modules;
        }

        /// <summary>
        /// 判断用户是否具有该角色
        /// </summary>
        /// <param name="role"></param>
        /// <returns></returns>
        public bool HasRole(rolesInfo role)
        {
            if (ROLES.Contains(role.QRST_CODE))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public List<rolesInfo> GetRoleList()
        {
            List<rolesInfo> roleLst = new List<rolesInfo>();
            for (int i = 0 ; i < ROLESARR.Length ;i++ )
            {
                roleLst.AddRange(rolesInfo.GetList(string.Format(" QRST_CODE = '{0}'",ROLESARR[i])));
            }
            return roleLst;
        }
       
        /// <summary>
        /// 获取用户的权限数组，即将该用户角色的权限进行合并
        /// </summary>
        /// <returns></returns>
       public int[][] GetRightArr()
        {
            List<rolesInfo> roleLst = GetRoleList();
            int[][] roleArr = roleLst[0].RoleArray;
            for (int i = 1 ; i < roleLst.Count ;i++ )
            {
                roleArr = rolesInfo.MergeRoleArr(roleArr, roleLst[i].RoleArray);
            }
           return roleArr;
        }


#region  一些静态方法
       public static int AddUser(userInfo model)
       {
           //TableLocker dblock = new TableLocker(sqlUtilities);
           Constant.IdbOperating.LockTable("userinfo",EnumDBType.MIDB);
           StringBuilder strSql = new StringBuilder();
           model.ID = sqlUtilities.GetMaxID("ID", "userinfo");
           model.QRST_CODE = "MIDB-16" + "-" + model.ID.ToString();
           model.PASSWORD = Secret.Encrypt(model.PASSWORD);
           strSql.Append("insert into userinfo(");
           strSql.Append("ID,NAME,PASSWORD,LEVELS,LOGINTIMES,LASTSITE,EMAIL,ADDRESS,REALNAME,CELLPHONE,AVAILABLE,QRST_CODE,DESCRIPTION,ROLES)");
           strSql.Append(" values (");
           strSql.Append(
               string.Format(
                   "{0},'{1}','{2}',{3},{4},{5},'{6}','{7}','{8}','{9}',{10},'{11}','{12}','{13}')", model.ID,
                   model.NAME, model.PASSWORD, model.LEVELS, model.LOGINTIMES, model.LASTSITE,
                   model.EMAIL, model.ADDRESS, model.REALNAME, model.CELLPHONE, model.AVAILABLE, model.QRST_CODE,
                   model.DESCRIPTION, model.ROLES));
           int rs = sqlUtilities.ExecuteSql(strSql.ToString());
            Constant.IdbOperating.UnlockTable("userinfo",EnumDBType.MIDB);
           return rs;
       }

        public static int updateUser(userInfo model)
        {
            model.PASSWORD = Secret.Encrypt(model.PASSWORD);
            StringBuilder strSql = new StringBuilder();
            strSql.Append(
                string.Format(
                    "update userinfo set NAME='{0}',PASSWORD='{1}',LEVELS='{2}',LOGINTIMES={3},LASTSITE={4},EMAIL='{5}',ADDRESS='{6}',REALNAME='{7}',CELLPHONE='{8}'," +
                    "AVAILABLE={9},DESCRIPTION='{10}',ROLES='{11}' where ID={12}",
                    model.NAME, model.PASSWORD, model.LEVELS, model.LOGINTIMES, model.LASTSITE,
                    model.EMAIL, model.ADDRESS, model.REALNAME, model.CELLPHONE, model.AVAILABLE, model.DESCRIPTION,
                    model.ROLES, model.ID));
            string path = sqlUtilities.GetDbConnection();
            return sqlUtilities.ExecuteSql(strSql.ToString());
        }

        public static int deleteUser(int id)
        {
            StringBuilder strSql=new StringBuilder();
			strSql.Append("delete from userinfo ");
            strSql.AppendFormat(" where ID = {0}",id);
            return sqlUtilities.ExecuteSql(strSql.ToString());
        }

        public static List<userInfo> GetList(string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select * ");
            strSql.Append(" FROM userinfo ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            List<userInfo> userLst = new List<userInfo>();
            DataSet ds = sqlUtilities.GetDataSet(strSql.ToString());
            for (int i = 0 ; i < ds.Tables[0].Rows.Count ;i++ )
            {
                userInfo model = new userInfo();

                if (ds.Tables[0].Rows[i]["ID"].ToString() != "")
                {
                    model.ID = long.Parse(ds.Tables[0].Rows[i]["ID"].ToString());
                }
                model.NAME = ds.Tables[0].Rows[i]["NAME"].ToString();
                model.PASSWORD = Secret.Decrypt(ds.Tables[0].Rows[i]["PASSWORD"].ToString()) ;
                if (ds.Tables[0].Rows[i]["LEVELS"].ToString() != "")
                {
                    model.LEVELS = long.Parse(ds.Tables[0].Rows[i]["LEVELS"].ToString());
                }
                if (ds.Tables[0].Rows[0]["LOGINTIMES"].ToString() != "")
                {
                    model.LOGINTIMES = long.Parse(ds.Tables[0].Rows[i]["LOGINTIMES"].ToString());
                }
                if (ds.Tables[0].Rows[0]["LASTSITE"].ToString() != "")
                {
                    model.LASTSITE = long.Parse(ds.Tables[0].Rows[i]["LASTSITE"].ToString());
                }
                if (ds.Tables[0].Rows[i]["LASTDATETIME"].ToString() != "")
                {
                    model.LASTDATETIME = DateTime.Parse(ds.Tables[0].Rows[i]["LASTDATETIME"].ToString());
                }
                model.EMAIL = ds.Tables[0].Rows[i]["EMAIL"].ToString();
                model.ADDRESS = ds.Tables[0].Rows[i]["ADDRESS"].ToString();
                model.REALNAME = ds.Tables[0].Rows[i]["REALNAME"].ToString();
                model.CELLPHONE = ds.Tables[0].Rows[i]["CELLPHONE"].ToString();
                if (ds.Tables[0].Rows[i]["AVAILABLE"].ToString() != "")
                {
                    model.AVAILABLE = long.Parse(ds.Tables[0].Rows[i]["AVAILABLE"].ToString());
                }
                model.QRST_CODE = ds.Tables[0].Rows[i]["QRST_CODE"].ToString();
                model.DESCRIPTION = ds.Tables[0].Rows[i]["DESCRIPTION"].ToString();
                model.ROLES = ds.Tables[0].Rows[i]["ROLES"].ToString();
                userLst.Add(model);
            }
            return userLst;
        }

        public static string RoleLst2RoleStr(string[] lstRole)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0 ; i < lstRole.Length ;i++ )
            {
                sb.Append(lstRole[i]);
                sb.Append(";");
            }
            return sb.ToString();
        }

      


#endregion

    }
}
