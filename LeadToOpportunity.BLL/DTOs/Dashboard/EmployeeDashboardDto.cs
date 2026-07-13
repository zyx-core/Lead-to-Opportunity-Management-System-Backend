namespace LeadToOpportunity.BLL.DTOs.Dashboard;

public class EmployeeDashboardDto
{
    public int DraftLeads { get; set; }

    public int UnderReviewLeads { get; set; }

    public int ApprovedLeads { get; set; }

    public int RejectedLeads { get; set; }

    public int ModificationRequestedLeads { get; set; }

    public int TotalLeads { get; set; }
}