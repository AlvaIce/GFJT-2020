using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QRST_DI_Basis
{ 
   public class RowColumnTransform
    {
        /// <summary>
        /// 根据经纬度获取行列号
        /// 最小行，最小列，最大行，最大列  
        /// </summary>
        /// <param name="LatLon">最小纬度、最小经度、最大纬度、最大经度</param>
        /// <returns></returns>
        public static int[] GetRowAndColum(string[] LatLon, string lv)
        {
            //LatLon[0] leftLat  LatLon[1] rightLat latLon[]
            //最小行，最小列，最大行，最大列  
            double a = getLevelRate(lv);
            int[] rowAndColum = new int[4];
            //原有代码 有错
            //rowAndColum[0] = Convert.ToInt32((C//onvert.ToDouble(LatLon[0]) + 90) * a);
            //rowAndColum[1] = Convert.ToInt32((Convert.ToDouble(LatLon[1]) + 180) * a);
            //rowAndColum[2] = Convert.ToInt32((Convert.ToDouble(LatLon[2]) + 90) * a);
            //rowAndColum[3] = Convert.ToInt32((Convert.ToDouble(LatLon[3]) + 180) * a);

            //当前计算方式是相交的按照不在范围内进行计算，若改为相交的在范围内则取消注释1和注释2，并注释3和4对应部分代码
            //最小行
            double minRow = (Convert.ToDouble(LatLon[0]) + 90) * a;
            double correctedMinRow = Math.Floor(minRow);
            //注释1
            //if (minRow == correctedMinRow)
            //{
            //    correctedMinRow--;
            //}
            rowAndColum[0] = Convert.ToInt32(correctedMinRow);

            //最小列
            double minColumn = (Convert.ToDouble(LatLon[1]) + 180) * a;
            double correctedMinColumn = Math.Floor(minColumn);
            //注释2
            //if (minColumn == correctedMinColumn)
            //{
            //    correctedMinColumn--;
            //}
            rowAndColum[1] = Convert.ToInt32(Math.Floor(correctedMinColumn));

            //最大行
            double maxRow = (Convert.ToDouble(LatLon[2]) + 90) * a;
            double correctedMaxRow = Math.Floor(maxRow);
            //注释3
            if (maxRow == correctedMaxRow)
            {
                correctedMaxRow--;
            }
            rowAndColum[2] = Convert.ToInt32(correctedMaxRow);

            //最大列
            double maxColumn = (Convert.ToDouble(LatLon[3]) + 180) * a;
            double correctedMaxColumn = Math.Floor(maxColumn);
            //注释4
            if (maxColumn == correctedMaxColumn)
            {
                correctedMaxColumn--;
            }
            rowAndColum[3] = Convert.ToInt32(correctedMaxColumn);

            return rowAndColum;
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

    }
}
