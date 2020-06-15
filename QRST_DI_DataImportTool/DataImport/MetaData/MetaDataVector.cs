using System;
using System.Text;
using DotSpatial.Data;
using System.IO;
using MySql.Data.MySqlClient;
using log4net;
using QRST_DI_SS_DBInterfaces.IDBEngine;

namespace QRST_DI_DataImportTool.DataImport.MetaData
{
    public class MetaDataVector:MetaData
    {
        log4net.ILog log = LogManager.GetLogger(typeof(MetaDataVector));
        public string[] vectorAttributeNames ={
                                "METANAME",           //
                                "PRODUCTNAME",          //
                                "PRODUCEDATE",             //
                                "PRODUCEORG",               //
                                "DATANAME",          //
                                "DATASOURCE",              //             
                                "DATATYPE",         //  
                                "EXTENTUP",            //
                                "EXTENTDOWN",             //
                                "EXTENTLEFT",            //
                                "EXTENTRIGHT",           //
                                "DATASIZE",        //
                                "DATAFORMART",                 //
                                "MAPPROJECTPARA",    //
                                "COORDINATE",        //
                                "ZONENO",         //
                                "SECURITY",  //
                                "DATAQULITY",
                                "SCALE",
                                "METAPRODUCEDATE",
                                "METAPRODUCEORG",
                                "METAPRODUCTOR",
                                "REMARK",
                                "GROUPCODE",
                                //以后扩展的属性放在这里
                                "SDE"
                            };
        public string[] vectorAttributeValues;
        /// <summary>
        /// 属性字段名
        /// </summary>

        #region Properties


        private string metaName;
        //
        public string MetaName
        {
            get { return vectorAttributeValues[0]; }
            set { vectorAttributeValues[0] = value; }
        }

        private string productName;
        //
        public string ProductName
        {
            get { return vectorAttributeValues[1]; }
            set { vectorAttributeValues[1] = value; }
        }

        private DateTime produceDate;
        //
        public DateTime ProduceDate
        {
            get { return Convert.ToDateTime(vectorAttributeValues[2]); }
            set { vectorAttributeValues[2] = value.ToString(); }
        }

        private string produceorg;
        //
        public string Produceorg
        {
            get { return vectorAttributeValues[3]; }
            set { vectorAttributeValues[3] = value; }
        }

        private string dataName;
        //
        public string DataName
        {
            get { return vectorAttributeValues[4]; }
            set { vectorAttributeValues[4] = value; }
        }

        private string dataSource;
        //
        public string DataSource
        {
            get { return vectorAttributeValues[5]; }
            set { vectorAttributeValues[5] = value; }
        }

        private string dataType;
        //
        public string DataType
        {
            get { return vectorAttributeValues[6]; }
            set { vectorAttributeValues[6] = value; }
        }

        private double extentUp;
        //
        public double ExtentUp
        {
            get { return double.Parse(vectorAttributeValues[7]); }
            set { vectorAttributeValues[7] = value.ToString(); }
        }

        private double extentDown;
        //
        public double ExtentDown
        {
            get { return double.Parse(vectorAttributeValues[8]); }
            set { vectorAttributeValues[8] = value.ToString(); }
        }

        private double extentLeft;
        //
        public double ExtentLeft
        {
            get { return double.Parse(vectorAttributeValues[9]); }
            set { vectorAttributeValues[9] = value.ToString(); }
        }

        private double extentRight;
        //
        public double ExtentRight
        {
            get { return double.Parse(vectorAttributeValues[10]); }
            set { vectorAttributeValues[10] = value.ToString(); }
        }

        private string dataSize;
        //
        public string DataSize
        {
            get { return vectorAttributeValues[11]; }
            set { vectorAttributeValues[11] = value; }
        }

        private string dataFormat;
        //
        public string DataFormat
        {
            get { return vectorAttributeValues[12]; }
            set { vectorAttributeValues[12] = value; }
        }

        private string mapProjectPara;
        //
        public string MapProjectPara
        {
            get { return vectorAttributeValues[13]; }
            set { vectorAttributeValues[13] = value; }
        }

        private string coordinate;
        //
        public string Coordinate
        {
            get { return vectorAttributeValues[14]; }
            set { vectorAttributeValues[14] = value; }
        }

