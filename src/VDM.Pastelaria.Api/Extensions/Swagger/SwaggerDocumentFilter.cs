using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Diagnostics.CodeAnalysis;

namespace VDM.Pastelaria.Api.Extensions.Swagger;

[ExcludeFromCodeCoverage]
public class SwaggerDocumentFilter : IDocumentFilter
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public SwaggerDocumentFilter(IHttpContextAccessor httpContextAccessor)
        => _httpContextAccessor = httpContextAccessor;

    public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
    {
        var request = _httpContextAccessor.HttpContext!.Request;
#if DEBUG
        if (request.Host.Host == "localhost")
            swaggerDoc.Servers.Add(new OpenApiServer { Url = $"{request.Scheme}://{request.Host.Value}" });
#endif
        swaggerDoc.Servers.Add(new OpenApiServer { Url = $"https://{request.Host.Value}" });
    }
}