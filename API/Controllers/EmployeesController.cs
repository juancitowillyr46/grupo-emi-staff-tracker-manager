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
    private readonly ICreateEmployeeUseCase _createEmployeeUseCase;
    private readonly IUpdateEmployeeUseCase _updateEmployeeUseCase;
    private readonly IDeleteEmployeeUseCase _deleteEmployeeUseCase;

    public EmployeesController(
        IGetAllEmployeesUseCase getAllEmployeesUseCase,
        IGetEmployeeByIdUseCase getEmployeeByIdUseCase,
        ICreateEmployeeUseCase createEmployeeUseCase,
        IUpdateEmployeeUseCase updateEmployeeUseCase,
        IDeleteEmployeeUseCase deleteEmployeeUseCase)
    {
        _getAllEmployeesUseCase = getAllEmployeesUseCase;
        _getEmployeeByIdUseCase = getEmployeeByIdUseCase;
        _createEmployeeUseCase = createEmployeeUseCase;
        _updateEmployeeUseCase = updateEmployeeUseCase;
        _deleteEmployeeUseCase = deleteEmployeeUseCase;
    }

    [HttpGet]
    [Authorize(Roles = "Admin,User")]
    public async Task<ActionResult<IReadOnlyCollection<EmployeeResponse>>> GetAll()
    {
        var employees = await _getAllEmployeesUseCase.ExecuteAsync();
        return Ok(employees);
    }

    [HttpGet("{id:int}")]
    [Authorize(Roles = "Admin,User")]
    public async Task<ActionResult<EmployeeResponse>> GetById(int id)
    {
        var employee = await _getEmployeeByIdUseCase.ExecuteAsync(id);
        return employee is null ? NotFound() : Ok(employee);
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<EmployeeResponse>> Create([FromBody] CreateEmployeeRequest request)
    {
        var created = await _createEmployeeUseCase.ExecuteAsync(request);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    [HttpPut("{id:int}")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<EmployeeResponse>> Update(int id, [FromBody] UpdateEmployeeRequest request)
    {
        var updated = await _updateEmployeeUseCase.ExecuteAsync(id, request);
        return updated is null ? NotFound() : Ok(updated);
    }

    [HttpDelete("{id:int}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await _deleteEmployeeUseCase.ExecuteAsync(id);
        return deleted ? NoContent() : NotFound();
    }
}
