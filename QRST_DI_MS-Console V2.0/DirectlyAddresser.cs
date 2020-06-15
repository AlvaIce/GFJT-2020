using System;

namespace Yaan_AppSysWinForm
{ 
    class DirectlyAddresser
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="LatLon">/最小行Y，最小列X，最大行，最大列  </param>
        /// <param name="lv"></param>
        /// <returns></returns>
        public static int[] GetRowAndColum(string[] LatLon, string lv)
        {
            #region
            ////LatLon[0] leftLat  LatLon[1] rightLat latLon[]
            ////最小行，最小列，最大行，最大列  
            //double a = getLevelRate(lv);//一个注释说是0.25度
            //int[] rowAndColum = new int[4];
            ////原有代码 有错
            ////rowAndColum[0] = Convert.ToInt32((C//onvert.ToDouble(LatLon[0]) + 90) * a);
            ////rowAndColum[1] = Convert.ToInt32((Convert.ToDouble(LatLon[1]) + 180) * a);
            ////rowAndColum[2] = Convert.ToInt32((Convert.ToDouble(LatLon[2]) + 90) * a);
            ////rowAndColum[3] = Convert.ToInt32((Convert.ToDouble(LatLon[3]) + 180) * a);

            ////当前计算方式是相交的按照不在范围内进行计算，若改为相交的在范围内则取消注释1和注释2，并注释3和4对应部分代码
            ////最小行
            //double minRow = (Convert.ToDouble(LatLon[0]) + 90) * a;//最小行
            //double correctedMinRow = Math.Floor(minRow);//取整
            ////注释1
            ////if (minRow == correctedMinRow)
            ////{
            ////    correctedMinRow--;
            ////}
            //rowAndColum[0] = Convert.ToInt32(correctedMinRow);
            ////最小列
            //double minColumn = (Convert.ToDouble(LatLon[1]) + 180) * a;//最小列
            //double correctedMinColumn = Math.Floor(minColumn);
            ////if (minColumn == correctedMinColumn)
            ////{
            ////    correctedMinColumn--;
            ////}
            //rowAndColum[1] = Convert.ToInt32(Math.Floor(correctedMinColumn));
            ////最大行
            //double maxRow = (Convert.ToDouble(LatLon[2]) + 90) * a;
            //double correctedMaxRow = Math.Floor(maxRow);
            ////注释3
            //if (maxRow == correctedMaxRow)
            //{
            //    correctedMaxRow--;
            //}
            //rowAndColum[2] = Convert.ToInt32(correctedMaxRow);

            ////最大列
            //double maxColumn = (Convert.ToDouble(LatLon[3]) + 180) * a;
            //double correctedMaxColumn = Math.Floor(maxColumn);
            ////注释4
            //if (maxColumn == correctedMaxColumn)
            //{
            //    correctedMaxColumn--;
            //}
            //rowAndColum[3] = Convert.ToInt32(correctedMaxColumn);
            //return rowAndColum;
            #endregion
            //LatLon[0] leftLat  LatLon[1] rightLat latLon[]
            //最小行，最小列，最大行，最大列  
            double a = getLevelRate(lv);//一个注释说是0.25度
            int[] rowAndColum = new int[4];
            //原有代码 有错
            //rowAndColum[0] = Convert.ToInt32((C//onvert.ToDouble(LatLon[0]) + 90) * a);
            //rowAndColum[1] = Convert.ToInt32((Convert.ToDouble(LatLon[1]) + 180) * a);
            //rowAndColum[2] = Convert.ToInt32((Convert.ToDouble(LatLon[2]) + 90) * a);
            //rowAndColum[3] = Convert.ToInt32((Convert.ToDouble(LatLon[3]) + 180) * a);

            //当前计算方式是相交的按照不在范围内进行计算，若改为相交的在范围内则取消注释1和注释2，并注释3和4对应部分代码
            //最小行
            double minRow = (Convert.ToDouble(LatLon[0]) + 90) * a;//最小行
            double correctedMinRow = Math.Floor(minRow);//取整
            //注释1
            if (minRow == correctedMinRow)
            {
                correctedMinRow--;
            }
            rowAndColum[0] = Convert.ToInt32(correctedMinRow);

            //最小列
            double minColumn = (Convert.ToDouble(LatLon[1]) + 180) * a;//最小列
            double correctedMinColumn = Math.Floor(minColumn);
            if (minColumn == correctedMinColumn)
            {
                correctedMinColumn--;
            }
            rowAndColum[1] = Convert.ToInt32(Math.Floor(correctedMinColumn));

            //最大行
            double maxRow = (Convert.ToDouble(LatLon[2]) + 90) * a;
            double correctedMaxRow = Math.Floor(maxRow);
            //注释3
            //if (maxRow == correctedMaxRow)
            //{
            //    correctedMaxRow--;
            //}
            rowAndColum[2] = Convert.ToInt32(correctedMaxRow);

            //最大列
            double maxColumn = (Convert.ToDouble(LatLon[3]) + 180) * a;
            double correctedMaxColumn = Math.Floor(maxColumn);
            //注释4
            //if (maxColumn == correctedMaxColumn)
            //{
            //    correctedMaxColumn--;
            //}
            rowAndColum[3] = Convert.ToInt32(correctedMaxColumn);

            return rowAndColum;
        }
        /// <summary> 
        /// 最小纬度，最小经度，最大纬度，最大经度  
        /// </summary>
        /// <param name="rowAndColum"></param>
        /// <param name="lv"></param>
        /// <returns></returns>
        public static double[] GetLatAndLong(string[] rowAndColum, string lv)//行列号的二值数组
        {
            double a = getLevelRate(lv);
            double[] latAndlong = new double[4];
            latAndlong[0] = Convert.ToDouble(rowAndColum[0]) / a - 90;//最小纬度
            latAndlong[1] = Convert.ToDouble(rowAndColum[1]) / a - 180;//最大经度
            latAndlong[2] = Convert.ToDouble(Convert.ToInt32(rowAndColum[0]) + 1) / a - 90;
            latAndlong[3] = Convert.ToDouble(Convert.ToInt32(rowAndColum[1]) + 1) / a - 180;
            return latAndlong;
        }

