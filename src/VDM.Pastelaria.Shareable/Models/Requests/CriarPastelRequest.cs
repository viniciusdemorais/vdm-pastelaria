using MediatR;
using OperationResult;

namespace VDM.Pastelaria.Shareable.Models.Requests;
public record CriarPastelRequest(string Sabor)
    : IRequest<Result>;