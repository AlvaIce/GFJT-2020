using System.Text;
using System.Xml;
using QRST_DI_Resources;
using QRST_DI_SS_Basis;
using QRST_DI_SS_Basis.MetaData;
using QRST_DI_SS_DBInterfaces.IDBEngine;

namespace QRST_DI_DS_Metadata.MetaDataCls
{
    public class MetaDataProWflRelation
    {
        public string[] relation = new string[]{
            "AlgName","DependAlg"
        };

        public string[] relationValues;

        public MetaDataProWflRelation()
        {
            relationValues = new string[relation.Length];
        }

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
        public string WflCode
        {
            set;
            get;
        }
        /// <summary>
        /// 
        /// </summary>
        public string AlgName
        {
            set { relationValues[0] = value; }
            get { return relationValues[0]; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string DependAlg
        {
            set { relationValues[1] = value; }
            get { return relationValues[1]; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string AlgVersion
        {
            set;
            get;
        }
        /// <summary>
        /// 
        /// </summary>
        public int? AlgOrder
        {
            set;
            get;
        }

        public void ReadAttributes(XmlNode node)
        {
            if (node != null)
            {
                for (int i = 0; i < relation.Length; i++)
                {
                    for (int j = 0; j < node.Attributes.Count; j++)
                    {
                        if (relation[i] == node.Attributes[j].Name)
                        {
                            relationValues[j] = node.Attributes[j].Value;
                            break;
                        }
                    }
                }
            }
        }

        public void ImportData(IDbBaseUtilities sqlBase)
        {
            //TableLocker dblock = new TableLocker(sqlBase);
            Constant.IdbOperating.LockTable("madb_wflrelation",EnumDBType.MIDB);
            ID = sqlBase.GetMaxID("ID", "madb_wflrelation");
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into madb_wflrelation(");
            strSql.Append("ID,WflCode,AlgName,DependAlg,AlgVersion,AlgOrder)");
            strSql.Append(" values (");
            //strSql.Append("@ID,@WflCode,@AlgName,@DependAlg,@AlgVersion,@AlgOrder)");
            //       MySqlParameter[] parameters = {
            //new MySqlParameter("@ID", MySqlDbType.Int32,11),
            //new MySqlParameter("@WflCode", MySqlDbType.VarChar,50),
            //new MySqlParameter("@AlgName", MySqlDbType.Text),
            //new MySqlParameter("@DependAlg", MySqlDbType.Text),
            //new MySqlParameter("@AlgVersion", MySqlDbType.VarChar,20),
            //new MySqlParameter("@AlgOrder", MySqlDbType.Int32,11)};
            //       parameters[0].Value = ID;
            //       parameters[1].Value = WflCode;
            //       parameters[2].Value = AlgName;
            //       parameters[3].Value = DependAlg;
            //       parameters[4].Value = AlgVersion;
            //       parameters[5].Value = AlgOrder;
            strSql.Append(string.Format("{0},'{1}','{2}','{3}','{4}',{5})", ID, WflCode, AlgName, DependAlg, AlgVersion,
                AlgOrder));
            sqlBase.ExecuteSql(strSql.ToString());
            Constant.IdbOperating.UnlockTable("madb_wflrelation",EnumDBType.MIDB);
        }
    }
}
