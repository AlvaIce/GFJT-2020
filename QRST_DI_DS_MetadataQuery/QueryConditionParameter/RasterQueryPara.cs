/*
 * 作者：zxw
 * 创建时间：2013-08-06
 * 描述：用于封装栅格数据查询参数，栅格数据查询包括时间查询，空间查询，卫星传感器查询等
*/ 
using System;
using System.Collections.Generic;
using System.Linq;
using QRST_DI_SS_Basis;
using QRST_DI_SS_Basis.MetadataQuery;

namespace QRST_DI_DS_MetadataQuery.QueryConditionParameter
{
    [Serializable]
    public class RasterQueryPara : QueryPara
    {
         //空间查询参数值，空间查询中空间范围值  
        public string EXTENTUP { get; set; }
        public string EXTENTDOWN { get; set; }
        public string EXTENTLEFT { get; set; }
        public string EXTENTRIGHT { get; set; }
         //记录字段名，由于每一个具有空间查询属性的元数据表都包含空间信息字段，下面八个字段用于存储不同源数据表中空间信息的字段名称
        public string DATAUPPERLEFTLAT { get; set; }
        public string DATAUPPERLEFTLONG { get; set; }
        public string DATAUPPERRIGHTLAT { get; set; }
        public string DATAUPPERRIGHTLONG { get; set; }
        public string DATALOWERRIGHTLAT { get; set; }
        public string DATALOWERRIGHTLONG { get; set; }
        public string DATALOWERLEFTLAT { get; set; }
        public string DATALOWERLEFTLONG { get; set; }
        public bool spacialAvailable = true;
         //时间查询参数值 
         public string STARTTIME { get; set; }
         public string ENDTIME { get; set; }

         //卫星传感器查询参数值
         public string SENSOR { get; set; }
         public string SATELLITE { get; set; }

         //关键字查询参数
         public string KEYWORDS { get; set; }

         //根据入库时间进行查询
         public string IMPORTSTARTTIME { get; set; }
         public string IMPORTENDTIME { get; set; }

         //非规影像级产品类型
         public bool isimageprod = false;
         public string IMAGEPRODTYPE { get; set; }


