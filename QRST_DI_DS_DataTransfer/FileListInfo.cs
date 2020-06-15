using System.Runtime.Serialization;
 
namespace QRST_DI_DS_DataTransfer
{
    [DataContract]
    public class FileListInfo
    {
        public FileListInfo()
        { }

        [DataMember]
        public string bucket { get; set; }

        [DataMember]
        public Fileinfo[] file_set { get; set; }

    }

    [DataContract]
    public class Fileinfo
    {
        public Fileinfo()
        { }

        [DataMember]
        public string file_name { get; set; }

        [DataMember]
        public string file_size { get; set; }
    }
}
