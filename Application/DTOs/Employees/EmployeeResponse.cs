namespace Application.DTOs.Employees;

public sealed record EmployeeResponse(
    int Id,
    string Name,
    int CurrentPosition,
    decimal Salary,
    int DepartmentId,
    decimal AnnualBonus,
    IReadOnlyCollection<PositionHistoryResponse> PositionHistories,
    IReadOnlyCollection<int> ProjectIds);
