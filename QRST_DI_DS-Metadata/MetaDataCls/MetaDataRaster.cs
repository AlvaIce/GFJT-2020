using System;
using System.Globalization;
using System.Data;
using System.Data.OleDb;
using System.IO;

namespace QRST_DI_DS_Metadata.MetaDataCls
{
    public class MetaDataRaster : MetaData
    {
        public string[] rasterAttributeNames ={
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
                                "RESOLUTION",
                                "BANDNUM",
                                "DATAFILE",
                                "SCENEYEAR",
                                "CORRECTREF",
                                "METAPRODUCEDATE",
                                "METAPRODUCEORG",
                                "METAPRODUCTOR",
                                "REMARK",
                                
                                //以后扩展的属性放在这里
                            };
        public string[] rasterAttributeValues;
        /// <summary>
        /// 属性字段名
        /// </summary>

        #region Properties
        private string metaName;
        //
        public string MetaName
        {
            get { return rasterAttributeValues[0]; }
            set { rasterAttributeValues[0] = value; }
        }

        private string productName;
        //
        public string ProductName
        {
            get { return rasterAttributeValues[1]; }
            set { rasterAttributeValues[1] = value; }
        }

        private DateTime produceDate;
        //
        public DateTime ProduceDate
        {
            get
            {
                DateTime dt = Convert.ToDateTime(rasterAttributeValues[2], CultureInfo.CurrentCulture);
                return dt;
            }
            set { rasterAttributeValues[2] = value.ToString(); }
        }

        private string produceorg;
        //
        public string Produceorg
        {
            get { return rasterAttributeValues[3]; }
            set { rasterAttributeValues[3] = value; }
        }

        private string dataName;
        //
        public string DataName
        {
            get { return rasterAttributeValues[4]; }
            set { rasterAttributeValues[4] = value; }
        }

        private string dataSource;
        //
        public string DataSource
        {
            get { return rasterAttributeValues[5]; }
            set { rasterAttributeValues[5] = value; }
        }

        private string dataType;
        //
        public string DataType
        {
            get { return rasterAttributeValues[6]; }
            set { rasterAttributeValues[6] = value; }
        }

        private double extentUp;
        //
        public double ExtentUp
        {
            get { return double.Parse(rasterAttributeValues[7]); }
            set { rasterAttributeValues[7] = value.ToString(); }
        }

        private double extentDown;
        //
        public double ExtentDown
        {
            get { return double.Parse(rasterAttributeValues[8]); }
            set { rasterAttributeValues[8] = value.ToString(); }
        }

        private double extentLeft;
        //
        public double ExtentLeft
        {
            get { return double.Parse(rasterAttributeValues[9]); }
            set { rasterAttributeValues[9] = value.ToString(); }
        }

        private double extentRight;
        //
        public double ExtentRight
        {
            get { return double.Parse(rasterAttributeValues[10]); }
            set { rasterAttributeValues[10] = value.ToString(); }
        }

        private string dataSize;
        //
        public string DataSize
        {
            get { return rasterAttributeValues[11]; }
            set { rasterAttributeValues[11] = value; }
        }

        private string dataFormat;
        //
        public string DataFormat
        {
            get { return rasterAttributeValues[12]; }
            set { rasterAttributeValues[12] = value; }
        }

        private string mapProjectPara;
        //
        public string MapProjectPara
        {
            get { return rasterAttributeValues[13]; }
            set { rasterAttributeValues[13] = value; }
        }

        private string coordinate;
        //
        public string Coordinate
        {
            get { return rasterAttributeValues[14]; }
            set { rasterAttributeValues[14] = value; }
        }

        private string zoneNo;
        //
        public string ZoneNo
        {
            get { return rasterAttributeValues[15]; }
            set { rasterAttributeValues[15] = value; }
        }

        private string security;
        //
        public string Security
        {
            get { return rasterAttributeValues[16]; }
            set { rasterAttributeValues[16] = value; }
        }

        private string dataQulity;
        //
        public string DataQulity
        {
            get { return rasterAttributeValues[17]; }
            set { rasterAttributeValues[17] = value; }
        }

        private string resolution;
        //
        public string Resolution
        {
            get { return rasterAttributeValues[18]; }
            set { rasterAttributeValues[18] = value; }
        }

        private string bandNum;
        //
        public string BandNum
        {
            get { return rasterAttributeValues[19]; }
            set { rasterAttributeValues[19] = value; }
        }

        private string dataFile;
        //
        public string DataFile
        {
            get { return rasterAttributeValues[20]; }
            set { rasterAttributeValues[20] = value; }
        }

        private string sceneYear;
        //
        public string SceneYear
        {
            get { return rasterAttributeValues[21]; }
            set { rasterAttributeValues[21] = value; }
        }

        private string correctRef;
        //
        public string CorrectRef
        {
            get { return rasterAttributeValues[22]; }
            set { rasterAttributeValues[22] = value; }
        }

        private string mataProduceData;
        //
        public string MetaProduceData
        {
            get { return rasterAttributeValues[23]; }
            set { rasterAttributeValues[23] = value; }
        }

        private string metaProduceorg;
        //
        public string MetaProduceorg
        {
            get { return rasterAttributeValues[24]; }
            set { rasterAttributeValues[24] = value; }
        }

