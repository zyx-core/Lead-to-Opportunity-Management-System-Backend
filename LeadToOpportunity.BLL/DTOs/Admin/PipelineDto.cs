

using LeadToOpportunity.Models.Enums;
namespace LeadToOpportunity.BLL.DTOs.Admin;
public class PipelineDto
{
    public int LeadId { get; set; }

    public string CompanyName { get; set; } = string.Empty;

    public string EmployeeName { get; set; } = string.Empty;

    public string ManagerName { get; set; } = string.Empty;

    public LeadStatus LeadStatus { get; set; }

    public OpportunityStage? OpportunityStage { get; set; }
}