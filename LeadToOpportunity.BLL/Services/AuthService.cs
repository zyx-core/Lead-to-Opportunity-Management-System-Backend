using LeadToOpportunity.BLL.DTOs.Auth;
using LeadToOpportunity.BLL.interfaces;
using LeadToOpportunity.BLL.Interfaces;
using LeadToOpportunity.DAL.Interfaces;
using LeadToOpportunity.Models.Entities;
using LeadToOpportunity.Models.Enums;
using LeadToOpportunity.Shared.Exceptions;

namespace LeadToOpportunity.BLL.Services;

public class AuthService : IAuthService
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordService _passwordService;
    private readonly IJwtService _jwtService;

    public AuthService(
        IUserRepository userRepository,
        IPasswordService passwordService,
        IJwtService jwtService
    )
    {
        _userRepository = userRepository;
        _passwordService = passwordService;
        _jwtService = jwtService;
    }

    public async Task RegisterAsync(RegisterRequestDto request)
    {
        var existingUser = await _userRepository.GetByEmailAsync(request.Email);
        if (existingUser != null)
        {
            throw new ConflictException("user already exist");
        }
        var user = new User
        {
             FirstName = request.FirstName,
             LastName = request.LastName,
             Email = request.Email,
            Region = request.Region,
             Role = UserRole.Employee
        };

        user.PasswordHash = _passwordService.HashPassword(user, request.Password);

        await _userRepository.AddAsync(user);
    }

    public async Task<LoginResponseDto> LoginAsync(LoginRequestDto request)
    {
        var user = await _userRepository.GetByEmailAsync(request.Email);
        if (user == null)
        {
            throw new UnauthorizedException("email or password is worng");
        }
        var isPasswordValid = _passwordService.VerifyPassword(user,
        request.Password, user.PasswordHash);

        if (!isPasswordValid)
        {
            throw new UnauthorizedException("email or password is worng");
        }
        var token = _jwtService.GenerateToken(user);

        return new LoginResponseDto
        {
            Token = token,
            Email = user.Email,
            Role = user.Role.ToString()
        };
    }
}