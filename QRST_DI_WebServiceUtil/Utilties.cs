using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
 
namespace QRST_DI_WebServiceUtil
{
    /// <summary>
    ///Utilties 的摘要说明
    /// </summary>
    public class Utilties
    {
        /// <summary>
        /// 将对象中的字段序列化成行
        /// </summary>
        /// <param name="objLst"></param>
        /// <returns></returns>
        public static DataTable ConvertObjField2DataTable(List<object> objLst)
        {
            if (objLst == null || objLst.Count == 0)
            {
                return null;
            }
            else
            {
                DataTable dt = new DataTable();
                Type objtype = objLst[0].GetType();
                FieldInfo[] fieldinfo = objtype.GetFields();
                for (int i = 0; i < fieldinfo.Length; i++)
                {
                    Type colType = fieldinfo[i].FieldType;
                    if ((colType.IsGenericType) && (colType.GetGenericTypeDefinition() == typeof(Nullable<>)))
                    {
                        colType = colType.GetGenericArguments()[0];
                    }
                    DataColumn dc = new DataColumn() { ColumnName = fieldinfo[i].Name, DataType = colType };
                    dt.Columns.Add(dc);
                }
                for (int j = 0; j < objLst.Count; j++)
                {
                    DataRow dr = dt.NewRow();
                    for (int k = 0; k < fieldinfo.Length; k++)
                    {
                        dr[k] = fieldinfo[k].GetValue(objLst[j]) == null ? DBNull.Value : fieldinfo[k].GetValue(objLst[j]);
                    }
                    dt.Rows.Add(dr);
                }
                return dt;
            }
        }

        /// <summary>
        /// 将属性序列化成行
        /// </summary>
        /// <param name="objLst"></param>
        /// <returns></returns>
        public static DataTable ConvertObjPro2DataTable(List<object> objLst)
        {
            if (objLst == null || objLst.Count == 0)
            {
                return null;
            }
            else
            {
                DataTable dt = new DataTable();
                Type objtype = objLst[0].GetType();
                PropertyInfo[] propertyInfoArr = objtype.GetProperties();
                for (int i = 0; i < propertyInfoArr.Length; i++)
                {
                    DataColumn dc = new DataColumn();
                    dc.ColumnName = propertyInfoArr[i].Name;

                    Type colType = propertyInfoArr[i].PropertyType;
                    if ((colType.IsGenericType) && (colType.GetGenericTypeDefinition() == typeof(Nullable<>)))
                    {
                        colType = colType.GetGenericArguments()[0];
                    }
                    dc.DataType = colType;
                    dt.Columns.Add(dc);
                }
                for (int j = 0; j < objLst.Count; j++)
                {
                    DataRow dr = dt.NewRow();
                    for (int k = 0; k < propertyInfoArr.Length; k++)
                    {
                        dr[k] = propertyInfoArr[k].GetValue(objLst[j], null) == null ? DBNull.Value : propertyInfoArr[k].GetValue(objLst[j], null);
                    }
                    dt.Rows.Add(dr);
                }
                return dt;
            }
        }
    }
}