using Microsoft.Extensions.Configuration;
using Serko.Expense.API.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Serko.Expense.API.Services
{
    public class ApiAuthenticationService : IApiAuthenticationService
    {
        private readonly IConfiguration configuration;

        public ApiAuthenticationService(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public async Task<bool> IsValidToken(string apiKey)
        {
            //You can save apiKey in appsettings, database with encryption/hash. Custom logic here.
            return await Task.Run(() => apiKey == configuration["ApiKey"]);
        }
    }
}
