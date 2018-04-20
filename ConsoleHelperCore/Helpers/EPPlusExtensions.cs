using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using OfficeOpenXml;

namespace ConsoleHelperCore.Helpers
{
    public static class EPPlusExtensions
    {
        public static DataTable ToDataTable(this ExcelPackage package, bool hasHeader = true)
        {
            ExcelWorksheet workSheet = package.Workbook.Worksheets.First();
            DataTable table = new DataTable();
            foreach (var firstRowCell in workSheet.Cells[1, 1, 1, workSheet.Dimension.End.Column])
            {
                table.Columns.Add(hasHeader ? firstRowCell.Text : $"Column {firstRowCell.Start.Column}");
            }
            var startRow = hasHeader ? 2 : 1;
            for (var rowNumber = startRow; rowNumber <= workSheet.Dimension.End.Row; rowNumber++)
            {
                var row = workSheet.Cells[rowNumber, 1, rowNumber, workSheet.Dimension.End.Column];
                var newRow = table.NewRow();
                foreach (var cell in row)
                {
                    newRow[cell.Start.Column - 1] = cell.Text;
                }
                table.Rows.Add(newRow);
            }
            return table;
        }

        public static IEnumerable<T> ConvertSheetToObjects<T>(this ExcelWorksheet worksheet) where T : new()
        {
            //DateTime Conversion
            var convertDateTime = new Func<double, DateTime>(excelDate =>
            {
                if (excelDate < 1)
                {
                    throw new ArgumentException("Excel dates cannot be smaller than 0.");
                }

                var dateOfReference = new DateTime(1900, 1, 1);

                if (excelDate > 60d)
                {
                    excelDate = excelDate - 2;
                }
                else
                {
                    excelDate = excelDate - 1;
                }

                return dateOfReference.AddDays(excelDate);
            });

            //Get the properties of T
            var tprops = (new T())
                .GetType()
                .GetProperties()
                .ToList();

            //Cells only contains references to cells with actual data
            var groups = worksheet.Cells
                .GroupBy(cell => cell.Start.Row)
                .ToList();

            //Assume the second row represents column data types (big assumption!)
            var types = groups
                .Skip(1)
                .First()
                .Select(rcell => rcell.Value.GetType())
                .ToList();

            //Assume first row has the column names
            var colnames = groups
                .First()
                .Select((hcell, idx) => new { Name = hcell.Value.ToString(), index = idx })
                .Where(o => tprops.Select(p => p.Name).Contains(o.Name))
                .ToList();

            //Everything after the header is data
            var rowvalues = groups
                .Skip(1) //Exclude header
                .Select(cg => cg.Select(c => c.Value).ToList());

            //Create the collection container
            var collection = rowvalues
                .Select(row =>
                {
                    var tnew = Activator.CreateInstance<T>(); ;
                    colnames.ForEach(colname =>
                    {
                        //This is the real wrinkle to using reflection - Excel stores all numbers as double including int
                        var val = row[colname.index];
                        var type = types[colname.index];
                        var prop = tprops.First(p => p.Name == colname.Name);

                        //If it is numeric it is a double since that is how excel stores all numbers
                        if (type == typeof(double))
                        {
                            //Unbox it
                            var unboxedVal = (double)val;

                            //Far from a complete list!
                            if (prop.PropertyType == typeof(Int32))
                            {
                                prop.SetValue(tnew, (int)unboxedVal);
                            }
                            else if (prop.PropertyType == typeof(double))
                            {
                                prop.SetValue(tnew, unboxedVal);
                            }
                            else if (prop.PropertyType == typeof(DateTime))
                            {
                                prop.SetValue(tnew, convertDateTime(unboxedVal));
                            }
                            else
                            {
                                throw new NotImplementedException(String.Format("Type '{0}' not implemented yet!", prop.PropertyType.Name));
                            }
                        }
                        else
                        {
                            //Its a string
                            prop.SetValue(tnew, val);
                        }
                    });

                    return tnew;
                });

            //Send it back
            return collection;
        }
    }
}
