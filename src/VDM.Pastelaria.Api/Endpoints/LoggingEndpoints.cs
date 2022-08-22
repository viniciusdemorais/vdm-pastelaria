using Microsoft.AspNetCore.Mvc;
using Serilog.Core;
using Serilog.Events;
using Swashbuckle.AspNetCore.Annotations;

namespace VDM.Pastelaria.Api.Endpoints;
public static class LoggingEndpoint
{
    public static void MapLoggingEndpoints(this WebApplication app)
    {
        app.MapGet("api/v1/loglevel/test", (ILogger<string> logger, string data) =>
        {
            logger.LogTrace("Message logged: {data}", data);
            logger.LogDebug("Message logged: {data}", data);
            logger.LogInformation("Message logged: {data}", data);
            logger.LogWarning("Message logged: {data}", data);
            logger.LogError("Message logged: {data}", data);
            logger.LogCritical("Message logged: {data}", data);
            return Results.Ok(data);
        })
       .WithTags("LogLevel");

        app.MapPut("api/v1/loglevel/change", (
            [FromServices] LoggingLevelSwitch loggingLevelSwitch,
            [FromQuery, SwaggerParameter("Verbose = 0, Debug = 1, Information = 2, Warning = 3, Error = 4, Fatal = 5", Required = true)] LogEventLevel logLevel) =>
        {
            loggingLevelSwitch.MinimumLevel = logLevel;
            return Results.Ok($"Changed to {logLevel}");
        })
       .WithTags("LogLevel");
    }
}