
using LeadToOpportunity.BLL.DTOs.Admin;
using LeadToOpportunity.BLL.DTOs.Leads;

using LeadToOpportunity.BLL.Leads;

namespace LeadToOpportunity.BLL.Interfaces;

public interface ILeadService
{
    Task<LeadResponseDto> CreateLeadAsync(
        CreateLeadRequestDto request,
        int employeeId
    );

    Task<LeadToOpportunity.Shared.Pagination.PagedResult<LeadResponseDto>> GetMyLeadAsync(
        int employeeId, int pageNumber, int pageSize
    );

    Task SubmitLeadAsync(
    int leadId,
    int employeeId,
    SubmitLeadRequestDto request);

    Task<LeadResponseDto?>GetLeadByIdAsync(
        int LeadId,
        int employeeId
    );
    Task UpdateLeadAsync(
        int leadId,
        int employeeId,
        UpdateLeadRequestDto request
    );
    Task DeleteLeadAsync(
        int leadId,
        int employeeId
    );
    Task<LeadToOpportunity.Shared.Pagination.PagedResult<LeadResponseDto>> GetManagerLeadsAsync(
        int managerId, int pageNumber, int pageSize
        );
    Task ApproveLeadAsync(int LeadId , int managerId);  

    Task RejectLeadAsync(int leadId,
    int managerId,
    RejectLeadRequestDto request);

    Task RequestModificationAsync(int leadId,int managerId,RequestedModificationDto request);

    Task <LeadResponseDto>GetManagerLeadByIdAsync(int leadId,
    int managerId);

    Task AssignManagerAsync(int adminId,AssignManagerRequestDto request);

    Task<LeadToOpportunity.Shared.Pagination.PagedResult<PipelineDto>> GetPipelineAsync(int pageNumber, int pageSize);

    
}