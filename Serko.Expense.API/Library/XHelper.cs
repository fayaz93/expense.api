using System.Xml.Linq;

namespace Serko.Expense.API.Library
{
    public class XHelper
    {
        public static readonly XName Expense = "expense";
        public static readonly XName CostCentre = "cost_centre";
        public static readonly XName Total = "total";
        public static readonly XName PaymentMethod = "payment_method";
    }
}