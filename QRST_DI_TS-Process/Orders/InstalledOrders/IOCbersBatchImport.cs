using System.Collections.Generic;
using System.IO;

namespace QRST_DI_TS_Process.Orders.InstalledOrders
{
    public class IOCbersBatchImport
    {
        public List<OrderClass> CreateCbersBatchImport(string SourceFileDir)
        {
            List<OrderClass> CbersBatchImportOrders = new List<OrderClass>();
            string[] fileArr = Directory.GetFiles(SourceFileDir);
            foreach (string file in fileArr)
            {
                string ext = file.ToLower();
                if(ext.EndsWith(".tar.gz"))
                    CbersBatchImportOrders.Add((new IOCbersDataImport()).CreateCbersImportOrder(file, "%OrderWorkspace%"));
            }
            return CbersBatchImportOrders;
        }
    }
}
