using System.Runtime.Serialization;

namespace QRST_DI_DS_Metadata.MetaDataCls
{
    [DataContract]
    public class Soil
    {
        public Soil()
        { }

        public static string[] soilattributenames = {"序号",
                                              "土壤名称",
                                              "土壤别名",
                                              "土壤子类",
                                              "光谱数据",
                                              "野外或实验室",
                                              "测量经度",
                                              "测量纬度",
                                              "测量日期",
                                              "公开类别",
                                              "收集来源",
                                              "数据收集单位",
                                              "联系人",
                                              "联系电话",
                                              "联系人邮箱",
                                              "测点高度",
                                              "光谱仪器",
                                              "土地利用类型",
                                              "质地",
                                              "土壤含水率",
                                              "土壤盐分",
                                              "有机质含量",
                                              "表面粗糙度",
                                              "阳离子含量",
                                              "成分及含量",
                                              "测量时间",
                                              "太阳天顶角或方位角",
                                              "观测天顶角或方位角",
                                              "目标照片",
                                              "测点海拔高度",
                                              "原始数据",
                                              "标准版数据",
                                              "地理特征",
                                              "备注信息",
                                              "数据质量",
                                              "覆盖物名称",
                                              "覆盖比例",
                                              "母质",
                                              "土壤酸碱度",
                                              "土层深度",
                                              "云量",
                                              "云状",
                                              "气温",
                                              "相对湿度",
                                              "气压",
                                              "标定日期"};

        [DataMember]
        public SoilInfo[] soils { get; set; }

    }

    [DataContract]
    public class SoilInfo
    {
        public SoilInfo()
        { }

        [DataMember]
        public string fseq { get; set; }

        [DataMember]
        public string trmc { get; set; }

        [DataMember]
        public string trbm { get; set; }

        [DataMember]
        public string trzl { get; set; }

        [DataMember]
        public string gpsj { get; set; }

        [DataMember]
        public string cldd { get; set; }

        [DataMember]
        public string cljd { get; set; }

        [DataMember]
        public string clwd { get; set; }

        [DataMember]
        public string clrq { get; set; }

        [DataMember]
        public string gklb { get; set; }

        [DataMember]
        public string sjly { get; set; }

        [DataMember]
        public string sjsjdw { get; set; }

        [DataMember]
        public string lxr { get; set; }

        [DataMember]
        public string lxdh { get; set; }

        [DataMember]
        public string lxryx { get; set; }

        [DataMember]
        public string cdgd { get; set; }

        [DataMember]
        public string gpyq { get; set; }

        [DataMember]
        public string tdlylx { get; set; }

        [DataMember]
        public string zhid { get; set; }

        [DataMember]
        public string trhsl { get; set; }

        [DataMember]
        public string tryf { get; set; }

        [DataMember]
        public string yjzhl { get; set; }

        [DataMember]
        public string bmcdd { get; set; }

        [DataMember]
        public string ylzhl { get; set; }

        [DataMember]
        public string cfjhl { get; set; }

        [DataMember]
        public string clsj { get; set; }

        [DataMember]
        public string tytdj { get; set; }

        [DataMember]
        public string gctdj { get; set; }

        [DataMember]
        public string mbzp { get; set; }

        [DataMember]
        public string cdhbgd { get; set; }

        [DataMember]
        public string yssj { get; set; }

        [DataMember]
        public string bzbsj { get; set; }

        [DataMember]
        public string dltz { get; set; }

        [DataMember]
        public string bzxx { get; set; }

        [DataMember]
        public string sjzl { get; set; }

        [DataMember]
        public string fgwmc { get; set; }

        [DataMember]
        public string fgbl { get; set; }

        [DataMember]
        public string muz { get; set; }

        [DataMember]
        public string trsjd { get; set; }

        [DataMember]
        public string tcsd { get; set; }

        [DataMember]
        public string yl { get; set; }

        [DataMember]
        public string yz { get; set; }

        [DataMember]
        public string qw { get; set; }

        [DataMember]
        public string xdsd { get; set; }

        [DataMember]
        public string qy { get; set; }

        [DataMember]
        public string bdrq { get; set; }

    }
}
