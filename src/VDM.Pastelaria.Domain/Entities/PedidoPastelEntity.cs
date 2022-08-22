using System.Diagnostics.CodeAnalysis;

namespace VDM.Pastelaria.Domain.Entities;
[ExcludeFromCodeCoverage]
public class PedidoPastelEntity
{
    public Guid IdPedido { get; set; }

    public PedidoEntity Pedido { get; set; } = default!;

    public Guid IdPastel { get; set; }

    public PastelEntity Pastel { get; set; } = default!;
}