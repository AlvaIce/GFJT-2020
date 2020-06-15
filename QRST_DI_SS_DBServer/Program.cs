/**
 * 描    述:1.提供MS、TSServer之间的通信服务。     TPC
            2.提供MS、TSServer与SQLiteEngine之间的通信。  IPC
            3.单端口多通道通信
 * 作    者:  jianghua
 * 创建日期:  201704010
 */

using System;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using QRST_DI_DS_DBEngine;
using QRST_DI_Resources;
using QRST_DI_SS_Basis.MetaData;
using QRST_DI_SS_DBClient.DBEngine;
using QRST_DI_SS_DBClient.TCP;
using QRST_DI_SS_DBInterfaces.IDBEngine;
using QRST_DI_SS_DBServer.DBEngine;
using QRST_DI_SS_DBServer.DBService;
using System.Collections.Generic;
using System.Net;
using System.Net.NetworkInformation;
using System.Collections;
using QRST_DI_SS_Basis;

namespace QRST_DI_SS_DBServer
{
    class Program
    {
        [DllImport("User32.dll", EntryPoint = "FindWindow")]
        private static extern int FindWindow(string lpClassName, string lpWindowName);

        [DllImport("user32.dll", EntryPoint = "ShowWindow", SetLastError = true)]
        static extern bool ShowWindow(IntPtr hWnd, uint nCmdShow);
        private static IDbBaseUtilities baseUtilities = null;
        public static string version = "1.7.0630.1519";

        static void Main(string[] args)
        {

            string isShow = ConfigurationManager.AppSettings["IsShowConsole"];
            if (isShow.Trim().ToLower().Equals("false"))
            {
                Console.Title = "QRST_DI_SS_DBServer";
                int hWnd = FindWindow(null, @"QRST_DI_SS_DBServer");
                ShowWindow((IntPtr)hWnd, 0);    //默认启动隐藏
            }
            try
            {
                //单机版本生成端口,写入到C:\Program Files\QRST\AppCongig.xml
                if (Constant.DbStorage == EnumDbStorage.SINGLE)
                {
                    int port = GetValidPorts();
                    XmlUtility.InsertPidPortNode("dbUtilityTcpPort", port.ToString());
                }
                //打印信息
                DisplayConfigInfo();

                StartDbOperatingServ();
                StartBaseUtilitiesServ();
                Constant.Create();

                #region SQLite单机模式 midb库更新DbServer路径
                if (Constant.DbStorage == EnumDbStorage.SINGLE)
                {
                    baseUtilities = Constant.IdbServerUtilities;
                    //更新绝对路径
                    UpdateServerPath();
                }
                #endregion

                #region 开启TCP服务
                Thread tcpServiceThread = new Thread(StartTCPService);
                tcpServiceThread.Start();

                Thread submitOrderTcpThread = new Thread(StartSubmitOrderTcpServer);
                submitOrderTcpThread.Start();

                Thread tileTcpThread = new Thread(StartTileTcpServer);
                tileTcpThread.Start();

                Thread sqliteTcpThread = new Thread(StartDbTcpServer);
                sqliteTcpThread.Start();

                Thread appTcpThread = new Thread(StartAppTcpServer);
                appTcpThread.Start();

                Thread getDataTcpThread = new Thread(StartGetDataTcpServer);
                getDataTcpThread.Start();


                //Thread utilityThread = new Thread(StartBaseUtilitiesServ);
                //utilityThread.Start();
                #endregion


            }
            catch (Exception e)
            {
                MyConsole.WriteLine(string.Format("!!!异常{0}:{1}", DateTime.Now.ToString(), e));
            }

            Console.ReadLine();
        }

        public static void DisplayConfigInfo()
        {
            Console.WriteLine(string.Format("Version:{0}", version));
            Console.WriteLine("#########数据库服务信息#########");
            Console.WriteLine(string.Format("数据库服务通信端口：{0}", Constant.dbUtilityTcpPort));
            Console.WriteLine(string.Format("数据库服务器地址：{0}", Constant.DbServerIp));
            Console.WriteLine("#########服务状态信息#########");

        }

        #region TCP服务
        private static void StartTCPService()
        {
            TileSiteTCPServiceServer.StartTCPService(Constant.dbUtilityTcpPort);
            Console.WriteLine("TCPServiceServer服务已启动，开始监听...");
        }

        private static void StartBaseUtilitiesServ()
        {
            DbUtilitiesTCPServer.StartTCPService(Constant.dbUtilityTcpPort);
            Console.WriteLine("SLUtilitiesTCPServer服务已启动，开始监听...");
        }

        private static void StartDbOperatingServ()
        {
            DbOperatingTCPServer operatingTcpServer=new DbOperatingTCPServer();
            operatingTcpServer.StartTCPService(Constant.dbUtilityTcpPort);
            Console.WriteLine("SLOperatingTCPServer服务已启动，开始监听...");
        }

        private static void StartSubmitOrderTcpServer()
        {
            QDB_SubmitOrderTCPServer.StartTCPService(Constant.dbUtilityTcpPort);
            Console.WriteLine("QDB_SubmitOrderTCPServer服务已启动，开始监听...");
        }

