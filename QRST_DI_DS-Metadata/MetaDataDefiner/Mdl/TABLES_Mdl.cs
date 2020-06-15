using System;
using System.Collections.Generic;
using System.Text;

namespace QRST_DI_DS_Metadata.MetaDataDefiner.Mdl
{
    /// <summary>
    /// 实体类TABLES_Mdl 。(属性说明自动提取数据库字段的描述信息)
    /// </summary>
    [Serializable]
    public class TABLES_Mdl
    {
        public TABLES_Mdl()
        { }
        #region Model
        private string _table_catalog;
        private string _table_schema;
        private string _table_name;
        private string _table_type;
        private string _engine;
        private long _version;
        private string _row_format;
        private long _table_rows;
        private long _avg_row_length;
        private long _data_length;
        private long _max_data_length;
        private long _index_length;
        private long _data_free;
        private long _auto_increment;
        private DateTime? _create_time;
        private DateTime? _update_time;
        private DateTime? _check_time;
        private string _table_collation;
        private long _checksum;
        private string _create_options;
        private string _defaultCharSet;
        private string _table_comment;

        private string _viewName;

        //列集合
        public List<COLUMNS_Mdl> columns = new List<COLUMNS_Mdl>();
        //主键字段集合
        public List<string> keyColumns = new List<string>();
        //索引集合
        //public  List<INDEX_Mdl> indexs = new List<INDEX_Mdl>();
        /// <summary>
        /// 
        /// </summary>
        public string TABLE_CATALOG
        {
            set { _table_catalog = value; }
            get { return _table_catalog; }
        }

        public string VIEWNAME
        {
            get { return TABLE_NAME+"_VIEW"; }
        }

        public  COLUMNS_Mdl GetColumnByColID(int colid)
        {
            for (int i = 0; i < columns.Count; i++)
            {
                if (columns[i].COLID == colid)
                {
                    return columns[i];
                }

            }
            return null;
        }

        public string DEFAULTCHARSET
        {
            set { _defaultCharSet = value; }
            get { return _defaultCharSet; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string TABLE_SCHEMA
        {
            set { _table_schema = value; }
            get { return _table_schema; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string TABLE_NAME
        {
            set { _table_name = value; }
            get { return _table_name; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string TABLE_TYPE
        {
            set { _table_type = value; }
            get { return _table_type; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string ENGINE
        {
            set { _engine = value; }
            get { return _engine; }
        }
        /// <summary>
        /// 
        /// </summary>
        public long VERSION
        {
            set { _version = value; }
            get { return _version; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string ROW_FORMAT
        {
            set { _row_format = value; }
            get { return _row_format; }
        }
        /// <summary>
        /// 
        /// </summary>
        public long TABLE_ROWS
        {
            set { _table_rows = value; }
            get { return _table_rows; }
        }
        /// <summary>
        /// 
        /// </summary>
        public long AVG_ROW_LENGTH
        {
            set { _avg_row_length = value; }
            get { return _avg_row_length; }
        }
        /// <summary>
        /// 
        /// </summary>
        public long DATA_LENGTH
        {
            set { _data_length = value; }
            get { return _data_length; }
        }
        /// <summary>
        /// 
        /// </summary>
        public long MAX_DATA_LENGTH
        {
            set { _max_data_length = value; }
            get { return _max_data_length; }
        }
        /// <summary>
        /// 
        /// </summary>
        public long INDEX_LENGTH
        {
            set { _index_length = value; }
            get { return _index_length; }
        }
        /// <summary>
        /// 
        /// </summary>
        public long DATA_FREE
        {
            set { _data_free = value; }
            get { return _data_free; }
        }
        /// <summary>
        /// 
        /// </summary>
        public long AUTO_INCREMENT
        {
            set { _auto_increment = value; }
            get { return _auto_increment; }
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime? CREATE_TIME
        {
            set { _create_time = value; }
            get { return _create_time; }
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime? UPDATE_TIME
        {
            set { _update_time = value; }
            get { return _update_time; }
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime? CHECK_TIME
        {
            set { _check_time = value; }
            get { return _check_time; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string TABLE_COLLATION
        {
            set { _table_collation = value; }
            get { return _table_collation; }
        }
        /// <summary>
        /// 
        /// </summary>
        public long CHECKSUM
        {
            set { _checksum = value; }
            get { return _checksum; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string CREATE_OPTIONS
        {
            set { _create_options = value; }
            get { return _create_options; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string TABLE_COMMENT
        {
            set { _table_comment = value; }
            get { return _table_comment; }
        }

        /// <summary>
        /// 将表定义转换为SQL脚本
        /// </summary>
        /// <returns></returns>
        public string CovertToSQLScript()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(string.Format("CREATE TABLE {0}.{1} ( ", TABLE_SCHEMA, TABLE_NAME));

            foreach (COLUMNS_Mdl columnsMdl in columns)
            {
                sb.Append(columnsMdl.ConvertToScript());
            }
            if (keyColumns.Count != 0)
            {
                sb.Append(" PRIMARY KEY (");
                sb.AppendFormat(" {0}", keyColumns[0]);
                //添加主键脚本)))
                for (int i = 1 ; i < keyColumns.Count ; i++)
                {
                    sb.Append(",");
                    sb.AppendFormat(" {0} ", keyColumns[0]);
                }
                sb.Append(")");
            }
            else
            {
                int index = sb.ToString().LastIndexOf(",");
                sb.Remove(index, 1);
            }
            



            ////添加索引脚本
            //foreach (INDEX_Mdl indexMdl in indexs)
            //{
            //    sb.AppendLine(indexMdl.ConvertToScript());
            //}

            sb.AppendFormat(") ENGINE = {0}  DEFAULT CHARSET =  {1} COMMENT = '{2}';", ENGINE, DEFAULTCHARSET, TABLE_COMMENT);

            return sb.ToString();
        }

        /// <summary>
        /// 删除表
        /// </summary>
        public string DeleteScript()
        {
            return string.Format("drop table if exists {0}", TABLE_NAME);
        }

        public string DeleteViewScript()
        {
            return string.Format("drop view if exists {0}", VIEWNAME);
        }
        #endregion Model

    }
}
