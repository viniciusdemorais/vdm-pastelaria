using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;
using VDM.Pastelaria.Domain.Contracts.Repositories;
using VDM.Pastelaria.Domain.Entities;

namespace VDM.Pastelaria.Data.Repositories;
[ExcludeFromCodeCoverage]
public class PedidoRepository : IPedidoRepository
{
    private readonly PastelariaContext _context;

    public PedidoRepository(PastelariaContext dbContext) => _context = dbContext;

    public async Task CriarAsync(PedidoEntity pedido)
    {
        _context.Pedidos.Add(pedido);
        await _context.SaveChangesAsync();
    }

    public async Task<PedidoEntity[]> ListarAsync() => await _context.Pedidos.ToArrayAsync();

    public async Task<PedidoEntity?> BuscarPorIdAsync(Guid? id) => await _context.Pedidos.FirstOrDefaultAsync(p => p.Id == id);
}