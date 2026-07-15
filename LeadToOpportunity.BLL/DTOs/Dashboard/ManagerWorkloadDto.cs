namespace LeadToOpportunity.BLL.DTOs.Dashboard;

public class ManagerWorkloadDto
{
    public int ManagerId { get; set; }
    public string ManagerName { get; set; } = string.Empty;
    public int PendingReviews { get; set; }
}
