using LeadToOpportunity.BLL.DTOs.Dashboard;

namespace LeadToOpportunity.BLL.Interfaces;

public interface IDashboardService
{
    Task<EmployeeDashboardDto> GetEmployeeDashboardAsync(int employeeId);
    Task<ManagerDashboardDto> GetManagerDashboardAsync(
    int managerId);
    Task<IEnumerable<ManagerWorkloadDto>> GetManagerWorkloadAsync();
}