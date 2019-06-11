using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Serko.Common.Log;
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
        private readonly IConfiguration configuration;
        private readonly IValidator validator;
        private readonly IParser parser;
        private readonly ILogger logger;

        public PaymentController(IConfiguration configuration, IValidator validator, IParser parser, ILogger logger)
        {
            this.configuration = configuration;
            this.validator = validator;
            this.parser = parser;
            this.logger = logger;
        }

        [HttpPost]
        [Route("expense/import")]
        public async Task<ExpenseResponse> CreateExpense(ExpenseRequest request)
        {
            var response = new ExpenseResponse();
            response.Request = request;

            // Get all XML tags. If no XML tags, return error
            logger.LogInformation<PaymentController>("Extracting XML tags");
            var tags = await parser.GetXmlTagsAsync(request.RawText);
            if(tags == null || !tags.Any())
            {
                response.ExecutionResult = new ExecutionResult
                {
                    Status = Constants.FAILED,
                    Errors = new List<string> { Constants.NO_XML_TAGS }
                };
                return response;
            }

            // Find "expense" tag. If no expense, return error
            logger.LogInformation<PaymentController>("Finding Expense tag");
            var expense = tags.FirstOrDefault(t => t.Length > 9 && t.Substring(0, 9) == Constants.EXPENSE_TAG);
            if (string.IsNullOrWhiteSpace(expense))
            {
                response.ExecutionResult = new ExecutionResult
                {
                    Status = Constants.FAILED,
                    Errors = new List<string> { Constants.MISSING_EXPENSE }
                };
                return response;
            }

            // Validate XML for its format
            logger.LogInformation<PaymentController>("Validating all XML tags");
            var xmlValidator = await validator.ValidateXmlTags(tags);
            if (!xmlValidator.IsValid)
            {
                response.ExecutionResult = new ExecutionResult
                {
                    Status = Constants.FAILED,
                    Errors = new List<string>(xmlValidator.ErrorMessages)
                };
                return response;
            }

            // Parse "expense"
            logger.LogInformation<PaymentController>("Parsing Expense");
            response = await parser.GetExpense(response, expense);
            if (decimal.TryParse(configuration[Configuration.GST], out decimal gst))
            {
                response.TotalExcludingGST = Math.Round(response.Total - (response.Total * gst) / 100, Configuration.RoundingDecimals);
            }
            return response;
        }
    }
}
