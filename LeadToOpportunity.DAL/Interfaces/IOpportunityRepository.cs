using LeadToOpportunity.Models.Entities;
using LeadToOpportunity.Models.Enums;

namespace LeadToOpportunity.DAL.Interfaces;

public interface IOpportunityRepository : IGenericRepository<Opportunity>
{
    Task<IEnumerable<Opportunity>> GetAllWithLeadAsync();

    Task<Opportunity?> GetWithLeadAsync(int id);

    Task<int > CountByStageAsync(OpportunityStage stage);
    Task<int> CountOpenOpportunitiesAsync();

    Task<IEnumerable<Opportunity>> GetByEmployeeIdAsync(int employeeId);

    Task<decimal> GetTotalEstimatedValueAsync();

    Task<Dictionary<OpportunityStage, int>> GetCountByStageAsync();
}