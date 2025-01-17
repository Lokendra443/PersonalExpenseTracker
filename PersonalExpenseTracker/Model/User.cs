using SQLite;
using System.ComponentModel.DataAnnotations;


namespace PersonalExpenseTracker.Model
{
    public class User
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        [Required]
        public string Username { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string PhoneNo { get; set; }

        [Required]
        public string PreferredCurrency { get; set; }


        [Required]
        [MinLength(6)]
        public string Password { get; set; }


    }
}
