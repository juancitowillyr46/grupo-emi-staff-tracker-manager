using Application.Contracts;

namespace Application.UseCases.Employees;

public sealed class DeleteEmployeeUseCase : IDeleteEmployeeUseCase
{
    private readonly IEmployeeRepository _employeeRepository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteEmployeeUseCase(IEmployeeRepository employeeRepository, IUnitOfWork unitOfWork)
    {
        _employeeRepository = employeeRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> ExecuteAsync(int id)
    {
        var employee = await _employeeRepository.GetByIdAsync(id);
        if (employee is null)
        {
            return false;
        }

        _employeeRepository.Delete(employee);
        await _unitOfWork.SaveChangesAsync();
        return true;
    }
}
