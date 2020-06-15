using System;
using System.Runtime.InteropServices;
using QRST_DI_Resources;
using QRST_DI_SS_DBInterfaces.IDBEngine;
 
namespace QRST_DI_DS_DBEngine
{
    public class SynDBTimeToLocalClass
    {
        //imports SetLocalTime function from kernel32.dll 
        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern int SetLocalTime(ref SystemTime lpSystemTime);

        private IDbBaseUtilities dbu;

        public SynDBTimeToLocalClass()
        {
            //dbu = new MySqlBaseUtilities();
            switch (Constant.QrstDbEngine)
            {
                case EnumDbEngine.MYSQL:
                    dbu = new MySqlBaseUtilities();
                    break;
                case EnumDbEngine.SQLITE:
                    dbu = new SQLiteBaseUtilities();
                    break;
                case EnumDbEngine.ClOUDDB:
                    break;
            }
        }

        private DateTime GetDBTime()
        {
            return dbu.GetDBNowTime();
        }

        public void SynDBTimeToLocal()
        {
            DateTime dbt = GetDBTime();     //2015-06-24 11:53:39

            ////实例一个Process类，启动一个独立进程 
            //Process p = new Process();
            ////Process类有一个StartInfo属性 
            ////设定程序名 
            //p.StartInfo.FileName = "cmd.exe";
            ////设定程式执行参数    “/C”表示执行完命令后马上退出
            //p.StartInfo.Arguments = "/c date 2020-2-20";
            ////关闭Shell的使用   
            //p.StartInfo.UseShellExecute = false;
            ////重定向标准输入      
            //p.StartInfo.RedirectStandardInput = true;
            //p.StartInfo.RedirectStandardOutput = true;
            ////重定向错误输出   
            //p.StartInfo.RedirectStandardError = true;
            ////设置不显示doc窗口  
            //p.StartInfo.CreateNoWindow = true;
            ////启动 
            //p.Start();

            ////从输出流取得命令执行结果 
            //string str= p.StandardOutput.ReadToEnd();



            // And then set up a structure with the required properties and call the api from code: 

            SystemTime systNew = new SystemTime();

            // 设置属性 
            systNew.wDay = (short)dbt.Day;
            systNew.wMonth = (short)dbt.Month;
            systNew.wYear = (short)dbt.Year;
            systNew.wHour = (short)dbt.Hour;
            systNew.wMinute = (short)dbt.Minute;
            systNew.wSecond = (short)dbt.Second;

            // 调用API，更新系统时间 
            int rst = SetLocalTime(ref systNew);
        }


    }

    //struct for date/time apis 
    public struct SystemTime
    {
        public short wYear;
        public short wMonth;
        public short wDayOfWeek;
        public short wDay;
        public short wHour;
        public short wMinute;
        public short wSecond;
        public short wMilliseconds;
    }

}
