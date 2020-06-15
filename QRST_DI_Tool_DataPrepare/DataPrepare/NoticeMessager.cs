using System;
using System.Net.Sockets;
using System.Threading;
using System.IO;
using System.Net;
using System.Management;

namespace DataPrepare
{
    /// <summary>
    /// 接收消息和发送消息类，此类拷贝自运管系统，用于接收高分入库Order发送到本程序刚入库的影像信息
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="msg"></param>
    /// <returns></returns>
    public delegate string RecievedMessageHandle(string sender, string msg);
    public class NoticeMessager
    {
        public event RecievedMessageHandle ReciewedMessage;
        private static string defaultListenPort = "52222";
        private static string defaultSendPort = "51111";
        private string listenPort = defaultListenPort;
        private string sendPort = defaultSendPort;
        private bool listenLocalAdress = false;
        private readonly Thread myThread;
        private TcpListener tcpListener;
        private IPAddress myIPAddress;
        private readonly int myPort;

        //private readonly System.Diagnostics.Stopwatch secondWatch;
        public string MessageString
        {
            get;
            internal set;
        }
        public string SenderIP
        {
            get;
            internal set;
        }
        public NoticeMessager(bool isSender)
        {
            listenPort = (isSender) ? defaultSendPort : defaultListenPort;
            sendPort = (isSender) ? defaultListenPort : defaultSendPort;
            MessageString = "";
            SenderIP = "";

            ThreadStart myThreaStartDelegate = new ThreadStart(Listening);
            myThread = new Thread(myThreaStartDelegate);
            myThread.Start();
        }


        private void Listening()
        {
            Socket socket = null;
            #region//获取本机IP，对于多IP地址无法自动判断选取
            //IPAddress[] addrIP = Dns.GetHostAddresses(string.Empty);
            //foreach (IPAddress ip in addrIP)
            //{
            //    if (ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
            //    {
            //        //获取IPv4地址
            //        myIPAddress = ip;
            //        break;
            //    }
            //}
            #endregion
            myIPAddress = IPAddress.Parse(getUsingIPAddress());
            while (true)
            {
                try
                {
                    tcpListener = new TcpListener(myIPAddress, int.Parse(listenPort));
                    tcpListener.Start();

                    break;
                }
                catch
                {

                }
            }
            while (true)
            {
                try
                {
                    socket = tcpListener.AcceptSocket();
                    NetworkStream stream = new NetworkStream(socket);
                    StreamReader sr = new StreamReader(stream);
                    string receiveMessage = sr.ReadLine();
                    string[] content = receiveMessage.Split(",".ToCharArray());
                    SenderIP = content[0];
                    MessageString = content[1];

                    if (ReciewedMessage != null)
                    {
                        ReciewedMessage(SenderIP, MessageString);
                    }
                }
                catch
                {
                    if (socket != null)
                    {
                        if (socket != null)
                        {
                            if (socket.Connected)
                            {
                                socket.Shutdown(SocketShutdown.Receive);
                            }
                            socket.Close();
                        }
                    }
                    myThread.Abort();
                }
            }
        }

        private string getUsingIPAddress()
        {
            try
            {
                string st = "";
                ManagementClass mc = new ManagementClass("Win32_NetworkAdapterConfiguration");
                ManagementObjectCollection moc = mc.GetInstances();
                foreach (ManagementObject mo in moc)
                {
                    if ((bool)mo["IPEnabled"] == true)
                    {
                        //st=mo["IpAddress"].ToString(); 
                        System.Array ar;
                        ar = (System.Array)(mo.Properties["IpAddress"].Value);
                        st = ar.GetValue(0).ToString();
                        break;
                    }
                }
                moc = null;
                mc = null;
                if (st == "")
                    st = "127.0.0.1";
                return st;
            }
            catch
            {
                return "unknow";
            }
            finally
            {
            }
        }

        public void SendMessage(string remoteIpString, string str)
        {
            IPAddress remoteIp = IPAddress.Parse(remoteIpString);

            int remotePort = int.Parse(sendPort);
            NetworkStream networkstream = null;
            TcpClient tcpclient = null;
            try
            {
                tcpclient = new TcpClient(remoteIpString, remotePort);
                networkstream = tcpclient.GetStream();
                StreamWriter streamWriter = new StreamWriter(networkstream);
                streamWriter.WriteLine(string.Format("{0},{1}", myIPAddress, str));
                streamWriter.WriteLine(string.Format("{0}", "这是一个小孩子"));
                streamWriter.Flush();
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
                if (networkstream != null)
                {
                    networkstream.Close();
                }
                if (tcpclient != null)
                {
                    tcpclient.Close();
                }
            }
        }
        public string GetMessage()
        {
            return MessageString;
        }
    }

}
