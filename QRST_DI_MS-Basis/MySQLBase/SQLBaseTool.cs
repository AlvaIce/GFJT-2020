using System;
using System.Collections.Generic;
using System.Text;
using QRST_DI_MS_Basis.QueryBase;
//using MySql.Data.MySqlClient;
using System.Data;
using QRST_DI_Resources;
using QRST_DI_SS_Basis;
using QRST_DI_SS_Basis.MetaData;
using QRST_DI_SS_DBInterfaces.IDBEngine;
using SimpleCondition = QRST_DI_MS_Basis.QueryBase.SimpleCondition;
 
namespace QRST_DI_MS_Basis.MySQLBase
{
    public class SQLBaseTool
    {
        #region 通用查询方法生成查询语句
        private LogicalOperator currLogiOper;
        /// <summary>
        /// 根据双List生成查询语句的条件语句
        /// </summary>
        /// <param name="listLogicalOperators"></param>
        /// <param name="listSimpleConditions"></param>
        /// <returns></returns>
        public string GetSqlCondition(List<LogicalOperator> listLogicalOperators, List<SimpleCondition> listSimpleConditions, string[] allFilelds)
        {
            string retStr = string.Empty;
            bool hasEightSpace = false;
            hasEightSpace = getSpaceStyle(allFilelds);

            if (hasEightSpace)
            {
                //处理空间范围检索条件，
                string minLatValue = string.Empty;
                string maxLatValue = string.Empty;
                string minLonValue = string.Empty;
                string maxLonValue = string.Empty;

                List<int> listMark = new List<int>();
                for (int i = 0; i < listSimpleConditions.Count; i++)
                {
                    if (listSimpleConditions[i].SC_Variable.Trim().Equals("左上经度") || listSimpleConditions[i].SC_Variable.Trim().Equals("左下经度"))
                    {
                        minLonValue = listSimpleConditions[i].SC_Value;
                        listMark.Add(i);
                    }
                    if (listSimpleConditions[i].SC_Variable.Trim().Equals("右上经度") || listSimpleConditions[i].SC_Variable.Trim().Equals("右下经度"))
                    {
                        maxLonValue = listSimpleConditions[i].SC_Value;
                        listMark.Add(i);
                    }
                    if (listSimpleConditions[i].SC_Variable.Trim().Equals("左上纬度") || listSimpleConditions[i].SC_Variable.Trim().Equals("右上纬度"))
                    {
                        maxLatValue = listSimpleConditions[i].SC_Value;
                        listMark.Add(i);
                    }
                    if (listSimpleConditions[i].SC_Variable.Trim().Equals("左下纬度") || listSimpleConditions[i].SC_Variable.Trim().Equals("右下纬度"))
                    {
                        minLatValue = listSimpleConditions[i].SC_Value;
                        listMark.Add(i);
                    }
                }
                for (int i = listMark.Count - 1; i >= 0; i--)
                {
                    listLogicalOperators.RemoveAt(listMark[i]);
                    listSimpleConditions.RemoveAt(listMark[i]);
                }

                retStr = String.Format("((左上纬度>'{0}' or 右上纬度>'{0}') and (左下纬度<'{1}' or 右下纬度<'{1}') and (左上经度<'{2}' or 左下经度<'{2}')and (右上经度>'{3}' or 右下经度>'{3}')) ", minLatValue, maxLatValue, maxLonValue, minLonValue);
            }
            if (listSimpleConditions.Count > 0)
            {
                if (string.IsNullOrEmpty(retStr))
                {
                    retStr += "(" + GetSimConStr(listSimpleConditions[0]) + ")";
                }
                else
                {
                    retStr += GetLogiStrFromEnum(listLogicalOperators[0]) + "(" + GetSimConStr(listSimpleConditions[0]) + ")";
                }
                this.currLogiOper = listLogicalOperators[0];
                if (listLogicalOperators.Count > 1)
                {
                    for (int i = 1; i < listLogicalOperators.Count; i++)
                    {
                        if (listLogicalOperators[i] != this.currLogiOper)
                        {
                            retStr = "(" + retStr + ")";
                            this.currLogiOper = listLogicalOperators[i];
                        }
                        else
                        {
                            retStr = retStr + GetLogiStrFromEnum(listLogicalOperators[i]) + "( " + GetSimConStr(listSimpleConditions[i]) + " )";
                        }
                    }
                }
            }
            return retStr;
        }

