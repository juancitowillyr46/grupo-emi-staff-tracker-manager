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
        return employee is null ? null : new EmployeeResponse(
            employee.Id,
            employee.Name,
            employee.CurrentPosition,
            employee.Salary,
            employee.DepartmentId,
            employee.CalculateAnnualBonus(),
            employee.PositionHistories.Select(history => new PositionHistoryResponse(
                history.EmployeeId,
                history.Position,
                history.StartDate,
                history.EndDate)).ToList(),
            employee.EmployeeProjects.Select(employeeProject => employeeProject.ProjectId).ToList());
    }
}
