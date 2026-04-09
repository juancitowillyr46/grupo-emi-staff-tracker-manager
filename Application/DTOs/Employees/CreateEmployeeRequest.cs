namespace Application.DTOs.Employees;

public sealed record CreateEmployeeRequest(
    int Id,
    string Name,
    int CurrentPosition,
    decimal Salary,
    int DepartmentId);
