/*
 * 作者：孔帅可
 * 时间：20140625
 * 作用：用于描述HJ1C卫星数据元数据
 * 
*/

using System;
using System.Text;
using System.Xml;
using QRST_DI_DS_Metadata.MetaDataDefiner.Dal;
using System.IO;
using System.Data;
using QRST_DI_DS_Metadata.Paths;
using QRST_DI_Resources;
using QRST_DI_SS_Basis;
using QRST_DI_SS_Basis.MetaData;
using QRST_DI_SS_DBInterfaces.IDBEngine;

namespace QRST_DI_DS_Metadata.MetaDataCls
{
	public class MetaDataHJ1C : MetaData
	{
		public string[] hj1cAttributeNames = {
													 "productId",
	"sceneId",
	"satelliteId",
	"sensorId",
	"recStationId",
	"productDate",
	"productLevel",
	"pixelSpacing",
	"productType",
	"sceneCount",
	"sceneShift",
	"overallQuality",
	"dataType",
	"satPath",
	"satRow",
	"satPathbias",
	"satRowbias",
	"scenePath",
	"sceneRow",
	"scenePathbias",
	"sceneRowbias",
	"sceneDate",
	"sceneTime",
	"sarMode",
	"imagingStartTime",
	"imagingStopTime",
	"antennaOffNadir",
	"mgcCode",
	"anteAngle",
	"beamNumber",
	"incidentAngle",
	"bands",
	"sampleDelay",
	"coordinateSystem",
	"rangeRes",
	"azimuthRes",
	"nominalRangeRes",
	"nominalAzimuthRes",
	"nominalPSLR",
	"nominalASLR",
	"imagingAlgor",
	"fdcMethod",
	"fdrMethod",
	"doIqComp",
	"doAGCComp",
	"doADCComp",
	"doInnerCalibComp",
	"doProcessorGainComp",
	"azimuthLooks",
	"rangeLooks",
	"weightRange",
	"weightAzimuth",
	"doSpeckle",
	"rangePatternComp",
	"azimuthPatternComp",
	"antennaPatternSource",
	"fileSize",
	"earthModel",
	"mapProjection",
	"resampleTechnique",
	"productOrientation",
	"ephemerisData",
	"attitudeData",
	"sceneCenterLat",
	"sceneCenterLong",
	"dataUpperLeftLat",
	"dataUpperLeftLong",
	"dataUpperRightLat",
	"dataUpperRightLong",
	"dataLowerLeftLat",
	"dataLowerLeftLong",
	"dataLowerRightLat",
	"dataLowerRightLong",
	"productUpperLeftLat",
	"productUpperLeftLong",
	"productUpperRightLat",
	"productUpperRightLong",
	"productLowerLeftLat",
	"productLowerLeftLong",
	"productLowerRightLat",
	"productLowerRightLong",
	"dataUpperLeftX",
	"dataUpperLeftY",
	"dataUpperRightX",
	"dataUpperRightY",
	"dataLowerLeftX",
	"dataLowerLeftY",
	"dataLowerRightX",
	"dataLowerRightY",
	"productUpperLeftX",
	"productUpperLeftY",
	"productUpperRightX",
	"productUpperRightY",
	"productLowerLeftX",
	"productLowerLeftY",
	"productLowerRightX",
	"productLowerRightY",
	"geodeticMethod",
	"dataFormatDes",
	"delStatus",
	"dataTempDir",
												 };
		public string[] hj1cAttributeValues;

		public MetaDataHJ1C(string _name, string _qrst_code)
		{
			hj1cAttributeValues = new string[hj1cAttributeNames.Length];
			Name = _name;
			QRST_CODE = _qrst_code;
		}

		public MetaDataHJ1C()
		{
			hj1cAttributeValues = new string[hj1cAttributeNames.Length];
		}

		#region Model

		/// <summary>
		/// 
		/// </summary>
		public int ID
		{
			set;
			get;
		}
		/// <summary>
		/// 
		/// </summary>
		public string Name
		{
			set;
			get;
		}
		/// <summary>
		/// 
		/// </summary>
		public string productId
		{
			set
			{
				hj1cAttributeValues[0] = value;
			}
			get
			{
				return hj1cAttributeValues[0];
			}
		}
		/// <summary>
		/// 
		/// </summary>
		public string sceneId
		{
			set
			{
				hj1cAttributeValues[1] = value;
			}
			get
			{
				return hj1cAttributeValues[1];
			}
		}
		public string satelliteId
		{
			set
			{
				hj1cAttributeValues[2] = value;
			}
			get
			{
				return hj1cAttributeValues[2];
			}
		}
		public string sensorId
		{
			set
			{
				hj1cAttributeValues[3] = value;
			}
			get
			{
				return hj1cAttributeValues[3];
			}
		}
		public string recStationId
		{
			set
			{
				hj1cAttributeValues[4] = value;
			}
			get
			{
				return hj1cAttributeValues[4];
			}
		}
		public DateTime productDate
		{
			set
			{
				hj1cAttributeValues[5] = value.ToString();
			}
			get
			{
				DateTime dt;
				if (DateTime.TryParse(hj1cAttributeValues[5], out dt))
				{
                    return dt;
				}
                return dt;
			}
		}
		
