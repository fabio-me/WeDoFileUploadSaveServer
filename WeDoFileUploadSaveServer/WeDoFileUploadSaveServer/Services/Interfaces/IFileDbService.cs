using WeDoFileUploadSaveServer.DTOs;

namespace WeDoFileUploadSaveServer.Services.Interfaces
{
    public interface IFileDbService
    {
        Task<FileDbSaveResultDTO> Create(IFormFile file, string key, string group);
        Task<FileDbViewDTO> View(string fileName);
        Task<FileDbDeleteResultDTO> Delete(string fileName);
    }
}
