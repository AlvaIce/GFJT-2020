using System;
using System.Text;

namespace QRST_DI_DS_Metadata.MetaDataDefiner.Mdl
{
    /// <summary>
    /// 实体类COLUMNS_Mdl 。(属性说明自动提取数据库字段的描述信息)
    /// </summary>
    [Serializable]
    public class COLUMNS_Mdl
    {
        public COLUMNS_Mdl()
        { }
        #region Model

        private int _colId;
        private string _table_catalog;
        private string _table_schema;
        private string _table_name;
        private string _column_name;
        private long _ordinal_position;
        private string _column_default;
        private string _IS_NULLABLE;
        private string _data_type;
        private long _character_maximum_length;
        private long _character_octet_length;
        private long _numeric_precision;
        private long _numeric_scale;
        private string _character_set_name;
        private string _collation_name;
        private string _column_type;
        private string _column_key;
        private string _extra;
        private string _privileges;
        private string _column_comment;
        private bool _isKey;

        private string _view_name;     //对应视图中的字段名称

        /// <summary>
        /// 
        /// </summary>
        public int COLID
        {
            set { _colId = value; }
            get { return _colId; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string TABLE_CATALOG
        {
            set { _table_catalog = value; }
            get { return _table_catalog; }
        }

        public string VIEW_NAME
        {
            set { _view_name = value; }
            get { return _view_name; }
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
        public string COLUMN_NAME
        {
            set { _column_name = value; }
            get { return _column_name; }
        }
        /// <summary>
        /// 
        /// </summary>
        public long ORDINAL_POSITION
        {
            set { _ordinal_position = value; }
            get { return _ordinal_position; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string COLUMN_DEFAULT
        {
            set { _column_default = value; }
            get { return _column_default; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string IS_NULLABLE
        {
            set { _IS_NULLABLE = value; }
            get { return _IS_NULLABLE; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string DATA_TYPE
        {
            set { _data_type = value; }
            get { return _data_type; }
        }
        /// <summary>
        /// 
        /// </summary>
        public long CHARACTER_MAXIMUM_LENGTH
        {
            set { _character_maximum_length = value; }
            get { return _character_maximum_length; }
        }
        /// <summary>
        /// 
        /// </summary>
        public long CHARACTER_OCTET_LENGTH
        {
            set { _character_octet_length = value; }
            get { return _character_octet_length; }
        }
        /// <summary>
        /// 
        /// </summary>
        public long NUMERIC_PRECISION
        {
            set { _numeric_precision = value; }
            get { return _numeric_precision; }
        }
        /// <summary>
        /// 
        /// </summary>
        public long NUMERIC_SCALE
        {
            set { _numeric_scale = value; }
            get { return _numeric_scale; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string CHARACTER_SET_NAME
        {
            set { _character_set_name = value; }
            get { return _character_set_name; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string COLLATION_NAME
        {
            set { _collation_name = value; }
            get { return _collation_name; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string COLUMN_TYPE
        {
            set { _column_type = value; }
            get { return _column_type; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string COLUMN_KEY
        {
            set { _column_key = value; }
            get { return _column_key; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string EXTRA
        {
            set { _extra = value; }
            get { return _extra; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string PRIVILEGES
        {
            set { _privileges = value; }
            get { return _privileges; }
        }

        public bool ISKEY
        {
            set { _isKey = value; }
            get { return _isKey; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string COLUMN_COMMENT
        {
            set { _column_comment = value; }
            get { return _column_comment; }
        }

        public bool Equailvant(COLUMNS_Mdl model)
        {
            if (this.COLUMN_NAME != model.COLUMN_NAME)
            {
                return false;
            }
            if (this.COLUMN_TYPE != model.COLUMN_TYPE)
            {
                return false;
            }
            if (this.COLUMN_COMMENT != model.COLUMN_COMMENT)
            {
                return false;
            }
            if (this.IS_NULLABLE != model.IS_NULLABLE)
            {
                return false;
            }
            if (this.EXTRA != model.EXTRA)
            {
                return false;
            }
            if (this.COLUMN_DEFAULT != model.COLUMN_DEFAULT)
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// 将列转换为mysql执行脚本
        /// </summary>
        /// <returns></returns>
        public string ConvertToScript()
        {
            string nullString;
            if (IS_NULLABLE == "NO")
            {
                nullString = "NOT NULL";
            }
            else
            {
                nullString = " NULL";
            }

            StringBuilder sb = new StringBuilder();

            sb.Append(string.Format(" {0} {1} {2} {3} ", COLUMN_NAME, COLUMN_TYPE, nullString, EXTRA)); //添加列名,数据类型，是否为空，是否自动增长

            if (COLUMN_DEFAULT != "") //添加列默认值
            {
                sb.Append("DEFAULT " + COLUMN_DEFAULT + " ");
            }
            if (COLUMN_COMMENT != "") //添加列评论
            {
                sb.Append(string.Format("COMMENT   '{0}'", COLUMN_COMMENT));
            }
            sb.Append(",");
            string str;
            return sb.ToString();
        }

        public string ConvertToAlterScript()
        {
            string nullString;
            if (IS_NULLABLE == "NO")
            {
                nullString = "NOT NULL";
            }
            else
            {
                nullString = " NULL";
            }

            StringBuilder sb = new StringBuilder();

            sb.Append(string.Format(" {0} {1} {2} {3} ", COLUMN_NAME, COLUMN_TYPE, nullString, EXTRA)); //添加列名,数据类型，是否为空，是否自动增长

            if (COLUMN_DEFAULT != "") //添加列默认值
            {
                sb.Append("DEFAULT " + COLUMN_DEFAULT + " ");
            }
            if (COLUMN_COMMENT != "") //添加列评论
            {
                sb.Append(string.Format("COMMENT  '{0}'", COLUMN_COMMENT));
            }
            //   sb.Append(",");
            return sb.ToString()+";";
        }

     
        #endregion Model

    }
}
