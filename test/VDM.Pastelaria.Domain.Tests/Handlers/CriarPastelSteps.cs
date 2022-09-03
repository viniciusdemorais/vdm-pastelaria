using FluentAssertions;
using NSubstitute;
using OperationResult;
using TechTalk.SpecFlow;
using VDM.Pastelaria.Domain.Contracts.Repositories;
using VDM.Pastelaria.Domain.Entities;
using VDM.Pastelaria.Domain.Handlers;
using VDM.Pastelaria.Shareable.Exceptions;
using VDM.Pastelaria.Shareable.Models.Requests;

namespace VDM.Pastelaria.Domain.Tests.Handlers;

[Binding]
public class CriarPastelSteps
{
    private readonly IPastelRepository _pastelRepository = Substitute.For<IPastelRepository>();
    private readonly ScenarioContext _context;
    private readonly CriarPastelHandler _sut;

    private CriarPastelRequest _request = default!;
    private Result _response;

    public CriarPastelSteps(ScenarioContext context)
    {
        _context = context;
        _sut = new(_pastelRepository);
    }

    [Given(@"um novo sabor")]
    public void GivenUmNovoSabor() => _request = new("Queijo");

    [Given(@"sabor não existe")]
    public void GivenSaborNaoExiste() => _pastelRepository.CriarAsync(Arg.Is<PastelEntity>(p => p.Sabor == _request.Sabor)).Returns(Result.Success());

    [Given(@"sabor existe")]
    public void GivenSaborExiste() => _pastelRepository.CriarAsync(Arg.Is<PastelEntity>(p => p.Sabor == _request.Sabor)).Returns(Result.Error(new AppException("Sabor já existe")));

    [When(@"criar pastel")]
    public async void WhenCriarPastel()
    {
        _response = await _sut.Handle(_request, CancellationToken.None);
        _context.Set((_response.IsSuccess, _response.Exception), "response");
    }

    [Then(@"deverá retornar sucesso")]
    public void ThenDeveraRetornarSucesso()
    {
        var (sucesso, _) = _response;
        sucesso.Should().BeTrue();
    }

    [Then(@"criação de pastel deve ter sido chamada")]
    public async void ThenCriacaoDePastelDeveTerSidoChamada() => await _pastelRepository.Received().CriarAsync(Arg.Is<PastelEntity>(p => p.Sabor == _request.Sabor));

}
