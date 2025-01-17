using PersonalExpenseTracker.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalExpenseTracker.Services
{
    public interface IUserService
    {
       void creatUserTable();

        string Register(string username, string email, string phoneNo, string preferredCurrency, string password);
        User Login(string username, string password);
    }
}
