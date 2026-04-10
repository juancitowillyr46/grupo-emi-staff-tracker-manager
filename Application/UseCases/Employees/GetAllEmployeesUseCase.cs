using Application.Contracts;
using Application.DTOs.Employees;

namespace Application.UseCases.Employees;

public sealed class GetAllEmployeesUseCase : IGetAllEmployeesUseCase
{
    private readonly IEmployeeRepository _employeeRepository;

    public GetAllEmployeesUseCase(IEmployeeRepository employeeRepository)
    {
        _employeeRepository = employeeRepository;
    }

    public async Task<IReadOnlyCollection<EmployeeResponse>> ExecuteAsync()
    {
        var employees = await _employeeRepository.GetAllAsync();
        return employees.Select(EmployeeResponseMapper.Map).ToList();
    }
}
