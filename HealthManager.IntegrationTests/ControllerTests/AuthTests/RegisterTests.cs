using HealthManager.Models;
using HealthManager.Models.DTO;
using HealthManager.Services.Authentication;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using HealthManagerIntegrationTests.Helpers;

namespace HealthManagerIntegrationTests.ControllerTests.AuthTests
{
    public class RegisterTests : IClassFixture<CustomWebApplicationEnv>
    {
        private HttpClient _httpClient;
        private readonly HealthManagerContext _dbContext;
        private readonly IJWTService _tokenService;

        public RegisterTests(CustomWebApplicationEnv custom)
        {
            _httpClient = custom.CreateClient(new Microsoft.AspNetCore.Mvc.Testing.WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false
            });
            var scope = custom.Services.CreateScope();
            _dbContext = scope.ServiceProvider.GetRequiredService<HealthManagerContext>();
            _tokenService = scope.ServiceProvider.GetRequiredService<IJWTService>();
        }

    }
}
