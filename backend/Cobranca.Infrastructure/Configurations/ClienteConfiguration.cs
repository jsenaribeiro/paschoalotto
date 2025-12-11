namespace Cobranca.Infrastructure.Configurations;

using Cobranca.Domain.Clientes;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Net.Security;

public class ClienteConfiguration : AbstractConfiguration<Cliente, uint>
{
    public override void Configure(EntityTypeBuilder<Cliente> builder)
    {
        builder.ToTable("clientes").HasKey(d => d.Id);

        builder.Property(d => d.Id)
            .ValueGeneratedOnAdd()
            .UseIdentityColumn();

        builder.Property(d => d.Nome)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(d => d.CpfCnpj)
            .IsRequired()
            .HasMaxLength(20)
            .HasColumnName("CPF_CNPJ");

        builder.HasIndex(d => d.CpfCnpj)
            .IsUnique();

        builder.Property(d => d.Email)
          .HasMaxLength(100);

        builder.Property(d => d.Telefone)
            .HasMaxLength(20);

        builder.Property(d => d.Endereco)
            .HasMaxLength(500);

        builder.HasMany(d => d.Titulos)
            .WithOne(t => t.Devedor)
            .HasForeignKey(t => t.DevedorId)
            .OnDelete(DeleteBehavior.Restrict);

        base.Configure(builder);
    }
}