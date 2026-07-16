using System.Security.AccessControl;
using LeadToOpportunity.DAL.Data;
using LeadToOpportunity.DAL.Interfaces;
using LeadToOpportunity.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace LeadToOpportunity.DAL.Repositories;

public class AuditLogRepository : GenericRepository<AuditLog>, IAuditLogRepository
{
    public AuditLogRepository(AppDbContext context) : base(context)
    {
        
    }
    public async Task<IEnumerable<AuditLog>>GetEntityHistoryAsync(string entityType,
    int entityId)
    {
        return await _context.AuditLogs
            .Include(a => a.Actor)
            .Where(a =>
                a.EntityType == entityType &&
                a.EntityId == entityId)
            .OrderByDescending(a => a.CreatedAt)
            .ToListAsync();
    }
}