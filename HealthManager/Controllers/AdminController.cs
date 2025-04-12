using HealthManager.Services.Appointments;
using Microsoft.AspNetCore.Mvc;

namespace HealthManager.Controllers
{
    public class AdminController : Controller
    {
        private readonly IAppointments _appointmentsService;

        public AdminController(IAppointments appointmentsService)
        {
            _appointmentsService = appointmentsService;
        }
        public IActionResult AppointmentsManager()
        {
            return View();
        }

        public async Task <JsonResult> CreateAppointmentRegisters()
        [HttpGet]
        public IActionResult CreateDoctor()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateDoctor(DoctorRegistrationViewModel doctorRequest)
        {

            using var transaction = await _dbcontext.Database.BeginTransactionAsync();
            try
            {
                var existingRegisters = await _appointmentsService.CheckForExistingRegisters();
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

                if (existingRegisters.Success.Equals(true))
                WorkingDay newDoctorSchedule = new WorkingDay
                {
                    return Json(new {success = false, message="There are already appointments for this month" });
                }
                else
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
                    await _appointmentsService.CreateAppointments();
                    return Json(new { success = true, message = "Appointments were successfully created." });
                }
                
                
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
            catch (Exception error)
            catch (Exception)
            {
                return Json(new {success= false, message = error });
                await transaction.RollbackAsync();
                return View(doctorRequest);
            }
        }
    }
}
