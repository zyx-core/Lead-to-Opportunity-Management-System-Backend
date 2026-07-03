using LeadToOpportunity.Models.Enums;

namespace LeadToOpportunity.Models.Entities;

public class LeadReview : BaseEntity
{
    public int LeadId {get;set;} 
    public int ReviewerId {get; set;} 
    public ReviewAction Action {get;set;}

    public string Comment{get;set;} = string.Empty;

    public Lead Lead {get;set;} = null!;
    public User Reviewer {get;set;}= null!;

}