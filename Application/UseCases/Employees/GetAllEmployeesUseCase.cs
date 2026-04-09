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
        return employees.Select(MapToResponse).ToList();
    }

    private static EmployeeResponse MapToResponse(Domain.Entities.Employee employee)
    {
        return new EmployeeResponse(
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
