using System.Text;
using System.Xml;
using QRST_DI_Resources;
using QRST_DI_SS_Basis;
using QRST_DI_SS_Basis.MetaData;
using QRST_DI_SS_DBInterfaces.IDBEngine;

namespace QRST_DI_DS_Metadata.MetaDataCls
{
    public  class MetaDataParaUtility
    {
        public MetaDataParaUtility()
		{
            ParaUtilityValues = new string[ParaUtilityAttributes.Length];
        }

        public string[] ParaUtilityAttributes = new string[]
        {
            "Satellite","Sensor","Resolution"
        };

        public string[] ParaUtilityValues;

        /// <summary>
        /// 
        /// </summary>
        public int ID
        {
            set;
            get;
        }
        /// <summary>
        /// 
        /// </summary>
        public string Satellite
        {
            set { ParaUtilityValues[0] = value; }
            get { return ParaUtilityValues[0]; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Sensor
        {
            set { ParaUtilityValues[1] = value; }
            get { return ParaUtilityValues[1]; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Resolution
        {
            set { ParaUtilityValues[2] = value; }
            get { return ParaUtilityValues[2]; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string ParaID
        {
            set;
            get;
        }

        public void ImportData(IDbBaseUtilities sqlBase)
        {

            //TableLocker dblock = new TableLocker(sqlBase);
            Constant.IdbOperating.LockTable("madb_wflutility",EnumDBType.MIDB); 
            ID = sqlBase.GetMaxID("ID", "madb_wflutility");
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into madb_wflutility(");
            strSql.Append("ID,Satellite,Sensor,Resolution,ParaID)");
            strSql.Append(" values (");
            strSql.Append(string.Format("{0},'{1}','{2}','{3}','{4}')",
                 ID, Satellite, Sensor, Resolution, ParaID));
            //       strSql.Append("@ID,@Satellite,@Sensor,@Resolution,@ParaID)");
            //       MySqlParameter[] parameters = {
            //new MySqlParameter("@ID", MySqlDbType.Int32,11),
            //new MySqlParameter("@Satellite", MySqlDbType.VarChar,200),
            //new MySqlParameter("@Sensor",MySqlDbType.Text),
            //new MySqlParameter("@Resolution", MySqlDbType.Text),
            //new MySqlParameter("@ParaID", MySqlDbType.VarChar,50)};
            //       parameters[0].Value = ID;
            //       parameters[1].Value = Satellite;
            //       parameters[2].Value = Sensor;
            //       parameters[3].Value = Resolution;
            //       parameters[4].Value = ParaID;

            sqlBase.ExecuteSql(strSql.ToString());
            Constant.IdbOperating.UnlockTable("madb_wflutility",EnumDBType.MIDB);
        }

        public void ReadAttribute(XmlNode node)
        {
            if(node != null)
            {
                for (int i = 0; i < ParaUtilityAttributes.Length;i++ )
                {
                    for (int j = 0; j < node.Attributes.Count;j++ )
                    {
                        if (ParaUtilityAttributes[i] == node.Attributes[j].Name)
                        {
                            ParaUtilityValues[j] = node.Attributes[j].Value;
                            break;
                        }
                    }
                }
            }
        }
    }
}
