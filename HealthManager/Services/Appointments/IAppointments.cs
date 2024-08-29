using HealthManager.Models;
using Microsoft.AspNetCore.Mvc;

namespace HealthManager.Services.Appointments
{
    public interface IAppointments
    {
        public Task<Appointment> GetAvailableAppointments();

        public Task<IActionResult> CreateAppointments();

    }
}
