using System.Security.Claims;

namespace ConstructionERP.Services
{
    public interface IJwtService 
    {
        string GenerateToken(int userId, string username, string email, string role);
        ClaimsPrincipal ValidateToken(string token);
        int? GetUserIdFromToken(string token);
        string GetRoleFromToken(string token);

        string GetUsernameFromToken(string token);
    }
}
