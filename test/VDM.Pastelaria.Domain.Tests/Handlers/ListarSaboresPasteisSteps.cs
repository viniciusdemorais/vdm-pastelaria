using FluentAssertions;
using Microsoft.Extensions.Logging;
using NSubstitute;
using OperationResult;
using TechTalk.SpecFlow;
using VDM.Pastelaria.Domain.Contracts.Repositories;
using VDM.Pastelaria.Domain.Entities;
using VDM.Pastelaria.Domain.Handlers;
using VDM.Pastelaria.Shareable.Models.Requests;
using VDM.Pastelaria.TestUtils;

namespace VDM.Pastelaria.Domain.Tests.Handlers;

[Binding]
public class ListarSaboresPasteisSteps
{
    private readonly ILogger<ListarSaboresPasteisHandler> _logger = Substitute.For<MockLogger<ListarSaboresPasteisHandler>>();
    private readonly IPastelRepository _pastelRepository = Substitute.For<IPastelRepository>();
    private readonly ScenarioContext _context;
    private readonly ListarSaboresPasteisHandler _sut;

    private ListarSaboresPasteisRequest _request = default!;
    private PastelEntity[] _listaPasteis = default!;
    private Result<string[]> _response;

    public ListarSaboresPasteisSteps(ScenarioContext context)
    {
        _context = context;
        _sut = new(_logger, _pastelRepository);
    }

    [Given(@"solicitacao para listar sobres")]
    public void GivenSolicitacaoParaListarSobres() => _request = new();

    [Given(@"existem sabores criados")]
    public void GivenExistemSaboresCriados()
    {
        _listaPasteis = new PastelEntity[] { new("Carne"), new("Queijo"), new("Banana") };
        _pastelRepository.ListarAsync().Returns(_listaPasteis);
    }

    [Given(@"não existem sabores criados")]
    public void GivenNaoExistemSaboresCriados()
    {
        _listaPasteis = Array.Empty<PastelEntity>();
        _pastelRepository.ListarAsync().Returns(_listaPasteis);
    }

    [When(@"listar sabores dos pasteis")]
    public async void WhenListarSaboresDosPasteis()
    {
        _response = await _sut.Handle(_request, CancellationToken.None);
        _context.Set((_response.IsSuccess, _response.Exception), "response");
    }

    [Then(@"deverá retornar listagem de pasteis")]
    public void ThenDeveraRetornarListagemDePasteis()
    {
        var (sucesso, resposta) = _response;
        sucesso.Should().BeTrue();
        resposta.Should().BeEquivalentTo(_listaPasteis.Select(lp => lp.Sabor).ToArray());
    }

    [Then(@"listagem de sabores deve ter sido chamada")]
    public async void ThenListagemDeSaboresDeveTerSidoChamada() => await _pastelRepository.Received().ListarAsync();

    [Then(@"deve ter sido logado os pasteis encontrados")]
    public void ThenDeveTerSidoLogadoOsPasteisEncontrados()
        => _logger.Received().Log(LogLevel.Debug, "Pasteis encontrados {@pasteis}", (object)_listaPasteis.Select(lp => lp.Sabor).ToArray());
}
