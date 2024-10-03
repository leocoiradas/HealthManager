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
            return View();
        }
        [HttpPut]
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

        public async Task <JsonResult> GetDoctorsBySpecialty(string specialty)
        {
            var doctorsBySpecialty = await _dbcontext.Doctors.Where(d => d.Specialty == specialty)
                .Select(a => a.Name + " " + a.Surname)
                .ToListAsync();
            return Json(doctorsBySpecialty);
        }

        public async Task <JsonResult> GetAppointmentDates(int doctorId)
        {
            var currentMonth = DateTime.Now.Month;
            var availableAppointments = await _dbcontext.Appointments
                .Where(a => a.DoctorId == doctorId && a.AppointmentDate.Month == currentMonth && a.Status == "Available")
                .Select(a => a.AppointmentDate.ToString("dd-MM-yyyy"))
                .ToListAsync();
            return Json(availableAppointments);
        }

        public async Task <JsonResult> GetAppointmentHours(DateOnly day, int doctorId)
        {
            var appointmentHours = await _dbcontext.Appointments
                .Where(a => a.DoctorId == doctorId && a.AppointmentDate == day && a.Status == "Available" )
                .Select(a => a.AppointmentHour)
                .ToListAsync();
            return Json(appointmentHours);
        }
    }
}
