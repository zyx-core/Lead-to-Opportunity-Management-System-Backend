using LeadToOpportunity.Models.Entities;

namespace LeadToOpportunity.DAL.Interfaces;

public interface IAuditLogRepository : IGenericRepository<AuditLog>
{
    Task<IEnumerable<AuditLog>>GetEntityHistoryAsync(
        string EntityType,
        int entityId
    );
}