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
    /// 质量检测功能模块，主要包括五个方面的质检，1,原始文件名规范检测；2,原始文件大小检测；3,元数据规范检测；
    /// 4,缩略图检测；5，数据格式检测
    /// 
    /// </summary>
    public class ITDataQualityInspection : TaskClass
    {
        //文件名检测的正则表达式
        public static readonly String namingExpressions = "GF1|GF2|GF4_WFV[1-4]{0,1}|PMS[1-2]{0,1}|B[1-9]{0,1}|IRS|PMI_E|W[0-9]{0,3}.[0-9]_S|N[0-9]{0,2}.[0-9]_[0-9]{8}_L[0-9]A[0-9]{10}";
        //元数据中必须包含的字段名称
        public static readonly String[] metaDataFileds = new String[] { "SatelliteID", "SensorID", "ReceiveTime",     
            "OrbitID", "ProduceType", "SceneID", "ProductID", "ProductLevel", "ProductQuality", "ProductQualityReport",
            "ProductFormat", "ProduceTime", "Bands", "ScenePath", "SceneRow", "SatPath", "SatRow", "SceneCount", 
            "SceneShift", "StartTime", "EndTime", "CenterTime", "ImageGSD", "WidthInPixels",
            "HeightInPixels","WidthInMeters", "HeightInMeters", "CloudPercent", "QualityInfo", "PixelBits", 
            "ValidPixelBits",  "RollViewingAngle", "PitchViewingAngle", "RollSatelliteAngle", "PitchSatelliteAngle",                  "YawSatelliteAngle", "SolarAzimuth", "SolarZenith", "SatelliteAzimuth", "SatelliteZenith", "GainMode",                    "IntegrationTime", "IntegrationLevel", "MapProjection", "EarthEllipsoid", "ZoneNo", "ResamplingKernel",                   "HeightMode", "MtfCorrection", "RelativeCorrectionData","TopLeftLatitude", "TopLeftLongitude",                            "TopRightLatitude", "TopRightLongitude", "BottomRightLatitude", 
            "BottomRightLongitude", "BottomLeftLatitude", "BottomLeftLongitude", "TopLeftMapX", "TopLeftMapY", 
            "TopRightMapX", "TopRightMapY", "BottomRightMapX", "BottomRightMapY", "BottomLeftMapX", "BottomLeftMapY"};

        /// <summary>
        /// 任务名,定义唯一标识，不可动态修改
        /// </summary>
        public override string TaskName
        {
            get { return "ITDataQualityInspection"; }
            set { }
        }

        public override void Process()
        {
            String filePath = this.ProcessArgu[0];
            if (!Directory.Exists(filePath))
            {
                this.ParentOrder.Logs.Add(string.Format("文件路径不存在"));
                return;
            }
            try
            {
                String originalFileName = Directory.GetFiles(filePath, "*.tar.gz")[0];  //得到路径下面的原始文件
                String unZipGFFilepath = StorageBasePath.SharePath_OrignalData(filePath); //得到解压后的文件夹
                if (!Directory.Exists(unZipGFFilepath))
                {
                    this.ParentOrder.Status = Orders.EnumOrderStatusType.Error;
                    this.ParentOrder.Logs.Add(string.Format("不存在解压后存放原始数据的文件夹"));
                    return;
                }
                if (!nameCheck(originalFileName))
                {
                    this.ParentOrder.Status = Orders.EnumOrderStatusType.Error;
                    this.ParentOrder.Logs.Add(string.Format("文件名检测不通过！"));
                    return;
                }
                if (!dataSizeCheck(originalFileName))
                {
                    this.ParentOrder.Status = Orders.EnumOrderStatusType.Error;
                    this.ParentOrder.Logs.Add(string.Format("文件大小检测不通过！"));
                    return;
                }
                if (!metaDataCheck(unZipGFFilepath))
                {
                    this.ParentOrder.Status = Orders.EnumOrderStatusType.Error;
                    this.ParentOrder.Logs.Add(string.Format("元数据检测不通过！"));
                    return;
                }
                if (!thumbnailCheck(unZipGFFilepath))
                {
                    this.ParentOrder.Status = Orders.EnumOrderStatusType.Error;
                    this.ParentOrder.Logs.Add(string.Format("拇指图检测不通过！"));
                    return;
                }
                if (!formatCheck(unZipGFFilepath))
                {
                    this.ParentOrder.Status = Orders.EnumOrderStatusType.Error;
                    this.ParentOrder.Logs.Add(string.Format("数据格式检测不通过！"));
                    return;
                }
                this.ParentOrder.Logs.Add(string.Format("质量检测部分完成！"));
            }
            catch (DirectoryNotFoundException e)
            {
                this.ParentOrder.Logs.Add(string.Format("不存在原始文件" + e.Message));
                return;
            }


        }

        /// <summary>
        /// 名字检测
        /// </summary>
        /// <returns></returns>
        private bool nameCheck(string filename)
        {
            return Regex.IsMatch(filename, namingExpressions);
        }

        /// <summary>
        /// 原始数据大小的质量检测，检测大小的范围为（30M-4096M）
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        private bool dataSizeCheck(string filename)
        {
            FileInfo fi = new FileInfo(filename);
            //if (fi.Length / (1024 * 1024) < 4096 && fi.Length / (1024 * 1024) > 30)
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
        /// 源数据检测
        /// 元数据规范检测，统一元数据格式以及元数据字段命名方式，空间信息，时间信息不能为空，元数据以xml的方式提供
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
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
                        result=true;
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
        public static bool xmlDataCheck(String xmlFileName,OrderLog orderlog)
        {
            XmlDocument doc = new XmlDocument();
            try
            {
                doc.Load(xmlFileName);
            }
            catch (System.Exception ex)
            {
                if (orderlog!=null)
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

        private bool thumbnailCheck(string filename)
        {
            String[] xmlFileNames = Directory.GetFiles(filename, "*_thumb.jpg");
            if (xmlFileNames.Length > 0)
            {
                return true;
            }
            return false;
        }

        private bool formatCheck(string filename)
        {
            //log = log + "\r\n开始数据格式检测";
            return true;

        }

        public void Getreport()
        {
            /*
            StringBuilder sb = new StringBuilder();
           // sb.AppendLine(string.Format("共检测文件'{0}'个：", dataPath.Count));
            int successcount = 0;
            for (int i = 0; i < dataPath.Count; i++)
            {
                sb.AppendLine(string.Format("文件名'{0}'：", Path.GetFileName(dataPath[i])));

                if (checkresult[i][0])
                {
                    sb.AppendLine("符合第一条规范，文件命名规范检核通过");
                }
                else
                {
                    sb.AppendLine("不符合第一条规范，文件命名规范检核未通过，请查看日志文件获取详情");
                    continue;
                }

                if (checkresult[i][1])
                {
                    sb.AppendLine("符合第二条规范，元数据规范检核通过");
                }
                else
                {
                    sb.AppendLine("不符合第二条规范，元数据规范检核未通过，请查看日志文件获取详情");
                    continue;
                }
                if (checkresult[i][2])
                {
                    sb.AppendLine("符合第三条规范，数据大小规范检核通过");
                }
                else
                {
                    sb.AppendLine("不符合第三条规范，数据大小规范检核未通过，请查看日志文件获取详情");
                    continue;
                }
                if (checkresult[i][3])
                {
                    sb.AppendLine("符合第四条规范，缩略图规范检核通过");
                }
                else
                {
                    sb.AppendLine("不符合第四条规范，缩略图规范检核未通过，请查看日志文件获取详情");
                    continue;
                }
                if (checkresult[i][4])
                {
                    sb.AppendLine("符合第四条规范，数据格式规范检核通过");
                }
                else
                {
                    sb.AppendLine("不符合第四条规范，数据格式规范检核未通过，请查看日志文件获取详情");
                    continue;
                }

                successcount++;

            }
            sb.AppendLine(string.Format("审核通过的文件数：{0}，未通过数：{1}", successcount, dataPath.Count - successcount));
            report = sb.ToString();
             * */
        }



    }
}
