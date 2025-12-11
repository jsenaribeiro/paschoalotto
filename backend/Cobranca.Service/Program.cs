using CobrancaBFF.API.Middleware;
using NLog.Extensions.Logging;
using NLog.Web;

var builder = WebApplication.CreateBuilder(args);

builder.Logging.SetMinimumLevel(LogLevel.Trace);
builder.Logging.AddNLog();
builder.Host.UseNLog();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwagger("v1");
builder.Services.AddCors("Angular");
builder.Services.AddDbContext(builder.Configuration);
builder.Services.AddDependencies();

var app = builder.Build();


app.UseCors("Angular");
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.RunMigrations();
app.Run();