using Cobranca.Domain.Clientes;
using Cobranca.Domain.Titulos;

namespace Cobranca.Domain;

public interface IUnitOfWork
{
   IClienteRepository Clientes { get; }

   ITituloRepository Titulos { get; }

   Task BeginAsync();

   Task CommitAsync();

   Task RollbackAsync();
}
