namespace QRST_DI_DS_Metadata.MetaDataDefiner.Mdl
{
    public  class metadatacatalognode_Mdl
    {
        public metadatacatalognode_Mdl()
		{}
		#region Model
        private decimal? _id;
        private decimal? _order_index;
		private string _group_code;
		private string _name;
		private string _data_code;
		private string _group_type;
		private string _description;
//判断是否是数据集
        private bool _isDataSet;   
		/// <summary>
		/// 
		/// </summary>
		public decimal? ID
		{
			set{ _id=value;}
			get{return _id;}
		}
        /// <summary>
        /// 
        /// </summary>
        public decimal? ORDER_INDEX
        {
            set { _order_index = value; }
            get { return _order_index; }
        }
		/// <summary>
		/// 
		/// </summary>
		public string GROUP_CODE
		{
			set{ _group_code=value;}
			get{return _group_code;}
		}

        /// <summary>
        /// 
        /// </summary>
        public bool IS_DATASET
        {
            set { _isDataSet = value; }
            get 
            {
                if (GROUP_TYPE == EnumDataKind.System_DataSet.ToString()||GROUP_TYPE.Contains("root"))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
		/// <summary>
		/// 
		/// </summary>
		public string NAME
		{
			set{ _name=value;}
			get{return _name;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string DATA_CODE
		{
			set{ _data_code=value;}
			get{return _data_code;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string GROUP_TYPE
		{
			set{ _group_type=value;}
			get{return _group_type;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string DESCRIPTION
		{
			set{ _description=value;}
			get{return _description;}
		}
		#endregion Model

        public override string ToString()
        {
            return this.NAME + ":" + this.DATA_CODE;
        }

    }
}
