using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using QRST_DI_DS_Metadata.MetaDataDefiner.Dal;
using QRST_DI_Resources;
using QRST_DI_SS_Basis;
using QRST_DI_SS_Basis.MetaData;
using QRST_DI_SS_DBInterfaces.IDBEngine;

namespace QRST_DI_DS_Metadata.MetaDataCls
{
    public  class MetaDataStandProWfl : MetaData
    {
        public string[] standProWflAttributes = new string[]{
             "DataTransModel","ProName","ProChsName","ProDesc","ProVersion","CutModle",
             "CutNumber","Artificial"
        };

        public string[] standProWflValues;

        public MetaDataParaUtility[] utilityList;
        public List<MetaDataStandAlgCmp> algCmpLst = new List<MetaDataStandAlgCmp>();
        public MetaDataProWflRelation[] relationArr;

        public MetaDataStandProWfl()
        {
            standProWflValues = new string[standProWflAttributes.Length];
        }

        #region 属性
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
		public string DataTransModel
		{
            set { standProWflValues[0]= value; }
            get { return standProWflValues[0]; }
		}
		/// <summary>
		/// 
		/// </summary>
		public string ProName
		{
            set { standProWflValues[1] = value; }
            get { return standProWflValues[1]; }
		}
		/// <summary>
		/// 
		/// </summary>
		public string ProChsName
		{
            set { standProWflValues[2] = value; }
            get { return standProWflValues[2]; }
		}
		/// <summary>
		/// 
		/// </summary>
		public string ProDesc
		{
            set { standProWflValues[3] = value; }
            get { return standProWflValues[3]; }
		}
		/// <summary>
		/// 
		/// </summary>
		public string ProVersion
		{
            set { standProWflValues[4] = value; }
            get { return standProWflValues[4]; }
		}
		/// <summary>
		/// 
		/// </summary>
		public string CutModle
		{
            set { standProWflValues[5] = value; }
            get { return standProWflValues[5]; }
		}
		/// <summary>
		/// 
		/// </summary>
		public int? CutNumber
		{
            set { standProWflValues[6] = value.ToString(); }
			get
            {
                try
                {
                    return int.Parse(standProWflValues[6]);
                }
                catch(Exception )
                {
                    return -1;
                }
            }
		}
		/// <summary>
		/// 
		/// </summary>
        public string Artificial
        {
            set { standProWflValues[7] = value; }
            get { return standProWflValues[7]; }
        }
		/// <summary>
		/// 
		/// </summary>
        public string QRST_CODE
        {
            set;
            get;
        }
		#endregion Model

