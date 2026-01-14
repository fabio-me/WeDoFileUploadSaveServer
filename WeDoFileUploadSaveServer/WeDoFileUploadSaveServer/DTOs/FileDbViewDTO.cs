namespace WeDoFileUploadSaveServer.DTOs
{
    public class FileDbViewDTO
    {
        public bool Success { get; set; }
        public string? Message { get; set; }
        public string? ContentType { get; set; }
        public byte[]? Data { get; set; }

        public FileDbViewDTO Ok(string contentType, byte[] data)
        {
            return new FileDbViewDTO
            {
                Success = true,
                ContentType = contentType,
                Data = data

            };
        }

        public FileDbViewDTO Fail(string message)
        {
            return new FileDbViewDTO
            {
                Success = false,
                Message = message
            };
        }
    }
}
