using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Diagnostics.CodeAnalysis;
using VDM.Pastelaria.Domain.Entities;

namespace VDM.Pastelaria.Data.EntityTypeConfiguration;
[ExcludeFromCodeCoverage]
public class PedidoEntityTypeConfiguration : IEntityTypeConfiguration<PedidoEntity>
{
    public void Configure(EntityTypeBuilder<PedidoEntity> builder)
    {
        builder.ToTable("Pedido");

        builder.Property(x => x.Id)
            .HasColumnName("Id")
            .IsRequired();

        builder.Property(x => x.DataCriacao)
            .HasColumnName("DataCriacao")
            .IsRequired();

        builder.Property(x => x.TempoEspera)
            .HasColumnName("TempoEspera")
            .IsRequired();

        builder.Property(x => x.DataConclusao)
            .HasColumnName("DataConclusao");

        builder.Property(x => x.Concluido)
            .HasColumnName("Concluido")
            .IsRequired().IsConcurrencyToken();

        builder.HasKey(x => x.Id)
            .HasName("PK_Pedido");
    }
}