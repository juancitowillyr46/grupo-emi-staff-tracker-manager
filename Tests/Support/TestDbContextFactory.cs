using Domain.Entities;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace StaffTrackerManager.Tests.Support;

internal static class TestDbContextFactory
{
    public static AppDbContext CreateContext()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        var context = new AppDbContext(options);
        Seed(context);
        return context;
    }

    private static void Seed(AppDbContext context)
    {
        context.Departments.AddRange(
            new Department(1, "IT"),
            new Department(2, "HR"));

        context.Positions.AddRange(
            new Position(1, "Employee", PositionType.Regular),
            new Position(2, "Team Lead", PositionType.Manager),
            new Position(3, "Engineering Manager", PositionType.Manager));

        context.Projects.AddRange(
            new Project(1, "Website"),
            new Project(2, "Mobile App"),
            new Project(3, "ERP"));

        context.SaveChanges();
    }
}
