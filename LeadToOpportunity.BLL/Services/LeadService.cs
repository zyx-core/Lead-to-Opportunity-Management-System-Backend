using LeadToOpportunity.BLL.DTOs.Leads;
using LeadToOpportunity.BLL.Interfaces;
using LeadToOpportunity.BLL.Leads;
using LeadToOpportunity.DAL.Interfaces;
using LeadToOpportunity.Models.Enums;
using LeadToOpportunity.Models.Entities;
using LeadToOpportunity.Shared.Exception;
using LeadToOpportunity.Shared.Exceptions;
using LeadToOpportunity.BLL.Mappers;
using LeadToOpportunity.BLL.DTOs.User;

using LeadToOpportunity.DAL.Repositories;
using LeadToOpportunity.DAL.Data;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using LeadToOpportunity.BLL.DTOs.Admin;
namespace LeadToOpportunity.BLL.Services;



public class LeadService : ILeadService
{
    private readonly ILeadRepository _leadRepository;
    private readonly IUserRepository _userRepository;

    private readonly IAuditLogService _auditLogService;
    private readonly IOpportunityRepository _opportunityRepository;

    private readonly ILeadReviewRepository _leadReviewRepository;
    private readonly AppDbContext _context;


    
    public static class AuditEntity
{
    public const string Lead = "Lead";

    public const string Opportunity = "Opportunity";
}
public static class AuditAction
{
    public const string Created = "Created";
    public const string Updated = "Updated";
    public const string Submitted = "Submitted";
    public const string Approved = "Approved";
    public const string Rejected = "Rejected";
    public const string ModificationRequested = "Modification Requested";
    public const string Deleted = "Deleted";
}

    public LeadService(ILeadRepository leadRepository,
    IUserRepository userRepository, IOpportunityRepository opportunityRepository,
    ILeadReviewRepository leadReviewRepository ,IAuditLogService auditLogService,
    AppDbContext context)
    {
        _leadRepository = leadRepository;
        _userRepository = userRepository;
        _opportunityRepository = opportunityRepository;
        _leadReviewRepository= leadReviewRepository;
        _auditLogService = auditLogService;
        _context = context;
    }


    public async Task<LeadResponseDto> CreateLeadAsync (CreateLeadRequestDto request, int employeeId)
    {
        var lead = new Models.Entities.Lead
    {
        CompanyName = request.CompanyName,
        ContactName = request.ContactName,
        Email = request.Email,
        Phone = request.Phone,
        Requirement = request.Requirement,
        EstimatedValue = request.EstimatedValue,
        Source = request.Source,
        Region = request.Region,

        CreatedByEmployeeId = employeeId,

        Status = LeadStatus.Draft
    };

    await _leadRepository.AddAsync(lead);

    await _auditLogService.LogAsync(
        AuditEntity.Lead,
        lead.Id,
        employeeId,
        AuditAction.Created,
        "",
        LeadStatus.Draft.ToString()
    );

    return LeadMapper.ToResponseDto(lead);
   
    }
     public async Task<LeadToOpportunity.Shared.Pagination.PagedResult<LeadResponseDto>> GetMyLeadAsync(int employeeId, int pageNumber, int pageSize)
    {
        var (items, totalCount) = await _leadRepository.GetByEmployeeAsync(employeeId, pageNumber, pageSize);

        return new LeadToOpportunity.Shared.Pagination.PagedResult<LeadResponseDto>
        {
            Items = items.Select(LeadMapper.ToResponseDto),
            TotalCount = totalCount,
            PageNumber = pageNumber,
            PageSize = pageSize
        };
    }
    public async Task<LeadResponseDto?> GetLeadByIdAsync(int leadId, int employeeId)
    {
        var lead = await _leadRepository.GetEmployeeLeadByIdAsync(leadId, employeeId);

        if(lead == null)
        {
            throw new NotFoundException("lead not found");
        }

        return LeadMapper.ToResponseDto(lead);
    }

    public async Task UpdateLeadAsync(int leadId, int employeeId, UpdateLeadRequestDto request)
    {
        var lead = await _leadRepository
        .GetEmployeeLeadByIdAsync(leadId, employeeId);

        if(lead == null)
        {
            throw new NotFoundException("lead not found");
        }

        if(lead.Status != LeadStatus.Draft &&
        lead.Status != LeadStatus.ModificationRequested)
        {
            throw new BadRequestException(
                "only Draft or Modifiction Leads can be edited"
            );
        }

    lead.CompanyName = request.CompanyName;
    lead.ContactName = request.ContactName;
    lead.Email = request.Email;
    lead.Phone = request.Phone;
    lead.Requirement = request.Requirement;
    lead.EstimatedValue = request.EstimatedValue;
    lead.Source = request.Source;
    lead.Region = request.Region;

    await _leadRepository.UpdateAsync(lead);
    await _auditLogService.LogAsync(
    AuditEntity.Lead,
    lead.Id,
    employeeId,
    AuditAction.Updated,
    LeadStatus.Draft.ToString(),
    LeadStatus.Draft.ToString(),
    "Lead details updated."
);

    }

