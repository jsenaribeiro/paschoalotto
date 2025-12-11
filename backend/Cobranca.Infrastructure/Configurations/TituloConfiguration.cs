namespace Cobranca.Infrastructure.Configurations;

using Cobranca.Domain.Titulos;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class TituloConfiguration : AbstractConfiguration<Titulo, uint>
{
    public override void Configure(EntityTypeBuilder<Titulo> builder)
    {
        builder.ToTable("titulos").HasKey(t => t.Id);

        builder.Property(d => d.Id)
            .ValueGeneratedOnAdd()
            .UseIdentityColumn();

        builder.Property(t => t.Numero)
            .IsRequired()
            .HasMaxLength(50);

        builder.HasIndex(t => t.Numero)
            .IsUnique();

        builder.Property(t => t.DataEmissao)
            .IsRequired();

        builder.Property(t => t.Status)
            .IsRequired()
            .HasConversion<int>();

        builder.Property(t => t.Observacao)
            .HasMaxLength(1000);

        builder.HasOne(t => t.Devedor)
            .WithMany(d => d.Titulos)
            .HasForeignKey(t => t.DevedorId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(t => t.Parcelas)
            .WithOne(p => p.Titulo)
            .HasForeignKey(p => p.TituloId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Ignore(p => p.Total);

        builder.Ignore(p => p.EmAtraso);

        base.Configure(builder);
    }
}