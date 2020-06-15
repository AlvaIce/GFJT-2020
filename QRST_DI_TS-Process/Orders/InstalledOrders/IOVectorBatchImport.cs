using System;
using System.Collections.Generic;
using System.IO;
using System.Data.OleDb;
using System.Data;

namespace QRST_DI_TS_Process.Orders.InstalledOrders
{
    public class IOVectorBatchImport
    {
        private string searchfilepath;

        public List<OrderClass> CreateVectorBatchImport(string SourceFileDir, string excelFilePath, string groupcode)
        {
            List<OrderClass> sortedVectorBatchImportOrders = new List<OrderClass>();
            DataTable dt = GetExcelDataSet(excelFilePath, "Sheet1").Tables[0];
            sortedVectorBatchImportOrders.Add((new IOCopyAssistFiles()).createCopyAssistFiles(SourceFileDir, "%OrderWorkspace%", groupcode));
            string dataname = null;

            for (int index = 0; index < dt.Rows.Count; index++)
            {
                dataname = dt.Rows[index]["数据名称"] + ".shp";
                if (!dataname.Equals(".shp"))
                {
                    GetShpFile(SourceFileDir, dataname);
                    OrderClass order = (new IOSortedVectorImport()).createSortedVectorDataImport(excelFilePath, searchfilepath, "%OrderWorkspace%", groupcode, index.ToString());
                    sortedVectorBatchImportOrders.Add((new IOSortedVectorImport()).createSortedVectorDataImport(excelFilePath, searchfilepath, "%OrderWorkspace%", groupcode, index.ToString()));
                }
            }
            return sortedVectorBatchImportOrders;
        }

        /// <summary>
        /// 递归查找文件夹下符合条件的shp文件
        /// </summary>
        /// <param name="SourceFileDir"></param>
        /// <param name="filename"></param>
        private void GetShpFile(string SourceFileDir, string filename)
        {
            if (Directory.GetFiles(SourceFileDir, filename).Length == 0)
            {
                string[] dir = Directory.GetDirectories(SourceFileDir);
                foreach (string file in dir)
                {
                    GetShpFile(file, filename);
                }
            }
            else
            {
                searchfilepath = Directory.GetFiles(SourceFileDir, filename)[0];
            }
        }


        /// <summary>
        /// 根据excel获取dataset
        /// </summary>
        /// <param name="path">excel路径</param>
        /// <param name="tname">数据薄名称</param>
        /// <returns>dataset</returns>
        private DataSet GetExcelDataSet(string path, string tname)
        {
            /*Office 2007*/
            string ace = "Microsoft.ACE.OLEDB.12.0";
            /*Office 97 - 2003*/
            string jet = "Microsoft.Jet.OLEDB.4.0";
            string xl2007 = "Excel 12.0 Xml";
            string xl2003 = "Excel 8.0";
            string imex = "IMEX=1";
            string hdr = "YES";
            string conn = "Provider={0};Data Source={1};Extended Properties=\"{2};HDR={3};{4}\";";
            string select = string.Format("SELECT * FROM [{0}$]", tname);

            //string select = sql;
            string ext = Path.GetExtension(path);
            OleDbDataAdapter oda;
            DataSet ds = new DataSet();
            switch (ext.ToLower())
            {
                case ".xlsx":
                    conn = String.Format(conn, ace, Path.GetFullPath(path), xl2007, hdr, imex);
                    break;
                case ".xls":
                    conn = String.Format(conn, jet, Path.GetFullPath(path), xl2003, hdr, imex);
                    break;
                default:
                    throw new Exception("File Not Supported!");
            }
            OleDbConnection con = new OleDbConnection(conn);
            con.Open();
            //select = string.Format(select, sql);
            oda = new OleDbDataAdapter(select, con);
            oda.Fill(ds, "test");
            con.Close();

            return ds;
        }

    }
}
