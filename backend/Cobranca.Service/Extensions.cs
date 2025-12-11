using Cobranca.Domain;
using Cobranca.Domain.Titulos;
using Cobranca.Infrastructure;
using Microsoft.EntityFrameworkCore;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

public static class Extensions
{
    public static IServiceCollection AddSwagger(this IServiceCollection services, string version)
    {
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc(version, new()
            {
                Title = "Cobrança API",
                Version = version,
                Description = "API para gestão de títulos em atraso com cálculos de multa e juros"
            });
        });


        return services;
    }

    public static IServiceCollection AddCors(this IServiceCollection services, string name)
    {
        services.AddCors(options =>
           options.AddPolicy(name, policy =>
              policy.WithOrigins("http://localhost:4200")
                    .AllowAnyMethod()
                    .AllowAnyHeader()));

        return services;
    }

    public static IServiceCollection AddDbContext(this IServiceCollection services, IConfiguration configuration, bool test = false)
    {
        var connection = configuration.GetConnectionString("DefaultConnection");

        if (test)
        {
            services.AddDbContext<SqlDbContext>(opt => opt.UseInMemoryDatabase("db"));
            return services;
        }

        services.AddDbContext<SqlDbContext>(options => options.UseSqlServer(
          connection, sqlOptions =>
          {
              sqlOptions.MigrationsAssembly(typeof(SqlDbContext).Assembly.FullName);
              sqlOptions.EnableRetryOnFailure(errorNumbersToAdd: null,
                maxRetryCount: 5, maxRetryDelay: TimeSpan.FromSeconds(30));
          }));

        return services;
    }

    public static IServiceCollection AddDependencies(this IServiceCollection services)
    {
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<TituloService>();

        return services;
    }

    public static void UseSwagger(this WebApplication app, bool onlyInDevelopment)
    {
        if (!onlyInDevelopment || app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Cobrança API v1");
                c.RoutePrefix = "swagger";
            });
        }
    }

    public static async void RunMigrations(this WebApplication app)
    {
        await using var scope = app.Services.CreateAsyncScope();

        var provider = scope.ServiceProvider;
        var context = provider.GetRequiredService<SqlDbContext>();
        var logger = provider.GetRequiredService<ILogger<Program>>();

        try
        {
            if (context.Database.GetPendingMigrations().Any())
            {
                logger.LogInformation("Aplicando migrações pendentes...");
                await context.Database.MigrateAsync();
                logger.LogInformation("Migrações aplicadas com sucesso.");
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Ocorreu um erro ao migrar ou popular o banco de dados");
        }
    }
}