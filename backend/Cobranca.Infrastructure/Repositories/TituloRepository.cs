using Cobranca.Domain.Titulos;
using Microsoft.EntityFrameworkCore;

namespace Cobranca.Infrastructure.Repositories;

public class TituloRepository : AbstractRepository<Titulo, uint>, ITituloRepository
{
    public TituloRepository(IServiceProvider provider) : base(provider) { }

    public async Task<Titulo[]> ObterAtrasadosAsync()
    {
        var titulos = await _contextSet
            .Include(t => t.Parcelas)
            .ThenInclude(p => p.Titulo)
            .SelectMany(t => t.Parcelas)
            .Where(p => p.DataVencimento < DateTime.Today)
            .Where(p => p.DataPagamento == null)
            .Select(p => p.Titulo)
            .Where(t => t != null)
            .Distinct()
            .ToArrayAsync();

        return titulos.Where(x => x is not null).ToArray()!;
    }
}