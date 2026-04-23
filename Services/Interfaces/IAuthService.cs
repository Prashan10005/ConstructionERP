using ConstructionERP.DTOs.Auth;

namespace ConstructionERP.Services.Interfaces
{
    public interface IAuthService
    {
        Task<LoginResponseDto> LoginAsync(LoginRequestDto request);
    }
}
