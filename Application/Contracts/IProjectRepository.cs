using Domain.Entities;

namespace Application.Contracts;

public interface IProjectRepository
{
    Task<IEnumerable<Project>> GetAllAsync();
    Task<Project?> GetByIdAsync(int id);
    Task AddAsync(Project project);
    void Update(Project project);
    void Delete(Project project);
}
