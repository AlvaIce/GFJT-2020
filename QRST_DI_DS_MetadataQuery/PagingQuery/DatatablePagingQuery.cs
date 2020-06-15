/*
 * 作者：gyh
 * 创建时间：2013-12-10
 * 描述：针对波谱数据得到的DataTable分页。
*/
 
using System.Data;

namespace QRST_DI_DS_MetadataQuery.PagingQuery
{
    public class DatatablePagingQuery : IPagingQuery
    {
        private DataTable _dt = null;
        private DataTable _CPDdt = null;

        public DatatablePagingQuery(DataTable dt)
        {
            _dt = dt;
        }


        public int GetTotalRecordNum()
        {
            if (_dt != null)
            {
                return _dt.Rows.Count;
            }
            else
                return 0;
        }

        public DataTable GetCurrentPageData(int startIndex, int length)
        {
            _CPDdt = _dt.Clone();
            for (int i = startIndex; i < ((_dt.Rows.Count - startIndex) >= length ? (startIndex + length) : _dt.Rows.Count); i++)
            {
                _CPDdt.ImportRow(_dt.Rows[i]);
            }
            return _CPDdt;
        }
    }
}
