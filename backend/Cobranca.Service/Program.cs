var builder = WebApplication.CreateBuilder(args);
var settings = builder.Configuration.AddEnvironmentVariables().Build();
var logger = builder.ConfigureLogging<Program>();
var test = true; // settings.GetValue<bool>("Testing");
var ioc = builder.Services;

try
{
   logger.LogInformation("Iniciando...");

   ioc.AddControllers();
   ioc.AddEndpointsApiExplorer();
   ioc.AddDbContext(settings, test);
   ioc.AddCors("Angular");
   ioc.AddSwagger("v1");
   ioc.AddDependencies();
   ioc.AddResponseCaching();

   var app = builder.Build();

   app.UseCors("Angular");
   app.UseSwagger("v1", false);
   app.UseHttpsRedirection();
   app.UseAuthorization();
   app.UseResponseCaching();
   app.MapControllers();
   app.RunMigrations(test);

   logger.LogInformation("Rodando...");

   await app.RunAsync();
}
catch (Exception ex)
{
   logger.LogError(ex, "Parando aplicação por erro inesperado");
   throw;
}
finally
{
   NLog.LogManager.Shutdown();
}