using System;
using System.Collections;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;
using System.Runtime.Serialization.Formatters;
using QRST_DI_SS_DBInterfaces.IDBService;

namespace QRST_DI_SS_DBServer.DBService
{
    class QDB_SearAPPTCPServer:MarshalByRefObject,IQDB_Searcher_APP
    {
        private static TcpServerChannel _chan = null;
        /// <summary>
        /// 开启TCP服务
        /// </summary>
        public  static void StartTCPService(string tcpPort)
        {
            try
            {
                BinaryServerFormatterSinkProvider serverProvider = new BinaryServerFormatterSinkProvider();
                serverProvider.TypeFilterLevel = TypeFilterLevel.Full;
                IDictionary props = new Hashtable();
                //props["port"] = tcpPort;
                props["name"] = "SearApp_TCP";
                props["typeFilterLevel"] = TypeFilterLevel.Full;
                _chan = new TcpServerChannel(
                    props, serverProvider);
                ChannelServices.RegisterChannel(_chan);

                RemotingConfiguration.RegisterWellKnownServiceType(typeof(QDB_SearAPPTCPServer),
                    "QDB_SearApp_TCP",
                    WellKnownObjectMode.SingleCall);
            }
            catch (Exception e)
            {
                throw new Exception("注册QDB_SearAPPTCPServer异常", e);
            }
        }
    }
}
