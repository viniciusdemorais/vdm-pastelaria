using Microsoft.AspNetCore.Http.Json;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.OpenApi.Models;
using System.Text.Json.Serialization;
using VDM.Pastelaria.Api.Extensions.Swagger;
using VDM.Pastelaria.IoC;

namespace VDM.Pastelaria.Api.Extensions;

public static class ConfigureServicesExtension
{
    public static void ConfigureServices(this IServiceCollection services, ConfigurationManager configuration)
    {
        services.Configure<JsonOptions>(options => options.SerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull);
        services.AddEndpointsApiExplorer();
        services.AddHttpContextAccessor();
        services.AddHealthChecks().AddCheck("self", () => HealthCheckResult.Healthy(), new[] { "api" });
        services.AddHealthChecksUI(x => x.AddHealthCheckEndpoint("SELF", $"{configuration["urls"].Split(';')[0].Replace("*", "localhost", StringComparison.OrdinalIgnoreCase)}/hc"))
                .AddInMemoryStorage();
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "VDM.Pastelaria.Api",
                Version = "v1",
            });
            c.EnableAnnotations();
            c.OperationFilter<SecurityRequirementsOperationFilter>();
            c.DocumentFilter<SwaggerDocumentFilter>();
            c.AddSecurityDefinition(SecurityRequirementsOperationFilter.OAuthScheme.Scheme, SecurityRequirementsOperationFilter.OAuthScheme);
        });
        services.ConfigureAppDependencies(configuration);
    }
}