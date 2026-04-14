namespace ConstructionERP.Services
{
    public interface ISessionService
    {
        void SetUserSession(int userId, string username, string email, string role);
        void ClearSession();
        bool IsUserLoggedIn();
        string GetUserRole();
        string GetUsername();
        int? GetUserId();

    }
}
