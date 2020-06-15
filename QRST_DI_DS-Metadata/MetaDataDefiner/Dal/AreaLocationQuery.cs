using System.Collections.Generic;
using System.Data;
using QRST_DI_Resources;
using QRST_DI_SS_DBInterfaces.IDBEngine;

namespace QRST_DI_DS_Metadata.MetaDataDefiner.Dal
{
    public class AreaLocationQuery
    {
        private IDbBaseUtilities baseUtilities;

        public AreaLocationQuery(IDbBaseUtilities _baseUtilities)
        {
            baseUtilities = _baseUtilities;
        }

        public List<string> GetProvinceLst()
        {
            List<string> provincesLst = new List<string>();
            string sql = "select distinct province from arealocation ";
            DataSet ds = baseUtilities.GetDataSet(sql);
            for (int i = 0; i < ds.Tables[0].Rows.Count;i++ )
            {
                provincesLst.Add(ds.Tables[0].Rows[i][0].ToString());
            }
            return provincesLst;
        }

        public List<string> GetCityLstByProvince(string province)
        {
            List<string> cityLst = new List<string>();
            string sql = string.Format("select distinct city from arealocation where province = '{0}' ",province);
            DataSet ds = baseUtilities.GetDataSet(sql);
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                cityLst.Add(ds.Tables[0].Rows[i][0].ToString());
            }
            return cityLst;
        }

        public List<string> GetCountyLstByProvinceCity(string province,string city)
        {
            List<string> countyLst = new List<string>();
            string sql = string.Format("select distinct county from arealocation where province = '{0}' and city = '{1}' ", province,city);
            DataSet ds = baseUtilities.GetDataSet(sql);
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                countyLst.Add(ds.Tables[0].Rows[i][0].ToString());
            }
            return countyLst;
        }

        public double[] GetSpacialInfo(string province,string city,string county)
        {
            string querySql = string.Format(" select maxx, maxy, minx, miny from arealocation where province = '{0}' and city = '{1}' and county = '{2}'",province,city,county);
            DataSet ds = baseUtilities.GetDataSet(querySql);
            if (ds == null ||ds.Tables.Count==0|| ds.Tables[0].Rows.Count < 1)
            {
                return null;
            }
            else
            {
                double[] extent = new double[4];
                extent[0] =double.Parse(ds.Tables[0].Rows[0]["maxx"].ToString());
                extent[1] = double.Parse(ds.Tables[0].Rows[0]["maxy"].ToString());
                extent[2] = double.Parse(ds.Tables[0].Rows[0]["minx"].ToString());
                extent[3] = double.Parse(ds.Tables[0].Rows[0]["miny"].ToString());
                return extent;
            }
        }

        public double[] GetSpacialInfo(string province, string city)
        {//select max(maxx) as maxx,max(maxy) as maxy,min(minx) as minx,min(miny) as miny from arealocation
            string querySql = string.Format("select max(maxx) as maxx,max(maxy) as maxy,min(minx) as minx,min(miny) as miny from arealocation where province = '{0}' and city = '{1}' ", province, city);
            DataSet ds = baseUtilities.GetDataSet(querySql);
            if (ds == null || ds.Tables[0].Rows.Count < 1)
            {
                return null;
            }
            else
            {
                double[] extent = new double[4];
                extent[0] = double.Parse(ds.Tables[0].Rows[0]["maxx"].ToString());
                extent[1] = double.Parse(ds.Tables[0].Rows[0]["maxy"].ToString());
                extent[2] = double.Parse(ds.Tables[0].Rows[0]["minx"].ToString());
                extent[3] = double.Parse(ds.Tables[0].Rows[0]["miny"].ToString());
                return extent;
            }
        }

        public double[] GetSpacialInfo(string province)
        {
            string querySql = string.Format("select max(maxx) as maxx,max(maxy) as maxy,min(minx) as minx,min(miny) as miny from arealocation where province = '{0}' ", province);
            DataSet ds = baseUtilities.GetDataSet(querySql);
            if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count < 1)
            {
                return null;
            }
            else
            {
                double[] extent = new double[4];
                extent[0] = double.Parse(ds.Tables[0].Rows[0]["maxx"].ToString());
                extent[1] = double.Parse(ds.Tables[0].Rows[0]["maxy"].ToString());
                extent[2] = double.Parse(ds.Tables[0].Rows[0]["minx"].ToString());
                extent[3] = double.Parse(ds.Tables[0].Rows[0]["miny"].ToString());
                return extent;
            }
        }
    }
}
