using HealthManager.Models;
using HealthManager.Models.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HealthManager.Controllers
{
    [Authorize(Roles = "Doctor")]
    
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
            var today = DateOnly.FromDateTime(DateTime.Now);
            List<Appointment> patientList = await _dbcontext.Appointments
                .Where(x => x.AppointmentDate == today && x.Status == "Reserved" && x.Attended == null)
                .Include(a => a.Patient)
                .ToListAsync();
            return View(patientList);
        }

        public async Task <IActionResult> CreateRecord(Guid appointmentId)
        {
            Appointment appointment = await _dbcontext.Appointments
                .Where(x => x.AppointmentId == appointmentId)
                .Include(x => x.Doctor)
                .Include(x => x.Patient)
                .FirstOrDefaultAsync();

            MedicalRecordViewModel record = new MedicalRecordViewModel
            {
                AppointmentId = appointment.AppointmentId,
                DoctorId = appointment.DoctorId,
                PatientId = (int)appointment.PatientId,
                DoctorName = appointment.Doctor.Name + " " + appointment.Doctor.Surname,
                PatientName = appointment.Patient.Name + " " + appointment.Patient.Surname,
            };
            return View(record);
        }

        [HttpPost]
        public async Task<IActionResult> CreateRecord(MedicalRecordViewModel recordViewModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    Guid appointmentId = recordViewModel.AppointmentId;
                    MedicalRecord existingRecord = await _dbcontext.MedicalRecords.Where(x => x.AppointmentId == appointmentId).FirstOrDefaultAsync();
                    if (existingRecord == null)
                    {
                        MedicalRecord newRecord = new MedicalRecord
                        {
                            AppointmentId = appointmentId,
                            DoctorId = recordViewModel.DoctorId,
                            PatientId = recordViewModel.PatientId,
                            Date = DateTime.Now,
                            Diagnosis = recordViewModel.Diagnosis,
                            Observations = recordViewModel.Observations,
                            Treatment = recordViewModel.Treatment,
                        };

                        //EScribir logica para establecer que el paciente asistio a la consulta para que el turno ya no aparezca en la 
                        //Lista de pacientes para hoy
                        await _dbcontext.Appointments
                           .Where(x => x.AppointmentId == newRecord.AppointmentId)
                           .ExecuteUpdateAsync(setters => setters.SetProperty(b => b.Attended, true));

                        await _dbcontext.MedicalRecords.AddAsync(newRecord);

                        await _dbcontext.SaveChangesAsync();
                    }

                    return RedirectToAction("PatientTodayList", "Doctor");
                }
                else
                {
                    return View(recordViewModel);
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

                {
                })
                .ToListAsync();
        }

    }

}
