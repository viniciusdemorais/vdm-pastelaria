using Serilog;
using Serilog.Core;
using Serilog.Events;
using System.Diagnostics.CodeAnalysis;

namespace VDM.Pastelaria.Api.Extensions;
[ExcludeFromCodeCoverage]
public static class LoggingExtension
{
    public static void UseVdmSerilog(this ConfigureHostBuilder host, LoggingLevelSwitch _logLevelSwitcher) =>
        host.UseSerilog((host, cfg) =>
        {
            var minimumLevel = Enum.TryParse<LogEventLevel>(host.Configuration["Serilog:MinimumLevel"], true, out var logEventLevel)
                ? logEventLevel
                : LogEventLevel.Information;
            _logLevelSwitcher.MinimumLevel = minimumLevel;
            cfg.ReadFrom.Configuration(host.Configuration);
            cfg.MinimumLevel.ControlledBy(_logLevelSwitcher)
            .MinimumLevel.Override("Microsoft", LogEventLevel.Error)
            .MinimumLevel.Override("HealthChecks", LogEventLevel.Error)
            .MinimumLevel.Override("System.Net.Http.HttpClient.health-checks", LogEventLevel.Error);
        });
}