		public string productLevel
		{
			set
			{
				hj1cAttributeValues[6] = value;
			}
			get
			{
				return hj1cAttributeValues[6];
			}
		}
		public string pixelSpacing
		{
			set
			{
				hj1cAttributeValues[7] = value;
			}
			get
			{
				return hj1cAttributeValues[7];
			}
		}
		public string productType
		{
			set
			{
				hj1cAttributeValues[8] = value;
			}
			get
			{
				return hj1cAttributeValues[8];
			}
		}
		public string sceneCount
		{
			set
			{
				hj1cAttributeValues[9] = value;
			}
			get
			{
				return hj1cAttributeValues[9];
			}
		}
		public string sceneShift
		{
			set
			{
				hj1cAttributeValues[10] = value;
			}
			get
			{
				return hj1cAttributeValues[10];
			}
		}
		public string overallQuality
		{
			set
			{
				hj1cAttributeValues[11] = value;
			}
			get
			{
				return hj1cAttributeValues[11];
			}
		}
		public string dataType
		{
			set
			{
				hj1cAttributeValues[12] = value;
			}
			get
			{
				return hj1cAttributeValues[12];
			}
		}
		public string satPath
		{
			set
			{
				hj1cAttributeValues[13] = value;
			}
			get
			{
				return hj1cAttributeValues[13];
			}
		}
		public string satRow
		{
			set
			{
				hj1cAttributeValues[14] = value;
			}
			get
			{
				return hj1cAttributeValues[14];
			}
		}
		public string satPathbias
		{
			set
			{
				hj1cAttributeValues[15] = value;
			}
			get
			{
				return hj1cAttributeValues[15];
			}
		}
		public string satRowbias
		{
			set
			{
				hj1cAttributeValues[16] = value;
			}
			get
			{
				return hj1cAttributeValues[16];
			}
		}
		public string scenePath
		{
			set
			{
				hj1cAttributeValues[17] = value;
			}
			get
			{
				return hj1cAttributeValues[17];
			}
		}
		public string sceneRow
		{
			set
			{
				hj1cAttributeValues[18] = value;
			}
			get
			{
				return hj1cAttributeValues[18];
			}
		}
		public string scenePathbias
		{
			set
			{
				hj1cAttributeValues[19] = value;
			}
			get
			{
				return hj1cAttributeValues[19];
			}
		}
		public string sceneRowbias
		{
			set
			{
				hj1cAttributeValues[20] = value;
			}
			get
			{
				return hj1cAttributeValues[20];
			}
		}
		public DateTime sceneDate
		{
			set
			{
				hj1cAttributeValues[21] = value.ToString();
			}
			get
			{
				DateTime dt;
                if (DateTime.TryParse(hj1cAttributeValues[21], out dt))
                {
                    return dt;
                }
                return dt;
			}
		}
		public string sceneTime
		{
			set
			{
				hj1cAttributeValues[22] = value;
			}
			get
			{
				return hj1cAttributeValues[22];
			}
		}
		public string sarMode
		{
			set
			{
				hj1cAttributeValues[23] = value;
			}
			get
			{
				return hj1cAttributeValues[23];
			}
		}
		public String imagingStartTime
		{
			set
			{
				hj1cAttributeValues[24] = value.ToString();
			}
			get
			{
				DateTime dt;
				if (DateTime.TryParse(hj1cAttributeValues[24], out dt))
				{
                    string tempStr = dt.ToString("yyyy-MM-dd HH:mm:ss");
                    //dt = DateTime.Parse(tempStr);
                    return tempStr;
				}
				else
				{
					return null;
				}
			}
		}
		public String imagingStopTime
		{
			set
			{
				hj1cAttributeValues[25] = value.ToString();
			}
			get
			{
				DateTime dt;
				if (DateTime.TryParse(hj1cAttributeValues[25], out dt))
				{
                    string tempStr = dt.ToString("yyyy-MM-dd HH:mm:ss");
                    //dt = DateTime.Parse(tempStr);
                    return tempStr;
				}
				else
				{
					return null;
				}
			}
		}
		public string antennaOffNadir
		{
			set
			{
				hj1cAttributeValues[26] = value;
			}
			get
			{
				return hj1cAttributeValues[26];
			}
		}
		public string mgcCode
		{
			set
			{
				hj1cAttributeValues[27] = value;
			}
			get
			{
				return hj1cAttributeValues[27];
			}
		}
		public string anteAngle
		{
			set
			{
				hj1cAttributeValues[28] = value;
			}
			get
			{
				return hj1cAttributeValues[28];
			}
		}
		public string beamNumber
		{
			set
			{
				hj1cAttributeValues[29] = value;
			}
			get
			{
				return hj1cAttributeValues[29];
			}
		}
		public string incidentAngle
		{
			set
			{
				hj1cAttributeValues[30] = value;
			}
			get
			{
				return hj1cAttributeValues[30];
			}
		}
		public string bands
		{
			set
			{
				hj1cAttributeValues[31] = value;
			}
			get
			{
				return hj1cAttributeValues[31];
			}
		}
		public string sampleDelay
		{
			set
			{
				hj1cAttributeValues[32] = value;
			}
			get
			{
				return hj1cAttributeValues[32];
			}
		}
		public string coordinateSystem
		{
			set
			{
				hj1cAttributeValues[33] = value;
			}
			get
			{
				return hj1cAttributeValues[33];
			}
		}
		public string rangeRes
		{
			set
			{
				hj1cAttributeValues[34] = value;
			}
			get
			{
				return hj1cAttributeValues[34];
			}
		}
		public string azimuthRes
		{
			set
			{
				hj1cAttributeValues[35] = value;
			}
			get
			{
				return hj1cAttributeValues[35];
			}
		}
		public string nominalRangeRes
		{
			set
			{
				hj1cAttributeValues[36] = value;
			}
			get
			{
				return hj1cAttributeValues[36];
			}
		}
		public string nominalAzimuthRes
		{
			set
			{
				hj1cAttributeValues[37] = value;
			}
			get
			{
				return hj1cAttributeValues[37];
			}
		}
		public string nominalPSLR
		{
			set
			{
				hj1cAttributeValues[38] = value;
			}
			get
			{
				return hj1cAttributeValues[38];
			}
		}
		public string nominalASLR
		{
			set
			{
				hj1cAttributeValues[39] = value;
			}
			get
			{
				return hj1cAttributeValues[39];
			}
		}
		public string imagingAlgor
		{
			set
			{
				hj1cAttributeValues[40] = value;
			}
			get
			{
				return hj1cAttributeValues[40];
			}
		}
		public string fdcMethod
		{
			set
			{
				hj1cAttributeValues[41] = value;
			}
			get
			{
				return hj1cAttributeValues[41];
			}
		}
		public string fdrMethod
		{
			set
			{
				hj1cAttributeValues[42] = value;
			}
			get
			{
				return hj1cAttributeValues[42];
			}
		}
		public string doIqComp
		{
			set
			{
				hj1cAttributeValues[43] = value;
			}
			get
			{
				return hj1cAttributeValues[43];
			}
		}
		public string doAGCComp
		{
			set
			{
				hj1cAttributeValues[44] = value;
			}
			get
			{
				return hj1cAttributeValues[44];
			}
		}
		public string doADCComp
		{
			set
			{
				hj1cAttributeValues[45] = value;
			}
			get
			{
				return hj1cAttributeValues[45];
			}
		}
		public string doInnerCalibComp
		{
			set
			{
				hj1cAttributeValues[46] = value;
			}
			get
			{
				return hj1cAttributeValues[46];
			}
		}
		public string doProcessorGainComp
		{
			set
			{
				hj1cAttributeValues[47] = value;
			}
			get
			{
				return hj1cAttributeValues[47];
			}
		}
		public string azimuthLooks
		{
			set
			{
				hj1cAttributeValues[48] = value;
			}
			get
			{
				return hj1cAttributeValues[48];
			}
		}
		public string rangeLooks
		{
			set
			{
				hj1cAttributeValues[49] = value;
			}
			get
			{
				return hj1cAttributeValues[49];
			}
		}
		public string weightRange
		{
			set
			{
				hj1cAttributeValues[50] = value;
			}
			get
			{
				return hj1cAttributeValues[50];
			}
		}
		public string weightAzimuth
		{
			set
			{
				hj1cAttributeValues[51] = value;
			}
			get
			{
				return hj1cAttributeValues[51];
			}
		}
		public string doSpeckle
		{
			set
			{
				hj1cAttributeValues[52] = value;
			}
			get
			{
				return hj1cAttributeValues[52];
			}
		}
		public string rangePatternComp
		{
			set
			{
				hj1cAttributeValues[53] = value;
			}
			get
			{
				return hj1cAttributeValues[53];
			}
		}
		public string azimuthPatternComp
		{
			set
			{
				hj1cAttributeValues[54] = value;
			}
			get
			{
				return hj1cAttributeValues[54];
			}
		}
		public string antennaPatternSource
		{
			set
			{
				hj1cAttributeValues[55] = value;
			}
			get
			{
				return hj1cAttributeValues[55];
			}
		}
		public string fileSize
		{
			set
			{
				hj1cAttributeValues[56] = value;
			}
			get
			{
				return hj1cAttributeValues[56];
			}
		}
		public string earthModel
		{
			set
			{
				hj1cAttributeValues[57] = value;
			}
			get
			{
				return hj1cAttributeValues[57];
			}
		}
		public string mapProjection
		{
			set
			{
				hj1cAttributeValues[58] = value;
			}
			get
			{
				return hj1cAttributeValues[58];
			}
		}
		public string resampleTechnique
		{
			set
			{
				hj1cAttributeValues[59] = value;
			}
			get
			{
				return hj1cAttributeValues[59];
			}
		}
		public string productOrientation
		{
			set
			{
				hj1cAttributeValues[60] = value;
			}
			get
			{
				return hj1cAttributeValues[60];
			}
		}
		public string ephemerisData
		{
			set
			{
				hj1cAttributeValues[61] = value;
			}
			get
			{
				return hj1cAttributeValues[61];
			}

		}
		public string attitudeData
		{
			set
			{
				hj1cAttributeValues[62] = value;
			}
			get
			{
				return hj1cAttributeValues[62];
			}
		}
		public double sceneCenterLat
		{
			set
			{
				hj1cAttributeValues[63] = value.ToString();
			}
			get
			{
				double dt;
				if (double.TryParse(hj1cAttributeValues[63], out dt))
				{
					return dt;
				}
				else
				{
					return 180;
				}
			}
		}
		public double sceneCenterLong
		{
			set
			{
				hj1cAttributeValues[64] = value.ToString();
			}
			get
			{
				double dt;
				if (double.TryParse(hj1cAttributeValues[64], out dt))
				{
					return dt;
				}
				else
				{
					return 180;
				}
			}
		}
		public double dataUpperLeftLat
		{
			set
			{
				hj1cAttributeValues[65] = value.ToString();
			}
			get
			{
				double dt;
				if (double.TryParse(hj1cAttributeValues[65], out dt))
				{
					return dt;
				}
				else
				{
					return -90;
				}
			}
		}
		public double dataUpperLeftLong
		{
			set
			{
				hj1cAttributeValues[66] = value.ToString();
			}
			get
			{
				double dt;
				if (double.TryParse(hj1cAttributeValues[66], out dt))
				{
					return dt;
				}
				else
				{
					return -180;
				}
			}
		}
		public double dataUpperRightLat
		{
			set
			{
				hj1cAttributeValues[67] = value.ToString();
			}
			get
			{
				double dt;
				if (double.TryParse(hj1cAttributeValues[67], out dt))
				{
					return dt;
				}
				else
				{
					return 90;
				}
			}
		}
		public double dataUpperRightLong
		{
			set
			{
				hj1cAttributeValues[68] = value.ToString();
			}
			get
			{
				double dt;
				if (double.TryParse(hj1cAttributeValues[68], out dt))
				{
					return dt;
				}
				else
				{
					return 180;
				}
			}
		}
		public double dataLowerLeftLat
		{
			set
			{
				hj1cAttributeValues[69] = value.ToString();
			}
			get
			{
				double dt;
				if (double.TryParse(hj1cAttributeValues[69], out dt))
				{
					return dt;
				}
				else
				{
					return -90;
				}
			}
		}
		public double dataLowerLeftLong
		{
			set
			{
				hj1cAttributeValues[70] = value.ToString();
			}
			get
			{
				double dt;
				if (double.TryParse(hj1cAttributeValues[70], out dt))
				{
					return dt;
				}
				else
				{
					return -180;
				}
			}
		}
		public double dataLowerRightLat
		{
			set
			{
				hj1cAttributeValues[71] = value.ToString();
			}
			get
			{
				double dt;
				if (double.TryParse(hj1cAttributeValues[71], out dt))
				{
					return dt;
				}
				else
				{
					return 90;
				}
			}
		}
		public double dataLowerRightLong
		{
			set
			{
				hj1cAttributeValues[72] = value.ToString();
			}
			get
			{
				double dt;
				if (double.TryParse(hj1cAttributeValues[72], out dt))
				{
					return dt;
				}
				else
				{
					return 180;
				}
			}
		}
		public double productUpperLeftLat
		{
			set
			{
				hj1cAttributeValues[73] = value.ToString();
			}
			get
			{
				double dt;
				if (double.TryParse(hj1cAttributeValues[73], out dt))
				{
					return dt;
				}
				else
				{
					return -90;
				}
			}
		}
		public double productUpperLeftLong
		{
			set
			{
				hj1cAttributeValues[74] = value.ToString();
			}
			get
			{
				double dt;
				if (double.TryParse(hj1cAttributeValues[74], out dt))
				{
					return dt;
				}
				else
				{
					return -180;
				}
			}
		}
		public double productUpperRightLat
		{
			set
			{
				hj1cAttributeValues[75] = value.ToString();
			}
			get
			{
				double dt;
				if (double.TryParse(hj1cAttributeValues[75], out dt))
				{
					return dt;
				}
				else
				{
					return 90;
				}
			}
		}
		public double productUpperRightLong
		{
			set
			{
				hj1cAttributeValues[76] = value.ToString();
			}
			get
			{
				double dt;
				if (double.TryParse(hj1cAttributeValues[76], out dt))
				{
					return dt;
				}
				else
				{
					return 180;
				}
			}
		}
		public double productLowerLeftLat
		{
			set
			{
				hj1cAttributeValues[77] = value.ToString();
			}
			get
			{
				double dt;
				if (double.TryParse(hj1cAttributeValues[77], out dt))
				{
					return dt;
				}
				else
				{
					return -90;
				}
			}
		}
		public double productLowerLeftLong
		{
			set
			{
				hj1cAttributeValues[78] = value.ToString();
			}
			get
			{
				double dt;
				if (double.TryParse(hj1cAttributeValues[78], out dt))
				{
					return dt;
				}
				else
				{
					return -180;
				}
			}
		}
		public double productLowerRightLat
		{
			set
			{
				hj1cAttributeValues[79] = value.ToString();
			}
			get
			{
				double dt;
				if (double.TryParse(hj1cAttributeValues[79], out dt))
				{
					return dt;
				}
				else
				{
					return 90;
				}
			}
		}
		public double productLowerRightLong
		{
			set
			{
				hj1cAttributeValues[80] = value.ToString();
			}
			get
			{
				double dt;
				if (double.TryParse(hj1cAttributeValues[80], out dt))
				{
					return dt;
				}
				else
				{
					return 180;
				}
			}
		}
		public double dataUpperLeftX
		{
			set
			{
				hj1cAttributeValues[81] = value.ToString();
			}
			get
			{
				double dt;
				if (double.TryParse(hj1cAttributeValues[81], out dt))
				{
					return dt;
				}
				else
				{
					return -90;
				}
			}
		}
		public double dataUpperLeftY
		{
			set
			{
				hj1cAttributeValues[82] = value.ToString();
			}
			get
			{
				double dt;
				if (double.TryParse(hj1cAttributeValues[82], out dt))
				{
					return dt;
				}
				else
				{
					return -180;
				}
			}
		}
		public double dataUpperRightX
		{
			set
			{
				hj1cAttributeValues[83] = value.ToString();
			}
			get
			{
				double dt;
				if (double.TryParse(hj1cAttributeValues[83], out dt))
				{
					return dt;
				}
				else
				{
					return 90;
				}
			}
		}
		public double dataUpperRightY
		{
			set
			{
				hj1cAttributeValues[84] = value.ToString();
			}
			get
			{
				double dt;
				if (double.TryParse(hj1cAttributeValues[84], out dt))
				{
					return dt;
				}
				else
				{
					return 180;
				}
			}
		}
		public double dataLowerLeftX
		{
			set
			{
				hj1cAttributeValues[85] = value.ToString();
			}
			get
			{
				double dt;
				if (double.TryParse(hj1cAttributeValues[85], out dt))
				{
					return dt;
				}
				else
				{
					return -90;
				}
			}
		}
		public double dataLowerLeftY
		{
			set
			{
				hj1cAttributeValues[86] = value.ToString();
			}
			get
			{
				double dt;
				if (double.TryParse(hj1cAttributeValues[86], out dt))
				{
					return dt;
				}
				else
				{
					return -180;
				}
			}
		}
		public double dataLowerRithtX
		{
			set
			{
				hj1cAttributeValues[87] = value.ToString();
			}
			get
			{
				double dt;
				if (double.TryParse(hj1cAttributeValues[87], out dt))
				{
					return dt;
				}
				else
				{
					return 90;
				}
			}
		}
		public double dataLowerRightY
		{
			set
			{
				hj1cAttributeValues[88] = value.ToString();
			}
			get
			{
				double dt;
				if (double.TryParse(hj1cAttributeValues[88], out dt))
				{
					return dt;
				}
				else
				{
					return 180;
				}
			}
		}
		public double productUpperLeftX
		{
			set
			{
				hj1cAttributeValues[89] = value.ToString();
			}
			get
			{
				double dt;
				if (double.TryParse(hj1cAttributeValues[89], out dt))
				{
					return dt;
				}
				else
				{
					return -90;
				}
			}
		}
		public double productUpperLeftY
		{
			set
			{
				hj1cAttributeValues[90] = value.ToString();
			}
			get
			{
				double dt;
				if (double.TryParse(hj1cAttributeValues[90], out dt))
				{
					return dt;
				}
				else
				{
					return -180;
				}
			}
		}
		public double productUpperRightX
		{
			set
			{
				hj1cAttributeValues[91] = value.ToString();
			}
			get
			{
				double dt;
				if (double.TryParse(hj1cAttributeValues[91], out dt))
				{
					return dt;
				}
				else
				{
					return 90;
				}
			}
		}
		public double productUpperRightY
		{
			set
			{
				hj1cAttributeValues[92] = value.ToString();
			}
			get
			{
				double dt;
				if (double.TryParse(hj1cAttributeValues[92], out dt))
				{
					return dt;
				}
				else
				{
					return 180;
				}
			}
		}
		public double productLowerLeftX
		{
			set
			{
				hj1cAttributeValues[93] = value.ToString();
			}
			get
			{
				double dt;
				if (double.TryParse(hj1cAttributeValues[93], out dt))
				{
					return dt;
				}
				else
				{
					return -90;
				}
			}
		}
		public double productLowerLeftY
		{
			set
			{
				hj1cAttributeValues[94] = value.ToString();
			}
			get
			{
				double dt;
				if (double.TryParse(hj1cAttributeValues[94], out dt))
				{
					return dt;
				}
				else
				{
					return -180;
				}
			}
		}
		public double productLowerRightX
		{
			set
			{
				hj1cAttributeValues[95] = value.ToString();
			}
			get
			{
				double dt;
				if (double.TryParse(hj1cAttributeValues[95], out dt))
				{
					return dt;
				}
				else
				{
					return 90;
				}
			}
		}
		public double productLowerRightY
		{
			set
			{
				hj1cAttributeValues[96] = value.ToString();
			}
			get
			{
				double dt;
				if (double.TryParse(hj1cAttributeValues[96], out dt))
				{
					return dt;
				}
				else
				{
					return 180;
				}
			}
		}
		public string geodeticMethod
		{
			set
			{
				hj1cAttributeValues[97] = value;
			}
			get
			{
				return hj1cAttributeValues[97];
			}
		}
		public string dataFormatDes
		{
			set
			{
				hj1cAttributeValues[98] = value;
			}
			get
			{
				return hj1cAttributeValues[98];
			}
		}
		public string delStatus
		{
			set
			{
				hj1cAttributeValues[99] = value;
			}
			get
			{
				return hj1cAttributeValues[99];
			}
		}
		public string dataTempDir
		{
			set
			{
				hj1cAttributeValues[100] = value;
			}
			get
			{
				return hj1cAttributeValues[100];
			}
		}
		public string QRST_CODE
		{
			set;
			get;
		}
		#endregion Model

