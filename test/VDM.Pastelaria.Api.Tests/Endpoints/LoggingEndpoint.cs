using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog.Core;
using Serilog.Events;
using System.Net;
using VDM.Pastelaria.TestUtils;

namespace VDM.Pastelaria.Api.Tests.Endpoints;
public class LoggingEndpoint
{
    private static readonly ILogger<string> _logger;
    private static readonly LoggingLevelSwitch _loggingLevelSwitch;
    private static readonly HttpClient _client;

    static LoggingEndpoint()
    {
        TestApplication app = new();
        _client = app.CreateClient();
        _logger = app.Services.GetRequiredService<ILogger<string>>();
        _loggingLevelSwitch = app.Services.GetRequiredService<LoggingLevelSwitch>();
    }

    [Fact]
    public async Task Dado_UmaEntrada_Quando_LogLevelTest_Deve_RetornarELogarEntrada()
    {
        //Arrange
        var entrada = "Testes teste";

        //Act
        var result = await _client.GetAsync($"api/v1/loglevel/teste?data={entrada}");

        //Assert
        result.StatusCode.Should().Be(HttpStatusCode.OK);
        var response = await result.Content.ReadAsStringAsync();
        response.Should().Be($"\"{entrada}\"");

        _logger.ReceivedLogCall(LogLevel.Trace, $"Message logged: {entrada}");
        _logger.ReceivedLogCall(LogLevel.Debug, $"Message logged: {entrada}");
        _logger.ReceivedLogCall(LogLevel.Information, $"Message logged: {entrada}");
        _logger.ReceivedLogCall(LogLevel.Warning, $"Message logged: {entrada}");
        _logger.ReceivedLogCall(LogLevel.Error, $"Message logged: {entrada}");
        _logger.ReceivedLogCall(LogLevel.Critical, $"Message logged: {entrada}");
    }

    [Theory]
    [InlineData(LogEventLevel.Verbose)]
    [InlineData(LogEventLevel.Debug)]
    [InlineData(LogEventLevel.Information)]
    [InlineData(LogEventLevel.Warning)]
    [InlineData(LogEventLevel.Error)]
    [InlineData(LogEventLevel.Fatal)]
    public async Task Dado_LogLevel_Quando_LogLevelChange_Deve_AlterarLogLevel(LogEventLevel logLevel)
    {

        //Act
        var result = await _client.PutAsync($"api/v1/loglevel/alterar?logLevel={logLevel}", content: default!);

        //Assert
        result.StatusCode.Should().Be(HttpStatusCode.OK);
        var response = await result.Content.ReadAsStringAsync();
        response.Should().Be($"\"Changed to {logLevel}\"");
        _loggingLevelSwitch.MinimumLevel.Should().Be(logLevel);
    }
}
