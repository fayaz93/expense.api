using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;

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