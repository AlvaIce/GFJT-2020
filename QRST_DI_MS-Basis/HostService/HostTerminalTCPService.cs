using System;

namespace QRST_DI_MS_Basis.HostService
{
    public class HostTerminalTCPService:MarshalByRefObject
    {

        public HostTerminalTCPService()
        { }

        /// <summary>
        /// 判断是否开机
        /// </summary>
        /// <returns></returns>
        public bool IsRunning
        {
            get { return true; }
        }

    }
}
