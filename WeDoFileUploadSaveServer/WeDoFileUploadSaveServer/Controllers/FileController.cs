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

        public FileController(IFileDbService fileDbService)
        {
            _fileDbService = fileDbService;
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
            string key_server)
        {
            //return Ok("abc");

            if (file == null || file.Length == 0)
                return BadRequest("Arquivo inválido");

            // HTTP ERROR 401
            if (key_server != "key_7194f9c8-61c2-4ba6-8c47-aec4cd70ed53")
                return Unauthorized();

            //string key = "f9093a0557c7a280e278916a27f04f37";
            //string group = "test";
            FileDbSaveConfirmeDTO fileDbSaveConfirme = await _fileDbService.Create(file, project_name, group);

            if (!fileDbSaveConfirme.Success)
                return BadRequest(fileDbSaveConfirme.Message);

            return Json(fileDbSaveConfirme);
        }

        [HttpPost]
        public async Task<IActionResult> Get(string fileName)
        {
            // implemte pedido de key do arquivo
            FileDbView fileDbView = await _fileDbService.View(fileName);
            //return File(fileDbView.Data, fileDbView.Extension);
            return Ok();
        }

        public async Task<IActionResult> View(string id)
        {
            string fileName = id;

            // implemte pedido de key do arquivo
            FileDbView fileDbView = await _fileDbService.View(fileName);

            // url esemplo usar nome do projeto
            // https://d1.awsstatic.com/onedam/marketing-channels/website/aws/en_US

            // criar metodo de validar nome para não sobre carregara o server

            // cruzar e jogar uma key no front pra autenticar no broswe a visualizar o arquivo

            if (!fileDbView.Success)
                return BadRequest(fileDbView.Message);

            //Response.Headers.Add("Content-Disposition", "inline");
            Response.Headers["Content-Disposition"] = "inline";
            return File(fileDbView.Data, fileDbView.ContentType);
        }
    }
}
