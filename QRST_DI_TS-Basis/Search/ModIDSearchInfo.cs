using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QRST_DI_TS_Basis.Search
{
    [Serializable]
    public class ModIDSearchInfo
    {
        public string ModID;
        public int ModRecordsCount;
        public string ModSql;
        public string IPaddress;
        public string ModDbFilePath;
        public PagedInfo modPageInfo = new PagedInfo();

        /// <summary>
        /// 构造函数1
        /// </summary>
        /// <param name="modID"></param>
        /// <param name="modTableRecordCount"></param>
        public ModIDSearchInfo(string modID)
        {
            this.ModID = modID;
            this.ModRecordsCount = 0;
        }
        /// <summary>
        /// 构造函数2
        /// </summary>
        /// <param name="modID"></param>
        /// <param name="ModDbFilePath"></param>
        public ModIDSearchInfo(string modID, string ModDbFilePath)
        {
            this.ModID = modID;
            this.ModDbFilePath = ModDbFilePath;
        }
        /// <summary>
        /// 构造函数3
        /// </summary>
        /// <param name="modID">配号名称</param>
        /// <param name="modTableRecordCount">配号中结果记录总数</param>
        public ModIDSearchInfo(string modID,int modTableRecordCount)
        {
            this.ModID = modID;
            this.ModRecordsCount = modTableRecordCount;
        }

    }
    [Serializable]
    public  class PagedInfo
    {
        public int recordStartIndex;

        public int recordNumber;

        public PagedInfo()
        {
            this.recordStartIndex = 0;
            this.recordNumber = 0;
        }

        /// <summary>
        /// 当前数据库文件的表中需要检索的数据的起始索引和检索记录数目
        /// </summary>
        /// <param name="tableStartindex">需要检索的起始索引</param>
        /// <param name="tableRecordNumber">需要检索的记录条数</param>
        public PagedInfo(int tableStartindex, int tableRecordNumber)
        {
            this.recordStartIndex = tableStartindex;
            this.recordNumber = tableRecordNumber;
        }

    }
}
