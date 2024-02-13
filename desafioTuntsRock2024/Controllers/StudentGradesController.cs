using desafioTuntsRock2024.Helper;
using desafioTuntsRock2024.Models;
using desafioTuntsRock2024.Models.ViewModel;
using Microsoft.AspNetCore.Mvc;

namespace desafioTuntsRock2024.Controllers
{
    public class StudentGradesController : Controller
    {
        private readonly SheetsHelper _sheetHelper;

        public StudentGradesController()
        {
            _sheetHelper = new SheetsHelper();
        }
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
            List<Student> listStudentsRemediation = _sheetHelper.CalculateGrades();
            return View(listStudentsRemediation);
        }

        [HttpPost]
        public IActionResult SaveAvg(List<StudentGradeRecordViewModel> students)
        {
            var listAvg = _sheetHelper.CalculateFinalGrade(students);
            var sheets = _sheetHelper.ConvertSpreadsheet(_sheetHelper.SpreadSheet());

            foreach (var studentAvg in listAvg)
            {
                var studentCorrespondente = sheets.FirstOrDefault(s => s.Registration == studentAvg.Registration);
                if (studentCorrespondente != null)
                {
                    studentCorrespondente.FinalGrade = studentAvg.FinalGrade;
                }
            }
            return View(nameof(FinalPassing), sheets); 
        }
    }
}
