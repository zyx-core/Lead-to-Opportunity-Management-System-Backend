using LeadToOpportunity.DAL.Data;
using LeadToOpportunity.DAL.Interfaces;
using LeadToOpportunity.Models.Entities;
using LeadToOpportunity.Models.Enums;
using Microsoft.EntityFrameworkCore;

namespace LeadToOpportunity.DAL.Repositories;

public class OpportunityRepository :
GenericRepository<Opportunity>, IOpportunityRepository
{
    public OpportunityRepository(AppDbContext context) : base(context)
    {
        
    }
    public async Task<IEnumerable<Opportunity>> GetAllWithLeadAsync()
    {
        return await _context.Opportunities
        .Include(o => o.Lead)
        .ToListAsync();
    }

public async Task<Opportunity?> GetWithLeadAsync(int id)
{
    return await _context.Opportunities
        .Include(o => o.Lead)
        .FirstOrDefaultAsync(o => o.Id == id);
}

public async Task<int> CountByStageAsync(OpportunityStage stage)
    {
          return await _context.Opportunities
        .CountAsync(o => o.Stage == stage);
    }

    public async Task<int> CountOpenOpportunitiesAsync()
{
    return await _context.Opportunities.CountAsync(o =>
        o.Stage != OpportunityStage.Won &&
        o.Stage != OpportunityStage.Lost);
}
}