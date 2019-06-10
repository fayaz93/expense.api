using System;
using System.Collections.Generic;
using System.Text;

namespace Serko.Expense.Contracts
{
    public class ExpenseResponse
    {
        //Request - Incase if client wants it, response can send request back
        public ExpenseRequest Request { get; set; } = new ExpenseRequest();

        // Result
        public ExecutionResult ExecutionResult { get; set; } = new ExecutionResult();

        //Default is Unknown if CostCenter is not found in request
        public string CostCentre { get; set; } //"UNKNOWN";
    }
}
