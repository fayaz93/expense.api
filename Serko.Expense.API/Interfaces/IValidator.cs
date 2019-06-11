using System.Collections.Generic;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Serko.Expense.API.Interfaces
{
    public interface IValidator
    {
        Task<(bool IsValid, List<string> ErrorMessages)> ValidateXmlTags(List<string> tags);

        Task<(bool IsValid, List<string> ErrorMessages)> ValidateExpense(XDocument expense);
    }
}
