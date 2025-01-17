using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalExpenseTracker.Services
{
    public class SessionService : ISessionService
    {
        private bool _isLoggedIn;
        private string _username;

        public void Login(string username)
        {
            _isLoggedIn = true;
            _username = username;
        }

        public void Logout()
        {
            _isLoggedIn = false;
            _username = null;
        }

        public bool IsLoggedIn()
        {
            return _isLoggedIn;
        }

        public string GetUsername()
        {
            return _username;
        }

    }
}
