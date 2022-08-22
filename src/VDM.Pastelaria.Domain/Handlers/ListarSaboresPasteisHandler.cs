using MediatR;
using Microsoft.Extensions.Logging;
using OperationResult;
using VDM.Pastelaria.Domain.Contracts.Repositories;
using VDM.Pastelaria.Shareable.Exceptions;
using VDM.Pastelaria.Shareable.Models.Requests;

namespace VDM.Pastelaria.Domain.Handlers;
public sealed class ListarSaboresPasteisHandler : IRequestHandler<ListarSaboresPasteisRequest, Result<string[]>>
{
    private readonly ILogger<ListarSaboresPasteisHandler> _logger;
    private readonly IPastelRepository _pastelRepository;

    public ListarSaboresPasteisHandler(ILogger<ListarSaboresPasteisHandler> logger, IPastelRepository pastelRepository)
    {
        _logger = logger;
        _pastelRepository = pastelRepository;
    }

    public async Task<Result<string[]>> Handle(ListarSaboresPasteisRequest _, CancellationToken cancellationToken)
    {
        var pasteis = (await _pastelRepository.ListarAsync()).Select(p => p.Sabor).ToArray();
        _logger.LogDebug("Pasteis encontrados {@pasteis}", (object)pasteis);
        return pasteis.Any()
            ? pasteis
            : new DadosNaoEncontradosException("Não existem Pasteis");
    }
}