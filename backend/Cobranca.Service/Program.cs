using NLog.Extensions.Logging;
using NLog.Web;

var test = true;
var builder = WebApplication.CreateBuilder(args);
var settings = builder.Configuration;

builder.Logging.SetMinimumLevel(LogLevel.Trace);
builder.Logging.AddNLog();
builder.Host.UseNLog();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwagger("v1");
builder.Services.AddCors("Angular");
builder.Services.AddDbContext(settings, test);
builder.Services.AddDependencies();

var app = builder.Build();

app.UseCors("Angular");
app.UseSwagger(false);
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.RunMigrations(test);
app.Run();