using Serko.Expense.Contracts;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Serko.Expense.API.Interfaces
{
    public interface IParser
    {
        Task<List<string>> GetXmlTagsAsync(string rawText);

        Task<ExpenseResponse> GetExpense(ExpenseResponse response, string expenseText);
    }
}
