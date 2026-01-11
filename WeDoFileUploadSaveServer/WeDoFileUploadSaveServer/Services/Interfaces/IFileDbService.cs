using WeDoFileUploadSaveServer.DTOs;

namespace WeDoFileUploadSaveServer.Services.Interfaces
{
    public interface IFileDbService
    {
        Task<FileDbSaveConfirmeDTO> Create(IFormFile file, string key, string group);
    }
}
