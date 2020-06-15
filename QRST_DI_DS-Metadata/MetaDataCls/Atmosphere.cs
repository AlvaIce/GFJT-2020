using System.Runtime.Serialization;

namespace QRST_DI_DS_Metadata.MetaDataCls
{
    [DataContract]
    public class Atmosphere
    {
        public Atmosphere()
        { }

        public static string[] atmosphereattributenames = {
                                              "序号",
                                              "站点名称",
                                              "站点编号",
                                              "站点属性",
                                              "监测日期",
                                              "站点经度",
                                              "站点纬度",
                                              "公开类别",
                                              "数据收集单位",
                                              "联系人",
                                              "联系电话",
                                              "联系人邮箱",
                                              "监测时间",
                                              "最高气压",
                                              "最低气压",
                                              "平均气温",
                                              "最高气温",
                                              "最低气温",
                                              "平均相对湿度",
                                              "最小相对湿度",
                                              "平均风速",
                                              "最大风速",
                                              "最大风速时风向",
                                              "极大风速",
                                              "极大风速时风向",
                                              "日照时数",
                                              "降水量",
                                              "总辐射",
                                              "净辐射",
                                              "散射辐射",
                                              "直接辐射",
                                              "反射辐射",
                                              "海拔高度",
                                              "备注信息",
                                              "数据质量"};

        [DataMember]
        public AtmosphereInfo[] atmospheres { get; set; }
    }

    [DataContract]
    public class AtmosphereInfo
    {
        public AtmosphereInfo()
        { }

        [DataMember]
        public string fseq { get; set; }

        [DataMember]
        public string zdmc { get; set; }

        [DataMember]
        public string zdbh { get; set; }

        [DataMember]
        public string zdsx { get; set; }

        [DataMember]
        public string jcrq { get; set; }

        [DataMember]
        public string zdjd { get; set; }

        [DataMember]
        public string zdwd { get; set; }

        [DataMember]
        public string gklb { get; set; }

        [DataMember]
        public string sjsjdw { get; set; }

        [DataMember]
        public string lxr { get; set; }

        [DataMember]
        public string lxdh { get; set; }

        [DataMember]
        public string lxryx { get; set; }

        [DataMember]
        public string jcsj { get; set; }

        [DataMember]
        public string zgqy { get; set; }

        [DataMember]
        public string zdqy { get; set; }

        [DataMember]
        public string pjqw { get; set; }

        [DataMember]
        public string zgqw { get; set; }

        [DataMember]
        public string zdqw { get; set; }

        [DataMember]
        public string pjxdsd { get; set; }

        [DataMember]
        public string zxxdsd { get; set; }

        [DataMember]
        public string pjfs { get; set; }

        [DataMember]
        public string zdfs { get; set; }

        [DataMember]
        public string zdfssfx { get; set; }

        [DataMember]
        public string jdfs { get; set; }

        [DataMember]
        public string jdfssfx { get; set; }

        [DataMember]
        public string rzss { get; set; }

        [DataMember]
        public string jsl { get; set; }

        [DataMember]
        public string zfs { get; set; }

        [DataMember]
        public string jfs { get; set; }

        [DataMember]
        public string ssfs { get; set; }

        [DataMember]
        public string zjfs { get; set; }

        [DataMember]
        public string fsfs { get; set; }

        [DataMember]
        public string hbgd { get; set; }

        [DataMember]
        public string bzxx { get; set; }

        [DataMember]
        public string sjzl { get; set; }

    }
}
