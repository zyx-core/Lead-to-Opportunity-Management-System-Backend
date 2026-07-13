using LeadToOpportunity.Models.Enums;

namespace LeadToOpportunity.BLL.DTOs;

public class OpportunityResponseDto
{
    public int id {get;set;}

    public int LeadId {get;set;}

    public string CompanyName {get; set;} = string.Empty;

    public OpportunityStage Stage {get; set;}

    public decimal EstimatedValue {get; set;}

    public DateTime ExpectedClosureDate {get; set;}
}