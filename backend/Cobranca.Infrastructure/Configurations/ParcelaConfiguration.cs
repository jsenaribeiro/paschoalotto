using Cobranca.Domain.Titulos;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Cobranca.Infrastructure.Configurations;

public class ParcelaConfiguration : AbstractConfiguration<Parcela, uint>
{
    public override void Configure(EntityTypeBuilder<Parcela> builder)
    {
        builder.ToTable("Parcelas").HasKey(p => p.Id);

        builder.Property(d => d.Id)
            .ValueGeneratedOnAdd()
            .UseIdentityColumn();

        builder.Property(p => p.Numero)
            .IsRequired();

        builder.Property(p => p.Valor)
            .IsRequired()
            .HasPrecision(18, 2);

        builder.Property(p => p.DataVencimento)
            .IsRequired();

        builder.Property(p => p.DataPagamento);

        builder.Property(p => p.Observacao)
            .HasMaxLength(500);

        builder.HasIndex(p => new { p.TituloId, p.Numero }).IsUnique();
        builder.HasIndex(p => p.DataVencimento);        

        builder.HasOne(p => p.Titulo)
            .WithMany(t => t.Parcelas)
            .HasForeignKey(p => p.TituloId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Ignore(p => p.Total);

        builder.Ignore(p => p.Status);

        builder.Ignore(p => p.DiasDeAtraso);

        base.Configure(builder);
    }
}