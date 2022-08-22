using MediatR;
using OperationResult;

namespace VDM.Pastelaria.Shareable.Models.Requests;
public record ListarSaboresPasteisRequest()
    : IRequest<Result<string[]>>;