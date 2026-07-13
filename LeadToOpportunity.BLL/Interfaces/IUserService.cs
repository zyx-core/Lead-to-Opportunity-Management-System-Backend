using LeadToOpportunity.BLL.DTOs.User;

namespace LeadToOpportunity.BLL.Interfaces;

public interface IUserService
{
    Task<IEnumerable<ManagerDto>> GetManagersAsync();
    Task<IEnumerable<UserResponseDto>> GetAllUsersAsync();

   Task<UserResponseDto> GetUserByIdAsync(int id);

Task<UserResponseDto> CreateUserAsync(CreateUserRequestDto request);

Task UpdateUserAsync(int id, UpdateUserRequestDto request);

   



}