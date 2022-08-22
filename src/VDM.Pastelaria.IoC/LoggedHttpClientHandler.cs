using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace VDM.Pastelaria.IoC;
[ExcludeFromCodeCoverage]
public class LoggedHttpClientHandler : HttpClientHandler
{
    private readonly ILogger<LoggedHttpClientHandler> _logger;

    public LoggedHttpClientHandler(ILogger<LoggedHttpClientHandler> logger) => _logger = logger;

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        if (_logger.IsEnabled(LogLevel.Debug))
        {
            if (request.Method != HttpMethod.Get)
            {
                _logger.LogDebug("Requisitando a api {url} com os dados: {dados}", request.RequestUri!.ToString(), await request.Content!.ReadAsStringAsync(cancellationToken));
            }
            else
            {
                _logger.LogDebug("Requisitando a api {url}", request.RequestUri!.ToString());
            }
        }

        var sp = Stopwatch.StartNew();
        var response = await base.SendAsync(request, cancellationToken);
        sp.Stop();

        if (response.IsSuccessStatusCode)
        {
            _logger.LogInformation("Finalizada a requisição a api {url} em {elapsed} com status {status} com retorno: {dados}", request.RequestUri!.ToString(), sp.Elapsed, response.StatusCode, await response.Content!.ReadAsStringAsync(cancellationToken));
        }
        else
        {
            _logger.LogError("Finalizada a requisição a api {url} em {elapsed} com status {status} com retorno: {dados}", request.RequestUri!.ToString(), sp.Elapsed, response.StatusCode, await response.Content!.ReadAsStringAsync(cancellationToken));
        }

        return response;
    }
}