using Prometheus.DotNetRuntime;
using Serilog.Core;
using VDM.Pastelaria.Api.Endpoints;
using VDM.Pastelaria.Api.Extensions;

DotNetRuntimeStatsBuilder.Default();
DotNetRuntimeStatsBuilder.Customize()
.WithContentionStats(CaptureLevel.Informational)
.WithGcStats(CaptureLevel.Verbose)
.WithThreadPoolStats(CaptureLevel.Informational)
.WithExceptionStats(CaptureLevel.Errors)
.WithJitStats()
.RecycleCollectorsEvery(TimeSpan.FromMinutes(20))
.StartCollecting();

LoggingLevelSwitch _logLevelSwitcher = new();

var builder = WebApplication.CreateBuilder(args);
builder.Host.UseVdmSerilog(_logLevelSwitcher);
builder.Services.AddSingleton(_logLevelSwitcher);
builder.Services.ConfigureServices(builder.Configuration);

var app = builder.Build();
app.Configure();
app.MapLoggingEndpoints();
app.MapPastelEndpoints();
app.Run();