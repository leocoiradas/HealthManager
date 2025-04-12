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

            try
            {
                var existingRegisters = await _appointmentsService.CheckForExistingRegisters();

                if (existingRegisters.Success.Equals(true))
                {
                    return Json(new {success = false, message="There are already appointments for this month" });
                }
                else
                {
                    await _appointmentsService.CreateAppointments();
                    return Json(new { success = true, message = "Appointments were successfully created." });
                }
                
                
            }
            catch (Exception error)
            {
                return Json(new {success= false, message = error });
                
            }
           
        }
    }
}
