using Microsoft.Extensions.Logging;
using NSubstitute;
using OperationResult;
using VDM.Pastelaria.Domain.Pipelines;
using VDM.Pastelaria.TestUtils;

namespace VDM.Pastelaria.Domain.Tests.Pipelines;
public class ExceptionPipelineBehaviorGenericTest
{
    private readonly ILogger<ExceptionPipelineBehavior<SampleRequestGeneric, Result<long>>> _logger;
    private readonly ExceptionPipelineBehavior<SampleRequestGeneric, Result<long>> _sut;

    public ExceptionPipelineBehaviorGenericTest()
    {
        _logger = Substitute.For<ILogger<ExceptionPipelineBehavior<SampleRequestGeneric, Result<long>>>>();
        _sut = new ExceptionPipelineBehavior<SampleRequestGeneric, Result<long>>(_logger);
    }

    [Theory]
    [InlineData(LogLevel.Debug)]
    [InlineData(LogLevel.Information)]
    public async Task Dado_UmaRequestGenericaQueEstouroException_Quando_Validar_Deve_RetornarSucessoFalse(LogLevel logLevel)
    {
        //Arrange
        var exception = new Exception("Fail");
        Task<Result<long>> Next() => throw exception;
        _logger.IsEnabled(logLevel).Returns(true);

        //Act
        var result = await _sut.Handle(new(), CancellationToken.None, Next);

        //Assert
        var msg = logLevel == LogLevel.Information
            ? $"Handling request of type {typeof(SampleRequestGeneric).FullName}"
            : $"Handling request of type {typeof(SampleRequestGeneric).FullName} with data {typeof(SampleRequestGeneric).FullName}";
        Assert.False(result.IsSuccess);
        _logger.ReceivedLogCall(LogLevel.Information, msg);
        _logger.ReceivedLogCall(LogLevel.Error, "Error on request handling", exception);
    }

    [Fact]
    public async Task Dado_UmaRequestQueEstouroException_Quando_Validar_Deve_RetornarSucessoTrue()
    {
        //Arrange
        static Task<Result<long>> Next() => Task.FromResult(Result.Success(1L));

        //Act
        var result = await _sut.Handle(new(), CancellationToken.None, Next);

        //Assert
        var msg = $"Handling request of type {typeof(SampleRequestGeneric).FullName}";
        _logger.ReceivedLogCall(LogLevel.Information, msg);
        _logger.ReceivedLogCall(LogLevel.Information, null, customMessageValidator: loggedStr => loggedStr.StartsWith($"Handled request of type {typeof(SampleRequestGeneric).FullName} in ", StringComparison.OrdinalIgnoreCase));

        Assert.True(result.IsSuccess);
        Assert.Equal(1L, result.Value);
    }
}