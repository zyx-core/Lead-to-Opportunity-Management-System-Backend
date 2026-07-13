using LeadToOpportunity.BLL.DTOs.User;
using LeadToOpportunity.BLL.interfaces;
using LeadToOpportunity.BLL.Interfaces;
using LeadToOpportunity.DAL.Interfaces;
using LeadToOpportunity.Models.Entities;
using LeadToOpportunity.Shared.Exception;
using LeadToOpportunity.Shared.Exceptions;

namespace LeadToOpportunity.BLL.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordService _passwordService;

    public UserService(IUserRepository userRepository, IPasswordService passwordService)
    {
        _userRepository = userRepository;
        _passwordService = passwordService;

    }

    public async Task<IEnumerable<ManagerDto>> GetManagersAsync()
    {
        var managers = await _userRepository.GetManagersAsync();

        return managers.Select(m => new ManagerDto
        {
            Id =m.Id,
            FullName = $"{m.FirstName} {m.LastName}",
            Email = m.Email
        }
        );
        
    }

    public async Task<IEnumerable<UserResponseDto>> GetAllUsersAsync()
{
    var users = await _userRepository.GetAllAsync();

    return users.Select(u => new UserResponseDto
    {
        Id = u.Id,
        FirstName = u.FirstName,
        LastName = u.LastName,
        Email = u.Email,
        Role = u.Role,
        Region = u.Region
    });
}
public async Task<UserResponseDto> GetUserByIdAsync(int id)
{
    var user = await _userRepository.GetByIdAsync(id);

    if (user == null)
        throw new NotFoundException("User not found.");

    return new UserResponseDto
    {
        Id = user.Id,
        FirstName = user.FirstName,
        LastName = user.LastName,
        Email = user.Email,
        Role = user.Role,
        Region = user.Region
    };
}
public async Task<UserResponseDto> CreateUserAsync(
    CreateUserRequestDto request)
{
    var existingUser =
        await _userRepository.GetByEmailAsync(request.Email);

    if (existingUser != null)
        throw new ConflictException("User already exists.");

    var user = new User
    {
        FirstName = request.FirstName,
        LastName = request.LastName,
        Email = request.Email,
        Region = request.Region,
        Role = request.Role
    };

    user.PasswordHash =
        _passwordService.HashPassword(user, request.Password);

    await _userRepository.AddAsync(user);

    return new UserResponseDto
    {
        Id = user.Id,
        FirstName = user.FirstName,
        LastName = user.LastName,
        Email = user.Email,
        Role = user.Role,
        Region = user.Region
    };
}
public async Task UpdateUserAsync(
    int id,
    UpdateUserRequestDto request)
{
    var user = await _userRepository.GetByIdAsync(id);

    if (user == null)
        throw new NotFoundException("User not found.");

    user.FirstName = request.FirstName;
    user.LastName = request.LastName;
    user.Email = request.Email;
    user.Role = request.Role;
    user.Region = request.Region;

    await _userRepository.UpdateAsync(user);
}
}