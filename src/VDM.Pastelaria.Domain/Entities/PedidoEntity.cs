using System.Diagnostics.CodeAnalysis;

namespace VDM.Pastelaria.Domain.Entities;
[ExcludeFromCodeCoverage]
public class PedidoEntity
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public DateTimeOffset DataCriacao { get; set; } = DateTimeOffset.Now;

    public TimeOnly TempoEspera { get; set; } = new TimeOnly(0, 15);

    public DateTimeOffset? DataConclusao { get; set; }

    public bool Concluido { get; set; }

    public IEnumerable<PedidoPastelEntity> PedidosPasteis { get; set; } = Enumerable.Empty<PedidoPastelEntity>();

    public void ConcluirPedido()
    {
        Concluido = true;
        DataConclusao = DateTimeOffset.Now;
    }
}