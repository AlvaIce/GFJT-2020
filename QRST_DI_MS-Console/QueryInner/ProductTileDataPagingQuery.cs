using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using QRST_DI_DS_MetadataQuery.PagingQuery;
using QRST_DI_DS_MetadataQuery.QueryConditionParameter;
using System.Data;

namespace QRST_DI_MS_Console.QueryInner
{
    public class ProductTileDataPagingQuery : IPagingQuery
    {
        localhostSqlite.Service sqliteclient;
        ProductTilePara prodPara;

        public ProductTileDataPagingQuery(localhostSqlite.Service _sqliteclient, ProductTilePara _prodPara)
        {
            sqliteclient = _sqliteclient;
            prodPara = _prodPara;
        }

        public int GetTotalRecordNum()
        {
            int allCount;
            sqliteclient.SearPRODTilePaged(prodPara.spacialPara, new int[] { int.Parse(prodPara.timePara[0]), int.Parse(prodPara.timePara[1]) }, prodPara.productType, prodPara.level, 0, 1, out allCount);
            return allCount;
        }

        public DataTable GetCurrentPageData(int startIndex, int length)
        {
            int allCount;
            DataSet ds = sqliteclient.SearPRODTilePaged(prodPara.spacialPara, new int[] { int.Parse(prodPara.timePara[0]), int.Parse(prodPara.timePara[1]) }, prodPara.productType, prodPara.level, startIndex, length, out allCount);
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
