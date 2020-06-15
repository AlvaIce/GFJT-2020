using System;
using System.Collections.Generic;
using System.Text;
using System.Globalization;
using System.Data;
using System.IO;
using System.Data.OleDb;
using QRST_DI_DS_Metadata.MetaDataDefiner.Mdl;
using QRST_DI_DS_Metadata.MetaDataDefiner.Dal;
using DotSpatial.Data;
using log4net;
using QRST_DI_Resources;
using QRST_DI_SS_Basis;
using QRST_DI_SS_Basis.MetaData;
using QRST_DI_SS_DBInterfaces.IDBEngine;

namespace QRST_DI_DS_Metadata.MetaDataCls
{
    public class MetaDataVector : MetaData
    {
        log4net.ILog log = LogManager.GetLogger(typeof(MetaDataVector));
        static string _metaNode = "基础矢量数据";
        static string _groupcode = "";
        public static string GetDefaultGroupCode(IDbBaseUtilities sqlBase)
        {
            if (_groupcode != "")
            {
                return _groupcode;
            }
            else
            {

                string sql = string.Format("select Group_Code from metadatacatalognode where Name = '{0}';", _metaNode);
                System.Data.DataSet ds = sqlBase.GetDataSet(sql);
                try
                {
                    return ds.Tables[0].Rows[0][0].ToString();
                }
                catch { }
                return "";
            }
        }

        public string[] vectorAttributeNames ={
                                "NAME",           //
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
                                "REFIMGTIME",
                                "REFIMGRESOLUTION",
                                //以后扩展的属性放在这里
                                "SDE"
                            };
        public string[] vectorAttributeValues;
        /// <summary>
        /// 属性字段名
        /// </summary>

        #region Properties
        public List<metadatacatalognode_Mdl> nodeLst;

        private string name;
        //
        public string Name
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
            get {
                DateTime dt= Convert.ToDateTime(vectorAttributeValues[2], CultureInfo.CurrentCulture);
                return dt; }
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

        private string refImgTime;
        //
        public string RefImgTime
        {
            get { return vectorAttributeValues[23]; }
            set { vectorAttributeValues[23] = value; }
        }

        private string refImgResolution;
        //
        public string RefImgResolution
        {
            get { return vectorAttributeValues[24]; }
            set { vectorAttributeValues[24] = value; }
        }

        public string GroupCode
        {
            get { return vectorAttributeValues[25]; }
            set { vectorAttributeValues[25] = value; }
        }

        public string SDE
        {
            get { return vectorAttributeValues[26]; }
            set { vectorAttributeValues[26] = value; }
        }
        #endregion

        #region Method

