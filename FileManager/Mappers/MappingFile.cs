using FileManager.DTOs;
using FileManager.Models;
using AutoMapper;

namespace FileManager.Mappers
{
    public class MappingFile : AutoMapper.Profile
    {
        public MappingFile()
        {
            CreateMap<FileEntity, FileDto>();
        }

    }
}
