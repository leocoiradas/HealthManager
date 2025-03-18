
namespace HealthManager.Services.Appointments
{
    public class AppointmentsLifeCicle : IHostedLifecycleService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        public AppointmentsLifeCicle(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            //await _appointmentsService.CheckAndCreateAppointments();
            using (var scope = _scopeFactory.CreateScope())
            {
                var appointmentsService = scope.ServiceProvider.GetRequiredService<IAppointments>();
                await appointmentsService.CheckAndCreateAppointments();
            }

        }

        public Task StartedAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        public Task StartingAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        public Task StoppedAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        public Task StoppingAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
