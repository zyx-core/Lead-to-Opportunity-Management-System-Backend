namespace LeadToOpportunity.BLL.DTOs.Dashboard;

public class ManagerDashboardDto
{
    public int PendingReviews { get; set; }

    public int ApprovedLeads { get; set; }

    public int RejectedLeads { get; set; }

    public int ModificationRequested { get; set; }

    public int OpenOpportunities { get; set; }

    public int WonDeals { get; set; }

    public int LostDeals { get; set; }

    public int TotalSubmitted { get; set; }

    public int ConvertedLeads { get; set; }

    public decimal TotalPipelineValue { get; set; }

    public List<OpportunityStageCountDto> OpportunitiesByStage { get; set; } = new();
}