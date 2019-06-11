using System.Threading.Tasks;

namespace Serko.Expense.API.Interfaces
{
    public interface IApiAuthenticationService
    {
        Task<bool> IsValidToken(string apiKey);
    }
}
