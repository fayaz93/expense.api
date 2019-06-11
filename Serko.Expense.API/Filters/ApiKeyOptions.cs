using Microsoft.AspNetCore.Authentication;
using System.Collections.Generic;

namespace Serko.Expense.API.Filters
{
    public class ApiKeyOptions : AuthenticationSchemeOptions
    {
        public IReadOnlyList<string> ApiKeys { get; set; }

        public ApiKeyOptions()
        {
            ApiKeys = new List<string>();
        }
    }
}