using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Serko.Expense.API.Interfaces
{
    public interface IApiAuthenticationService
    {
        Task<bool> IsValidToken(string apiKey);
    }
}
