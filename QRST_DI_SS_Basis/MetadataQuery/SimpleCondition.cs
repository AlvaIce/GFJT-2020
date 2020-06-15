/*
 * 作者：zxw
 * 创建时间：2013-07-31
 * 描述：主要用于描述SQL查询中简单的查询条件的结构，例如一个表table1中有int型字段a,要查询大于10的所有记录(a>10)，则用SimpleCondition
 * 可以表示为accessPointField = "a";comparisonOperatorField=">",valueField = "10"
*/

using System;

namespace QRST_DI_SS_Basis.MetadataQuery
{
    [Serializable]
    public class SimpleCondition
    {
        private string _accessPointFiled;          //访问字段
        private string _comparisonOperatorField;   //比较操作字段，如>,<,=,<>,like,>=,<=
        private string _valueField;                           //值字段

        public string accessPointField
        {
            get
            {
                return _accessPointFiled;
            }
            set
            {
                _accessPointFiled = value;
            }
        }
        public string comparisonOperatorField
        {
            get
            {
                return _comparisonOperatorField;
            }
            set
            {
                _comparisonOperatorField = value;
            }
        }

        public string valueField
        {
            get
            {
                return _valueField;
            }
            set
            {
                _valueField = value;
            }
        }


        //public static string Serialize(SimpleCondition ob)
        //{

        //}
        //public static SimpleCondition Deserialize(string str)
        //{

        //}
    }
}
