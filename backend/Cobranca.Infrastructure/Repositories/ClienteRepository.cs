using Cobranca.Domain.Clientes;

namespace Cobranca.Infrastructure.Repositories;

public class ClienteRepository : AbstractRepository<Cliente, uint>, IClienteRepository
{
    public ClienteRepository(IServiceProvider provider) : base(provider) { }
}