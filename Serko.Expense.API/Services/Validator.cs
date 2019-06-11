using Serko.Expense.API.Interfaces;
using Serko.Expense.API.Library;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace Serko.Expense.API.Services
{
    public class Validator: IValidator
    {
        public async Task<(bool IsValid, List<string> ErrorMessages)> ValidateXmlTags(List<string> tags)
        {
            var isValid = true;
            var errorMessages = new List<string>();

            await Task.Run(() =>
            {
                foreach (var xml in tags)
                {
                    try
                    {
                        XDocument.Parse(xml);
                    }
                    catch (XmlException)
                    {
                        isValid = false;
                        errorMessages.Add($"Invalid Xml in text. Received: {xml}");
                    }
                }
            });

            return (isValid, errorMessages);
        }

        public async Task<(bool IsValid, List<string> ErrorMessages)> ValidateExpense(XDocument expense)
        {
            var isValid = true;
            var errorMessages = new List<string>();

            await Task.Run(() =>
            {
                var totalValue = expense?.Descendants()?.FirstOrDefault(d => d.Name == XHelper.Total)?.Value;
                if(string.IsNullOrWhiteSpace(totalValue))
                {
                    isValid = false;
                    errorMessages.Add(Constants.MISSING_TOTAL);
                }

                var isTotalExists = decimal.TryParse(totalValue, out decimal total);
                if (isValid && !isTotalExists)
                {
                    isValid = false;
                    errorMessages.Add(Constants.INVALID_TOTAL);
                }

                var duplicates = expense.Descendants()
                                .GroupBy(e => e.ToString())
                                .Where(g => g.Count() > 1)
                                .Select(g => g.First())
                                .ToList();
                if (duplicates.Any())
                {
                    isValid = false;
                    errorMessages.Add(Constants.DUPLICATE_ELEMENTS);
                    errorMessages.AddRange(duplicates.Select(d => d.Name.ToString()));
                }
            });

            return (isValid, errorMessages);
        }

    }
}
