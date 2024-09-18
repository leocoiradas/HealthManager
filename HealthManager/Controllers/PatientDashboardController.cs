using HealthManager.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HealthManager.Controllers
{
    public class PatientDashboardController : Controller
    {
        private readonly HealthManagerContext _dbcontext;

        public PatientDashboardController(HealthManagerContext dbcontext)
        {
            _dbcontext = dbcontext;
        }
        public IActionResult Index()
        {
            return View();
        }
        public async Task <IActionResult> MyAppointments()
        {
            /* var userCookie = Request.Cookies["User"];

             if (string.IsNullOrEmpty(userCookie))
             {
                  return RedirectToAction("Login", "Authorize");
             }


             if (!int.TryParse(userCookie, out int patientId))
             {
                 return BadRequest("Invalid cookie value. There is no cookie.");
             }

             var patientAppointments = await _dbcontext.Appointments
                 .Where(p => p.PatientId == patientId && p.Status=="Reserved")
                 .ToListAsync();*/
            var patientAppointments = await _dbcontext.Appointments
                 .Where(p => p.Status == "Reserved")
                 .ToListAsync();

            return View(patientAppointments);
        }

        public async Task <IActionResult> MedicalRegisters()
        {
            var medicalRegisters = await _dbcontext.MedicalRegisters.ToListAsync();
            return View(medicalRegisters);
        }
    }
}
