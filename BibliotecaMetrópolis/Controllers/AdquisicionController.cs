using Microsoft.AspNetCore.Mvc;
using BibliotecaMetrópolis.Models;

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
