using ExpressiveAnnotations.Attributes;
using SQLite;
using System.ComponentModel.DataAnnotations;



namespace PersonalExpenseTracker.Model
{
    public class Transaction
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Amount must be greater than 0.")]
        public decimal Amount { get; set; }

        [Required]
        public string Title { get; set; } = string.Empty;

        [Required]
        public string Type { get; set; } = string.Empty ;

        [Required]
        public DateTime Date { get; set; }

        
        public string? Notes { get; set; }


        // New fields for Debt transactions
        
        public string? SourceOfDebt { get; set; } // 

        
        public DateTime? DueDate { get; set; } // Nullable for Credit/Debit
        public string? Status { get; set; }
        


        // Navigation property for the many-to-many relationship
        [Ignore]
        public List<Tag> Tags { get; set; } = new List<Tag>();

    }
}
