namespace WeDoFileUploadSaveServer.Repositories.Models
{
    public class FileDb
    {
        public long Id { get; set; }
        public bool IsDeleted { get; set; }
        public string? Key { get; set; } // KeyFile
        public string? Group { get; set; }
        public string? Name { get; set; }
        public long Size { get; set; }
        public string? Extension { get; set; }
        public byte[]? Data { get; set; }
    }
}
