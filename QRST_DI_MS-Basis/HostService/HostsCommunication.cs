using System;

namespace QRST_DI_MS_Basis.HostService
{
    public class HostsCommunication
    {
        public HostsCommunication(DataSourceHost dshostOrigin, DataSourceHost dshostDestination, String msgCom)
        {
            OriginHost = dshostOrigin;
            DestinationHost = dshostDestination;
            CommunicationMessage = msgCom;
            Datetime = DateTime.Now;
        }

        public HostsCommunication(DataSourceHost dshostOrigin, DataSourceHost dshostDestination, String msgCom, DateTime dt)
        {
            OriginHost = dshostOrigin;
            DestinationHost = dshostDestination;
            CommunicationMessage = msgCom;
            Datetime = dt;
        }


        public string OriginHostNameIP
        {
            get {
                if (OriginHost==null)
                {
                    return "";
                }
                return string.Format("{0}({1})", OriginHost.Name, OriginHost.IP);
            }
        }

        public string DestinationHostNameIP
        {
            get
            {
                if (OriginHost == null)
                {
                    return "";
                }
                return string.Format("{0}({1})", DestinationHost.Name, DestinationHost.IP);
            }
        }

        public DataSourceHost OriginHost;

        public DataSourceHost DestinationHost;

        public string CommunicationMessage { get; set; }
        public DateTime Datetime { get; set; }

    }
}
