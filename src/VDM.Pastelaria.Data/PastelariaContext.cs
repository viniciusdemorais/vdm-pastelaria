using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;
using VDM.Pastelaria.Domain.Entities;

namespace VDM.Pastelaria.Data;
[ExcludeFromCodeCoverage]
public class PastelariaContext : DbContext
{
    public PastelariaContext(DbContextOptions<PastelariaContext> options)
        : base(options) => Database.EnsureCreated();

    public DbSet<PastelEntity> Pasteis { get; set; } = default!;

    public DbSet<PedidoEntity> Pedidos { get; set; } = default!;

    public DbSet<PedidoPastelEntity> PedidosPasteis { get; set; } = default!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
        => modelBuilder.ApplyConfigurationsFromAssembly(typeof(PastelariaContext).Assembly);
}