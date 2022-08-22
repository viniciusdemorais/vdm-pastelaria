using Microsoft.AspNetCore.Authorization;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Diagnostics.CodeAnalysis;

namespace VDM.Pastelaria.Api.Extensions.Swagger;

[ExcludeFromCodeCoverage]
public class SecurityRequirementsOperationFilter : IOperationFilter
{
    public static readonly OpenApiSecurityScheme OAuthScheme = new()
    {
        Reference = new OpenApiReference
        {
            Type = ReferenceType.SecurityScheme,
            Id = TOKEN_TYPE
        },
        Description = TOKEN_DESCRIPTION,
        Name = AUTHORIZATION,
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = TOKEN_TYPE,
        BearerFormat = "JWT"
    };

    internal const string TOKEN_TYPE = "bearer";
    private const string AUTHORIZATION = "Authorization";
    private const string TOKEN_DESCRIPTION = "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 12345abcdef\"";

    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        if (context.ApiDescription.ActionDescriptor.EndpointMetadata.OfType<AuthorizeAttribute>().Any())
        {
            operation.Responses.Add("401", new OpenApiResponse { Description = "Unauthorized" });
            operation.Responses.Add("403", new OpenApiResponse { Description = "Forbidden" });
            operation.Security = new[] { new OpenApiSecurityRequirement { [OAuthScheme] = Array.Empty<string>() } };
        }
    }
}