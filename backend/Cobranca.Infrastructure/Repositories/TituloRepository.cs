using Cobranca.Domain.Titulos;
using Microsoft.EntityFrameworkCore;

namespace Cobranca.Infrastructure.Repositories;

public class TituloRepository : AbstractRepository<Titulo, uint>, ITituloRepository
{
    public TituloRepository(IServiceProvider provider) : base(provider) { }

    public async Task<Titulo[]> ObterAtrasadosAsync()
    {
        var tituloIds = await _contextSet
            .Include(t => t.Parcelas)
            .SelectMany(t => t.Parcelas)
            .Include(p => p.Titulo)
            .ThenInclude(t => t.Devedor)
            .Where(p => p.DataVencimento < DateTime.Today)
            .Where(p => p.DataPagamento == null)
            .Where(p => p.Titulo != null)
            .Select(p => p.Titulo)
            .Select(t => t.Id)
            .Distinct()
            .ToArrayAsync();

        var titulos = await _contextSet
            .Include(t => t.Devedor)
            .Include(t => t.Parcelas)
            .Where(t => tituloIds.Contains(t.Id))
            .ToArrayAsync();

        return titulos.Where(x => x is not null).ToArray()!;
    }

    public Task<Titulo[]> ObterPorStatusAsync(StatusTitulo status)
    {
        return _contextSet
            .Include(t => t.Devedor)
            .Include(t => t.Parcelas)
            .Where(t => t.Status == status)
            .ToArrayAsync();
    }

    public Task<Titulo[]> ObterTodosAsync()
    {
        return _contextSet
            .Include(t => t.Devedor)
            .Include(t => t.Parcelas)
            .ToArrayAsync();
    }
}