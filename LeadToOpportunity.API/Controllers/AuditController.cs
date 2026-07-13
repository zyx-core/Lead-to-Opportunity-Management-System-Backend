using LeadToOpportunity.BLL.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LeadToOpportunity.API.Controllers;

[ApiController]
[Route("api/audit")]
[Authorize]
public class AuditController : ControllerBase
{
    private readonly IAuditLogService _auditLogService;

    public AuditController(IAuditLogService auditLogService)
    {
        _auditLogService = auditLogService;
    }

    [HttpGet("{entityType}/{entityId}")]
    public async Task<IActionResult>GetHistory(
        string entityType,
        int entityId
    )
    {
        var history = await _auditLogService.GetEntityHistoryAsync(
            entityType,
            entityId
        );

        return Ok(history);
    }
}