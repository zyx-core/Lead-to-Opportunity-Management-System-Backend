using LeadToOpportunity.BLL.DTOs.Dashboard;
using LeadToOpportunity.BLL.Interfaces;
using LeadToOpportunity.DAL.Interfaces;
using LeadToOpportunity.Models.Enums;

namespace LeadToOpportunity.BLL.Services;

public class DashboardService : IDashboardService
{
    private readonly ILeadRepository _leadRepository;
    private readonly IOpportunityRepository _opportunityRepository;

    public DashboardService(ILeadRepository leadRepository,IOpportunityRepository opportunityRepository)
    {
        _leadRepository = leadRepository;
        _opportunityRepository = opportunityRepository;
    }

    public async Task<EmployeeDashboardDto> GetEmployeeDashboardAsync(
        int employeeId)
    {
        var draft = await _leadRepository.CountEmployeeLeadsByStatusAsync(
            employeeId, LeadStatus.Draft);

        var underReview = await _leadRepository.CountEmployeeLeadsByStatusAsync(
            employeeId, LeadStatus.UnderReview);

        var approved = await _leadRepository.CountEmployeeLeadsByStatusAsync(
            employeeId, LeadStatus.Approved);

        var rejected = await _leadRepository.CountEmployeeLeadsByStatusAsync(
            employeeId, LeadStatus.Rejected);

        var modificationRequested =
            await _leadRepository.CountEmployeeLeadsByStatusAsync(
                employeeId,
                LeadStatus.ModificationRequested);

        return new EmployeeDashboardDto
        {
            DraftLeads = draft,
            UnderReviewLeads = underReview,
            ApprovedLeads = approved,
            RejectedLeads = rejected,
            ModificationRequestedLeads = modificationRequested,
            TotalLeads = draft + underReview + approved +
                         rejected + modificationRequested
        };
        

    }
    public async Task<ManagerDashboardDto> GetManagerDashboardAsync(
    int managerId)
{
    var stageCounts = await _opportunityRepository.GetCountByStageAsync();

    var opportunitiesByStage = stageCounts
        .Select(kv => new OpportunityStageCountDto
        {
            Stage = kv.Key.ToString(),
            Count = kv.Value
        })
        .ToList();

    return new ManagerDashboardDto
    {
        PendingReviews =
            await _leadRepository.CountManagerLeadsByStatusAsync(
                managerId,
                LeadStatus.UnderReview),

        ApprovedLeads =
            await _leadRepository.CountManagerLeadsByStatusAsync(
                managerId,
                LeadStatus.Approved),

        RejectedLeads =
            await _leadRepository.CountManagerLeadsByStatusAsync(
                managerId,
                LeadStatus.Rejected),

        ModificationRequested =
            await _leadRepository.CountManagerLeadsByStatusAsync(
                managerId,
                LeadStatus.ModificationRequested),

        WonDeals =
            await _opportunityRepository.CountByStageAsync(
                OpportunityStage.Won),

        LostDeals =
            await _opportunityRepository.CountByStageAsync(
                OpportunityStage.Lost),

        OpenOpportunities =
             await _opportunityRepository.CountOpenOpportunitiesAsync(),

        TotalSubmitted = await _leadRepository.CountTotalSubmittedAsync(),

        ConvertedLeads = await _leadRepository.CountConvertedAsync(),

        TotalPipelineValue = await _opportunityRepository.GetTotalEstimatedValueAsync(),

        OpportunitiesByStage = opportunitiesByStage
    };
}

public async Task<IEnumerable<ManagerWorkloadDto>> GetManagerWorkloadAsync()
{
    var leads = await _leadRepository.GetAllWithManagerAsync();

    return leads
        .Where(l => l.Status == LeadStatus.UnderReview && l.AssignedManager != null)
        .GroupBy(l => l.AssignedManagerId)
        .Select(g => new ManagerWorkloadDto
        {
            ManagerId = g.Key!.Value,
            ManagerName = $"{g.First().AssignedManager!.FirstName} {g.First().AssignedManager!.LastName}",
            PendingReviews = g.Count()
        })
        .OrderByDescending(w => w.PendingReviews)
        .ToList();
}
}