using LeadToOpportunity.BLL.DTOs.AuditLogs;
using LeadToOpportunity.BLL.Interfaces;
using LeadToOpportunity.BLL.Mappers;
using LeadToOpportunity.DAL.Interfaces;
using LeadToOpportunity.Models.Entities;

namespace LeadToOpportunity.BLL.Services;

public class AuditLogService : IAuditLogService
{
    private readonly IAuditLogRepository _auditRepository;

    public AuditLogService(
        IAuditLogRepository auditRepository)
    {
        _auditRepository = auditRepository;
    }

    public async Task LogAsync(
        string entityType,
        int entityId,
        int actorId,
        string action,
        string fromStatus,
        string toStatus,
        string comment = "")
    {
        var log = new AuditLog
        {
            EntityType = entityType,
            EntityId = entityId,
            ActorId = actorId,
            Action = action,
            FromStatus = fromStatus,
            ToStatus = toStatus,
            Comment = comment
        };

        await _auditRepository.AddAsync(log);
    }

    public async Task<IEnumerable<AuditLogResponseDto>>
        GetEntityHistoryAsync(
        string entityType,
        int entityId)
    {
        var logs = await _auditRepository
            .GetEntityHistoryAsync(entityType, entityId);

        return logs.Select(l => l.ToResponseDto());
    }
}