using System.Diagnostics.CodeAnalysis;

namespace VDM.Pastelaria.Api.Extensions;
[ExcludeFromCodeCoverage]
public static class CorrelationIdExtension
{
    public static IApplicationBuilder UseCorrelationId(this WebApplication app) =>
        app.Use(async (httpContext, next) =>
        {
            if (!httpContext.Request.Headers.ContainsKey("X-Correlation-Id"))
            {
                httpContext.Request.Headers.Add("X-Correlation-Id", Guid.NewGuid().ToString());
            }

            await next();
        });
}