using LeadToOpportunity.Models.Enums;

namespace LeadToOpportunity.BLL.Leads;

public class LeadResponseDto
{
    public int Id { get; set; }

    public string CompanyName { get; set; } = string.Empty;

    public string ContactName { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string Phone { get; set; } = string.Empty;

    public string Requirement { get; set; }

    public decimal EstimatedValue { get; set; }


    public string Source { get; set; }

    public string Region { get; set; }

    public string? ManagerComment { get; set; }

    public LeadStatus Status { get; set; }

    public DateTime CreatedAt { get; set; }
}