using Application.Contracts;
using Application.DTOs.Employees;

namespace Application.UseCases.Catalogs;

public sealed class GetProjectsUseCase : IGetProjectsUseCase
{
    private readonly IProjectRepository _projectRepository;

    public GetProjectsUseCase(IProjectRepository projectRepository)
    {
        _projectRepository = projectRepository;
    }

    public async Task<IReadOnlyCollection<ReferenceResponse>> ExecuteAsync()
    {
        var projects = await _projectRepository.GetAllAsync();
        return projects
            .Select(project => new ReferenceResponse(project.Id, project.Name))
            .ToList();
    }
}