		public override void ImportData(IDbBaseUtilities sqlBase)
		{			
			try
            {
                //TableLocker dblock = new TableLocker(sqlBase);
                Constant.IdbOperating.LockTable("prod_hj1c",EnumDBType.MIDB);
				string presql = string.Format("select ID,QRST_CODE from prod_hj1c where Name ='{0}'", Name);
				DataSet ds = sqlBase.GetDataSet(presql);
				if (ds != null && ds.Tables.Count != 0 && ds.Tables[0].Rows.Count > 0)
				{
					ID = int.Parse(ds.Tables[0].Rows[0]["ID"].ToString());
					QRST_CODE = ds.Tables[0].Rows[0]["QRST_CODE"].ToString();
					presql = string.Format("delete from prod_hj1c where QRST_CODE ='{0}'", QRST_CODE);
					//DataSet ds = sqlBase.GetDataSet(presql);
					int i = sqlBase.ExecuteSql(presql);
				}
				else
				{
					tablecode_Dal tablecode = new tablecode_Dal(sqlBase);
					ID = sqlBase.GetMaxID("ID", "prod_hj1c");
					QRST_CODE = tablecode.GetDataQRSTCode("prod_hj1c", ID);
				}

				StringBuilder strSql = new StringBuilder();
				strSql.Append("insert into prod_hj1c(");
				strSql.Append("ID,Name,productId,sceneId,satelliteId,sensorId,recStationId,productDate,productLevel,pixelSpacing,productType,sceneCount,sceneShift,overallQuality,dataType,satPath,satRow,satPathbias,satRowbias,scenePath,sceneRow,scenePathbias,sceneRowbias,sceneDate,sceneTime,sarMode,imagingStartTime,imagingStopTime,antennaOffNadir,mgcCode,anteAngle,beamNumber,incidentAngle,bands,sampleDelay,coordinateSystem,rangeRes,azimuthRes,nominalRangeRes,nominalAzimuthRes,nominalPSLR,nominalASLR,imagingAlgor,fdcMethod,fdrMethod,doIqComp,doAGCComp,doADCComp,doInnerCalibComp,doProcessorGainComp,azimuthLooks,rangeLooks,weightRange,weightAzimuth,doSpeckle,rangePatternComp,azimuthPatternComp,antennaPatternSource,fileSize,earthModel,mapProjection,resampleTechnique,productOrientation,ephemerisData,attitudeData,sceneCenterLat,sceneCenterLong,dataUpperLeftLat,dataUpperLeftLong,dataUpperRightLat,dataUpperRightLong,dataLowerLeftLat,dataLowerLeftLong,dataLowerRightLat,dataLowerRightLong,productUpperLeftLat,productUpperLeftLong,productUpperRightLat,productUpperRightLong,productLowerLeftLat,productLowerLeftLong,productLowerRightLat,productLowerRightLong,dataUpperLeftX,dataUpperLeftY,dataUpperRightX,dataUpperRightY,dataLowerLeftX,dataLowerLeftY,dataLowerRithtX,dataLowerRightY,productUpperLeftX,productUpperLeftY,productUpperRightX,productUpperRightY,productLowerLeftX,productLowerLeftY,productLowerRightX,productLowerRightY,geodeticMethod,dataFormatDes,delStatus,dataTempDir,QRST_CODE)");
				strSql.Append(" values (");
                strSql.Append(
                    string.Format(
                        "{0},'{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}','{12}','{13}','{14}','{15}','{16}','{17}','{18}'," +
                        "'{19}','{20}','{21}','{22}','{23}','{24}','{25}','{26}','{27}','{28}','{29}','{30}','{31}','{32}'," +
                        "'{33}','{34}','{35}','{36}','{37}','{38}','{39}','{40}','{41}','{42}','{43}','{44}','{45}','{46}','{47}'," +
                        "'{48}','{49}','{50}','{51}','{52}','{53}','{54}','{55}','{56}','{57}','{58}','{59}','{60}','{61}','{62}'," +
                        "'{63}','{64}',{65},{66},{67},{68},{69},{70},{71},{72},{73},{74},{75},{76},{77},{78}," +
                        "{79},{80},{81},{82},{83},{84},{85},{86},{87},{88},{89},{90},{91},{92}," +
                        "{93},{94},{95},{96},{97},{98},'{99}','{100}','{101}','{102}','{103}')", ID, Name, productId,
                        sceneId, satelliteId, sensorId, recStationId, productDate.ToString("yyyy-MM-dd HH:mm:ss"), productLevel, pixelSpacing, productType,
                        sceneCount, sceneShift, overallQuality, dataType, satPath, satRow, satPathbias, satRowbias, scenePath,
                        sceneRow, scenePathbias, sceneRowbias, sceneDate.ToString("yyyy-MM-dd HH:mm:ss"), sceneTime, sarMode, imagingStartTime, imagingStopTime,
                        antennaOffNadir, mgcCode, anteAngle, beamNumber, incidentAngle, bands, sampleDelay, coordinateSystem,
                        rangeRes, azimuthRes, nominalRangeRes, nominalAzimuthRes, nominalPSLR, nominalASLR, imagingAlgor,
                        fdcMethod, fdrMethod, doIqComp, doAGCComp, doADCComp, doInnerCalibComp, doProcessorGainComp,
                        azimuthLooks, rangeLooks, weightRange, weightAzimuth, doSpeckle, rangePatternComp,
                        azimuthPatternComp, antennaPatternSource, fileSize, earthModel, mapProjection,
                        resampleTechnique, productOrientation, ephemerisData, attitudeData, sceneCenterLat,
                        sceneCenterLong, dataUpperLeftLat, dataUpperLeftLong, dataUpperRightLat, dataUpperRightLong,
                        dataLowerLeftLat, dataLowerLeftLong, dataLowerRightLat, dataLowerRightLong, productUpperLeftLat,
                        productUpperLeftLong, productUpperRightLat, productUpperRightLong, productLowerLeftLat,
                        productLowerLeftLong, productLowerRightLat, productLowerRightLong, dataUpperLeftX, dataUpperLeftY,
                        dataUpperRightX, dataUpperRightY, dataLowerLeftX, dataLowerLeftY, dataLowerRithtX, dataLowerRightY,
                        productUpperLeftX, productUpperLeftY, productUpperRightX, productUpperRightY, productLowerLeftX,
                        productLowerLeftY, productLowerRightX, productLowerRightY, geodeticMethod, dataFormatDes, delStatus,
                        dataTempDir, QRST_CODE));
                //			strSql.Append("@ID,@Name,@productId,@sceneId,@satelliteId,@sensorId,@recStationId,@productDate,@productLevel,@pixelSpacing,@productType,@sceneCount,@sceneShift,@overallQuality,@dataType,@satPath,@satRow,@satPathbias,@satRowbias,@scenePath,@sceneRow,@scenePathbias,@sceneRowbias,@sceneDate,@sceneTime,@sarMode,@imagingStartTime,@imagingStopTime,@antennaOffNadir,@mgcCode,@anteAngle,@beamNumber,@incidentAngle,@bands,@sampleDelay,@coordinateSystem,@rangeRes,@azimuthRes,@nominalRangeRes,@nominalAzimuthRes,@nominalPSLR,@nominalASLR,@imagingAlgor,@fdcMethod,@fdrMethod,@doIqComp,@doAGCComp,@doADCComp,@doInnerCalibComp,@doProcessorGainComp,@azimuthLooks,@rangeLooks,@weightRange,@weightAzimuth,@doSpeckle,@rangePatternComp,@azimuthPatternComp,@antennaPatternSource,@fileSize,@earthModel,@mapProjection,@resampleTechnique,@productOrientation,@ephemerisData,@attitudeData,@sceneCenterLat,@sceneCenterLong,@dataUpperLeftLat,@dataUpperLeftLong,@dataUpperRightLat,@dataUpperRightLong,@dataLowerLeftLat,@dataLowerLeftLong,@dataLowerRightLat,@dataLowerRightLong,@productUpperLeftLat,@productUpperLeftLong,@productUpperRightLat,@productUpperRightLong,@productLowerLeftLat,@productLowerLeftLong,@productLowerRightLat,@productLowerRightLong,@dataUpperLeftX,@dataUpperLeftY,@dataUpperRightX,@dataUpperRightY,@dataLowerLeftX,@dataLowerLeftY,@dataLowerRithtX,@dataLowerRightY,@productUpperLeftX,@productUpperLeftY,@productUpperRightX,@productUpperRightY,@productLowerLeftX,@productLowerLeftY,@productLowerRightX,@productLowerRightY,@geodeticMethod,@dataFormatDes,@delStatus,@dataTempDir,@QRST_CODE)");
                //			MySqlParameter[] parameters = {
                //				new MySqlParameter("@ID", MySqlDbType.Decimal,20),
                //				new MySqlParameter("@Name", MySqlDbType.Text),
                //				new MySqlParameter("@productId", MySqlDbType.VarChar,45),
                //				new MySqlParameter("@sceneId", MySqlDbType.VarChar,45),
                //				new MySqlParameter("@satelliteId", MySqlDbType.VarChar,45),
                //				new MySqlParameter("@sensorId", MySqlDbType.VarChar,45),
                //				new MySqlParameter("@recStationId", MySqlDbType.VarChar,45),
                //new MySqlParameter("@productDate",MySqlDbType.DateTime),
                //new MySqlParameter("@productLevel",MySqlDbType.VarChar,45),
                //new MySqlParameter("@pixelSpacing",MySqlDbType.VarChar,45),
                //new MySqlParameter("@productType",MySqlDbType.VarChar,45),
                //new MySqlParameter("@sceneCount",MySqlDbType.VarChar,45),
                //new MySqlParameter("@sceneShift",MySqlDbType.VarChar,45),
                //new MySqlParameter("@overallQuality",MySqlDbType.VarChar,45),
                //new MySqlParameter("@dataType",MySqlDbType.VarChar,45),
                //new MySqlParameter("@satPath",MySqlDbType.VarChar,45),
                //new MySqlParameter("@satRow",MySqlDbType.VarChar,45),
                //new MySqlParameter("@satPathbias",MySqlDbType.VarChar,45),
                //new MySqlParameter("@satRowbias",MySqlDbType.VarChar,45),
                //new MySqlParameter("@scenePath",MySqlDbType.VarChar,45),
                //new MySqlParameter("@sceneRow",MySqlDbType.VarChar,45),
                //new MySqlParameter("@scenePathbias",MySqlDbType.VarChar,45),
                //new MySqlParameter("@sceneRowbias",MySqlDbType.VarChar,45),
                //new MySqlParameter("@sceneDate",MySqlDbType.DateTime),
                //new MySqlParameter("@sceneTime",MySqlDbType.VarChar,45),
                //new MySqlParameter("@sarMode",MySqlDbType.VarChar,45),
                //new MySqlParameter("@imagingStartTime",MySqlDbType.DateTime),
                //new MySqlParameter("@imagingStopTime",MySqlDbType.DateTime),
                //new MySqlParameter("@antennaOffNadir",MySqlDbType.VarChar,45),
                //new MySqlParameter("@mgcCode",MySqlDbType.VarChar,45),
                //new MySqlParameter("@anteAngle",MySqlDbType.VarChar,45),
                //new MySqlParameter("@beamNumber",MySqlDbType.VarChar,45),
                //new MySqlParameter("@incidentAngle",MySqlDbType.VarChar,45),
                //new MySqlParameter("@bands",MySqlDbType.VarChar,45),
                //new MySqlParameter("@sampleDelay",MySqlDbType.VarChar,45),
                //new MySqlParameter("@coordinateSystem",MySqlDbType.VarChar,45),
                //new MySqlParameter("@rangeRes",MySqlDbType.VarChar,45),
                //new MySqlParameter("@azimuthRes",MySqlDbType.VarChar,45),
                //new MySqlParameter("@nominalRangeRes",MySqlDbType.VarChar,45),
                //new MySqlParameter("@nominalAzimuthRes",MySqlDbType.VarChar,45),
                //new MySqlParameter("@nominalPSLR",MySqlDbType.VarChar,45),
                //new MySqlParameter("@nominalASLR",MySqlDbType.VarChar,45),
                //new MySqlParameter("@imagingAlgor",MySqlDbType.VarChar,45),
                //new MySqlParameter("@fdcMethod",MySqlDbType.VarChar,45),
                //new MySqlParameter("@fdrMethod",MySqlDbType.VarChar,45),
                //new MySqlParameter("@doIqComp",MySqlDbType.VarChar,45),
                //new MySqlParameter("@doAGCComp",MySqlDbType.VarChar,45),
                //new MySqlParameter("@doADCComp",MySqlDbType.VarChar,45),
                //new MySqlParameter("@doInnerCalibComp",MySqlDbType.VarChar,45),
                //new MySqlParameter("@doProcessorGainComp",MySqlDbType.VarChar,45),
                //new MySqlParameter("@azimuthLooks",MySqlDbType.VarChar,45),
                //new MySqlParameter("@rangeLooks",MySqlDbType.VarChar,45),
                //new MySqlParameter("@weightRange",MySqlDbType.VarChar,45),
                //new MySqlParameter("@weightAzimuth",MySqlDbType.VarChar,45),
                //new MySqlParameter("@doSpeckle",MySqlDbType.VarChar,45),
                //new MySqlParameter("@rangePatternComp",MySqlDbType.VarChar,45),
                //new MySqlParameter("@azimuthPatternComp",MySqlDbType.VarChar,45),
                //new MySqlParameter("@antennaPatternSource",MySqlDbType.VarChar,45),
                //new MySqlParameter("@fileSize",MySqlDbType.VarChar,45),
                //new MySqlParameter("@earthModel",MySqlDbType.VarChar,45),
                //new MySqlParameter("@mapProjection",MySqlDbType.VarChar,45),
                //new MySqlParameter("@resampleTechnique",MySqlDbType.VarChar,45),
                //new MySqlParameter("@productOrientation",MySqlDbType.VarChar,45),
                //new MySqlParameter("@ephemerisData",MySqlDbType.VarChar,45),
                //new MySqlParameter("@attitudeData",MySqlDbType.VarChar,45),
                //new MySqlParameter("@sceneCenterLat", MySqlDbType.Decimal,20),
                //new MySqlParameter("@sceneCenterLong",MySqlDbType.Decimal,20),
                //new MySqlParameter("@dataUpperLeftLat",MySqlDbType.Decimal,20),
                //new MySqlParameter("@dataUpperLeftLong",MySqlDbType.Decimal,20),
                //new MySqlParameter("@dataUpperRightLat",MySqlDbType.Decimal,20),
                //new MySqlParameter("@dataUpperRightLong",MySqlDbType.Decimal,20),
                //new MySqlParameter("@dataLowerLeftLat",MySqlDbType.Decimal,20),
                //new MySqlParameter("@dataLowerLeftLong",MySqlDbType.Decimal,20),
                //new MySqlParameter("@dataLowerRightLat",MySqlDbType.Decimal,20),
                //new MySqlParameter("@dataLowerRightLong",MySqlDbType.Decimal,20),
                //new MySqlParameter("@productUpperLeftLat",MySqlDbType.Decimal,20),
                //new MySqlParameter("@productUpperLeftLong",MySqlDbType.Decimal,20),
                //new MySqlParameter("@productUpperRightLat",MySqlDbType.Decimal,20),
                //new MySqlParameter("@productUpperRightLong",MySqlDbType.Decimal,20),
                //new MySqlParameter("@productLowerLeftLat",MySqlDbType.Decimal,20),
                //new MySqlParameter("@productLowerLeftLong",MySqlDbType.Decimal,20),
                //new MySqlParameter("@productLowerRightLat",MySqlDbType.Decimal,20),
                //new MySqlParameter("@productLowerRightLong",MySqlDbType.Decimal,20),
                //new MySqlParameter("@dataUpperLeftX",MySqlDbType.Decimal,20),
                //new MySqlParameter("@dataUpperLeftY",MySqlDbType.Decimal,20),
                //new MySqlParameter("@dataUpperRightX",MySqlDbType.Decimal,20),
                //new MySqlParameter("@dataUpperRightY",MySqlDbType.Decimal,20),
                //new MySqlParameter("@dataLowerLeftX",MySqlDbType.Decimal,20),
                //new MySqlParameter("@dataLowerLeftY",MySqlDbType.Decimal,20),
                //new MySqlParameter("@dataLowerRithtX",MySqlDbType.Decimal,20),
                //new MySqlParameter("@dataLowerRightY",MySqlDbType.Decimal,20),
                //new MySqlParameter("@productUpperLeftX",MySqlDbType.Decimal,20),
                //new MySqlParameter("@productUpperLeftY",MySqlDbType.Decimal,20),
                //new MySqlParameter("@productUpperRightX",MySqlDbType.Decimal,20),
                //new MySqlParameter("@productUpperRightY",MySqlDbType.Decimal,20),
                //new MySqlParameter("@productLowerLeftX",MySqlDbType.Decimal,20),
                //new MySqlParameter("@productLowerLeftY",MySqlDbType.Decimal,20),
                //new MySqlParameter("@productLowerRightX",MySqlDbType.Decimal,20),
                //new MySqlParameter("@productLowerRightY",MySqlDbType.Decimal,20),
                //new MySqlParameter("@geodeticMethod", MySqlDbType.VarChar,45),
                //new MySqlParameter("@dataFormatDes", MySqlDbType.VarChar,45),
                //new MySqlParameter("@delStatus", MySqlDbType.VarChar,45),
                //new MySqlParameter("@dataTempDir", MySqlDbType.VarChar,45),
                //new MySqlParameter("@QRST_CODE", MySqlDbType.VarChar,45)};
                //			parameters[0].Value = ID;
                //			parameters[1].Value = Name;
                //			parameters[2].Value =  productId;
                //			parameters[3].Value=sceneId;
                //			parameters[4].Value = satelliteId;
                //			parameters[5].Value = sensorId;
                //			parameters[6].Value = recStationId;
                //			parameters[7].Value = productDate;
                //			parameters[8].Value = productLevel;
                //			parameters[9].Value = pixelSpacing;
                //			parameters[10].Value = productType;
                //			parameters[11].Value = sceneCount;
                //			parameters[12].Value=sceneShift;
                //			parameters[13].Value = overallQuality;
                //			parameters[14].Value = dataType;
                //			parameters[15].Value = satPath;
                //			parameters[16].Value = satRow;
                //			parameters[17].Value = satPathbias;
                //			parameters[18].Value = satRowbias;
                //			parameters[19].Value = scenePath;
                //			parameters[20].Value = sceneRow;
                //			parameters[21].Value = scenePathbias;
                //			parameters[22].Value = sceneRowbias;
                //			parameters[23].Value = sceneDate;
                //			parameters[24].Value = sceneTime;
                //			parameters[25].Value = sarMode;
                //			parameters[26].Value = imagingStartTime;
                //			parameters[27].Value = imagingStopTime;
                //			parameters[28].Value = antennaOffNadir;
                //			parameters[29].Value = mgcCode;
                //			parameters[30].Value = anteAngle;
                //			parameters[31].Value = beamNumber;
                //			parameters[32].Value = incidentAngle;
                //			parameters[33].Value = bands;
                //			parameters[34].Value = sampleDelay;
                //			parameters[35].Value = coordinateSystem;
                //			parameters[36].Value = rangeRes;
                //			parameters[37].Value = azimuthRes;
                //			parameters[38].Value = nominalRangeRes;
                //			parameters[39].Value = nominalAzimuthRes;
                //			parameters[40].Value = nominalPSLR;
                //			parameters[41].Value = nominalASLR;
                //			parameters[42].Value = imagingAlgor;
                //			parameters[43].Value = fdcMethod;
                //			parameters[44].Value = fdrMethod;
                //			parameters[45].Value = doIqComp;
                //			parameters[46].Value = doAGCComp;
                //			parameters[47].Value = doADCComp;
                //			parameters[48].Value = doInnerCalibComp;
                //			parameters[49].Value = doProcessorGainComp;
                //			parameters[50].Value = azimuthLooks;
                //			parameters[51].Value = rangeLooks;
                //			parameters[52].Value = weightRange;
                //			parameters[53].Value = weightAzimuth;
                //			parameters[54].Value = doSpeckle;
                //			parameters[55].Value = rangePatternComp;
                //			parameters[56].Value = azimuthPatternComp;
                //			parameters[57].Value = antennaPatternSource;
                //			parameters[58].Value = fileSize;
                //			parameters[59].Value = earthModel;
                //			parameters[60].Value = mapProjection;
                //			parameters[61].Value = resampleTechnique;
                //			parameters[62].Value = productOrientation;
                //			parameters[63].Value = ephemerisData;
                //			parameters[64].Value = attitudeData;
                //			parameters[65].Value = sceneCenterLat;
                //			parameters[66].Value = sceneCenterLong;
                //			parameters[67].Value = dataUpperLeftLat;
                //			parameters[68].Value = dataUpperLeftLong;
                //			parameters[69].Value = dataUpperRightLat;
                //			parameters[70].Value = dataUpperRightLong;
                //			parameters[71].Value = dataLowerLeftLat;
                //			parameters[72].Value = dataLowerLeftLong;
                //			parameters[73].Value = dataLowerRightLat;
                //			parameters[74].Value = dataLowerRightLong;
                //			parameters[75].Value = productUpperLeftLat;
                //			parameters[76].Value = productUpperLeftLong;
                //			parameters[77].Value = productUpperRightLat;
                //			parameters[78].Value = productUpperRightLong;
                //			parameters[79].Value = productLowerLeftLat;
                //			parameters[80].Value = productLowerLeftLong;
                //			parameters[81].Value = productLowerRightLat;
                //			parameters[82].Value = productLowerRightLong;
                //			parameters[83].Value = dataUpperLeftX;
                //			parameters[84].Value = dataUpperLeftY;
                //			parameters[85].Value = dataUpperRightX;
                //			parameters[86].Value = dataUpperRightY;
                //			parameters[87].Value = dataLowerLeftX;
                //			parameters[88].Value = dataLowerLeftY;
                //			parameters[89].Value = dataLowerRithtX;
                //			parameters[90].Value = dataLowerRightY;
                //			parameters[91].Value = productUpperLeftX;
                //			parameters[92].Value = productUpperLeftY;
                //			parameters[93].Value = productUpperRightX;
                //			parameters[94].Value = productUpperRightY;
                //			parameters[95].Value = productLowerLeftX;
                //			parameters[96].Value = productLowerLeftY;
                //			parameters[97].Value = productLowerRightX;
                //			parameters[98].Value = productLowerRightY;
                //			parameters[99].Value = geodeticMethod;
                //			parameters[100].Value = dataFormatDes;
                //			parameters[101].Value = delStatus;
                //			parameters[102].Value = dataTempDir;
                //			parameters[103].Value = QRST_CODE;
                sqlBase.ExecuteSql(strSql.ToString());


                //string destCorrectedData = this.GetCorrectedDataPath();
                ////如果纠正归档数据目录存在且里面有文件则1，否则为-1
                //string corDataPath = (Directory.Exists(destCorrectedData) && Directory.GetFiles(destCorrectedData).Length > 1) ? "1" : "-1";
                //string updatesql = string.Format("update prod_hj1c set CorDataFlag = {0} where Name = '{1}'", corDataPath, Name);

                //sqlBase.ExecuteSql(updatesql);
                Constant.IdbOperating.UnlockTable("prod_hj1c",EnumDBType.MIDB);
            }
			catch (Exception ex)
			{
				throw new Exception("元数据导入失败" + ex.ToString());
			}
		}