        private string zoneNo;
        //
        public string ZoneNo
        {
            get { return vectorAttributeValues[15]; }
            set { vectorAttributeValues[15] = value; }
        }

        private string security;
        //
        public string Security
        {
            get { return vectorAttributeValues[16]; }
            set { vectorAttributeValues[16] = value; }
        }

        private string dataQulity;
        //
        public string DataQulity
        {
            get { return vectorAttributeValues[17]; }
            set { vectorAttributeValues[17] = value; }
        }

        private string scale;
        //
        public string Scale
        {
            get { return vectorAttributeValues[18]; }
            set { vectorAttributeValues[18] = value; }
        }

        private string mataProduceData;
        //
        public string MetaProduceDate
        {
            get { return vectorAttributeValues[19]; }
            set { vectorAttributeValues[19] = value; }
        }

        private string metaProduceorg;
        //
        public string MetaProduceorg
        {
            get { return vectorAttributeValues[20]; }
            set { vectorAttributeValues[20] = value; }
        }

        private string metaProductor;
        //
        public string MetaProductor
        {
            get { return vectorAttributeValues[21]; }
            set { vectorAttributeValues[21] = value; }
        }

        private string remark;
        //
        public string Remark
        {
            get { return vectorAttributeValues[22]; }
            set { vectorAttributeValues[22] = value; }
        }


        public string GroupCode
        {
            get { return vectorAttributeValues[23]; }
            set { vectorAttributeValues[23] = value; }
        }

        public string SDE
        {
            get { return vectorAttributeValues[24]; }
            set { vectorAttributeValues[24] = value; }
        }
        #endregion

      

        public MetaDataVector()
        {
           // _dataType = EnumMetadataTypes.VECTOR;
            vectorAttributeValues = new string[vectorAttributeNames.Length];
        }

        public override void ReadAttributes(string fileName)
        {
            base.ReadAttributes(fileName);

            //从矢量文件中获取元数据信息
            Shapefile _sf=null;
            try
            {
                _sf = Shapefile.OpenFile(fileName);
                DataType = _sf.Header.ShapeType.ToString();  //数据类型：点、线、面
                ExtentUp = _sf.Header.Ymax;
                ExtentDown = _sf.Header.Ymin;
                ExtentLeft = _sf.Header.Xmin;
                ExtentRight = _sf.Header.Xmax;
                DataFormat = "Shapefile";
                DataSize = ((double)_sf.Header.FileLength / (1024.0 * 1024.0)).ToString("0.00") + "MB";
                ProduceDate = _sf.Attributes.UpdateDate;
                MetaProduceDate = _sf.Attributes.UpdateDate.ToShortDateString();
                MetaName = Path.GetFileNameWithoutExtension(_sf.Header.Filename);
                ProductName = Path.GetFileNameWithoutExtension(_sf.Header.Filename);
                DataName = Path.GetFileName(_sf.Header.Filename);
                if (_sf.Projection != null)
                    MapProjectPara = _sf.Projection.ToString();
                if (_sf.Projection != null && _sf.Projection.GeographicInfo != null)
                    Coordinate = _sf.Projection.GeographicInfo.Name;
            }
            catch (Exception ex)
            {
                log.Error(string.Format("元数据信息读取失败：{0}；{1}",fileName,ex.ToString()));
                throw new Exception("元数据信息读取失败！");
            }
            finally
            {
                if(_sf!=null)
                    _sf.Close();
            }
          
        

        }

