using System;
using System.Collections.Generic;
using QRST_DI_SS_Basis;
using QRST_DI_SS_Basis.MetaData;
 
namespace QRST_DI_TS_Basis.Search
{
    [Serializable]
    public class PagedSearchTool
    {
        /// <summary>
        /// 获取分页检索中的每个配号的检索分页信息
        /// </summary>
        /// <param name="startIndex">查询起始索引</param>
        /// <param name="modsInfo">配号信息列表</param>
        /// <param name="offset">查询要求返回的记录数目</param>
        /// <returns></returns>
        public static void GetPageInfo(int startIndex, List<ModIDSearchInfo> modsInfo, int offset)
        {
            //List<ModIDInfo> updateModsInfo = new List<ModIDInfo>();

            //int returnRecordSize = offset;
            //List<ModIDInfo> listPagesInfo = new List<ModIDInfo>();
            int allRecordsCount = SumModsRecordsCount(modsInfo);
            
            //查询起始位置已超过结果数目
            if (startIndex>=allRecordsCount)
            {
                return;
            }
            if (startIndex + offset > allRecordsCount)
            {
                offset = allRecordsCount - startIndex;
            }
            //所有符合要求的数据记录组成的list，”等候叫号“
            List<string> OrderedRecords = getRecordInfoList(modsInfo);
            //用户要求的页大小和页索引来得到本次查询应返回给用户的所有记录
            List<string> pageOrderedRecord = getListPaged(OrderedRecords, offset, startIndex);

            //临时保存每个配号中，本次查询应返回给用户的记录的起始索引和数目。给配号对象的PageInfo赋值后清空。
            List<string> mod_PageRecordsList = new List<string>();
            foreach (ModIDSearchInfo modinfo in modsInfo)
            {
                //对于每个配号，从本次用户要求的返回结果对应的pageOrderedRecord中提取该配号要查询的记录，给该配号对象ModInfo的PageInfo对象赋值。
                mod_PageRecordsList.Clear();
                for (int i = 0; i < pageOrderedRecord.Count; i++)
                {
                    if (pageOrderedRecord[i].Substring(0, pageOrderedRecord[i].IndexOf('_')) == modinfo.ModID)
                    {
                        mod_PageRecordsList.Add(pageOrderedRecord[i]);
                    }
                }

                //若某个配号的分页信息为null，则初始化
                if (modinfo.modPageInfo == null)
                {
                    modinfo.modPageInfo = new PagedInfo();
                }
                int recordstartIndex=-1;
                int recordNumber = 0;
                if (mod_PageRecordsList.Count != 0)
                {
                    recordstartIndex = Convert.ToInt32(mod_PageRecordsList[0].Substring(mod_PageRecordsList[0].IndexOf('_') + 1));
                    recordNumber = mod_PageRecordsList.Count;
                    modinfo.modPageInfo = new PagedInfo(recordstartIndex, recordNumber);
                    //updateModsInfo.Add(modinfo);
                }
                else
                {
                    recordNumber = 0;
                    //modsInfo.Remove(modinfo);
                    modinfo.modPageInfo = new PagedInfo(recordstartIndex, recordNumber);
                }
            }
            //modsInfo = new List<ModIDInfo>(updateModsInfo);
        }

        /// <summary>
        /// 根据用户的查询起始索引和查询结果数目，从所有记录构成的list中截取应返回的记录组成新的list。此list中可计算当前页查询时需从哪个配号获取哪几条数据（从第几条数据开始，一共几条）
        /// </summary>
        /// <param name="OrderedRecords">排列后的所有记录链表，即方法getRecordInfoList的返回值</param>
        /// <param name="offset">记录数目，检索偏移量</param>
        /// <param name="startIndex">起始索引</param>
        /// <returns>该页应返回的记录的信息组成的链表</returns>
        private static List<string> getListPaged(List<string> OrderedRecords, int offset, int startIndex)
        {
            List<string> pageList = new List<string>();

            //int allrecordsCount = OrderedRecords.Count;

            for (int i = 0; i < offset;i++ )
            {
                pageList.Add(OrderedRecords[startIndex + i]);
            }

            ////int maxPageIndex=allrecordsCount/returnRecordSize+1;
            //if (allrecordsCount % returnRecordSize != 0 && pageIndex == allrecordsCount / returnRecordSize + 1)
            //{
            //    //不能除尽的情况
            //    int lastPageSize = allrecordsCount % returnRecordSize;
            //    for (int i = 0; i < lastPageSize; i++)
            //    {
            //        pageList.Add(OrderedRecords[(pageIndex - 1) * returnRecordSize + i]);
            //    }
            //}
            //else
            //{
            //    for (int i = 0; i < returnRecordSize; i++)
            //    {
            //        pageList.Add(OrderedRecords[(pageIndex - 1) * returnRecordSize + i]);
            //    }
            //}

            return pageList;
        }
        /// <summary>
        /// 计算所有配号中的满足要求记录数之和
        /// </summary>
        /// <param name="modsInfo">配号信息列表</param>
        /// <returns>所有配号中的满足要求记录数之和</returns>
        public static int SumModsRecordsCount(List<ModIDSearchInfo> modsInfo)
        {
            int count = 0;

            for (int i = 0; i < modsInfo.Count; i++)
            {
                count += modsInfo[i].ModRecordsCount;
            }
            return count;
        }
        /// <summary>
        /// 把配号信息按照记录数进行排序
        /// </summary>
        /// <param name="modsInfo">配号信息列表</param>
        /// <returns>按记录数目进行排序后的配号信息列表</returns>
        private static List<ModIDSearchInfo> OrderModsInfo(List<ModIDSearchInfo> modsInfo)
        {
            List<ModIDSearchInfo> OrderedList = new List<ModIDSearchInfo>();
            if (modsInfo.Count > 0)
            {
                OrderedList.Add(modsInfo[0]);
            }

            bool hasInsert;
            for (int i = 1; i < modsInfo.Count; i++)
            {
                hasInsert = false;
                for (int j = 0; j < OrderedList.Count; j++)
                {
                    if (modsInfo[i].ModRecordsCount > OrderedList[j].ModRecordsCount)
                    {
                        OrderedList.Insert(j, modsInfo[i]);
                        hasInsert = true;
                        break;
                    }
                    else
                    { continue; }
                }
                if (!hasInsert)
                {
                    OrderedList.Add(modsInfo[i]);
                }
            }
            //int i, j;
            //bool isChanged ;

            //for (j = modsInfo.Count; j < 0;j-- )
            //{
            //    isChanged = false;
            //    for (i = 0; i < j;i++ )
            //    {
            //        if(modsInfo[i].ModRecordsCount<)
            //        {

            //        }
            //    }
            //}
            return OrderedList;
        }

