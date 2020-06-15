using System;
using System.Threading;
using System.Net.Sockets;
using System.Net;
using System.IO;
using QRST_DI_Resources;

 
namespace QRST_DI_MS_Basis
{
    public delegate void RecievedMessage2Handle(backupMessage msg);
    public class NoticeMessagerForFileBackup
    {
        public static bool SystemExited = false;
        public event RecievedMessage2Handle ReciewedMessage2;
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
        backupMessage msg = new backupMessage();
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
        public NoticeMessagerForFileBackup()
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
                    if (SystemExited)
                    {
                        break;
                    }
                    socket = tcpListener.AcceptSocket();
                    NetworkStream stream = new NetworkStream(socket);
                    StreamReader sr = new StreamReader(stream);

                    msg.ipAddress = sr.ReadLine();
                    msg.orderName = sr.ReadLine();
                    msg.originalFile = sr.ReadLine();
                    msg.targetFile = sr.ReadLine();
                    msg.backupSize = sr.ReadLine();
                    msg.allSize = sr.ReadLine();
                    msg.usedTime = sr.ReadLine();
                    msg.remainingTime = sr.ReadLine();
                    msg.averageSpeed = sr.ReadLine();
                    msg.backupStatus = sr.ReadLine();
                    msg.backupType = sr.ReadLine();
                    //string ip = sr.ReadLine();
                    //string receiveMessage = sr.ReadLine();                    
                    //string[] content = receiveMessage.Split(",".ToCharArray());
                    //SenderIP = content[0];
                    //MessageString = content[1];

                    if (sr.ReadToEnd() != null)
                    {
                        ReciewedMessage2(msg);
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

        public void SendMessage(string remoteIpString, Entry entry1)
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
                //请勿改变下面的先后顺序
                streamWriter.WriteLine(entry1.OrderName);
                streamWriter.WriteLine(entry1.OriginalFolder);
                streamWriter.WriteLine(entry1.BackupType);
                streamWriter.WriteLine(entry1.TargetFolder);
                streamWriter.WriteLine(entry1.isNeedWinrar.ToString());
                streamWriter.WriteLine(entry1.BackupOrNot.ToString());
                streamWriter.WriteLine(entry1.BackupString);
                //streamWriter.WriteLine(string.Format("{0},{1}", myIPAddress, str));
                streamWriter.Flush();
            }
            catch (Exception ex)
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

    /// <summary>
    /// 状态类，用于记录备份机器上返回的文件备份状态
    /// </summary>
    public class backupMessage
    {
        public string ipAddress { set; get; }
        public string orderName { set; get; }
        public string originalFile { set; get; }
        public string targetFile { set; get; }
        public string backupSize { set; get; }
        public string allSize { set; get; }
        public string usedTime { set; get; }
        public string remainingTime { set; get; }
        public string averageSpeed { set; get; }
        public string backupType { set; get; }
        public string backupStatus { set; get; }
    }

    /// <summary>
    /// 订单类，用于记录一个文件备份订单
    /// </summary>
    public class Entry
    {
        private string strOrderName;
        public string OrderName
        {
            set { strOrderName = value; }
            get { return strOrderName; }
        }

        private string strOriginalFolder;
        public string OriginalFolder
        {
            get { return strOriginalFolder; }
            set { strOriginalFolder = value; }
        }

        private string strBackupType;
        public string BackupType
        {
            get { return strBackupType; }
            set { strBackupType = value; }
        }

        private string strTargetFolder;
        public string TargetFolder
        {
            get { return strTargetFolder; }
            set { strTargetFolder = value; }
        }

        private bool boolisNeedWinrar;
        public bool isNeedWinrar
        {
            get { return boolisNeedWinrar; }
            set { boolisNeedWinrar = value; }
        }

        private bool boolBackupOrNot;//此字段用于记录是选择备份某些格式的文件还是不备份某些格式的文件，需要和下面的stringBackup配合使用
        public bool BackupOrNot
        {
            get { return boolBackupOrNot; }
            set { boolBackupOrNot = value; }
        }

        private string stringBackupString;
        public string BackupString
        {
            get { return stringBackupString; }
            set { stringBackupString = value; }
        }
        public Entry(string name, string backuptype, string originalFolder, string targetFolder, bool isneedwinrar, bool backupornot, string backupstring)
        {
            strOrderName = name;
            strBackupType = backuptype;
            strOriginalFolder = originalFolder;
            strTargetFolder = targetFolder;
            boolBackupOrNot = backupornot;
            boolisNeedWinrar = isneedwinrar;
            BackupString = backupstring;
        }
    }
}
