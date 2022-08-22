namespace VDM.Pastelaria.Shareable.Exceptions;
public class DadosNaoEncontradosException : AppException
{
    public DadosNaoEncontradosException(string mensagem)
        : base(mensagem)
    {
    }
}