using FileManager.Interfaces;
using AutoMapper;
using FileManager.DTOs;
using FileManager.Models;
using System.IO;

namespace FileManager.Services
{
    public class FileService
    {
        private readonly IFileRepository _repository;
        private readonly IMapper _mapper;
        private readonly string _uploadPath;

        public FileService(IFileRepository repository, IMapper mapper, IConfiguration config)
        {
            _repository = repository;
            _mapper = mapper;
            _uploadPath = Path.Combine(Directory.GetCurrentDirectory(), config["UploadPath"]);

            if (!Directory.Exists(_uploadPath))
                Directory.CreateDirectory(_uploadPath);
        }

        public async Task<FileDto> UploadAsync(FileUploadDto fileUploadDto)
        {
            var filePath = Path.Combine(_uploadPath, fileUploadDto.File.FileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await fileUploadDto.File.CopyToAsync(stream);
            }

            var fileEntity = new FileEntity
            {
                FileName = fileUploadDto.File.FileName,
                FileSize = fileUploadDto.File.Length,
                FilePath = filePath,
                Uploader = fileUploadDto.Uploader,
                UploadedAt = DateTime.UtcNow
            };

            var save = await _repository.AddAsync(fileEntity);
            return _mapper.Map<FileDto>(save);
        }

        public async Task<List<FileDto>> GetAllFile()
        {
            var list = await _repository.GetAllAsync();
            return _mapper.Map<List<FileDto>>(list);
        }

        public async Task<(byte[] data, string name)?> DownloadAsync(int id)
        {
            var result = await _repository.GetByIdAsync(id);
            if (result == null) return null;

            var bytes = await File.ReadAllBytesAsync(result.FilePath);
            return (bytes, result.FileName);
        }

        public async Task DeleteAsync(int id)
        {
            var file = await _repository.GetByIdAsync(id);
            if (file == null) return;

            if (System.IO.File.Exists(file.FilePath))
                System.IO.File.Delete(file.FilePath);

            await _repository.DeleteAsync(file);
        }
    }
}
