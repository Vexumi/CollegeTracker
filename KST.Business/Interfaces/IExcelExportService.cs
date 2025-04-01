namespace KST.Business.Interfaces;

public interface IExcelExportService
{
    Stream ExportToExcel<T>(IEnumerable<T> data, string worksheetName = "Data");
}