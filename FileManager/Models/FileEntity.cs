namespace FileManager.Models
{
    public class FileEntity
    {
        public int Id { get; set; }
        public string FileName { get; set; } =string.Empty;

        public long FileSize { get; set; }
        public string FilePath { get; set; } =string.Empty; 

        public string Uploader { get; set; } =string.Empty;
        public DateTime UploadedAt { get; set; } = DateTime.UtcNow;
    }
}
