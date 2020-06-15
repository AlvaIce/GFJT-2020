using System;
using System.Collections;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;
using System.Runtime.Serialization.Formatters;
using QRST_DI_SS_DBInterfaces.IDBService;

namespace QRST_DI_SS_DBClient.TCP
{
    class QDB_SearAPPTCPClient
    {
        public static IQDB_Searcher_APP InitTCPClient_StorageChl(string storageSiteIP, string tcpStoragePort)
        {
            if (ChannelServices.GetChannel("SearApp_TCP") == null)
            {
                BinaryClientFormatterSinkProvider clientProvider = new BinaryClientFormatterSinkProvider();
                IDictionary props = new Hashtable();
                props["port"] = tcpStoragePort;
                props["name"] = "SearApp_TCP";
                props["typeFilterLevel"] = TypeFilterLevel.Full;
                TcpClientChannel chan = new TcpClientChannel(
                props, clientProvider);
                ChannelServices.RegisterChannel(chan);
            }
            Type typeofRI = typeof(IQDB_Searcher_APP);
            return (IQDB_Searcher_APP)Activator.GetObject(typeofRI,
                            string.Format("tcp://{0}:{1}/QDB_SearApp_TCP", storageSiteIP, tcpStoragePort));

        }
    }
}
