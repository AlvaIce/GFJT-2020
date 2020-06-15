/*
 * 作者：zxw
 * 创建时间：2013-07-31
 * 描述：用于描述一个查询中的字段排序，如升序，降序
*/

using System;

namespace QRST_DI_SS_Basis.MetadataQuery
{
    [Serializable]
    public class OrderBy
    {
        private string _accessPointField;          //访问字段
        private OrderType _orderType;            //排序方式 

        public string accessPointField
        {
            get
            {
                return _accessPointField;
            }
            set
            {
                _accessPointField = value;
            }
        }

        public OrderType orderType
        {
            get
            {
                return _orderType;
            }
            set
            {
                _orderType = value;
            }
        }

        //public static string Serialize(OrderBy ob) 
        //{
 
        //}
        //public static OrderBy Deserialize(string str)
        //{ 
        
        //}
    }

    public enum OrderType
    {
        ASC = 0,
        DESC = 1,
    }
}
