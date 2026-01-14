using WeDoFileUploadSaveServer.Models;
using WeDoFileUploadSaveServer.DTOs;
using WeDoFileUploadSaveServer.Repositories.Contexts;
using WeDoFileUploadSaveServer.Services.Interfaces;
using WeDoFileUploadSaveServer.Repositories.Models;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Xml.Linq;
using System.Drawing;
using System.Security.Policy;
using Microsoft.EntityFrameworkCore;

namespace WeDoFileUploadSaveServer.Services
{
    public class FileDbService : IFileDbService
    {
        private readonly DbContextMariaDB _context;
        private readonly IConfiguration _configuration;

        public FileDbService(DbContextMariaDB context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
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

        private int GetSizeLimit()
        {
            int maxFileSizeInMb = 1;

            var confMaxFileSizeInMb = _configuration["max-file-size-in-mb"];
            if (confMaxFileSizeInMb != null && int.TryParse(confMaxFileSizeInMb, out var parsedSize))
            {
                maxFileSizeInMb = parsedSize;
            }

            return maxFileSizeInMb * 1024 * 1024;
        }

        public async Task<FileDbSaveResultDTO> Create(IFormFile file, string projectName, string group)
        {
            if (file == null || file.Length == 0)
                return new FileDbSaveResultDTO().Fail("INVALID_FILE");

            if (file.Length > GetSizeLimit())
                return new FileDbSaveResultDTO().Fail("FILE_LARGER_THAN_ALLOWED");

            if (string.IsNullOrEmpty(projectName))
                return new FileDbSaveResultDTO().Fail("INVALID_PROJECT_NAME");

            if (string.IsNullOrEmpty(group))
                return new FileDbSaveResultDTO().Fail("INVALID_GROUP");

            using var memoryStream = new MemoryStream();
            await file.CopyToAsync(memoryStream);

            FileDb fileDb = new FileDb
            {
                ProjectName = projectName,
                Group = group,
                Name = GetNameRadom(),
                Size = file.Length,
                ContentType = file.ContentType,
                Data = memoryStream.ToArray()
            };

            _context.FileDb.Add(fileDb);
            int rowsAffected = await _context.SaveChangesAsync();

            if (rowsAffected > 0)
            {
                return new FileDbSaveResultDTO().Ok(fileDb.Name);
            }
            return new FileDbSaveResultDTO();
        }

        public async Task<FileDbViewDTO> View(string fileName)
        {
            if (string.IsNullOrWhiteSpace(fileName))
                return new FileDbViewDTO().Fail("INVALID_FILE_NAME");

            FileDb? fileDb = await _context.FileDb
                .FirstOrDefaultAsync(f => f.Name == fileName && f.IsDeleted == false);

            if (fileDb == null)
                return new FileDbViewDTO().Fail("NOT_FOUND");

            if (fileDb.ContentType == null ||
                fileDb.Data == null)
                return new FileDbViewDTO().Fail("FILE_ERROR");

            return new FileDbViewDTO().Ok(fileDb.ContentType, fileDb.Data);
        }

        public async Task<FileDbDeleteResultDTO> Delete(string fileName)
        {
            if (string.IsNullOrWhiteSpace(fileName))
               return new FileDbDeleteResultDTO().Fail("INVALID_FILE_NAME");

            FileDb? fileDbForDelete = await _context.FileDb
                .FirstOrDefaultAsync(f => f.Name == fileName && f.IsDeleted == false);
            if (fileDbForDelete == null)
                return new FileDbDeleteResultDTO().Fail("NOT_FOUND");

            fileDbForDelete.IsDeleted = true;
            _context.FileDb.Update(fileDbForDelete);
            int rowsAffected = await _context.SaveChangesAsync();

            if (rowsAffected > 0)
            {
                return new FileDbDeleteResultDTO().Ok();
            }
            return new FileDbDeleteResultDTO().Fail("ERROR_DATABASE");
        }
    }
}
