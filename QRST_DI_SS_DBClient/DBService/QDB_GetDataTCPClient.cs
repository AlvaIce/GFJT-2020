using System;
using System.Collections;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;
using System.Runtime.Serialization.Formatters;
using QRST_DI_SS_DBInterfaces.IDBService;

namespace QRST_DI_SS_DBClient.TCP
{
   public class QDB_GetDataTCPClient
    {
        public static IQDB_GetData InitTCPClient_StorageChl(string storageSiteIP, string tcpStoragePort)
        {
            if (ChannelServices.GetChannel("GetData_TCP") == null)
            {
                BinaryClientFormatterSinkProvider clientProvider = new BinaryClientFormatterSinkProvider();
                IDictionary props = new Hashtable();
                props["port"] = tcpStoragePort;
                props["name"] = "GetData_TCP";
                props["typeFilterLevel"] = TypeFilterLevel.Full;
                TcpClientChannel chan = new TcpClientChannel(
                props, clientProvider);
                ChannelServices.RegisterChannel(chan);
            }
            Type typeofRI = typeof(IQDB_GetData);
            return (IQDB_GetData)Activator.GetObject(typeofRI,
                            string.Format("tcp://{0}:{1}/QDB_GetData_TCP", storageSiteIP, tcpStoragePort));

        }
    }
}
