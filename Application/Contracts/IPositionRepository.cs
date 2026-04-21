using Domain.Entities;

namespace Application.Contracts;

public interface IPositionRepository
{
    Task<IEnumerable<Position>> GetAllAsync();
    Task<Position?> GetByIdAsync(int id);
}
