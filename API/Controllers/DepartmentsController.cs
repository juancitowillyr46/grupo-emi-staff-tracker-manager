using Application.DTOs.Employees;
using Application.UseCases.Catalogs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DepartmentsController : ControllerBase
{
    private readonly IGetDepartmentsUseCase _getDepartmentsUseCase;

    public DepartmentsController(IGetDepartmentsUseCase getDepartmentsUseCase)
    {
        _getDepartmentsUseCase = getDepartmentsUseCase;
    }

    /// <summary>
    /// Gets all departments.
    /// </summary>
    [HttpGet]
    [Authorize(Roles = "Admin,User")]
    public async Task<ActionResult<IReadOnlyCollection<ReferenceResponse>>> GetAll()
    {
        var departments = await _getDepartmentsUseCase.ExecuteAsync();
        return Ok(departments);
    }
}