        /// <summary>
        /// 根据双List生成查询语句的条件语句
        /// </summary>
        /// <param name="listLogicalOperators"></param>
        /// <param name="listSimpleConditions"></param>
        /// <returns></returns>
        public string GetSqlCondition(List<LogicalOperator> listLogicalOperators, List<SimpleCondition> listSimpleConditions, string[] allFilelds, string additionalCondition)
        {
            string retStr = string.Empty;

            retStr = GetSqlCondition(listLogicalOperators, listSimpleConditions, allFilelds);
            if (additionalCondition.Trim() != string.Empty)
            {
                retStr += " and (" + additionalCondition.Trim("()".ToCharArray()) + ")";
            }

            return retStr;
        }
        /// <summary>
        /// 判断是否字段列表中包含8个空间字段
        /// </summary>
        /// <param name="allFilelds"></param>
        /// <returns></returns>
        private bool getSpaceStyle(string[] allFilelds)
        {
            string strFields = string.Concat(allFilelds);
            if (strFields.Contains("左上经度") && strFields.Contains("右上经度") && strFields.Contains("左下经度") && strFields.Contains("左下经度") && strFields.Contains("左上纬度") && strFields.Contains("右上纬度") && strFields.Contains("左下纬度") && strFields.Contains("右下纬度"))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// 把简单条件类型转化为字符串
        /// </summary>
        /// <param name="simpleCondition"></param>
        /// <returns></returns>
        public string GetSimConStr(SimpleCondition simpleCondition)
        {
            string retStr = string.Empty;

            retStr += simpleCondition.SC_Variable;
            retStr += " " + GetFieldOperatorStrFromEnum(simpleCondition.SC_FieldOperator);
            retStr += " '" + simpleCondition.SC_Value + "'";

            return retStr;
        }
        /// <summary>
        /// 从枚举值的逻辑运算符转换为字符串
        /// </summary>
        /// <param name="logicalOperator"></param>
        /// <returns></returns>
        public string GetLogiStrFromEnum(LogicalOperator logicalOperator)
        {
            string retLogiStr = string.Empty;
            switch (logicalOperator)
            {
                case LogicalOperator.ANDto:
                    retLogiStr = " and ";
                    break;
                case LogicalOperator.ORto:
                    retLogiStr = " or ";
                    break;
                case LogicalOperator.NotDefine:
                    retLogiStr = string.Empty;
                    break;
                default:
                    break;
            }
            return retLogiStr;
        }

        /// <summary>
        /// 从枚举值的字段运算符转换为字符串
        /// </summary>
        /// <param name="logicalOperator"></param>
        /// <returns></returns>
        public string GetFieldOperatorStrFromEnum(FieldOperator fieldOperator)
        {
            string retLogiStr = string.Empty;
            switch (fieldOperator)
            {
                case FieldOperator.Eto:
                    retLogiStr = " = ";
                    break;
                case FieldOperator.NEto:
                    retLogiStr = " != ";
                    break;
                case FieldOperator.Gto:
                    retLogiStr = " > ";
                    break;
                case FieldOperator.GEto:
                    retLogiStr = " >= ";
                    break;
                case FieldOperator.Lto:
                    retLogiStr = " < ";
                    break;
                case FieldOperator.LEto:
                    retLogiStr = " <= ";
                    break;
                case FieldOperator.Liketo:
                    retLogiStr = " Like ";
                    break;
                case FieldOperator.NLiketo:
                    retLogiStr = " NOT Like ";
                    break;
                case FieldOperator.NotDefine:
                    retLogiStr = string.Empty;
                    break;
                default:
                    break;
            }
            return retLogiStr;
        }
        #endregion

        #region 拼接查询的条件语句（PL）
        /// <summary>
        /// 拼接算法组件查询的条件语句（）
        /// </summary>
        /// <param name="AlgEnName"></param>
        /// <param name="AlgCnName"></param>
        /// <param name="ComponentVersion"></param>
        /// <param name="IsOnCloud"></param>
        /// <returns></returns>
        public string GetQueryCondition_AlgorithmPL(string AlgEnName, string AlgCnName, string ComponentVersion)
        {
            string QueryStr = "";
            //算法英文名字
            if (!string.IsNullOrEmpty(AlgEnName))
            {
                QueryStr += " (";
                QueryStr += String.Format("AlgorithmName = '{0}'", AlgEnName.Trim());
                QueryStr += ") and ";
            }
            //算法中文名字
            if (!string.IsNullOrEmpty(AlgCnName))
            {
                QueryStr += " (";
                QueryStr += String.Format("ChsName like '%{0}%'", AlgCnName.Trim());
                QueryStr += ") and ";
            }
            //算法版本号
            if (!string.IsNullOrEmpty(ComponentVersion))
            {
                QueryStr += " (";
                QueryStr += String.Format("Version = '{0}'", ComponentVersion.Trim());
                QueryStr += ") and ";
            }

            QueryStr = QueryStr.TrimEnd(" and".ToCharArray());

            return QueryStr.ToString();

        }

        /// <summary>
        /// 获取标准纠正后数据的查询条件
        /// </summary>
        /// <param name="position">空间范围 最小纬度 最小经度 最大纬度 最大经度</param>
        /// <param name="datetime">起时间 止时间</param>
        /// <param name="satellite">卫星 GF1 GF2 ...</param>
        /// <param name="sensor">传感器 CCD</param>
        /// <param name="DnMark">昼夜标识</param>
        /// <param name="PixelSpacing">分辨率</param>
        /// <param name="DataSizeRange">数据大小范围，单位为KB</param>
        /// <param name="CloudNumRange">云量范围</param>
        /// <returns></returns>
        public string GetQueryCondition_CorrectedData(List<string> position, List<DateTime> datetime, List<string> satellite, List<string> sensor, string DnMark, List<string> PixelSpacing, List<int> DataSizeRange, List<int> CloudNumRange)
        {
            string QueryStr = "";
            //空间范围
            if (position.Count == 4)
            {
                string minLatValue = position[0];
                string minLonValue = position[1];
                string maxLatValue = position[2];
                string maxLonValue = position[3];

                
                //QueryStr += String.Format("((左上纬度>'{0}' or 右上纬度>'{0}') and (左下纬度<'{1}' or 右下纬度<'{1}') and (左上经度<'{2}' or 左下经度<'{2}')and (右上经度>'{3}' or 右下经度>'{3}')) ", minLatValue, maxLatValue, maxLonValue, minLonValue);
                QueryStr += String.Format("((dataUpperLeftLat>'{0}' or dataUpperRightLat>'{0}') and (dataLowerLeftLat<'{1}' or dataLowerRightLat<'{1}') and (dataUpperLeftLong<'{2}' or dataLowerLeftLong<'{2}')and (dataUpperRightLong>'{3}' or dataLowerRightLong>'{3}')) ", minLatValue, maxLatValue, maxLonValue, minLonValue);
                //QueryStr += " and";
                QueryStr += " and";
            }
            if (datetime.Count == 2)
            {
                QueryStr += "(";
                DateTime dt1 = datetime[0];
                DateTime dt2 = datetime[1];
                QueryStr += string.Format("SCENEDATE between '{0}' and '{1}'", dt1, dt2);
                //QueryStr += searchHJ.getTime(Convert.ToString(dt1), Convert.ToString(dt2), "SCENEDATE");
                QueryStr += ") and";
            }
            if (satellite.Count != 0)
            {
                QueryStr += " (";
                foreach (string s in satellite)
                {
                    QueryStr += string.Format(" SATELLITE = '{0}' or ", s);

                    //addQueryStr = addQueryStr.TrimEnd(" or".ToCharArray()) + ")";
                }
                QueryStr = QueryStr.TrimEnd(" or ".ToCharArray()) + ")";
                QueryStr += " and ";
            }

            if (sensor.Count != 0)
            {
                QueryStr += " (";
                foreach (string s in sensor)
                {
                    QueryStr += string.Format(" SENSOR = '{0}' or ", s);
                }
                QueryStr = QueryStr.TrimEnd(" or ".ToCharArray()) + ")";
                QueryStr += " and ";

            }

            //缺昼夜标记

            if (PixelSpacing.Count != 0)
            {
                QueryStr += " (";
                foreach (string s in PixelSpacing)
                {
                    QueryStr += string.Format("PIXELSPACING = '{0}' or ", s);
                }
                QueryStr = QueryStr.TrimEnd(" or ".ToCharArray()) + ")";
                QueryStr += "and";
            }
            QueryStr = QueryStr.TrimEnd(" and ".ToCharArray());

            //缺数据大小范围
            //缺云量范围

            return QueryStr.ToString();
        }

        /// <summary>
        /// 获取产品生产流程的查询条件语句
        /// </summary>
        /// <param name="ProductEnName"></param>
        /// <param name="ProductCnName"></param>
        /// <param name="ProductLevel"></param>
        /// <param name="version"></param>
        /// <returns></returns>
        public string GetQueryCondition_ProductWFL(string ProductEnName, string ProductCnName, string ProductLevel, string version)
        {
            string QueryStr = "";
            //生产流程英文名字
            if (!string.IsNullOrEmpty(ProductEnName))
            {
                QueryStr += " (";
                QueryStr += String.Format("ProName = '{0}'", ProductEnName.Trim());
                QueryStr += ") and ";
            }
            //生产流程中文名字
            if (!string.IsNullOrEmpty(ProductCnName))
            {
                QueryStr += " (";
                QueryStr += String.Format("ProChsName like '%{0}%'", ProductCnName.Trim());
                QueryStr += ") and ";
            }
            //生产产品级别
            if (!string.IsNullOrEmpty(ProductLevel))
            {
                //QueryStr += " (";
                //QueryStr += String.Format("ProChsName = '{0}'", ProductLevel.Trim());
                //QueryStr += ") and ";
            }
            //算法版本号
            if (!string.IsNullOrEmpty(version))
            {
                QueryStr += " (";
                QueryStr += String.Format("ProVersion = '{0}'", version.Trim());
                QueryStr += ") and ";
            }

            QueryStr = QueryStr.TrimEnd(" and".ToCharArray());

            return QueryStr.ToString();
        }
        #endregion

        #region 拼接查询的条件语句（JCGX）
        /// <summary>
        /// 未用到
        /// </summary>
        public enum JCGXQueryDataType
        {
            /// <summary>
            /// 纠正后数据
            /// </summary>
            CorrectedData,
            /// <summary>
            /// 切片数据（产品切片）
            /// </summary>
            TileData,
            /// <summary>
            /// 算法组件
            /// </summary>
            AlgorithmCMP,
            /// <summary>
            /// 文档
            /// </summary>
            Document,
            /// <summary>
            /// 工具包
            /// </summary>
            Toolkit
        }
        /// <summary>
        /// 未用到
        /// </summary>
        /// <param name="jcgxQueryType"></param>
        /// <returns></returns>
        public string GetQueryCondition_JCGXQuery(JCGXQueryDataType jcgxQueryType)
        {
            string returnQueryString = string.Empty;
            switch (jcgxQueryType)
            {
                case JCGXQueryDataType.CorrectedData:
                    break;
                case JCGXQueryDataType.TileData:
                    break;
                case JCGXQueryDataType.AlgorithmCMP:
                    break;
                case JCGXQueryDataType.Document:
                    break;
                case JCGXQueryDataType.Toolkit:
                    break;
                default:
                    break;
            }
            return returnQueryString;

        }
        /// <summary>
        /// 拼接算法组件查询的条件语句（）
        /// </summary>
        /// <param name="listUploadDate"></param>
        /// <param name="ArtificialType"></param>
        /// <param name="AlgEnName"></param>
        /// <param name="DllName"></param>
        /// <param name="AlgCnName"></param>
        /// <param name="SuitableSatellites"></param>
        /// <param name="SuitableSensors"></param>
        /// <param name="IsOnCloud"></param>
        /// <returns></returns>
        public string GetQueryCondition_AlgorithmJCGX(string AddQueryTable, List<DateTime> listUploadDate, List<string> ArtificialType, List<string> AlgEnName, List<string> DllName, List<string> AlgCnName, List<string> SuitableSatellites, List<string> SuitableSensors, string IsOnCloud)
        {
            string QueryStr = "";

            //时间范围
            if (listUploadDate.Count == 2)
            {
                QueryStr += "(";
                DateTime dt1 = listUploadDate[0];
                DateTime dt2 = listUploadDate[1];
                QueryStr += string.Format("UpLoadTime between '{0}' and '{1}'", dt1, dt2);
                //QueryStr += searchHJ.getTime(Convert.ToString(dt1), Convert.ToString(dt2), "SCENEDATE");
                QueryStr += ") and";
            }

            //人工交互类型

            if (ArtificialType.Count != 0)
            {
                QueryStr += " (";
                foreach (string s in ArtificialType)
                {
                    QueryStr += string.Format(" Artificial = '{0}' or ", s);

                    //addQueryStr = addQueryStr.TrimEnd(" or".ToCharArray()) + ")";
                }
                QueryStr = QueryStr.TrimEnd(" or ".ToCharArray()) + ")";
                QueryStr += " and ";
            }

            //算法英文名（模糊查询）
            //if (AlgEnName != "")
            //{
            //    QueryStr += string.Format("( AlgorithmName like '%{0}%')", AlgEnName);

            //    QueryStr += "and ";
            //}
            if (AlgEnName.Count != 0)
            {
                QueryStr += " (";
                foreach (string s in AlgEnName)
                {
                    QueryStr += string.Format("AlgorithmName like '%{0}%' or ", s);
                }
                QueryStr = QueryStr.TrimEnd(" or ".ToCharArray()) + ")";
                QueryStr += "and ";
            }
            //算法用到DLL名称（模糊查询）
            //if (DllName != "")
            //{
            //    QueryStr += string.Format("( DllName like '%{0}%')", AlgCnName);

            //    QueryStr += "and ";
            //}
            if (DllName.Count != 0)
            {
                QueryStr += " (";
                foreach (string s in DllName)
                {
                    QueryStr += string.Format("DllName like '%{0}%' or ", s);
                }
                QueryStr = QueryStr.TrimEnd(" or ".ToCharArray()) + ")";
                QueryStr += "and ";
            }
            //算法中文名（模糊查询）
            //if (AlgCnName != "")
            //{
            //    QueryStr += string.Format("( ChsName like '%{0}%')", AlgCnName);

            //    QueryStr += "and ";
            //}
            if (AlgCnName.Count != 0)
            {
                QueryStr += " (";
                foreach (string s in AlgCnName)
                {
                    QueryStr += string.Format("ChsName like '%{0}%' or ", s);
                }
                QueryStr = QueryStr.TrimEnd(" or ".ToCharArray()) + ")";
                QueryStr += "and ";
            }

            //算法适用的卫星类型和传感器类型
            if (!(SuitableSatellites.Count == 0 && SuitableSensors.Count == 0))
            {
                //string AddQueryTable="madb_parautility_view";
                string QueryStrAdd = GetAlgQueryStr_SuitableSateSensor(SuitableSatellites, SuitableSensors, AddQueryTable);
                QueryStr += QueryStrAdd;
                QueryStr += "and ";
            }
            //是否在公有云中
            if (!string.IsNullOrEmpty(IsOnCloud))
            {
                QueryStr += " (";
                QueryStr += String.Format("PublicCloud = '{0}'", IsOnCloud.Trim());
                QueryStr += ") and";
            }
            QueryStr = QueryStr.TrimEnd(" and ".ToCharArray());

            return QueryStr;
        }

        private string GetAlgQueryStr_SuitableSateSensor(List<string> SuitableSatellites, List<string> SuitableSensors, string AddQueryTableName)
        {
            string strSQL = string.Empty;

            strSQL += " (";
            strSQL += string.Format("QRST_CODE in (select DISTINCT ParaID from {0} where (", AddQueryTableName);
            if (SuitableSatellites.Count != 0)
            {
                strSQL += " (";
                foreach (string s in SuitableSatellites)
                {
                    strSQL += string.Format("Satellite = '{0}' or ", s.Trim());
                }
                strSQL = strSQL.TrimEnd(" or ".ToCharArray()) + ")";
                strSQL += " and ";
            }
            if (SuitableSensors.Count != 0)
            {
                strSQL += " (";
                foreach (string s in SuitableSensors)
                {
                    strSQL += string.Format("Sensor = '{0}' or ", s.Trim());
                }
                strSQL = strSQL.TrimEnd(" or ".ToCharArray()) + ")";
                strSQL += " and ";
            }
            strSQL = strSQL.TrimEnd(" and ".ToCharArray());
            strSQL += " ))) ";
            return strSQL;
        }
        /// <summary>
        /// 拼接文档查询的条件语句（）
        /// </summary>
        /// <param name="DocumentName"></param>
        /// <param name="ProgramName"></param>
        /// <param name="Author"></param>
        /// <param name="KeyWords"></param>
        /// <param name="IsOnCloud"></param>
        /// <returns></returns>
        public string GetQueryCondition_DocumentJCGX(List<string> DocumentName,List<string> DocumentType, List<string> ProgramName,List<DateTime> DocumentReleaseTime, List<string> Author, List<string> KeyWords, string IsOnCloud)
        {
            string QueryStr = "";

            //文档名称
            if (DocumentName.Count != 0)
            {
                QueryStr += " (";
                foreach (string s in DocumentName)
                {
                    QueryStr += string.Format("documentname like '%{0}%' or ", s);
                }
                QueryStr = QueryStr.TrimEnd(" or ".ToCharArray()) + ")";
                QueryStr += "and ";
            }

            //文档类型
            if (DocumentType.Count != 0)
            {
                QueryStr += " (";
                foreach (string s in DocumentType)
                {
                    QueryStr += string.Format("documenttype like '%{0}%' or ", s);
                }
                QueryStr = QueryStr.TrimEnd(" or ".ToCharArray()) + ")";
                QueryStr += "and ";
            }

            //项目名称
            if (ProgramName.Count != 0)
            {
                QueryStr += " (";
                foreach (string s in ProgramName)
                {
                    QueryStr += string.Format("programname like '%{0}%' or ", s);
                }
                //foreach (string s in ProgramName)
                //{
                //    QueryStr += string.Format("programname = '{0}' or ", s);
                //}
                QueryStr = QueryStr.TrimEnd(" or ".ToCharArray()) + ")";
                QueryStr += "and ";
            }

            //时间范围
            if (DocumentReleaseTime.Count == 2)
            {
                QueryStr += "(";
                DateTime dt1 = DocumentReleaseTime[0];
                DateTime dt2 = DocumentReleaseTime[1];
                QueryStr += string.Format("filetime between '{0}' and '{1}'", dt1, dt2);
                //QueryStr += searchHJ.getTime(Convert.ToString(dt1), Convert.ToString(dt2), "SCENEDATE");
                QueryStr += ") and";
            }

            //作者
            if (Author.Count != 0)
            {
                QueryStr += " (";
                foreach (string s in Author)
                {
                    QueryStr += string.Format("author like '%{0}%' or ", s);
                }
                QueryStr = QueryStr.TrimEnd(" or ".ToCharArray()) + ")";
                QueryStr += "and ";
            }

            //关键字
            if (KeyWords.Count != 0)
            {
                QueryStr += " (";
                foreach (string s in KeyWords)
                {
                    QueryStr += string.Format("keywords like '%{0}%' or ", s);
                }
                QueryStr = QueryStr.TrimEnd(" or ".ToCharArray()) + ")";
                QueryStr += "and ";
            }

            //是否在公有云中
            if (!string.IsNullOrEmpty(IsOnCloud))
            {
                QueryStr += String.Format("PublicCloud = '{0}'", IsOnCloud.Trim());
                QueryStr += " and ";
            }
            QueryStr = QueryStr.TrimEnd(" and".ToCharArray());

            return QueryStr.ToString();
        }
        /// <summary>
        /// 拼接工具查询的条件语句（）
        /// </summary>
        /// <param name="ToolkitName"></param>
        /// <param name="OStype"></param>
        /// <param name="Author"></param>
        /// <param name="KeyWords"></param>
        /// <param name="IsOnCloud"></param>
        /// <returns></returns>
        public string GetQueryCondition_ToolkitJCGX(List<DateTime> listReleaseTime, List<string> ToolkitName, List<string> ToolkitType, List<string> SuitableSatellites, List<string> SuitableSensors, List<string> OSBits, List<string> OStype, List<string> Author, List<string> KeyWords, string IsOnCloud)
        {
            string QueryStr = "";

            //时间范围
            if (listReleaseTime.Count == 2)
            {
                QueryStr += "(";
                DateTime dt1 = listReleaseTime[0];
                DateTime dt2 = listReleaseTime[1];
                QueryStr += string.Format("releaseTime between '{0}' and '{1}'", dt1, dt2);
                //QueryStr += searchHJ.getTime(Convert.ToString(dt1), Convert.ToString(dt2), "SCENEDATE");
                QueryStr += ") and";
            }

            //工具名称
            if (ToolkitName.Count!=0)
            {
                QueryStr += " (";
                foreach (string s in ToolkitName)
                {
                    QueryStr += string.Format("toolname like '%{0}%' or ", s);
                }
                QueryStr = QueryStr.TrimEnd(" or ".ToCharArray()) + ")";
                QueryStr += "and";
                //QueryStr += " (";
                //QueryStr += String.Format("toolname = '{0}'", ToolkitName.Trim());
                //QueryStr += ") and ";
            }
            //工具类型
            if (ToolkitType.Count != 0)
            {
                QueryStr += " (";
                foreach (string s in ToolkitType)
                {
                    QueryStr += string.Format(" type like '%{0}%' or ", s);
                }
                QueryStr = QueryStr.TrimEnd(" or ".ToCharArray()) + ")";
                QueryStr += " and ";
            }
            //List<string> SuitableSatellites, List<string> SuitableSensors, List<string> OSBits,
            //适用的卫星类型
            if (SuitableSatellites.Count != 0)
            {
                QueryStr += " (";
                foreach (string s in SuitableSatellites)
                {
                    QueryStr += string.Format("satelliteId like '%{0}%' or ", s);
                }
                QueryStr = QueryStr.TrimEnd(" or ".ToCharArray()) + ")";
                QueryStr += "and";
            }
            //适用的传感器类型
            if (SuitableSensors.Count != 0)
            {
                QueryStr += " (";
                foreach (string s in SuitableSensors)
                {
                    QueryStr += string.Format("sensorId like '%{0}%' or ", s);
                }
                QueryStr = QueryStr.TrimEnd(" or ".ToCharArray()) + ")";
                QueryStr += "and";
            }
            //操作系统位数
            if (OSBits.Count != 0)
            {
                QueryStr += " (";
                foreach (string s in OSBits)
                {
                    QueryStr += string.Format(" bit = '{0}' or ", s);
                }
                QueryStr = QueryStr.TrimEnd(" or ".ToCharArray()) + ")";
                QueryStr += " and ";
            }
            //工具使用的操作系统OS平台
            if (OStype.Count != 0)
            {
                QueryStr += " (";
                foreach (string s in OStype)
                {
                    QueryStr += string.Format("OS like '%{0}%' or ", s);
                }
                QueryStr = QueryStr.TrimEnd(" or ".ToCharArray()) + ")";
                QueryStr += "and";
            }

            //作者
            if (Author.Count != 0)
            {
                QueryStr += " (";
                foreach (string s in Author)
                {
                    QueryStr += string.Format("author like '%{0}%' or ", s);
                }
                QueryStr = QueryStr.TrimEnd(" or ".ToCharArray()) + ")";
                QueryStr += "and";
            }

            //关键字
            if (KeyWords.Count != 0)
            {
                QueryStr += " (";
                foreach (string s in KeyWords)
                {
                    QueryStr += string.Format("keywords like '%{0}%' or ", s);
                }
                QueryStr = QueryStr.TrimEnd(" or ".ToCharArray()) + ")";
                QueryStr += "and";
            }

            //是否在公有云中
            if (!string.IsNullOrEmpty(IsOnCloud))
            {
                QueryStr += " (";
                QueryStr += String.Format("PublicCloud = '{0}'", IsOnCloud.Trim());
                QueryStr += ") and";
            }
            QueryStr = QueryStr.TrimEnd(" and ".ToCharArray());

            return QueryStr.ToString();
        }
        /// <summary>
        /// 拼接纠正后数据查询的条件语句（），查询用户上传的纠正后数据
        /// </summary>
        /// <param name="position"></param>
        /// <param name="datetime"></param>
        /// <param name="satellite"></param>
        /// <param name="sensor"></param>
        /// <param name="PixelSpacing"></param>
        /// <param name="DataSizeRange"></param>
        /// <param name="IsOnCloud"></param>
        /// <param name="dataType"></param>
        /// <returns></returns>
        public string GetQueryCondition_CorrectedDataJCGX(List<string> position, List<DateTime> datetime, List<string> satellite, List<string> sensor, List<string> PixelSpacing, List<int> DataSizeRange, string IsOnCloud, string dataType)
        {
            string QueryStr = "";
            if (!string.IsNullOrEmpty(dataType))
            {
                QueryStr += String.Format("( DataType = '{0}' )", dataType);
                QueryStr += " and ";
            }

            //空间范围
            if (position.Count == 4)
            {
                string minLatValue = position[0];
                string minLonValue = position[1];
                string maxLatValue = position[2];
                string maxLonValue = position[3];

                QueryStr += String.Format("((dataUpperLeftLat>'{0}' or dataUpperRightLat>'{0}') and (dataLowerLeftLat<'{1}' or dataLowerRightLat<'{1}') and (dataUpperLeftLong<'{2}' or dataLowerLeftLong<'{2}')and (dataUpperRightLong>'{3}' or dataLowerRightLong>'{3}')) ", minLatValue, maxLatValue, maxLonValue, minLonValue);
                QueryStr += " and";
            }
            //时间范围
            if (datetime.Count == 2)
            {
                QueryStr += "(";
                DateTime dt1 = datetime[0];
                DateTime dt2 = datetime[1];
                QueryStr += string.Format("sceneDate between '{0}' and '{1}'", dt1, dt2);
                //QueryStr += searchHJ.getTime(Convert.ToString(dt1), Convert.ToString(dt2), "SCENEDATE");
                QueryStr += ") and";
            }
            //卫星类型
            if (satellite.Count != 0)
            {
                QueryStr += " (";
                foreach (string s in satellite)
                {
                    QueryStr += string.Format(" satelliteId = '{0}' or ", s);

                    //addQueryStr = addQueryStr.TrimEnd(" or".ToCharArray()) + ")";
                }
                QueryStr = QueryStr.TrimEnd(" or ".ToCharArray()) + ")";
                QueryStr += " and ";
            }
            //传感器
            if (sensor.Count != 0)
            {
                QueryStr += " (";
                foreach (string s in sensor)
                {
                    QueryStr += string.Format(" sensorId = '{0}' or ", s);
                }
                QueryStr = QueryStr.TrimEnd(" or ".ToCharArray()) + ")";
                QueryStr += " and ";

            }
            //分辨率
            if (PixelSpacing.Count != 0)
            {
                QueryStr += " (";
                foreach (string s in PixelSpacing)
                {
                    QueryStr += string.Format("pixelSpacing = '{0}' or ", s);
                }
                QueryStr = QueryStr.TrimEnd(" or ".ToCharArray()) + ")";
                QueryStr += " and";
            }
            //数据大小范围
            if (DataSizeRange.Count == 2)
            {
                QueryStr += " (";
                int sizeRangeBegin = DataSizeRange[0];
                int sizeRangeEnd = DataSizeRange[1];
                QueryStr += string.Format("FileSize > '{0}' and FileSize < '{1}'", sizeRangeBegin, sizeRangeEnd);
                //QueryStr += searchHJ.getTime(Convert.ToString(dt1), Convert.ToString(dt2), "SCENEDATE");
                QueryStr += ") and";
            }
            //是否在公有云中
            if (!string.IsNullOrEmpty(IsOnCloud))
            {
                QueryStr += " (";
                QueryStr += String.Format("PublicCloud = '{0}'", IsOnCloud.Trim());
                QueryStr += ") and";
            }
            QueryStr = QueryStr.TrimEnd(" and ".ToCharArray());
            

            return QueryStr.ToString();
        }
        /// <summary>
        /// 拼接纠正后数据查询的条件语句（），查询数据库中标准的纠正后数据
        /// </summary>
        /// <param name="position"></param>
        /// <param name="datetime"></param>
        /// <param name="satellite"></param>
        /// <param name="sensor"></param>
        /// <param name="PixelSpacing"></param>
        /// <returns></returns>
        public string GetQueryCondition_CorrectedDataJCGX_1(List<string> position, List<DateTime> datetime, List<string> satellite, List<string> sensor, List<string> PixelSpacing, List<int> DataSizeRange, string IsOnCloud, string dataType)
        {
            string QueryStr = "";
            if (!string.IsNullOrEmpty(dataType))
            {
                QueryStr += String.Format("( DataType = '{0}' )", dataType);
                QueryStr += " and ";
            }
            //空间范围
            if (position.Count == 4)
            {
                string minLatValue = position[0];
                string minLonValue = position[1];
                string maxLatValue = position[2];
                string maxLonValue = position[3];

                QueryStr += String.Format("((dataUpperLeftLat>'{0}' or dataUpperRightLat>'{0}') and (dataLowerLeftLat<'{1}' or dataLowerRightLat<'{1}') and (dataUpperLeftLong<'{2}' or dataLowerLeftLong<'{2}')and (dataUpperRightLong>'{3}' or dataLowerRightLong>'{3}')) ", minLatValue, maxLatValue, maxLonValue, minLonValue);
                QueryStr += " and";
            }
            //时间范围
            if (datetime.Count == 2)
            {
                QueryStr += "(";
                DateTime dt1 = datetime[0];
                DateTime dt2 = datetime[1];
                QueryStr += string.Format("SCENEDATE between '{0}' and '{1}'", dt1, dt2);
                //QueryStr += searchHJ.getTime(Convert.ToString(dt1), Convert.ToString(dt2), "SCENEDATE");
                QueryStr += ") and";
            }
            //卫星类型
            if (satellite.Count != 0)
            {
                QueryStr += " (";
                foreach (string s in satellite)
                {
                    QueryStr += string.Format(" SATELLITE = '{0}' or ", s);

                    //addQueryStr = addQueryStr.TrimEnd(" or".ToCharArray()) + ")";
                }
                QueryStr = QueryStr.TrimEnd(" or ".ToCharArray()) + ")";
                QueryStr += " and ";
            }
            //传感器
            if (sensor.Count != 0)
            {
                QueryStr += " (";
                foreach (string s in sensor)
                {
                    QueryStr += string.Format(" SENSOR = '{0}' or ", s);
                }
                QueryStr = QueryStr.TrimEnd(" or ".ToCharArray()) + ")";
                QueryStr += " and ";

            }
            //分辨率
            if (PixelSpacing.Count != 0)
            {
                QueryStr += " (";
                foreach (string s in PixelSpacing)
                {
                    QueryStr += string.Format("PIXELSPACING = '{0}' or ", s);
                }
                QueryStr = QueryStr.TrimEnd(" or ".ToCharArray()) + ")";
                QueryStr += "and";
            }

            //数据大小范围
            if (DataSizeRange.Count == 2)
            {
                QueryStr += " (";
                int sizeRangeBegin = DataSizeRange[0];
                int sizeRangeEnd = DataSizeRange[1];
                QueryStr += string.Format("FileSize > '{0}' and FileSize < '{1}'", sizeRangeBegin, sizeRangeEnd);
                //QueryStr += searchHJ.getTime(Convert.ToString(dt1), Convert.ToString(dt2), "SCENEDATE");
                QueryStr += ") and";
            }
            //是否在公有云中
            if (!string.IsNullOrEmpty(IsOnCloud))
            {
                QueryStr += " (";
                QueryStr += String.Format("PublicCloud = '{0}'", IsOnCloud.Trim());
                QueryStr += ") and";
            }
            QueryStr = QueryStr.TrimEnd(" and ".ToCharArray());

            QueryStr = QueryStr.TrimEnd(" and ".ToCharArray());
            //缺云量范围

            return QueryStr.ToString();
        }
        #region 用户评价信息的查询，添加，删除，以及获取评价数量
        /// <summary>
        /// 组成查询用户评价信息条件
        /// </summary>
        /// <param name="DataId"></param>
        /// <param name="UserId"></param>
        /// <returns></returns>
        public string GetQueryCondition_UserEvaluation(string DataId, string UserId)
        {
            string QueryStr = "";
            if (!string.IsNullOrEmpty(DataId))
            {
                QueryStr = QueryStr + string.Format(" dataID = '{0}' and ", DataId);
            }
            if (!string.IsNullOrEmpty(UserId))
            {
                QueryStr = QueryStr + string.Format(" UserID = '{0}'", UserId);
            }
            QueryStr = QueryStr.TrimEnd(" and ".ToCharArray());
            return QueryStr;
        }

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public bool AddUserEvaluation(string dataID, string UserID, string Description, int Score, string BuyId, IDbBaseUtilities sqlBase)
        {
            DateTime EvTime = DateTime.Now;
            //TableLocker dblock = new TableLocker(sqlBase);
            Constant.IdbOperating.LockTable("isdb_userevaluation",EnumDBType.MIDB);
            int ID = sqlBase.GetMaxID("ID", "isdb_userevaluation");
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into isdb_userevaluation(");
            strSql.Append("ID,dataID,UserID,EvTime,Description,Score,BuyId)");
            strSql.Append(" values (");
            strSql.Append(string.Format("{0},'{1}','{2}','{3}','{4}',{5},'{6}')", ID, dataID, UserID, EvTime, Description,
                Score, BuyId));
     //       strSql.Append("@ID,@dataID,@UserID,@EvTime,@Description,@Score,@BuyId)");
     //       MySqlParameter[] parameters = {
					//new MySqlParameter("@ID", MySqlDbType.Int32,7),
					//new MySqlParameter("@dataID", MySqlDbType.VarChar,200),
					//new MySqlParameter("@UserID", MySqlDbType.Text),
					//new MySqlParameter("@EvTime", MySqlDbType.DateTime),
					//new MySqlParameter("@Description", MySqlDbType.Text),
					//new MySqlParameter("@Score", MySqlDbType.Int32,11),
					//new MySqlParameter("@BuyId", MySqlDbType.VarChar,25)};
     //       parameters[0].Value = ID;
     //       parameters[1].Value = dataID;
     //       parameters[2].Value = UserID;
     //       parameters[3].Value = EvTime;
     //       parameters[4].Value = Description;
     //       parameters[5].Value = Score;
     //       parameters[6].Value = BuyId;

            int rows = sqlBase.ExecuteSql(strSql.ToString());
            Constant.IdbOperating.UnlockTable("isdb_userevaluation",EnumDBType.MIDB);
            if (rows > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool DeleteUserEvaluation(int ID, IDbBaseUtilities sqlBase)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(String.Format("delete from isdb_userevaluation where ID={0}",ID));
            //strSql.Append(" where ID=@ID ");
            //MySqlParameter[] parameters = {
            //        new MySqlParameter("@ID", MySqlDbType.Int32,7)			};
            //parameters[0].Value = ID;
            int rows = sqlBase.ExecuteSql(strSql.ToString());
            if (rows > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public int GetEvaluationCount(string dataID, IDbBaseUtilities sqlBase)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) FROM isdb_userevaluation ");
            strSql.AppendFormat(" where dataID = '{0}' ", dataID);
            DataSet ds = sqlBase.GetDataSet(strSql.ToString());
            if (ds.Tables[0].Rows[0][0] != null)
            {
                return Convert.ToInt32(ds.Tables[0].Rows[0][0]);
            }
            else
            {
                return 0;
            }
        }
        #endregion

        #region 更新 公共信息
        public bool UpdatePublicInfo(string DataID, string Price, int DownloadCount, double AverageScore, IDbBaseUtilities sqlBase)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(
                string.Format(
                    "update isdb_publicinfo set Price='{0}',DownloadCount={1},AverageScore={2} where DataID='{3}'",
                    Price, DownloadCount, AverageScore, DataID));
     //       strSql.Append("update isdb_publicinfo set ");
     //       strSql.Append("Price=@Price,");
     //       strSql.Append("DownloadCount=@DownloadCount,");
     //       strSql.Append("AverageScore=@AverageScore");
     //       strSql.Append(" where DataID=@DataID ");
     //       MySqlParameter[] parameters = {
					//new MySqlParameter("@Price", MySqlDbType.Text),
					//new MySqlParameter("@DownloadCount", MySqlDbType.Int32,11),
					//new MySqlParameter("@AverageScore", MySqlDbType.Double,10),
					//new MySqlParameter("@DataID", MySqlDbType.Text)};
     //       parameters[0].Value = Price;
     //       parameters[1].Value = DownloadCount;
     //       parameters[2].Value = AverageScore;
     //       parameters[3].Value = DataID;

            int rows = sqlBase.ExecuteSql(strSql.ToString());
            if (rows > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        #endregion
        #endregion

        #region 省市县信息查询 zxw
        /// <summary>
        /// 根据国家查询省
        /// </summary>
        /// <param name="nations"></param>
        /// <param name="sqlBase"></param>
        /// <returns></returns>
        public DataSet SearchProvinces(IDbBaseUtilities sqlBase)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat(" CODE like '__0000000'");
            return SearchRegion(sb.ToString(), sqlBase);
        }

        /// <summary>
        /// 根据省，查该省得地级市
        /// </summary>
        /// <param name="province"></param>
        /// <param name="sqlBase"></param>
        /// <returns></returns>
        public DataSet SearchCityByProvince(string province, IDbBaseUtilities sqlBase)
        {
            DataSet ds = SearchRegion(string.Format(" NAME = '{0}'", province), sqlBase);
            if (ds != null)
            {
                string provinceCode = ds.Tables[0].Rows[0][0].ToString();
                if (!string.IsNullOrEmpty(provinceCode) && provinceCode.Substring(2, 7) == "0000000")
                {
                    string subCode = provinceCode.Substring(0, 2);
                    StringBuilder sb = new StringBuilder();
                    sb.AppendFormat(" CODE like '{0}__00000' and CODE != '{1}'", subCode, provinceCode);
                    return SearchRegion(sb.ToString(), sqlBase);
                }
            }
            return null;
        }

        /// <summary>
        /// 根据城市名称，查询该城市所有的县市
        /// </summary>
        /// <param name="cityName"></param>
        /// <param name="sqlBase"></param>
        /// <returns></returns>
        public DataSet SearchCountyByCity(string provinceName, string cityName, IDbBaseUtilities sqlBase)
        {
            DataSet ds0 = SearchRegion(string.Format(" NAME = '{0}'", provinceName), sqlBase);
            if (ds0 == null || ds0.Tables[0].Rows.Count == 0)
            {
                return null;
            }
            string provinceCode = ds0.Tables[0].Rows[0][0].ToString();
            DataSet ds = SearchRegion(string.Format(" NAME = '{0}' and CODE like '{1}__00000'", cityName, provinceCode.Substring(0, 2)), sqlBase);
            if (ds != null)
            {
                string cityCode = ds.Tables[0].Rows[0][0].ToString();
                if (!string.IsNullOrEmpty(cityCode))
                {
                    string subCode = cityCode.Substring(0, 4);
                    StringBuilder sb = new StringBuilder();
                    sb.AppendFormat(" CODE like '{0}__000' and CODE != '{1}'", subCode, cityCode);
                    return SearchRegion(sb.ToString(), sqlBase);
                }
            }
            return null;
        }

        public DataSet SearchRegion(string whereCondition, IDbBaseUtilities sqlBase)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select CODE,NAME,FULLNAME from areainfo_view ");
            if (!string.IsNullOrEmpty(whereCondition))
            {
                strSql.AppendFormat(" where {0}", whereCondition);
            }
            return sqlBase.GetDataSet(strSql.ToString());
        }
        #endregion


        public string GetQueryCondition_GFFDataJCGX(List<string> position, List<DateTime> listDatetime, List<string> satellite, List<string> sensor, List<string> PixelSpacing, List<int> DataSizeRange, string IsOnCloud)
        {
            string QueryStr = "";
            //空间范围
            if (position.Count == 4)
            {
                string minLatValue = position[0];
                string minLonValue = position[1];
                string maxLatValue = position[2];
                string maxLonValue = position[3];

                QueryStr += String.Format("((dataUpperLeftLat>'{0}' or dataUpperRightLat>'{0}') and (dataLowerLeftLat<'{1}' or dataLowerRightLat<'{1}') and (dataUpperLeftLong<'{2}' or dataLowerLeftLong<'{2}')and (dataUpperRightLong>'{3}' or dataLowerRightLong>'{3}')) ", minLatValue, maxLatValue, maxLonValue, minLonValue);
                QueryStr += " and";
            }
            //时间范围
            if (listDatetime.Count == 2)
            {
                QueryStr += "(";
                DateTime dt1 = listDatetime[0];
                DateTime dt2 = listDatetime[1];
                QueryStr += string.Format("sceneDate between '{0}' and '{1}'", dt1, dt2);
                //QueryStr += searchHJ.getTime(Convert.ToString(dt1), Convert.ToString(dt2), "SCENEDATE");
                QueryStr += ") and";
            }
            //卫星类型
            if (satellite.Count != 0)
            {
                QueryStr += " (";
                foreach (string s in satellite)
                {
                    QueryStr += string.Format(" satellite = '{0}' or ", s);

                    //addQueryStr = addQueryStr.TrimEnd(" or".ToCharArray()) + ")";
                }
                QueryStr = QueryStr.TrimEnd(" or ".ToCharArray()) + ")";
                QueryStr += " and ";
            }
            //传感器
            if (sensor.Count != 0)
            {
                QueryStr += " (";
                foreach (string s in sensor)
                {
                    QueryStr += string.Format(" sensor = '{0}' or ", s);
                }
                QueryStr = QueryStr.TrimEnd(" or ".ToCharArray()) + ")";
                QueryStr += " and ";

            }
            //分辨率
            if (PixelSpacing.Count != 0)
            {
                QueryStr += " (";
                foreach (string s in PixelSpacing)
                {
                    QueryStr += string.Format("pixelSpacing = '{0}' or ", s);
                }
                QueryStr = QueryStr.TrimEnd(" or ".ToCharArray()) + ")";
                QueryStr += "and";
            }
            //数据大小范围
            if (DataSizeRange.Count == 2)
            {
                QueryStr += "(";
                int sizeRangeBegin = DataSizeRange[0];
                int sizeRangeEnd = DataSizeRange[1];
                QueryStr += string.Format("FileSize > '{0}' and FileSize < '{1}'", sizeRangeBegin, sizeRangeEnd);
                //QueryStr += searchHJ.getTime(Convert.ToString(dt1), Convert.ToString(dt2), "SCENEDATE");
                QueryStr += ") and";
            }
            //是否在公有云中
            if (!string.IsNullOrEmpty(IsOnCloud))
            {
                QueryStr += " (";
                QueryStr += String.Format("PublicCloud = '{0}'", IsOnCloud.Trim());
                QueryStr += ") and";
            }
            QueryStr = QueryStr.TrimEnd(" and ".ToCharArray());
            //缺云量范围

            return QueryStr.ToString();
        }
    }
}

