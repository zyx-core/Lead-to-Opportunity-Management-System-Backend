using LeadToOpportunity.Models.Entities;

namespace LeadToOpportunity.DAL.Interfaces;

public interface IUserRepository : IGenericRepository<User>
{
    Task<User?> GetByEmailAsync(string email);
    Task<IEnumerable<User>> GetManagersAsync();
    Task<IEnumerable<User>> GetEmployeesAsync();
    Task<(IEnumerable<User> Items, int TotalCount)> GetPagedUsersAsync(int pageNumber, int pageSize);
}