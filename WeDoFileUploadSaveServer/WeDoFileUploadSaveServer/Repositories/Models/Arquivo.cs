namespace WeDoFileUploadSaveServer.Repositories.Models
{
    public class Arquivo
    {
        public int Id { get; set; }
        public string? NomeOriginal { get; set; }
        public string? Extensao { get; set; }
        public string? ContentType { get; set; }
        public long Tamanho { get; set; }
        public byte[]? Conteudo { get; set; }
    }
}
