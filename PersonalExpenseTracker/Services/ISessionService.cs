namespace PersonalExpenseTracker.Services
{
    public interface ISessionService
    {
        void Login(string username);
        void Logout();
        bool IsLoggedIn();
        string GetUsername();
    }
}
