namespace QRST_DI_DS_Metadata.MetaDataDefiner.Mdl
{
    public class INDEX_Mdl
    {
        private string _indexName;
        private string _columnName;
        private string _indexType;

        public string INDEXNAME
        {
            set { _indexName = value; }
            get { return _indexName; }
        }

        public string COLUMNNAME
        {
            set { _columnName = value; }
            get { return _columnName; }
        }

        public string INDEXTYPE
        {
            set { _indexType = value; }
            get { return _indexType; }
        }

        public string ConvertToScript()
        {
            string str = string.Format("{0} '{1}' ('{2}' ASC); ", INDEXTYPE, INDEXNAME, COLUMNNAME);
            return str;
        }
    }
}
