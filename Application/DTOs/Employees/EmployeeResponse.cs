namespace Application.DTOs.Employees;

public sealed record EmployeeResponse(
    int Id,
    string Name,
    int CurrentPosition,
    string CurrentPositionName,
    decimal Salary,
    int DepartmentId,
    string DepartmentName,
    decimal AnnualBonus,
    IReadOnlyCollection<PositionHistoryResponse> PositionHistories,
    IReadOnlyCollection<ReferenceResponse> Projects);
