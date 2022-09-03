#nullable disable
using FluentAssertions;
using Microsoft.Extensions.Logging;
using NSubstitute;
using System.Diagnostics.CodeAnalysis;

namespace VDM.Pastelaria.TestUtils;
public abstract class MockLogger<T> : ILogger<T>
{
    void ILogger.Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        => Log(logLevel, formatter(state, exception), exception);

    public abstract void Log(LogLevel logLevel, object state, Exception exception = null);

    [ExcludeFromCodeCoverage]
    public virtual bool IsEnabled(LogLevel logLevel) => true;

    public abstract IDisposable BeginScope<TState>(TState state);
}

public static class MockLogger
{
    public static void ReceivedLogCall<T>(this T mock, LogLevel logLevel, string str, Exception error = null, Func<string, bool> customMessageValidator = null)
        where T : class
    {
        var logged = false;
        foreach (var call in mock.ReceivedCalls())
        {
            var logCallArgs = call.GetArguments();
            if (logCallArgs.Length == 5 &&
                logLevel.Equals(logCallArgs[0]) &&
                (
                    customMessageValidator == null && str!.Equals(logCallArgs[2].ToString(), StringComparison.OrdinalIgnoreCase) ||
                    customMessageValidator != null && customMessageValidator(logCallArgs[2].ToString()!)
                ) &&
                logCallArgs[3] == error)
            {
                logged = true;
                break;
            }

        }
        logged.Should().BeTrue($"Should be received a log call with LogLevel {logLevel} and the following compiled message {str}");
    }
}