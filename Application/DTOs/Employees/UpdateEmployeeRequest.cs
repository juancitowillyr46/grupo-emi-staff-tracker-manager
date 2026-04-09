namespace Application.DTOs.Employees;

public sealed record UpdateEmployeeRequest(
    string Name,
    int CurrentPosition,
    decimal Salary,
    int DepartmentId);
