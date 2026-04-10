namespace Application.UseCases.Employees;

public interface IAssignProjectToEmployeeUseCase
{
    Task<AssignProjectsResult> ExecuteAsync(int employeeId, IReadOnlyCollection<int> projectIds);
}
