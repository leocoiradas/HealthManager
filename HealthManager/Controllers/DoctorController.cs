using HealthManager.Models;
using HealthManager.Models.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HealthManager.Controllers
{
    //[Authorize(Roles = "Doctor")]
    
    public class DoctorController : Controller
    {
        private readonly HealthManagerContext _dbcontext;
        public DoctorController(HealthManagerContext context)
        {
            _dbcontext = context;
        }

        public IActionResult Doctors()
        {
            List<Doctor> doctorList = _dbcontext.Doctors.ToList();
            return View(doctorList);
        }

        [HttpGet]
        public async Task<IActionResult> PatientTodayList()
        {
            List<Appointment> patientList = await _dbcontext.Appointments
                .Where(x => x.AppointmentDate == DateOnly.FromDateTime(DateTime.Now))
                .ToListAsync();
            return View(patientList);
        }

        public async Task<JsonResult> GetDoctorsBySpecialty(int specialty)
        {
            var doctorsBySpecialty = await _dbcontext.Doctors.Where(d => d.Specialty == specialty)
                .Select(a => new
                {
                    DoctorId = a.DoctorId,
                    Name = a.Name + " " + a.Surname,
                })
                .ToListAsync();
            return Json(doctorsBySpecialty);
        }

    }

}
