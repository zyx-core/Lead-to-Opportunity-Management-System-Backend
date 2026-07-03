using LeadToOpportunity.Models.Enums;

namespace LeadToOpportunity.Models.Entities;

public class Opportunity : BaseEntity
{
    public int LeadId {get;set;}
    public Lead Lead {get;set;} = null!;
    public int OwnerId {get;set;}
    public User Owner {get;set;} = null!;
    public OpportunityStage Stage {get;set;}
    public DateTime ExpectedCloseDate {get;set;}
    public decimal EstimatedValue {get;set;} 

    }