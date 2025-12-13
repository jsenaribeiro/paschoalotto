using Cobranca.Domain.Titulos;
using Microsoft.EntityFrameworkCore;

namespace Cobranca.Infrastructure.Repositories;

public class TituloRepository : AbstractRepository<Titulo, uint>, ITituloRepository
{
    public TituloRepository(IServiceProvider provider) : base(provider) { }

    public async Task<Titulo[]> ObterAtrasadosAsync()
    {
        var query = from t in _contextSet
                    join p in _context.Parcelas on t.Id equals p.TituloId
                    where p.DataVencimento < DateTime.Today
                       && p.DataPagamento == null
                       && t.Audit.DeletedAt == null
                       && p.Audit.DeletedAt == null
                    select t;

        return await query
            .Include(t => t.Devedor)
            .Include(t => t.Parcelas)
            .Distinct()
            .ToArrayAsync();
    }

    public Task<Titulo[]> ObterPorStatusAsync(StatusTitulo status)
    {
        return _contextSet
            .Include(t => t.Devedor)
            .Include(t => t.Parcelas)
            .Where(t => t.Status == status)
            .Where(t => t.Audit.DeletedAt == null)
            .ToArrayAsync();
    }

    public Task<Titulo[]> ObterTodosAsync()
    {
        return _contextSet
            .Include(t => t.Devedor)
            .Include(t => t.Parcelas)
            .Where(t => t.Audit.DeletedAt == null)
            .ToArrayAsync();
    }
}