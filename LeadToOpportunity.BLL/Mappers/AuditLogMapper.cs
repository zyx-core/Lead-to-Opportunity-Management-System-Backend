using LeadToOpportunity.BLL.DTOs.AuditLogs;
using LeadToOpportunity.Models.Entities;

namespace LeadToOpportunity.BLL.Mappers;

public static class AuditLogMapper
{
    public static AuditLogResponseDto ToResponseDto(
        this AuditLog log)
    {
        return new AuditLogResponseDto
        {
            EntityType = log.EntityType,
            EntityId = log.EntityId,
            Action = log.Action,
            FromStatus = log.FromStatus,
            ToStatus = log.ToStatus,
            Comment = log.Comment,
            ActorName = $"{log.Actor.FirstName} {log.Actor.LastName}",
            Timestamp = log.CreatedAt
        };
    }
}