        private static void StartTileTcpServer()
        {
            QDB_SearTileTCPServer.StartTCPService(Constant.dbUtilityTcpPort);
            Console.WriteLine("QDB_SearTileTCPServer服务已启动，开始监听...");
        }

        private static void StartDbTcpServer()
        {
            QDB_SearDbTCPServer.StartTCPService(Constant.dbUtilityTcpPort);
            Console.WriteLine("QDB_SearSQLiteTCPServer服务已启动，开始监听...");
           
        }

        private static void StartAppTcpServer()
        {
            QDB_SearAPPTCPServer.StartTCPService(Constant.dbUtilityTcpPort);
            Console.WriteLine("QDB_SearAPPTCPServer服务已启动，开始监听...");
        }

        private static void StartGetDataTcpServer()
        {
            QDB_GetDataTCPServer.StartTCPService(Constant.dbUtilityTcpPort);
            Console.WriteLine("QDB_GetDataTCPServer服务已启动，开始监听...");
        }

        private static void UpdateServerPath()
        {
            #region 向MIDB库的appsetting表更新DBServer路径
            //string serverRootPath = Environment.CurrentDirectory;
            string appPath = Application.StartupPath.ToString();
            string sql =
                string.Format(
                    "update appsettings set value='{0}' where key='DBServerAbsolutePath'", appPath);
            if (Constant.QrstDbEngine == EnumDbEngine.MYSQL)
            {
                //避免MySQL数据库转义符丢失.SQLite库正常
                appPath = appPath.Replace("\\", "|");
                sql = string.Format("update appsettings set appsettings.value='{0}' where appsettings.key='DBServerAbsolutePath'", appPath);

            }
            baseUtilities.ExecuteSql(sql);

            #endregion
        }
        #endregion

        #region IPC服务

        #endregion

        #region 自动生成本机服务端口
        /// <summary>
        /// 获取一定范围内有效的指定书目端口
        /// </summary>
        /// <param name="portNum"></param>
        /// <param name="minNum"></param>
        /// <param name="maxNum"></param>
        /// <returns></returns>
        private static int GetValidPorts()
        {
            int validPort = 51112;
            try
            {


            //本机占用的端口列表
            ArrayList usedPorts = (ArrayList)GetUsedPort();

            int i = 4000;  //生成端口号初始值
            //循环有效性判断
            while (i < 55535)
            {
                //随机生成
                int port = GenerateValidPort(i, i + 50);
                if (IsUsedPort(port, usedPorts))
                {

                    i = i + 50;
                }
                else
                {
                    validPort = port;
                    break;
                }
            }
            }
            catch(Exception e)
            {
                return validPort;
            }
            return validPort;
        }

        /// <summary>
        /// 得到本机所有已占用的端口
        /// </summary>
        /// <returns></returns>
        private static IList GetUsedPort()
        {
            //获取本地计算机的网络连接和通信统计数据的信息            

            IPGlobalProperties ipGlobalProperties = IPGlobalProperties.GetIPGlobalProperties();

            //返回本地计算机上的所有Tcp监听程序            

            IPEndPoint[] ipsTCP = ipGlobalProperties.GetActiveTcpListeners();

            //返回本地计算机上的所有UDP监听程序            

            IPEndPoint[] ipsUDP = ipGlobalProperties.GetActiveUdpListeners();

            //返回本地计算机上的Internet协议版本4(IPV4 传输控制协议(TCP)连接的信息。            

            TcpConnectionInformation[] tcpConnInfoArray = ipGlobalProperties.GetActiveTcpConnections();

            IList allPorts = new ArrayList();

            foreach (IPEndPoint ep in ipsTCP)

            {

                allPorts.Add(ep.Port);

            }

            foreach (IPEndPoint ep in ipsUDP)

            {

                allPorts.Add(ep.Port);

            }

            foreach (TcpConnectionInformation conn in tcpConnInfoArray)

            {

                allPorts.Add(conn.LocalEndPoint.Port);

            }

            return allPorts;
        }

        /// <summary>
        /// 判断端口是否
        /// </summary>
        /// <param name="port"></param>
        /// <returns></returns>
        private static bool IsUsedPort(int port, IList usedPortList)
        {
            bool isUsed = false;
            if (usedPortList.Contains(port))
                isUsed = true;
            return isUsed;
        }

        /// <summary>
        /// 在端口范围内生成一个随机数
        /// </summary>
        /// <param name="usedPorts"></param>
        /// <param name="minNum"></param>
        /// <param name="maxNum"></param>
        /// <param name="validPort"></param>
        /// <param name="portNum"></param>
        /// <returns></returns>
        private static int GenerateValidPort(int minNum, int maxNum)
        {
            int rtn = 0;
            Random r = new Random();
            byte[] buffer = Guid.NewGuid().ToByteArray();
            int iSeed = BitConverter.ToInt32(buffer, 0);
            r = new Random(iSeed);
            rtn = r.Next(minNum, maxNum + 1);
            return rtn;
        }
        #endregion
    }
}
