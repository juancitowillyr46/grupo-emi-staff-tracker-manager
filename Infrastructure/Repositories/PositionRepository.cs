using Application.Contracts;
using Domain.Entities;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public sealed class PositionRepository : IPositionRepository
{
    private readonly AppDbContext _context;

    public PositionRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Position>> GetAllAsync()
    {
        return await _context.Positions.ToListAsync();
    }

    public async Task<Position?> GetByIdAsync(int id)
    {
        return await _context.Positions.FirstOrDefaultAsync(x => x.Id == id);
    }
}
