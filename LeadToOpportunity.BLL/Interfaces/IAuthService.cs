using LeadToOpportunity.BLL.DTOs.Auth;

namespace LeadToOpportunity.BLL.Interfaces;

public interface IAuthService
{
    Task RegisterAsync(RegisterRequestDto request);

    Task<LoginResponseDto> LoginAsync(LoginRequestDto request);
}