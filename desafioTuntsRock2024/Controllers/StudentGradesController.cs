using desafioTuntsRock2024.Models;
using Microsoft.AspNetCore.Mvc;

namespace desafioTuntsRock2024.Controllers
{
    public class StudentGradesController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult CalculateGrades()
        {
            return RedirectToAction(nameof(FinalPassing));
        }

        public IActionResult FinalPassing()
        {
            var list = new List<Student>
            {
                new()
                {
                    Registration = 1,
                    Name = "Caroline",
                    Absence = 8,
                    P1 = 9,
                    P2 = 10,
                    P3 = 11,
                    FinalPassing = 0,
                    Situation = "Aprovado"
                }
            };
            return View(list);
        }
    }
}
