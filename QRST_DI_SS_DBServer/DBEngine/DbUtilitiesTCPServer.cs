using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;
using System.Runtime.Serialization.Formatters;
using QRST_DI_SS_DBInterfaces.IDBEngine;
using QRST_DI_DS_DBEngine;
using QRST_DI_Resources;

namespace QRST_DI_SS_DBServer.DBEngine
{
    class DbUtilitiesTCPServer 
    {
        private static TcpServerChannel _chan = null;
        private SQLiteBaseUtilities _sqLiteBaseUtilities = null;

        

        /// <summary>
        /// 开启TCP服务
        /// </summary>
        public static void StartTCPService(string tcpPort)
        {
            try
            {
                BinaryServerFormatterSinkProvider serverProvider = new BinaryServerFormatterSinkProvider();
                serverProvider.TypeFilterLevel = TypeFilterLevel.Full;

                IDictionary props = new Hashtable();
                props["port"] = tcpPort;
                props["name"] = "DbUtilities_TCP";
                props["typeFilterLevel"] = TypeFilterLevel.Full;
                _chan = new TcpServerChannel(
                    props, serverProvider);
                ChannelServices.RegisterChannel(_chan);
                //根据不同数据库引擎配置调用对应数据库远程对象
                switch (Constant.QrstDbEngine)
                {
                    case EnumDbEngine.ClOUDDB:
                        break;
                    case EnumDbEngine.MYSQL:
                        RemotingConfiguration.RegisterWellKnownServiceType(typeof(MySqlBaseUtilities),
    "QDB_DbUtilities_TCP",
    WellKnownObjectMode.Singleton);
                        break;
                    case EnumDbEngine.SQLITE:
                        RemotingConfiguration.RegisterWellKnownServiceType(typeof(SQLiteBaseUtilities),
        "QDB_DbUtilities_TCP",
        WellKnownObjectMode.Singleton);
                        break;
                }
            }
            catch (Exception e)
            {
                throw new Exception("注册DbUtilitiesTCPServer异常", e);
            }
        }

      
    }
}
