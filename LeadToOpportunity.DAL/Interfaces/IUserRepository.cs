using LeadToOpportunity.Models.Entities;

namespace LeadToOpportunity.DAL.Interfaces;

public interface IUserRepository : IGenericRepository<User>
{
    Task<User?> GetByEmailAsync(string email);
    Task<IEnumerable<User>> GetManagerAsync();
    Task<IEnumerable<User>> GetEmployeeAsync();
}