        /// <summary>
        /// 获取层级字母
        /// </summary>
        /// <param name="lv"></param>
        /// <returns></returns>
        public static double getLevelRate(string lv)
        {
            if (lv.Length <= 0)
            {
                return 2;
            }

            double a = 1;
            switch (lv)
            {
                case "F":
                    a = 0.02;
                    break;
                case "E":
                    a = 0.04;
                    break;
                case "D":
                    a = 0.1;
                    break;
                case "C":
                    a = 0.2;
                    break;
                case "B":
                    a = 0.4;
                    break;
                case "A":
                    a = 1;
                    break;
                case "9":
                    a = 2;
                    break;
                case "8":
                    a = 4;      //0.25度
                    break;
                case "7":
                    a = 10;     //0.1
                    break;
                case "6":
                    a = 20;
                    break;
                case "5":
                    a = 40;
                    break;
                case "4":
                    a = 100;
                    break;
                case "3":
                    a = 200;
                    break;
                case "2":
                    a = 400;
                    break;
                case "1":
                    a = 1000;   //
                    break;
                default:
                    break;
            }
            return a;
        }
        /// <summary>
        /// 层级获取间隔度数
        /// </summary>
        /// <param name="strLv"></param>
        /// <returns></returns>
        public static string GetDegreeByStrLv(string strLv)
        {
            string deg = "8";
            switch (strLv)
            {
                case "9":
                    deg = "0.5";
                    break;
                case "8":
                    deg = "0.25";
                    break;
                case "7":
                    deg = "0.1";
                    break;
                case "6":
                    deg = "0.05";
                    break;
                case "5":
                    deg = "0.025";
                    break;
                case "4":
                    deg = "0.01";
                    break;
                case "3":
                    deg = "0.005";
                    break;
                case "2":
                    deg = "0.0025";
                    break;
                case "1":
                    deg = "0.001";
                    break;
                default:
                    break;
            }
            return deg;
        }
        public static double getRateDoule(string Rate)
        {
            return double.Parse(Rate.TrimEnd("米".ToCharArray()));
        }


