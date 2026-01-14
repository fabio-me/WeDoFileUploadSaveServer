using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WeDoFileUploadSaveServer.DTOs;
using WeDoFileUploadSaveServer.Services;
using WeDoFileUploadSaveServer.Services.Interfaces;

namespace WeDoFileUploadSaveServer.Controllers
{
    public class FileController : Controller
    {
        public readonly IFileDbService _fileDbService;
        public readonly IConfiguration _configuration;

        public FileController(IFileDbService fileDbService, IConfiguration configuration)
        {
            _fileDbService = fileDbService;
            _configuration = configuration;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Upload(
            IFormFile file,
            string project_name,
            string group,
            string api_key_server)
        {
            //return Ok("abc");

            // adicionar white liste com os ips aultorizados

            if (file == null || file.Length == 0)
                return BadRequest("Arquivo inválido");

            // HTTP ERROR 401
            if (api_key_server != _configuration["api-key-server"])
                return Unauthorized();

            //string key = "f9093a0557c7a280e278916a27f04f37";
            //string group = "test";
            FileDbSaveResultDTO fileDbSaveResultDTO = await _fileDbService.Create(file, project_name, group);

            if (!fileDbSaveResultDTO.Success)
                return BadRequest(fileDbSaveResultDTO.Message);

            return Json(fileDbSaveResultDTO);
        }

        public async Task<IActionResult> Get(string id) //media
        {
            string fileName = id;

            // implemte pedido de key do arquivo
            FileDbViewDTO fileDbViewDTO = await _fileDbService.View(fileName);

            // criar metodo de validar nome para não sobre carregara o server

            // cruzar e jogar uma key no front pra autenticar no broswe a visualizar o arquivo

            if (!fileDbViewDTO.Success ||
                fileDbViewDTO.Data == null ||
                fileDbViewDTO.ContentType == null)
                return BadRequest(fileDbViewDTO.Message);

            Response.Headers["Content-Disposition"] = "inline";
            return File(fileDbViewDTO.Data, fileDbViewDTO.ContentType);
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(
            string file_name,
            [FromHeader] string api_key_server)
        {
            if (api_key_server != _configuration["api-key-server"])
                return Unauthorized();

            FileDbDeleteResultDTO fileDbDeleteResultDTO = await _fileDbService.Delete(file_name);

            return Json(fileDbDeleteResultDTO);
        }
    }
}
