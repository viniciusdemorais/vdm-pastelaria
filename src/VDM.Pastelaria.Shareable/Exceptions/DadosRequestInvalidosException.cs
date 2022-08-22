namespace VDM.Pastelaria.Shareable.Exceptions;
public class DadosRequestInvalidosException : ApplicationException
{
    public DadosRequestInvalidosException(IDictionary<string, IEnumerable<string>> mensagens)
        : base("Dados inválidos") => Erros = mensagens.Select((KeyValuePair<string, IEnumerable<string>> m) => m.Key + ": " + string.Join(", ", m.Value));

    public IEnumerable<string> Erros { get; }
}