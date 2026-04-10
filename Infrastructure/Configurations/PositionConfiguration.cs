using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations;

public sealed class PositionConfiguration : IEntityTypeConfiguration<Position>
{
    public void Configure(EntityTypeBuilder<Position> builder)
    {
        builder.ToTable("Positions");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(150);

        builder.Property(x => x.Type)
            .IsRequired();

        builder.HasData(
            new { Id = 1, Name = "Employee", Type = PositionType.Regular },
            new { Id = 2, Name = "Team Lead", Type = PositionType.Manager },
            new { Id = 3, Name = "Engineering Manager", Type = PositionType.Manager },
            new { Id = 4, Name = "Sales Manager", Type = PositionType.Manager });
    }
}
