using Application.DTOs.Employees;

namespace Application.UseCases.Catalogs;

public interface IGetDepartmentsUseCase
{
    Task<IReadOnlyCollection<ReferenceResponse>> ExecuteAsync();
}
