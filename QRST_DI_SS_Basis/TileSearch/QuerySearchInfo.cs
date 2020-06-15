using QRST_DI_SS_Basis.MetaData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QRST_DI_SS_Basis.TileSearch
{
    [Serializable]
    public class QuerySearchInfo
    {
        public string SQLString;

        public DateTime ObjectTime;

        public List<ModIDSearchInfo> QRST_ModsList;

        public QuerySearchInfo(string sqlString, DateTime objectCreateTime)
        {
            this.SQLString = sqlString;
            this.ObjectTime = objectCreateTime;
        }
        public int allcount;
    }
}
