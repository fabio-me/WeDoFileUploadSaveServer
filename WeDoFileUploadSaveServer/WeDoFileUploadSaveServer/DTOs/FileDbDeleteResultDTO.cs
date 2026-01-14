namespace WeDoFileUploadSaveServer.DTOs
{
    public class FileDbDeleteResultDTO
    {
        public bool Success {  get; set; }
        public string? Message { get; set; }

        public FileDbDeleteResultDTO Ok()
        {
            return new FileDbDeleteResultDTO
            {
                Success = true,
                Message = "FILE_DELETE_SUCCESS"
            };
        }

        public FileDbDeleteResultDTO Fail(string message)
        {
            return new FileDbDeleteResultDTO
            {
                Success = false,
                Message = message
            };
        }
    }
}
