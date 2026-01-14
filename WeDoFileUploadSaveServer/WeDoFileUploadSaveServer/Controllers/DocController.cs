using Microsoft.AspNetCore.Mvc;

namespace WeDoFileUploadSaveServer.Controllers
{
    public class DocController : Controller
    {
        public IActionResult Index()
        {
            return PartialView();
        }
    }
}
