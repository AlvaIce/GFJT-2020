using System.Runtime.Serialization;

namespace QRST_DI_DS_Metadata.MetaDataCls
{
    [DataContract]
    public class Vegetation
    {
        public Vegetation()
        { }

        public static string[] vegetationattributenames = {
                                              "序号",
                                              "植被名称",
                                              "植被类别",
                                              "测量部位",
                                              "光谱数据",
                                              "野外或实验室",
                                              "测量经度",
                                              "测量纬度",
                                              "测量日期",
                                              "物候期",
                                              "公开类别",
                                              "数据来源",
                                              "数据收集单位",
                                              "联系人",
                                              "联系电话",
                                              "联系人邮箱",
                                              "测量时间",
                                              "测点高度",
                                              "光谱仪器",
                                              "叶色",
                                              "叶绿素含量",
                                              "叶面积指数",
                                              "平均根径",
                                              "平均胸径",
                                              "冠径",
                                              "土壤类别",
                                              "植被高度",
                                              "太阳天顶角或方位角",
                                              "观测天顶角或方位角",
                                              "目标照片",
                                              "测点海拔高度",
                                              "原始数据",
                                              "标准版数据",
                                              "地理特征",
                                              "备注信息",
                                              "数据质量",
                                              "覆盖率",
                                              "生长时间",
                                              "生长状况",
                                              "叶绿素测量方法",
                                              "叶面积指数测量方法",
                                              "云量",
                                              "云状",
                                              "气温",
                                              "相对湿度",
                                              "气压",
                                              "能见度",
                                              "风速",
                                              "风向",
                                              "标定日期"};

        [DataMember]
        public VegetationInfo[] vegetations { get; set; }
    }

    [DataContract]
    public class VegetationInfo
    {
        public VegetationInfo()
        { }

        [DataMember]
        public string fseq { get; set; }

        [DataMember]
        public string zbmc { get; set; }

        [DataMember]
        public string zblb { get; set; }

        [DataMember]
        public string clbw { get; set; }

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
        public string whq { get; set; }

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
        public string clsj { get; set; }

        [DataMember]
        public string gpyq { get; set; }

        [DataMember]
        public string ys { get; set; }

        [DataMember]
        public string ylshl { get; set; }

        [DataMember]
        public string ymjzs { get; set; }

        [DataMember]
        public string pjgj { get; set; }

        [DataMember]
        public string pjxj { get; set; }

        [DataMember]
        public string gj { get; set; }

        [DataMember]
        public string trlb { get; set; }

        [DataMember]
        public string zbgd { get; set; }

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
        public string fgl { get; set; }

        [DataMember]
        public string szsj { get; set; }

        [DataMember]
        public string szzk { get; set; }

        [DataMember]
        public string ylsclff { get; set; }

        [DataMember]
        public string ymjzsclff { get; set; }

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
        public string njd { get; set; }

        [DataMember]
        public string fs { get; set; }

        [DataMember]
        public string fx { get; set; }

        [DataMember]
        public string bdrq { get; set; }

    }
}
