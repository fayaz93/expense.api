using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Serko.Expense.API.Interfaces;
using Serko.Expense.API.Library;
using Serko.Expense.Contracts;

namespace Serko.Expense.API.Controllers
{
    [Authorize(AuthenticationSchemes = Constants.AUTHENTICATION_SCHEME)]
    [Route("api/v1/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly IValidator validator;
        private readonly IParser parser;

        public PaymentController(IValidator validator, IParser parser)
        {
            this.validator = validator;
            this.parser = parser;
        }

        [HttpPost]
        [Route("expense/import")]
        public async Task<ExpenseResponse> CreateExpense(ExpenseRequest request)
        {
            var response = new ExpenseResponse();

            await Task.Run(() => {
                response.Request = request;
            });

            return response;
        }
    }
}
