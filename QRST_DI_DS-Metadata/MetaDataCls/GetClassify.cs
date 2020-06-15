using System.Runtime.Serialization;

namespace QRST_DI_DS_Metadata.MetaDataCls
{
    [DataContract]
    public class GetClassify
    {
        public GetClassify()
        {
        }

        [DataMember]
        public gettype[] types { get; set; }

    }

    [DataContract]
    public class gettype
    {
        public gettype()
        {
        }

        [DataMember]
        public string type { get; set; }
    }

}
