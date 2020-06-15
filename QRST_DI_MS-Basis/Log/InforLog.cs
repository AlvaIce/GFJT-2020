using System.Collections.Generic;
using log4net;
using QRST_DI_TS_Basis.Search;
using QRST_DI_SS_Basis.TileSearch;

namespace QRST_DI_MS_Basis.Log
{

    public class InforLog<T>
    {
        static string elemStr;
        //写入日志,获得输出信息的日志对象
        public static ILog inforLog = LogManager.GetLogger("infor");

        /// <summary>
        /// 循环输出List对象String Item，
        /// </summary>
        /// <param name="list"></param>
        /// <returns>item 字符串</returns>
        public static string returnListStrElem(List<T> list)
        {
            elemStr = "";
            if (list.Count != 0)
            {
                foreach (T item in list)
                {
                    elemStr += item.ToString() + ",";
                }
            }
            else
                elemStr = "NULL";
            return elemStr;
        }

        /// <summary>
        /// 获取TileLevelPosition对象的成员
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public static string returnTileLevPosElem(List<TileLevelPosition> list)
        {
            elemStr = "tileLevel=";
            string posStr = "position=";
            if (list.Count > 0)
            {
                foreach (TileLevelPosition item in list)
                {
                    elemStr += item.TileLevel + "；";
                    posStr += InforLog<int>.returnSArrStrElem(item.tileRowandColumn) + "；";
                }
                elemStr = elemStr + " " + posStr;
            }
            else
                elemStr = "NULL";
            return elemStr;
        }

        /// <summary>
        ///获取一维数组成员
        /// </summary>
        /// <param name="sArr"></param>
        /// <returns></returns>
        public static string returnSArrStrElem(T[] sArr)
        {
            elemStr = "";
            foreach (T elem in sArr)
            {
                elemStr += elem.ToString() + "，";
            }
            return elemStr;
        }

        /// <summary>
        /// 把二维数组值变成字符串
        /// </summary>
        /// <param name="arr">二维数组</param>
        /// <returns></returns>
        public static string returnTArrStrElem(string[][] arr)
        {
            elemStr = "";
            int i = arr.GetLength(0);       //行数
            int j = arr[0].Length;       //列数
            string s = "[";
            if (i > 0 && j > 0)
            {
                for (int m = 0; m < i; m++)
                {
                    for (int n = 0; n < j; n++)
                    {
                        s += arr[m][n] + ",";
                    }
                    s += "],";
                    elemStr = elemStr + s;
                    s = "[";
                }

            }
            else
                elemStr = "NULL";
            return elemStr;
        }

        //
    }
}
