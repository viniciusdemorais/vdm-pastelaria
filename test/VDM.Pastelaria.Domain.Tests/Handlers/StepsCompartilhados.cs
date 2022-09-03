using FluentAssertions;
using TechTalk.SpecFlow;
using VDM.Pastelaria.Shareable.Exceptions;

namespace VDM.Pastelaria.Domain.Tests.Handlers;

[Binding]
public class StepsCompartilhados
{
    private readonly ScenarioContext _context;

    public StepsCompartilhados(ScenarioContext context) => _context = context;

    [Then(@"deverá retornar AppException com mensagem '(.*)'")]
    public void EntaoDeveraRetornarAppExceptionComMensagem(string mensagem)
    {
        var (sucesso, erro) = _context.Get<(bool, Exception)>("response");
        sucesso.Should().BeFalse();
        erro.Should().BeOfType<AppException>()
            .Which.Message.Should().Be(mensagem);
    }

    [Then(@"deverá retornar DadosNaoEncontradosException com mensagem '(.*)'")]
    public void EntaoDeveraRetornarDadosNaoEncontradosExceptionComMensagem(string mensagem)
    {
        var (sucesso, erro) = _context.Get<(bool, Exception)>("response");
        sucesso.Should().BeFalse();
        erro.Should().BeOfType<DadosNaoEncontradosException>()
            .Which.Message.Should().Be(mensagem);
    }
}
