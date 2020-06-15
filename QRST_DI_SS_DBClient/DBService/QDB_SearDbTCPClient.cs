using System;
using System.Collections;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;
using System.Runtime.Serialization.Formatters;
using QRST_DI_SS_DBInterfaces.IDBService;

namespace QRST_DI_SS_DBClient.DBService
{
    public class QDB_SearDbTCPClient
    {
        public static IQDB_Searcher_Db InitTCPClient_StorageChl(string storageSiteIP, string tcpStoragePort)
        {
            if (ChannelServices.GetChannel("SearSQLite_TCP") == null)
            {
                BinaryClientFormatterSinkProvider clientProvider = new BinaryClientFormatterSinkProvider();
                IDictionary props = new Hashtable();
                props["port"] = tcpStoragePort;
                props["name"] = "SearSQLite_TCP";
                props["typeFilterLevel"] = TypeFilterLevel.Full;
                TcpClientChannel chan = new TcpClientChannel(
                props, clientProvider);
                ChannelServices.RegisterChannel(chan);
            }
            Type typeofRI = typeof(IQDB_Searcher_Db);
            return (IQDB_Searcher_Db)Activator.GetObject(typeofRI,
                            string.Format("tcp://{0}:{1}/QDB_SearSQLite_TCP", storageSiteIP, tcpStoragePort));

        }
    }
}
