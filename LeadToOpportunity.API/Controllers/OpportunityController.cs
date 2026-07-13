using System.Security.Claims;
using LeadToOpportunity.BLL.DTOs.Opportunities;
using LeadToOpportunity.Shared.Exception;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LeadToOpportunity.API.Controllers;

[ApiController]
[Route("api/opportunities")]
[Authorize(Roles ="Manager")]
public class OpportunityController : ControllerBase
{
    private readonly IOpportunityService _opportunityService;


    public OpportunityController(IOpportunityService opportunityService)
    {
        _opportunityService = opportunityService;
    }
     private int GetManagerId()
    {
        return int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var opportunity = await _opportunityService.GetAllAsync();

        return Ok(opportunity);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var opportunity = await _opportunityService.GetByIdAsync(id);

        if(opportunity == null)
        {
            return NotFound();
        }
        return Ok(opportunity);
    }

    [HttpPut("{id}/stage")]
    public async Task<IActionResult> UpdateStage(int id,UpdateOpportunityStageDto request)
    {
        await _opportunityService.UpdateStageAsync(id,GetManagerId(), request);

        return NoContent();
    }

    [HttpPost("{id}/won")]
    public async Task<IActionResult> MarkWon(int id)
    {
        await _opportunityService.MarkWonAsync(id,GetManagerId());
        return NoContent();
    }

    [HttpPost("{id}/lost")]

    public async Task<IActionResult> MarkLost(int id)
    {
        await _opportunityService.MarkLostAsync(id,GetManagerId());

        return NoContent();
    }
    }