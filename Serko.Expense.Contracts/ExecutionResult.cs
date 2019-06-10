using System;
using System.Collections.Generic;
using System.Text;

namespace Serko.Expense.Contracts
{
    public class ExecutionResult
    {
        //Status of the execution. Possible values are SUCCESS, FAILURE
        public string Status { get; set; } = "FAILURE";

        //Errors/Validations to client incase of FAILURE
        public List<string> Errors { get; set; } = new List<string>();
    }
}
