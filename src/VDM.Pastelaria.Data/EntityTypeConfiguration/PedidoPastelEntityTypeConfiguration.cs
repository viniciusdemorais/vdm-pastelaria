using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Diagnostics.CodeAnalysis;
using VDM.Pastelaria.Domain.Entities;

namespace VDM.Pastelaria.Data.EntityTypeConfiguration;
[ExcludeFromCodeCoverage]
public class PedidoPastelEntityTypeConfiguration : IEntityTypeConfiguration<PedidoPastelEntity>
{
    public void Configure(EntityTypeBuilder<PedidoPastelEntity> builder)
    {
        builder.ToTable("PedidoPastel");

        builder.Property(b => b.IdPedido)
            .HasColumnName("IdPedido")
            .IsRequired();

        builder.Property(b => b.IdPastel)
            .HasColumnName("IdPastel")
            .IsRequired();

        builder.HasKey(b => new { b.IdPedido, b.IdPastel });

        builder.HasOne(b => b.Pedido)
            .WithMany(s => s.PedidosPasteis)
            .HasForeignKey(sc => sc.IdPedido);

        builder.HasOne(b => b.Pastel)
            .WithMany(s => s.PedidosPasteis)
            .HasForeignKey(sc => sc.IdPastel);
    }
}