using LeadToOpportunity.BLL.DTOs.User;

namespace LeadToOpportunity.BLL.Interfaces;

public interface IUserService
{
    Task<IEnumerable<ManagerDto>> GetManagersAsync();
    Task<LeadToOpportunity.Shared.Pagination.PagedResult<UserResponseDto>> GetAllUsersAsync(int pageNumber, int pageSize);

   Task<UserResponseDto> GetUserByIdAsync(int id);

Task<UserResponseDto> CreateUserAsync(CreateUserRequestDto request);

Task UpdateUserAsync(int id, UpdateUserRequestDto request);

   



}