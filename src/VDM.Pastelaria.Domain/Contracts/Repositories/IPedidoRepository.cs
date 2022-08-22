using VDM.Pastelaria.Domain.Entities;

namespace VDM.Pastelaria.Domain.Contracts.Repositories;
public interface IPedidoRepository
{
    Task<PedidoEntity?> BuscarPorIdAsync(Guid? id);

    Task<PedidoEntity[]> ListarAsync();

    Task CriarAsync(PedidoEntity pedido);
}