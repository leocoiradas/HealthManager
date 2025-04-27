using HealthManager.Models.DTO;
using HealthManager.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace HealthManager.Controllers
{
    public class AdminController : Controller
    {
        private readonly HealthManagerContext _dbcontext;

        public AdminController(HealthManagerContext context)
        {
            _dbcontext = context;
        }
        {
        }

        [HttpGet]
        public async Task<IActionResult> CreateDoctor()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateDoctor(DoctorRegistrationViewModel doctorRequest)
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
                return View();
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                return View(doctorRequest);
            }
        }
    }
}
