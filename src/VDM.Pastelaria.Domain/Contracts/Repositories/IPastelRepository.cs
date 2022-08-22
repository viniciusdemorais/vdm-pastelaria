using OperationResult;
using VDM.Pastelaria.Domain.Entities;

namespace VDM.Pastelaria.Domain.Contracts.Repositories;
public interface IPastelRepository
{
    Task<PastelEntity?> BuscarPorIdAsync(Guid? id);

    Task<Result> CriarAsync(PastelEntity pastel);

    Task<PastelEntity[]> ListarAsync();
}