using System;
using System.Collections.Generic;
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
    public class MetaDataRasterSpecial : MetaData
    {
        public string[] rasterAttributeNames ={
                                "TaskID",           //
                                "ProductName",          //
                                "SubsystemID",             //
                                "MethodID",               //
                                "MAXLAT",          //
                                "MINLAT",              //             
                                "MAXLON",         //  
                                "MINLON",            //
                                "SubmitTime",             //
                                "Format",            //
                                "Type",           //
                                "RenderMethod",        //
                                "Description",                 //
                                "GROUPCODE"   //
                                //"QRST_CODE",        //
                                
                                //以后扩展的属性放在这里
                            };
        public string[] rasterAttributeValues;
        /// <summary>
        /// 属性字段名
        /// </summary>

        #region Properties
        private int taskID;
        //
        public int TaskID
        {
            get { return int.Parse(rasterAttributeValues[0]); }
            set { rasterAttributeValues[0] = value.ToString(); }
        }

        private string productName;
        //
        public string ProductName
        {
            get { return rasterAttributeValues[1]; }
            set { rasterAttributeValues[1] = value; }
        }

        private int subsystemID;
        //
        public int SubsystemID
        {
            get { return int.Parse(rasterAttributeValues[2]); }
            set { rasterAttributeValues[2] = value.ToString(); }
        }

        private int methodID;
        //
        public int MethodID
        {
            get { return int.Parse(rasterAttributeValues[3]); }
            set { rasterAttributeValues[3] = value.ToString(); }
        }

        private double maxLAT;
        //
        public double MAXLAT
        {
            get { return double.Parse(rasterAttributeValues[4]); }
            set { rasterAttributeValues[4] = value.ToString(); }
        }

        private double minLAT;
        //
        public double MINLAT
        {
            get { return double.Parse(rasterAttributeValues[5]); }
            set { rasterAttributeValues[5] = value.ToString(); }
        }

        private double maxLON;
        //
        public double MAXLON
        {
            get { return double.Parse(rasterAttributeValues[6]); }
            set { rasterAttributeValues[6] = value.ToString(); }
        }

        private double minLON;
        //
        public double MINLON
        {
            get { return double.Parse(rasterAttributeValues[7]); }
            set { rasterAttributeValues[7] = value.ToString(); }
        }

        private DateTime submitTime;
        //
        public DateTime SubmitTime
        {
            get
            {
                DateTime dt = Convert.ToDateTime(rasterAttributeValues[8], CultureInfo.CurrentCulture);
                return dt;
            }
            set { rasterAttributeValues[8] = value.ToString(); }
        }

        private string format;
        //
        public string Format
        {
            get { return rasterAttributeValues[9]; }
            set { rasterAttributeValues[9] = value; }
        }

        private string type;
        //
        public string Type
        {
            get { return rasterAttributeValues[10]; }
            set { rasterAttributeValues[10] = value; }
        }

        private string renderMethod;
        //
        public string RenderMethod
        {
            get { return rasterAttributeValues[11]; }
            set { rasterAttributeValues[11] = value; }
        }


        private string description;
        //
        public string Description
        {
            get { return rasterAttributeValues[12]; }
            set { rasterAttributeValues[12] = value; }
        }

        private string groupCode;
        //
        public string GROUPCODE
        {
            get { return rasterAttributeValues[13]; }
            set { rasterAttributeValues[13] = value; }
        }




        public List<metadatacatalognode_Mdl> nodeLst;
        #endregion

        #region Method

        public MetaDataRasterSpecial()
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

            rasterAttributeValues[1] = dt.Rows[0]["数据名称"].ToString();
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

        public override string GetRelateDataPath()
        {
            //\\192.168.2.109\zhsjk\基础空间数据库\[分支]\NAME_QRSTCODE\
            if (!string.IsNullOrEmpty(this.productName) && nodeLst != null)
            {
                string relatePath = "";
                for (int i = 0; i < nodeLst.Count; i++)
                {
                    relatePath = string.Format(@"{0}\{1}", nodeLst[i].NAME, relatePath);
                }
                relatePath = string.Format(@"{0}\{1}#{2}", relatePath, this.productName, this.QRST_CODE);
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
            if (qrst_code.Contains("INDB-4"))
            {
                querySql = string.Format("select * from prod_airport_raster where QRST_CODE = '{0}'", qrst_code);
            }
            else if (qrst_code.Contains("INDB-5"))
            {
                querySql = string.Format("select * from prod_design_raster where QRST_CODE = '{0}'", qrst_code);
            }
            else if (qrst_code.Contains("INDB-6"))
            {
                querySql = string.Format("select * from prod_disaster_raster where QRST_CODE = '{0}'", qrst_code);
            }
            else if (qrst_code.Contains("INDB-7"))
            {
                querySql = string.Format("select * from prod_roadnetwork_raster where QRST_CODE = '{0}'", qrst_code);
            }
            else if (qrst_code.Contains("INDB-8"))
            {
                querySql = string.Format("select * from prod_shipping_raster where QRST_CODE = '{0}'", qrst_code);
            }
            else if (qrst_code.Contains("INDB-9"))
            {
                querySql = string.Format("select * from prod_travel_raster where QRST_CODE = '{0}'", qrst_code);
            }
            else
            {
                querySql = "";
            }

            System.Data.DataSet ds = sqlBase.GetDataSet(querySql);

            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                this.productName = ds.Tables[0].Rows[0]["ProductName"].ToString();
                this.QRST_CODE = qrst_code;

                metadatacatalognode_Mdl nodeMdl = new metadatacatalognode_Mdl();
                nodeMdl.GROUP_CODE = ds.Tables[0].Rows[0]["GROUPCODE"].ToString();
                metadatacatalognode_r_Dal nodeDal = new metadatacatalognode_r_Dal(sqlBase);
                nodeLst = nodeDal.GetParent(nodeMdl);

                this.IsCreated = true;
            }
        }
        #endregion
    }
}

