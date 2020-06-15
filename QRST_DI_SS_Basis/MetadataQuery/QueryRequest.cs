/*
 * 作者：zxw
 * 创建时间：2013-07-31
 * 描述：用于描述一个查询请求
*/

using System;

namespace QRST_DI_SS_Basis.MetadataQuery
{
    [Serializable]
    public class QueryRequest
    {
        private string _protocolVersion ="v1.0";
        private string _industryCode = "0001";                       //行业编码
        private string _userName;
        private string _passWord;
        private string _dataBase;                                           //数据库编码
        private string _tableCode;                                         //表编码
        private string[] _elementSet;                                     //查询字段集
        private ComplexCondition _complexCondition;
        private OrderBy[] _orderBy;
        private int _recordSetStartPointSpecified = 0;          //请求记录的起始位置，默认为0
        private int _offset = 10000;                                             //请求返回的记录数，若小于0则返回所有记录,默认50条记录
        
        public string protocolVersion
        {
            get
            {
                return _protocolVersion;
            }
            set
            {
                _protocolVersion = value;
            }
        }

        public string industryCode
        {
            get
            {
                return _industryCode;
            }
            set
            {
                _industryCode = value;
            }
        }

        public string userName
        {
            get
            {
                return _userName;
            }
            set
            {
                _userName = value;
            }
        }

        public string passWord
        {
            get
            {
                return _passWord;
            }
            set
            {
                _passWord = value;
            }
        }

        public string dataBase
        {
            get
            {
                return _dataBase;
            }
            set
            {
                _dataBase = value;
            }
        }

        public string tableCode
        {
            get
            {
                return _tableCode;
            }
            set
            {
                _tableCode = value;
            }
        }

        public string[] elementSet
        {
            get
            {
                return _elementSet;
            }
            set
            {
                _elementSet = value;
            }
        }

        public OrderBy[] orderBy
        {
            get
            {
                return _orderBy;
            }
            set
            {
                _orderBy = value;
            }
        }

        public int recordSetStartPointSpecified
        {
            get
            {
                return _recordSetStartPointSpecified;
            }
            set
            {
                _recordSetStartPointSpecified = value;
            }
        }

        public int offset
        {
            get
            {
                return _offset;
            }
            set
            {
                _offset = value;
            }
        }

        public ComplexCondition complexCondition
        {
            get
            {
                return _complexCondition;
            }
            set
            {
                _complexCondition = value;
            }
        }

        //public static string Serialize(QueryRequest ob)
        //{

        //}
        //public static QueryRequest Deserialize(string str)
        //{

        //}
    }
}
