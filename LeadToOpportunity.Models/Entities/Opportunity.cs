using LeadToOpportunity.Models.Enums;

namespace LeadToOpportunity.Models.Entities;

public class Opportunity : BaseEntity
{
    public int LeadId { get; set; }

    public Lead Lead { get; set; } = null!;

    public OpportunityStage Stage { get; set; }
        = OpportunityStage.Qualification;

    public decimal EstimatedValue { get; set; }

    public DateTime ExpectedClosureDate { get; set; }

    public string? Notes { get; set; } 

    }