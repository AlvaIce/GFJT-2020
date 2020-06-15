using System.Collections.Generic;
using System.IO;

namespace QRST_DI_TS_Process.Orders.InstalledOrders
{
    public class IOAlgCmpBatchImport
    {
        public List<OrderClass> CreateAlgCmpBatchImport(string SourceFileDir)
        {
            List<OrderClass> cmpmportOrders = new List<OrderClass>();
            string[] fileArr = Directory.GetFiles(SourceFileDir);
            foreach (string file in fileArr)
            {
                string ext = file.ToLower();
                if (ext.EndsWith(".zip"))
                    cmpmportOrders.Add((new IOImportZipCmp()).CreateZipCmpImportOrder(file));
            }
            return cmpmportOrders;
        }
    }
}
