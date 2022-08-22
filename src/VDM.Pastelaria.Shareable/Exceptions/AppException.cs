namespace VDM.Pastelaria.Shareable.Exceptions;
public class AppException : Exception
{
    public AppException(string mensagem)
        : base(mensagem)
    {
    }

    public AppException(int codigo, string mensagem)
        : base(mensagem) => CodigoErro = codigo;

    public int? CodigoErro { get; set; }
}