        public MetaDataVector()
        {
            _dataType = EnumMetadataTypes.VECTOR;
            vectorAttributeValues = new string[vectorAttributeNames.Length];
        }
        /// <summary>
        /// 读取Vector数据中元数据
        /// </summary>
        /// <param name="fileName">文件名</param>
        /// <param name="otherParameters">其它属性值</param>
        /// <param name="output_attribute_string">输出属性值</param>
        public void readVectorAttribute(string fileName, string[] otherParameters, out string[] outputAttributeValues, int index)
        {
            DataTable dt;
            try
            {
                dt = GetExcelDataSet(fileName, "Sheet1").Tables[0];

                //string[] attributeValues = new string[vectorAttributeNames.Length];

                vectorAttributeValues[0] = dt.Rows[index]["元数据名称"].ToString();
                vectorAttributeValues[1] = dt.Rows[index]["产品名称"].ToString();
                vectorAttributeValues[2] = dt.Rows[index]["产品生产日期"].ToString();
                vectorAttributeValues[3] = dt.Rows[index]["产品生产单位"].ToString();
                vectorAttributeValues[4] = dt.Rows[index]["数据名称"].ToString();
                vectorAttributeValues[5] = dt.Rows[index]["数据来源"].ToString();
                vectorAttributeValues[6] = dt.Rows[index]["数据类型"].ToString();
                vectorAttributeValues[7] = dt.Rows[index]["数据范围（up）"].ToString();
                vectorAttributeValues[8] = dt.Rows[index]["数据范围（down）"].ToString();
                vectorAttributeValues[9] = dt.Rows[index]["数据范围（left）"].ToString();
                vectorAttributeValues[10] = dt.Rows[index]["数据范围（right）"].ToString();
                vectorAttributeValues[11] = dt.Rows[index]["数据量"].ToString();
                vectorAttributeValues[12] = dt.Rows[index]["数据格式"].ToString();
                vectorAttributeValues[13] = dt.Rows[index]["地图投影参数"].ToString();
                vectorAttributeValues[14] = dt.Rows[index]["坐标系"].ToString();
                vectorAttributeValues[15] = dt.Rows[index]["度带号"].ToString();
                vectorAttributeValues[16] = dt.Rows[index]["密级"].ToString();
                vectorAttributeValues[17] = dt.Rows[index]["数据质量"].ToString();
                vectorAttributeValues[18] = dt.Rows[index]["地图比例尺"].ToString();
                vectorAttributeValues[19] = dt.Rows[index]["元数据制作日期"].ToString();           //数据上传日期
                vectorAttributeValues[20] = dt.Rows[index]["元数据制作单位"].ToString();       //本单位
                vectorAttributeValues[21] = dt.Rows[index]["元数据制作人"].ToString();        //当前登入用户
                vectorAttributeValues[22] = dt.Rows[index]["备注"].ToString();
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
            try
            {
                vectorAttributeValues[23] = dt.Rows[index]["参考影像时间"].ToString();
            }
            catch (System.Exception ex)
            {//没有这个字段
                vectorAttributeValues[23] = "";
            }
            try
            {
                vectorAttributeValues[24] = dt.Rows[index]["参考影像分辨率"].ToString();
            }
            catch (System.Exception ex)
            {//没有这个字段
                vectorAttributeValues[24] = "";
            }
            //设置groupcode
            vectorAttributeValues[25] = otherParameters[0];
            //设置SDE
            vectorAttributeValues[26] = otherParameters[1];

            outputAttributeValues = vectorAttributeValues;
        }

        /// <summary>
        /// 根据excel获取dataset
        /// </summary>
        /// <param name="path">excel路径</param>
        /// <param name="tname">数据薄名称</param>
        /// <returns>dataset</returns>
        private System.Data.DataSet GetExcelDataSet(string path, string tname)
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
            System.Data.DataSet ds = new System.Data.DataSet();
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

        //zxw 20131221 ,修改了矢量数据的相对路径寻址规则 
        //hxz 20161125 ,修改了矢量数据的相对路径寻址规则，解决同名同路径的问题（增加QRST_CODE）
        public override string GetRelateDataPath()
        {
            //\\192.168.2.109\zhsjk\基础空间数据库\[分支]\NAME_QRSTCODE\
            if (!string.IsNullOrEmpty(this.Name) && nodeLst != null)
            {
                string relatePath = "";
                for (int i = 0; i < nodeLst.Count; i++)
                {
                    relatePath = string.Format(@"{0}\{1}", nodeLst[i].NAME, relatePath);
                }
                relatePath = string.Format(@"{0}\{1}#{2}", relatePath, this.Name,this.QRST_CODE);
                relatePath = relatePath.TrimEnd(@"\".ToCharArray());
                relatePath = relatePath.Replace(":", "");
                return relatePath;
            }
            else
                return null;
        }

        /// <summary>
        /// 获取元数据实体，必须元数据入库后才有元数据实体
        /// </summary>
        /// <param name="qrst_code">库表中的数据QRST_CODE</param>
        /// <param name="sqlBase">数据库链接</param>
        public override void GetModel(string qrst_code, IDbBaseUtilities sqlBase)
        {
            string querySql;
            if(qrst_code.Contains("BSDB")){
                querySql = string.Format("select * from prods_vector where QRST_CODE = '{0}'", qrst_code);
            }else{
                querySql = "";
            }

            System.Data.DataSet ds = sqlBase.GetDataSet(querySql);

            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                this.Name = ds.Tables[0].Rows[0]["NAME"].ToString();
                this.QRST_CODE = qrst_code;

                metadatacatalognode_Mdl nodeMdl = new metadatacatalognode_Mdl();
                nodeMdl.GROUP_CODE = ds.Tables[0].Rows[0]["GROUPCODE"].ToString();
                metadatacatalognode_r_Dal nodeDal = new metadatacatalognode_r_Dal(sqlBase);
                nodeLst = nodeDal.GetParent(nodeMdl);

                this.IsCreated = true;
            }
        }
        public override void ReadAttributes(string fileName)
        {
            base.ReadAttributes(fileName);

            //从矢量文件中获取元数据信息
            Shapefile _sf = null;
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
                Name = Path.GetFileNameWithoutExtension(_sf.Header.Filename);
                ProductName = Path.GetFileNameWithoutExtension(_sf.Header.Filename);
                DataName = Path.GetFileName(_sf.Header.Filename);
                if (_sf.Projection != null)
                    MapProjectPara = _sf.Projection.ToString();
                if (_sf.Projection != null && _sf.Projection.GeographicInfo != null)
                    Coordinate = _sf.Projection.GeographicInfo.Name;
            }
            catch (Exception ex)
            {
                log.Error(string.Format("元数据信息读取失败：{0}；{1}", fileName, ex.ToString()));
                throw new Exception("元数据信息读取失败！");
            }
            finally
            {
                if (_sf != null)
                    _sf.Close();
            }



        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sqlBase">Universial.dbOperating.BSDB</param>
        public override void ImportData(IDbBaseUtilities sqlBase)
        {
            //TableLocker dblock = new TableLocker(sqlBase);
            Constant.IdbOperating.LockTable("prods_vector",EnumDBType.MIDB);
            tablecode_Dal tablecode = new tablecode_Dal(sqlBase);
            int id = sqlBase.GetMaxID("ID", "prods_vector");
            QRST_CODE = tablecode.GetDataQRSTCode("prods_vector", id);

            //this.GroupCode = ConfigurationManager.AppSettings["VectorGroupCode"]; 
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into prods_vector(");
            strSql.Append(
                "NAME,PRODUCTNAME,PRODUCEDATE,PRODUCEORG,DATANAME,DATASOURCE,DATATYPE,EXTENTUP,EXTENTDOWN,EXTENTLEFT,EXTENTRIGHT,DATASIZE,DATAFORMAT,MAPPROJECTPARA,COORDINATE," +
                "ZONENO,SECURITY,DATAQULITY,SCALE,METAPRODUCEDATE,METAPRODUCEORG,METAPRODUCTOR,REMARK,GROUPCODE,SDE,QRST_CODE)");
            strSql.Append(" values (");
            strSql.Append(string.Format(
                "'{0}','{1}','{2}','{3}','{4}','{5}','{6}',{7},{8},{9},{10},'{11}','{12}','{13}','{14}','{15}','{16}','{17}','{18}'," +
                "'{19}','{20}','{21}'," + "'{22}','{23}','{24}','{25}')", Name, ProductName, ProduceDate.ToString("yyyy-MM-dd HH:mm:ss"), Produceorg,
                DataName, DataSource, DataType, ExtentUp, ExtentDown, ExtentLeft, ExtentRight, DataSize, DataFormat,
                MapProjectPara, Coordinate, ZoneNo, Security, DataQulity, Scale, MetaProduceDate, MetaProduceorg,
                MetaProductor, Remark, GroupCode, SDE, QRST_CODE));

            sqlBase.ExecuteSql(strSql.ToString());
            Constant.IdbOperating.UnlockTable("prods_vector",EnumDBType.MIDB);
            
            log.Info("元数据信息导入完成!");

        }

        #endregion
    }
}
