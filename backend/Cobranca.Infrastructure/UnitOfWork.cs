using Cobranca.Domain;
using Cobranca.Domain.Clientes;
using Cobranca.Domain.Titulos;
using Cobranca.Infrastructure.Repositories;

namespace Cobranca.Infrastructure;

public class UnitOfWork : IUnitOfWork
{
   public UnitOfWork(IServiceProvider provider)
   {
      Clientes = new ClienteRepository(provider);
      Titulos = new TituloRepository(provider);
   }

   public IClienteRepository Clientes { get; }

   public ITituloRepository Titulos { get; }

   public Task BeginAsync() => throw new NotImplementedException();

   public Task CommitAsync() => throw new NotImplementedException();

   public Task RollbackAsync() => throw new NotImplementedException();
}