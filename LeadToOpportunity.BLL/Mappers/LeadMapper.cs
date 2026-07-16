using LeadToOpportunity.BLL.DTOs.Leads;
using LeadToOpportunity.BLL.Leads;

using LeadToOpportunity.Models.Entities;

namespace LeadToOpportunity.BLL.Mappers;

public static class LeadMapper
{
    public static LeadResponseDto ToResponseDto(this Lead lead)
    {
        return new LeadResponseDto
        {
            Id = lead.Id,
            CompanyName = lead.CompanyName,
            ContactName = lead.ContactName,
            Email = lead.Email,
            Phone = lead.Phone,
            Requirement = lead.Requirement,
            EstimatedValue = lead.EstimatedValue,
            Source = lead.Source,
            Region = lead.Region,
            Status = lead.Status,
            CreatedAt = lead.CreatedAt,
            ManagerId = lead.AssignedManagerId ?? 0,      


        ManagerComment = lead.Reviews
            .OrderByDescending(r => r.Id)
            .FirstOrDefault()?.Comment


            
            
        };
    }
}