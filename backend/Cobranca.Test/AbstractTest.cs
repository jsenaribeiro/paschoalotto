using Cobranca.Domain;
using Cobranca.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cobranca.Test;

public abstract class AbstractTest
{
    protected readonly ServiceProvider _provider;

    protected IUnitOfWork _unitOfWork => _provider.GetRequiredService<IUnitOfWork>();

    protected AbstractTest()
    {
        var settings = new Dictionary<string, string>
        {
            {"AllowedHosts", "*"},
            {"Logging:LogLevel:Default", "Information"},
            {"Logging:LogLevel:Microsoft.AspNetCore", "Warning"},
            {"ConnectionStrings:DefaultConnection", ""}
        };

        var configuration = new ConfigurationBuilder()
           .AddInMemoryCollection(settings!)
           .Build();

        _provider = new ServiceCollection()
           .AddScoped<IUnitOfWork, UnitOfWork>()
           .AddDbContext(configuration, true)
           .AddLogging()
           .BuildServiceProvider();
    }
}