        public override void ImportData()
        {
            IDbBaseUtilities isdbUtility = Universial.dbOperating.GetsqlBaseObj("ISDB");
            int id = isdbUtility.GetMaxID("ID", "prods_vector");
                QRST_CODE = "0001-BSDB-V-" + id;
                //this.GroupCode = ConfigurationManager.AppSettings["VectorGroupCode"]; ;
                StringBuilder strSql = new StringBuilder();
                strSql.Append("insert into prods_vector(");
                strSql.Append("METANAME,PRODUCTNAME,PRODUCEDATE,PRODUCEORG,DATANAME,DATASOURCE,DATATYPE,EXTENTUP,EXTENTDOWN,EXTENTLEFT,EXTENTRIGHT,DATASIZE,DATAFORMAT,MAPPROJECTPARA,COORDINATE,ZONENO,SECURITY,DATAQULITY,SCALE,METAPRODUCEDATE,METAPRODUCEORG,METAPRODUCTOR,REMARK,GROUPCODE,SDE,QRST_CODE)");
                strSql.Append(" values (");
                strSql.Append("@METANAME,@PRODUCTNAME,@PRODUCEDATE,@PRODUCEORG,@DATANAME,@DATASOURCE,@DATATYPE,@EXTENTUP,@EXTENTDOWN,@EXTENTLEFT,@EXTENTRIGHT,@DATASIZE,@DATAFORMAT,@MAPPROJECTPARA,@COORDINATE,@ZONENO,@SECURITY,@DATAQULITY,@SCALE,@METAPRODUCEDATE,@METAPRODUCEORG,@METAPRODUCTOR,@REMARK,@GROUPCODE,@SDE,@QRST_CODE)");
                MySqlParameter[] parameters = {
					new MySqlParameter("@METANAME", MySqlDbType.Text),
					new MySqlParameter("@PRODUCTNAME", MySqlDbType.Text),
					new MySqlParameter("@PRODUCEDATE", MySqlDbType.DateTime),
					new MySqlParameter("@PRODUCEORG", MySqlDbType.Text),
					new MySqlParameter("@DATANAME", MySqlDbType.Text),
					new MySqlParameter("@DATASOURCE", MySqlDbType.Text),
					new MySqlParameter("@DATATYPE", MySqlDbType.Text),
					new MySqlParameter("@EXTENTUP", MySqlDbType.Decimal,20),
					new MySqlParameter("@EXTENTDOWN", MySqlDbType.Decimal,20),
					new MySqlParameter("@EXTENTLEFT", MySqlDbType.Decimal,20),
					new MySqlParameter("@EXTENTRIGHT", MySqlDbType.Decimal,20),
					new MySqlParameter("@DATASIZE", MySqlDbType.Text),
					new MySqlParameter("@DATAFORMAT", MySqlDbType.Text),
					new MySqlParameter("@MAPPROJECTPARA", MySqlDbType.Text),
					new MySqlParameter("@COORDINATE", MySqlDbType.Text),
					new MySqlParameter("@ZONENO", MySqlDbType.Text),
					new MySqlParameter("@SECURITY", MySqlDbType.Text),
					new MySqlParameter("@DATAQULITY", MySqlDbType.Text),
					new MySqlParameter("@SCALE", MySqlDbType.Text),
					new MySqlParameter("@METAPRODUCEDATE", MySqlDbType.DateTime),
					new MySqlParameter("@METAPRODUCEORG", MySqlDbType.Text),
					new MySqlParameter("@METAPRODUCTOR", MySqlDbType.Text),
					new MySqlParameter("@REMARK", MySqlDbType.Text),
					new MySqlParameter("@GROUPCODE", MySqlDbType.Text),
					new MySqlParameter("@SDE", MySqlDbType.Text),
					new MySqlParameter("@QRST_CODE", MySqlDbType.VarChar,45)};
                parameters[0].Value = this.MetaName;
                parameters[1].Value = this.ProductName;
                parameters[2].Value = this.ProduceDate;
                parameters[3].Value = this.Produceorg;
                parameters[4].Value = this.DataName;
                parameters[5].Value = this.DataSource;
                parameters[6].Value = this.DataType;
                parameters[7].Value = this.ExtentUp;
                parameters[8].Value = this.ExtentDown;
                parameters[9].Value = this.ExtentLeft;
                parameters[10].Value = this.ExtentRight;
                parameters[11].Value = this.DataSize;
                parameters[12].Value = this.DataFormat;
                parameters[13].Value = this.MapProjectPara;
                parameters[14].Value = this.Coordinate;
                parameters[15].Value = this.ZoneNo;
                parameters[16].Value = this.Security;
                parameters[17].Value = this.DataQulity;
                parameters[18].Value = this.Scale;
                parameters[19].Value = this.MetaProduceDate;
                parameters[20].Value = this.MetaProduceorg;
                parameters[21].Value = this.MetaProductor;
                parameters[22].Value = this.Remark;
                parameters[23].Value = this.GroupCode;
                parameters[24].Value = this.SDE;
                parameters[25].Value = QRST_CODE;

            isdbUtility.ExecuteSql(strSql.ToString(), parameters);
                log.Info("元数据信息导入完成!");
         
           
        }

    }
}
