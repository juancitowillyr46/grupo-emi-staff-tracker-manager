using Application.DTOs.Employees;
using Application.UseCases.Employees;
using Domain.Entities;
using Infrastructure;
using Infrastructure.Persistence;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using StaffTrackerManager.Tests.Support;

namespace StaffTrackerManager.Tests.Application;

public class EmployeeUseCasesTests
{
    [Fact]
    public async Task CreateEmployeeUseCase_ShouldReturnResponseWithNamesAndBonus()
    {
        using var context = TestDbContextFactory.CreateContext();
        var repository = new EmployeeRepository(context);
        var unitOfWork = new UnitOfWork(context);
        var useCase = new CreateEmployeeUseCase(repository, unitOfWork);

        var request = new CreateEmployeeRequest("John Doe", 2, 100m, 1);

        var response = await useCase.ExecuteAsync(request);

        Assert.Equal("John Doe", response.Name);
        Assert.Equal("Team Lead", response.CurrentPositionName);
        Assert.Equal("IT", response.DepartmentName);
        Assert.Equal(20m, response.AnnualBonus);
        Assert.Single(response.PositionHistories);
        Assert.Equal("Team Lead", response.PositionHistories.Single().Position);
        Assert.Null(response.PositionHistories.Single().EndDate);
        Assert.Empty(response.Projects);
    }

    [Fact]
    public async Task AssignProjectToEmployeeUseCase_ShouldAssignMultipleProjects()
    {
        using var context = TestDbContextFactory.CreateContext();
        var position = context.Positions.Single(x => x.Id == 2);
        var employee = new Employee("John Doe", 2, 100m, 1);
        employee.SetCurrentPosition(position);
        context.Employees.Add(employee);
        await context.SaveChangesAsync();

        var repository = new EmployeeRepository(context);
        var unitOfWork = new UnitOfWork(context);
        var useCase = new AssignProjectToEmployeeUseCase(repository, unitOfWork);

        var result = await useCase.ExecuteAsync(employee.Id, new[] { 1, 2, 3 });

        Assert.True(result.Success);
        Assert.Equal(3, await context.EmployeeProjects.CountAsync());
    }

    [Fact]
    public async Task AssignProjectToEmployeeUseCase_ShouldRejectDuplicateAssignments()
    {
        using var context = TestDbContextFactory.CreateContext();
        var position = context.Positions.Single(x => x.Id == 2);
        var employee = new Employee("John Doe", 2, 100m, 1);
        employee.SetCurrentPosition(position);
        context.Employees.Add(employee);
        await context.SaveChangesAsync();

        var repository = new EmployeeRepository(context);
        var unitOfWork = new UnitOfWork(context);
        var useCase = new AssignProjectToEmployeeUseCase(repository, unitOfWork);

        await useCase.ExecuteAsync(employee.Id, new[] { 1, 2 });
        var result = await useCase.ExecuteAsync(employee.Id, new[] { 2, 3 });

        Assert.False(result.Success);
        Assert.Equal(AssignProjectsErrorType.DuplicateProjects, result.ErrorType);
    }

    [Fact]
    public async Task GetEmployeesByDepartmentWithProjectsUseCase_ShouldReturnOnlyEmployeesWithProjects()
    {
        using var context = TestDbContextFactory.CreateContext();

        var teamLead = context.Positions.Single(x => x.Id == 2);
        var regular = context.Positions.Single(x => x.Id == 1);

        var employeeWithProject = new Employee("John Doe", 2, 100m, 1);
        employeeWithProject.SetCurrentPosition(teamLead);

        var employeeWithoutProject = new Employee("Jane Doe", 1, 80m, 1);
        employeeWithoutProject.SetCurrentPosition(regular);

        var otherDepartmentEmployee = new Employee("Mark Doe", 2, 90m, 2);
        otherDepartmentEmployee.SetCurrentPosition(teamLead);

        context.Employees.AddRange(employeeWithProject, employeeWithoutProject, otherDepartmentEmployee);
        await context.SaveChangesAsync();

        var repository = new EmployeeRepository(context);
        var useCase = new GetEmployeesByDepartmentWithProjectsUseCase(repository);

        await repository.AddEmployeeProjectAsync(employeeWithProject.Id, 1);
        await repository.AddEmployeeProjectAsync(otherDepartmentEmployee.Id, 2);
        await context.SaveChangesAsync();

        var result = await useCase.ExecuteAsync(1);

        Assert.Single(result);
        var employee = result.Single();
        Assert.Equal("John Doe", employee.Name);
        Assert.Equal("Team Lead", employee.CurrentPositionName);
        Assert.Equal("IT", employee.DepartmentName);
        Assert.Single(employee.Projects);
        Assert.Equal("Website", employee.Projects.Single().Name);
    }

    [Fact]
    public async Task UpdateEmployeeUseCase_ShouldTrackPositionHistoryWhenPositionChanges()
    {
        using var context = TestDbContextFactory.CreateContext();
        var teamLead = context.Positions.Single(x => x.Id == 2);

        var employee = new Employee("John Doe", 2, 100m, 1);
        employee.InitializeCurrentPosition(teamLead, new DateTime(2026, 1, 1));
        context.Employees.Add(employee);
        await context.SaveChangesAsync();

        var repository = new EmployeeRepository(context);
        var unitOfWork = new UnitOfWork(context);
        var useCase = new UpdateEmployeeUseCase(repository, unitOfWork);

        var request = new UpdateEmployeeRequest("John Doe Updated", 3, 120m, 1);
        var response = await useCase.ExecuteAsync(employee.Id, request);

        Assert.NotNull(response);
        Assert.Equal("John Doe Updated", response!.Name);
        Assert.Equal("Engineering Manager", response.CurrentPositionName);
        Assert.Equal(2, response.PositionHistories.Count);

        var firstHistory = response.PositionHistories.OrderBy(x => x.StartDate).First();
        var secondHistory = response.PositionHistories.OrderBy(x => x.StartDate).Last();

        Assert.Equal("Team Lead", firstHistory.Position);
        Assert.NotNull(firstHistory.EndDate);
        Assert.Equal("Engineering Manager", secondHistory.Position);
        Assert.Null(secondHistory.EndDate);
    }
}
