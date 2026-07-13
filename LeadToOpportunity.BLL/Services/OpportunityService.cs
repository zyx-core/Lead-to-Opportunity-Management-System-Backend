using System.Net.Cache;
using System.Threading.Tasks.Dataflow;
using LeadToOpportunity.BLL.DTOs;
using LeadToOpportunity.BLL.DTOs.Opportunities;
using LeadToOpportunity.BLL.Interfaces;
using LeadToOpportunity.DAL.Interfaces;
using LeadToOpportunity.Models.Enums;

namespace LeadToOpportunity.BLL.Services;

public class OpportunityService : IOpportunityService
{
    private readonly IOpportunityRepository _opportunityRepository;
    private readonly IAuditLogService _auditLogService;

    public OpportunityService(IOpportunityRepository opportunityRepository,
    IAuditLogService auditLogService)
    {
        _opportunityRepository = opportunityRepository;
        _auditLogService = auditLogService;
    }

    public async Task<IEnumerable<OpportunityResponseDto>> GetAllAsync()
    {
        var opportunities = await _opportunityRepository.GetAllWithLeadAsync();

        return opportunities.Select(o => new OpportunityResponseDto
        {
            id = o.Id,
            LeadId = o.LeadId,
            CompanyName = o.Lead.CompanyName,
           Stage = o.Stage,
           EstimatedValue = o.EstimatedValue,
           ExpectedClosureDate = o.ExpectedClosureDate

        });
    }
    public async Task<OpportunityResponseDto?> GetByIdAsync(int id)
{
    var opportunity = await _opportunityRepository.GetWithLeadAsync(id);

    if (opportunity == null)
        return null;

    return new OpportunityResponseDto
    {
        id = opportunity.Id,
        LeadId = opportunity.LeadId,
        CompanyName = opportunity.Lead.CompanyName,
        Stage = opportunity.Stage,
        EstimatedValue = opportunity.EstimatedValue,
        ExpectedClosureDate = opportunity.ExpectedClosureDate
    };
}
public async Task UpdateStageAsync(
    int id,int managerId,
    UpdateOpportunityStageDto request)
{
    var opportunity = await _opportunityRepository.GetByIdAsync(id);

    if (opportunity == null)
        throw new Exception("Opportunity not found.");
    var oldStage = opportunity.Stage;

    opportunity.Stage = request.Stage;


    await _opportunityRepository.UpdateAsync(opportunity);
    await _auditLogService.LogAsync(
    "Opportunity",
    opportunity.Id,
    managerId,
    "Stage Updated",
    oldStage.ToString(),
    opportunity.Stage.ToString()
);
    
}
public async Task MarkWonAsync(int id,int managerId)
{
    var opportunity = await _opportunityRepository.GetByIdAsync(id);

    if (opportunity == null)
        throw new Exception("Opportunity not found.");
        var oldStage = opportunity.Stage;
    
if (opportunity.Stage == OpportunityStage.Won || opportunity.Stage == OpportunityStage.Lost)
    {
        // 2. Prevent changing it if it's already closed
        throw new Exception($"Cannot change the stage because the opportunity is already {opportunity.Stage}.");
    }
    opportunity.Stage = Models.Enums.OpportunityStage.Won;

    await _opportunityRepository.UpdateAsync(opportunity);
    await _auditLogService.LogAsync(
    "Opportunity",
    opportunity.Id,
    managerId,
    "Stage Updated",
    oldStage.ToString(),
    opportunity.Stage.ToString()
);
}
public async Task MarkLostAsync(int id,int managerId)
{
    var opportunity = await _opportunityRepository.GetByIdAsync(id);

    if (opportunity == null)
        throw new Exception("Opportunity not found.");

      if (opportunity.Stage == OpportunityStage.Won || opportunity.Stage == OpportunityStage.Lost)
    {
        
        throw new Exception($"Cannot change the stage because the opportunity is already {opportunity.Stage}.");
    }

var oldStage = opportunity.Stage;
    opportunity.Stage = OpportunityStage.Lost;

    await _opportunityRepository.UpdateAsync(opportunity);
    await _auditLogService.LogAsync(
    "Opportunity",
    opportunity.Id,
    managerId,
    "Stage Updated",
    oldStage.ToString(),
    opportunity.Stage.ToString()
);
}
}