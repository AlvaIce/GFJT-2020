/*
 * 作者：zxw
 * 创建时间：2013-09-01
 * 描述：支持mysql元数据后台分页查询
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using DotSpatial.Data;
using DotSpatial.Topology;
using QRST_DI_SS_Basis;
using QRST_DI_SS_Basis.MetadataQuery;
using Rule = QRST_DI_SS_Basis.MetadataQuery.Rule;
 
namespace QRST_DI_DS_MetadataQuery.PagingQuery
{
    public class MetaDataPagingQuery : IPagingQuery
    {
        ViewBasedQuery queryObj;

        public MetaDataPagingQuery(ViewBasedQuery _queryObj)
        {
            queryObj = _queryObj;
        }

        public int GetTotalRecordNum()
        {
            return queryObj.GetRecordCount();
        }
        public static int record = 0;
        public static Dictionary<int, DataTable> dic = new Dictionary<int, DataTable>();
        int currentP = 0;
        public DataTable GetCurrentPageData(int startIndex, int length)
        {
            queryObj.queryRequest.recordSetStartPointSpecified = startIndex;
            queryObj.queryRequest.offset = length;
            QueryResponse qr = queryObj.Query();
            int count1 = qr.recordCount;
            int c = qr.recordSet.Tables[0].Rows.Count;
            if (qr != null && qr.recordSet != null && qr.recordSet.Tables.Count > 0&&qr.recordSet.Tables[0].Rows.Count>0)
            {
                System.Data.DataTable tab = qr.recordSet.Tables[0];
                if (ComplexCondition._usingGeometry)
                {
                    tab = GeometryFilter(tab);
                    int tabCount = tab.Rows.Count;
                    if (tabCount < length && count1 == length)
                    {
                        int count = 0;
                        int num = 0;
                        while (tabCount + count != length)
                        {
                            queryObj.queryRequest.recordSetStartPointSpecified = startIndex + length + num;
                            queryObj.queryRequest.offset = 1;
                            record = queryObj.queryRequest.recordSetStartPointSpecified +1;
                            QueryResponse qresponse = queryObj.Query();
                            num++;
                            if (qresponse.recordSet.Tables[0].Rows.Count != 0)
                            {
                                System.Data.DataTable tab1 = qresponse.recordSet.Tables[0];
                                tab1 = GeometryFilter(tab1);
                                if (tab1.Rows.Count != 0)
                                {
                                    tab.Merge(tab1);
                                    count++;
                                }
                            }
                            else
                            {
                                break;
                            }
                        }

                    }
                    currentP++;
                    dic.Add(currentP, tab);
                }
               
                return tab;
            }
            else
            {
                if (qr.recordSet!=null&& qr.recordSet.Tables.Count>0)
                {
                    return qr.recordSet.Tables[0];
                }
                else
                {
                    return null;
                }
            }
        }

        public DataTable GeometryFilter(DataTable tab)
        {
            IFeature geo = ComplexCondition.QueryGeometry;
            if (geo == null)
            {
                return tab;
            }

            if (queryObj.queryRequest.complexCondition.ruleName == Rule.Intersect)
            {
                for (int i = tab.Rows.Count - 1; i > -1; i--)
                {
                    IGeometry poly = getGeomFromRow(tab.Rows[i]);
                    if (!geo.Intersects(poly))
                    {
                        tab.Rows.RemoveAt(i);
                    }
                }
                
            }
            else if (queryObj.queryRequest.complexCondition.ruleName == Rule.Contain)
            {
                for (int i = tab.Rows.Count - 1; i > -1; i--)
                {
                    IGeometry poly = getGeomFromRow(tab.Rows[i]);
                    if (!geo.Contains(poly))
                    {
                        tab.Rows.RemoveAt(i);
                    }
                }
                
            }
            return tab;
        }

        private IGeometry getGeomFromRow(DataRow dr)
        {
            int flag = -1;
            foreach (DataColumn dc in dr.Table.Columns)
            {
                if (dc.Caption.Contains("经度") || dc.Caption.Contains("纬度"))
                {
                    flag = 1;
                    break;
                }
                else if (dc.Caption.Contains("数据范围"))
                {
                    flag = 0;
                    break;
                }
            }



            List<Coordinate> coords = new List<Coordinate>();

            if (flag == 1)
            {
                double lulat = Convert.ToDouble(dr["左上纬度"].ToString());
                double lulon = Convert.ToDouble(dr["左上经度"].ToString());
                double rulat = Convert.ToDouble(dr["右上纬度"].ToString());
                double rulon = Convert.ToDouble(dr["右上经度"].ToString());
                double rdlat = Convert.ToDouble(dr["右下纬度"].ToString());
                double rdlon = Convert.ToDouble(dr["右下经度"].ToString());
                double ldlat = Convert.ToDouble(dr["左下纬度"].ToString());
                double ldlon = Convert.ToDouble(dr["左下经度"].ToString());

                coords.Add(new Coordinate(lulon, lulat));
                coords.Add(new Coordinate(rulon, rulat));
                coords.Add(new Coordinate(rdlon, rdlat));
                coords.Add(new Coordinate(ldlon, ldlat));
                coords.Add(new Coordinate(lulon, lulat));

            }
            else if (flag == 0)
            {
                double maxlat = Convert.ToDouble(dr["数据范围上"].ToString());
                double minlat = Convert.ToDouble(dr["数据范围下"].ToString());
                double minlon = Convert.ToDouble(dr["数据范围左"].ToString());
                double maxlon = Convert.ToDouble(dr["数据范围右"].ToString());

                coords.Add(new Coordinate(minlon, maxlat));
                coords.Add(new Coordinate(maxlon, maxlat));
                coords.Add(new Coordinate(maxlon, minlat));
                coords.Add(new Coordinate(minlon, minlat));
                coords.Add(new Coordinate(minlon, maxlat));

            }

            IGeometry poly = new Polygon(coords);
            return poly;

        }
    }
}
