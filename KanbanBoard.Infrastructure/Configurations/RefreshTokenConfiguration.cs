using KanbanBoard.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KanbanBoard.Infrastructure.Configurations
{
    public class RefreshTokenConfiguration : IEntityTypeConfiguration<RefreshToken>
    {
        public void Configure(EntityTypeBuilder<RefreshToken> builder)
        {
            builder.HasKey(rt => rt.Id);

            builder.Property(rt => rt.Token)
                .IsRequired()
                .HasMaxLength(256);

            builder.Property(rt => rt.ExpiryDate)
                .IsRequired();

            builder.Property(rt => rt.IsRevoked)
                .IsRequired();

            builder.Property(rt => rt.RevokedReason)
                .HasMaxLength(500);

            builder.Property(rt => rt.ReplacedByToken)
                .HasMaxLength(256);

            builder.Property(rt => rt.ClientIpAddress)
                .IsRequired()
                .HasMaxLength(45); // IPv6 max length

            builder.Property(rt => rt.UserAgent)
                .IsRequired()
                .HasMaxLength(500);

            builder.Property(rt => rt.UserId)
                .IsRequired();

            // Indexes
            builder.HasIndex(rt => rt.Token)
                .IsUnique();

            builder.HasIndex(rt => rt.UserId);

            builder.HasIndex(rt => rt.ExpiryDate);
        }
    }
}