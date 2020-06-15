/*
 * 作者：zxw
 * 创建时间：2013-07-31
 * 描述：解析字段表示，例如：EVDB-18.12,代表Modis卫星数据的视图或者表中的第12个字段
*/ 
using System;

namespace QRST_DI_DS_MetadataQuery
{
    public class FieldElement
    {
        public int index;

        public FieldElement(string fieldCode)
        {
            string[] strArr = fieldCode.Split('.');
            if (strArr.Length <= 0)
            {
                throw new Exception(string.Format("字段'{0}'解析出错",fieldCode));
            }
            else
            {
                try
                { 
                    index = int.Parse(strArr[strArr.Length-1]);
                }
                catch (Exception ex)
                {
                    throw new Exception(string.Format("字段'{0}'解析出错", fieldCode));
                }
            }
        }
    }
}
