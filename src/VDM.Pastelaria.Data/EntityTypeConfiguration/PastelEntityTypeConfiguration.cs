using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Diagnostics.CodeAnalysis;
using VDM.Pastelaria.Domain.Entities;

namespace VDM.Pastelaria.Data.EntityTypeConfiguration;

[ExcludeFromCodeCoverage]
public class PastelEntityTypeConfiguration : IEntityTypeConfiguration<PastelEntity>
{
    public void Configure(EntityTypeBuilder<PastelEntity> builder)
    {
        builder.ToTable("Pastel");

        builder.Property(x => x.Id)
            .HasColumnName("Id")
            .IsRequired();

        builder.Property(x => x.Sabor)
            .HasColumnName("Sabor")
            .IsRequired();

        builder.HasIndex(u => u.Sabor)
            .IsUnique();

        builder.HasKey(x => x.Id)
            .HasName("PK_Pastel");
    }
}