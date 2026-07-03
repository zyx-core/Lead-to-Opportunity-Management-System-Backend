using LeadToOpportunity.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LeadToOpportunity.DAL.Configurations;

public class LeadReviewConfiguration : IEntityTypeConfiguration<LeadReview>
{
    public void Configure(EntityTypeBuilder<LeadReview> builder)
    {
        builder.ToTable("LeadReviews");

        builder.HasKey(lr => lr.Id);

        builder.Property(lr => lr.Comment)
               .HasMaxLength(1000);

        builder.Property(lr => lr.Action)
               .IsRequired();

        builder.HasOne(lr => lr.Lead)
               .WithMany(l => l.Reviews)
               .HasForeignKey(lr => lr.LeadId)
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(lr => lr.Reviewer)
               .WithMany()
               .HasForeignKey(lr => lr.ReviewerId)
               .OnDelete(DeleteBehavior.Restrict);
    }
}