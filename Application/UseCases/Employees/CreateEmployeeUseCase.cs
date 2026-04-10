using Application.Contracts;
using Application.DTOs.Employees;
using Domain.Entities;

namespace Application.UseCases.Employees;

public sealed class CreateEmployeeUseCase : ICreateEmployeeUseCase
{
    private readonly IEmployeeRepository _employeeRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateEmployeeUseCase(IEmployeeRepository employeeRepository, IUnitOfWork unitOfWork)
    {
        _employeeRepository = employeeRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<EmployeeResponse> ExecuteAsync(CreateEmployeeRequest request)
    {
        var position = await _employeeRepository.GetPositionByIdAsync(request.CurrentPosition);
        if (position is null)
        {
            throw new ArgumentException("Current position is invalid.", nameof(request.CurrentPosition));
        }

        var employee = new Employee(
            request.Name,
            request.CurrentPosition,
            request.Salary,
            request.DepartmentId);

        employee.SetCurrentPosition(position);

        await _employeeRepository.AddAsync(employee);
        await _unitOfWork.SaveChangesAsync();

        var createdEmployee = await _employeeRepository.GetByIdAsync(employee.Id);
        return createdEmployee is null ? EmployeeResponseMapper.Map(employee) : EmployeeResponseMapper.Map(createdEmployee);
    }
}
