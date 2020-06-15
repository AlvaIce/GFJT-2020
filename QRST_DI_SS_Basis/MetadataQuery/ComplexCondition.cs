/*
 * 作者：zxw
 * 创建时间：2013-07-31
 * 描述：主要用于描述SQL查询中的复杂条件查询语句，如（a>b）||(c<d&&e=f)
 * 一个ComplexCondition是由0或多个ComplexCondition和0或多个SimpleCondition组成
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DotSpatial.Data;

namespace QRST_DI_SS_Basis.MetadataQuery
{
    [Serializable]
    public class ComplexCondition
    {
        private ComplexCondition[] _complexCondition;
        private EnumLogicalOperator _logicOperator = EnumLogicalOperator.AND;                                           //描述SQL逻辑操作符号，用于连接ComplexCondition和SimpleCondition,默认为AND
        private SimpleCondition[] _simpleCondition;
		private double[] _selectRation;
		private  Rule _ruleName;
        public static IFeature QueryGeometry { get; set; }
        public static bool _usingGeometry = false;
        public ComplexCondition[] complexCondition
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

        public EnumLogicalOperator logicOperator
        {
            get
            {
                return _logicOperator;
            }
            set
            {
                _logicOperator = value;
            }
        }

        public SimpleCondition[] simpleCondition
        {
            get
            {
                return _simpleCondition;
            }
            set
            {
                _simpleCondition = value;
            }
        }
		public double[] selectRation
		{
			get
			{
				return _selectRation;
			}
			set
			{
				_selectRation = value;
			}
		}
		public Rule ruleName
		{
			get
			{
				return _ruleName;
			}
			set
			{
				_ruleName = value;
			}
		}
        //public static string Serialize(ComplexCondition ob)
        //{

        //}
        //public static ComplexCondition Deserialize(string str)
        //{

        //}
        /// <summary>
        /// 将复杂条件对象转换为sql子句。
        /// </summary>
        /// <returns></returns>
        public string GetSqlStr(Dictionary<string, string> fields)
        {
                StringBuilder sqlStr = new StringBuilder(" (");
            try
            {
                if (complexCondition != null)
                {
                    for (int i = 0; i < complexCondition.Length && complexCondition[i] != null; i++)
                    {
                        string csql = complexCondition[i].GetSqlStr(fields);
                        if (!string.IsNullOrEmpty(csql.Trim()))
                        {
                            sqlStr.Append(complexCondition[i].GetSqlStr(fields));
                            if (i < complexCondition.Length - 1 && !string.IsNullOrEmpty(complexCondition[i + 1].GetSqlStr(fields).Trim()))
                                sqlStr.AppendFormat(" {0} ", _logicOperator.ToString());
                        }

                    }
                    if (complexCondition.Length > 0 && simpleCondition != null && simpleCondition.Length > 0 && !sqlStr.ToString().Trim().Equals("("))
                    {
                        sqlStr.AppendFormat(" {0} ", _logicOperator.ToString());
                    }
                }
                if (simpleCondition != null)
                {
					bool isJingHao=true;
                    bool _isGrid = false;
                    bool _isVector = false;
                    for (int j = 0; j < simpleCondition.Length && simpleCondition[j] != null; j++)
                    {
                        string field = fields[simpleCondition[j].accessPointField];
						if (ruleName == Rule.Intersect)
						{
							if (field.Contains("经度") || field.Contains("纬度"))
							{
								isJingHao = false;
								_isGrid = true;
								_isVector = false;
                                continue;
							}
							if (field.Contains("数据范围"))
							{
								_isGrid = false;
								_isVector = true;
                                continue;
							}
						}
						if (simpleCondition[j].valueField.Contains('(')&&isJingHao)
							sqlStr.AppendFormat("{0} {1} '{2}' ", field, simpleCondition[j].comparisonOperatorField, simpleCondition[j].valueField);
						else
						{
							sqlStr.AppendFormat("{0} {1} '{2}' ", field, simpleCondition[j].comparisonOperatorField, simpleCondition[j].valueField);
							isJingHao = true;
						}
						if (j < simpleCondition.Length - 1 && simpleCondition[j + 1] != null)
                        {
                            sqlStr.AppendFormat(" {0} ", _logicOperator.ToString());
                        }
                    }
					if (simpleCondition.Length > 0)
					{
						if (ruleName == Rule.Intersect)
						{
                            StringBuilder strTemp = new StringBuilder();
                            string strtem = sqlStr.ToString().TrimEnd(' ');
                            if (strtem.EndsWith("AND"))
                                strtem = strtem.TrimEnd(("AND").ToCharArray());
                            strTemp.Append(strtem);
                            sqlStr.Clear();
                            sqlStr = strTemp;
							try
							{
								if (_isGrid)
								{
									string fieldStr = String.Format("And ((右下纬度<='{0}' And 右下经度 >='{1}' and 左上纬度>='{6}' and 左上经度<='{7}') or (左下纬度<='{2}' and 左下经度 <='{3}' and 右上纬度>='{4}' and 右上经度 >='{5}'))", Math.Max(selectRation[1], selectRation[3]), Math.Min(selectRation[0], selectRation[2]), Math.Max(selectRation[1], selectRation[3]), Math.Max(selectRation[0], selectRation[2]), Math.Min(selectRation[1], selectRation[3]), Math.Min(selectRation[0], selectRation[2]), Math.Min(selectRation[1], selectRation[3]), Math.Max(selectRation[0], selectRation[2]));
									sqlStr.Append(fieldStr);
								}
								else if (_isVector)
								{
									string fieldStr = String.Format("And ((数据范围下<='{0}' And 数据范围右 >='{1}' and 数据范围上>='{6}' and 数据范围左<='{7}') or (数据范围下<='{2}' and 数据范围左 <='{3}' and 数据范围上>='{4}' and 数据范围右 >='{5}'))", Math.Max(selectRation[1], selectRation[3]), Math.Min(selectRation[0], selectRation[2]), Math.Max(selectRation[1], selectRation[3]), Math.Max(selectRation[0], selectRation[2]), Math.Min(selectRation[1], selectRation[3]), Math.Min(selectRation[0], selectRation[2]), Math.Min(selectRation[1], selectRation[3]), Math.Max(selectRation[0], selectRation[2]));
									sqlStr.Append(fieldStr);
								}
							}
							catch
							{
							}
						}
					}
                }
                sqlStr.Append(") ");
                string sql = sqlStr.ToString().Trim();
				if (sql.StartsWith("(And"))
				{
					sql = sql.TrimStart(("(And").ToCharArray());
					sql = sql.TrimEnd(')');
					sql += "))";
				}
				if (sql.Equals("()"))
				{
					return "";
				}
				return sql;
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("查询条件语句初始化失败，请检查'ComplexCondition'的构建是否合法！{0}",sqlStr));
            }
        }

        #region       定义一些构造特殊条件的方法，例如空间查询、时间查询等
        /// <summary>
        /// 构造空间查询条件
        /// </summary>
        /// <param name="spatialFieldName">表中空间信息范围字段名称，该数组中有四个值，依次代表最大经度、最小经度、最大纬度、最小纬度，不能为空</param>
        /// <param name="spatialValue">空间范围的值，分别对应字段中的值</param>
        /// <returns></returns>
        public static ComplexCondition ConstructSpatialCondition(string[] spatialFieldName, string[] spatialValue)
        {
            ComplexCondition spatialCondition = new ComplexCondition();

            return spatialCondition;
        }
        #endregion


    }

    public enum EnumLogicalOperator
    {
        AND = 0,
        OR = 1,
    }
	public enum Rule
	{
		/// <summary>
		/// 相交
		/// </summary>
		Intersect = 0,
		/// <summary>
		/// 包含
		/// </summary>
		Contain = 1,
	}
}
