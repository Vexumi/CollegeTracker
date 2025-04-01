using System.ComponentModel;
using System.Reflection;
using ClosedXML.Excel;
using KST.Business.Interfaces;

namespace KST.Business.Services;

public class ExcelExportService : IExcelExportService
{
    public Stream ExportToExcel(IEnumerable<object> data, string worksheetName = "Data")
    {
        try
        {
            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add(worksheetName);

                var properties = data.First().GetType().GetProperties();

                for (int i = 0; i < properties.Length; i++)
                {
                    var displayNameAttribute = properties[i].GetCustomAttribute<DisplayNameAttribute>();
                    worksheet.Cell(1, i + 1).Value = displayNameAttribute?.DisplayName ?? properties[i].Name;
                }

                int row = 2;
                foreach (var item in data)
                {
                    for (int i = 0; i < properties.Length; i++)
                    {
                        try
                        {
                            var value = properties[i].GetValue(item);

                            var nullableDateOnly = value as DateOnly?;
                            if (nullableDateOnly != null)
                            {
                                value = nullableDateOnly?.ToDateTime(TimeOnly.MinValue);
                            }
                            else if (value is DateOnly dateOnly)
                            {
                                value = dateOnly.ToDateTime(TimeOnly.MinValue);
                            }

                            worksheet.Cell(row, i + 1).Value = value == null ? string.Empty : Convert.ToString(value);
                        }
                        catch (Exception ex)
                        {
                            worksheet.Cell(row, i + 1).Value = "Error";
                        }
                    }
                    row++;
                }

                worksheet.Columns().AdjustToContents();

                var stream = new MemoryStream();
                workbook.SaveAs(stream);
                stream.Seek(0, SeekOrigin.Begin);
                return stream;
            }
        }
        catch
        {
            return Stream.Null;
        }
    }
    
    public Stream ExportToExcel(Dictionary<string, IEnumerable<object>> data)
    {
        try
        {
            using (var workbook = new XLWorkbook())
            {
                foreach (var sheetData in data)
                {
                    var worksheetName = sheetData.Key;
                    var items = sheetData.Value;

                    var worksheet = workbook.Worksheets.Add(worksheetName);

                    if (items == null || !items.Any())
                    {
                        continue;
                    }

                    var firstItem = items.First();
                    var properties = firstItem.GetType().GetProperties();

                    for (int i = 0; i < properties.Length; i++)
                    {
                        var displayNameAttribute = properties[i].GetCustomAttribute<DisplayNameAttribute>();
                        worksheet.Cell(1, i + 1).Value = displayNameAttribute?.DisplayName ?? properties[i].Name;
                    }

                    int row = 2;
                    foreach (var item in items)
                    {
                        for (int i = 0; i < properties.Length; i++)
                        {
                            try
                            {
                                var value = properties[i].GetValue(item);

                                var nullableDateOnly = value as DateOnly?;
                                if (nullableDateOnly != null)
                                {
                                    value = nullableDateOnly?.ToDateTime(TimeOnly.MinValue);
                                }
                                else if (value is DateOnly dateOnly)
                                {
                                    value = dateOnly.ToDateTime(TimeOnly.MinValue);
                                }

                                worksheet.Cell(row, i + 1).Value = value == null ? string.Empty : Convert.ToString(value);
                            }
                            catch (Exception ex)
                            {
                                worksheet.Cell(row, i + 1).Value = "Error";
                            }
                        }
                        row++;
                    }

                    worksheet.Columns().AdjustToContents();
                }

                var stream = new MemoryStream();
                workbook.SaveAs(stream);
                stream.Seek(0, SeekOrigin.Begin);
                return stream;
            }
        }
        catch
        {
            return Stream.Null;
        }
    }
}