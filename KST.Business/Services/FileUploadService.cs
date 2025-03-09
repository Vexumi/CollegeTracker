using KST.Business.Infrastructure;
using KST.Business.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace KST.Business.Services;

public class FileUploadService: IFileUploadService
{
    private readonly string _rootPath;

    public FileUploadService(IOptions<AttachmentsOptions> attachmentOptions)
    {
        _rootPath = attachmentOptions.Value.RootPath;

        if (!Directory.Exists(_rootPath))
        {
            Directory.CreateDirectory(_rootPath);
        }
    }
    
    public async Task<string?> UploadFileAsync(long projectId, IFormFile file)
    {
        if (file == null || file.Length == 0)
        {
            return null; // Or throw an exception if you prefer
        }

        string projectDirectoryPath = Path.Combine(_rootPath, projectId.ToString());

        if (!Directory.Exists(projectDirectoryPath))
        {
            Directory.CreateDirectory(projectDirectoryPath);
        }

        string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
        string filePath = Path.Combine(projectDirectoryPath, fileName);

        try
        {
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return Path.Combine(projectId.ToString(), fileName);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error uploading file: {ex}");
            return null;
        }
    }

    public bool DeleteFile(string filePath)
    {
        if (string.IsNullOrEmpty(filePath))
        {
            return false;
        }

        string fullPath = Path.Combine(_rootPath, filePath);

        try
        {
            if (File.Exists(fullPath))
            {
                File.Delete(fullPath);
                return true;
            }
            else
            {
                Console.WriteLine($"File not found: {fullPath}"); // Log this
                return false;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error deleting file: {ex}"); // Log this
            return false;
        }
    }

    public FileStream? GetFileStream(string filePath)
    {
        if (string.IsNullOrEmpty(filePath))
        {
            return null;
        }

        string fullPath = Path.Combine(_rootPath, filePath);

        if (!File.Exists(fullPath))
        {
            return null;
        }

        try
        {
            return new FileStream(fullPath, FileMode.Open, FileAccess.Read);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error opening file stream: {ex}");
            return null;
        }
    }

    public string? GetFullFilePath(string filePath)
    {
        if (string.IsNullOrEmpty(filePath))
        {
            return null;
        }

        return Path.Combine(_rootPath, filePath);
    }
}