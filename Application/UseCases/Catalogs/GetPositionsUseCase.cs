using Application.Contracts;
using Application.DTOs.Employees;

namespace Application.UseCases.Catalogs;

public sealed class GetPositionsUseCase : IGetPositionsUseCase
{
    private readonly IPositionRepository _positionRepository;

    public GetPositionsUseCase(IPositionRepository positionRepository)
    {
        _positionRepository = positionRepository;
    }

    public async Task<IReadOnlyCollection<ReferenceResponse>> ExecuteAsync()
    {
        var positions = await _positionRepository.GetAllAsync();
        return positions
            .Select(position => new ReferenceResponse(position.Id, position.Name))
            .ToList();
    }
}
