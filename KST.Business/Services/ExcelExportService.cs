using System.ComponentModel;
using System.Reflection;
using ClosedXML.Excel;
using KST.Business.Interfaces;

namespace KST.Business.Services;

public class ExcelExportService : IExcelExportService
{
    public Stream ExportToExcel<T>(IEnumerable<T> data, string worksheetName = "Data")
    {
        try
        {
            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add(worksheetName);

                var properties = typeof(T).GetProperties();

                // Write headers
                for (int i = 0; i < properties.Length; i++)
                {
                    var displayNameAttribute = properties[i].GetCustomAttribute<DisplayNameAttribute>();
                    worksheet.Cell(1, i + 1).Value = displayNameAttribute?.DisplayName ?? properties[i].Name;
                }

                // Write data
                int row = 2;
                foreach (var item in data)
                {
                    for (int i = 0; i < properties.Length; i++)
                    {
                        // Use a try-catch block for each cell to handle potential type conversion errors
                        try
                        {
                            var value = properties[i].GetValue(item);

                            //Handle Nullable<DateOnly>
                            var nullableDateOnly = value as DateOnly?;
                            if (nullableDateOnly != null)
                            {
                                value = nullableDateOnly?.ToDateTime(TimeOnly.MinValue);
                            }
                            else if (value is DateOnly dateOnly)
                            {
                                value = dateOnly.ToDateTime(TimeOnly.MinValue);
                            }

                            // Явное преобразование в строку, чтобы избежать ошибок типов
                            worksheet.Cell(row, i + 1).Value = value == null ? string.Empty : Convert.ToString(value);
                        }
                        catch (Exception ex)
                        {
                            worksheet.Cell(row, i + 1).Value = "Error"; // Indicate an error in the cell
                        }
                    }
                    row++;
                }

                worksheet.Columns().AdjustToContents();

                var stream = new MemoryStream();
                workbook.SaveAs(stream);
                stream.Seek(0, SeekOrigin.Begin); // Reset stream position to the beginning
                return stream;
            }
        }
        catch
        {
            return Stream.Null;
        }
    }
}