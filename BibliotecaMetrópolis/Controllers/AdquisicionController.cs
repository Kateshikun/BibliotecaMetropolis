using Microsoft.AspNetCore.Mvc;

namespace BibliotecaMetrópolis.Controllers
{
    public class AdquisicionController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }
    }
}
