namespace LeadToOpportunity.BLL.DTOs.AuditLogs;

public class AuditLogResponseDto
{
    public string EntityType { get; set; } = string.Empty;

    public int EntityId { get; set; }

    public string Action { get; set; } = string.Empty;

    public string FromStatus { get; set; } = string.Empty;

    public string ToStatus { get; set; } = string.Empty;

    public string Comment { get; set; } = string.Empty;

    public string ActorName { get; set; } = string.Empty;

    public DateTime Timestamp { get; set; }
}