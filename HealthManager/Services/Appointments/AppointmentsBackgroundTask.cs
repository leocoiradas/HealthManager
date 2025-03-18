using HealthManager.Models.DTO;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace HealthManager.Services.Appointments
{
    public class AppointmentsBackgroundTask: BackgroundService
    {
        
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ILogger<AppointmentsBackgroundTask> _logger;

        public AppointmentsBackgroundTask(IServiceScopeFactory scopeFactory , ILogger<AppointmentsBackgroundTask> logger)
        {
            _logger = logger;
            _scopeFactory = scopeFactory;
        }


        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
               
                //await _appointmentsService.CheckAndCreateAppointments();
                await Task.Delay(TimeSpan.FromDays(30), stoppingToken);
                using (var scope = _scopeFactory.CreateScope())
                {
                    var appointmentsService = scope.ServiceProvider.GetRequiredService<IAppointments>();
                    await appointmentsService.CheckAndCreateAppointments();
                    _logger.LogInformation("Turnos creados a las: {time}", DateTimeOffset.Now);
                }
            }
        }
    }
}
