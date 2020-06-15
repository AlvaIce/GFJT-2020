/*
 * 作者：zxw
 * 创建时间：2013-08-06
 * 描述：用于封装文档数据查询参数
*/
using System;
using System.Collections.Generic;
using System.Linq;
using QRST_DI_SS_Basis;
using QRST_DI_SS_Basis.MetadataQuery;
 
namespace QRST_DI_DS_MetadataQuery.QueryConditionParameter
{
    [Serializable]
    public class DocQueryPara:QueryPara
    {
        public string NAME { get; set; }
        public string STARTTIME { get; set; }
        public string ENDTIME { get; set; }
        public string KEYWORDS { get; set; }

        public override ComplexCondition GetSpecificCondition(IGetQuerySchema querySchema)
        {
            ComplexCondition cp = new ComplexCondition();
            Dictionary<string, string> dic = querySchema.GetFieldName();
            List<SimpleCondition> queryCondition = new List<SimpleCondition>();
            TVMapping mapping = new TVMapping();
            //公共字段对应值
            QRST_CODE = mapping.GetValue("QRST_CODE");
            QRST_CODE = GetField(dic, QRST_CODE.Split(":".ToArray()));
            THUMBNAIL = mapping.GetValue("THUMBNAIL");
            THUMBNAIL = GetField(dic, THUMBNAIL.Split(":".ToArray()));

            /*
            string nameField = mapping.GetValue("DOC.NAME");
            nameField = GetField(dic, nameField.Split(":".ToArray()));
            if (!string.IsNullOrEmpty(nameField) && !string.IsNullOrEmpty(NAME))  //存在时间查询字段
            {
                SimpleCondition sm = new SimpleCondition();
                sm.accessPointField = nameField;
                sm.comparisonOperatorField = "like";
                sm.valueField = string.Format("%{0}%", NAME); ;
                queryCondition.Add(sm);
            }
             */

            string timeField = mapping.GetValue("DOC.TIME");
            timeField = GetField(dic, timeField.Split(":".ToArray()));
            if (!string.IsNullOrEmpty(timeField) && !string.IsNullOrEmpty(STARTTIME))  //存在时间查询字段
            {
                SimpleCondition sm = new SimpleCondition();
                sm.accessPointField = timeField;
                sm.comparisonOperatorField = ">=";
                sm.valueField = STARTTIME;
                queryCondition.Add(sm);
                if (!string.IsNullOrEmpty(ENDTIME))
                {
                    SimpleCondition sm1 = new SimpleCondition();
                    sm1.accessPointField = timeField;
                    sm1.comparisonOperatorField = "<=";
                    sm1.valueField = ENDTIME;
                    queryCondition.Add(sm1);
                }
            }

            string[] keywordsField = mapping.GetValue("DOC.GLOBALKEYWORDS").Split(":".ToCharArray());
            ComplexCondition keyWordCondition = new ComplexCondition();
            keyWordCondition.logicOperator = EnumLogicalOperator.OR;
            List<SimpleCondition> scs = new List<SimpleCondition>();
            for (int i = 0; i < keywordsField.Length; i++)
            {
                if (dic.ContainsValue(keywordsField[i]))
                {
                    SimpleCondition sm = new SimpleCondition();
                    sm.accessPointField = keywordsField[i];
                    sm.comparisonOperatorField = "like";
                    sm.valueField = string.Format("%{0}%", KEYWORDS);
                    scs.Add(sm);
                }
            }
            keyWordCondition.simpleCondition = scs.ToArray();

            cp.simpleCondition = queryCondition.ToArray();
            cp.complexCondition = new ComplexCondition[1] { keyWordCondition };
            return cp;
            
        }
    }
}