    public async Task DeleteLeadAsync(int leadId, int employeeId)
    {
        var lead = await _leadRepository.GetEmployeeLeadByIdAsync(leadId,employeeId);

        if(lead == null)
        {
            throw new NotFoundException("Lead not found");
        }
        if(lead.Status != LeadStatus.Draft)
        {
            throw new BadRequestException("only draft lead can be deleted.");
        }
        await _auditLogService.LogAsync(
    AuditEntity.Lead,
    lead.Id,
    employeeId,
    AuditAction.Deleted,
    LeadStatus.Draft.ToString(),
    "Deleted"
);
        await _leadRepository.DeleteAsync(lead);
        
    }
    public async Task SubmitLeadAsync(
        int leadId,
        int employeeId,
        SubmitLeadRequestDto request
    )
    {
        var lead = await _leadRepository
        .GetEmployeeLeadByIdAsync(leadId, employeeId);

        if(lead == null)
        {
            throw new NotFoundException("Lead not found");
        }
        if(lead.Status != LeadStatus.Draft && lead.Status != LeadStatus.ModificationRequested)
        {
            throw new BadRequestException(
                "Only draft and modificationRequested lead can be submitted"
            );
        }

        var manager = await _userRepository.GetByIdAsync(request.ManagerId);

        if(manager == null)
        {
            throw new NotFoundException("manager not found");
        }
        if (manager.Role != UserRole.Manager)
        {
            throw new BadRequestException(
                "Selected user is not a manager"
            );
        }

        var fromStatus = lead.Status.ToString();

         lead.AssignedManagerId = request.ManagerId;
        lead.Status = LeadStatus.Submitted;

        await _leadRepository.UpdateAsync(lead);

        await _auditLogService.LogAsync(
            AuditEntity.Lead,
            lead.Id,
            employeeId,
            AuditAction.Submitted,
            fromStatus,
            LeadStatus.Submitted.ToString()
        );

        lead.Status = LeadStatus.UnderReview;

        await _leadRepository.UpdateAsync(lead);

        await _auditLogService.LogAsync(
            AuditEntity.Lead,
            lead.Id,
            employeeId,
            "Under Review",
            LeadStatus.Submitted.ToString(),
            LeadStatus.UnderReview.ToString()
        );

    }
    public async Task<LeadToOpportunity.Shared.Pagination.PagedResult<LeadResponseDto>> GetManagerLeadsAsync(int managerId, int pageNumber, int pageSize)
    {
       var (items, totalCount) = await _leadRepository.GetManagerLeadsAsync(managerId, pageNumber, pageSize);

       return new LeadToOpportunity.Shared.Pagination.PagedResult<LeadResponseDto>
       {
           Items = items.Select(LeadMapper.ToResponseDto),
           TotalCount = totalCount,
           PageNumber = pageNumber,
           PageSize = pageSize
       };
    }

