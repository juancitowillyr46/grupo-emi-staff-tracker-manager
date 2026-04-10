using Application.Contracts;
using Application.DTOs.Employees;

namespace Application.UseCases.Employees;

public sealed class UpdateEmployeeUseCase : IUpdateEmployeeUseCase
{
    private readonly IEmployeeRepository _employeeRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateEmployeeUseCase(IEmployeeRepository employeeRepository, IUnitOfWork unitOfWork)
    {
        _employeeRepository = employeeRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<EmployeeResponse?> ExecuteAsync(int id, UpdateEmployeeRequest request)
    {
        var employee = await _employeeRepository.GetByIdAsync(id);
        if (employee is null)
        {
            return null;
        }

        var position = await _employeeRepository.GetPositionByIdAsync(request.CurrentPosition);
        if (position is null)
        {
            throw new ArgumentException("Current position is invalid.", nameof(request.CurrentPosition));
        }

        employee.UpdateDetails(request.Name, request.CurrentPosition, request.Salary, request.DepartmentId);
        employee.SetCurrentPosition(position);

        await _unitOfWork.SaveChangesAsync();

        var updatedEmployee = await _employeeRepository.GetByIdAsync(employee.Id);
        return updatedEmployee is null ? EmployeeResponseMapper.Map(employee) : EmployeeResponseMapper.Map(updatedEmployee);
    }
}
