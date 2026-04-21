using Application.DTOs.Employees;
using Application.UseCases.Catalogs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PositionsController : ControllerBase
{
    private readonly IGetPositionsUseCase _getPositionsUseCase;

    public PositionsController(IGetPositionsUseCase getPositionsUseCase)
    {
        _getPositionsUseCase = getPositionsUseCase;
    }

    /// <summary>
    /// Gets all positions.
    /// </summary>
    [HttpGet]
    [Authorize(Roles = "Admin,User")]
    public async Task<ActionResult<IReadOnlyCollection<ReferenceResponse>>> GetAll()
    {
        var positions = await _getPositionsUseCase.ExecuteAsync();
        return Ok(positions);
    }
}
