namespace QRST_DI_DS_Metadata.MetaDataDefiner.Mdl
{
    public class tablecode_Mdl
    {
        public tablecode_Mdl()
		{}
		#region Model
		private long _id;
		private string _qrst_code;
		private string _table_name;
		private string _description;
		/// <summary>
		/// auto_increment
		/// </summary>
		public long ID
		{
			set{ _id=value;}
			get{return _id;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string QRST_CODE
		{
			set{ _qrst_code=value;}
			get{return _qrst_code;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string TABLE_NAME
		{
			set{ _table_name=value;}
			get{return _table_name;}
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

    }
}
