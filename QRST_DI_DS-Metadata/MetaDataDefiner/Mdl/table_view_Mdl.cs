namespace QRST_DI_DS_Metadata.MetaDataDefiner.Mdl
{
    public class table_view_Mdl
    {
        public table_view_Mdl()
		{}
		#region Model
		private int _id;
		private string _table_name;
		private string _view_name;
		private string _field_name;
		private string _view_field_name;
		private string _memo;
		/// <summary>
		/// 
		/// </summary>
		public int ID
		{
			set{ _id=value;}
			get{return _id;}
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
		public string VIEW_NAME
		{
            get { return _table_name+"_"+"VIEW"; }
		}
		/// <summary>
		/// 
		/// </summary>
		public string FIELD_NAME
		{
			set{ _field_name=value;}
			get{return _field_name;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string VIEW_FIELD_NAME
		{
			set{ _view_field_name=value;}
			get{return _view_field_name;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string MEMO
		{
			set{ _memo=value;}
			get{return _memo;}
		}
		#endregion Model

    }
}
