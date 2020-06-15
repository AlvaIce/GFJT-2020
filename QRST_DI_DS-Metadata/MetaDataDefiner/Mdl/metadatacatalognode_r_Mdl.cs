using System;

namespace QRST_DI_DS_Metadata.MetaDataDefiner.Mdl
{
    public class metadatacatalognode_r_Mdl
    {
        public metadatacatalognode_r_Mdl()
		{}
		#region Model
		private long _id;
		private string _group_code;
		private string _name;
		private string _child_code;
		private long _user_id;
		private DateTime? _datetime;
		private string _description;
		private string _qrst_code;
		/// <summary>
		/// 
		/// </summary>
		public long ID
		{
			set{ _id=value;}
			get{return _id;}
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
		public string NAME
		{
			set{ _name=value;}
			get{return _name;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string CHILD_CODE
		{
			set{ _child_code=value;}
			get{return _child_code;}
		}
		/// <summary>
		/// 
		/// </summary>
		public long USER_ID
		{
			set{ _user_id=value;}
			get{return _user_id;}
		}
		/// <summary>
		/// 
		/// </summary>
		public DateTime? DATETIME
		{
			set{ _datetime=value;}
			get{return _datetime;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string DESCRIPTION
		{
			set{ _description=value;}
			get{return _description;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string QRST_CODE
		{
			set{ _qrst_code=value;}
			get{return _qrst_code;}
		}
		#endregion Model

    }
}
