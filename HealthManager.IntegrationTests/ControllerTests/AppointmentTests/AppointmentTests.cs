using HealthManager.Models;
using HealthManager.Models.DTO;
using HealthManager.Services.Authentication;
using HealthManagerIntegrationTests;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace HealthManagerIntegrationTests.ControllerTests.AppointmentTests
{
    public class AppointmentTests : IClassFixture<CustomWebApplicationEnv>
    {
        private HttpClient _httpClient;
        private readonly HealthManagerContext _dbContext;
        private readonly IJWTService _tokenService;
        public AppointmentTests(CustomWebApplicationEnv custom)
        {
            _httpClient = custom.CreateClient();
            var scope = custom.Services.CreateScope();
            _dbContext = scope.ServiceProvider.GetRequiredService<HealthManagerContext>();
            _tokenService = scope.ServiceProvider.GetRequiredService<IJWTService>();
        }

    }
}
