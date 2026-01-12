namespace WeDoFileUploadSaveServer.DTOs
{
    public class FileDbView
    {
        public bool Success { get; set; }
        public string? Message { get; set; }
        public string? ContentType { get; set; }
        public byte[]? Data { get; set; }

        public FileDbView Ok(string contentType, byte[] data)
        {
            return new FileDbView
            {
                Success = true,
                ContentType = contentType,
                Data = data

            };
        }

        public FileDbView Fail(string message)
        {
            return new FileDbView
            {
                Success = false,
                Message = message
            };
        }
    }
}
