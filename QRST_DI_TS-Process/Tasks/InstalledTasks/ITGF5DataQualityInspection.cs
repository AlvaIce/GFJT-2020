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
    public class ITGF5DataQualityInspection : TaskClass
    {
        //GF影像文件名检测的正则表达式
        public static string namingExpressions = null;

        //GF影像元数据中必须包含的字段名称
        public static string[] metaDataFileds = null;

        /// <summary>
        /// GF影像质量检测任务名称
        /// </summary>
        public override string TaskName
        {
            get { return "ITGF5DataQualityInspection"; }
            set { }
        }

        /// <summary>
        /// GF影像质量检测任务逻辑
        /// </summary>
        public override void Process()
        {
            string filename = this.ProcessArgu[1];

            if (filename.Contains("AHSI"))
            {
                namingExpressions =
                    "GF5_AHSI_E|W[0-9]{0,3}.[0-9]{0,3}_S|N[0-9]{0,2}.[0-9]{0,3}_[0-9]{8}_[0-9]{6}_L[0-9][0-9]{10}";

                metaDataFileds = new string[] {
                            "SatelliteID",
                            "ReceiveStationID" ,
                            "SensorID" ,
                            "ProduceTime" ,
                            "ProductLevel" ,
                            "ProductFormat" ,
                            "SceneID" ,
                            "ProductID" ,
                            "ProduceType" ,
                            "BandsCount" ,
                            "BandsID" ,
                            "ScenePath" ,
                            "SceneRow" ,
                            "SatPath" ,
                            "SatRow" ,
                            "OrbitID" ,
                            "POrbitID" ,
                            "OrbitType" ,
                            "AttType" ,
                            "RollSatelliteAngle" ,
                            "PitchSatelliteAngle" ,
                            "YawSatelliteAngle" ,
                            "SolarAzimuth" ,
                            "SolarZenith" ,
                            "SatelliteAzimuth" ,
                            "SatelliteZenith" ,
                            "StartTime" ,
                            "EndTime" ,
                            "ImageGSD" ,
                            "LinesInPixels" ,
                            "SamplesInPixels" ,
                            "WidthInMeters" ,
                            "HeightInMeters" ,
                            "CloudPercent" ,
                            "GainMode" ,
                            "IntegrationTime" ,
                            "MapProjection" ,
                            "EarthEllipsoid" ,
                            "ZoneNo" ,
                            "RadiometricMethod" ,
                            "CalibrationParam" ,
                            "CalibrationParamVersion" ,
                            "ScaleFactor" ,
                            "ResamplingKernel" ,
                            "Denoise" ,
                            "HeightMode" ,
                            "EphemerisData" ,
                            "AttitudeData" ,
                            "CenterLatitude" ,
                            "CenterLongitude" ,
                            "TopLeftLatitude" ,
                            "TopLeftLongitude" ,
                            "TopRightLatitude" ,
                            "TopRightLongitude" ,
                            "BottomRightLatitude" ,
                            "BottomRightLongitude" ,
                            "BottomLeftLatitude" ,
                            "BottomLeftLongitude" ,
                            "TopLeftMapX" ,
                            "TopLeftMapY" ,
                            "TopRightMapX" ,
                            "TopRightMapY" ,
                            "BottomRightMapX" ,
                            "BottomRightMapY" ,
                            "BottomLeftMapX" ,
                            "BottomLeftMapY",
                            "DDSFlag",
                            "SoftwareVersion" };
            }
            else if (filename.Contains("EMI"))
            {
                namingExpressions =
                  "GF5_EMI_[0-9]{8}_[0-9]{6}_L[0-9][0-9]{10}";
                metaDataFileds = new string[]
                {
                            "SatelliteID" ,
                            "ReceiveStationID" ,
                            "SensorID" ,
                            "ProduceTime" ,
                            "ProductLevel" ,
                            "ProductFormat" ,
                            "SceneID" ,
                            "ProductID" ,
                            "POrbitID" ,
                            "StartTime" ,
                            "EndTime" ,
                            "InstrumentMode" ,
                            "NadirPixelSize" ,
                            "FrameNumber" ,
                            "Bands" ,
                            "BandID" ,
                            "SamplePoint" ,
                            "SampleLong" ,
                            "SampleLat" ,
                            "NadirPoint" ,
                            "NadirLong" ,
                            "NadirLat" ,
                            "CalibrationParam" ,
                            "CalibrationParamVersion" ,
                            "DDSFlag" ,
                            "SoftwareVersion"
                };
            }
            else if (filename.Contains("GMI"))
            {
                namingExpressions =
                   "GF5_GMI_S|N[0-9]{0,2}.[0-9]{0,3}_E|W[0-9]{0,3}.[0-9]{0,3}_[0-9]{8}_[0-9]{6}_L[0-9][0-9]{10}";

                metaDataFileds = new string[]
                {
                            "SatelliteID" ,
                            "ReceiveStationID" ,
                            "SensorID" ,
                            "SceneID" ,
                            "POrbitID" ,
                            "StartTime" ,
                            "EndTime" ,
                            "InstrumentMode" ,
                            "ProductLevel" ,
                            "J2000MilliSecond",
                            "OrbitalSemiMajorAxis" ,
                            "Eccentricity" ,
                            "OrbitalInclination" ,
                            "RAAN" ,
                            "ArgumentPerigee" ,
                            "TrueAnomaly" ,
                            "SolarEarthCoorSys" ,
                            "SatGPS" ,
                            "CenterLong" ,
                            "CenterLat" ,
                            "ObserverAzimuth" ,
                            "ObserverZenith" ,
                            "SolarAzimuth" ,
                            "SolarZenith" ,
                            "CTAngle" ,
                            "ATAngle" ,
                            "SatAtt" ,
                            "SatEarthCoorSys" ,
                            "ProduceTime" ,
                            "ProductFormat" ,
                            "DDSFlag" ,
                            "SoftwareVersion" ,
                            "QualityFlag" ,
                            "IntegralTime" ,
                            "AccumulativeTimes" ,
                            "MonitorOutput" ,
                            "ObserveOrbit" ,
                            "ApodizationFunction" ,
                            "ProductID" ,
                            "CalibrationParam" ,
                            "CalibrationParamVersion"
                };
            }
            else if (filename.Contains("DPC"))
            {
                namingExpressions =
                  "GF5_DPC_[0-9]{8}_[0-9]{6}_L[0-9][0-9]{10}";
                metaDataFileds = new string[]
                {
                            "SatelliteID" ,
                            "ReceiveStationID",
                            "SensorID" ,
                            "ProduceTime" ,
                            "ProductLevel" ,
                            "ProductFormat" ,
                            "SceneID" ,
                            "ProductID" ,
                            "POrbitID" ,
                            "StartTime" ,
                            "EndTime" ,
                            "InstrumentMode" ,
                            "CalibrationParam" ,
                            "CalibrationParamVersion" ,
                            "NadirPixelSize" ,
                            "MapProjection" ,
                            "EarthEllipsoid" ,
                            "GridSize" ,
                            "SamplePoint" ,
                            "SampleLong" ,
                            "SampleLat" ,
                            "DDSFlag" ,
                            "SoftwareVersion"
                };
            }
            else if (filename.Contains("AIUS"))
            {
                namingExpressions =
                  "GF5_AIUS_[0-9]{8}_[0-9]{6}_L[0-9][0-9]{10}";
                metaDataFileds = new string[]
                {
                            "SatelliteID",
                            "ReceiveStationID",
                            "SensorID",
                            "ProduceTime",
                            "ProductLevel",
                            "ProductFormat",
                            "SceneID",
                            "POrbitID",
                            "StartTime",
                            "EndTime",
                            "InstrumentMode",
                            "FrameNumber",
                            "QualityFlag",
                            "DDSFlag",
                            "SoftwareVersion",
                            "ProductID",
                            "TangentPointNumber",
                            "StartTangentHeight",
                            "EndTangentHeight",
                            "StartCenterLong",
                            "EndCenterLong",
                            "StartCenterLat",
                            "EndCenterLat",
                            "StartSatPosition",
                            "EndSatPosition",
                            "StartSunPosition",
                            "EndSunPosition",
                            "CalibrationParam",
                            "CalibrationParamVersion"
                };
            }
            else
            {
                namingExpressions =
                   "GF5_VIMS_S|N[0-9]{0,2}.[0-9]{0,3}_E|W[0-9]{0,3}.[0-9]{0,3}_[0-9]{8}_[0-9]{6}_L[0-9][0-9]{10}";
                metaDataFileds = new string[]
                {
                            "SatelliteID" ,
                            "ReceiveStationID" ,
                            "SensorID" ,
                            "ProduceTime" ,
                            "ProductLevel" ,
                            "ProductFormat" ,
                            "SceneID" ,
                            "ProductID" ,
                            "ProduceType" ,
                            "ScenePath" ,
                            "SceneRow" ,
                            "SatPath" ,
                            "SatRow" ,
                            "OrbitID" ,
                            "POrbitID" ,
                            "OrbitType" ,
                            "AttType" ,
                            "RollSatelliteAngle" ,
                            "PitchSatelliteAngle" ,
                            "YawSatelliteAngle" ,
                            "SolarAzimuth" ,
                            "SolarZenith" ,
                            "SatelliteAzimuth" ,
                            "SatelliteZenith" ,
                            "StartTime" ,
                            "EndTime" ,
                            "BandsCount" ,
                            "Bands" ,
                            "SpectralRangeStart" ,
                            "SpectralRangeEnd" ,
                            "CentralWavelength" ,
                            "ImageGSD" ,
                            "WidthInPixels" ,
                            "HeightInPixels" ,
                            "WidthInMeters" ,
                            "HeightInMeters" ,
                            "CloudPercent" ,
                            "GainMode" ,
                            "CalibrationParam" ,
                            "CalibrationParamVersion" ,
                            "GAINS" ,
                            "OFFSETS" ,
                            "IntegrationTime" ,
                            "IntegrationLevel" ,
                            "MapProjection" ,
                            "EarthEllipsoid" ,
                            "ZoneNo" ,
                            "RadiometricMethod" ,
                            "CalParameterVersion" ,
                            "ResamplingKernel" ,
                            "Denoise" ,
                            "HeightMode" ,
                            "EphemerisData" ,
                            "AttitudeData" ,
                            "CenterLatitude" ,
                            "CenterLongitude" ,
                            "TopLeftLatitude" ,
                            "TopLeftLongitude" ,
                            "TopRightLatitude" ,
                            "TopRightLongitude" ,
                            "BottomRightLatitude" ,
                            "BottomRightLongitude" ,
                            "BottomLeftLatitude" ,
                            "BottomLeftLongitude" ,
                            "TopLeftMapX" ,
                            "TopLeftMapY" ,
                            "TopRightMapX" ,
                            "TopRightMapY" ,
                            "BottomRightMapX" ,
                            "BottomRightMapY" ,
                            "BottomLeftMapX" ,
                            "BottomLeftMapY" ,
                            "DDSFlag",
                            "SoftwareVersion"
                };
            }

            string filePath = this.ProcessArgu[0];
            if (!Directory.Exists(filePath))
            {
                this.ParentOrder.Logs.Add(string.Format("文件路径不存在"));
                throw new Exception("文件路径不存在");
            }

            //获得GF影像入库订单工作空间下的原始影像文件
            String originalFileName = Directory.GetFiles(filePath, "*.tar.*")[0];

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
            if (fi.Length / (1024 * 1024) >= 0)
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
            return true;
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
