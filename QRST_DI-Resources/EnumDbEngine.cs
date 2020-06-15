using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QRST_DI_Resources
{
    public enum EnumDbEngine
    {
        SQLITE=0,
        MYSQL=1,
        ClOUDDB=2,
        NULL=3
         
    }

    public enum EnumDbStorage
    {
        //单机
        SINGLE=0,

        //多机
        MULTIPLE=1,

        //集群
        CLUSTER=2,

        NULL=3
    }
}
