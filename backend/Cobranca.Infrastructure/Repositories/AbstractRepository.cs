using Cobranca.Domain;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Linq.Dynamic.Core;
using Microsoft.Data.SqlClient;

namespace Cobranca.Infrastructure.Repositories;

public abstract class AbstractRepository<E, I> : IRepository<E, I>
   where E : Entity<I> where I : IEquatable<I>
{
    #region fields

    protected readonly SqlDbContext _context;

    protected DbSet<E> _contextSet => _context.Set<E>();

    protected readonly ILogger<IRepository<E, I>> _logger;

    private (string? Field, Order Order) _sort = (null, Order.ASC);

    private bool _showSoftDeletedRecords = false;

    private bool _asNoTracking = false;

    private IQueryable<E> _query;

    #endregion

    #region constructors

    public AbstractRepository(IServiceProvider provider)
    {
        _context = provider.GetRequiredService<SqlDbContext>();
        _logger = provider.GetRequiredService<ILogger<IRepository<E, I>>>();
        _query = _contextSet;
    }

    #endregion

    #region fluents

    private void fluentReset()
    {
        _query = _contextSet;
        _showSoftDeletedRecords = false;
        _sort = (null, Order.ASC);
        _asNoTracking = false;
    }

    private T fluentAs<T>(bool reset, Func<T> function)
    {
        if (reset) fluentReset();
        return function();
    }

    public IReadRepository<E, I> As(QueryFlags flags)
    {
        _asNoTracking = (flags & QueryFlags.NoTracked) == QueryFlags.NoTracked;
        _showSoftDeletedRecords = (flags & QueryFlags.ShowDeleteds) == QueryFlags.ShowDeleteds;

        return this;
    }

    private IQueryable<E> queryBy
    {
        get
        {
            var query = _query ?? _contextSet;

            if (_asNoTracking) query = query.AsNoTracking();

            if (_showSoftDeletedRecords == false)
                query = query.Include(x => x.Audit)
                               .Where(x => x.Audit.DeletedAt == null);

            var (field, order) = _sort;

            query = string.IsNullOrWhiteSpace(field) ? query
               : order == Order.ASC ? query.OrderBy(field)
               : query.OrderBy($"{field} descending");

            return fluentAs(true, () => query);
        }
    }

    private IReadRepository<E, I> fluentOf(Action action) { action(); return this; }

    public IReadRepository<E, I> OrderBy(string? field, Order order) =>
       fluentOf(() => _sort = (field, order));

    public IReadRepository<E, I> Where(Expression<Func<E, bool>> predicate) =>
       fluentOf(() => _query = _query.Where(predicate));

    #endregion

    #region terminals

    public Task<E?> LoadAsync(I id) => _contextSet
       .FirstOrDefaultAsync(x => x.Id.Equals(id));

    public IAsyncEnumerable<E> LoopAsync() => queryBy.AsAsyncEnumerable();

    public Task<E[]> ListAsync() => queryBy.AsQueryable().ToArrayAsync();

    public async Task<PageList<E>> ListAsync((int number, int length) page)
    {
        var query = queryBy.AsQueryable();

        var skip = (page.number - 1) * page.length;

        var paged = page.length > 0
           ? query.Skip(skip).Take(page.length)
           : query;

        var items = await paged.ToArrayAsync();
        var total = await query.CountAsync();

        return new(items, total);
    }

    public Task<E> CreateAsync(E? entity) => TryAsync(async () =>
    {
        ArgumentNullException.ThrowIfNull(entity);

        entity.Audit = Audit.CreateOf(entity);

        await _contextSet.AddAsync(entity);
        await _context.SaveChangesAsync();

        return entity;
    });

    public Task<E> UpdateAsync(E? entity) => TryAsync(async () =>
    {
        ArgumentNullException.ThrowIfNull(entity);

        entity.Audit = Audit.UpdateOf(entity);
        _contextSet.Update(entity);

        await _context.SaveChangesAsync();

        return entity;
    });

    public async Task<bool> DeleteAsync(E? entity)
    {
        ArgumentNullException.ThrowIfNull(entity);

        try
        {
            entity.Audit = Audit.DeleteOf(entity);

            await _context.SaveChangesAsync();

            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            return false;
        }
    }

    public async Task<bool> ExistsAsync()
    {
        if (_showSoftDeletedRecords == true)
            return await fluentAs(true, () => _query.AnyAsync());

        return _query
            .Include(x => x.Audit)
            .Select(x => x.Audit)
            .Any(x => x.DeletedAt == null);
    }

    public async Task<long> CountAsync()
    {
        if (_showSoftDeletedRecords == true)
            return await fluentAs(true, () => _query.LongCountAsync());

        return _query.Include(x => x.Audit)
                     .Select(x => x.Audit)
                     .LongCount(x => x.DeletedAt == null);
    }

    #endregion

    #region exceptions

    private Task<E> TryAsync(Func<Task<E>> callback)
    {
        try
        {
            return callback();
        }
        catch (DbUpdateConcurrencyException ex)
        {
            _logger.LogError(ex, ex.Message);

            throw new ApplicationException("Conflito no banco de dados");
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, ex.Message);

            if (ex.InnerException is SqlException sqlException)
            {
                var codigoErroDb = sqlException.Number;
                var errosDeDuplicidade = new[] { 2601, 2627 };

                if (errosDeDuplicidade.Contains(codigoErroDb))
                    throw new ApplicationException("Violação de campo único");
            }

            throw;
        }
        catch (ArgumentNullException)
        {
            throw;
        }
        catch (ArgumentException)
        {
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogCritical(ex, ex.Message);
            throw new Exception("Erro inesperado", ex);
        }
    }

    #endregion
}