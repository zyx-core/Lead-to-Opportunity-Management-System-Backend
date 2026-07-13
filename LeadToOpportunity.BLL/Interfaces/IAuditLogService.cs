using LeadToOpportunity.BLL.DTOs.AuditLogs;

namespace LeadToOpportunity.BLL.Interfaces;

public interface IAuditLogService
{
    Task LogAsync(
        string entityType,
        int entityId,
        int actorId,
        string action,
        string fromStatus,
        string toStatus,
        string comment = "");

    Task<IEnumerable<AuditLogResponseDto>> GetEntityHistoryAsync(
        string entityType,
        int entityId);
}