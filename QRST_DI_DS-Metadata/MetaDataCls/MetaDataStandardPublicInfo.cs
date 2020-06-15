using System;
using System.Linq;
using System.Text;
using QRST_DI_Resources;
using QRST_DI_SS_Basis;
using QRST_DI_SS_Basis.MetaData;
using QRST_DI_SS_DBInterfaces.IDBEngine;

namespace QRST_DI_DS_Metadata.MetaDataCls
{
    /// <summary>
    /// 用于描述标准数据中的公共字段，如文件大小、下载次数、是否公有等信息.
    /// </summary>
    public class MetaDataStandardPublicInfo
    {
        public MetaDataStandardPublicInfo()
        {
            _ispublic = "true";
            _downloadcount = 0;
        }
        #region Model
        private int _id;
        private int  _downloadcount;
        private Double _datasize;
        private string _ispublic;
        private string _datacode;
        private string _qrst_code;

        IDbBaseUtilities sqlutilities;
        /// <summary>
        /// 
        /// </summary>
        public int ID
        {
            set { _id = value; }
            get { return _id; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int  DownLoadCount
        {
            set { _downloadcount = value; }
            get { return _downloadcount; }
        }
        /// <summary>
        /// 
        /// </summary>
        public Double DataSize
        {
            set { _datasize = value; }
            get { return _datasize; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string IsPublic
        {
            set { _ispublic = value; }
            get { return _ispublic; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string DataCode
        {
            set { _datacode = value; }
            get { return _datacode; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string QRST_CODE
        {
            set { _qrst_code = value; }
            get { return _qrst_code; }
        }
        #endregion Model

        public static void Add(MetaDataStandardPublicInfo model,IDbBaseUtilities sqlutilities)
        {
            int count = sqlutilities.GetDataSet(string.Format(" select * from publicinfo where DataCode = '{0}' ",model.DataCode)).Tables[0].Rows.Count;
            if (count > 0)
            {
                sqlutilities.ExecuteSql(string.Format(" delete from publicinfo where DataCode = '{0}'", model.DataCode));
            }
            //TableLocker dblock = new TableLocker(sqlutilities);
            Constant.IdbOperating.LockTable("publicinfo",EnumDBType.MIDB);
            model.ID = sqlutilities.GetMaxID("ID", "publicinfo");
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into publicinfo(");
            strSql.Append("ID,DownLoadCount,FileSize,PublicCloud,DataCode,QRST_CODE)");
            strSql.Append(" values (");
            strSql.Append(
                string.Format("{0},{1},{2},'{3}','{4}','{5}')", model.ID, model.DownLoadCount, model.DataSize,
                    model.IsPublic, model.DataCode, model.QRST_CODE));
            //       strSql.Append("@ID,@DownLoadCount,@FileSize,@PublicCloud,@DataCode,@QRST_CODE)");
            //       MySqlParameter[] parameters = {
            //new MySqlParameter("@ID", MySqlDbType.Int32,7),
            //new MySqlParameter("@DownLoadCount", MySqlDbType.Int32,7),
            //new MySqlParameter("@FileSize", MySqlDbType.Double,10),
            //new MySqlParameter("@PublicCloud", MySqlDbType.VarChar,50),
            //new MySqlParameter("@DataCode", MySqlDbType.VarChar,50),
            //new MySqlParameter("@QRST_CODE", MySqlDbType.VarChar,50)};
            //       parameters[0].Value = model.ID;
            //       parameters[1].Value = model.DownLoadCount;
            //       parameters[2].Value = model.DataSize;
            //       parameters[3].Value = model.IsPublic;
            //       parameters[4].Value = model.DataCode;
            //       parameters[5].Value = model.QRST_CODE;

            sqlutilities.ExecuteSql(strSql.ToString());
            Constant.IdbOperating.UnlockTable("publicinfo",EnumDBType.MIDB);
        }

        public static void Update(MetaDataStandardPublicInfo model,IDbBaseUtilities sqlutilities)
        {
            	StringBuilder strSql=new StringBuilder();
            strSql.Append(
                string.Format(
                    "update publicinfo set DownLoadCount={0},FileSize={1},PublicCloud='{2}',DataCode='{3}',QRST_CODE='{4}' where ID={5}",
                    model.DownLoadCount, model.DataSize, model.IsPublic, model.DataCode, model.QRST_CODE, model.ID));
			//strSql.Append("update publicinfo set ");
			//strSql.Append("DownLoadCount=@DownLoadCount,");
   //         strSql.Append("FileSize=@FileSize,");
   //         strSql.Append("PublicCloud=@PublicCloud,");
			//strSql.Append("DataCode=@DataCode,");
			//strSql.Append("QRST_CODE=@QRST_CODE");
			//strSql.Append(" where ID=@ID ");
			//MySqlParameter[] parameters = {
			//		new MySqlParameter("@ID", MySqlDbType.Int32,7),
			//		new MySqlParameter("@DownLoadCount", MySqlDbType.Int32,7),
			//		new MySqlParameter("@FileSize", MySqlDbType.Double,10),
			//		new MySqlParameter("@PublicCloud", MySqlDbType.VarChar,50),
			//		new MySqlParameter("@DataCode", MySqlDbType.VarChar,50),
			//		new MySqlParameter("@QRST_CODE", MySqlDbType.VarChar,50)};
			//parameters[0].Value = model.ID;
			//parameters[1].Value = model.DownLoadCount;
			//parameters[2].Value = model.DataSize;
			//parameters[3].Value = model.IsPublic;
			//parameters[4].Value = model.DataCode;
			//parameters[5].Value = model.QRST_CODE;

			sqlutilities.ExecuteSql(strSql.ToString());
        }

        public static void Delete(int ID,IDbBaseUtilities sqlutilities)
        {
            StringBuilder strSql=new StringBuilder();
			//strSql.Append("delete from publicinfo ");
			//strSql.Append(" where ID=@ID ");
			//MySqlParameter[] parameters = {
			//		new MySqlParameter("@ID", MySqlDbType.Int32)};
			//parameters[0].Value = ID;
            strSql.Append(string.Format("delete from publicinfo where ID={0}", ID));
            sqlutilities.ExecuteSql(strSql.ToString());
        }
    }
}
