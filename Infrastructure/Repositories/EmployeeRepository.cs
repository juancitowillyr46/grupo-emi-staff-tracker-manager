using Application.Contracts;
using Domain.Entities;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public sealed class EmployeeRepository : IEmployeeRepository
{
    private readonly AppDbContext _context;

    public EmployeeRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Employee>> GetAllAsync()
    {
        return await _context.Employees
            .Include(x => x.Department)
            .Include(x => x.PositionHistories)
            .Include(x => x.EmployeeProjects)
            .ToListAsync();
    }

    public async Task<Employee?> GetByIdAsync(int id)
    {
        return await _context.Employees
            .Include(x => x.Department)
            .Include(x => x.PositionHistories)
            .Include(x => x.EmployeeProjects)
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<IEnumerable<Employee>> GetByDepartmentAsync(int departmentId)
    {
        return await _context.Employees
            .Where(x => x.DepartmentId == departmentId)
            .Include(x => x.PositionHistories)
            .Include(x => x.EmployeeProjects)
            .ToListAsync();
    }

    public async Task<IEnumerable<Employee>> GetByDepartmentWithProjectsAsync(int departmentId)
    {
        return await _context.Employees
            .Where(x => x.DepartmentId == departmentId && x.EmployeeProjects.Any())
            .Include(x => x.Department)
            .Include(x => x.PositionHistories)
            .Include(x => x.EmployeeProjects)
            .ToListAsync();
    }

    public async Task AddAsync(Employee employee)
    {
        await _context.Employees.AddAsync(employee);
    }

    public void Update(Employee employee)
    {
        _context.Employees.Update(employee);
    }

    public void Delete(Employee employee)
    {
        _context.Employees.Remove(employee);
    }
}
