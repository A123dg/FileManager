using api.Data;
using FileManager.Interfaces;
using FileManager.Models;
using Microsoft.EntityFrameworkCore;
using System;

namespace FileManager.Repositories
{
    public class FileRepository : IFileRepository
    {
        private readonly ApplicationDBContext _context;
        public FileRepository(ApplicationDBContext context)
        {
            _context = context;
        }

        public async Task<FileEntity> AddAsync(FileEntity file)
        {
            _context.Files.Add(file);
            await _context.SaveChangesAsync();
            return file;
        }

        public async Task<List<FileEntity>> GetAllAsync()
        {
            return await _context.Files.ToListAsync();
        }

        public async Task<FileEntity?> GetByIdAsync(int id)
        {
            return await _context.Files.FindAsync(id);
        }

        public async Task DeleteAsync(FileEntity file)
        {
            _context.Files.Remove(file);
            await _context.SaveChangesAsync();
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task<List<FileEntity>> GetByUploaderAsync(string uploader)
        {
            return await _context.Files
                                 .Where(f => f.Uploader == uploader)
                                 .ToListAsync();
        }
    }
}
