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
            .Where(x => !x.IsDeleted)
            .Include(x => x.Department)
            .Include(x => x.CurrentPositionInfo)
            .Include(x => x.PositionHistories)
            .Include(x => x.EmployeeProjects)
            .ThenInclude(x => x.Project)
            .ToListAsync();
    }

    public async Task<Employee?> GetByIdAsync(int id)
    {
        return await _context.Employees
            .Where(x => !x.IsDeleted)
            .Include(x => x.Department)
            .Include(x => x.CurrentPositionInfo)
            .Include(x => x.PositionHistories)
            .Include(x => x.EmployeeProjects)
            .ThenInclude(x => x.Project)
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<IEnumerable<Employee>> GetByDepartmentAsync(int departmentId)
    {
        return await _context.Employees
            .Where(x => x.DepartmentId == departmentId && !x.IsDeleted)
            .Include(x => x.CurrentPositionInfo)
            .Include(x => x.PositionHistories)
            .Include(x => x.EmployeeProjects)
            .ThenInclude(x => x.Project)
            .ToListAsync();
    }

    public async Task<IEnumerable<Employee>> GetByDepartmentWithProjectsAsync(int departmentId)
    {
        return await _context.Employees
            .Where(x => x.DepartmentId == departmentId && !x.IsDeleted && x.EmployeeProjects.Any())
            .Include(x => x.Department)
            .Include(x => x.CurrentPositionInfo)
            .Include(x => x.PositionHistories)
            .Include(x => x.EmployeeProjects)
            .ThenInclude(x => x.Project)
            .ToListAsync();
    }

    public async Task<Project?> GetProjectByIdAsync(int projectId)
    {
        return await _context.Projects.FirstOrDefaultAsync(x => x.Id == projectId);
    }

    public async Task<Position?> GetPositionByIdAsync(int positionId)
    {
        return await _context.Positions.FirstOrDefaultAsync(x => x.Id == positionId);
    }

    public async Task<IReadOnlyCollection<Project>> GetProjectsByIdsAsync(IEnumerable<int> projectIds)
    {
        var ids = projectIds.ToArray();

        return await _context.Projects
            .Where(project => ids.Contains(project.Id))
            .ToListAsync();
    }

    public async Task<IReadOnlyCollection<int>> GetAssignedProjectIdsAsync(int employeeId)
    {
        return await _context.EmployeeProjects
            .Where(employeeProject => employeeProject.EmployeeId == employeeId)
            .Select(employeeProject => employeeProject.ProjectId)
            .ToListAsync();
    }

    public async Task AddAsync(Employee employee)
    {
        await _context.Employees.AddAsync(employee);
    }

    public async Task AddEmployeeProjectAsync(int employeeId, int projectId)
    {
        await _context.EmployeeProjects.AddAsync(new EmployeeProject(employeeId, projectId));
    }

    public void Update(Employee employee)
    {
        _context.Employees.Update(employee);
    }

    public void Delete(Employee employee)
    {
        employee.SoftDelete();
        _context.Employees.Update(employee);
    }
}
