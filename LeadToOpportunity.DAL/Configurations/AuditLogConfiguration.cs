using LeadToOpportunity.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LeadToOpportunity.DAL.Configurations;

public class AuditLogConfiguration : IEntityTypeConfiguration<AuditLog>
{
    public void Configure(EntityTypeBuilder<AuditLog> builder)
    {
        builder.ToTable("AuditLogs");

        builder.HasKey(a => a.Id);

        builder.Property(a => a.EntityType)
               .IsRequired()
               .HasMaxLength(50);

        builder.Property(a => a.Action)
               .IsRequired()
               .HasMaxLength(100);

        builder.Property(a => a.FromStatus)
               .HasMaxLength(50);

        builder.Property(a => a.ToStatus)
               .HasMaxLength(50);

        builder.Property(a => a.Comment)
               .HasMaxLength(1000);

        builder.HasOne(a => a.Actor)
               .WithMany()
               .HasForeignKey(a => a.ActorId)
               .OnDelete(DeleteBehavior.Restrict);
    }
}