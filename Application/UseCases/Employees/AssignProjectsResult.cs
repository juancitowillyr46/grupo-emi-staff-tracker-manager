namespace Application.UseCases.Employees;

public enum AssignProjectsErrorType
{
    None = 0,
    EmployeeNotFound = 1,
    ProjectsNotFound = 2,
    DuplicateProjects = 3,
    InvalidRequest = 4
}

public sealed record AssignProjectsResult(
    bool Success,
    AssignProjectsErrorType ErrorType = AssignProjectsErrorType.None,
    string? ErrorMessage = null);
