using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using OperationResult;
using System.Net;
using System.Text.Json;
using VDM.Pastelaria.Api.Extensions;
using VDM.Pastelaria.Shareable.Exceptions;
using VDM.Pastelaria.Shareable.Models.Responses;

namespace VDM.Pastelaria.Api.Tests.Extensions;
public class EndpointExtensionTests
{
    private static readonly IMediator _mediator = Substitute.For<IMediator>();
    private static readonly IServiceProvider _serviceProvider = new ServiceCollection().AddLogging().BuildServiceProvider();

    internal record MediatorRequest() : IRequest<Result>;
    internal record MediatorComValorRequest(string Nome) : IRequest<Result<string>>;

    [Fact]
    public async Task Dado_MediatorRequest_Quando_Send_Deve_RetornarDadosRequestInvalidosException()
    {
        //Arrange
        var context = new DefaultHttpContext()
        {
            RequestServices = _serviceProvider,
            Response = { Body = new MemoryStream() }
        };
        MediatorRequest request = new();
        DadosRequestInvalidosException erro = new(new Dictionary<string, IEnumerable<string>> { ["Teste"] = (new[] { "Mensagem de erro" }) });
        _mediator.Send(request).Returns(Result.Error(erro));

        //Act
        await (await _mediator.SendCommand(request)).ExecuteAsync(context);

        // Assert
        await _mediator.Received().Send(request);
        context.Response.StatusCode.Should().Be((int)HttpStatusCode.BadRequest);
        context.Response.Body.Position = 0;
        var resposta = await JsonSerializer.DeserializeAsync<IEnumerable<string>>(context.Response.Body, new JsonSerializerOptions(JsonSerializerDefaults.Web));
        resposta.Should().Contain("Teste: Mensagem de erro");
    }

    [Fact]
    public async Task Dado_MediatorRequest_Quando_Send_Deve_DadosNaoEncontradosException()
    {
        //Arrange
        var context = new DefaultHttpContext()
        {
            RequestServices = _serviceProvider,
            Response = { Body = new MemoryStream() }
        };
        MediatorRequest request = new();
        DadosNaoEncontradosException erro = new("Mensagem de erro");
        _mediator.Send(request).Returns(Result.Error(erro));

        //Act
        await (await _mediator.SendCommand(request)).ExecuteAsync(context);

        // Assert
        await _mediator.Received().Send(request);
        context.Response.StatusCode.Should().Be((int)HttpStatusCode.NotFound);
        context.Response.Body.Position = 0;
        var resposta = await JsonSerializer.DeserializeAsync<ErroResponse>(context.Response.Body, new JsonSerializerOptions(JsonSerializerDefaults.Web));
        resposta!.Mensagem.Should().Contain("Mensagem de erro");
    }

    [Fact]
    public async Task Dado_MediatorRequest_Quando_Send_Deve_AppException()
    {
        //Arrange
        var context = new DefaultHttpContext()
        {
            RequestServices = _serviceProvider,
            Response = { Body = new MemoryStream() }
        };
        MediatorRequest request = new();
        AppException erro = new(99, "Mensagem de erro");
        _mediator.Send(request).Returns(Result.Error(erro));

        //Act
        await (await _mediator.SendCommand(request)).ExecuteAsync(context);

        // Assert
        await _mediator.Received().Send(request);
        context.Response.StatusCode.Should().Be((int)HttpStatusCode.UnprocessableEntity);
        context.Response.Body.Position = 0;
        var resposta = await JsonSerializer.DeserializeAsync<ErroResponse>(context.Response.Body, new JsonSerializerOptions(JsonSerializerDefaults.Web));
        resposta!.Mensagem.Should().Contain("Mensagem de erro");
    }

    [Fact]
    public async Task Dado_MediatorRequest_Quando_Send_Deve_Exception()
    {
        //Arrange
        var context = new DefaultHttpContext()
        {
            RequestServices = _serviceProvider,
            Response = { Body = new MemoryStream() }
        };
        MediatorRequest request = new();
        Exception erro = new("Mensagem de erro");
        _mediator.Send(request).Returns(Result.Error(erro));

        //Act
        await (await _mediator.SendCommand(request)).ExecuteAsync(context);

        // Assert
        await _mediator.Received().Send(request);
        context.Response.StatusCode.Should().Be((int)HttpStatusCode.InternalServerError);
    }

    [Fact]
    public async Task Dado_MediatorComValorRequest_Quando_Send_Deve_Exception()
    {
        //Arrange
        var context = new DefaultHttpContext()
        {
            RequestServices = _serviceProvider,
            Response = { Body = new MemoryStream() }
        };
        MediatorComValorRequest request = new("Teste");
        Exception erro = new("Mensagem de erro");
        _mediator.Send(request).Returns(Result.Error<string>(erro));

        //Act
        await (await _mediator.SendCommand(request)).ExecuteAsync(context);

        // Assert
        await _mediator.Received().Send(request);
        context.Response.StatusCode.Should().Be((int)HttpStatusCode.InternalServerError);
    }
}
