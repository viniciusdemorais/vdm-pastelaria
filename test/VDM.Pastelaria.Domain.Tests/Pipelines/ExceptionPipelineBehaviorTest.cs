using Microsoft.Extensions.Logging;
using NSubstitute;
using OperationResult;
using VDM.Pastelaria.Domain.Pipelines;
using VDM.Pastelaria.TestUtils;

namespace VDM.Pastelaria.Domain.Tests.Pipelines;
public class ExceptionPipelineBehaviorTest
{
    private readonly ILogger<ExceptionPipelineBehavior<SampleRequest, Result>> _logger;
    private readonly ExceptionPipelineBehavior<SampleRequest, Result> _sut;

    public ExceptionPipelineBehaviorTest()
    {
        _logger = Substitute.For<ILogger<ExceptionPipelineBehavior<SampleRequest, Result>>>();
        _sut = new ExceptionPipelineBehavior<SampleRequest, Result>(_logger);
    }

    [Theory]
    [InlineData(LogLevel.Debug)]
    [InlineData(LogLevel.Information)]
    public async Task Dado_UmaRequestGenericaQueEstouroException_Quando_Validar_Deve_RetornarSucessoFalse(LogLevel logLevel)
    {
        //Arrange
        var exception = new Exception("Fail");
        Task<Result> Next() => throw exception;
        _logger.IsEnabled(logLevel).Returns(true);

        //Act
        var result = await _sut.Handle(new(), CancellationToken.None, Next);

        //Assert
        var msg = logLevel == LogLevel.Information
            ? $"Handling request of type {typeof(SampleRequest).FullName}"
            : $"Handling request of type {typeof(SampleRequest).FullName} with data {typeof(SampleRequest).FullName}";
        Assert.False(result.IsSuccess);
        _logger.ReceivedLogCall(LogLevel.Information, msg);
        _logger.ReceivedLogCall(LogLevel.Error, "Error on request handling", exception);
    }

    [Fact]
    public async Task Dado_UmaRequestQueEstouroException_Quando_Validar_Deve_RetornarSucessoTrue()
    {
        //Arrange
        static Task<Result> Next() => Task.FromResult(Result.Success());

        //Act
        var result = await _sut.Handle(new(), CancellationToken.None, Next);

        //Assert
        var msg = $"Handling request of type {typeof(SampleRequest).FullName}";
        _logger.ReceivedLogCall(LogLevel.Information, msg);
        _logger.ReceivedLogCall(LogLevel.Information, null, customMessageValidator: loggedStr => loggedStr.StartsWith($"Handled request of type {typeof(SampleRequest).FullName} in ", StringComparison.OrdinalIgnoreCase));

        Assert.True(result.IsSuccess);
    }
}