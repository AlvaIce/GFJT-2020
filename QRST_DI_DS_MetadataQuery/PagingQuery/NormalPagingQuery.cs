/*
 * 作者：zxw
 * 创建时间：2013-09-06
 * 描述：一般的sql分页查询
*/
using System.Text;
using System.Data;
using QRST_DI_SS_DBInterfaces.IDBEngine;
 
namespace QRST_DI_DS_MetadataQuery.PagingQuery
{
    public class NormalPagingQuery : IPagingQuery
    {
        private IDbBaseUtilities sqlBase;  //mysql查询对象
        private string queryField;      //sql查询语句的查询字段部分
        private string condition;      //sql查询语句的where部分
        public string tableName;      //查询表名字
        public string desql;
       /// <summary>
        /// 构造mysql分页查询对象
       /// </summary>
        /// <param name="_sqlBase">mysql查询对象</param>
        /// <param name="_selectStr">sql查询语句的查询字段部分,如 ‘a,b,c’ 表示三个字段，或者‘*’表示全部字段</param>
        /// <param name="_tableName">表名 </param>
        /// <param name="_whereCondition">sql查询语句的where部分,如：a = '张三' and b like '王五',全查则为“” </param>
        public NormalPagingQuery(IDbBaseUtilities _sqlBase,string _fieldStr,string _tableName,string _whereCondition)
        {
            sqlBase = _sqlBase;
            queryField = _fieldStr;
            condition = _whereCondition;
            tableName = _tableName;
            if (condition.ToString() == "")
            {
                desql = string.Format("select * from {0}", _tableName);
            }
            else
            {
                desql = string.Format("select * from {0} where {1}", _tableName, condition.ToString());
            }
        }
        public NormalPagingQuery()
        { 
        
        }
        //public string desql;
        public int GetTotalRecordNum()
        {
            if(sqlBase != null)
            {
                string sql = string.Format("select count(*) from {0} ",tableName);
                //desql = string.Format("select * from {0} ", tableName);
                if(!string.IsNullOrEmpty(condition))
                {
                    sql = sql + string.Format("where {0}",condition);
                }
                DataSet ds = sqlBase.GetDataSet(sql);
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    return int.Parse(ds.Tables[0].Rows[0][0].ToString());
                }
            }
            return 0;
        }

        public DataTable GetCurrentPageData(int startIndex, int length)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("select {0} from {1}", queryField,tableName);
            if(!string.IsNullOrEmpty(condition))
            {
                sb.AppendFormat(" where {0}",condition);
            }
            sb.AppendFormat(" limit {0},{1}",startIndex,length);
            DataSet ds = sqlBase.GetDataSet(sb.ToString());
            if(ds!=null && ds.Tables.Count>0)
            {
                return ds.Tables[0];
            }
            return null;
        }
    }
}
