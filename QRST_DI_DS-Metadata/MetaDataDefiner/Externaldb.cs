using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using QRST_DI_Resources;
using QRST_DI_SS_Basis;
using QRST_DI_SS_Basis.MetaData;
using QRST_DI_SS_DBInterfaces.IDBEngine;


namespace QRST_DI_DS_Metadata.MetaDataDefiner
{
    public class Externaldb
    {
        public Externaldb()
		{}
		#region Model
		private int _id;
		private string _name;
		private string _url;
		private string _description;
		private byte[] _image;
		/// <summary>
		/// 
		/// </summary>
		public int ID
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
		public string URL
		{
			set{ _url=value;}
			get{return _url;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string DESCRIPTION
		{
			set{ _description=value;}
			get{return _description;}
		}
		/// <summary>
		/// 
		/// </summary>
		public byte[] IMAGE
		{
			set{ _image=value;}
			get{return _image;}
		}
		#endregion Model



         #region 静态方法
        public static void Add(Externaldb model, IDbBaseUtilities dataBaseUtility)
        {
            //TableLocker dblock = new TableLocker(dataBaseUtility);
            Constant.IdbOperating.LockTable("externaldb",EnumDBType.MIDB);
            model.ID = dataBaseUtility.GetMaxID("ID", "externaldb");
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into externaldb(");
            strSql.Append("ID,NAME,URL,DESCRIPTION,IMAGE)");
            strSql.Append(" values (");
            strSql.Append(String.Format("{0},'{1}','{2}','{3}',{4})", model.ID, model.NAME, model.URL, model.DESCRIPTION,
                model.IMAGE));
            //strSql.Append("@ID,@NAME,@URL,@DESCRIPTION,@IMAGE)");
            //MySqlParameter[] parameters = {
            //        new MySqlParameter("@ID", MySqlDbType.Int32,3),
            //        new MySqlParameter("@NAME", MySqlDbType.VarChar,40),
            //        new MySqlParameter("@URL", MySqlDbType.VarChar,500),
            //        new MySqlParameter("@DESCRIPTION", MySqlDbType.VarChar,2000),
            //        new MySqlParameter("@IMAGE", MySqlDbType.Blob)};
            //parameters[0].Value = model.ID;
            //parameters[1].Value = model.NAME;
            //parameters[2].Value = model.URL;
            //parameters[3].Value = model.DESCRIPTION;
            //parameters[4].Value = model.IMAGE;

            dataBaseUtility.ExecuteSql(strSql.ToString());
            Constant.IdbOperating.UnlockTable("externaldb",EnumDBType.MIDB);
        }

        public static void Delete(int id,IDbBaseUtilities dataBaseUtility)
        {
            	StringBuilder strSql=new StringBuilder();
			strSql.Append("delete from externaldb ");
			strSql.AppendFormat("where ID = {0}",id);

            dataBaseUtility.ExecuteSql(strSql.ToString());
        }

        public static void Update(Externaldb model, IDbBaseUtilities dataBaseUtility)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(
                string.Format("update externaldb set NAME='{0}',URL='{1}',DESCRIPTION='{2}',IMAGE='{3}' where ID={4}",
                    model.NAME, model.URL, model.DESCRIPTION, model.IMAGE, model.ID));
            //strSql.Append("update externaldb set ");
            //strSql.Append("NAME=@NAME,");
            //strSql.Append("URL=@URL,");
            //strSql.Append("DESCRIPTION=@DESCRIPTION,");
            //strSql.Append("IMAGE=@IMAGE");
            //strSql.Append(" where ID=@ID ");
            //MySqlParameter[] parameters = {
            //        new MySqlParameter("@ID", MySqlDbType.Int32,3),
            //        new MySqlParameter("@NAME", MySqlDbType.VarChar,40),
            //        new MySqlParameter("@URL", MySqlDbType.VarChar,500),
            //        new MySqlParameter("@DESCRIPTION", MySqlDbType.VarChar,2000),
            //        new MySqlParameter("@IMAGE", MySqlDbType.Blob)};
            //parameters[0].Value = model.ID;
            //parameters[1].Value = model.NAME;
            //parameters[2].Value = model.URL;
            //parameters[3].Value = model.DESCRIPTION;
            //parameters[4].Value = model.IMAGE;

            dataBaseUtility.ExecuteSql(strSql.ToString());
        }

        public static List<Externaldb> GetList(string whereCondition, IDbBaseUtilities dataBaseUtility)
        {
            List<Externaldb> externaldbLst = new List<Externaldb>();

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select ID,NAME,URL,DESCRIPTION,IMAGE from externaldb ");
            if (!string.IsNullOrEmpty(whereCondition))
            {
                strSql.AppendFormat(" where {0}",whereCondition);
            }

            DataSet ds = dataBaseUtility.GetDataSet(strSql.ToString());
            for (int i = 0 ; i < ds.Tables[0].Rows.Count ;i++ )
            {
                Externaldb model = new Externaldb();
                if (ds.Tables[0].Rows[i]["ID"].ToString() != "")
                {
                    model.ID = int.Parse(ds.Tables[0].Rows[i]["ID"].ToString());
                }
                model.NAME = ds.Tables[0].Rows[i]["NAME"].ToString();
                model.URL = ds.Tables[0].Rows[i]["URL"].ToString();
                model.DESCRIPTION = ds.Tables[0].Rows[i]["DESCRIPTION"].ToString();
                if (ds.Tables[0].Rows[i]["IMAGE"] != DBNull.Value)
            {
                     model.IMAGE = (byte[])ds.Tables[0].Rows[i]["IMAGE"];
             }
                else
                {
                    model.IMAGE = null;
                }
                externaldbLst.Add(model);
            }
            return externaldbLst;
        }
       #endregion
    }
}
