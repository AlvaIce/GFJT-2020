/*
 * 作者：zxw
 * 创建时间：2013-08-06
 * 描述：用于封装矢量数据查询参数
*/ 
using System;
using System.Collections.Generic;
using System.Linq;
using QRST_DI_SS_Basis;
using QRST_DI_SS_Basis.MetadataQuery;

namespace QRST_DI_DS_MetadataQuery.QueryConditionParameter
{
    [Serializable]
    public class VectorQueryPara:QueryPara
    {
         //空间查询参数
         public string EXTENTUP { get; set; }
         public string EXTENTDOWN { get; set; }
         public string EXTENTLEFT { get; set; }
         public string EXTENTRIGHT { get; set; }

         public string extentUpField { get; set; }
         public string extentDownField { get; set; }
         public string extentLeftField { get; set; }
         public string extentRightField { get; set; }

         public bool spacialAvailable;
        //时间查询参数
         public string STARTTIME { get; set; }
         public string ENDTIME { get; set; }

         //关键字查询参数
         public string KEYWORDS { get; set; }

        //数据类型GROUPCODE  zxw 20131020
         public string GROUPCODE;

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
             spacialAvailable = true;
             extentUpField = mapping.GetValue("VECTOR.EXTENTUP");
             extentUpField = GetField(dic, extentUpField.Split(":".ToArray()));
             if (string.IsNullOrEmpty(extentUpField))
             {
                 spacialAvailable = false;
                 queryCondition.Clear();
             }
             else
             {
                 SimpleCondition sm = new SimpleCondition();
                 sm.accessPointField = extentUpField;
                 sm.comparisonOperatorField = "<=";
                 sm.valueField = EXTENTUP;
                 queryCondition.Add(sm);
             }

             extentDownField = mapping.GetValue("VECTOR.EXTENTDOWN");
             extentDownField = GetField(dic, extentDownField.Split(":".ToArray()));
             if (string.IsNullOrEmpty(extentDownField) || !spacialAvailable)
             {
                 spacialAvailable = false;
                 queryCondition.Clear();
             }
             else
             {
                 SimpleCondition sm = new SimpleCondition();
                 sm.accessPointField = extentDownField;
                 sm.comparisonOperatorField = ">=";
                 sm.valueField = EXTENTDOWN;
                 queryCondition.Add(sm);
             }
             
              extentLeftField = mapping.GetValue("VECTOR.EXTENTLEFT");
             extentLeftField = GetField(dic, extentLeftField.Split(":".ToArray()));
             if (string.IsNullOrEmpty(extentLeftField) || !spacialAvailable)
             {
                 spacialAvailable = false;
                 queryCondition.Clear();
             }
             else
             {
                 SimpleCondition sm = new SimpleCondition();
                 sm.accessPointField = extentLeftField;
                 sm.comparisonOperatorField = ">=";
                 sm.valueField = EXTENTLEFT;
                 queryCondition.Add(sm);
             }
             //DATALOWERRIGHTLAT
              extentRightField = mapping.GetValue("VECTOR.EXTENTRIGHT");
             extentRightField = GetField(dic, extentRightField.Split(":".ToArray()));
             if (string.IsNullOrEmpty(extentRightField) || !spacialAvailable)
             {
                 spacialAvailable = false;
                 queryCondition.Clear();
             }
             else
             {
                 SimpleCondition sm = new SimpleCondition();
                 sm.accessPointField = extentRightField;
                 sm.comparisonOperatorField = "<=";
                 sm.valueField = EXTENTRIGHT;
                 queryCondition.Add(sm);
             }

             string timeField = mapping.GetValue("VECTOR.PRODUCEDATE");
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

             //添加数据查询类型
             SimpleCondition typeSm = new SimpleCondition();
             typeSm.accessPointField = "类型编码";
             typeSm.comparisonOperatorField = "=";
             typeSm.valueField = GROUPCODE;
             queryCondition.Add(typeSm);

             string[] keywordsField = mapping.GetValue("VECTOR.KEYWORDS").Split(":".ToCharArray());
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
