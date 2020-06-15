using System;
using QRST_DI_DS_Metadata.MetaDataCls;
using System.IO;
using QRST_DI_DS_Metadata.Paths;
using System.Globalization;
 
namespace QRST_DI_DS_Metadata
{
    public class MetaDataReader
    {
        #region Read MetaData

        //OERS.Interface.WaitingForm.IWaitingForm wform;
        /// <summary>
        /// 读取元数据信息
        /// </summary>
        /// <param name="dataType">数据类型</param>
        /// <param name="dataFile">数据路径</param>
        /// <returns>元数据信息类实例</returns>
        public MetaData ReadMetaData(EnumMetadataTypes dataType, string dataFile)
        {
            MetaData md = null;

            switch (dataType)
            {
                case EnumMetadataTypes.Unknown:
                    break;
                case EnumMetadataTypes.MODIS:
                    md = ReadMetaDataModis(dataFile) as MetaData;
                    break;
                case EnumMetadataTypes.CBERS:
                    md = ReadMetaDataCbers(dataFile) as MetaData;
                    break;
                case EnumMetadataTypes.HJ:
                    md = ReadMetaDataHj(dataFile) as MetaData;
                    break;
                case EnumMetadataTypes.NOAA:
                    md = ReadMetaDataNOAA(dataFile) as MetaData;
                    break;
                case EnumMetadataTypes.TM:
                    md = ReadMetaDataTM(dataFile) as MetaData;
                    break;
                case EnumMetadataTypes.DEM:
                    md = ReadMetaDataDEM(dataFile) as MetaData;
                    break;
                case EnumMetadataTypes.TJQBird:
                    md = ReadMetaDataDOM(dataFile) as MetaData;
                    break;
                case EnumMetadataTypes.ZJCOASTALOS:
                    md = ReadMetaDataDOM(dataFile) as MetaData;
                    break;
                case EnumMetadataTypes.SZWORLDVIEW2:
                    md = ReadMetaDataDOM(dataFile) as MetaData;
                    break;
                case EnumMetadataTypes.HHALOS:
                    md = ReadMetaDataDOM(dataFile) as MetaData;
                    break;
                default:
                    break;
            }

            return md;
        }

