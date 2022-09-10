using HealthChecks.UI.Client;
using Prometheus;
using Serilog;
using Serilog.Events;
using Swashbuckle.AspNetCore.SwaggerUI;
using System.Diagnostics.CodeAnalysis;
using VDM.Pastelaria.Data;

namespace VDM.Pastelaria.Api.Extensions;

[ExcludeFromCodeCoverage]
public static class AppConfigureExtension
{
    public static void Configure(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        scope.ServiceProvider.GetRequiredService<PastelariaContext>().Database.EnsureCreated();

        if (app.Environment.IsDevelopment())
            app.UseDeveloperExceptionPage();

        app.UseSwagger();
        app.UseSwaggerUI(c =>
        {
            c.DocumentTitle = "VDM.Pastelaria.Api Swagger Docs";
            c.DisplayRequestDuration();
            c.DocExpansion(DocExpansion.None);
            c.EnableDeepLinking();
            c.ShowExtensions();
            c.ShowCommonExtensions();
            c.EnableValidator();
        });
        app.UseHttpMetrics();
        app.UseMetricServer();
        app.UseCorrelationId();
        app.UseHeaderPropagation();
        app.UseSerilogRequestLogging(opt => opt.GetLevel = (ctx, _, _) => ctx.Request.Path is { Value: "/hc" or "/liveness" } ? LogEventLevel.Verbose : LogEventLevel.Information);
        app.MapHealthChecks("/hc", new() { Predicate = _ => true, ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse });
        app.MapHealthChecks("/liveness", new() { Predicate = r => r.Name.Contains("self", StringComparison.OrdinalIgnoreCase) });
        app.MapHealthChecksUI(x => x.AsideMenuOpened = false);
    }
}