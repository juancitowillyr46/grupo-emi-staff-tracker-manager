using Domain.Entities;
using Infrastructure.Security;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations;

public sealed class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("Users");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Username)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(x => x.PasswordHash)
            .IsRequired()
            .HasMaxLength(256);

        builder.Property(x => x.Role)
            .IsRequired()
            .HasMaxLength(20);

        builder.HasIndex(x => x.Username)
            .IsUnique();

        builder.HasData(
            new { Id = 1, Username = "admin", PasswordHash = PasswordHasher.Hash("Admin123!"), Role = "Admin" },
            new { Id = 2, Username = "user", PasswordHash = PasswordHasher.Hash("User123!"), Role = "User" });
    }
}
