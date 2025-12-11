namespace Cobranca.Infrastructure.Configurations;

using Cobranca.Domain;
using Cobranca.Domain.Clientes;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class AbstractConfiguration<E, I> 
    : IEntityTypeConfiguration<E> 
        where I : IEquatable<I> 
        where E : Entity<I>
{
    public virtual void Configure(EntityTypeBuilder<E> builder)
    {
        builder.OwnsOne(t => t.Audit, a =>
        {
            a.Property(p => p.CreatedAt)
                .IsRequired()
                .HasColumnName("CreatedAt")
                .HasDefaultValueSql("GETUTCDATE()");

            a.Property(p => p.UpdatedAt)
                .HasColumnName("UpdatedAt");

            a.WithOwner();
        });
    }
}