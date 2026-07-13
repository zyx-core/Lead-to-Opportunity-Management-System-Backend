using LeadToOpportunity.DAL.Data;
using LeadToOpportunity.DAL.Interfaces;
using LeadToOpportunity.Models.Entities;

namespace LeadToOpportunity.DAL.Repositories;

public class LeadReviewRepository : GenericRepository<LeadReview>, ILeadReviewRepository
{
    public LeadReviewRepository(AppDbContext context) : base(context)
    {
        
    }
}