		public override void ReadAttributes(string fileName)
		{
			XmlDocument root = new XmlDocument();
			try
			{
				root.Load(fileName);
			}
			catch (System.Exception ex)
			{
				throw new Exception("xml文件损坏，请检查！");
			}
			XmlNode node = null;

			try
			{
				for (int i = 0; i < hj1cAttributeNames.Length; i++)
				{
					node = root.GetElementsByTagName(hj1cAttributeNames[i]).Item(0);
					if (node == null)
					{
						hj1cAttributeValues[i] = "";
					}
					else
					{
						hj1cAttributeValues[i] = node.InnerText;
					}
				}
			}
			catch (Exception ex)
			{
				throw new Exception("读取元数据信息出错" + ex.ToString());
			}
		}

		//HJ1C-Strip-SAR-12-166-A1-20140319-L20000083629
		public override string GetRelateDataPath()
		{
			string[] strArr = Name.Split("-".ToCharArray());
			if (strArr.Length == 8)
			{
				string satellite = strArr[0];
				string sensor = strArr[2];
				string year = strArr[6].Substring(0, 4);
				string month = strArr[6].Substring(4, 2);
				string day = strArr[6].Substring(6, 2);
				return string.Format("实验验证数据库\\HJ1C卫星数据\\{0}\\{1}\\{2}\\{3}\\{4}\\{5}\\", satellite, sensor, year, month, day, Path.GetFileNameWithoutExtension(Path.GetFileNameWithoutExtension(Name)));
			}
			else
			{
				return base.GetRelateDataPath();
			}

		}