    public async Task ApproveLeadAsync(
        int leadId,
        int managerId
    )
    {

        await using var transaction =
    await _context.Database.BeginTransactionAsync();

     try {  var lead= await _leadRepository.GetLeadForApprovalAsync(leadId,managerId);
        if(lead == null)
        {
            throw new NotFoundException("Lead not found");
        }
        lead.Status = LeadStatus.Approved;

        await _leadRepository.UpdateAsync(lead);

        await _auditLogService.LogAsync(
            AuditEntity.Lead,
            lead.Id,
            managerId,
            AuditAction.Approved,
            LeadStatus.UnderReview.ToString(),
            LeadStatus.Approved.ToString()
        );

        var opportunity = new Opportunity
        {
            LeadId = lead.Id,
            EstimatedValue = lead.EstimatedValue,
            Stage = OpportunityStage.Qualification,
            ExpectedClosureDate = DateTime.UtcNow.AddMonths(1)
        };

        await _opportunityRepository.AddAsync(opportunity);

        await _auditLogService.LogAsync(
            AuditEntity.Opportunity,
            opportunity.Id,
            managerId,
            AuditAction.Created,
            "",
            OpportunityStage.Qualification.ToString()
        );

        lead.Status = LeadStatus.Converted;

        await _leadRepository.UpdateAsync(lead);

        await _auditLogService.LogAsync(
            AuditEntity.Lead,
            lead.Id,
            managerId,
            "Converted",
            LeadStatus.Approved.ToString(),
            LeadStatus.Converted.ToString()
        );

        await transaction.CommitAsync();
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }

    public async Task RejectLeadAsync(
        int leadId,
        int managerId,
        RejectLeadRequestDto request
    )
    {
        await using var transaction =
        await _context.Database.BeginTransactionAsync();
        try{
        var lead = await _leadRepository.GetLeadForApprovalAsync(leadId, managerId);
        if(lead == null)
        {
            throw new NotFoundException("lead not found");
        }

        lead.Status = LeadStatus.Rejected;

        await _leadRepository.UpdateAsync(lead);

        
        var review = new LeadReview
    {
       LeadId = lead.Id,
        ReviewerId = managerId,
        Action = ReviewAction.Reject,
        Comment = request.response
    };


    await _leadReviewRepository.AddAsync(review);
    await _auditLogService.LogAsync(
            AuditEntity.Lead,
            lead.Id,
            managerId,
            AuditAction.Rejected,
            LeadStatus.UnderReview.ToString(),
            LeadStatus.Rejected.ToString(),
            request.response
        );

    await transaction.CommitAsync();
    }
        catch
        {
             await transaction.RollbackAsync();
        throw;
        }
    }

    public async Task RequestModificationAsync(
        int leadId,
        int managerId,
        RequestedModificationDto request
    )
    {
        await using var transaction =
        await _context.Database.BeginTransactionAsync();
        try{
        var lead = await _leadRepository.GetLeadForApprovalAsync(leadId,managerId);

        if(lead == null)
        {
            throw new NotFoundException("Lead not found");
        }

        lead.Status = LeadStatus.ModificationRequested;

        await _leadRepository.UpdateAsync(lead);

        var review = new LeadReview
        {
            LeadId = lead.Id,
            ReviewerId = managerId,
            Action = ReviewAction.RequestModification,
            Comment = request.Comment
        };
        await _leadReviewRepository.AddAsync(review);

        await _auditLogService.LogAsync(
            AuditEntity.Lead,
            lead.Id,
            managerId,
            AuditAction.ModificationRequested,
            LeadStatus.UnderReview.ToString(),
            LeadStatus.ModificationRequested.ToString(),
            request.Comment
        );
         await transaction.CommitAsync();
        }
        catch
        {
            await transaction.RollbackAsync();
        throw;
        }
    }

    public async Task<LeadResponseDto>GetManagerLeadByIdAsync(int leadId,
    int managerId)
    {
        var lead = await _leadRepository
        .GetManagerLeadByIdAsync(leadId,managerId);

        if(lead == null)
        {
            throw new NotFoundException("lead not found");
        }

        return LeadMapper.ToResponseDto(lead);
    }

    public async Task AssignManagerAsync(int adminId,AssignManagerRequestDto request)
    {
        var lead = await _leadRepository.GetByIdAsync(request.LeadId);

        if(lead == null)
        {
            throw new NotFoundException("lead not found");
        }

        var manager = await _userRepository.GetByIdAsync(request.ManagerId);

        if(manager == null)
        {
            throw new NotFoundException("Manager not found");
        }
        if(!(lead.Status ==  LeadStatus.Draft || lead.Status == LeadStatus.UnderReview))
        {
            Console.WriteLine(lead.Status);
            throw new BadRequestException("Manager already accepted");
        }
       
        if(manager.Role != UserRole.Manager)
        {
            throw new BadRequestException("Selected user is not a manager");
        }

        lead.AssignedManagerId = manager.Id;

        await _leadRepository.UpdateAsync(lead);

        await _auditLogService.LogAsync(
        AuditEntity.Lead,
        lead.Id,
        adminId,
        "Manager Reassigned",
        "",
        "",
        $"Assigned to manager {manager.FirstName} {manager.LastName}"
    );


    }

    public async Task<LeadToOpportunity.Shared.Pagination.PagedResult<PipelineDto>> GetPipelineAsync(int pageNumber, int pageSize)
    {
        var (items, totalCount) = await _leadRepository.GetPipelineAsync(pageNumber, pageSize);

        var dtos = items.Select(l => new PipelineDto
        {
            LeadId = l.Id,
            CompanyName = l.CompanyName,
            EmployeeName = $"{l.CreatedByEmployee.FirstName} {l.CreatedByEmployee.LastName}",
            ManagerName = l.AssignedManager == null ? "" : $"{l.AssignedManager.FirstName} {l.AssignedManager.LastName}",
            LeadStatus = l.Status,
            OpportunityStage = l.Opportunity?.Stage
        });

        return new LeadToOpportunity.Shared.Pagination.PagedResult<PipelineDto>
        {
            Items = dtos,
            TotalCount = totalCount,
            PageNumber = pageNumber,
            PageSize = pageSize
        };
    }


}