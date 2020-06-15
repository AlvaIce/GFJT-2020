/*
 * 作者：zxw
 * 创建时间：2013-09-24
 * 描述：描述通过插件导入的元数据，该元数据的路径规则存放于表字段relatePath,即每一个表必须要拥有relatePath
*/
using System;
using System.Collections.Generic;
using System.Text;
using QRST_DI_DS_Metadata.MetaDataDefiner.Dal;
using System.Data;
using QRST_DI_DS_Metadata.Paths;
using QRST_DI_Resources;
using QRST_DI_SS_Basis;
using QRST_DI_SS_Basis.MetaData;
using QRST_DI_SS_DBInterfaces.IDBEngine;

namespace QRST_DI_DS_Metadata.MetaDataCls
{
    public class MetaDataObject:MetaData
    {
        private int ID;
        public string table_code;
        public string relatePath;

        private Dictionary<string, string> _fieldVvalue;

        public Dictionary<string, string> fieldVvalue
        {
            set { 
                _fieldVvalue = value;
                if (_fieldVvalue.ContainsKey("relatePath"))
                {
                    relatePath = _fieldVvalue["relatePath"];
                }
                else
                    throw new Exception("没有定义元数据的相对路径！");
            }
            get { return _fieldVvalue; }
        }
        public string keyField;                    //键值字段，用来确定记录的唯一性

        public MetaDataObject() { }

        public MetaDataObject(string _qrst_code)
        {
            QRST_CODE = _qrst_code;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="qrst_code"></param>
        /// <param name="sqlBase"></param>
        public override void GetModel(string qrst_code, IDbBaseUtilities sqlBase)
        {
            try
            {
                //qrst_code:0001-EVDB-32-1,tableCode:EVDB-32
                table_code = StoragePath.GetTableCodeByQrstCode(qrst_code);
                tablecode_Dal tableCode = new tablecode_Dal(sqlBase);
                string tableName = tableCode.GetTableName(table_code);
                StringBuilder strSql = new StringBuilder();
                strSql.AppendFormat("select * from {0} ", tableName);
                strSql.AppendFormat(" where QRST_CODE = '{0}'", qrst_code);

                using (DataSet ds = sqlBase.GetDataSet(strSql.ToString()))
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        relatePath = ds.Tables[0].Rows[0]["relatePath"].ToString();
                        IsCreated = true;
                    }
                    else
                    {
                        IsCreated = false;
                    }
                }
            }
            catch(Exception ex)
            {
                IsCreated = false;
            }
        }

        public override string GetRelateDataPath()
        {
            return relatePath;
        }

        public override void ImportData(IDbBaseUtilities sqlBase)
        {
           // base.ImportData(sqlBase);
            if(fieldVvalue == null || fieldVvalue.Count == 0)
            {
                throw new Exception("ImportData：导入的元数据为空！");
            }
            //获取数据表明
            string tableName;
            try
            {
                tablecode_Dal tableCode = new tablecode_Dal(sqlBase);
                tableName = tableCode.GetTableName(table_code);
            }
            catch(Exception ex)
            {
                throw new Exception("ImportData：获取数据表失败！"+ex.ToString());
            }
           
            try
            {
                //检测元数据是否已经存在
                if(!string.IsNullOrEmpty(keyField)&&fieldVvalue.ContainsKey(keyField))
                {
                    string keyValue = fieldVvalue[keyField];
                    string presql = string.Format("select  ID,QRST_CODE  from {0} where {1} = '{2}'",tableName,keyField,keyValue);
                    DataSet ds = sqlBase.GetDataSet(presql);
                    if (ds != null && ds.Tables.Count != 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        ID = int.Parse(ds.Tables[0].Rows[0]["ID"].ToString());
                        QRST_CODE = ds.Tables[0].Rows[0]["QRST_CODE"].ToString();
                        presql = string.Format("delete from {0} where QRST_CODE ='{1}'",tableName, QRST_CODE);
                        //DataSet ds = sqlBase.GetDataSet(presql);
                        int i = sqlBase.ExecuteSql(presql);
                    }
                }
                if(string.IsNullOrEmpty(QRST_CODE))
                {
                    //TableLocker dblock = new TableLocker(sqlBase);
                    Constant.IdbOperating.LockTable(tableName,EnumDBType.MIDB); 
                    tablecode_Dal tablecode = new tablecode_Dal(sqlBase);
                    ID = sqlBase.GetMaxID("ID", tableName);
                    QRST_CODE = tablecode.GetDataQRSTCode(tableName, ID);
                    Constant.IdbOperating.UnlockTable(tableName,EnumDBType.MIDB);
                }

                DataSet tableStruce = sqlBase.GetTableStruct( tableName);
                List<string> tableFields = new List<string>();
                for (int i = 0; i < tableStruce.Tables[0].Rows.Count;i++ )
                {
                    tableFields.Add(tableStruce.Tables[0].Rows[i][0].ToString().ToLower());
                }
                //组装sql语句
                StringBuilder insertSql = new StringBuilder();
                StringBuilder fields = new StringBuilder();
                StringBuilder values = new StringBuilder();
                foreach (KeyValuePair<string, string> kvp in fieldVvalue)
                {
                    if (tableFields.Contains(kvp.Key.ToLower()))
                    {
                          fields.Append(kvp.Key+", ");
                         values.AppendFormat("'{0}', ",kvp.Value);
                    }
                }
                fields.Append("ID,QRST_CODE ");
                values.AppendFormat("'{0}','{1}'  ",ID,QRST_CODE);

                insertSql.AppendFormat("insert into {0} ({1}) values ({2})",tableName,fields.ToString(),values.ToString());
                sqlBase.ExecuteSql(insertSql.ToString());
            }
            catch(Exception ex)
            {
                throw new Exception("ImportData：元数据导入失败："+ex.ToString());
            }
        }
    }
}
