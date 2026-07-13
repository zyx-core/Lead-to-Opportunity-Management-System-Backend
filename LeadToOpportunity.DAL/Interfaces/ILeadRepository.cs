using LeadToOpportunity.Models.Entities;
using LeadToOpportunity.Models.Enums;

namespace LeadToOpportunity.DAL.Interfaces;

public interface ILeadRepository : IGenericRepository<Lead>
{
    Task<IEnumerable<Lead>>GetByEmployeeAsync(int employeeId);
    Task<Lead?> GetEmployeeLeadByIdAsync(int leadId, int employeeId);
    Task<IEnumerable<Lead>> GetDraftLeadAsync(int employeeId);
    Task<IEnumerable<Lead>> GetManagerLeadsAsync(int managerId);
    Task<Lead?> GetLeadForApprovalAsync(int leadId, int managerId);
    Task<Lead?> GetManagerLeadByIdAsync(int leadId,int managerId);
    Task<int> CountEmployeeLeadsByStatusAsync(int employeeId,LeadStatus status);

    Task<int> CountManagerLeadsByStatusAsync(int managerId ,LeadStatus status);

    Task<IEnumerable<Lead>> GetPipelineAsync();
}