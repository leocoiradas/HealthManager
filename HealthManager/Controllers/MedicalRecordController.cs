using HealthManager.Models.DTO;
using HealthManager.Models;
using Microsoft.AspNetCore.Mvc;

namespace HealthManager.Controllers
{
    public class MedicalRecordController : Controller
    {
        private readonly HealthManagerContext _dbcontext;
        public MedicalRecordController(HealthManagerContext context)
        {
            _dbcontext = context;
        }
        public IActionResult Index() { 
            return View();
        }

        [HttpGet]
        public IActionResult CreateRecord(MedicalRecord record)
        {
            MedicalRecordViewModel recordViewModel = new MedicalRecordViewModel
            {
                DoctorId = record.DoctorId,
                DoctorName =record.Doctor.Name + record.Doctor.Surname,
                PatientId = record.PatientId,
                PatientName = record.Patient.Name + record.Patient.Surname,
            };
            return View(recordViewModel);
        }

        [HttpPost]
        public async Task <IActionResult> CreateRecord(MedicalRecordViewModel model)
        {
            if (ModelState.IsValid)
            {
                MedicalRecord newRecord = new MedicalRecord
                {
                    Id = new Guid(),
                    AppointmentId = model.AppointmentId,
                    PatientId = model.PatientId,
                    DoctorId = model.DoctorId,
                    Date = DateTime.UtcNow,
                    Diagnosis = model.Diagnosis,
                    Observations = model.Observations,
                    Treatment = model.Treatment ?? "No Treatment Required.",
                };

                await _dbcontext.MedicalRecords.AddAsync(newRecord);

                return RedirectToAction("PatientTodayList", "Doctor");
            }
            else
            {
                
                return View(model);
            }
        }
    }
}