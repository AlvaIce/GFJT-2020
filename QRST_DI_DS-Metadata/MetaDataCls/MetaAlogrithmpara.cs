using System.Xml;

namespace QRST_DI_DS_Metadata.MetaDataCls
{
    public  class MetaAlogrithmpara
    {
        public MetaAlogrithmpara()
		{
            AlogrithmparaValues = new string[AlogrithmparaAttributes.Length];
        }

        public string[] AlogrithmparaAttributes = new string[]
        {
            "ParaName","ParaChsName","Description","ParaType","ParaValue","ParaSource"
        };


        public string[] AlogrithmparaValues;

		#region Model

		/// <summary>
		/// 
		/// </summary>
        public int ID
        {
            set;
            get;
        }
		/// <summary>
		/// 
		/// </summary>
		public string ParaName
		{
            set { AlogrithmparaValues[0]= value; }
            get { return AlogrithmparaValues[0]; }
		}
		/// <summary>
		/// 
		/// </summary>
		public string ParaChsName
		{
            set { AlogrithmparaValues[1] = value; }
            get { return AlogrithmparaValues[1]; }
		}
		/// <summary>
		/// 
		/// </summary>
		public string Description
		{
            set { AlogrithmparaValues[2] = value; }
            get {return AlogrithmparaValues[2]; }
		}
		/// <summary>
		/// 
		/// </summary>
		public string ParaType
		{
            set { AlogrithmparaValues[3] = value; }
            get { return AlogrithmparaValues[3]; }
		}
		/// <summary>
		/// 
		/// </summary>
		public string ParaValue
		{
            set { AlogrithmparaValues[4] = value; }
            get { return AlogrithmparaValues[4]; }
		}
		/// <summary>
		/// 
		/// </summary>
		public string ParaSource
		{
            set { AlogrithmparaValues[5] = value; }
            get { return AlogrithmparaValues[5]; }
		}
		/// <summary>
		/// 
		/// </summary>
        public string ParaID
        {
            set;
            get;
        }
		#endregion Model
        public void ReadAttribute(XmlNode node)
        {
           if(node != null)
           {
               for (int i = 0; i < node.ChildNodes.Count;i++ )
               {
                   if (i< AlogrithmparaValues.Length )
                   AlogrithmparaValues[i] = node.ChildNodes[i].InnerText;
               }
           }
        }
    
    }
}
