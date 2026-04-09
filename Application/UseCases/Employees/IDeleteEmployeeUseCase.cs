namespace Application.UseCases.Employees;

public interface IDeleteEmployeeUseCase
{
    Task<bool> ExecuteAsync(int id);
}
