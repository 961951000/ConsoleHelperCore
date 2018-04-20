using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;
using CsvHelper;

namespace ConsoleHelperCore.Helpers
{
    public class ExcelHelper
    {
        public static void DatatableToExcel(string path, DataTable dt)
        {
            using (var textWriter = File.CreateText(path))
            using (var csv = new CsvWriter(textWriter))
            {
                // Write columns
                foreach (DataColumn column in dt.Columns)
                {
                    csv.WriteField(column.ColumnName);
                }
                csv.NextRecord();
                // Write row values
                foreach (DataRow row in dt.Rows)
                {
                    for (var i = 0; i < dt.Columns.Count; i++)
                    {
                        csv.WriteField(row[i]);
                    }
                    csv.NextRecord();
                }
            }
        }

        public static void ListToExcel<T>(string filename, IEnumerable<T> list)
        {
            using (TextWriter writer = File.CreateText(filename))
            {
                var csv = new CsvWriter(writer);
                csv.WriteRecords(list);
            }
        }
    }
}
