using Domain.Entities;

namespace Application.Contracts;

public interface IEmployeeRepository
{
    Task<IEnumerable<Employee>> GetAllAsync();
    Task<Employee?> GetByIdAsync(int id);
    Task<IEnumerable<Employee>> GetByDepartmentAsync(int departmentId);
    Task<IEnumerable<Employee>> GetByDepartmentWithProjectsAsync(int departmentId);
    Task<Project?> GetProjectByIdAsync(int projectId);
    Task<IReadOnlyCollection<Project>> GetProjectsByIdsAsync(IEnumerable<int> projectIds);
    Task<IReadOnlyCollection<int>> GetAssignedProjectIdsAsync(int employeeId);
    Task AddAsync(Employee employee);
    Task AddEmployeeProjectAsync(int employeeId, int projectId);
    void Update(Employee employee);
    void Delete(Employee employee);
}
