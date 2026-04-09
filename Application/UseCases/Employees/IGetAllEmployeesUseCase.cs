using Application.DTOs.Employees;

namespace Application.UseCases.Employees;

public interface IGetAllEmployeesUseCase
{
    Task<IReadOnlyCollection<EmployeeResponse>> ExecuteAsync();
}
