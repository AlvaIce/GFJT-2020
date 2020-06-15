using System;

namespace QRST_DI_DS_Metadata.MetaDataCls
{
    public static class TimeConvert
    {
        private static int[] leapyear = { 31,29,31,30,31,30,31,31,30,31,30,31};
        private static int[] nomalyear = { 31, 29, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31 };
        private static bool isLeapYear(int year)
        {
            if ( (year%4 == 0 && year%100 == 0) || year%400 == 0)
            {
                return true;
            }
            return false;
        }

        public static string datetimeConvert(string strYear, string strDay, string strMillisecond)
        {
            int year = int.Parse(strYear);
            int day = int.Parse(strDay);
            long millisecond = long.Parse(strMillisecond);
            string monthday, hour;
            //小时，分，秒，毫秒：hour
            long temp = millisecond / 3600000;
            hour = temp+":";
            millisecond = millisecond - temp * 3600000;
            temp = millisecond / 60000;
            hour += temp + ":";
            millisecond = millisecond - temp * 60000;
            temp = millisecond / 1000;
            hour += temp + ".";
            millisecond = millisecond - temp * 1000;
            hour += millisecond;
            //月日：monthday
            int mouth = 0;
            if (isLeapYear(year))
            {
                while (day > leapyear[mouth])
                {
                    day -= leapyear[mouth];
                    mouth++;
                }
            } 
            else
            {
                while (day > nomalyear[mouth])
                {
                    day -= nomalyear[mouth];
                    mouth++;
                }
            }
            mouth++;
            monthday = String.Format("{0}-{1}", mouth, day);
            return String.Format("{0}-{1} {2}", strYear, monthday, hour);
        }


        public static string datetimeConvert(string strYear, string strDay)
        {
            int year = int.Parse(strYear);
            int day = int.Parse(strDay);
            string monthday;
            //月日：monthday
            int mouth = 0;
            if (isLeapYear(year))
            {
                while (day > leapyear[mouth])
                {
                    day -= leapyear[mouth];
                    mouth++;
                }
            }
            else
            {
                while (day > nomalyear[mouth])
                {
                    day -= nomalyear[mouth];
                    mouth++;
                }
            }
            mouth++;
            monthday = String.Format("{0}-{1}", mouth, day);
            return String.Format("{0}-{1}", strYear, monthday);
        }
    }
}
