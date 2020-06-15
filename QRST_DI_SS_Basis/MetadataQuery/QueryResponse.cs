/*
 * 作者：zxw
 * 创建时间：2013-07-31
 * 描述：用于封装请求应答信息
*/

using System;
using System.Data;

namespace QRST_DI_SS_Basis.MetadataQuery
{
    [Serializable]
    public class QueryResponse
    {
        private DataSet _recordSet;
        private string _exception;
        private int _totalRecordCount;    //记录查询总记录数  zxw 20130831 

        public DataSet recordSet
        {
            get
            {
                return _recordSet;
            }
            set
            {
                _recordSet = value;
            }
        }

        public string exception
        {
            get
            {
                return _exception;
            }
            set
            {
                _exception = value;
            }
        }

        public int recordCount
        {
            get
            {
                if (recordSet != null)
                {
                    return recordSet.Tables[0].Rows.Count;
                }
                else
                {
                    return 0;
                }
            }
        }

        public int totalRecordCount
        {
            get
            {
                return _totalRecordCount;
            }
            set
            {
                _totalRecordCount = value;
            }
        }
        
    }
}
