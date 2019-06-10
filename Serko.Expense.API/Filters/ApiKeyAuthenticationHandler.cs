using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Serko.Expense.API.Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace Serko.Expense.API.Filters
{
    public class ApiKeyAuthenticationHandler : AuthenticationHandler<ApiKeyOptions>
    {
        private Interfaces.IApiAuthenticationService authenticationService;

        public ApiKeyAuthenticationHandler(
                IOptionsMonitor<ApiKeyOptions> options, 
                ILoggerFactory logger, 
                UrlEncoder encoder, 
                ISystemClock clock,
                Interfaces.IApiAuthenticationService authenticationService) : base(options, logger, encoder, clock)
        {
            this.authenticationService = authenticationService;
        }

        protected async override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            var headers = Request.Headers;
            var apiKey = headers[Constants.API_KEY_HEADER_NAME].FirstOrDefault();
            
            if (!await authenticationService.IsValidToken(apiKey))
            {
                return AuthenticateResult.Fail("ApiKey is missing or invalid.");
            }

            var claims = new[] { new Claim("token", apiKey) };
            var identity = new ClaimsIdentity(claims, nameof(ApiKeyAuthenticationHandler));
            var ticket = new AuthenticationTicket(new ClaimsPrincipal(identity), Scheme.Name);
            return AuthenticateResult.Success(ticket);
        }
    }
}
