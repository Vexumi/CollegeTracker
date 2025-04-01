namespace KST.Business.Interfaces;

public interface IExcelExportService
{
    Stream ExportToExcel(IEnumerable<object> data, string worksheetName = "List 1");
    Stream ExportToExcel(Dictionary<string, IEnumerable<object>> data);
}