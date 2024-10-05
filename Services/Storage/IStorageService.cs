namespace zstore.net.Services.Storage;

/**
  * Represents a storage service
  * This interface is used to abstract the storage service implementation
  */
public interface IStorageService
{
  /**
    * Uploads a file to the storage service
    * @param file The file to upload
    * @param folder The folder to upload the file to
    * @returns The URI of the uploaded file
    */
  Task<string> UploadAsync(IFormFile file, string? folder = null);

  /**
    * Downloads a file from the storage service
    * @param fileUri The URI of the file to download
    * @returns The file stream
    */
  Task DeleteAsync(string fileUri);
}