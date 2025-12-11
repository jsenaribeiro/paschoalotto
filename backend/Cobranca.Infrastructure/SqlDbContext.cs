using Cobranca.Domain;
using Cobranca.Domain.Clientes;
using Cobranca.Domain.Titulos;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace Cobranca.Infrastructure;

using EntityProp = Microsoft.EntityFrameworkCore.Metadata.IMutableProperty;

public class SqlDbContext : DbContext
{
    public DbSet<Titulo> Titulos { get; set; } = null!;

    public DbSet<Parcela> Parcelas { get; set; } = null!;

    public DbSet<Cliente> Clientes { get; set; } = null!;

    public SqlDbContext(DbContextOptions<SqlDbContext> options) : base(options) { }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        base.OnConfiguring(options);

        options.EnableSensitiveDataLogging();
    }

    protected override void OnModelCreating(ModelBuilder mb)
    {
        base.OnModelCreating(mb);

        mb.ApplyConfigurationsFromAssembly(typeof(SqlDbContext).Assembly);

        var isDateTimeType = (EntityProp p) => p.ClrType == typeof(DateTime) || p.ClrType == typeof(DateTime?);
        var isDecimalType = (EntityProp p) => p.ClrType == typeof(decimal) || p.ClrType == typeof(decimal?);

        var entityProperties = mb.Model.GetEntityTypes().SelectMany(t => t.GetProperties());

        entityProperties.Where(isDateTimeType).ToList().ForEach(p => p.SetColumnType("datetime2"));
        entityProperties.Where(isDecimalType).ToList().ForEach(p => p.SetColumnType("decimal(18,2)"));
    }
}