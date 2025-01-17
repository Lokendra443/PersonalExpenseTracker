using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalExpenseTracker.Model
{
    public class LoginModel
    {
        [Required]
        public string Username { get; set; }

        [Required]
        [MinLength(6, ErrorMessage = "Password must be at least 6 characters long")]
        public string Password { get; set; }

        [Required]
        public string PreferredCurrency { get; set; }
    }
}
