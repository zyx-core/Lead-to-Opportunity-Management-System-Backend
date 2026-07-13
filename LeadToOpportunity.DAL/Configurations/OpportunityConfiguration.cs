using LeadToOpportunity.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LeadToOpportunity.DAL.Configurations;

public class OpportunityConfiguration : IEntityTypeConfiguration<Opportunity>
{
    public void Configure(EntityTypeBuilder<Opportunity> builder)
    {
        builder.ToTable("Opportunities");

        builder.HasKey(o => o.Id);

        builder.Property(o => o.EstimatedValue)
            .HasPrecision(18, 2);

        builder.Property(o => o.Stage)
            .IsRequired();

        builder.Property(o => o.Notes)
            .HasMaxLength(1000);

        
    }
}