namespace Application.DTOs.Employees;

public sealed record PositionHistoryResponse(
    int EmployeeId,
    string Position,
    DateTime StartDate,
    DateTime? EndDate);
