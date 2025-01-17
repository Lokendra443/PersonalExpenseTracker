using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalExpenseTracker.Model
{
    public class Debt
    {
        public string Title { get; set; }  // Name or title of the debt (e.g., "Car Loan")
        public decimal Amount { get; set; } // Amount of the debt
        public DateTime DueDate { get; set; } // Due date for the debt

        public string Source { get; set; } // Source of the debt (e.g., loan type, credit card)
        public string Status { get; set; } // Status of the debt (e.g., "Pending", "Cleared")

    }
}
