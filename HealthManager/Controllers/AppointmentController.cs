using HealthManager.Models;
using Microsoft.AspNetCore.Mvc;

namespace HealthManager.Controllers
{
    public class AppointmentController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult CreateAppointment(Appointment newAppointment)
        {
            if (ModelState.IsValid)
            {

            }
            return RedirectToAction("Index");
        }
    }
}
