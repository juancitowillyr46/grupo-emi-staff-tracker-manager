namespace Application.DTOs.Employees;

public sealed record CreateEmployeeRequest(
    string Name,
    int CurrentPosition,
    decimal Salary,
    int DepartmentId);
