using Application.DTOs.Employees;
using Application.UseCases.Catalogs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProjectsController : ControllerBase
{
    private readonly IGetProjectsUseCase _getProjectsUseCase;

    public ProjectsController(IGetProjectsUseCase getProjectsUseCase)
    {
        _getProjectsUseCase = getProjectsUseCase;
    }

    /// <summary>
    /// Gets all projects.
    /// </summary>
    [HttpGet]
    [Authorize(Roles = "Admin,User")]
    public async Task<ActionResult<IReadOnlyCollection<ReferenceResponse>>> GetAll()
    {
        var projects = await _getProjectsUseCase.ExecuteAsync();
        return Ok(projects);
    }
}