         public override ComplexCondition GetSpecificCondition(IGetQuerySchema querySchema)
         {
             ComplexCondition cp = new ComplexCondition();
             Dictionary<string, string> dic = querySchema.GetFieldName();

             List<SimpleCondition> queryCondition = new List<SimpleCondition>();
             //组装空间查询条件，如果部分空间字段找不到，则忽略空间查询条件
             spacialAvailable = true;
             TVMapping mapping =new TVMapping();
             //公共字段对应值
             QRST_CODE = mapping.GetValue("QRST_CODE");
             QRST_CODE = GetField(dic, QRST_CODE.Split(":".ToArray()));
             THUMBNAIL = mapping.GetValue("THUMBNAIL");
             THUMBNAIL = GetField(dic, THUMBNAIL.Split(":".ToArray()));  
             #region 构建空间查询条件
              DATAUPPERLEFTLAT = mapping.GetValue("RASTER.DATAUPPERLEFTLAT");
             DATAUPPERLEFTLAT = GetField(dic, DATAUPPERLEFTLAT.Split(":".ToArray()));
             if (string.IsNullOrEmpty(DATAUPPERLEFTLAT))
             {
                 spacialAvailable = false;
                 queryCondition.Clear();
             }
             else
             {
                 SimpleCondition sm = new SimpleCondition();
                 sm.accessPointField = DATAUPPERLEFTLAT;
                 sm.comparisonOperatorField = "<=";
                 sm.valueField = EXTENTUP;
                 queryCondition.Add(sm);
             }

             DATAUPPERRIGHTLAT = mapping.GetValue("RASTER.DATAUPPERRIGHTLAT");
             DATAUPPERRIGHTLAT = GetField(dic, DATAUPPERRIGHTLAT.Split(":".ToArray()));
             if (string.IsNullOrEmpty(DATAUPPERRIGHTLAT) || !spacialAvailable)
             {
                 spacialAvailable = false;
                 queryCondition.Clear();
             }
             else
             {
                 SimpleCondition sm = new SimpleCondition();
                 sm.accessPointField = DATAUPPERRIGHTLAT;
                 sm.comparisonOperatorField = "<=";
                 sm.valueField = EXTENTUP;
                 queryCondition.Add(sm);
             }
             //DATALOWERLEFTLAT
             DATALOWERLEFTLAT = mapping.GetValue("RASTER.DATALOWERLEFTLAT");
             DATALOWERLEFTLAT = GetField(dic, DATALOWERLEFTLAT.Split(":".ToArray()));
             if (string.IsNullOrEmpty(DATALOWERLEFTLAT) || !spacialAvailable)
             {
                 spacialAvailable = false;
                 queryCondition.Clear();
             }
             else
             {
                 SimpleCondition sm = new SimpleCondition();
                 sm.accessPointField = DATALOWERLEFTLAT;
                 sm.comparisonOperatorField = ">=";
                 sm.valueField = EXTENTDOWN;
                 queryCondition.Add(sm);
             }
             //DATALOWERRIGHTLAT
            DATALOWERRIGHTLAT = mapping.GetValue("RASTER.DATALOWERRIGHTLAT");
             DATALOWERRIGHTLAT = GetField(dic, DATALOWERRIGHTLAT.Split(":".ToArray()));
             if (string.IsNullOrEmpty(DATALOWERRIGHTLAT) || !spacialAvailable)
             {
                 spacialAvailable = false;
                 queryCondition.Clear();
             }
             else
             {
                 SimpleCondition sm = new SimpleCondition();
                 sm.accessPointField = DATALOWERRIGHTLAT;
                 sm.comparisonOperatorField = ">=";
                 sm.valueField = EXTENTDOWN;
                 queryCondition.Add(sm);
             }
             //DATAUPPERLEFTLONG
             DATAUPPERLEFTLONG = mapping.GetValue("RASTER.DATAUPPERLEFTLONG");
             DATAUPPERLEFTLONG = GetField(dic, DATAUPPERLEFTLONG.Split(":".ToArray()));
             if (string.IsNullOrEmpty(DATAUPPERLEFTLONG) || !spacialAvailable)
             {
                 spacialAvailable = false;
                 queryCondition.Clear();
             }
             else
             {
                 SimpleCondition sm = new SimpleCondition();
                 sm.accessPointField = DATAUPPERLEFTLONG;
                 sm.comparisonOperatorField = ">=";
                 sm.valueField = EXTENTLEFT;
                 queryCondition.Add(sm);
             }
             //DATALOWERLEFTLONG
              DATALOWERLEFTLONG = mapping.GetValue("RASTER.DATALOWERLEFTLONG");
             DATALOWERLEFTLONG = GetField(dic, DATALOWERLEFTLONG.Split(":".ToArray()));
             if (string.IsNullOrEmpty(DATALOWERLEFTLONG) || !spacialAvailable)
             {
                 spacialAvailable = false;
                 queryCondition.Clear();
             }
             else
             {
                 SimpleCondition sm = new SimpleCondition();
                 sm.accessPointField = DATALOWERLEFTLONG;
                 sm.comparisonOperatorField = ">=";
                 sm.valueField = EXTENTLEFT;
                 queryCondition.Add(sm);
             }
             //DATAUPPERRIGHTLONG
             DATAUPPERRIGHTLONG = mapping.GetValue("RASTER.DATAUPPERRIGHTLONG");
             DATAUPPERRIGHTLONG = GetField(dic, DATAUPPERRIGHTLONG.Split(":".ToArray()));
             if (string.IsNullOrEmpty(DATAUPPERRIGHTLONG) || !spacialAvailable)
             {
                 spacialAvailable = false;
                 queryCondition.Clear();
             }
             else
             {
                 SimpleCondition sm = new SimpleCondition();
                 sm.accessPointField = DATAUPPERRIGHTLONG;
                 sm.comparisonOperatorField = "<=";
                 sm.valueField = EXTENTRIGHT;
                 queryCondition.Add(sm);
             }
             // DATALOWERRIGHTLONG
             DATALOWERRIGHTLONG = mapping.GetValue("RASTER.DATALOWERRIGHTLONG");
             DATALOWERRIGHTLONG = GetField(dic, DATALOWERRIGHTLONG.Split(":".ToArray()));
             if (string.IsNullOrEmpty(DATALOWERRIGHTLONG) || !spacialAvailable)
             {
                 spacialAvailable = false;
                 queryCondition.Clear();
             }
             else
             {
                 SimpleCondition sm = new SimpleCondition();
                 sm.accessPointField = DATALOWERRIGHTLONG;
                 sm.comparisonOperatorField = "<=";
                 sm.valueField = EXTENTRIGHT;
                 queryCondition.Add(sm);
             }
             #endregion
             
             #region 构建时间查询条件
             string starttimeField = mapping.GetValue("RASTER.STARTTIME");
             starttimeField = GetField(dic, starttimeField.Split(":".ToArray()));
             if (!string.IsNullOrEmpty(starttimeField) && !string.IsNullOrEmpty(STARTTIME))  //存在时间查询字段
             {
                 SimpleCondition sm = new SimpleCondition();
                 sm.accessPointField = starttimeField;
                 sm.comparisonOperatorField = ">=";
                 sm.valueField = STARTTIME;
                 queryCondition.Add(sm);
    
             }
             string endtimeField = mapping.GetValue("RASTER.ENDTIME");
             endtimeField = GetField(dic, endtimeField.Split(":".ToArray()));
             if (!string.IsNullOrEmpty(endtimeField) && !string.IsNullOrEmpty(ENDTIME))  //存在时间查询字段
             {
                 SimpleCondition sm = new SimpleCondition();
                 sm.accessPointField = endtimeField;
                 sm.comparisonOperatorField = "<=";
                 sm.valueField = ENDTIME;
                 queryCondition.Add(sm);

                
             }
#endregion 
             #region 构建入库时间查询条件
             string importStarttimeField = mapping.GetValue("RASTER.IMPORTSTARTTIME");
             importStarttimeField = GetField(dic, importStarttimeField.Split(":".ToArray()));
             if (!string.IsNullOrEmpty(importStarttimeField) && !string.IsNullOrEmpty(IMPORTSTARTTIME))  //存在时间查询字段
             {
                 SimpleCondition sm = new SimpleCondition();
                 sm.accessPointField = importStarttimeField;
                 sm.comparisonOperatorField = ">=";
                 sm.valueField = IMPORTSTARTTIME;
                 queryCondition.Add(sm);

             }
             string importEndtimeField = mapping.GetValue("RASTER.IMPORTENDTIME");
             importEndtimeField = GetField(dic, importEndtimeField.Split(":".ToArray()));
             if (!string.IsNullOrEmpty(importEndtimeField) && !string.IsNullOrEmpty(IMPORTENDTIME))  //存在时间查询字段
             {
                 SimpleCondition sm = new SimpleCondition();
                 sm.accessPointField = importEndtimeField;
                 sm.comparisonOperatorField = "<=";
                 sm.valueField = IMPORTENDTIME;
                 queryCondition.Add(sm);


             }
             #endregion 

             #region  卫星传感器条件
             string sensorField = mapping.GetValue("RASTER.SENSOR");
             sensorField = GetField(dic, sensorField.Split(":".ToArray()));
             
             //添加为可以同时查询多个卫星 传感器查询   修改时间：20131018   zxw
             ComplexCondition sensorCondition = new ComplexCondition();
             if (!string.IsNullOrEmpty(sensorField)&&!string.IsNullOrEmpty(SENSOR))
             {
                 string[] senArr = SENSOR.Split(",".ToCharArray());
                 sensorCondition.logicOperator = EnumLogicalOperator.OR;
                 sensorCondition.simpleCondition = new SimpleCondition[senArr.Length];
                 for (int i = 0; i < senArr.Length; i++)
                 {
                     sensorCondition.simpleCondition[i] = new SimpleCondition();
                     sensorCondition.simpleCondition[i].accessPointField = sensorField;
                     sensorCondition.simpleCondition[i].comparisonOperatorField = "=";
                     sensorCondition.simpleCondition[i].valueField = senArr[i].Trim();
                 }
             }
            
             //if(senArr.Length>0)
             //{
             //    SENSOR = senArr[0];
             //}
             //if (!string.IsNullOrEmpty(sensorField)&&!string.IsNullOrEmpty(SENSOR))  //存在传感器查询字段
             //{
             //    SimpleCondition sm = new SimpleCondition();
             //    sm.accessPointField = sensorField;
             //    sm.comparisonOperatorField = "=";
             //    sm.valueField = SENSOR;
             //    queryCondition.Add(sm);
             //}

             string satelliteField = mapping.GetValue("RASTER.SATELLITE");
             satelliteField = GetField(dic, satelliteField.Split(":".ToArray()));
             ComplexCondition satelliteCondition = new ComplexCondition();
             if (!string.IsNullOrEmpty(satelliteField) && !string.IsNullOrEmpty(SATELLITE))
             {
                 string[] satArr = SATELLITE.Split(",".ToCharArray());
                 satelliteCondition.logicOperator = EnumLogicalOperator.OR;
                 satelliteCondition.simpleCondition = new SimpleCondition[satArr.Length];
                 for (int i = 0; i < satArr.Length;i++ )
                 {
                     satelliteCondition.simpleCondition[i] = new SimpleCondition();
                     satelliteCondition.simpleCondition[i].accessPointField = satelliteField;
                     satelliteCondition.simpleCondition[i].comparisonOperatorField = "=";
                     satelliteCondition.simpleCondition[i].valueField = satArr[i].Trim();
                 }
             }

                 ComplexCondition prodtypeCondition = new ComplexCondition();
             if (isimageprod)
             {
                 string prodtypeField = mapping.GetValue("RASTER.IMAGEPRODTYPE");
                 prodtypeField = GetField(dic, prodtypeField.Split(":".ToArray()));
                 if (!string.IsNullOrEmpty(prodtypeField) && !string.IsNullOrEmpty(IMAGEPRODTYPE))
                 {
                     string[] typeArr = IMAGEPRODTYPE.Split(",".ToCharArray());
                     prodtypeCondition.logicOperator = EnumLogicalOperator.OR;
                     prodtypeCondition.simpleCondition = new SimpleCondition[typeArr.Length];
                     for (int i = 0; i < typeArr.Length; i++)
                     {
                         prodtypeCondition.simpleCondition[i] = new SimpleCondition();
                         prodtypeCondition.simpleCondition[i].accessPointField = prodtypeField;
                         prodtypeCondition.simpleCondition[i].comparisonOperatorField = "=";
                         prodtypeCondition.simpleCondition[i].valueField = typeArr[i].Trim();
                     }
                 }
             }
             //if(satArr.Length>0)
             //{
             //    SATELLITE = satArr[0];
             //}
             //string satelliteField = mapping.GetValue("RASTER.SATELLITE");
             //satelliteField = GetField(dic, satelliteField.Split(":".ToArray()));
             //if (!string.IsNullOrEmpty(satelliteField)&&!string.IsNullOrEmpty(SATELLITE))  //存在时间查询字段
             //{
             //    SimpleCondition sm = new SimpleCondition();
             //    sm.accessPointField = satelliteField;
             //    sm.comparisonOperatorField = "=";
             //    sm.valueField = SATELLITE;
             //    queryCondition.Add(sm);
             //}
             #endregion

             #region 关键字
             string[] keywordsField = mapping.GetValue("RASTER.KEYWORDS").Split(":".ToCharArray());
             ComplexCondition keyWordCondition = new ComplexCondition();
             keyWordCondition.logicOperator = EnumLogicalOperator.OR;
             List<SimpleCondition> scs = new List<SimpleCondition>();
             for (int i = 0; i < keywordsField.Length;i++ )
             {
                 if(dic.ContainsValue(keywordsField[i]))
                 {
                     SimpleCondition sm = new SimpleCondition();
                     sm.accessPointField = keywordsField[i];
                     sm.comparisonOperatorField = "like";
                     sm.valueField = string.Format("%{0}%",KEYWORDS);
                     scs.Add(sm);
                 }
             }
             keyWordCondition.simpleCondition = scs.ToArray();
             #endregion
             cp.simpleCondition = queryCondition.ToArray();
             //添加复杂查询条件
             cp.complexCondition = new ComplexCondition[4] { sensorCondition, satelliteCondition, prodtypeCondition,keyWordCondition };

             return cp;
         }

    }
}
