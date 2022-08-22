using MediatR;
using OperationResult;
using VDM.Pastelaria.Domain.Contracts.Repositories;
using VDM.Pastelaria.Shareable.Models.Requests;

namespace VDM.Pastelaria.Domain.Handlers;
public sealed class CriarPastelHandler : IRequestHandler<CriarPastelRequest, Result>
{
    private readonly IPastelRepository _pastelRepository;

    public CriarPastelHandler(IPastelRepository pastelRepository) => _pastelRepository = pastelRepository;

    public async Task<Result> Handle(CriarPastelRequest request, CancellationToken cancellationToken)
        => await _pastelRepository.CriarAsync(new(request.Sabor));
}