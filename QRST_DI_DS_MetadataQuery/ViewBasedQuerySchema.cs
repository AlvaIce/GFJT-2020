/*
 * 作者：zxw
 * 创建时间：2013-08-01
 * 描述：基于视图的查询方案,即外部查询基于表视图
 * 修改时间：2013-08-06
 * 修改内容：支持将elementSet[0]标记为“*”时查询全部字段
*/ 
using System;
using System.Collections.Generic;
using QRST_DI_DS_Metadata.MetaDataDefiner.Dal;
using System.Data;
using System.Configuration;
using QRST_DI_SS_DBInterfaces.IDBEngine;

namespace QRST_DI_DS_MetadataQuery
{
    public class ViewBasedQuerySchema:IGetQuerySchema
    {
        public IDbBaseUtilities baseUtilities;
        protected string[] elementSet;                                               //查询字段码
        protected string tableCode;                                                  //数据库表
        public string tableName;
        protected string viewName;                                            //表对应的视图名，从TVMapping.config文件中读出


        public ViewBasedQuerySchema(string[] _elementSet, string _tableCode, IDbBaseUtilities _baseUtilities)
        {
            baseUtilities = _baseUtilities;
            elementSet = _elementSet;
            tableCode = _tableCode;
            tablecode_Dal tablecode = new tablecode_Dal(baseUtilities);
            tableName = tablecode.GetTableName(tableCode);

            //读取配置文件
            try
            {
              //  string str = AppDomain.CurrentDomain.BaseDirectory;
                ExeConfigurationFileMap configFileMap =new ExeConfigurationFileMap();
                configFileMap.ExeConfigFilename = string.Format(@"{0}{1}", AppDomain.CurrentDomain.BaseDirectory, "TVMapping.config");            //默认为当前路径
              //  string appPath = 
                Configuration config = ConfigurationManager.OpenMappedExeConfiguration(configFileMap,ConfigurationUserLevel.None);
                viewName = config.AppSettings.Settings[tableName].Value;
                if(string.IsNullOrEmpty(viewName))
                {
                    throw new Exception(string.Format("没有找到'{0}'对应的视图"));
                }
            }catch(Exception ex)
            {
                throw new Exception("获取视图名称失败！");
            }
        }



        /// <summary>
        /// 根据字段编码与字段名一一对应
        /// </summary>
        /// <returns></returns>
        virtual public Dictionary<string, string> GetFieldName()
        {
            try
            {
                Dictionary<string, string> dic = new Dictionary<string, string>();
                DataSet ds = GetTableStruct();

                //如果第一个字母为“*”，则返回全部字段
                if (elementSet[0].Equals("*"))
                {  
                    for (int j = 0; j < ds.Tables[0].Rows.Count; j++)
                  {
                      dic.Add(ds.Tables[0].Rows[j]["FieldCode"].ToString(), ds.Tables[0].Rows[j][0].ToString());
                  }
                    return dic;
                }

                for (int i = 0; i < elementSet.Length; i++)
                {
                    if (!dic.ContainsKey(elementSet[i]))
                    {
                        FieldElement fieldElement = new FieldElement(elementSet[i]);
                        if (fieldElement.index - 1 > ds.Tables[0].Rows.Count)
                        {
                            throw new Exception(string.Format("查询的字段'{0}'越界！",elementSet[i]));
                        }
                        dic.Add(elementSet[i], ds.Tables[0].Rows[fieldElement.index - 1][0].ToString());
                    }
                }
                return dic;
            }catch(Exception ex)       //将异常抛到上层
            {
                throw ex;
            }
        }

        public string GetTableName()
        {
            return viewName;
        }

        public DataSet GetTableStruct()
        {
            try
            {
                //SQLite迁移  jianghua 20170417
                //DataSet ds = baseUtilities.GetDataSet(string.Format("show columns from {0}", viewName));
                DataSet ds = baseUtilities.GetTableStruct(viewName);
                if (ds != null && ds.Tables[0] != null)
                {
                    DataColumn dc = new DataColumn() { ColumnName = "FieldCode", DataType = Type.GetType("System.String") };
                    //移除“null”,"key","default","Extra"列

                    //ds.Tables[0].Columns.RemoveAt(4);
                    //ds.Tables[0].Columns.RemoveAt(3);
                    //ds.Tables[0].Columns.RemoveAt(2);

                    ds.Tables[0].Columns.Add(dc);
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        ds.Tables[0].Rows[i]["FieldCode"] = string.Format("{0}.{1}", tableCode, i + 1);
                    }
                }
                else
                {
                    throw new Exception(string.Format("未能找到'{0}'的表结构信息", viewName));
                }
                return ds;
            }
            catch (Exception ex)
            {
                throw new Exception("表结构信息查询失败！" + ex);
            }
        } 
    }
}
