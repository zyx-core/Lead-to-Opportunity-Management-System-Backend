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
    public DbSet <Opportunity> Opportunities => Set<Opportunity>();
    public DbSet<AuditLog> AuditLogs => Set<AuditLog>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
    }
}