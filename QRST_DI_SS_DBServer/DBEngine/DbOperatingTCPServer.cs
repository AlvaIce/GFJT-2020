using System;
using System.Collections;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;
using System.Runtime.Serialization.Formatters;
using QRST_DI_DS_DBEngine;
using QRST_DI_Resources;
using QRST_DI_SS_Basis;
using QRST_DI_SS_Basis.MetaData;
using QRST_DI_SS_DBInterfaces.IDBEngine;

namespace QRST_DI_SS_DBServer.DBEngine
{
    class DbOperatingTCPServer
    {
        private static TcpServerChannel _chan = null;
        private DBSQLiteOperating _sqLiteOperating = null;
        

        /// <summary>
        /// 开启TCP服务
        /// </summary>
        public void StartTCPService(string tcpPort)
        {
            try
            {
                BinaryServerFormatterSinkProvider serverProvider = new BinaryServerFormatterSinkProvider();
                serverProvider.TypeFilterLevel = TypeFilterLevel.Full;
                IDictionary props = new Hashtable();
                //props["port"] = tcpPort;
                props["name"] = "DbOperating_TCP";
                props["typeFilterLevel"] = TypeFilterLevel.Full;
                _chan = new TcpServerChannel(
                    props, serverProvider);
                ChannelServices.RegisterChannel(_chan);
                switch (Constant.QrstDbEngine)
                {
                    case EnumDbEngine.MYSQL:
                        RemotingConfiguration.RegisterWellKnownServiceType(typeof(DBMySqlOperating),
        "QDB_DbOperating_TCP",
        WellKnownObjectMode.Singleton);
                        break;
                    case EnumDbEngine.SQLITE:
                        RemotingConfiguration.RegisterWellKnownServiceType(typeof(DBSQLiteOperating),
        "QDB_DbOperating_TCP",
        WellKnownObjectMode.Singleton);
                        break;
                    case EnumDbEngine.ClOUDDB:
                        break;
                }
            }
            catch (Exception e)
            {
                throw new Exception("注册DbOperatingTCPServer异常", e);
            }
        }

    }
}
