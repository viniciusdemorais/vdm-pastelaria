using FluentValidation;
using MediatR;
using OperationResult;
using VDM.Pastelaria.Domain.Pipelines;
using VDM.Pastelaria.Domain.Tests.Pipelines;

namespace VDM.Pastelaria.Domain.Tests;
public class ValidationPipelineBehaviorGenericResultTest
{
    private readonly AbstractValidator<SampleRequestGeneric> _validator;
    private readonly IPipelineBehavior<SampleRequestGeneric, Result<long>> _sut;

    public ValidationPipelineBehaviorGenericResultTest()
    {
        _validator = new SampleRequestValidatorGeneric();
        _sut = new ValidationPipelineBehavior<SampleRequestGeneric, Result<long>>(_validator);
    }

    [Fact]
    public async Task Dado_UmModeloComErro_Quando_Validar_Deve_RetornarException()
    {
        //Arrange
        var request = new SampleRequestGeneric();

        //Act
        var (success, result, exception) = await _sut.Handle(request, CancellationToken.None, default!);

        //Assert
        Assert.False(success);
        Assert.NotNull(exception);
    }

    [Fact]
    public async Task Dado_UmModeloValido_Quando_Validar_Deve_RetornarSucesso()
    {
        //Arrange
        var request = new SampleRequestGeneric { Name = "Not null" };

        static Task<Result<long>> Next() => Task.FromResult(Result.Success(1L));

        //Act
        var (success, result, exception) = await _sut.Handle(request, CancellationToken.None, Next);

        //Assert
        Assert.True(success);
        Assert.Null(exception);
        Assert.NotEqual(0, result);
    }
}
