using System;
using System.Collections.Generic;
using System.Linq;
using QRST_DI_SS_Basis.MetadataQuery;
using QRST_DI_DS_MetadataQuery.QueryConditionParameter;
using System.Data;
using DotSpatial.Data;
using DotSpatial.Topology;
using QRST_DI_DS_MetadataQuery.PagingQuery;
using QRST_DI_SS_DBInterfaces.IDBService;
using QRST_DI_TS_Basis.DirectlyAddress;
 
namespace QRST_DI_MS_Desktop.QueryInner
{
    public class TileDataPagingQuery:IPagingQuery
    {
        IQDB_Searcher_Tile sqliteclient;
        DataTilePara tilePara;
        private int allcount = 0;
        public TileDataPagingQuery(IQDB_Searcher_Tile _sqliteclient,DataTilePara _tilePara)
        {
            sqliteclient = _sqliteclient;
            tilePara = _tilePara;
        }

        public int GetTotalRecordNum()
        {
            return allcount;
            //int allCount;
            //if(tilePara.ColAndRow[0]=="")
            //    sqliteclient.SearTilePaged1(tilePara.spacialPara, new int[] { int.Parse(tilePara.timePara[0]), int.Parse(tilePara.timePara[1]) }, tilePara.satelliteType, tilePara.sensorType, tilePara.dataTileType, tilePara.level, tilePara.otherQuery, 0, 1, out allCount);
            //else
            //    sqliteclient.SearTilePagedBaseColAndRow(tilePara.ColAndRow, new int[] { int.Parse(tilePara.timePara[0]), int.Parse(tilePara.timePara[1]) }, tilePara.satelliteType, tilePara.sensorType, tilePara.dataTileType, tilePara.level,tilePara.otherQuery, 0, 1, out allCount);
            //return allCount;
        }

