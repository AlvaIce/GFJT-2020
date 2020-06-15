using System.Collections.Generic;
using QRST_DI_DS_MetadataQuery.PagingQuery;
using QRST_DI_DS_MetadataQuery.QueryConditionParameter;
using System.Data;
using System.Linq;
using QRST_DI_SS_DBInterfaces.IDBService;
 
namespace QRST_DI_MS_Desktop.QueryInner
{
    public class ProductTileDataPagingQuery : IPagingQuery
    {
        IQDB_Searcher_Tile sqliteclient;
        ProductTilePara prodPara;
        private int allcount = 0;

        public ProductTileDataPagingQuery(IQDB_Searcher_Tile _sqliteclient, ProductTilePara _prodPara)
        {
            sqliteclient = _sqliteclient;
            prodPara = _prodPara;
        }

        public int GetTotalRecordNum()
        {
            return allcount;
            //int allCount;
            //sqliteclient.SearPRODTilePaged(prodPara.spacialPara, new int[] { int.Parse(prodPara.timePara[0]), int.Parse(prodPara.timePara[1]) }, prodPara.productType, prodPara.level, 0, 1, out allCount);
            //return allCount;
        }

        public DataTable GetCurrentPageData(int startIndex, int length)
        {
            DataSet ds = sqliteclient.SearPRODTilePaged(prodPara.spacialPara.ToList(),
                new List<int> {int.Parse(prodPara.timePara[0]), int.Parse(prodPara.timePara[1])},
                prodPara.productType.ToList(), prodPara.level.ToList(), out allcount, startIndex, length);
			//DataSet ds = sqliteclient.SearPRODTilePaged1(prodPara.spacialPara, new int[] { int.Parse(prodPara.timePara[0]), int.Parse(prodPara.timePara[1]) }, prodPara.productType, prodPara.level, startIndex, length, out allCount);
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
