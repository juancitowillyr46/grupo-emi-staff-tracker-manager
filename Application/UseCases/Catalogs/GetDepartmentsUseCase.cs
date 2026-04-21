using Application.Contracts;
using Application.DTOs.Employees;

namespace Application.UseCases.Catalogs;

public sealed class GetDepartmentsUseCase : IGetDepartmentsUseCase
{
    private readonly IDepartmentRepository _departmentRepository;

    public GetDepartmentsUseCase(IDepartmentRepository departmentRepository)
    {
        _departmentRepository = departmentRepository;
    }

    public async Task<IReadOnlyCollection<ReferenceResponse>> ExecuteAsync()
    {
        var departments = await _departmentRepository.GetAllAsync();
        return departments
            .Select(department => new ReferenceResponse(department.Id, department.Name))
            .ToList();
    }
}
