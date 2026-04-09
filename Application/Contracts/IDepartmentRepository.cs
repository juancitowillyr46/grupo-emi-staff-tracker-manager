using Domain.Entities;

namespace Application.Contracts;

public interface IDepartmentRepository
{
    Task<IEnumerable<Department>> GetAllAsync();
    Task<Department?> GetByIdAsync(int id);
    Task AddAsync(Department department);
    void Update(Department department);
    void Delete(Department department);
}
