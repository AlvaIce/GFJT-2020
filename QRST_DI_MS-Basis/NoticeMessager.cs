using System;
using System.Threading;
using System.Net.Sockets;
using System.Net;
using System.IO;
using QRST_DI_Resources;

 
namespace QRST_DI_MS_Basis
{
    public delegate void RecievedMessageHandle(string sender,string msg);
    public class NoticeMessager
    {
        public event RecievedMessageHandle ReciewedMessage;
        private static string defaultListenPort = "51111";
        private static string defaultSendPort = "52222";
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
        public NoticeMessager()
        {
            string _lPort = Constant.NoticeListenPort;
            string _sPort = Constant.NoticeSendPort;
            listenPort = (_lPort != "") ? _lPort : defaultListenPort;
            sendPort = (_sPort != "") ? _sPort : defaultSendPort;
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
            myIPAddress = IPAddress.Parse(Constant.UsingIPAddress);
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

        public void SendMessage(string remoteIpString, string str)
        {
            IPAddress remoteIp =  IPAddress.Parse(remoteIpString);

            int remotePort = int.Parse(sendPort);
            NetworkStream networkstream = null;
            TcpClient tcpclient = null;
            try
            {
                tcpclient = new TcpClient(remoteIpString, remotePort);
                networkstream = tcpclient.GetStream();
                StreamWriter streamWriter = new StreamWriter(networkstream);
                streamWriter.WriteLine(string.Format("{0},{1}", myIPAddress, str));
                streamWriter.Flush();
            }
            catch(Exception ex)
            {

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
