using Domain.Entities;

namespace Application.DTOs.Employees;

public static class EmployeeResponseMapper
{
    public static EmployeeResponse Map(Employee employee)
    {
        return new EmployeeResponse(
            employee.Id,
            employee.Name,
            employee.CurrentPosition,
            employee.CurrentPositionInfo?.Name ?? string.Empty,
            employee.Salary,
            employee.DepartmentId,
            employee.Department?.Name ?? string.Empty,
            employee.CalculateAnnualBonus(),
            employee.PositionHistories.Select(history => new PositionHistoryResponse(
                history.EmployeeId,
                history.Position,
                history.StartDate,
                history.EndDate)).ToList(),
            employee.EmployeeProjects
                .Select(employeeProject => employeeProject.Project)
                .Where(project => project is not null)
                .Select(project => new ReferenceResponse(project!.Id, project.Name))
                .ToList());
    }
}
