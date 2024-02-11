using desafioTuntsRock2024.Helper;
using desafioTuntsRock2024.Models;
using Google.Apis.Sheets.v4.Data;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

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