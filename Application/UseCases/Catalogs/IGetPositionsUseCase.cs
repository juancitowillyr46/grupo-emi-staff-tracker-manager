using Application.DTOs.Employees;

namespace Application.UseCases.Catalogs;

public interface IGetPositionsUseCase
{
    Task<IReadOnlyCollection<ReferenceResponse>> ExecuteAsync();
}
