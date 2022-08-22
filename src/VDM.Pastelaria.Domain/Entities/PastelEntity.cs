using System.Diagnostics.CodeAnalysis;

namespace VDM.Pastelaria.Domain.Entities;
[ExcludeFromCodeCoverage]
public class PastelEntity
{
    public PastelEntity(string sabor) => Sabor = sabor;

    public Guid Id { get; set; } = Guid.NewGuid();

    public string Sabor { get; set; }

    public IEnumerable<PedidoPastelEntity> PedidosPasteis { get; set; } = Enumerable.Empty<PedidoPastelEntity>();
}