		public string GetCorrectedDataPath()
		{
			string[] strArr = Name.Split("_".ToCharArray());
			if (strArr.Length == 6)
			{
				string satellite = strArr[0];
				string sensor = strArr[1];
				string year = strArr[4].Substring(0, 4);
				string month = strArr[4].Substring(4, 2);
				string day = strArr[4].Substring(6, 2);
				return string.Format("{6}数据产品库\\数据预处理产品\\{0}\\{1}\\{2}\\{3}\\{4}\\{5}\\", satellite, sensor, year, month, day, Path.GetFileNameWithoutExtension(Path.GetFileNameWithoutExtension(Name)), StoragePath.StoreBasePath);
			}
			else
			{
				return "";
			}
		}

		public override void GetModel(string qrst_code,IDbBaseUtilities sqlBase)
		{
			StringBuilder strSql = new StringBuilder();
			strSql.Append("select * from prod_hj1c ");
			strSql.AppendFormat(" where QRST_CODE = '{0}'", qrst_code);

			using (DataSet ds = sqlBase.GetDataSet(strSql.ToString()))
			{
				if (ds.Tables[0].Rows.Count > 0)
				{
					Name = ds.Tables[0].Rows[0]["NAME"].ToString();
					satelliteId = ds.Tables[0].Rows[0]["satelliteId"].ToString();
					sensorId = ds.Tables[0].Rows[0]["sensorId"].ToString();
					IsCreated = true;
				}
				else
				{
					IsCreated = false;
				}
			}
		}
	}
}
