using Application.DTOs.Employees;
using Application.UseCases.Employees;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EmployeesController : ControllerBase
{
    private readonly IGetAllEmployeesUseCase _getAllEmployeesUseCase;
    private readonly IGetEmployeeByIdUseCase _getEmployeeByIdUseCase;
    private readonly IGetEmployeesByDepartmentWithProjectsUseCase _getEmployeesByDepartmentWithProjectsUseCase;
    private readonly IAssignProjectToEmployeeUseCase _assignProjectToEmployeeUseCase;
    private readonly ICreateEmployeeUseCase _createEmployeeUseCase;
    private readonly IUpdateEmployeeUseCase _updateEmployeeUseCase;
    private readonly IDeleteEmployeeUseCase _deleteEmployeeUseCase;

    public EmployeesController(
        IGetAllEmployeesUseCase getAllEmployeesUseCase,
        IGetEmployeeByIdUseCase getEmployeeByIdUseCase,
        IGetEmployeesByDepartmentWithProjectsUseCase getEmployeesByDepartmentWithProjectsUseCase,
        IAssignProjectToEmployeeUseCase assignProjectToEmployeeUseCase,
        ICreateEmployeeUseCase createEmployeeUseCase,
        IUpdateEmployeeUseCase updateEmployeeUseCase,
        IDeleteEmployeeUseCase deleteEmployeeUseCase)
    {
        _getAllEmployeesUseCase = getAllEmployeesUseCase;
        _getEmployeeByIdUseCase = getEmployeeByIdUseCase;
        _getEmployeesByDepartmentWithProjectsUseCase = getEmployeesByDepartmentWithProjectsUseCase;
        _assignProjectToEmployeeUseCase = assignProjectToEmployeeUseCase;
        _createEmployeeUseCase = createEmployeeUseCase;
        _updateEmployeeUseCase = updateEmployeeUseCase;
        _deleteEmployeeUseCase = deleteEmployeeUseCase;
    }

    /// <summary>
    /// Gets all employees.
    /// </summary>
    [HttpGet]
    [Authorize(Roles = "Admin,User")]
    public async Task<ActionResult<IReadOnlyCollection<EmployeeResponse>>> GetAll()
    {
        var employees = await _getAllEmployeesUseCase.ExecuteAsync();
        return Ok(employees);
    }

    /// <summary>
    /// Gets an employee by id.
    /// </summary>
    [HttpGet("{id:int}")]
    [Authorize(Roles = "Admin,User")]
    public async Task<ActionResult<EmployeeResponse>> GetById(int id)
    {
        var employee = await _getEmployeeByIdUseCase.ExecuteAsync(id);
        return employee is null ? NotFound() : Ok(employee);
    }

    /// <summary>
    /// Gets employees for a department that have at least one project assigned.
    /// </summary>
    [HttpGet("department/{departmentId:int}/with-projects")]
    [Authorize(Roles = "Admin,User")]
    public async Task<ActionResult<IReadOnlyCollection<EmployeeResponse>>> GetByDepartmentWithProjects(int departmentId)
    {
        var employees = await _getEmployeesByDepartmentWithProjectsUseCase.ExecuteAsync(departmentId);
        return Ok(employees);
    }

    /// <summary>
    /// Assigns one or more projects to an employee.
    /// </summary>
    [HttpPost("{employeeId:int}/projects")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> AssignProjects(int employeeId, [FromBody] AssignProjectRequest request)
    {
        var result = await _assignProjectToEmployeeUseCase.ExecuteAsync(employeeId, request.ProjectIds);

        return result.ErrorType switch
        {
            AssignProjectsErrorType.None => NoContent(),
            AssignProjectsErrorType.EmployeeNotFound => NotFound(result.ErrorMessage),
            AssignProjectsErrorType.ProjectsNotFound => NotFound(result.ErrorMessage),
            AssignProjectsErrorType.DuplicateProjects => BadRequest(result.ErrorMessage),
            AssignProjectsErrorType.InvalidRequest => BadRequest(result.ErrorMessage),
            _ => BadRequest(result.ErrorMessage)
        };
    }

    /// <summary>
    /// Creates a new employee.
    /// </summary>
    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<EmployeeResponse>> Create([FromBody] CreateEmployeeRequest request)
    {
        try
        {
            var created = await _createEmployeeUseCase.ExecuteAsync(request);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    /// <summary>
    /// Updates an employee.
    /// </summary>
    [HttpPut("{id:int}")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<EmployeeResponse>> Update(int id, [FromBody] UpdateEmployeeRequest request)
    {
        try
        {
            var updated = await _updateEmployeeUseCase.ExecuteAsync(id, request);
            return updated is null ? NotFound() : Ok(updated);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    /// <summary>
    /// Deletes an employee.
    /// </summary>
    [HttpDelete("{id:int}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await _deleteEmployeeUseCase.ExecuteAsync(id);
        return deleted ? NoContent() : NotFound();
    }
}