        /// <summary>
        /// 读取Vector数据的元数据信息
        /// </summary>
        /// <param name="dataFile">Vector元数据路径</param>
        /// <returns>元数据信息类</returns>
        public MetaDataVector ReadMetaDataVector(string excelFilePath,int index,string groupcode)
        {
            MetaDataVector mdVector = new MetaDataVector();

            //读取元数据，并赋值
            string[] otherParameters = {
                                           //GROPUCODE
                                           groupcode,
                                           //SDE
                                           " "
                                       };
            try
            {
                mdVector.readVectorAttribute(excelFilePath, otherParameters, out mdVector.vectorAttributeNames,index);
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
            return mdVector;
        }

        /// <summary>
        /// 读取全国范围TM数据的元数据信息
        /// </summary>
        /// <param name="dataFile">全国范围TM元数据路径</param>
        /// <returns>元数据信息类</returns>
        public MetaDataRaster ReadMetaDataTM(string excelFilePath)
        {
            MetaDataRaster mdTM = new MetaDataRaster();
            try
            {
                mdTM.readRasterAttribute(excelFilePath);
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
            return mdTM;
        }

        /// <summary>
        /// 读取全国范围30米DEM数据的元数据信息
        /// </summary>
        /// <param name="dataFile">全国范围30米DEM元数据路径</param>
        /// <returns>元数据信息类</returns>
        public MetaDataRaster ReadMetaDataDEM(string excelFilePath)
        {
            MetaDataRaster mdDEM = new MetaDataRaster();
            try
            {
                mdDEM.readRasterAttribute(excelFilePath);
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
            return mdDEM;
        }

        /// <summary>
        /// 读取DOM数据的元数据信息
        /// </summary>
        /// <param name="dataFile">DOM元数据路径</param>
        /// <returns>元数据信息类</returns>
        public MetaDataRaster ReadMetaDataDOM(string excelFilePath)
        {
            MetaDataRaster mdDOM = new MetaDataRaster();
            try
            {
                mdDOM.readRasterAttribute(excelFilePath);
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
            return mdDOM;
        }

        /// <summary>
        /// 读取NOAA数据的元数据信息
        /// </summary>
        /// <param name="dataFile">NOAA数据路径</param>
        /// <returns>元数据信息类</returns>
        public MetaDataNOAA ReadMetaDataNOAA(string dataFile)
        {
            MetaDataNOAA mdNOAA = new MetaDataNOAA();

            //读取元数据，并赋值
            string[] otherParameters = {
                                           //原文件路径
                                           dataFile.Substring(0,dataFile.Length-3),
                                           //缩略图路径
                                           " "
                                       };
            try
            {
                mdNOAA.readNoaaAttribute(dataFile, otherParameters, out mdNOAA.noaaAttributeNames);
                mdNOAA.OverviewFilePath = String.Format("{0}实验验证数据库\\NOAA\\{1}\\{2}\\{3}\\{4}\\{5}\\{6}\\{6}.JPG", StoragePath.StoreBasePath, mdNOAA.Satellite, mdNOAA.Sensor, string.Format("{0:0000}", mdNOAA.StartDate.Year), string.Format("{0:00}", mdNOAA.StartDate.Month), string.Format("{0:00}", mdNOAA.StartDate.Day), Path.GetFileNameWithoutExtension(dataFile));
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
            for(int i=0;i<mdNOAA.noaaAttributeValues.Length-2;i++)
            {
                string str = mdNOAA.noaaAttributeValues[i];
                if (str == null || str.Equals(""))
                {
                    throw new Exception(String.Format("元数据信息不全，缺少{0}信息", mdNOAA.noaaAttributeNames[i]));
                }
            }
            return mdNOAA;
        }

        
        /// <summary>
        /// 读取Modis数据的元数据信息
        /// </summary>
        /// <param name="dataFile">Modis数据路径</param>
        /// <returns>元数据信息类</returns>
        public MetaDataModis ReadMetaDataModis(string dataFile)
        {
            MetaDataModis mdModis = new MetaDataModis();

            string[] paths = dataFile.Split("\\".ToCharArray());
            string datapath = paths[paths.Length - 1].ToString();
            if (datapath.StartsWith("MOBRGB"))//缩略图文件 MOBRGB.A2009255.0320.005.2010341202801.jpg
            {
                mdModis.BeginDate = Convert.ToDateTime(TimeConvert.datetimeConvert(datapath.Substring(8, 4), datapath.Substring(12, 3)), CultureInfo.CurrentCulture);
                return mdModis;
            }


            //文件类型检查
            if (System.IO.Path.GetExtension(dataFile)!=".hdf")
            {
                throw new Exception("MetaDataUtilities:Modis数据类型输入错误。");
            }
            string ancillaryFile ="";
            string overViewFile ="";
            //读取元数据，并赋值
            string[] otherParameters = {
                                           //原文件路径
                                           dataFile.Substring(0,dataFile.Length-4),
                                           //辅助数据路径
                                           ancillaryFile,
                                           //缩略图路径
                                           overViewFile,
                                       };
            try
            {
                mdModis.readModisAttribute(dataFile, otherParameters, out mdModis.modisAttributeValues);
            }
            catch (System.Exception ex)
            {
                throw ex;
            }

            if (mdModis.Satellite == null || mdModis.Satellite.Equals(""))
            {
                throw new Exception("缺少卫星信息！！");
            }
            else if (mdModis.Sensor == null || mdModis.Sensor.Equals(""))
            {
                throw new Exception("缺少传感器信息！！");
            }
            else if (mdModis.BeginDate == null || mdModis.BeginDate.Equals("") || mdModis.EndDate == null || mdModis.EndDate.Equals(""))
            {
                throw new Exception("缺少成像时间信息！！");
            }
            for (int i = 9; i < 17; i++)
            {
                string str = mdModis.modisAttributeValues[i];
                if (str == null || str.Equals(""))
                {
                    throw new Exception(String.Format("元数据信息不全，缺少{0}信息", mdModis.modisAttributeNames[i]));
                }
            }

            return mdModis;
        }

        /// <summary>
        /// 读取CBERS数据的元数据信息
        /// </summary>
        /// <param name="dataFile">CBERS数据路径</param>
        /// <returns>元数据信息类</returns>
        public MetaDataCbers ReadMetaDataCbers(string dataFile)
        {
            MetaDataCbers mdCbers = new MetaDataCbers();
            
            //解压元数据信息文件(.xml or .odl)
            MetaDataCompression mdComp=new MetaDataCompression();
            string metaDataInfoFile = mdComp.DecompressCBERS(dataFile);

            //检查元数据信息文件
            bool isXML = true;      //元数据信息文件类型标识，true为xml文件；false为odl文件
            if (System.IO.Path.GetExtension(metaDataInfoFile).ToLower()==".xml")
            {
                isXML = true;
            }
            else if (System.IO.Path.GetExtension(metaDataInfoFile).ToLower() == ".odl")
            {
                isXML = false;
            }
            else
            {
                throw new Exception("MetaDataUtilities:CBERS元数据信息文件输入错误。");
            }
            string tempdataPath = dataFile;
            tempdataPath = tempdataPath.Substring(0, tempdataPath.LastIndexOf("."));
            //读取元数据，并赋值
            if (isXML)
            {
                string[] otherParameters = {
                                           //原文件路径
                                           dataFile.Substring(0,tempdataPath.Length-4),
                                           //缩略图路径
                                           " "
                                       };
                try
                {
                    mdCbers.readCbersHjAttribute(metaDataInfoFile, otherParameters, out mdCbers.cbersAttributeValues);
                }
                catch (System.Exception ex)
                {
                    throw ex;
                }
            }
            else
            {
                //待扩展
            }
            for (int i = 0; i <= 21; i++)
            {
                string str = mdCbers.cbersAttributeNames[i];
                if (str == null || str.Equals("无"))
                {
                    throw new Exception(String.Format("元数据信息不全，缺少{0}信息", mdCbers.cbersAttributeNames[i]));
                }
            }

            return mdCbers;
        }
        
        /// <summary>
        /// 读取HJ数据的元数据信息
        /// </summary>
        /// <param name="dataFile">HJ数据路径</param>
        /// <returns>元数据信息类</returns>
        public MetaDataHj ReadMetaDataHj(string dataFile)
        {
            try
            {
                return ReadMetaDataHj(dataFile, "-1");
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
            
        }

        /// <summary>
        /// 读取HJ数据的元数据信息
        /// </summary>
        /// <param name="dataFile">HJ数据路径</param>
        /// <returns>元数据信息类</returns>
        public MetaDataHj ReadMetaDataHj(string dataFile,string cordatapath)
        {
            MetaDataHj mdHj = new MetaDataHj();
            string metaDataInfoFile="";
            //检查是元数据信息文件还是压缩包
            if (System.IO.Path.GetExtension(dataFile) == ".xml" || System.IO.Path.GetExtension(dataFile) == ".XML")
            {
                metaDataInfoFile = dataFile;
            }
            else
            {
                //解压元数据信息文件(.xml)
                MetaDataCompression mdComp = new MetaDataCompression();
                metaDataInfoFile = mdComp.DecompressHJ(dataFile);
            }
            //检查元数据信息文件

            if (System.IO.Path.GetExtension(metaDataInfoFile) != ".xml" && System.IO.Path.GetExtension(metaDataInfoFile) != ".XML")
            {
                throw new Exception("MetaDataUtilities:HJ元数据信息文件输入错误。");
            }
            string tempdataPath = metaDataInfoFile;
            tempdataPath=tempdataPath.Substring(0,tempdataPath.LastIndexOf("."));
            

            //读取元数据，并赋值
            string[] otherParameters = {
                                           //原文件路径
                                           
                                           //dataFile.Substring(0,tempdataPath.LastIndexOf(".")),
                                           System.IO.Path.GetFileName(dataFile),
                                           //缩略图路径
                                           " ",
                                           //纠正后数据路径 无则为-1
                                           cordatapath
                                       };
            try
            {
                mdHj.readCbersHjAttribute(metaDataInfoFile, otherParameters, out mdHj.hjAttributeValues);
            }
            catch (System.Exception ex)
            {
                throw ex;
            }

            for (int i = 0; i <= 25;i++ )
            {
                string str = mdHj.hjAttributeValues[i];
                //if (str == null || str.Equals("无"))
                //{
                //    throw new Exception(String.Format("元数据信息不全，缺少{0}信息", mdHj.hjAttributeNames[i]));
                //}
            }
            
            return mdHj;
        }
                
        #endregion

    }
}