        public override void ImportData(IDbBaseUtilities sqlBase)
        {
            try
            {
                //TableLocker dblock = new TableLocker(sqlBase);
                Constant.IdbOperating.LockTable("madb_proworkflow",EnumDBType.MIDB);
                tablecode_Dal tablecode = new tablecode_Dal(sqlBase);
                ID = sqlBase.GetMaxID("ID", "madb_proworkflow");
                QRST_CODE = tablecode.GetDataQRSTCode("madb_proworkflow", ID);
                StringBuilder strSql = new StringBuilder();
                strSql.Append("insert into madb_proworkflow(");
                strSql.Append("ID,DataTransModel,ProName,ProChsName,ProDesc,ProVersion,CutModle,CutNumber,Artificial,QRST_CODE)");
                strSql.Append(" values (");
                strSql.Append(string.Format("{0},'{1}','{2}','{3}','{4}','{5}','{6}',{7},'{8}','{9}')", ID,
                    DataTransModel, ProName, ProChsName, ProDesc, ProVersion, CutModle, CutNumber, Artificial, QRST_CODE));

                //           strSql.Append("@ID,@DataTransModel,@ProName,@ProChsName,@ProDesc,@ProVersion,@CutModle,@CutNumber,@Artificial,@QRST_CODE)");
                //           MySqlParameter[] parameters = {
                //new MySqlParameter("@ID", MySqlDbType.Int32,11),
                //new MySqlParameter("@DataTransModel", MySqlDbType.VarChar,200),
                //new MySqlParameter("@ProName", MySqlDbType.Text),
                //new MySqlParameter("@ProChsName", MySqlDbType.Text),
                //new MySqlParameter("@ProDesc", MySqlDbType.Text),
                //new MySqlParameter("@ProVersion", MySqlDbType.VarChar,50),
                //new MySqlParameter("@CutModle", MySqlDbType.Text),
                //new MySqlParameter("@CutNumber", MySqlDbType.Int32,11),
                //new MySqlParameter("@Artificial", MySqlDbType.Text),
                //new MySqlParameter("@QRST_CODE", MySqlDbType.VarChar,50)};
                //           parameters[0].Value = ID;
                //           parameters[1].Value = DataTransModel;
                //           parameters[2].Value = ProName;
                //           parameters[3].Value = ProChsName;
                //           parameters[4].Value = ProDesc;
                //           parameters[5].Value = ProVersion;
                //           parameters[6].Value = CutModle;
                //           parameters[7].Value = CutNumber;
                //           parameters[8].Value = Artificial;
                //           parameters[9].Value = QRST_CODE;

                sqlBase.ExecuteSql(strSql.ToString());
                Constant.IdbOperating.UnlockTable("madb_proworkflow",EnumDBType.MIDB);

                //将算法依赖关系写入数据库
                for (int j = 0; j < relationArr.Length; j++)
                {
                    relationArr[j].WflCode = QRST_CODE;
                    for (int k = 0; k < algCmpLst.Count;k++ )
                    {
                        if (algCmpLst[k].AlgorithmName == relationArr[j].AlgName)
                        {
                            relationArr[j].AlgVersion = algCmpLst[k].Version;
                            break;
                        }
                    }
                    relationArr[j].ImportData(sqlBase);
                }
                //根据算法名字和版本信息判断算法组件是否已经存在，若存在，则不导入算法，否则将算法导入
                for (int i = 0; i < algCmpLst.Count; i++)
                {
                    if (!algCmpLst[i].ExistInDb(sqlBase))
                    {
                        algCmpLst[i].utilityList = utilityList;
                        algCmpLst[i].ImportData(sqlBase);
                    }
                    else
                    {
                        algCmpLst[i] = null;
                    }
                }
                //导入utility
                for (int i = 0; i < utilityList.Length;i++ )
                {
                    utilityList[i].ParaID = QRST_CODE;
                    utilityList[i].ImportData(sqlBase);
                }
            }
            catch (Exception ex)
            {
                //撤销之前的操作，抛出异常
               // sqlBase.ExecuteSql(string.Format("delete from madb_algorithmcmp where  QRST_CODE = '{0}'", QRST_CODE));
                throw new Exception("元数据导入失败" + ex.ToString());
            }
        }

        public override void ReadAttributes(string fileName)
        {
            XmlDocument root = new XmlDocument();
            try
            {
                root.Load(fileName);
            }
            catch (System.Exception ex)
            {
                throw new Exception("xml文件损坏，请检查！");
            }
            XmlNode node = null;

            try
            {
                for (int i = 0; i < standProWflAttributes.Length; i++)
                {
                    node = root.GetElementsByTagName(standProWflAttributes[i]).Item(0);
                    if (node == null)
                    {
                        standProWflValues[i] = "";
                    }
                    else
                    {
                        standProWflValues[i] = node.InnerText;
                    }
                }
                //获取产品流程的算法组件信息
                XmlNodeList paranodeLst = root.GetElementsByTagName("AlgCompt");
                if (paranodeLst != null)
                {
                    for (int i = 0; i < paranodeLst.Count;i++ )
                    {
                        MetaDataStandAlgCmp tempCmp = new MetaDataStandAlgCmp();
                        tempCmp.ReadAttributesByXmlNode(paranodeLst[i]);
                        algCmpLst.Add(tempCmp);
                    }
                }

                //获取utility信息
                XmlNodeList utilityArr = root.GetElementsByTagName("Utility");
                utilityList = new MetaDataParaUtility[utilityArr.Count];
                for (int j = 0; j < utilityArr.Count; j++)
                {
                    utilityList[j] = new MetaDataParaUtility();
                    utilityList[j].ReadAttribute(utilityArr[j]);
                }

                //获取产品流程依赖关系
                XmlNodeList relationNodeArr = root.GetElementsByTagName("relation");
                relationArr = new MetaDataProWflRelation[relationNodeArr.Count];
                for (int k = 0; k < relationNodeArr.Count; k++)
                {
                    relationArr[k] = new MetaDataProWflRelation();
                    relationArr[k].AlgOrder = k + 1;
                    relationArr[k].ReadAttributes(relationNodeArr[k]);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("读取元数据信息出错" + ex.ToString());
            }
        }



       
    }
}
