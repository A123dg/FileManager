using FileManager.Models;

namespace FileManager.Interfaces
{
    public interface IFileRepository
    {
        Task<FileEntity> AddAsync(FileEntity file);
        Task<List<FileEntity>> GetAllAsync();
        Task<FileEntity?> GetByIdAsync(int id);
        Task DeleteAsync(FileEntity File );
        Task SaveChangesAsync();
    }
}
