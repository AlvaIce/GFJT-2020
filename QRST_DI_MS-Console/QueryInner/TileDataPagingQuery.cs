using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using QRST_DI_DS_MetadataQuery.PagingQuery;
using QRST_DI_DS_MetadataQuery.QueryConditionParameter;
using System.Data;

namespace QRST_DI_MS_Console.QueryInner
{
    public class TileDataPagingQuery:IPagingQuery
    {
        localhostSqlite.Service sqliteclient;
        DataTilePara tilePara;

        public TileDataPagingQuery(localhostSqlite.Service _sqliteclient,DataTilePara _tilePara)
        {
            sqliteclient = _sqliteclient;
            tilePara = _tilePara;
        }

        public int GetTotalRecordNum()
        {
            int allCount;
			if(tilePara.ColAndRow[0]=="")
				sqliteclient.SearTilePaged1(tilePara.spacialPara, new int[] { int.Parse(tilePara.timePara[0]), int.Parse(tilePara.timePara[1]) }, tilePara.satelliteType, tilePara.sensorType, tilePara.dataTileType, tilePara.level, tilePara.otherQuery, 0, 1, out allCount);
            else
				sqliteclient.SearTilePagedBaseColAndRow(tilePara.ColAndRow, new int[] { int.Parse(tilePara.timePara[0]), int.Parse(tilePara.timePara[1]) }, tilePara.satelliteType, tilePara.sensorType, tilePara.dataTileType, tilePara.level,tilePara.otherQuery, 0, 1, out allCount);
			return allCount;
        }

        public DataTable GetCurrentPageData(int startIndex, int length)
        {
            int allCount;
			DataSet ds;
            //DataSet ds = sqliteclient.SearTilePaged(tilePara.spacialPara, new int[] { int.Parse(tilePara.timePara[0]), int.Parse(tilePara.timePara[1]) }, tilePara.satelliteType, tilePara.sensorType, tilePara.dataTileType, tilePara.level, startIndex, length, out allCount);
			if(tilePara.ColAndRow[0]=="")
				ds = sqliteclient.SearTilePaged1(tilePara.spacialPara, new int[] { int.Parse(tilePara.timePara[0]), int.Parse(tilePara.timePara[1]) }, tilePara.satelliteType, tilePara.sensorType, tilePara.dataTileType, tilePara.level,tilePara.otherQuery, startIndex, length, out allCount);
			else
				ds = sqliteclient.SearTilePagedBaseColAndRow(tilePara.ColAndRow, new int[] { int.Parse(tilePara.timePara[0]), int.Parse(tilePara.timePara[1]) }, tilePara.satelliteType, tilePara.sensorType, tilePara.dataTileType, tilePara.level, tilePara.otherQuery, startIndex, length, out allCount);
			if (ds != null && ds.Tables.Count> 0)
            {
                return ds.Tables[0];
            }
            else
            {
                return null;
            }
        }
    }
}
