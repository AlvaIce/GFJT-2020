using System;
using System.Data;
using Microsoft.Office.Interop.Excel;
using System.Reflection;

namespace QRST_DI_MS_Basis.ExcelOperation
{
    public class ExcelOperation
    {
        /// <summary>
        /// 将表中数据导出到Excel中
        /// </summary>
        /// <param name="ds"></param>
        /// <param name="filePath"></param>
        public static void ExcelExport(DataSet ds, String path)
        {
            if (ds == null || ds.Tables.Count == 0)
                return;

            Microsoft.Office.Interop.Excel.Application xlsApp = new Microsoft.Office.Interop.Excel.Application();
            if (xlsApp == null)
            {
                throw new Exception("Excel应用创建失败！请检查是否安装了Excel03或以上版本！");
            }
            xlsApp.Visible = true;
            // System.Globalization.CultureInfo CurrentCI = System.Threading.Thread.CurrentThread.CurrentCulture;
            //System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");

            Workbooks workbooks = xlsApp.Workbooks;
            Workbook workbook = workbooks.Add(Microsoft.Office.Interop.Excel.XlWBATemplate.xlWBATWorksheet);
            Microsoft.Office.Interop.Excel.Worksheet worksheet = (Microsoft.Office.Interop.Excel.Worksheet)(workbook.Worksheets[1]);

            Microsoft.Office.Interop.Excel.Range range;

            //为每一列添加标题
            for (int i = 1 ; i <= ds.Tables[0].Columns.Count ; i++)
            {
                worksheet.Cells[1, i] = ds.Tables[0].Columns[i - 1].ColumnName;
                range = (Microsoft.Office.Interop.Excel.Range)worksheet.Cells[1, i + 1];
                range.Interior.ColorIndex = 15;
                range.Font.Bold = true;
            }
            //将表中数据复制到Excel中
            for (int r = 1 ; r <= ds.Tables[0].Rows.Count ; r++)
            {
                for (int col = 1 ; col <= ds.Tables[0].Columns.Count ; col++)
                {
                    worksheet.Cells[r + 1, col] = ds.Tables[0].Rows[r - 1][col - 1].ToString();
                }
            }
            xlsApp.Visible = true;

            workbook.SaveAs(path, Missing.Value, null, null, false, false, XlSaveAsAccessMode.xlNoChange, null, null, null, null, null);

            workbook.Close();
            workbook = null;
            xlsApp.Quit();
            xlsApp = null;
        }

        /// <summary>
        /// 将表中数据导出到Excel中
        /// </summary>
        /// <param name="ds"></param>
        /// <param name="filePath"></param>
        public static void ExcelExport(System.Data.DataTable dt, String path)
        {
            if (dt == null)
                return;

            Microsoft.Office.Interop.Excel.Application xlsApp = new Microsoft.Office.Interop.Excel.Application();
            if (xlsApp == null)
            {
                throw new Exception("Excel应用创建失败！请检查是否安装了Excel03或以上版本！");
            }
            xlsApp.Visible = true;
            // System.Globalization.CultureInfo CurrentCI = System.Threading.Thread.CurrentThread.CurrentCulture;
            //System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");

            Workbooks workbooks = xlsApp.Workbooks;
            Workbook workbook = workbooks.Add(Microsoft.Office.Interop.Excel.XlWBATemplate.xlWBATWorksheet);
            Microsoft.Office.Interop.Excel.Worksheet worksheet = (Microsoft.Office.Interop.Excel.Worksheet)(workbook.Worksheets[1]);

            Microsoft.Office.Interop.Excel.Range range;

            //为每一列添加标题
            for (int i = 1 ; i <= dt.Columns.Count ; i++)
            {
                worksheet.Cells[1, i] = dt.Columns[i - 1].ColumnName;
                range = (Microsoft.Office.Interop.Excel.Range)worksheet.Cells[1, i + 1];
                range.Interior.ColorIndex = 15;
                range.Font.Bold = true;
                
            }
            //将表中数据复制到Excel中
            for (int r = 1 ; r <= dt.Rows.Count ; r++)
            {
                for (int col = 1 ; col <= dt.Columns.Count ; col++)
                {
                    worksheet.Cells[r + 1, col] = dt.Rows[r - 1][col - 1].ToString();
                }
            }
            xlsApp.Visible = true;

            workbook.SaveAs(path, Missing.Value, null, null, false, false, XlSaveAsAccessMode.xlNoChange, null, null, null, null, null);

            workbook.Close();
            workbook = null;
            xlsApp.Quit();
            xlsApp = null;
        }
    }
}