        private string metaProductor;
        //
        public string MetaProductor
        {
            get { return rasterAttributeValues[25]; }
            set { rasterAttributeValues[25] = value; }
        }

        private string remark;
        //
        public string Remark
        {
            get { return rasterAttributeValues[26]; }
            set { rasterAttributeValues[26] = value; }
        }




        #endregion

        #region Method

        public MetaDataRaster()
        {
            _dataType = EnumMetadataTypes.RASTER;
            rasterAttributeValues = new string[rasterAttributeNames.Length];
        }
        /// <summary>
        /// 读取Excel数据中元数据
        /// </summary>
        /// <param name="fileName">文件名</param>
        /// <param name="otherParameters">其它属性值</param>
        /// <param name="output_attribute_string">输出属性值</param>
        public void readRasterAttribute(string fileName)
        {

            DataTable dt = GetExcelDataSet(fileName, "Sheet1").Tables[0];

            //string[] attributeValues = new string[vectorAttributeNames.Length];

            rasterAttributeValues[0] = dt.Rows[0]["元数据名称"].ToString();
            rasterAttributeValues[1] = dt.Rows[0]["产品名称"].ToString();
            rasterAttributeValues[2] = dt.Rows[0]["产品生产日期"].ToString();
            rasterAttributeValues[3] = dt.Rows[0]["产品生产单位"].ToString();
            rasterAttributeValues[4] = dt.Rows[0]["数据名称"].ToString();
            rasterAttributeValues[5] = dt.Rows[0]["数据来源"].ToString();
            rasterAttributeValues[6] = dt.Rows[0]["数据类型"].ToString();
            rasterAttributeValues[7] = dt.Rows[0]["数据范围（up）"].ToString();
            rasterAttributeValues[8] = dt.Rows[0]["数据范围（down）"].ToString();
            rasterAttributeValues[9] = dt.Rows[0]["数据范围（left）"].ToString();
            rasterAttributeValues[10] = dt.Rows[0]["数据范围（right）"].ToString();
            rasterAttributeValues[11] = dt.Rows[0]["数据量"].ToString();
            rasterAttributeValues[12] = dt.Rows[0]["数据格式"].ToString();
            rasterAttributeValues[13] = dt.Rows[0]["地图投影参数"].ToString();
            rasterAttributeValues[14] = dt.Rows[0]["坐标系"].ToString();
            rasterAttributeValues[15] = dt.Rows[0]["度带号"].ToString();
            rasterAttributeValues[16] = dt.Rows[0]["密级"].ToString();
            rasterAttributeValues[17] = dt.Rows[0]["数据质量"].ToString();
            rasterAttributeValues[18] = dt.Rows[0]["影像分辨率"].ToString();
            rasterAttributeValues[19] = dt.Rows[0]["影像波段数"].ToString();
            rasterAttributeValues[20] = dt.Rows[0]["数据源"].ToString();
            rasterAttributeValues[21] = dt.Rows[0]["成像时间"].ToString();
            try
            {
                rasterAttributeValues[22] = dt.Rows[0]["纠正参考"].ToString();
            }
            catch (System.Exception ex)
            {
                rasterAttributeValues[22] = "";
            }
            rasterAttributeValues[23] = dt.Rows[0]["元数据制作日期"].ToString();
            rasterAttributeValues[24] = dt.Rows[0]["元数据制作单位"].ToString();
            rasterAttributeValues[25] = dt.Rows[0]["元数据制作人"].ToString();
            rasterAttributeValues[26] = dt.Rows[0]["备注"].ToString();
        }

        /// <summary>
        /// 根据excel获取dataset
        /// </summary>
        /// <param name="path">excel路径</param>
        /// <param name="tname">数据薄名称</param>
        /// <returns>dataset</returns>
        private DataSet GetExcelDataSet(string path, string tname)
        {
            /*Office 2007*/
            string ace = "Microsoft.ACE.OLEDB.12.0";
            /*Office 97 - 2003*/
            string jet = "Microsoft.Jet.OLEDB.4.0";
            string xl2007 = "Excel 12.0 Xml";
            string xl2003 = "Excel 8.0";
            string imex = "IMEX=1";
            string hdr = "Yes";
            string conn = "Provider={0};Data Source={1};Extended Properties=\"{2};HDR={3};{4}\";";
            string select = string.Format("SELECT * FROM [{0}$]", tname);

            //string select = sql;
            string ext = Path.GetExtension(path);
            OleDbDataAdapter oda;
            DataSet ds = new DataSet();
            switch (ext.ToLower())
            {
                case ".xlsx":
                    conn = String.Format(conn, ace, Path.GetFullPath(path), xl2007, hdr, imex);
                    break;
                case ".xls":
                    conn = String.Format(conn, jet, Path.GetFullPath(path), xl2003, hdr, imex);
                    break;
                default:
                    throw new Exception("File Not Supported!");
            }
            OleDbConnection con = new OleDbConnection(conn);
            con.Open();
            //select = string.Format(select, sql);
            oda = new OleDbDataAdapter(select, con);
            oda.Fill(ds, "MetaData");
            con.Close();

            return ds;
        }

        #endregion
    }
}
