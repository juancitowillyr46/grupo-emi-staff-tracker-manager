using Application.Contracts;
using Application.DTOs.Employees;

namespace Application.UseCases.Employees;

public sealed class GetEmployeesByDepartmentWithProjectsUseCase : IGetEmployeesByDepartmentWithProjectsUseCase
{
    private readonly IEmployeeRepository _employeeRepository;

    public GetEmployeesByDepartmentWithProjectsUseCase(IEmployeeRepository employeeRepository)
    {
        _employeeRepository = employeeRepository;
    }

    public async Task<IReadOnlyCollection<EmployeeResponse>> ExecuteAsync(int departmentId)
    {
        var employees = await _employeeRepository.GetByDepartmentWithProjectsAsync(departmentId);
        return employees.Select(EmployeeResponseMapper.Map).ToList();
    }
}
