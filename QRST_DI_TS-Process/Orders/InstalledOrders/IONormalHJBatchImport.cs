using System;
using System.Collections.Generic;
using System.IO;

namespace QRST_DI_TS_Process.Orders.InstalledOrders
{
    public class IONormalHJBatchImport
    {
        public List<OrderClass> CreateNormalHJBatchImport(string SourceFileDir)
        {
            List<OrderClass> NormalHJBatchImportOrders = new List<OrderClass>();
            foreach(string file in Directory.GetFiles(SourceFileDir,"*.tar.gz") )
            {
                try
                {
                    NormalHJBatchImportOrders.Add((new IONormalHJDataImport()).CreateNormalHJdataImportOrder(file, "%OrderWorkspace%"));
                }
                catch(Exception)
                {
                }
            }
            return NormalHJBatchImportOrders;
        }
    }
}
