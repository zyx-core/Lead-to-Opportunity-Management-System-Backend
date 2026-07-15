using System.Runtime.CompilerServices;
using System.Security.Claims;

using LeadToOpportunity.BLL.DTOs.Leads;
using LeadToOpportunity.BLL.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LeadToOpportunity.API.Controllers;


[ApiController]
[Route("api/leads")]
[Authorize(Roles =  "Employee")]
public class LeadController : ControllerBase
{
    private readonly ILeadService _leadService;
    public LeadController(ILeadService leadService)
    {
        _leadService = leadService;
    }

    //helper method
    private int GetEmployeeId()
    {
    return int.Parse(
        User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
    }
    [HttpPost]
    public async Task<IActionResult> Create(CreateLeadRequestDto request)
    {
        var lead = await _leadService.CreateLeadAsync(
            request,
            GetEmployeeId()
        );

        return Ok(lead);
    }
    [HttpGet]
public async Task<IActionResult> GetMyLeads([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 5)
{
    var pagedResult = await _leadService.GetMyLeadAsync(GetEmployeeId(), pageNumber, pageSize);

    return Ok(pagedResult);
}

    [HttpGet("{id}")]
    public async Task<IActionResult> GetLead(int id)
    {
        var lead = await _leadService.GetLeadByIdAsync(
            id,
            GetEmployeeId()
        );

        return Ok(lead);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult>update(
        int id,UpdateLeadRequestDto request
    )
    {
        await _leadService.UpdateLeadAsync(
            id,GetEmployeeId(),request
        );
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult>Delete(int id)
    {
        await _leadService.DeleteLeadAsync(id,GetEmployeeId());
        return NoContent();
    }

    [HttpPost("{id}/submit")]
    public async Task<IActionResult> Submit(
        int id,
        SubmitLeadRequestDto request
    )
    {
        await _leadService.SubmitLeadAsync(id,
        GetEmployeeId(), request);

        return NoContent();
    }
}