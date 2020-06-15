/*
 * 作者：zxw
 * 创建时间：2013-08-06
 * 描述：查询参数抽象类
*/
using System;
using System.Collections.Generic;
using System.Linq;
using QRST_DI_SS_Basis;
using QRST_DI_SS_Basis.MetadataQuery;
 
namespace QRST_DI_DS_MetadataQuery.QueryConditionParameter
{
    [Serializable]
    public class QueryPara
    {
          public string logicalOperator;
         //其他条件
         public ComplexCondition complexCondition { get; set; }
                /// <summary>
        /// 查询的表的编码
        /// </summary>
        public string dataCode { get; set; }

        public string QRST_CODE { get; set; }
        public string THUMBNAIL { get; set; }

        /// <summary>
        /// 将特定条件组装成ComplexCondition
        /// </summary>
        /// <param name="querySchema"></param>
        /// <returns></returns>
        virtual public ComplexCondition GetSpecificCondition(IGetQuerySchema querySchema) { return null; }

        /// <summary>
        /// 从fields中找到属于要查询表的字段
        /// </summary>
        /// <param name="dic"></param>
        /// <param name="fields"></param>
        /// <returns></returns>
        public string GetField(Dictionary<string, string> dic, string[] fields)
        {
            for (int i = 0; i < fields.Length; i++)
            {
                if (dic.ContainsValue(fields[i]))
                    return fields[i];
            }
            return null;
        }

        /// <summary>
        /// 获取公共字段（QRST,THUMBNAIL）在表中的映射字段
        /// </summary>
        /// <returns></returns>
        public void GetPublicFieldMappedValue(IGetQuerySchema querySchema)
        {
            Dictionary<string, string> dic = querySchema.GetFieldName();
            TVMapping mapping = new TVMapping();
            //公共字段对应值
            QRST_CODE = mapping.GetValue("QRST_CODE");
            QRST_CODE = GetField(dic, QRST_CODE.Split(":".ToArray()));
            THUMBNAIL = mapping.GetValue("THUMBNAIL");
            THUMBNAIL = GetField(dic, THUMBNAIL.Split(":".ToArray()));
        }

    }
}
