using CsvHelper;
using System.Data;
using System.Globalization;
using System.IO;
using CsvHelper.Configuration;
using System.Linq;
using OfficeOpenXml;

namespace KGS.Web.Code.Attributes
{
    public class ImportData
    {
        public static DataSet ConvertExcelToDataSet(Stream fileStream)
        {
            var dataSet = new DataSet();

            using (var package = new ExcelPackage(fileStream))
            {
                foreach (var worksheet in package.Workbook.Worksheets)
                {
                    var dataTable = new System.Data.DataTable(worksheet.Name);
                    var startRow = worksheet.Dimension.Start.Row;
                    var endRow = worksheet.Dimension.End.Row;
                    var startColumn = worksheet.Dimension.Start.Column;
                    var endColumn = worksheet.Dimension.End.Column;

                    // Add columns
                    for (int col = startColumn; col <= endColumn; col++)
                    {
                        dataTable.Columns.Add(worksheet.Cells[startRow, col].Text);
                    }

                    // Add rows
                    for (int row = startRow + 1; row <= endRow; row++)
                    {
                        var dataRow = dataTable.NewRow();
                        for (int col = startColumn; col <= endColumn; col++)
                        {
                            dataRow[col - 1] = worksheet.Cells[row, col].Text;
                        }
                        dataTable.Rows.Add(dataRow);
                    }
                    string[] columnNames = dataTable.Columns.Cast<DataColumn>()
                                   .Select(x => x.ColumnName)
                                   .ToArray();
                    if (columnNames.Any(t => t == "Result"))
                    {
                        dataTable.Columns.Remove("Result");
                        dataTable.Columns.Remove("Reason");
                    }
                    System.Data.DataColumn newColumn = new System.Data.DataColumn("Result", typeof(System.String));
                    newColumn.DefaultValue = "Failed";
                    dataTable.Columns.Add(newColumn);
                    System.Data.DataColumn newColumn1 = new System.Data.DataColumn("Reason", typeof(System.String));
                    newColumn.DefaultValue = " ";
                    dataTable.Columns.Add(newColumn1);

                    dataSet.Tables.Add(dataTable);
                }
            }

            return dataSet;
        }

        public static DataSet ConvertCsvToDataSet(Stream fileStream, string csvDelimiter = ",")
        {
            var dataSet = new DataSet();

            using (var reader = new StreamReader(fileStream))
            using (var csv = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                Delimiter = csvDelimiter,
                HasHeaderRecord = true
            }))
            {
                var records = csv.GetRecords<dynamic>().ToList();
                if (records.Count > 0)
                {
                    var dataTable = new System.Data.DataTable("CSVData");

                    // Add columns
                    foreach (var header in records[0].Keys)
                    {
                        dataTable.Columns.Add(header);
                    }

                    // Add rows
                    foreach (var record in records)
                    {
                        var dataRow = dataTable.NewRow();
                        foreach (var header in record.Keys)
                        {
                            dataRow[header] = record[header];
                        }
                        dataTable.Rows.Add(dataRow);
                    }
                    string[] columnNames = dataTable.Columns.Cast<DataColumn>()
                                   .Select(x => x.ColumnName)
                                   .ToArray();
                    if (columnNames.Any(t => t == "Result"))
                    {
                        dataTable.Columns.Remove("Result");
                        dataTable.Columns.Remove("Reason");
                    }
                    System.Data.DataColumn newColumn = new System.Data.DataColumn("Result", typeof(System.String));
                    newColumn.DefaultValue = "Failed";
                    dataTable.Columns.Add(newColumn);
                    System.Data.DataColumn newColumn1 = new System.Data.DataColumn("Reason", typeof(System.String));
                    newColumn.DefaultValue = " ";
                    dataTable.Columns.Add(newColumn1);

                    dataSet.Tables.Add(dataTable);
                }
            }

            return dataSet;
        }

    }
}