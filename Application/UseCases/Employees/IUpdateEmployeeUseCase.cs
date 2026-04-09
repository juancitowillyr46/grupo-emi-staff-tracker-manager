using Application.DTOs.Employees;

namespace Application.UseCases.Employees;

public interface IUpdateEmployeeUseCase
{
    Task<EmployeeResponse?> ExecuteAsync(int id, UpdateEmployeeRequest request);
}
