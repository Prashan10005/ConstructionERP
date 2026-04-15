using Microsoft.AspNetCore.Http;

namespace ConstructionERP.Services
{
    public class SessionService : ISessionService
    {
        private readonly IHttpContextAccessor _httpContextAccessor; // dependency declaration

        // injecting dependency via constructor
        public SessionService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public void SetUserSession(int userId, string username, string email, string role)
        {
            var session = _httpContextAccessor.HttpContext.Session; //access session
            session.SetString("UserId", userId.ToString());
            session.SetString("Username", username);
            session.SetString("Email", email);
            session.SetString("UserRole", role);
        }

        public void ClearSession()
        {
            _httpContextAccessor.HttpContext.Session.Clear();
        }

        public bool IsUserLoggedIn()
        {
            return _httpContextAccessor.HttpContext.Session.GetString("UserId") != null;
        }

        public string GetUserRole()
        {
            return _httpContextAccessor.HttpContext.Session.GetString("UserRole");
        }

        public string GetUsername()
        {
            return _httpContextAccessor.HttpContext.Session.GetString("Username");
        }

        public int? GetUserId()
        {
            var userId = _httpContextAccessor.HttpContext.Session.GetString("UserId");
            return userId != null ? int.Parse(userId) : null;
        }
    }
}
