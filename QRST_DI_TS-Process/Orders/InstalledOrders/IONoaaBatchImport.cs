using System.Collections.Generic;
using System.IO;

namespace QRST_DI_TS_Process.Orders.InstalledOrders
{
    public class IONoaaBatchImport
    {

        public List<OrderClass> CreateNoaaBatchImport(string SourceFileDir)
        {
            List<OrderClass> NoaaBatchImportList = new List<OrderClass>();
            string []files = Directory.GetFiles(SourceFileDir);
            foreach(string SourceFilePath in files)    
            {
                string str = Path.GetExtension(SourceFilePath).ToUpper();
                if (str == ".WI" || str == ".GC")
                {
                    NoaaBatchImportList.Add((new IONoaaDataImport()).CreateNoaaImportOrder(SourceFilePath, "%OrderWorkspace%"));
                }
            }
            return NoaaBatchImportList;
        }

    }
}
