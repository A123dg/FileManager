namespace FileManager.DTOs
{
    public class FileUploadDto
    {
        public IFormFile File { get; set; } 
        public string Uploader { get; set; } = string.Empty;
    }
}
