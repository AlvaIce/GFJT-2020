using System;
using QRST_DI_Resources;
using QRST_DI_SS_Basis;
using QRST_DI_SS_Basis.MetadataQuery;
using QRST_DI_SS_DBInterfaces.IDBEngine;
 
namespace QRST_DI_DS_MetadataQuery
{
    public class ViewBasedQuery : AbstractQuery, IQuery
    {
        public ViewBasedQuery(QueryRequest _queryRequest)
        {
            queryRequest = _queryRequest;
            //DBMySqlOperating basedb = new DBMySqlOperating();
            baseUtilities = (IDbBaseUtilities) Constant.IdbOperating.GetsqlBaseObj(queryRequest.dataBase);
            querySchema = new ViewBasedQuerySchema(queryRequest.elementSet, queryRequest.tableCode, baseUtilities);
        }

        public ViewBasedQuery(QueryRequest _queryRequest,IGetQuerySchema _querySchema)
        {
            queryRequest = _queryRequest;
            //DBMySqlOperating basedb = new DBMySqlOperating();
            baseUtilities = (IDbBaseUtilities)Constant.IdbOperating.GetsqlBaseObj(queryRequest.dataBase);
            querySchema = _querySchema;
        }

        public QueryResponse Query()
        {
            QueryResponse qr = new QueryResponse();
            try
            {
                if (baseUtilities == null)
                {
                    throw new Exception(string.Format("没有匹配的数据库'{0}'", queryRequest.dataBase));
                }
                qr.recordSet = baseUtilities.GetDataSet(GetQuerySql());
            }
            catch (Exception ex)
            {
                qr.exception = string.Format("查询请求失败：{0}", ex.ToString());
            }
            return qr;
        }

        public QueryResponse GetTableStruct()
        {
            QueryResponse qr = new QueryResponse();
            try
            {
                //DBMySqlOperating basedb = new DBMySqlOperating();
                //MySqlBaseUtilities baseUtilities = (MySqlBaseUtilities)basedb.GetType().GetProperty(queryRequest.dataBase).GetValue(basedb, null);
               IDbBaseUtilities baseUtilities = (IDbBaseUtilities)Constant.IdbOperating.GetsqlBaseObj(queryRequest.dataBase);
                if (baseUtilities == null)
                {
                    throw new Exception(string.Format("没有匹配的数据库'{0}'", queryRequest.dataBase));
                }
                qr.recordSet = querySchema.GetTableStruct();
            }
            catch (Exception ex)
            {
                qr.exception = string.Format("表结构查询请求失败：{0}", ex.ToString());
            }
            return qr;
        }
    }
}
