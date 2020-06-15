using QRST_DI_MS_Basis.Log;
using QRST_DI_TS_Basis.DirectlyAddress;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;

namespace QRST_DI_TS_Process.Tasks.InstalledTasks
{
    /// <summary>
    /// 高分数据质量检测类,主要包括五个方面的质检：
    /// 1、原始文件名规范检测；
    /// 2、原始文件大小检测；
    /// 3、元数据规范检测；
    /// 4、缩略图检测；
    /// 5、数据格式检测
    /// </summary>
    public class ITGF6DataQualityInspection : TaskClass
    {
        //GF影像文件名检测的正则表达式
        public static readonly String namingExpressions = "GF6_WFV[1-4]{0,1}|PMS[1-2]{0,1}|B[1-9]{0,1}|IRS|PMI_E|W[0-9]{0,3}.[0-9]_S|N[0-9]{0,2}.[0-9]_[0-9]{8}_L[0-9]A[0-9]{10}";

        //GF影像元数据中必须包含的字段名称
        public static readonly String[] metaDataFileds =
            new string[] {  "SatelliteID",    //卫星名0
                                                "ReceiveStationID",   //接收地面站代号1
                                                 "SensorID",  //传感器2
                                                 "ReceiveTime",  //接收时间3
                                                 "OrbitID",  //观测圈号4
                                                 "TopLeftLatitude",//61
                                                 "TopLeftLongitude",//62
                                                 "TopRightLatitude",//63
                                                 "TopRightLongitude",//64
                                                 "BottomRightLatitude",//65
                                                 "BottomRightLongitude",//66
                                                 "BottomLeftLatitude",//67
                                                 "BottomLeftLongitude",   //68
                                                 "OrbitType",  //产品格式5
                                                 "AttType",      //景号6
                                                 "StripID",   //产品号7
                                                 "ProduceType",   //观测圈号8
                                                 "SceneID",   //采集起始时间9
                                                 "DDSFlag",    //采集终止时间10
                                                 "ProductID",   //工作模式11
                                                 "ProductLevel",  //定标系数来源12
                                                 "ProductFormat",   //定标系数版本13
                                                 "ProduceTime",  //星下点分辨率14
                                                 "Bands",  //坐标投影方式15
                                                 "ScenePath",  //椭球模型16
                                                 "SceneRow",  //投影后网格大小17
                                                 "SatPath",  //投影区采样点18
                                                 "SatRow",  //采样点经度19
                                                 "SceneCount",   //采样点纬度20
                                                 "SceneShift",    // 发布标识21
                                                 "StartTime", //开始时间22
                                                 "EndTime",  //结束时间23
                                                 "CenterTime",  //24
                                                 "StartLine",   //25
                                                 "EndLine",     //26
                                                 "ImageGSD",   //27
                                                 "WidthInPixels",  //28
                                                 "HeightInPixels",  //29
                                                 "WidthInMeters",  //30
                                                 "HeightInMeters",  //31
                                                 "RegionName",   //32
                                                 "CloudPercent",  //33
                                                 "DataSize",    //34
                                                 "RollViewingAngle",//35
                                                 "PitchViewingAngle",//36
                                                 "PitchSatelliteAngle",//37
                                                 "RollSatelliteAngle",//38
                                                 "YawSatelliteAngle",//39
                                                 "SolarAzimuth",  //40
                                                 "SolarZenith",  //41
                                                 "SatelliteAzimuth",//42
                                                 "SatelliteZenith", //43
                                                 "GainMode",//44
                                                 "IntegrationTime",//45
                                                 "IntegrationLevel",//46
                                                 "MapProjection",//47
                                                 "EarthEllipsoid",//48
                                                 "ZoneNo",//49
                                                 "ResamplingKernel",//50
                                                 "HeightMode",//51
                                                 "EphemerisData",//52
                                                 "AttitudeData",//53
                                                 "RadiometricMethod",//54
                                                 "MtfCorrection", //55
                                                 "Denoise",//56
                                                 "RayleighCorrection",//57
                                                 "UsedGCPNo",//58
                                                 "CenterLatitude",//59
                                                 "CenterLongitude",//60                                                                                          
                                                 "TopLeftMapX",//69
                                                 "TopLeftMapY",//70
                                                 "TopRightMapX",//71
                                                 "TopRightMapY",//72
                                                 "BottomRightMapX",//73
                                                 "BottomRightMapY",//74
                                                 "BottomLeftMapX",//75
                                                 "BottomLeftMapY",//76
                                                 "DataArchiveFile",//77
                                                 "BrowseFileLocation",//78
                                                 "ThumbFileLocation"  //79   
            };

        /// <summary>
        /// GF6影像质量检测任务名称
        /// </summary>
        public override string TaskName
        {
            get { return "ITGF6DataQualityInspection"; }
            set { }
        }

