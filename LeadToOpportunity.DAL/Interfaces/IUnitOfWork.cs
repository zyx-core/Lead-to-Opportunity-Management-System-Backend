using Microsoft.EntityFrameworkCore.Storage;

namespace LeadToOpportunity.DAL.Interfaces;

public interface IUnitOfWork
{
    Task<IDbContextTransaction> BeginTransactionAsync();

    Task<int> SaveChangesAsync();
}