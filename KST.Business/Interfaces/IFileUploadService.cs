using Microsoft.AspNetCore.Http;

namespace KST.Business.Interfaces;

public interface IFileUploadService
{
    Task<string?> UploadFileAsync(long projectId, IFormFile file);
    bool DeleteFile(string filePath);
    FileStream? GetFileStream(string filePath);
    string? GetFullFilePath(string filePath);
}