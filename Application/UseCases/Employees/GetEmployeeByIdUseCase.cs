using Application.Contracts;
using Application.DTOs.Employees;

namespace Application.UseCases.Employees;

public sealed class GetEmployeeByIdUseCase : IGetEmployeeByIdUseCase
{
    private readonly IEmployeeRepository _employeeRepository;

    public GetEmployeeByIdUseCase(IEmployeeRepository employeeRepository)
    {
        _employeeRepository = employeeRepository;
    }

    public async Task<EmployeeResponse?> ExecuteAsync(int id)
    {
        var employee = await _employeeRepository.GetByIdAsync(id);
        return employee is null ? null : EmployeeResponseMapper.Map(employee);
    }
}
