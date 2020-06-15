using System;
using System.Collections.Generic;

namespace QRST_DI_MS_Basis.QueryBase
{
     
    /// <summary>
    ///  查询条件对象
    /// </summary>
    [Serializable]
    public class QueryObj
    {
        ComplexCondition conditionObj;

        string dataBaseCode;

        string userName;

        string userPassward;

        //string[] returnFields;
    }

    public class QueryObj2
    {
        List<SimpleCondition> ListSimpleCondition;

        List<LogicalOperator> ListLogicalOperator;
    }

    [Serializable]
    public class ComplexCondition
    {
        private SimpleCondition[] simpleConditions;

        private LogicalOperator logicalOperator;

        private ComplexCondition complexCondition;

        /// <summary>
        /// 简单条件数组
        /// </summary>
        public SimpleCondition[] SimpleConditions
        {
            get
            {
                return simpleConditions;
            }
            set
            {
                simpleConditions = value;
            }
        }
        /// <summary>
        /// 逻辑操作符，复杂条件与简单条件之间 或者 简单条件之间
        /// </summary>
        public LogicalOperator LogiOperator
        {
            get
            {
                return logicalOperator;
            }
            set
            {
                logicalOperator = value;
            }
        }
        /// <summary>
        /// 自身递归，表示复杂条件
        /// </summary>
        public ComplexCondition CpxCondition
        {
            get
            {
                return complexCondition;
            }
            set
            {
                complexCondition = value;
            }
        }
    }
    [Serializable]
    public class SimpleCondition
    {

        private string sc_Variable;

        private FieldOperator sc_FieldOperator;

        private string sc_Value;

        /// <summary>
        /// 简单条件中的变量
        /// </summary>
        public string SC_Variable
        {
            get
            {
                return sc_Variable;
            }
            set
            {
                sc_Variable = value;
            }
        }
        /// <summary>
        /// 简单条件中的数字操作符
        /// </summary>
        public FieldOperator SC_FieldOperator
        {
            get
            {
                return sc_FieldOperator;
            }
            set
            {
                sc_FieldOperator = value;
            }
        }
        /// <summary>
        /// 简单条件中的值
        /// </summary>
        public string SC_Value
        {
            get
            {
                return sc_Value;
            }
            set
            {
                sc_Value = value;
            }
        }
        public SimpleCondition()
        {
        }
        public SimpleCondition(string FieldName, FieldOperator FieldOperator,string FieldValue)
        {
            this.sc_Variable = FieldName;
            this.sc_FieldOperator = FieldOperator;
            this.sc_Value = FieldValue;
        }
    }

    public enum FieldOperator
    {
        /// <summary>
        /// 等于
        /// </summary>
        Eto,
        /// <summary>
        /// 不等于
        /// </summary>
        NEto,
        /// <summary>
        /// 大于
        /// </summary>
        Gto,
        /// <summary>
        /// 大于等于
        /// </summary>
        GEto,
        /// <summary>
        /// 小于
        /// </summary>
        Lto,
        /// <summary>
        /// 小于等于
        /// </summary>
        LEto,
        /// <summary>
        /// Like
        /// </summary>
        Liketo,
        /// <summary>
        /// Not Like
        /// </summary>
        NLiketo,
        /// <summary>
        /// 未定义
        /// </summary>
        NotDefine
    }
    public enum LogicalOperator
    {
        /// <summary>
        /// “与”运算
        /// </summary>
        ANDto,
        /// <summary>
        /// “或”运算
        /// </summary>
        ORto,
        /// <summary>
        /// 未定义
        /// </summary>
        NotDefine
    }
}
