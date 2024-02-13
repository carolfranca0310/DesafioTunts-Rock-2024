using desafioTuntsRock2024.Helper;
using Microsoft.AspNetCore.Mvc;

namespace desafioTuntsRock2024.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Info()
        {
            return View();
        }

    }
}