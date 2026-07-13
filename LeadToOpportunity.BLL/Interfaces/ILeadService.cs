
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

    Task<IEnumerable<LeadResponseDto>>GetMyLeadAsync(
        int employeeId
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
    Task<IEnumerable<LeadResponseDto>> GetManagerLeadsAsync(
        int managerId
        );
    Task ApproveLeadAsync(int LeadId , int managerId);  

    Task RejectLeadAsync(int leadId,
    int managerId,
    RejectLeadRequestDto request);

    Task RequestModificationAsync(int leadId,int managerId,RequestedModificationDto request);

    Task <LeadResponseDto>GetManagerLeadByIdAsync(int leadId,
    int managerId);

    Task AssignManagerAsync(int leadId,int adminId,AssignManagerRequestDto request);

    Task<IEnumerable<PipelineDto>> GetPipelineAsync();

    
}