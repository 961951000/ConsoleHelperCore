using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;
using OfficeOpenXml;

namespace ConsoleHelperCore.Helpers
{
    public class EPPlusHelper
    {
        /// <summary>
        /// Datable loaded into memory stream.
        /// </summary>
        /// <param name="data">The data source.</param>
        /// <param name="workbookName">The name of the workbook.</param>
        public static MemoryStream TableToStream(DataTable data, string worksheetName = "Sheet1")
        {
            using (ExcelPackage package = new ExcelPackage())
            {
                ExcelWorksheet worksheet = package.Workbook.Worksheets.Add(worksheetName);
                worksheet.Cells["A1"].LoadFromDataTable(data, true);
                var stream = new MemoryStream(package.GetAsByteArray());

                return stream;
            }
        }

        /// <summary>
        /// Collection loaded into memory stream.
        /// </summary>
        /// <param name="data">The data source.</param>
        /// <param name="workbookName">The name of the workbook.</param>
        public static MemoryStream CollectionToStream<T>(IEnumerable<T> data, string worksheetName = "Sheet1")
        {
            using (ExcelPackage package = new ExcelPackage())
            {
                ExcelWorksheet worksheet = package.Workbook.Worksheets.Add(worksheetName);
                worksheet.Cells["A1"].LoadFromCollection(data);
                var stream = new MemoryStream(package.GetAsByteArray());

                return stream;
            }
        }
    }
}