        public static string GetResolutionByStrLv(string strLv)
        {
            string res = "7";
            switch (strLv)
            {
                case "9":
                    res = "50米";
                    break;
                case "8":
                    res = "25米";
                    break;
                case "7":
                    res = "10米";
                    break;
                case "6":
                    res = "5米";
                    break;
                case "5":
                    res = "2.5米";
                    break;
                case "4":
                    res = "1米";
                    break;
                case "3":
                    res = "0.5米";
                    break;
                case "2":
                    res = "0.25米";
                    break;
                default:
                    break;
            }
            return res;
        }

        public static string NewGetResolutionByStrLv(string strLv)
        {
            string res = "7";
            switch (strLv)
            {
                //case "9":
                //    res = "50米";
                //    break;
                case "8":
                    res = "16米";
                    break;
                case "7":
                    res = "8米";
                    break;
                case "6":
                    res = "2米";
                    break;
                case "5":
                    res = "1米";
                    break;
                case "4":
                    res = "1米";
                    break;
                //case "3":
                //    res = "0.5米";
                //    break;
                //case "2":
                //    res = "0.25米";
                //    break;
                default:
                    break;
            }
            return res;
        }
        public static string GetStrLvByResolution(string res)
        {
            string strlv = "7";
            switch (res)
            {
                case "50米":
                    strlv = "9";
                    break;
                case "25米":
                    strlv = "8";
                    break;
                case "10米":
                    strlv = "7";
                    break;
                case "5米":
                    strlv = "6";
                    break;
                case "2.5米":
                    strlv = "5";
                    break;
                case "1米":
                    strlv = "4";
                    break;
                case "0.5米":
                    strlv = "3";
                    break;
                case "0.25米":
                    strlv = "2";
                    break;
                default:
                    break;
            }
            return strlv;
        }

        public static string getLevel(string Rate)
        {
            string a = "1";
            switch (Rate)
            {
                case "5000米":
                    a = "F";
                    break;
                case "2500米":
                    a = "E";
                    break;
                case "1000米":
                    a = "D";
                    break;
                case "500米":
                    a = "C";
                    break;
                case "250米":
                    a = "B";
                    break;
                case "100米":
                    a = "A";
                    break;
                case "50米":
                    a = "9";
                    break;
                case "25米":
                    a = "8";
                    break;
                case "10米":
                    a = "7";
                    break;
                case "5米":
                    a = "6";
                    break;
                case "2.5米":
                    a = "5";
                    break;
                case "1米":
                    a = "4";
                    break;
                case "0.5米":
                    a = "3";
                    break;
                case "0.25米":
                    a = "2";      //0.25度
                    break;
                case "0.1米":
                    a = "1";     //0.1
                    break;
                default:
                    break;
            }
            return a;
        }
        public static string getClassifyName(string num)
        {
            string ClassifyName = "";
            switch (num)
            { 
                case "编号":
                    ClassifyName = "类型";
                    break;
                case "1":
                    //ClassifyName = "水体";
                    ClassifyName = "大棚";
                    break;
                case "2":
                    //ClassifyName = "裸地";
                    ClassifyName = "农田";
                    break;
                case "3":
                    //ClassifyName = "植被";
                    ClassifyName = "裸地";
                    break;
                case "4":
                    ClassifyName = "建筑地";
                    break;
                case "5":
                    ClassifyName = "其他";
                    break;
                case "总计":
                    ClassifyName = "总计";
                    break;
                default: break;
            }
            return ClassifyName;
        }
    }
}