        public DataTable GetCurrentPageData(int startIndex, int length)
        {
            System.Data.DataSet ds;
            //DataSet ds = sqliteclient.SearTilePaged(tilePara.spacialPara, new int[] { int.Parse(tilePara.timePara[0]), int.Parse(tilePara.timePara[1]) }, tilePara.satelliteType, tilePara.sensorType, tilePara.dataTileType, tilePara.level, startIndex, length, out allCount);
            List<Coordinate> list = new List<Coordinate>();
            if (ComplexCondition.QueryGeometry != null)
            {
                list = ComplexCondition.QueryGeometry.Coordinates.ToList();
            }
            List<double[]> lds = new List<double[]>();
            foreach (Coordinate item in list)
            {
                lds.Add(item.ToArray());
            }

            //Coordinate[] coordinate = QRST_DI_DS_MetadataQuery.ComplexCondition.QueryGeometry.Coordinates.ToArray();
            if (tilePara.ColAndRow[0] == "")
            {
                if (lds.Count > 0)
                {
                    string s = UserInterfaces.ruc3DSearcher.rule.ToString();
                    ds = sqliteclient.SearFliterTilePaged1(lds, s, tilePara.spacialPara.ToList(),
                        new List<int> {int.Parse(tilePara.timePara[0]), int.Parse(tilePara.timePara[1])},
                        tilePara.satelliteType.ToList(), tilePara.sensorType.ToList(), tilePara.dataTileType.ToList(),
                        tilePara.level.ToList(), tilePara.otherQuery, out allcount, startIndex, length);
                }
                else
                {
                    ds = sqliteclient.SearTilePaged1(tilePara.spacialPara.ToList(),
                        new List<int> {int.Parse(tilePara.timePara[0]), int.Parse(tilePara.timePara[1])},
                        tilePara.satelliteType.ToList(), tilePara.sensorType.ToList(), tilePara.dataTileType.ToList(),
                        tilePara.level.ToList(), tilePara.otherQuery, out allcount, startIndex, length);
                }
            }
            else
                ds = sqliteclient.SearTilePagedBaseColAndRow(tilePara.ColAndRow.ToList(),
                    new List<int> {int.Parse(tilePara.timePara[0]), int.Parse(tilePara.timePara[1])},
                    tilePara.satelliteType.ToList(), tilePara.sensorType.ToList(), tilePara.dataTileType.ToList(),
                    tilePara.level.ToList(), tilePara.otherQuery, out allcount, startIndex, length);

            //if (ds != null && ds.Tables.Count > 0)
            //{
            //    System.Data.DataTable tab = ds.Tables[0];

            //    if (QRST_DI_DS_MetadataQuery.ComplexCondition._usingGeometry)
            //    {
            //        tab = GeometryFilter(tab);
            //    }
            //    return tab;
            //}
            //else
            //{
            //    return null;
            //}
            return ds.Tables[0];
        }
        private DataTable GeometryFilter(DataTable tab)
        {
            IFeature geo = ComplexCondition.QueryGeometry;
            if (geo == null)
            {
                return tab;
            }

            if(UserInterfaces.ruc3DSearcher.rule ==QRST_DI_SS_Basis.MetadataQuery.Rule.Intersect)
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
            else if (UserInterfaces.ruc3DSearcher.rule == QRST_DI_SS_Basis.MetadataQuery.Rule.Contain)
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
            string row = dr["ROW"].ToString();
            string col = dr["COL"].ToString();
            string lv = dr["LEVEL"].ToString();
            string[] rowAndColum =  { row, col };

            //int flag = -1;
            //foreach (DataColumn dc in dr.Table.Columns)
            //{
            //    if (dc.Caption.Contains("经度") || dc.Caption.Contains("纬度"))
            //    {
            //        flag = 1;
            //        break;
            //    }
            //    else if (dc.Caption.Contains("数据范围"))
            //    {
            //        flag = 0;
            //        break;
            //    }
            //}


            List<Coordinate> coords = new List<Coordinate>();

            //if (flag == 1)
            //{
            /// <summary> 
            /// 最小纬度，最小经度，最大纬度，最大经度 
       
            double lulat = Convert.ToDouble(DirectlyAddressing.GetLatAndLong(rowAndColum, lv)[2].ToString());
            double lulon = Convert.ToDouble(DirectlyAddressing.GetLatAndLong(rowAndColum, lv)[1].ToString());
            double rulat = Convert.ToDouble(DirectlyAddressing.GetLatAndLong(rowAndColum, lv)[2].ToString());
            double rulon = Convert.ToDouble(DirectlyAddressing.GetLatAndLong(rowAndColum, lv)[3].ToString());
            double rdlat = Convert.ToDouble(DirectlyAddressing.GetLatAndLong(rowAndColum, lv)[0].ToString());
            double rdlon = Convert.ToDouble(DirectlyAddressing.GetLatAndLong(rowAndColum, lv)[3].ToString());
            double ldlat = Convert.ToDouble(DirectlyAddressing.GetLatAndLong(rowAndColum, lv)[0].ToString());
            double ldlon = Convert.ToDouble(DirectlyAddressing.GetLatAndLong(rowAndColum, lv)[1].ToString());

            coords.Add(new Coordinate(lulon, lulat));
            coords.Add(new Coordinate(rulon, rulat));
            coords.Add(new Coordinate(rdlon, rdlat));
            coords.Add(new Coordinate(ldlon, ldlat));
            coords.Add(new Coordinate(lulon, lulat));

            //}
            //else if (flag == 0)
            //{
            //    double maxlat = Convert.ToDouble(dr["数据范围上"].ToString());
            //    double minlat = Convert.ToDouble(dr["数据范围下"].ToString());
            //    double minlon = Convert.ToDouble(dr["数据范围左"].ToString());
            //    double maxlon = Convert.ToDouble(dr["数据范围右"].ToString());

            //    coords.Add(new Coordinate(minlon, maxlat));
            //    coords.Add(new Coordinate(maxlon, maxlat));
            //    coords.Add(new Coordinate(maxlon, minlat));
            //    coords.Add(new Coordinate(minlon, minlat));
            //    coords.Add(new Coordinate(minlon, maxlat));

            //}

            IGeometry poly = new Polygon(coords);
            return poly;

        }
    }
}
