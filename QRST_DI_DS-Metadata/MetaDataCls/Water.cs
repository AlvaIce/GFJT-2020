using System.Runtime.Serialization;

namespace QRST_DI_DS_Metadata.MetaDataCls
{
    [DataContract]
    public class Water
    {
        public Water()
        { }

        public static string[] waterattributenames = {"序号",
                                              "水域名称",
                                              "所属类别",
                                              "光谱数据",
                                              "野外或实验室",
                                              "测量经度",
                                              "测量纬度",
                                              "测量日期",
                                              "测点深度",
                                              "公开类别",
                                              "收集来源",
                                              "数据收集单位",
                                              "联系人",
                                              "联系电话",
                                              "联系人邮箱",
                                              "测点高度",
                                              "测量时间",
                                              "光谱仪器",
                                              "叶绿素a浓度",
                                              "有色可溶性有机物浓度",
                                              "悬浮物浓度",
                                              "总氮含量",
                                              "总磷含量",
                                              "太阳天顶角或方位角",
                                              "观测天顶角或方位角",
                                              "目标照片",
                                              "测点海拔高度",
                                              "原始数据",
                                              "标准版数据",
                                              "地理特征",
                                              "备注信息",
                                              "数据质量",
                                              "水质状况",
                                              "水色",
                                              "水温",
                                              "水深",
                                              "透明度",
                                              "浊度",
                                              "油含量",
                                              "盐度",
                                              "PH值",
                                              "溶解氧",
                                              "高猛酸钾指数",
                                              "化学需氧量",
                                              "云量",
                                              "云状",
                                              "能见度",
                                              "气温",
                                              "相对湿度",
                                              "气压",
                                              "风速",
                                              "风向",
                                              "标定日期"};

        [DataMember]
        public WaterInfo[] waters { get; set; }
    }

    [DataContract]
    public class WaterInfo
    {
        public WaterInfo()
        { }

        [DataMember]
        public string fseq { get; set; }

        [DataMember]
        public string symc { get; set; }

        [DataMember]
        public string sslb { get; set; }

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
        public string cdsd { get; set; }

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
        public string ylsnd { get; set; }

        [DataMember]
        public string yskrxyjwnd { get; set; }

        [DataMember]
        public string xfwnd { get; set; }

        [DataMember]
        public string zhdl { get; set; }

        [DataMember]
        public string zhll { get; set; }

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
        public string szzk { get; set; }

        [DataMember]
        public string sse { get; set; }

        [DataMember]
        public string sw { get; set; }

        [DataMember]
        public string ssh { get; set; }

        [DataMember]
        public string tmdu { get; set; }

        [DataMember]
        public string zdu { get; set; }

        [DataMember]
        public string yhl { get; set; }

        [DataMember]
        public string ydu { get; set; }

        [DataMember]
        public string phz { get; set; }

        [DataMember]
        public string rjy { get; set; }

        [DataMember]
        public string gmsjzs { get; set; }

        [DataMember]
        public string hxxyl { get; set; }

        [DataMember]
        public string yl { get; set; }

        [DataMember]
        public string yz { get; set; }

        [DataMember]
        public string zjd { get; set; }

        [DataMember]
        public string qw { get; set; }

        [DataMember]
        public string xdsd { get; set; }

        [DataMember]
        public string qy { get; set; }

        [DataMember]
        public string fs { get; set; }

        [DataMember]
        public string fx { get; set; }

        [DataMember]
        public string bdrq { get; set; }

    }
}