        /// <summary>
        /// 1.先把modsInfo按照记录数据排序。
        /// 2.之后每一个配号的记录信息转化为List<string>,形式为“配号_记录编号”。如配号2下有9条记录，则List为{“2_0,2_1,...,2_8”}.
        /// 3.每个配号的信息按照第2步转化完成后，按顺序“数数”。如一共有三个配号0、1、2，按照记录数排序后顺序为2、0、1，分别有记录数为9、7、7，
        ///   则数数之后结果为2_0,0_0,1_0,2_1,0_1,1_1,... ...,2_6,0_6,1_6,2_7,2_8   
        /// </summary>
        /// <param name="modsInfo"></param>
        /// <returns></returns>
        private static List<string> getRecordInfoList(List<ModIDSearchInfo> modsInfo)
        {
            List<string> OrderedAllRecord = new List<string>();

            //记录总数
            int sumRecords = SumModsRecordsCount(modsInfo);
            if (sumRecords != 0)
            {
                //把ModsInfo信息按照结果记录数目进行从多到少排序
                modsInfo = OrderModsInfo(modsInfo);

                //把配号信息转化为字符串链表。如配号1中有3条记录，则字符串链表为1_0,1_1,1_2
                List<string>[] ModsString_Arr = new List<string>[modsInfo.Count]; ;

                //是否有为null的list，即是否有的配号下符合要求的记录数目为0
                bool hasNullList = false;

                for (int i = 0; i < modsInfo.Count; i++)
                {
                    if (modsInfo[i].ModRecordsCount != 0)
                    {
                        ModsString_Arr[i] = new List<string>();
                        for (int j = 0; j < modsInfo[i].ModRecordsCount; j++)
                        {
                            ModsString_Arr[i].Add(string.Format("{0}_{1}", modsInfo[i].ModID, j));
                        }
                    }
                    else
                    {
                        hasNullList = true;
                        break;
                    }
                }
                //如果有为null的list，即有的配号下符合要求的记录数目为0
                if (hasNullList)
                {
                    //计算当前不是null的ModsString_Arr数组项的数目
                    int currArrLength = ModsString_Arr.Length;
                    for (int j = 0; j < ModsString_Arr.Length; j++)
                    {
                        if (ModsString_Arr[j] == null)
                        {
                            currArrLength = j;
                            break;
                        }
                    }
                    Array.Resize(ref ModsString_Arr, currArrLength);
                }
                for (int i = 0; i < sumRecords; i++)
                {
                    int listIndex = i % ModsString_Arr.Length;

                    string temp = ModsString_Arr[listIndex][0];
                    ModsString_Arr[listIndex].RemoveAt(0);
                    OrderedAllRecord.Add(temp);
                    if (ModsString_Arr[listIndex].Count == 0)
                    {
                        //不能直接删除数组后面的部分，即不能直接resize。因为可能后几个list的长度相同，则当长度相同的list中的第一个count为0时，其实后边的list中还有值。
                        if (listIndex + 1 == ModsString_Arr.Length)
                        {
                            //计算当前已经没有取值的第一个list
                            int currArrLength = ModsString_Arr.Length;
                            for (int j = 0; j < ModsString_Arr.Length; j++)
                            {
                                if (ModsString_Arr[j].Count == 0)
                                {
                                    currArrLength = j;
                                    break;
                                }
                            }
                            Array.Resize(ref ModsString_Arr, currArrLength);
                        }
                    }
                }
            }

            return OrderedAllRecord;
        }
    }
    
}
