using LeadToOpportunity.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LeadToOpportunity.Configurations;

public class LeadConfiguration : IEntityTypeConfiguration<Lead>
{
    public void Configure(EntityTypeBuilder<Lead> builder)
    {
        builder.ToTable("Leads");
        builder.HasKey(l => l.Id);

        builder.Property(l => l.CompanyName)
        .IsRequired()
        .HasMaxLength(100);

        builder.Property(l=> l.Email)
        .IsRequired()
        .HasMaxLength(150);

        builder.Property(l => l.Phone)
        .IsRequired()
        .HasMaxLength(20);

        builder.Property(l => l.Requirement)
        .HasMaxLength(1000);

        builder.Property(l => l.EstimatedValue)
       .HasPrecision(18,2);

        builder.Property(l => l.Source)
       .HasMaxLength(100);

        builder.Property(l => l.Region)
       .HasMaxLength(100);

       builder.Property(l => l.Status)
       .IsRequired();

       builder
       .HasOne(l => l.CreatedByEmployee)
       .WithMany(u=> u.CreatedLeads)
       .HasForeignKey(l => l.CreatedByEmployeeId)
       .OnDelete(DeleteBehavior.Restrict);

       builder
       .HasOne(l => l.AssignedManager)
       .WithMany(u => u.AssignedLeads)
       .HasForeignKey(l => l.AssignedManagerId)
       .OnDelete(DeleteBehavior.Restrict);

       builder
       .HasMany(l => l.Reviews)
       .WithOne(r => r.Lead)
       .HasForeignKey(r => r.LeadId);

       builder
       .HasOne(l => l.Opportunity)
       .WithOne(o => o.Lead)
       .HasForeignKey<Opportunity>(o => o.LeadId);

    }
}