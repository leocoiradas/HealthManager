using HealthManager.Models;
using HealthManager.Models.DTO;
using HealthManager.Services.Appointments;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace HealthManager.Controllers
{
    public class AppointmentController : Controller
    {
        private readonly HealthManagerContext _dbcontext;
        private readonly IAppointments _appointmentsService;
        public AppointmentController(HealthManagerContext context, IAppointments appointmentsService)
        {
            _dbcontext = context;
            _appointmentsService = appointmentsService;
        }
        public IActionResult Index()
        {
            return View();
        }
        public async Task <IActionResult> ReserveAppointment()
        {
            var appointmentsList = await _dbcontext.Appointments
                .Where(a => a.Status == "Available")
                .Select(a => new AppointmentViewModel
                {
                    AppointmentId = a.AppointmentId,
                    AppointmentTime = a.AppointmentDate.ToString("dd/MM/yyyy") + " " + a.AppointmentHour.ToString("HH:mm") + " Hs."
                })
                .ToListAsync();
            ViewData["AppointmentsAvailable"] = new SelectList(appointmentsList, "AppointmentId", "AppointmentTime");

            var specialties = await _dbcontext.Doctors.Select(d => d.Specialty).Distinct().ToListAsync();
            ViewData["Specialties"] = new SelectList(specialties, specialties);
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> ReserveAppointment(AppointmentViewModel appointmentRequest)
        {
            if (ModelState.IsValid)
            {
                Appointment reserveAppointment = await _dbcontext.Appointments.FindAsync(appointmentRequest.AppointmentId);
                if (reserveAppointment != null)
                {
                    reserveAppointment.Status = "Reserved";
                    reserveAppointment.PatientId = appointmentRequest.PatientId;
                    _dbcontext.Appointments.Update(reserveAppointment);
                    await _dbcontext.SaveChangesAsync();

                }
                return View();
            }

            return View(appointmentRequest);
        }

        
        public async Task <JsonResult> GetAppointmentDates(int doctorId)
        {
            var currentMonth = DateTime.Now.Month;
            var currentDay = DateTime.Now.Day;
            var availableAppointments = await _dbcontext.Appointments
                .Where(a => a.DoctorId == doctorId && a.AppointmentDate.Month == currentMonth && a.AppointmentDate.Day > currentDay && a.Status == "Available")
                .Select(a => a.AppointmentDate.ToString("dd/MM/yyyy"))
                .Distinct()
                .ToListAsync();
            return Json(availableAppointments);
        }

        public async Task <JsonResult> GetAppointmentHours(string day, int doctorId)
        {
            var dateFromString = DateTime.Parse(day);
            var onlyDateFromDateTime = DateOnly.FromDateTime(dateFromString);
            var appointmentHours = await _dbcontext.Appointments
                .Where(a => a.DoctorId == doctorId && a.AppointmentDate.Equals(onlyDateFromDateTime) && a.Status == "Available" )
                .OrderBy(a => a.AppointmentHour)
                .Select(a => a.AppointmentHour.ToString("HH:mm"))
                .ToListAsync();
            return Json(appointmentHours);
        }
    }
}
