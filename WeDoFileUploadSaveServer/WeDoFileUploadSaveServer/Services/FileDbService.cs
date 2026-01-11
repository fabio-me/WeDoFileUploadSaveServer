using WeDoFileUploadSaveServer.Models;
using WeDoFileUploadSaveServer.DTOs;
using WeDoFileUploadSaveServer.Repositories.Contexts;
using WeDoFileUploadSaveServer.Services.Interfaces;
using WeDoFileUploadSaveServer.Repositories.Models;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Xml.Linq;
using System.Drawing;
using System.Security.Policy;

namespace WeDoFileUploadSaveServer.Services
{
    public class FileDbService : IFileDbService
    {
        public readonly DbContextMariaDB _context;

        public FileDbService(DbContextMariaDB context)
        {
            _context = context;
        }

        private string clearGuid(string guid)
        {
            string result = string.Empty;

            foreach (char c in guid)
            {
                if (c != '-')
                    result += c;
            }

            return result;
        }

        private string GetNameRadom()
        {
            string newGuid = Guid.NewGuid().ToString();
            string newGuid1 = Guid.NewGuid().ToString();
            string guidLong = newGuid + newGuid1;

            return clearGuid(guidLong);
        }

        private long GetSizeLimit()
        {
            long numberInMB = 10;
            return numberInMB * 1024 * 1024;
        }

        public async Task<FileDbSaveConfirmeDTO> Create(IFormFile file, string key, string group)
        {
            if (file == null || file.Length == 0)
                return new FileDbSaveConfirmeDTO().Fail("INVALID_FILE");

            if (file.Length > GetSizeLimit())
                return new FileDbSaveConfirmeDTO().Fail("FILE_LARGER_THAN_ALLOWED");

            if (string.IsNullOrEmpty(key))
                return new FileDbSaveConfirmeDTO().Fail("INVALID_KEY");

            if (string.IsNullOrEmpty(group))
                return new FileDbSaveConfirmeDTO().Fail("INVALID_GROUP");

            using var memoryStream = new MemoryStream();
            await file.CopyToAsync(memoryStream);

            FileDb fileDb = new FileDb
            {
                Key = key,
                Group = group,
                Name = GetNameRadom(),
                Size = file.Length,
                Extension = Path.GetExtension(file.FileName),
                Data = memoryStream.ToArray()
            };

            _context.FileDb.Add(fileDb);
            int rowsAffected = await _context.SaveChangesAsync();

            if (rowsAffected > 0)
            {
                string pathName = $"/{fileDb.Name}";
                return new FileDbSaveConfirmeDTO().Ok(pathName);
            }
            return new FileDbSaveConfirmeDTO();
        }
    }
}
