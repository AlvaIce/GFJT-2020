using System;
using System.Collections;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;
using System.Runtime.Serialization.Formatters;
using QRST_DI_SS_DBInterfaces.IDBEngine;

namespace QRST_DI_SS_DBClient.DBEngine
{
   public class DbOperatingTCPClient
    {
        public static IDbOperating InitTCPClient_StorageChl(string storageSiteIP, string tcpStoragePort)
        {
            if (ChannelServices.GetChannel("DbOperating_TCP") == null)
            {
                BinaryClientFormatterSinkProvider clientProvider = new BinaryClientFormatterSinkProvider();
                IDictionary props = new Hashtable();
                props["port"] = tcpStoragePort;
                props["name"] = "DbOperating_TCP";
                props["typeFilterLevel"] = TypeFilterLevel.Full;
                TcpClientChannel chan = new TcpClientChannel(
                props, clientProvider);
                ChannelServices.RegisterChannel(chan);
            }
            Type typeofRI = typeof(IDbOperating);
            return (IDbOperating)Activator.GetObject(typeofRI,
                            string.Format("tcp://{0}:{1}/QDB_DbOperating_TCP", storageSiteIP, tcpStoragePort));

        }
    }
}
