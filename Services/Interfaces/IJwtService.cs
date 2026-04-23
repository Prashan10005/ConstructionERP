using System.Security.Claims;

namespace ConstructionERP.Services.Interfaces
{
    public interface IJwtService
    {
        string GenerateToken(int userId, string username, string email, string role);
        ClaimsPrincipal ValidateToken(string token);
    }
}