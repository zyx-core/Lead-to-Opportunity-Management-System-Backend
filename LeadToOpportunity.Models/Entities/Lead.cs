using LeadToOpportunity.Models.Enums;

namespace LeadToOpportunity.Models.Entities;

public class Lead : BaseEntity
{
    public string CompanyName{get; set;} = string.Empty;
    public string ContactName{get;set;} = string.Empty;
    public string Email {get; set;} = string.Empty;

    public string Phone {get; set ;} = string.Empty;

    public string Requirement {get; set; } = string.Empty;
    public decimal EstimatedValue {get; set;}

    public string Source {get ;set; } =string.Empty;

    public string Region {get; set;} = string.Empty;

    public LeadStatus Status {get; set; } = LeadStatus.Draft;

    public int CreatedByEmployeeId {get; set;}
    public User CreatedByEmployee {get; set;}= null!;
    public int? AssignedManagerId {get;set;}
    public User? AssignedManager{get; set;}
    public ICollection<LeadReview> Reviews {get; set;} = new List<LeadReview>();
    public Opportunity? Opportunity {get; set;}
}