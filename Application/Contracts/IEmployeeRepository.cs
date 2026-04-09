using Domain.Entities;

namespace Application.Contracts;

public interface IEmployeeRepository
{
    Task<IEnumerable<Employee>> GetAllAsync();
    Task<Employee?> GetByIdAsync(int id);
    Task<IEnumerable<Employee>> GetByDepartmentAsync(int departmentId);
    Task AddAsync(Employee employee);
    void Update(Employee employee);
    void Delete(Employee employee);
}
