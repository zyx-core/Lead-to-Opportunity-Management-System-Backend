using LeadToOpportunity.Models.Entities;
using LeadToOpportunity.Models.Enums;

namespace LeadToOpportunity.DAL.Interfaces;

public interface ILeadRepository : IGenericRepository<Lead>
{
    Task<(IEnumerable<Lead> Items, int TotalCount)> GetByEmployeeAsync(int employeeId, int pageNumber, int pageSize);
    Task<Lead?> GetEmployeeLeadByIdAsync(int leadId, int employeeId);
    Task<IEnumerable<Lead>> GetDraftLeadAsync(int employeeId);
    Task<(IEnumerable<Lead> Items, int TotalCount)> GetManagerLeadsAsync(int managerId, int pageNumber, int pageSize);
    Task<Lead?> GetLeadForApprovalAsync(int leadId, int managerId);
    Task<Lead?> GetManagerLeadByIdAsync(int leadId,int managerId);
    Task<int> CountEmployeeLeadsByStatusAsync(int employeeId,LeadStatus status);

    Task<int> CountManagerLeadsByStatusAsync(int managerId ,LeadStatus status);

    Task<(IEnumerable<Lead> Items, int TotalCount)> GetPipelineAsync(int pageNumber, int pageSize);

    Task<int> CountTotalSubmittedAsync();

    Task<int> CountConvertedAsync();

    Task<IEnumerable<Lead>> GetAllWithManagerAsync();
}