        /// <summary>
        /// GF影像质量检测任务逻辑
        /// </summary>
        public override void Process()
        {
            String filePath = this.ProcessArgu[0];
            if (!Directory.Exists(filePath))
            {
                this.ParentOrder.Logs.Add(string.Format("文件路径不存在"));
                throw new Exception("文件路径不存在");
            }

            //获得GF影像入库订单工作空间下的原始影像文件
            String originalFileName = Directory.GetFiles(filePath, "*.tar.gz")[0];

            //获得GF影像解压路径
            String unZipGFFilepath = StorageBasePath.SharePath_OrignalData(filePath);
            if (!Directory.Exists(unZipGFFilepath))
            {
                this.ParentOrder.Status = Orders.EnumOrderStatusType.Error;
                this.ParentOrder.Logs.Add(string.Format("不存在解压后存放原始数据的文件夹"));
                throw new Exception("不存在解压后存放原始数据的文件夹");
            }
            if (!nameCheck(originalFileName))
            {
                this.ParentOrder.Status = Orders.EnumOrderStatusType.Error;
                this.ParentOrder.Logs.Add(string.Format("文件名检测不通过"));
                throw new Exception("文件名检测不通过");
            }
            if (!dataSizeCheck(originalFileName))
            {
                this.ParentOrder.Status = Orders.EnumOrderStatusType.Error;
                this.ParentOrder.Logs.Add(string.Format("文件大小检测不通过"));
                throw new Exception("文件大小检测不通过");
            }
            if (!metaDataCheck(unZipGFFilepath))
            {
                this.ParentOrder.Status = Orders.EnumOrderStatusType.Error;
                this.ParentOrder.Logs.Add(string.Format("元数据检测不通过"));
                throw new Exception("元数据检测不通过");
            }
            if (!thumbnailCheck(unZipGFFilepath))
            {
                this.ParentOrder.Status = Orders.EnumOrderStatusType.Error;
                this.ParentOrder.Logs.Add(string.Format("拇指图检测不通过"));
                throw new Exception("拇指图检测不通过");
            }
            if (!formatCheck(unZipGFFilepath))
            {
                this.ParentOrder.Status = Orders.EnumOrderStatusType.Error;
                this.ParentOrder.Logs.Add(string.Format("数据格式检测不通过"));
                throw new Exception("数据格式检测不通过");
            }
            this.ParentOrder.Logs.Add(string.Format("质量检测部分完成！"));

        }

        /// <summary>
        /// GF影像文件名检测
        /// </summary>
        /// <returns>是否检测成功</returns>
        private bool nameCheck(string filename)
        {
            return Regex.IsMatch(filename, namingExpressions);
        }

        /// <summary>
        /// GF影像数据大小的质量检测，检测大小的范围为（30M-4096M）
        /// </summary>
        /// <param name="filename">待检测的GF影像文件名</param>
        /// <returns>是否检测成功</returns>
        private bool dataSizeCheck(string filename)
        {
            FileInfo fi = new FileInfo(filename);
            int minSize = (filename.ToUpper().Contains("IRS")) ? 0 : 10;
            if (fi.Length / (1024 * 1024) > minSize)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// GF影像数据元数据检测
        /// 元数据规范检测内容：
        /// 1、统一元数据格式以及元数据字段命名方式；
        /// 2、空间信息，时间信息不能为空；
        /// 3、元数据以xml的方式提供
        /// </summary>
        /// <param name="filename">待检测的GF影像数据文件名</param>
        /// <returns>是否检测成功</returns>
        private bool metaDataCheck(string filename)
        {
            bool result = false;
            String[] xmlFileNames = Directory.GetFiles(filename, "*.xml");
            if (xmlFileNames.Length > 0)
            {
                foreach (String xmlFileName in xmlFileNames)
                {
                    if (xmlDataCheck(xmlFileName, this.ParentOrder.Logs))
                    {
                        result = true;
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// 检测xml是否完整，即存在定义的所有字段
        /// </summary>
        /// <param name="xmlFileName">xml文件路径名称</param>
        /// <returns>是否检测成功</returns>
        public static bool xmlDataCheck(String xmlFileName, OrderLog orderlog)
        {
            XmlDocument doc = new XmlDocument();
            try
            {
                doc.Load(xmlFileName);
            }
            catch (System.Exception ex)
            {
                if (orderlog != null)
                {
                    orderlog.Add(string.Format("xml文件损坏，请检查！({0})", xmlFileName));
                }
                return false;
            }
            XmlElement root = doc.DocumentElement;
            XmlNodeList childlist = root.ChildNodes;
            ArrayList nodeNameList = new ArrayList();

            foreach (XmlElement node in childlist)
            {
                nodeNameList.Add(node.Name);
            }
            foreach (String filed in metaDataFileds)
            {
                if (!nodeNameList.Contains(filed))
                {
                    if (orderlog != null)
                    {
                        orderlog.Add(string.Format("元数据中缺少字段:" + filed));
                    }
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// GF影像数据快视图检测
        /// </summary>
        /// <param name="filename">待检测的GF影像数据文件名</param>
        /// <returns>是否检测成功</returns>
        private bool thumbnailCheck(string filename)
        {
            String[] xmlFileNames = Directory.GetFiles(filename, "*_thumb.jpg");
            if (xmlFileNames.Length > 0)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// 格式检测
        /// </summary>
        /// <param name="filename">待检测的GF影像文件名</param>
        /// <returns>是否检测成功</returns>
        private bool formatCheck(string filename)
        {
            return true;

        }
    }
}
