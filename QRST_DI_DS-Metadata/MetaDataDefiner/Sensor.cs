using System;
using System.Text;
using System.Data;
using QRST_DI_Resources;
using QRST_DI_SS_Basis;
using QRST_DI_SS_Basis.MetaData;
using QRST_DI_SS_DBInterfaces.IDBEngine;

namespace QRST_DI_DS_Metadata.MetaDataDefiner
{
    [Serializable]
    public class Sensors
    {
        public Sensors()
        {
            ID = -1;
        }
        #region Model
        private long _id;
        private string _name;
        private string _description;
        private string _qrst_code;
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
        public string DESCRIPTION
        {
            set { _description = value; }
            get { return _description; }
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

   

        public static void Add(Sensors model,IDbBaseUtilities dataBaseUtility)
        {
            //TableLocker dblock = new TableLocker(dataBaseUtility);
            Constant.IdbOperating.LockTable("sensors",EnumDBType.MIDB);
            int maxID = dataBaseUtility.GetMaxID("ID","sensors");
            model.QRST_CODE = string.Format("EVDB-R2-{0}", maxID);
            model.ID = maxID;
            StringBuilder strSql=new StringBuilder();
             strSql.Append("insert into sensors(");
             strSql.Append("ID,NAME,DESCRIPTION,QRST_CODE)");
             strSql.Append(" values (");
             strSql.AppendFormat("{0},'{1}','{2}','{3}')",model.ID,model.NAME,model.DESCRIPTION,model.QRST_CODE);
             dataBaseUtility.ExecuteSql(strSql.ToString());
             Constant.IdbOperating.UnlockTable("sensors",EnumDBType.MIDB);
        }

        	public static void Update(Sensors model,IDbBaseUtilities dataBaseUtility)
		{
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update sensors set ");
            strSql.AppendFormat("NAME = '{0}',Description = '{1}' ", model.NAME, model.DESCRIPTION);
            strSql.AppendFormat(" where ID={0} ",model.ID);
 
            dataBaseUtility.ExecuteSql(strSql.ToString());
		}

            public static void Delete(int id, IDbBaseUtilities dataBaseUtility)
            {
                dataBaseUtility.ExecuteSql(string.Format("delete from sensors where id = {0}",id));
            }

            public static DataSet GetSensorDataSet(IDbBaseUtilities dataBaseUtility,string queryCondition)
            {
                string sql;
                if (string.IsNullOrEmpty(queryCondition))
                {
                    sql = "select * from sensors ";
                }
                else
                {
                    sql = string.Format("select * from sensors where {0}",queryCondition);
                }

                return dataBaseUtility.GetDataSet(sql);
            }


    }
}
