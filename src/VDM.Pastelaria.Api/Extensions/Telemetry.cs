using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace VDM.Pastelaria.Api.Extensions;
[ExcludeFromCodeCoverage]
public static class Telemetry
{
    public static readonly string ServiceName = "VDM.Pastelaria.Api";
    public static readonly string ServiceVersion = "1.0.0";
    public static readonly ActivitySource MyActivitySource = new(ServiceName);
}