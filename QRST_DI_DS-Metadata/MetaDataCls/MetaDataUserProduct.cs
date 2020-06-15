using System;
using System.Text;
using System.Xml;
using QRST_DI_DS_Metadata.MetaDataDefiner.Dal;
using QRST_DI_Resources;
using QRST_DI_SS_Basis;
using QRST_DI_SS_Basis.MetaData;
using QRST_DI_SS_DBInterfaces.IDBEngine;

namespace QRST_DI_DS_Metadata.MetaDataCls
{
    public class MetaDataUserProduct : MetaData
    {
        public string[] userProductAttributes = new string[]{
             "MainProductID","ProductName","ProductNameCh","Mode","OrderSource","Operator","OrderSubmitTime",
             "Satellite","Sensors","Resolution","East","West","South","North","QRST_CODE"
        };

        public MetaDataUserProduct()
        {
            userProductValues = new string[userProductAttributes.Length];
        }

        public string[] userProductValues;
        #region Model
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
        public string MainProductID
        {
            set { userProductAttributes[0] = value; }
            get { return userProductAttributes[0]; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string ProductName
        {
            set { userProductAttributes[1] = value; }
            get { return userProductAttributes[1]; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string ProductNameCh
        {
            set { userProductAttributes[2] = value; }
            get { return userProductAttributes[2]; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Mode
        {
            set { userProductAttributes[3] = value; }
            get { return userProductAttributes[3]; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string OrderSource
        {
            set { userProductAttributes[4] = value; }
            get { return userProductAttributes[4]; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Operator
        {
            set { userProductAttributes[5] = value; }
            get { return userProductAttributes[5]; }
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime OrderSubmitTime
        {
            set {
                userProductAttributes[6] = value.ToString();
            }
            get {
                try
                {
                    DateTime dt = DateTime.Parse(userProductAttributes[6]);
                    return dt;
                }catch(Exception ex)
                {
                    throw;
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Satellite
        {
            set { userProductAttributes[7] = value; }
            get { return userProductAttributes[7]; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Sensors
        {
            set { userProductAttributes[8] = value; }
            get { return userProductAttributes[8]; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Resolution
        {
            set { userProductAttributes[9] = value; }
            get { return userProductAttributes[9]; }
        }
        /// <summary>
        /// 
        /// </summary>
        public decimal? East
        {
            set { userProductAttributes[10] = value.ToString(); }
            get
            {
                try
                {
                    return decimal.Parse(userProductAttributes[10]);
                }
                catch (Exception)
                {
                    return null;
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public decimal? West
        {
            set { userProductAttributes[11] = value.ToString(); }
            get
            {
                try
                {
                    return decimal.Parse(userProductAttributes[11]);
                }
                catch (Exception)
                {
                    return null;
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public decimal? South
        {
            set { userProductAttributes[12] = value.ToString(); }
            get
            {
                try
                {
                    return decimal.Parse(userProductAttributes[12]);
                }
                catch (Exception)
                {
                    return null;
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public decimal? North
        {
            set { userProductAttributes[13] = value.ToString(); }
            get
            {
                try
                {
                    return decimal.Parse(userProductAttributes[13]);
                }
                catch (Exception)
                {
                    return null;
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public string QRST_CODE
        {
            set { userProductAttributes[14] = value; }
            get { return userProductAttributes[14]; }
        }
        #endregion Model

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
                //读取一般信息
                for (int i = 0; i < userProductAttributes.Length; i++)
                {
                    node = root.GetElementsByTagName(userProductAttributes[i]).Item(0);
                    if (node == null)
                    {
                        userProductValues[i] = "";
                    }
                    else
                    {
                        userProductValues[i] = node.InnerText;
                    }
                }
                node = root.GetElementsByTagName("Area").Item(0);
                if(node != null)
                {
                    userProductAttributes[10] = node.ChildNodes[0].InnerText;
                    userProductAttributes[11] = node.ChildNodes[1].InnerText;
                    userProductAttributes[12] = node.ChildNodes[2].InnerText;
                    userProductAttributes[13] = node.ChildNodes[3].InnerText;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("读取元数据信息失败！" + ex.ToString());
            }
        }

        public override void ImportData(IDbBaseUtilities sqlBase)
        {
            try
            {
                //TableLocker dblock = new TableLocker(sqlBase);
                Constant.IdbOperating.LockTable("ipdb_userproduct",EnumDBType.MIDB);
                tablecode_Dal tablecode = new tablecode_Dal(sqlBase);
                ID = sqlBase.GetMaxID("ID", "ipdb_userproduct");
                QRST_CODE = tablecode.GetDataQRSTCode("ipdb_userproduct", ID);

                StringBuilder strSql = new StringBuilder();
                strSql.Append("insert into ipdb_userproduct(");
                strSql.Append(
                    "ID,MainProductID,ProductName,ProductNameCh,Mode,OrderSource,Operator,OrderSubmitTime,Satellite,Sensors,Resolution,East,West,South,North,QRST_CODE)");
                strSql.Append(" values (");
                strSql.Append(string.Format(
                    "{0},'{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}',{11},{12},{13},{14},'{15}')", ID,
                    MainProductID, ProductName, ProductNameCh, Mode, OrderSource, Operator, OrderSubmitTime.ToString("yyyy-MM-dd HH:mm:ss"), Satellite,
                    Sensors, Resolution, East, West, South, North, QRST_CODE));
                //strSql.Append(
     //               "@ID,@MainProductID,@ProductName,@ProductNameCh,@Mode,@OrderSource,@Operator,@OrderSubmitTime,@Satellite,@Sensors,@Resolution,@East,@West,@South,@North,@QRST_CODE)");
     //           MySqlParameter[] parameters = {
					//new MySqlParameter("@ID", MySqlDbType.Int32,9),
					//new MySqlParameter("@MainProductID", MySqlDbType.VarChar,15),
					//new MySqlParameter("@ProductName", MySqlDbType.VarChar,50),
					//new MySqlParameter("@ProductNameCh", MySqlDbType.VarChar,200),
					//new MySqlParameter("@Mode", MySqlDbType.VarChar,50),
					//new MySqlParameter("@OrderSource", MySqlDbType.VarChar,50),
					//new MySqlParameter("@Operator", MySqlDbType.VarChar,50),
					//new MySqlParameter("@OrderSubmitTime", MySqlDbType.DateTime),
					//new MySqlParameter("@Satellite", MySqlDbType.VarChar,50),
					//new MySqlParameter("@Sensors", MySqlDbType.VarChar,50),
					//new MySqlParameter("@Resolution", MySqlDbType.VarChar,45),
					//new MySqlParameter("@East", MySqlDbType.Decimal,10),
					//new MySqlParameter("@West", MySqlDbType.Decimal,10),
					//new MySqlParameter("@South", MySqlDbType.Decimal,10),
					//new MySqlParameter("@North", MySqlDbType.Decimal,10),
					//new MySqlParameter("@QRST_CODE", MySqlDbType.VarChar,45)};
     //           parameters[0].Value = ID;
     //           parameters[1].Value = MainProductID;
     //           parameters[2].Value = ProductName;
     //           parameters[3].Value = ProductNameCh;
     //           parameters[4].Value = Mode;
     //           parameters[5].Value = OrderSource;
     //           parameters[6].Value = Operator;
     //           parameters[7].Value = OrderSubmitTime;
     //           parameters[8].Value = Satellite;
     //           parameters[9].Value = Sensors;
     //           parameters[10].Value = Resolution;
     //           parameters[11].Value = East;
     //           parameters[12].Value = West;
     //           parameters[13].Value = South;
     //           parameters[14].Value = North;
     //           parameters[15].Value = QRST_CODE;

                sqlBase.ExecuteSql(strSql.ToString());
                Constant.IdbOperating.UnlockTable("ipdb_userproduct",EnumDBType.MIDB);
            }
            catch(Exception ex)
            {
                throw new Exception("元数据导入失败" + ex.ToString());
            }

        }

    }
}
