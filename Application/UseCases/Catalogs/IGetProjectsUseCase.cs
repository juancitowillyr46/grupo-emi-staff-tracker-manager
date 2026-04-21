using Application.DTOs.Employees;

namespace Application.UseCases.Catalogs;

public interface IGetProjectsUseCase
{
    Task<IReadOnlyCollection<ReferenceResponse>> ExecuteAsync();
}
