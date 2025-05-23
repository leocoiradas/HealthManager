using HealthManager.Models;
using HealthManager.Models.DTO;
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
            DateOnly today = DateOnly.FromDateTime(DateTime.Now);
            TimeOnly currentHour = TimeOnly.FromDateTime(DateTime.Now);
            var userIdString = User.FindFirst("Id")?.Value;
            int.TryParse(userIdString, out int userIdInt);
            var patientAppointments = await _dbcontext.Appointments
                 .Where(p => p.Status == "Reserved" && p.PatientId == userIdInt && p.AppointmentDate >= today && p.AppointmentHour > currentHour)
                 .Include(a => a.Doctor)
                 .ThenInclude(d => d.SpecialtyNavigation)
                 .ToListAsync();
            List<PatientAppointmentsViewModel> appointmentsVm = patientAppointments.Select(x => new PatientAppointmentsViewModel
            {
                AppointmentId = x.AppointmentId,
                DoctorName = x.Doctor.Name + " " + x.Doctor.Surname,
                DoctorSpecialty = x.Doctor.SpecialtyNavigation.SpecialtyName,
                AppointmentDate = x.AppointmentDate,
                AppointmentHour = x.AppointmentHour
            }).ToList();

            return View(appointmentsVm);
        }
        
        [HttpPost]
        public async Task<JsonResult> CancelAppointment(Guid appointmentId)
        {
            Appointment databaseAppointment = await _dbcontext.Appointments.FindAsync(appointmentId);
            var requestDate = DateOnly.FromDateTime(DateTime.Now);
            if (databaseAppointment.AppointmentDate.CompareTo(requestDate) >= 0)
            {
                databaseAppointment.Status = "Available";
                databaseAppointment.PatientId = null;
                _dbcontext.Appointments.Update(databaseAppointment);
                await _dbcontext.SaveChangesAsync();
                return Json(new { success = true });
            }
            else
            {
                ModelState.AddModelError("CancelError", "The appoinment has to be cancelled before the appointment date.");
                return Json(new { success = false, message = "No existe el turno" });
            }
        }
    }
}
