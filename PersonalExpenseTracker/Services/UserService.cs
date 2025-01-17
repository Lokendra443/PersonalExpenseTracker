using PersonalExpenseTracker.Model;
using SQLite;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalExpenseTracker.Services
{
    public class UserService : IUserService
    {
        string dbPath = @"C:\Users\loken\Desktop\db.db3";
        private SQLiteConnection _conn;

        public UserService()
        {
            _conn = new SQLiteConnection(dbPath);  // Ensure connection is established
            creatUserTable();  // Initialize the database connection and create table if necessary
        }

        public void creatUserTable()
        {
            // Only create the table if it doesn't exist already
            _conn.CreateTable<User>();
        }

        public User Login(string username, string password)
        {
            var user = _conn.Table<User>().FirstOrDefault(u => u.Username == username && u.Password == password);
            if (user == null)
            {
                throw new Exception("Invalid username or password");
            }
            return user; // Return the logged-in user
        }

        // Register a new user
        public string Register(string username, string email, string phoneNo, string preferredCurrency, string password)
        {
            // Check if the username or email alrady exists
            var existingUser = _conn.Table<User>().FirstOrDefault(u => u.Username == username || u.Email == email);
            if (existingUser != null)
            {
                // If a user with the same username or email exists, return an error message
                return "Username or email already exists";
            }

            // If user is not exists, create a new user and insert it into the database
            var newUser = new User
            {
                Username = username,
                Email = email,
                PhoneNo = phoneNo,
                PreferredCurrency = preferredCurrency,
                Password = password
            };

            _conn.Insert(newUser); // Insert the new user into the database

            return "User registered successfully.";
        }
    }
}
