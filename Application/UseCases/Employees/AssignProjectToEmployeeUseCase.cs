using Application.Contracts;

namespace Application.UseCases.Employees;

public sealed class AssignProjectToEmployeeUseCase : IAssignProjectToEmployeeUseCase
{
    private readonly IEmployeeRepository _employeeRepository;
    private readonly IUnitOfWork _unitOfWork;

    public AssignProjectToEmployeeUseCase(IEmployeeRepository employeeRepository, IUnitOfWork unitOfWork)
    {
        _employeeRepository = employeeRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<AssignProjectsResult> ExecuteAsync(int employeeId, IReadOnlyCollection<int> projectIds)
    {
        if (projectIds is null || projectIds.Count == 0)
        {
            return new AssignProjectsResult(
                false,
                AssignProjectsErrorType.InvalidRequest,
                "At least one projectId must be provided.");
        }

        var uniqueProjectIds = projectIds.Distinct().ToArray();
        if (uniqueProjectIds.Length != projectIds.Count)
        {
            return new AssignProjectsResult(
                false,
                AssignProjectsErrorType.DuplicateProjects,
                "ProjectIds cannot contain duplicates.");
        }

        var employee = await _employeeRepository.GetByIdAsync(employeeId);
        if (employee is null)
        {
            return new AssignProjectsResult(
                false,
                AssignProjectsErrorType.EmployeeNotFound,
                "Employee was not found.");
        }

        var alreadyAssignedProjectIds = await _employeeRepository.GetAssignedProjectIdsAsync(employeeId);
        if (uniqueProjectIds.Any(projectId => alreadyAssignedProjectIds.Contains(projectId)))
        {
            return new AssignProjectsResult(
                false,
                AssignProjectsErrorType.DuplicateProjects,
                "One or more projects are already assigned to this employee.");
        }

        var projects = await _employeeRepository.GetProjectsByIdsAsync(uniqueProjectIds);
        if (projects.Count != uniqueProjectIds.Length)
        {
            var foundIds = projects.Select(project => project.Id).ToHashSet();
            var missingIds = uniqueProjectIds.Where(projectId => !foundIds.Contains(projectId));
            var missingText = string.Join(", ", missingIds);

            return new AssignProjectsResult(
                false,
                AssignProjectsErrorType.ProjectsNotFound,
                $"The following projectIds were not found: {missingText}.");
        }

        foreach (var project in projects)
        {
            await _employeeRepository.AddEmployeeProjectAsync(employeeId, project.Id);
        }

        await _unitOfWork.SaveChangesAsync();
        return new AssignProjectsResult(true);
    }
}
