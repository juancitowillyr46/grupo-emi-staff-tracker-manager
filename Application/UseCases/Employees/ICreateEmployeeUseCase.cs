using Application.DTOs.Employees;

namespace Application.UseCases.Employees;

public interface ICreateEmployeeUseCase
{
    Task<EmployeeResponse> ExecuteAsync(CreateEmployeeRequest request);
}
