using WeDoFileUploadSaveServer.Repositories.Models;

namespace WeDoFileUploadSaveServer.DTOs
{
    public class FileDbSaveResultDTO
    {
        public bool Success { get; set; }
        public string? Message { get; set; }
        public string? File_name { get; set; }

        public FileDbSaveResultDTO Ok(string fileName)
        {
            return new FileDbSaveResultDTO
            {
                Success = true,
                File_name = fileName,
                Message = "OPERATION_COMPLETED_SUCCESSFULLY"
            };
        }

        public FileDbSaveResultDTO Fail(string message)
        {
            return new FileDbSaveResultDTO
            {
                Success = false,
                Message = message,
            };
        }
    }
}
