using LeadToOpportunity.BLL.Leads;

namespace LeadToOpportunity.BLL.DTOs.Opportunities;

public interface IOpportunityService
{
    Task<IEnumerable<OpportunityResponseDto>>GetAllAsync();
    Task <OpportunityResponseDto?> GetByIdAsync(int id);

    Task UpdateStageAsync(int id,int managerId,
    UpdateOpportunityStageDto request);

    Task MarkWonAsync (int id,int managerId);
    Task MarkLostAsync (int id,int managerId);
   
}