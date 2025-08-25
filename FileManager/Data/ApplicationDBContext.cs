using FileManager.Mappers;
using FileManager.Models;
using Microsoft.EntityFrameworkCore;

namespace api.Data
{
    public class ApplicationDBContext : Microsoft.EntityFrameworkCore.DbContext
    {
        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options)
            : base(options)
        {
        }

        public  DbSet<FileEntity>  Files { get; set; }
    }
}
