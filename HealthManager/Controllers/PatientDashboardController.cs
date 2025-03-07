using HealthManager.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HealthManager.Controllers
{
    [Authorize(Roles = "Patient")]
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
                 .Where(p => p.PatientId == 123)
                 .ToListAsync();

            return View(patientAppointments);
        }
        
        [HttpPut]
        public async Task <IActionResult> CancelAppointment(Guid appointmentId)
        {
            var databaseAppointment = await _dbcontext.Appointments
                .Where(p => p.AppointmentId == appointmentId)
                .FirstOrDefaultAsync();
            var requestDate = DateOnly.FromDateTime(DateTime.Now);
            if (databaseAppointment.AppointmentDate.CompareTo(requestDate) < 0)
            {
                databaseAppointment.Status = "Available";
                databaseAppointment.PatientId = null;
                _dbcontext.Appointments.Update(databaseAppointment);
                await _dbcontext.SaveChangesAsync();
                return RedirectToAction("MyAppointments" , "PatientDashboard");
            }
            else
            {
                ModelState.AddModelError("CancelError", "The appoinment has to be cancelled before the appointment date.");
                return RedirectToAction("MyAppointments" , "PatientDashboard");
            }
        }

        public async Task <IActionResult> MedicalRegisters()
        {
            var medicalRegisters = await _dbcontext.MedicalRegisters.ToListAsync();
            return View(medicalRegisters);
        }
    }
}
