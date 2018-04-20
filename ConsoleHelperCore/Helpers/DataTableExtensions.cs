using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;

namespace ConsoleHelperCore.Helpers
{
    public static class DataTableExtensions
    {
        public static void WriteToCsvFile(this DataTable dt, FileInfo file)
        {
            var sb = new StringBuilder();
            var columnNames = dt.Columns.Cast<DataColumn>().Select(column => column.ColumnName);
            sb.AppendLine(string.Join(",", columnNames));

            foreach (DataRow row in dt.Rows)
            {
                var fields = row.ItemArray.Select(field => field.ToString());
                sb.AppendLine(string.Join(",", fields));
            }

            File.WriteAllText(file.FullName, sb.ToString());
        }

        public static void WriteToExcelFile(this DataTable dt, FileInfo file)
        {
            var csvFile = new FileInfo($"{Path.Combine(file.FullName).Replace(file.Extension, "")}.csv");
            dt.WriteToCsvFile(csvFile);

            var app = new Microsoft.Office.Interop.Excel.Application();
            var wb = app.Workbooks.Open(csvFile.FullName, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);
            wb.SaveAs(file.FullName, Microsoft.Office.Interop.Excel.XlFileFormat.xlOpenXMLWorkbook, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Microsoft.Office.Interop.Excel.XlSaveAsAccessMode.xlExclusive, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);
            wb.Close();
            app.Quit();

            File.Delete(csvFile.FullName);
        }
    }
}
