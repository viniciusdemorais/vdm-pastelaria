using VDM.Pastelaria.Shareable.Exceptions;

namespace VDM.Pastelaria.Shareable.Models.Responses;
public record ErroResponse
{
    public ErroResponse(AppException appException)
    {
        CodigoErro = appException.CodigoErro;
        Mensagem = appException.Message;
    }

    public int? CodigoErro { get; init; }

    public string Mensagem { get; init; }
}