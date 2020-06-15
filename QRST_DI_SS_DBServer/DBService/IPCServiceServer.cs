using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Ipc;
using QRST_DI_SS_DBInterfaces.IDBService;

namespace QRST_DI_SS_DBServer.DBService
{
    public class IPCServiceServer : MarshalByRefObject, IIPCService
    {
        public static void StartIPCService(string portName)
        {
            IDictionary ht = new Dictionary<string, string>();
            ht["portName"] = portName;
            ht["name"] = "Console-WebService IPC";
            ht["authorizedGroup"] = "Users";

            IpcServerChannel serverChannel = new IpcServerChannel(ht, null, null);
            ChannelServices.RegisterChannel(serverChannel, false);

            // Expose an object
            RemotingConfiguration.RegisterWellKnownServiceType(typeof (IPCServiceServer), "HJDB_Console_IPC_Service",
                WellKnownObjectMode.Singleton);
        }
    }
}
