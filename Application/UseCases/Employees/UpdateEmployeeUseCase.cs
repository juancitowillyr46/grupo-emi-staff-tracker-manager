using Application.Contracts;
using Application.DTOs.Employees;

namespace Application.UseCases.Employees;

public sealed class UpdateEmployeeUseCase : IUpdateEmployeeUseCase
{
    private readonly IEmployeeRepository _employeeRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateEmployeeUseCase(IEmployeeRepository employeeRepository, IUnitOfWork unitOfWork)
    {
        _employeeRepository = employeeRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<EmployeeResponse?> ExecuteAsync(int id, UpdateEmployeeRequest request)
    {
        var employee = await _employeeRepository.GetByIdAsync(id);
        if (employee is null)
        {
            return null;
        }

        employee.UpdateDetails(request.Name, request.CurrentPosition, request.Salary, request.DepartmentId);

        await _unitOfWork.SaveChangesAsync();

        return new EmployeeResponse(
            employee.Id,
            request.Name,
            request.CurrentPosition,
            request.Salary,
            request.DepartmentId,
            employee.CalculateAnnualBonus(),
            employee.PositionHistories.Select(history => new PositionHistoryResponse(
                history.EmployeeId,
                history.Position,
                history.StartDate,
                history.EndDate)).ToList(),
            employee.EmployeeProjects.Select(employeeProject => employeeProject.ProjectId).ToList());
    }
}
