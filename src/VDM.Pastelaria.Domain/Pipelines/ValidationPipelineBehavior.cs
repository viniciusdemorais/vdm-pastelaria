using FluentValidation;
using MediatR;
using OperationResult;
using System.Reflection;
using VDM.Pastelaria.Shareable.Exceptions;
using VDM.Pastelaria.Shareable.Models.Validation;

namespace VDM.Pastelaria.Domain.Pipelines;
public sealed class ValidationPipelineBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>, IValidatable
{
#pragma warning disable S2743 // Static fields should not be used in generic types
    private static readonly Type _type;
    private static readonly MethodInfo? _operationResultError;
#pragma warning restore S2743 // Static fields should not be used in generic types
    private readonly AbstractValidator<TRequest> _validator;

    static ValidationPipelineBehavior()
    {
        _type = typeof(TResponse);
        if (_type.IsGenericType)
            _operationResultError = ValidationPipelineBehaviorMeta._genericErrorMethod.MakeGenericMethod(_type.GetGenericArguments());
    }

    public ValidationPipelineBehavior(AbstractValidator<TRequest> validator)
        => _validator = validator;

    public Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
    {
        var validationResult = _validator.Validate(request);
        if (validationResult.IsValid)
            return next.Invoke();

        var errors = validationResult.Errors.GroupBy(v => v.PropertyName, v => v.ErrorMessage).ToDictionary(v => v.Key, v => v.Select(y => y));
        var validationError = new DadosRequestInvalidosException(errors);
        return _type == ValidationPipelineBehaviorMeta._typeOperationResult
            ? Task.FromResult((TResponse)Convert.ChangeType(Result.Error(validationError), _type))
            : Task.FromResult((TResponse)Convert.ChangeType(_operationResultError!.Invoke(null, new object[] { validationError }), _type)!);
    }

    internal static class ValidationPipelineBehaviorMeta
    {
        internal static readonly Type _typeOperationResult = typeof(Result);
        internal static readonly MethodInfo _genericErrorMethod = _typeOperationResult!.GetMethod(nameof(Result.Error), 1, new[] { typeof(Exception) })!;
    }
}