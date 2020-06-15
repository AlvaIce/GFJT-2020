using System.Runtime.Serialization;

namespace QRST_DI_DS_MetadataQuery.JSONutilty
{
    [DataContract]
    public class namenodeinfo
    {
        public namenodeinfo()
        { }
        #region Model
       
    
        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public string memPerc { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember(Name = "osVersion")]
        public string osVersion { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public string osArch { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public string host { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public string TxBytes { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public string memTotal { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public string RxBytes { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public string blocks { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public string used { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public string adminState { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public string osVendorName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public string nondfsused { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public string lastcontact { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public string ethAddress { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public string remaining { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public string percentRemaining { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public string capacity { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public string memUsed { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public string perc { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public string TxPackets { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public string RxPackets { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public string percentUsed { get; set; }

        //private bool unVisible;

        //public void  SetInVisible(bool visible)
        //{
        //    unVisible = visible;
        //}

        //public bool InVisible()
        //{
        //    return unVisible;
        //}

        #endregion Model

    }
}
