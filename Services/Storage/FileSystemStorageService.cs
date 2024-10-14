namespace zstore.net.Services.Storage;

/**
  * Represents a storage service that stores files in the file system
  * This class implements the IStorageService interface
  */
public class FileSystemStorageService : IStorageService
{
  private readonly IWebHostEnvironment _environment;

  public FileSystemStorageService(IWebHostEnvironment environment)
  {
    _environment = environment;
  }

  public async Task<string> UploadAsync(IFormFile file, string? folder = null)
  {
    var uploadsFolder = Path.Combine(_environment.WebRootPath, folder ?? "uploads");

    if (!Directory.Exists(uploadsFolder))
    {
      Directory.CreateDirectory(uploadsFolder);
    }

    var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
    var filePath = Path.Combine(uploadsFolder, fileName);

    using (var fileStream = new FileStream(filePath, FileMode.Create))
    {
      await file.CopyToAsync(fileStream);
    }

    // Return the relative URI for the saved file
    return Path.Combine(folder ?? "uploads", fileName).Replace("\\", "/");
  }

  public Task DeleteAsync(string fileUri)
  {
    var filePath = Path.Combine(_environment.WebRootPath, fileUri.TrimStart('/'));
    if (File.Exists(filePath))
    {
      File.Delete(filePath);
    }

    return Task.CompletedTask;
  }

}