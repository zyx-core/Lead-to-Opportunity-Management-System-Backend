namespace LeadToOpportunity.BLL.DTOs.Leads;

public class UpdateLeadRequestDto
{
    public string CompanyName { get; set; } = string.Empty;

    public string ContactName { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string Phone { get; set; } = string.Empty;

    public string Requirement { get; set; } = string.Empty;

    public decimal EstimatedValue { get; set; }

    public string Source { get; set; } = string.Empty;

    public string Region { get; set; } = string.Empty;

   
}