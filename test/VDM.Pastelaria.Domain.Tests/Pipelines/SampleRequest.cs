using FluentValidation;
using MediatR;
using OperationResult;
using VDM.Pastelaria.Shareable.Models.Validation;

namespace VDM.Pastelaria.Domain.Tests.Pipelines;

public class SampleRequest : IRequest<Result>, IValidatable
{
    public string? Name { get; set; }
}

public class SampleRequestValidator : AbstractValidator<SampleRequest>
{
    public SampleRequestValidator()
    {
        RuleFor(x => x.Name).NotNull();
        RuleFor(x => x.Name).NotEmpty();
    }
}

public class SampleRequestGeneric : IRequest<Result<long>>, IValidatable
{
    public string? Name { get; set; }
}

public class SampleRequestValidatorGeneric : AbstractValidator<SampleRequestGeneric>
{
    public SampleRequestValidatorGeneric()
        => RuleFor(x => x.Name).NotNull();
}