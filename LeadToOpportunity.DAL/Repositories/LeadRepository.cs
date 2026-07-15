using System.Runtime.ExceptionServices;
using LeadToOpportunity.DAL.Data;
using LeadToOpportunity.DAL.Interfaces;
using LeadToOpportunity.Models.Entities;
using LeadToOpportunity.Models.Enums;
using Microsoft.EntityFrameworkCore;

namespace LeadToOpportunity.DAL.Repositories;

public class LeadRepository : GenericRepository<Lead>, ILeadRepository
{
    

    public LeadRepository(AppDbContext context) 
    : base(context)
    {
      
    }
    
    public async Task<(IEnumerable<Lead> Items, int TotalCount)> GetByEmployeeAsync(int employeeId, int pageNumber, int pageSize)
    {
        var query = _context.Leads.Where(l => l.CreatedByEmployeeId == employeeId);
        
        var totalCount = await query.CountAsync();
        
        var items = await query
            .OrderByDescending(l => l.CreatedAt)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
            
        return (items, totalCount);
    }

    public async Task<Lead?> GetEmployeeLeadByIdAsync(int LeadId, int employeeId)
    {
        return await _context.Leads
        .Include(l => l.Reviews)
        .FirstOrDefaultAsync(
            l=>
            l.Id==LeadId&&
            l.CreatedByEmployeeId ==employeeId
        );
    }

    public async Task<IEnumerable<Lead>> GetDraftLeadAsync(int employeeId)
    {
        return await _context.Leads
        .Where(l=>
        l.CreatedByEmployeeId == employeeId &&
        l.Status == LeadStatus.Draft)
        .OrderByDescending(l=>l.CreatedAt)
        .ToListAsync();

        
    }

    public async Task<(IEnumerable<Lead> Items, int TotalCount)> GetManagerLeadsAsync(int managerId, int pageNumber, int pageSize)
    {
        var query = _context.Leads
            .Where(l => l.AssignedManagerId == managerId && l.Status == LeadStatus.UnderReview)
            .OrderByDescending(l => l.CreatedAt);

        var totalCount = await query.CountAsync();

        var items = await query
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return (items, totalCount);
    }

    public async Task<Lead?> GetLeadForApprovalAsync(
        int LeadId,
        int managerId
    )
    {
        return await _context.Leads
        .FirstOrDefaultAsync(l=>
        l.Id == LeadId&&
        l.AssignedManagerId == managerId &&
        l.Status == LeadStatus.UnderReview
        );
    }

    public async Task<Lead?> GetManagerLeadByIdAsync(int leadId,int managerId)
    {
        return await _context.Leads
        .FirstOrDefaultAsync(l=>
        l.Id == leadId &&
        l.AssignedManagerId == managerId);
    }

    public async Task<int> CountEmployeeLeadsByStatusAsync(int employeeId,LeadStatus status)
    {
        return await _context.Leads
        .CountAsync(l =>
        l.CreatedByEmployeeId == employeeId &&
        l.Status == status
        );
    }

    public async Task<int> CountManagerLeadsByStatusAsync(
    int managerId,
    LeadStatus status)
   {
    return await _context.Leads.CountAsync(l =>
        l.AssignedManagerId == managerId &&
        l.Status == status);
    }

    public async Task<(IEnumerable<Lead> Items, int TotalCount)> GetPipelineAsync(int pageNumber, int pageSize)
    {
        var query = _context.Leads
            .Include(l => l.CreatedByEmployee)
            .Include(l => l.AssignedManager)
            .Include(l => l.Opportunity)
            .OrderByDescending(l => l.CreatedAt);

        var totalCount = await query.CountAsync();

        var items = await query
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return (items, totalCount);
    }

public async Task<int> CountTotalSubmittedAsync()
{
    return await _context.Leads
        .CountAsync(l => l.Status != LeadStatus.Draft);
}

public async Task<int> CountConvertedAsync()
{
    return await _context.Leads
        .CountAsync(l => l.Status == LeadStatus.Converted);
}

public async Task<IEnumerable<Lead>> GetAllWithManagerAsync()
{
    return await _context.Leads
        .Include(l => l.AssignedManager)
        .Where(l => l.AssignedManagerId != null)
        .ToListAsync();
}
}