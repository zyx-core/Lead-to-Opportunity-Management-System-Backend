using LeadToOpportunity.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LeadToOpportunity.DAL.Configurations;

public class OpportunityConfiguration: IEntityTypeConfiguration<Opportunity>
{
    public void Configure(EntityTypeBuilder<Opportunity> builder)
    {
        builder.ToTable("Oppertunities");
        builder.HasKey(o=>o.Id);
        builder.Property(o=>o.EstimatedValue)
        .HasPrecision(18,2);
        builder.Property(o=>o.Stage)
        .IsRequired();
        builder.HasOne(o=>o.Owner)
        .WithMany(u=>u.Opportunities)
        .HasForeignKey(o=>o.OwnerId)
        .OnDelete(DeleteBehavior.Restrict);
    }
    
}