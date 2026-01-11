using WeDoFileUploadSaveServer.Repositories.Models;

namespace WeDoFileUploadSaveServer.DTOs
{
    public class FileDbSaveConfirmeDTO
    {
        public bool Success { get; set; }
        public string? Message { get; set; }
        public string? PathName { get; set; }

        public FileDbSaveConfirmeDTO Ok(string pathName)
        {
            return new FileDbSaveConfirmeDTO
            {
                Success = true,
                PathName = pathName,
                Message = "OPERATION_COMPLETED_SUCCESSFULLY"
            };
        }

        public FileDbSaveConfirmeDTO Fail(string message)
        {
            return new FileDbSaveConfirmeDTO
            {
                Success = false,
                Message = message,
            };
        }
    }
}
