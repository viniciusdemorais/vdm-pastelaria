using MediatR;
using OperationResult;
using System.Net;
using VDM.Pastelaria.Shareable.Exceptions;
using VDM.Pastelaria.Shareable.Models.Responses;

namespace VDM.Pastelaria.Api.Extensions;

public static class EndpointExtension
{
    public static async Task<IResult> SendCommand<T>(this IMediator mediator, IRequest<Result<T>> request)
       => await mediator.Send(request) switch
       {
           (true, var result, _) => Results.Ok(result),
           var (_, _, error) => HandleError(error!)
       };

    public static async Task<IResult> SendCommand(this IMediator mediator, IRequest<Result> request, int statusCode = 200)
        => await mediator.Send(request) switch
        {
            (true, _) => Results.StatusCode(statusCode),
            var (_, error) => HandleError(error!)
        };

    private static IResult HandleError(Exception error)
        => error switch
        {
            DadosRequestInvalidosException e => Results.BadRequest(e.Erros),
            DadosNaoEncontradosException e => new StatusCodeResult<ErroResponse>((int)HttpStatusCode.NotFound, new ErroResponse(e)),
            AppException e => new StatusCodeResult<ErroResponse>((int)HttpStatusCode.UnprocessableEntity, new ErroResponse(e)),
            _ => Results.StatusCode(500)
        };

    private readonly record struct StatusCodeResult<T>(int StatusCode, T Value) : IResult
    {
        public Task ExecuteAsync(HttpContext httpContext)
        {
            httpContext.Response.StatusCode = StatusCode;
            return httpContext.Response.WriteAsJsonAsync(Value, Value!.GetType(), options: null, contentType: "application/json");
        }
    }
}