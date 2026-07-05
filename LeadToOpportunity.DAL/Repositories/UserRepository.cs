using LeadToOpportunity.DAL.Data;
using LeadToOpportunity.DAL.Interfaces;
using LeadToOpportunity.Models.Entities;
using LeadToOpportunity.Models.Enums;
using Microsoft.EntityFrameworkCore;

namespace LeadToOpportunity.DAL.Repositories;

public class UserRepository : GenericRepository<User>, IUserRepository
{
    public UserRepository(AppDbContext context)
        : base(context)
    {
    }
    public async Task<User?> GetByEmailAsync(string email)
    {
        return await _dbSet.FirstOrDefaultAsync(u=>u.Email == email);
    }

    public async Task<IEnumerable<User>> GetManagerAsync()
    {
        return await _dbSet.Where(u=>u.Role==UserRole.Manager)
        .ToListAsync();
    }
    public async Task<IEnumerable<User>> GetEmployeeAsync()
    {
        return await _dbSet
        .Where(u=>u.Role==UserRole.Employee)
        .ToListAsync();
    }
}