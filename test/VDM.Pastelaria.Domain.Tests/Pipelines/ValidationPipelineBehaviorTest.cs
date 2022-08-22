using FluentAssertions;
using FluentValidation;
using MediatR;
using OperationResult;
using System.Globalization;
using VDM.Pastelaria.Domain.Pipelines;
using VDM.Pastelaria.Shareable.Exceptions;

namespace VDM.Pastelaria.Domain.Tests.Pipelines;

public class ValidationPipelineBehaviorTest
{
    private readonly AbstractValidator<SampleRequest> _validator;
    private readonly IPipelineBehavior<SampleRequest, Result> _sut;

    public ValidationPipelineBehaviorTest()
    {
        _validator = new SampleRequestValidator();
        _sut = new ValidationPipelineBehavior<SampleRequest, Result>(_validator);
    }

    [Fact]
    public async Task Dado_UmModeloComErro_Quando_Validar_Deve_RetornarDadosRequestInvalidosException()
    {
        //Arrange
        CultureInfo.DefaultThreadCurrentCulture = CultureInfo.CurrentUICulture = CultureInfo.CurrentCulture = CultureInfo.GetCultureInfo("pt-br");

        var request = new SampleRequest();

        //Act
        var (success, exception) = await _sut.Handle(request, CancellationToken.None, default!);

        //Assert
        Assert.False(success);
        Assert.NotNull(exception);
        exception!.Message.Should().Be("Dados inválidos");
        exception.Should().BeOfType<DadosRequestInvalidosException>()
            .Which.Erros.Should().BeEquivalentTo(new[] { "Name: 'Name' não pode ser nulo., 'Name' deve ser informado." });
    }

    [Fact]
    public async Task Dado_UmModeloValido_Quando_Validar_Deve_RetornarSucesso()
    {
        //Arrange
        var request = new SampleRequest { Name = "Not null" };

        static Task<Result> Next() => Task.FromResult(Result.Success());

        //Act
        var (success, exception) = await _sut.Handle(request, CancellationToken.None, Next);

        //Assert
        Assert.True(success);
        Assert.Null(exception);
    }
}
