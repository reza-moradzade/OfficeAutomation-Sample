using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OfficeAutomation.Core.Entities.Security;
using System;

namespace OfficeAutomation.Data.Configurations
{
    // Configures the RefreshToken entity for EF Core mapping.
    public class RefreshTokenConfiguration : IEntityTypeConfiguration<RefreshToken>
    {
        public void Configure(EntityTypeBuilder<RefreshToken> builder)
        {
            // Map to table "RefreshTokens".
            builder.ToTable("RefreshTokens");

            // Primary key.
            builder.HasKey(rt => rt.RefreshTokenId);

            // Default value for the primary key.
            builder.Property(rt => rt.RefreshTokenId)
                   .HasDefaultValueSql("newid()");

            // Token is required and limited to 450 characters.
            builder.Property(rt => rt.Token)
                   .IsRequired()
                   .HasMaxLength(450);

            // Default creation timestamp.
            builder.Property(rt => rt.CreatedAt)
                   .HasDefaultValueSql("sysutcdatetime()");

            // Maximum length for IP addresses.
            builder.Property(rt => rt.CreatedByIp)
                   .HasMaxLength(45);

            builder.Property(rt => rt.RevokedByIp)
                   .HasMaxLength(45);

            // Relationship with User entity.
            builder.HasOne(rt => rt.User)
                   .WithMany() // Optionally: WithMany(u => u.RefreshTokens)
                   .HasForeignKey(rt => rt.UserId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
