using Serko.Expense.API.Interfaces;
using Serko.Expense.API.Library;
using Serko.Expense.Contracts;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Serko.Expense.API.Services
{
    public class Parser : IParser
    {
        private readonly IValidator validator;

        public Parser(IValidator validator)
        {
            this.validator = validator;
        }

        public async Task<List<string>> GetXmlTagsAsync(string text)
        {
            var matches = new List<string>();

            await Task.Run(() =>
            {
                var regex = new Regex("<([^<>]+).*>.*</\\1>", RegexOptions.Singleline | RegexOptions.Multiline);
                var match = regex.Match(text);
                while (match.Success)
                {
                    matches.Add(match.Value);
                    match = match.NextMatch();
                }
            });

            if (!matches.Any())
                return null;

            return matches;
        }

        public async Task<ExpenseResponse> GetExpense(ExpenseResponse response, string expenseText)
        {
            var expenseDocument = XDocument.Parse(expenseText);
            var expenseElements = expenseDocument.Descendants();

            var validationResult = await validator.ValidateExpense(expenseDocument);
            response.ExecutionResult.Status = validationResult.IsValid ? Constants.SUCCESS : Constants.FAILED;
            response.ExecutionResult.Errors = validationResult.ErrorMessages;

            response.CostCentre = expenseElements?.FirstOrDefault(d => d.Name == XHelper.CostCentre)?.Value ?? "UNKNOWN";
            response.PaymentMethod = expenseElements?.FirstOrDefault(d => d.Name == XHelper.PaymentMethod)?.Value;

            decimal.TryParse(expenseElements?.FirstOrDefault(d => d.Name == XHelper.Total)?.Value, out decimal total);
            response.Total = response.TotalExcludingGST = total;
            return response;
        }
    }
}
