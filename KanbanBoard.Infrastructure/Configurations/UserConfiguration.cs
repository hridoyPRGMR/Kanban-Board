using KanbanBoard.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KanbanBoard.Infrastructure.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(u => u.Id);

            builder.Property(u => u.Name)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(u => u.Email)
                .IsRequired()
                .HasMaxLength(255);
            builder.HasIndex(u => u.Email)
                .IsUnique();

            builder.Property(u => u.PasswordHash).IsRequired().HasMaxLength(250);

            builder.Property(u => u.UserName).IsRequired().HasMaxLength(250);
            builder.HasIndex(u => u.UserName).IsUnique();

            builder.Property(u => u.Name).IsRequired().HasMaxLength(250);

            builder.Property(u => u.PhoneNumber).HasMaxLength(20);
            builder.HasIndex(u => u.PhoneNumber).IsUnique();
        }
    }
}