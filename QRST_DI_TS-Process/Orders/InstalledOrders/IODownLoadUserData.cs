using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using QRST_DI_TS_Process.Tasks.InstalledTasks;

namespace QRST_DI_TS_Process.Orders.InstalledOrders
{ 
    public class IODownLoadUserData
    {
        /// <summary>
        /// 用于下载用户数据
        /// </summary>
        /// <returns></returns>
        public OrderClass CreateDownLoadUserData(string dataID, string pathID, string opID)
        {
            return CreateDownLoadUserData(dataID, pathID, opID, "", "");
        }
        /// <summary>
        /// 用于下载用户数据
        /// </summary>
        /// <returns></returns>
        public OrderClass CreateDownLoadUserData(string dataID, string pathID, string opID, string gfdataName, string webservice)
        {
            OrderClass orderClass = new OrderClass();
            orderClass.Type = EnumOrderType.Installed;
            orderClass.OrderCode = OrderManager.GetNewCode();
            //订单赋参
            orderClass.OrderParams = new string[] { dataID, pathID, opID, gfdataName, webservice };

            //获取元数据文件
            orderClass.Tasks.Add(new ITDownLoadData());
            orderClass.TaskParams.Add(new string[] { dataID, pathID, opID, gfdataName, webservice });



            return orderClass;
        }
    }
}
