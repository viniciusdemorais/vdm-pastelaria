using MediatR;
using Microsoft.Extensions.Logging;
using OperationResult;
using System.Diagnostics;
using System.Reflection;

namespace VDM.Pastelaria.Domain.Pipelines;
public sealed class ExceptionPipelineBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly MethodInfo? _operationResultError;
    private readonly Type _type = typeof(TResponse);
    private readonly Type _typeOperationResult = typeof(Result);
    private readonly ILogger<ExceptionPipelineBehavior<TRequest, TResponse>> _logger;

    public ExceptionPipelineBehavior(ILogger<ExceptionPipelineBehavior<TRequest, TResponse>> logger)
    {
        if (_type.IsGenericType)
        {
            _operationResultError = _typeOperationResult.GetMethod(nameof(Result.Error), 1, new[] { typeof(Exception) })!;
            _operationResultError = _operationResultError.MakeGenericMethod(_type.GetGenericArguments());
        }

        _logger = logger;
    }

    public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
    {
        var eventId = default(EventId);
        try
        {
            if (_logger.IsEnabled(LogLevel.Debug))
                _logger.LogInformation(eventId, "Handling request of type {TypeName} with data {@Data}", typeof(TRequest).FullName, request);
            else
                _logger.LogInformation(eventId, "Handling request of type {TypeName}", typeof(TRequest).FullName);

            var sp = Stopwatch.StartNew();
            var response = await next.Invoke();
            sp.Stop();
            _logger.LogInformation(eventId, "Handled request of type {TypeName} in {Elapsed}", typeof(TRequest).FullName, sp.Elapsed);
            return response;
        }
        catch (Exception e)
        {
            _logger.LogError(eventId, e, "Error on request handling");
            return _type == _typeOperationResult
                ? (TResponse)Convert.ChangeType(Result.Error(e), _type)
                : (TResponse)Convert.ChangeType(_operationResultError!.Invoke(null, new object[] { e })!, _type)!;
        }
    }
}