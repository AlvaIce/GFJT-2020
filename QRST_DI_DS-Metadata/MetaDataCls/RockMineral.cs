using System.Runtime.Serialization;

namespace QRST_DI_DS_Metadata.MetaDataCls
{
    [DataContract]
    public class RockMineral
    {
        public RockMineral()
        { }

        public static string[] rockMineralattributenames = {
                                              "序号",
                                              "岩矿名称",
                                              "岩矿类别",
                                              "岩矿子类",
                                              "光谱数据",
                                              "所属类别",
                                              "野外或实验室",
                                              "测量经度",
                                              "测量纬度",
                                              "测量日期",
                                              "公开类别",
                                              "数据来源",
                                              "数据收集单位",
                                              "联系人",
                                              "联系电话",
                                              "联系人邮箱",
                                              "测点高度",
                                              "测量时间",
                                              "光谱仪器",
                                              "岩石产状",
                                              "结构构造",
                                              "自色",
                                              "成分及含量",
                                              "太阳天顶角或方位角",
                                              "观测天顶角或方位角",
                                              "目标照片",
                                              "测点海拔高度",
                                              "原始数据",
                                              "标准版数据",
                                              "地理特征",
                                              "备注信息",
                                              "数据质量",
                                              "岩矿露头面积",
                                              "表面覆盖状况",
                                              "地面覆盖物",
                                              "岩矿构造",
                                              "地质年代",
                                              "硬度",
                                              "分析方法",
                                              "风化状况",
                                              "矿物组成",
                                              "云量",
                                              "天气状况",
                                              "标定日期",
                                              "光泽",
                                              "解理类型",
                                              "透明度",
                                              "云状"};

        [DataMember]
        public RockMineralInfo[] rocks { get; set; }
    }

    [DataContract]
    public class RockMineralInfo
    {
        public RockMineralInfo()
        { }

        [DataMember]
        public string fseq { get; set; }

        [DataMember]
        public string ykmc { get; set; }

        [DataMember]
        public string yklb { get; set; }

        [DataMember]
        public string ykzl { get; set; }

        [DataMember]
        public string gpsj { get; set; }

        [DataMember]
        public string sslb { get; set; }

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
        public string clsj { get; set; }

        [DataMember]
        public string gpyq { get; set; }

        [DataMember]
        public string yscz { get; set; }

        [DataMember]
        public string jggz { get; set; }

        [DataMember]
        public string zis { get; set; }

        [DataMember]
        public string cfjhl { get; set; }

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
        public string ykltmj { get; set; }

        [DataMember]
        public string bmfgzk { get; set; }

        [DataMember]
        public string bmfgw { get; set; }

        [DataMember]
        public string ykgz { get; set; }

        [DataMember]
        public string dznd { get; set; }

        [DataMember]
        public string yingd { get; set; }

        [DataMember]
        public string yxff { get; set; }

        [DataMember]
        public string fhzk { get; set; }

        [DataMember]
        public string kwzc { get; set; }

        [DataMember]
        public string yl { get; set; }

        [DataMember]
        public string tqwk { get; set; }

        [DataMember]
        public string bdrq { get; set; }

        [DataMember]
        public string gz { get; set; }

        [DataMember]
        public string jllx { get; set; }

        [DataMember]
        public string tmd { get; set; }

        [DataMember]
        public string yz { get; set; }
    }
}
