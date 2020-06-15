using System;
using System.Collections.Generic;

namespace DataPrepare
{
    class StaticTools
    {
        /// <summary>
        /// 静态工具类
        /// </summary>
        /// <param name="strList"></param>
        /// <returns></returns>
        public static string getReturnMsg(List<string> strList,string taskUserName)
        {
            /*
             * T4220457838340#AndroidApp任务接收#结束#综合数据库#赵亚萌#0001-EVDB-32-6&0001-EVDB-32-7#GF1_PMS1_E82.2_N44.7_20130816_L1A0000070789.tar.gz&GF1_PMS1_E82.2_N44.7_20130820_L1A0000072202.tar.gz#高分系列卫星数据
             */

            string result = "";
            result += GenerateTaskId() + "#";
            result += "AndroidApp任务接收#";
            result += "结束#";
            result += Constant.SystemName+"#";
            result += taskUserName+"#";
            foreach (string str in strList)
            {
                string[] subStrs = str.Split(new char[] { '#' });
                result += subStrs[1] + "&";
            }
            result += "#";
            foreach (string str in strList)
            {
                string[] subStrs = str.Split(new char[] { '#' });
                result += subStrs[0] + "&";
            }
            result += "#";
            result += "高分系列卫星数据" + "#";
            return result;
        }

        /// <summary>
        /// 动态生成任务ID
        /// </summary>
        /// <returns></returns>
        public static string GenerateTaskId()
        {
            string type = "T";
            string date = ((DateTime.Now.Year - 1900) * 365 + DateTime.Now.DayOfYear).ToString("00000");
            string time = ((int)DateTime.Now.TimeOfDay.TotalSeconds).ToString("00000");
            string msecond = DateTime.Now.Millisecond.ToString("000");
            System.Threading.Thread.Sleep(1);
            //返回任务单号
            return string.Format("{0}{1}{2}{3}", type, date, time, msecond);
        }

        public static double[] GetLatAndLong(string row, string column, string lv)//行列号的二值数组
        {
            double a = getLevelRate(lv);
            double[] latAndlong = new double[4];
            latAndlong[0] = Convert.ToDouble(row) / a - 90;//最小纬度
            latAndlong[1] = Convert.ToDouble(column) / a - 180;//最小经度
            latAndlong[2] = Convert.ToDouble(Convert.ToInt32(row) + 1) / a - 90;//最大纬度
            latAndlong[3] = Convert.ToDouble(Convert.ToInt32(column) + 1) / a - 180;//最大经度
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
        /// 通过原始数据的卫星号、传感器号判断是否为给定的层级
        /// </summary>
        /// <param name="satellite"></param>
        /// <param name="sensor"></param>
        /// <param name="level"></param>
        /// <returns></returns>
        public static Boolean isResolutionMatch(string satellite, string sensor, string level)
        {
            List<string> levels = StaticTools.getLevelBySatelliteAndSensor(satellite, sensor);
            if (levels.Contains(level))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 通过原始数据的卫星和传感器得到所在的层级
        /// </summary>
        public static List<string> getLevelBySatelliteAndSensor(string satellite, string sensor)
        {
            List<string> result = new List<string>();
            if (sensor.Contains("PMS"))
            {
                if (satellite.Equals("GF1"))
                {
                    result.Add("5");
                    result.Add("7");
                }
                else
                {
                    result.Add("4");
                    result.Add("6");
                }
            }
            else if (sensor.Contains("WFV"))
            {
                result.Add("8");
            }
            return result;
        }

        /// <summary>
        /// 向集成共享发送消息
        /// </summary>
        /// <param name="msg"></param>
        public static void SendMessage(String msg)
        {
            Form1.mcc.SendMessage(msg);
        }


    }
}
