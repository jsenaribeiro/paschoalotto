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
builder.Services.AddDbContext(settings, test);
builder.Services.AddCors("Angular");
builder.Services.AddSwagger("v1");
builder.Services.AddDependencies();
builder.Services.AddResponseCaching();

var app = builder.Build();

app.UseCors("Angular");
app.UseSwagger("v1", false);
app.UseHttpsRedirection();
app.UseAuthorization();
app.UseResponseCaching();
app.MapControllers();
app.RunMigrations(test);
app.Run();