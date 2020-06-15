using System;
using System.Collections;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;
using System.Runtime.Serialization.Formatters;
using QRST_DI_SS_DBInterfaces.IDBService;

namespace QRST_DI_SS_DBClient.TCP
{
    public class QDB_SearTileTCPClient
    {
        public static IQDB_Searcher_Tile InitTCPClient_StorageChl(string storageSiteIP, string tcpStoragePort)
        {
            if (ChannelServices.GetChannel("SearTile_TCP") == null)
            {
                BinaryClientFormatterSinkProvider clientProvider = new BinaryClientFormatterSinkProvider();
                IDictionary props = new Hashtable();
                props["port"] = tcpStoragePort;
                props["name"] = "SearTile_TCP";
                props["typeFilterLevel"] = TypeFilterLevel.Full;
                TcpClientChannel chan = new TcpClientChannel(
                props, clientProvider);
                ChannelServices.RegisterChannel(chan);
            }
            Type typeofRI = typeof(IQDB_Searcher_Tile);
            return (IQDB_Searcher_Tile)Activator.GetObject(typeofRI,
                            string.Format("tcp://{0}:{1}/QDB_SearTile_TCP", storageSiteIP, tcpStoragePort));

        }
    }
}
