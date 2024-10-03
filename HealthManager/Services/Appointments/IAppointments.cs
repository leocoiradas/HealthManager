using HealthManager.Models;
using HealthManager.Models.DTO;
using Microsoft.AspNetCore.Mvc;

namespace HealthManager.Services.Appointments
{
    public interface IAppointments
    {
        public Task<MethodResponse> CheckForExistingRegisters();

        public Task<MethodResponse> CreateAppointments();

    }
}
