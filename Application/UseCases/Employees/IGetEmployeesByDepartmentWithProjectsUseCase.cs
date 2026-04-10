using Application.DTOs.Employees;

namespace Application.UseCases.Employees;

public interface IGetEmployeesByDepartmentWithProjectsUseCase
{
    Task<IReadOnlyCollection<EmployeeResponse>> ExecuteAsync(int departmentId);
}
