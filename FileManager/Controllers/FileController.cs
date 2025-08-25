using FileManager.Models;
using FileManager.DTOs;
using Microsoft.AspNetCore.Mvc;
using FileManager.Services;
namespace FileManager.Controllers
{
    [ApiController]
    [Route("api/filles")]
    public class FileController : ControllerBase
    {

        private readonly FileService _fileService;
        public FileController(FileService fileService )
        {
            _fileService = fileService;
        }
        [HttpPost("upload")]
        public async Task<IActionResult> UploadFile([FromForm] FileUploadDto fileDto)
        {
           var result = await _fileService.UploadAsync(fileDto);
            return Ok(result);
        }
        [HttpGet]
        public async Task<IActionResult> GetAllAynce()
        {
            var result =  await _fileService.GetAllFile();
            return Ok(result);
        }
        [HttpGet("download/{id}")]
        public async Task<IActionResult> Download( int id)
        {
            var result = await _fileService.DownloadAsync(id);
            if(result == null) return NotFound();
            return File(result.Value.data, "application/octet-stream", result.Value.name);

        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _fileService.DeleteAsync(id);
            return NoContent();

            

        }
    }
}
