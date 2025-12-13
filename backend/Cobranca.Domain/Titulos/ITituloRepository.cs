namespace Cobranca.Domain.Titulos;

public interface ITituloRepository : IRepository<Titulo, uint>
{
    Task<Titulo[]> ObterAtrasadosAsync();
    Task<Titulo[]> ObterPorStatusAsync(TituloStatus statusTitulo);
    Task<Titulo[]> ObterTodosAsync();
}