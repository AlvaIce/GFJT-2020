using System.Runtime.Serialization;

namespace QRST_DI_DS_Metadata.MetaDataCls
{
    [DataContract]
    public class CityObj
    {
        public CityObj()
        { }

        public static string[] cityobjattributenames = {
                                              "序号",
                                              "人工目标名称",
                                              "人工目标类别",
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
                                              "材质情况",
                                              "测量部位",
                                              "颜色",
                                              "材料成分和含量",
                                              "结构草图",
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
                                              "表面粗糙度",
                                              "表面温度",
                                              "云量",
                                              "云状",
                                              "气温",
                                              "相对湿度",
                                              "气压",
                                              "能见度",
                                              "标定日期"};

        [DataMember]
        public CityObjInfo[] city_objs { get; set; }
    }

    [DataContract]
    public class CityObjInfo
    {
        public CityObjInfo()
        { }

        [DataMember]
        public string fseq { get; set; }

        [DataMember]
        public string csmbmc { get; set; }

        [DataMember]
        public string csmblb { get; set; }

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
        public string czqk { get; set; }

        [DataMember]
        public string clbw { get; set; }

        [DataMember]
        public string yans { get; set; }

        [DataMember]
        public string clcfhhl { get; set; }

        [DataMember]
        public string jgct { get; set; }

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
        public string bmccd { get; set; }

        [DataMember]
        public string bmwd { get; set; }

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
        public string bdrq { get; set; }

    }
}
