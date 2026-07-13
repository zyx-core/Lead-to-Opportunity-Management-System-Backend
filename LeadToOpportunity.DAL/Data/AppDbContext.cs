using LeadToOpportunity.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace LeadToOpportunity.DAL.Data;
public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options): base(options){
    }
    public DbSet<User> Users => Set<User>();
    public DbSet<Lead> Leads => Set<Lead>();
    public DbSet <LeadReview> LeadReviews => Set<LeadReview>();
    public DbSet <Opportunity> Opportunities{get;set;}
    public DbSet<AuditLog> AuditLogs => Set<AuditLog>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);

        modelBuilder.Entity<Lead>()
    .HasOne(l => l.CreatedByEmployee)
    .WithMany(u => u.CreatedLeads)
    .HasForeignKey(l => l.CreatedByEmployeeId)
    .OnDelete(DeleteBehavior.Restrict);

modelBuilder.Entity<Lead>()
    .HasOne(l => l.AssignedManager)
    .WithMany(u => u.AssignedLeads)
    .HasForeignKey(l => l.AssignedManagerId)
    .OnDelete(DeleteBehavior.Restrict);

 
    }

  
}