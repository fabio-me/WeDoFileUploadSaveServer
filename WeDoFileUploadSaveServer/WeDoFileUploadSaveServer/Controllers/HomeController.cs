using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Diagnostics;
using WeDoFileUploadSaveServer.DTOs;
using WeDoFileUploadSaveServer.Models;
using WeDoFileUploadSaveServer.Repositories.Contexts;
using WeDoFileUploadSaveServer.Repositories.Models;
using WeDoFileUploadSaveServer.Services;
using WeDoFileUploadSaveServer.Services.Interfaces;

namespace WeDoFileUploadSaveServer.Controllers
{
    public class HomeController : Controller
    {
        private readonly IFileDbService _fileDbService;
        private readonly ILogger<HomeController> _logger;
        private readonly DbContextMariaDB _context;

        public HomeController(IFileDbService fileDbService, ILogger<HomeController> logger, DbContextMariaDB context)
        {
            _fileDbService = fileDbService;
            _logger = logger;
            _context = context;
        }

        public IActionResult FileSave()
        {

            //return View();
            return PartialView();
        }

        // tests
        [HttpPost("upload")]
        public async Task<IActionResult> Upload(IFormFile file, string key, string group, string key_server)
        {
            if (file == null || file.Length == 0)
                return BadRequest("Arquivo inválido");

            // HTTP ERROR 401
            if (key_server != "key_7194f9c8-61c2-4ba6-8c47-aec4cd70ed53")
                return Unauthorized();

            //string key = "f9093a0557c7a280e278916a27f04f37";
            //string group = "test";
            FileDbSaveConfirmeDTO fileDbSaveConfirme = await _fileDbService.Create(file, key, group);

            return Json(fileDbSaveConfirme);
        }


        // downloads
        // https://localhost:7216/Home/GetFile?nome=guid
        [HttpGet]
        public async Task<IActionResult> GetFile([FromQuery] string nome)
        {
            if (string.IsNullOrWhiteSpace(nome))
                return BadRequest("Nome do arquivo é obrigatório");

            var arquivo = await _context.Arquivo
                .FirstOrDefaultAsync(a => a.NomeOriginal == nome);

            if (arquivo == null)
                return NotFound("Arquivo não encontrado");

            return File(
                arquivo.Conteudo,
                arquivo.ContentType,
                arquivo.NomeOriginal
            );
        }


        //Visualizar
        // https://localhost:7216/Home/VisualizarFile?nome=guid
        [HttpGet]
        public async Task<IActionResult> VisualizarFile([FromQuery] string nome)
        {
            if (string.IsNullOrWhiteSpace(nome))
                return BadRequest("Nome do arquivo é obrigatório");

            var arquivo = await _context.Arquivo
                .FirstOrDefaultAsync(a => a.NomeOriginal == nome);

            if (arquivo == null)
                return NotFound("Arquivo não encontrado");

            Response.Headers.Add("Content-Disposition", "inline");
            return File(arquivo.Conteudo, arquivo.ContentType);
        }
        //


        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
