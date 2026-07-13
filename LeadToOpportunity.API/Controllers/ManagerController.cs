using System.Security.Claims;
using LeadToOpportunity.BLL.DTOs.Leads;
using LeadToOpportunity.BLL.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LeadToOpportunity.API.Controllers;


[ApiController]
[Route("api/manager")]
[Authorize(Roles = "Manager")]
public class ManagerController : ControllerBase
{
    private readonly ILeadService _leadService;

    public ManagerController(ILeadService leadservice)
    {
        _leadService = leadservice;
    }

    private int GetManagerId()
    {
        return int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
    }

    [HttpGet("leads")]
    public async Task<IActionResult> GetAssignedLeads()
    {
        var leads =await _leadService.GetManagerLeadsAsync(GetManagerId());

        return Ok(leads);
    }

    [HttpPost("leads/{id}/approve")]
    public async Task<IActionResult> ApproveLead(int id)
    {
        await _leadService.ApproveLeadAsync(id,
        GetManagerId());
        return NoContent();
    }

    [HttpPost("leads/{id}/reject")]
    public async Task<IActionResult>RejectLead(
        int id, RejectLeadRequestDto request
    )
    {
        await _leadService.RejectLeadAsync(id,GetManagerId(),request);
        return NoContent();
    }

    [HttpPost("leads/{id}/request-modification")]
    public async Task<IActionResult> RequestModification(int id,
    RequestedModificationDto request)
    {
        await _leadService.RequestModificationAsync(id,
        GetManagerId(),request);

        return NoContent();
    }

    [HttpGet("leads/{id}")]
    public async Task<IActionResult> GetLead(int id)
    {
        var lead= await _leadService.GetManagerLeadByIdAsync(id,
        GetManagerId());

        if(lead == null)
        return NotFound();

        return Ok(lead);

    }
}