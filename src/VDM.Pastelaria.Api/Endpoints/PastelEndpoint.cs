using MediatR;
using VDM.Pastelaria.Api.Extensions;
using VDM.Pastelaria.Shareable.Models.Requests;

namespace VDM.Pastelaria.Api.Endpoints;

public static class PastelEndpoint
{
    public static void MapPastelEndpoints(this WebApplication app)
    {
        app.MapGet("api/v1/pastel/sabores", async (IMediator mediator)
                => await mediator.SendCommand(new ListarSaboresPasteisRequest()))
                   .WithName("ListarSabores")
                   .WithTags("Pastel")
                   .Produces(200, typeof(string[]));

        app.MapPost("api/v1/pastel", async (IMediator mediator, CriarPastelRequest request)
                => await mediator.SendCommand(request))
                   .WithName("CriarPastel")
                   .WithTags("Pastel")
                   .Produces(200);
    }
}