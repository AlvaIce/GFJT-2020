using System;
using System.Text;
using MySql.Data.MySqlClient;
using System.Data;
using QRST_DI_Resources;
using QRST_DI_SS_Basis;
using QRST_DI_SS_Basis.MetaData;
using QRST_DI_SS_DBClient.DBEngine;
using QRST_DI_SS_DBInterfaces.IDBEngine;
 
namespace QRST_DI_TileStore
{
    /// <summary>
    /// 实体类tilelog 。(属性说明自动提取数据库字段的描述信息)
    /// </summary>
    [Serializable]
    public class tilelog
    {
        public tilelog()
        { }
        #region Model
        /// <summary>
        /// 
        /// </summary>
        public int ID { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string SenderIP { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string ReceiverIP { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string FileName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string OperationType { get; set; }
        #endregion Model
        #region 静态方法
        //public static MySqlBaseUtilities mysqlUtilities = new MySqlBaseUtilities();
        private static IDbOperating iSqLiteOperating = Constant.IdbOperating;
        public  static IDbBaseUtilities IBaseUtilities
        {
            get
            {
                return iSqLiteOperating.GetSubDbUtilities(EnumDBType.MIDB);
            }
        }
        /// <summary>
        /// 将切片同步日志写入数据库
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static int Add(tilelog model)
        {
            int i = -1;
            if(model != null)
            {
                //TableLocker dblock = new TableLocker(mysqlUtilities);
                //dblock.LockTable("tilelog");
                iSqLiteOperating.LockTable("tilelog", EnumDBType.MIDB);
              StringBuilder strSql=new StringBuilder();
              model.ID = IBaseUtilities.GetMaxID("ID", "tilelog");
			strSql.Append("insert into tilelog(");
			strSql.Append("ID,SenderIP,ReceiverIP,FileName,OperationType)");
			strSql.Append(" values (");
			strSql.Append("@ID,@SenderIP,@ReceiverIP,@FileName,@OperationType)");
			MySqlParameter[] parameters = {
					new MySqlParameter("@ID", MySqlDbType.Int32,11),
					new MySqlParameter("@SenderIP", MySqlDbType.Text),
					new MySqlParameter("@ReceiverIP", MySqlDbType.VarChar,20),
					new MySqlParameter("@FileName", MySqlDbType.Text),
					new MySqlParameter("@OperationType", MySqlDbType.VarChar,10)};
			parameters[0].Value = model.ID;
			parameters[1].Value = model.SenderIP;
			parameters[2].Value = model.ReceiverIP;
			parameters[3].Value = model.FileName;
			parameters[4].Value = model.OperationType;

              i = IBaseUtilities.ExecuteSql(strSql.ToString(), parameters);
                iSqLiteOperating.UnlockTable("tilelog", EnumDBType.MIDB);
            }
            return i;
        }

        public static int Delete(string whereString)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from tilelog ");
            if(!string.IsNullOrEmpty(whereString))
            {
                strSql.AppendFormat("where {0}",whereString);
            }
            return IBaseUtilities.ExecuteSql(strSql.ToString());
        }

        public static DataSet Query(string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select ID,SenderIP,ReceiverIP,FileName,OperationType ");
            strSql.Append(" FROM tilelog ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            return IBaseUtilities.GetDataSet(strSql.ToString());
        }
        #endregion
    }

    public enum EnumTileOperationType
    {
        Add = 0,
        Delete = 1,

    }
}
