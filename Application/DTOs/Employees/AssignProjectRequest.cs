namespace Application.DTOs.Employees;

public sealed record AssignProjectRequest(IReadOnlyCollection<int> ProjectIds);
