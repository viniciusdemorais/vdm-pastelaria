using Microsoft.EntityFrameworkCore;
using OperationResult;
using System.Diagnostics.CodeAnalysis;
using VDM.Pastelaria.Domain.Contracts.Repositories;
using VDM.Pastelaria.Domain.Entities;
using VDM.Pastelaria.Shareable.Exceptions;

namespace VDM.Pastelaria.Data.Repositories;
[ExcludeFromCodeCoverage]
public class PastelRepository : IPastelRepository
{
    private readonly PastelariaContext _context;

    public PastelRepository(PastelariaContext dbContext) => _context = dbContext;

    public async Task<Result> CriarAsync(PastelEntity pastel)
    {
        if (_context.Pasteis.Any(p => p.Sabor == pastel.Sabor))
            return new AppException("Sabor já existe");

        _context.Pasteis.Add(pastel);
        await _context.SaveChangesAsync();
        return Result.Success();
    }

    public async Task<PastelEntity[]> ListarAsync() => await _context.Pasteis.ToArrayAsync();

    public async Task<PastelEntity?> BuscarPorIdAsync(Guid? id) => await _context.Pasteis.FirstOrDefaultAsync(p => p.Id == id);
}