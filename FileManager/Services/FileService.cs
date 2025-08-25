using FileManager.Interfaces;
using AutoMapper;
using FileManager.DTOs;
using FileManager.Models;
using System.IO;
using System.IO.Compression;

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
            using var ms = new MemoryStream();
            await fileUploadDto.File.CopyToAsync(ms);

            var fileEntity = new FileEntity
            {
                FileName = fileUploadDto.File.FileName,
                FileSize = fileUploadDto.File.Length,
                Uploader = fileUploadDto.Uploader,
                UploadedAt = DateTime.UtcNow,
                FileData = ms.ToArray() 
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
        public async Task<(byte[] data, string name)?> DownloadByUploaderAsync(string uploader)
        {
            var files = await _repository.GetByUploaderAsync(uploader);
            if (!files.Any()) return null;

            using (var memoryStream = new MemoryStream())
            {
                using (var zip = new ZipArchive(memoryStream, ZipArchiveMode.Create, true))
                {
                    foreach (var file in files)
                    {
                        if (System.IO.File.Exists(file.FilePath))
                        {
                            var entry = zip.CreateEntry(file.FileName, CompressionLevel.Fastest);
                            using (var entryStream = entry.Open())
                            using (var fileStream = new FileStream(file.FilePath, FileMode.Open, FileAccess.Read))
                            {
                                await fileStream.CopyToAsync(entryStream);
                            }
                        }
                    }
                }
                return (memoryStream.ToArray(), $"{uploader}_files.zip");
            }
        }
    }
}
