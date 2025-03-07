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

        [HttpGet]
        public IActionResult CreateDoctor()
        {
            return View();
        }
        [HttpPost]
        public async Task <IActionResult> CreateDoctor(DoctorRegistrationViewModel doctorRequest)
        {
            
            using var transaction = await _dbcontext.Database.BeginTransactionAsync();
            try
            {
                Specialty newDoctorSpecialty = (Specialty)_dbcontext.Specialties.Where(x => x.SpecialtyId == doctorRequest.Specialty);
                Doctor newDoctor = new Doctor
                {
                    Name = doctorRequest.Name,
                    Surname = doctorRequest.Surname,
                    Specialty = doctorRequest.Specialty,
                    Email = doctorRequest.Email,
                    Password = BCrypt.Net.BCrypt.HashPassword(doctorRequest.Password),
                };
                await _dbcontext.Doctors.AddAsync(newDoctor);
                await _dbcontext.SaveChangesAsync();

                WorkingDay newDoctorSchedule = new WorkingDay
                {
                    DoctorId = newDoctor.DoctorId,
                    Monday = doctorRequest.Monday,
                    Tuesday = doctorRequest.Tuesday,
                    Wednesday = doctorRequest.Wednesday,
                    Thursday = doctorRequest.Thursday,
                    Friday = doctorRequest.Friday,
                    Saturday = doctorRequest.Saturday,
                    Sunday = doctorRequest.Sunday,
                };
                await _dbcontext.WorkingDays.AddAsync(newDoctorSchedule);
                await _dbcontext.SaveChangesAsync();

                DoctorShift newAppointmentInfo = new DoctorShift
                {
                    DoctorId = newDoctor.DoctorId,
                    ShiftStart = doctorRequest.WorkingHoursStart,
                    ShiftEnd = doctorRequest.WorkingHoursEnd,
                    ConsultDuration = doctorRequest.ConsultationDuration,
                };

                await _dbcontext.DoctorShifts.AddAsync(newAppointmentInfo);
                await _dbcontext.SaveChangesAsync();

                await transaction.CommitAsync();
            }
            catch (Exception)
            {

                await transaction.RollbackAsync();
            }
            
            return View();
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
