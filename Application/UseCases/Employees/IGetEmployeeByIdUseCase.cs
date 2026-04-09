using Application.DTOs.Employees;

namespace Application.UseCases.Employees;

public interface IGetEmployeeByIdUseCase
{
    Task<EmployeeResponse?> ExecuteAsync(int id);
}
