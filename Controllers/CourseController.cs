using BtkAkademi.Models;
using Microsoft.AspNetCore.Mvc;

namespace BtkAkademi.Controllers
{
    public class CourseController : Controller
    {

        public IActionResult Index()
        {
            var model = Repository.Applications;
            return View(model);
        }
        public IActionResult Apply()
        {
            return View();
        }

        [HttpPost] //yazılmaz ise get işlemi gibi davranır
        [ValidateAntiForgeryToken] // kullanılan arama motorunun güvenliğini test eder
        public IActionResult Apply([FromForm]Candidate model)
        {
            if (ModelState.IsValid)
            {
                Repository.Add(model);
                return View("Feedback", model); //direk köke gönderir
            }

        }
    }
}