namespace Serko.Expense.API.Library
{
    public class Constants
    {
        public const string API_KEY_HEADER_NAME     = "SerkoExpenseApiKey";
        public const string AUTHENTICATION_SCHEME   = "SerkoExpenseAuthenticationScheme";
        public const string SUCCESS                 = "Success";
        public const string FAILED                  = "Failed";
        public const string NO_XML_TAGS             = "Given text does not contain any XML elements!";
        public const string MISSING_EXPENSE         = "Expense XML is missing from given text.";
        public const string MULTIPLE_EXPENSE        = "Multiple 'expense' found.";
        public const string MISSING_TOTAL           = "Expense does not have a total.";
        public const string INVALID_TOTAL           = "Given 'total' is not valid.";
        public const string DUPLICATE_ELEMENTS      = "Expense has duplicate xml elements.";
        public const string EXPENSE_TAG             = "<expense>";
    }
}
