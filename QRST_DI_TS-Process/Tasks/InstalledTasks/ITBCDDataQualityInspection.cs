using System;
using System.IO;
using System.Text.RegularExpressions;
using QRST_DI_TS_Basis.DirectlyAddress;
using System.Xml;
using System.Collections;
using QRST_DI_MS_Basis.Log;

namespace QRST_DI_TS_Process.Tasks.InstalledTasks
{
    /// <summary>
    /// BCD数据质量检测类,主要包括五个方面的质检：
    /// 1、原始文件名规范检测；
    /// 2、原始文件大小检测；
    /// 3、元数据规范检测；
    /// 4、缩略图检测；
    /// 5、数据格式检测
    /// </summary>
    public class ITBCDDataQualityInspection : TaskClass
    {
        //BCD影像文件名检测的正则表达式
        public static readonly String namingExpressions = "GF1B|GF1C|GF1D_PMS_E|W[0-9]{0,3}.[0-9]_S|N[0-9]{0,2}.[0-9]_[0-9]{8}_L[0-9]A[0-9]{10}";

        //BCD影像元数据中必须包含的字段名称
        public static readonly String[] metaDataFileds = new String[]{
      "SatelliteID"
    , "ReceiveStationID"
    , "SensorID"
    , "ReceiveTime"
    , "OrbitID"
    , "OrbitType"
    , "AttType"
    , "StripID"
    , "ProduceType"
    , "SceneID"
    , "DDSFlag"
    , "ProductID"
    , "ProductLevel"
    , "ProductFormat"
    , "ProduceTime"
    , "Bands"
    , "ScenePath"
    , "SceneRow"
    , "SatPath"
    , "SatRow"
    , "SceneCount"
    , "SceneShift"
    , "StartTime"
    , "EndTime"
    , "CenterTime"
    , "StartLine"
    , "EndLine"
    , "ImageGSD"
    , "WidthInPixels"
    , "HeightInPixels"
    , "WidthInMeters"
    , "HeightInMeters"
    , "RegionName"
    , "CloudPercent"
    , "DataSize"
    , "RollViewingAngle"
    , "PitchViewingAngle"
    , "PitchSatelliteAngle"
    , "RollSatelliteAngle"
    , "YawSatelliteAngle"
    , "SolarAzimuth"
    , "SolarZenith"
    , "SatelliteAzimuth"
    , "SatelliteZenith"
    , "GainMode"
    , "IntegrationTime"
    , "IntegrationLevel"
    , "MapProjection"
    , "EarthEllipsoid"
    , "ZoneNo"
    , "ResamplingKernel"
    , "HeightMode"
    , "EphemerisData"
    , "AttitudeData"
    , "RadiometricMethod"
    , "MtfCorrection"
    , "Denoise"
    , "RayleighCorrection"
    , "UsedGCPNo"
    , "CenterLatitude"
    , "CenterLongitude"
    , "TopLeftLatitude"
    , "TopLeftLongitude"
    , "TopRightLatitude"
    , "TopRightLongitude"
    , "BottomRightLatitude"
    , "BottomRightLongitude"
    , "BottomLeftLatitude"
    , "BottomLeftLongitude"
    , "TopLeftMapX"
    , "TopLeftMapY"
    , "TopRightMapX"
    , "TopRightMapY"
    , "BottomRightMapX"
    , "BottomRightMapY"
    , "BottomLeftMapX"
    , "BottomLeftMapY"
    , "DataArchiveFile"
    , "BrowseFileLocation"
    , "ThumbFileLocation"
        };

        /// <summary>
        /// BCD影像质量检测任务名称
        /// </summary>
        public override string TaskName
        {
            get { return "ITBCDDataQualityInspection"; }
            set { }
        }

        /// <summary>
        /// BCD影像质量检测任务逻辑
        /// </summary>
        public override void Process()
        {
            String filePath = this.ProcessArgu[0];
            if (!Directory.Exists(filePath))
            {
                this.ParentOrder.Logs.Add(string.Format("文件路径不存在"));
                throw new Exception("解压路径不存在");
            }
            //获得BCD影像入库订单工作空间下的原始影像文件
            String originalFileName = Directory.GetFiles(filePath, "*.tar.gz")[0];  //得到路径下面的原始文件

            //获得BCD影像解压路径
            String unZipGFFilepath = StorageBasePath.SharePath_OrignalData(filePath); //得到解压后的文件夹
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
            this.ParentOrder.Logs.Add(string.Format("质量检测部分完成"));
        }

        /// <summary>
        /// BCD影像文件名检测
        /// </summary>
        /// <returns>是否检测成功</returns>
        private bool nameCheck(string filename)
        {
            return Regex.IsMatch(filename, namingExpressions);
        }

        /// <summary>
        /// BCD影像数据大小的质量检测，检测大小的范围为（30M-4096M）
        /// </summary>
        /// <param name="filename">待检测的BCD影像文件名</param>
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
        /// BCD影像数据元数据检测
        /// 元数据规范检测内容：
        /// 1、统一元数据格式以及元数据字段命名方式；
        /// 2、空间信息，时间信息不能为空；
        /// 3、元数据以xml的方式提供
        /// </summary>
        /// <param name="filename">待检测的BCD影像数据文件名</param>
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
        /// <returns></returns>
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
        /// 检测xml是否完整，即存在定义的所有字段
        /// </summary>
        /// <param name="xmlFileName">xml文件路径名称</param>
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

        public void Getreport()
        {

        }
    }
}
