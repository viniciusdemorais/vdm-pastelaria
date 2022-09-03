using MediatR;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using OperationResult;
using System.Text;
using System.Text.Json;
using VDM.Pastelaria.Shareable.Models.Requests;

namespace VDM.Pastelaria.Api.Tests.Endpoints;
public class PastelEndpoint
{
    private static readonly IMediator _mediator;
    private static readonly HttpClient _client;

    static PastelEndpoint()
    {
        TestApplication app = new();
        _client = app.CreateClient();
        _mediator = app.Services.GetRequiredService<IMediator>();
    }

    [Fact]
    public async Task Dado_UmaRequest_Quando_ListarPasteis_Deve_ChamarMediator()
    {
        //Arrange
        ListarSaboresPasteisRequest request = new();
        _mediator.Send(request).Returns(Result.Success<string[]>(default!));

        //Act
        await _client.GetAsync("api/v1/pastel/sabores");

        //Assert
        await _mediator.Received().Send(request);
    }

    [Fact]
    public async Task Dado_UmaRequest_Quando_CriarPasteis_Deve_ChamarMediator()
    {
        //Arrange
        CriarPastelRequest request = new("Queijo");
        _mediator.Send(request).Returns(Result.Success());

        //Act
        await _client.PostAsync("api/v1/pastel",
            new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json"));

        //Assert
        await _mediator.Received().Send(request);
    }
}
