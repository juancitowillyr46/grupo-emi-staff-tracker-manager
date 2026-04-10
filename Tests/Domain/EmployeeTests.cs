using Domain.Entities;

namespace StaffTrackerManager.Tests.Domain;

public class EmployeeTests
{
    public static IEnumerable<object[]> BonusCases =>
    [
        [1, 100m, 10m],
        [2, 100m, 20m]
    ];

    [Theory]
    [MemberData(nameof(BonusCases))]
    public void CalculateAnnualBonus_ShouldReturnExpectedValue(int positionId, decimal salary, decimal expectedBonus)
    {
        var employee = new Employee("John Doe", positionId, salary, 1);
        var positionType = positionId == 1 ? PositionType.Regular : PositionType.Manager;
        employee.SetCurrentPosition(new Position(positionId, "Test Position", positionType));

        var bonus = employee.CalculateAnnualBonus();

        Assert.Equal(expectedBonus, bonus);
    }

    [Fact]
    public void SoftDelete_ShouldMarkEmployeeAsDeleted()
    {
        var employee = new Employee("John Doe", 1, 100m, 1);

        employee.SoftDelete();

        Assert.True(employee.IsDeleted);
        Assert.NotNull(employee.DeletedAt);
    }
}
