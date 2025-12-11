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
    protected readonly SqlDbContext _context;

    protected DbSet<E> _contextSet => _context.Set<E>();

    protected readonly ILogger<IRepository<E, I>> _logger;

    private (string? Field, Ordering Order) _sort = (null, Ordering.ASC);

    private IQueryable<E> _query;

    private bool _showDeleted = false;

    public AbstractRepository(IServiceProvider provider)
    {
        _context = provider.GetRequiredService<SqlDbContext>();
        _logger = provider.GetRequiredService<ILogger<IRepository<E, I>>>();
        _query = _contextSet;
    }

    private IReadRepository<E, I> fluentOf(Action action) { action(); return this; }

    public IReadRepository<E, I> OrderBy(string? field, Ordering order) =>
       fluentOf(() => _sort = (field, order));

    public IReadRepository<E, I> WithDeletedRecords() =>
        fluentOf(() => _showDeleted = true);

    public IReadRepository<E, I> FilterBy(Expression<Func<E, bool>> predicate) =>
       fluentOf(() => _query = _query.Where(predicate));

    public Task<E?> LoadAsync() => _query.FirstOrDefaultAsync();

    public Task<E?> LoadAsync(I id) => _contextSet
       .FirstOrDefaultAsync(x => x.Id.Equals(id));

    public async Task<bool> ExistsAsync()
    {
        if (_showDeleted)
        {
            _showDeleted = false;
            return await _query.AnyAsync(); 
        }

        return _query
            .Include(x => x.Audit)
            .Select(x => x.Audit)
            .Any(x => x.DeletedAt == null);
    }

    public async Task<long> CountAsync()
    {
        if (_showDeleted)
        {
            _showDeleted = false;
            return await _query.LongCountAsync();
        }

        return _query
            .Include(x => x.Audit)
            .Select(x => x.Audit)
            .LongCount(x => x.DeletedAt == null);
    }

    public async Task<E[]> ListAsync(bool isReadOnly)
    {
        var query = isReadOnly ? _query : _query.AsNoTracking();

        var result = await query.ToArrayAsync();

        _query = _contextSet;
        _showDeleted = false;

        if (_showDeleted)
        {
            _showDeleted = false;
            return result;
        }

        return result.Where(p => p.Audit.DeletedAt == null).ToArray();
    }

    public async Task<PageList<E>> ListAsync(int number, int length)
    {
        _query ??= _contextSet;
        number = number == 0 ? 1 : number;

        var (field, order) = _sort;
        var skip = (number - 1) * length;

        var ordered = string.IsNullOrWhiteSpace(field) ? _query
           : order == Ordering.ASC ? _query.OrderBy(field)
           : _query.OrderBy($"{field} descending");

        var paging = length > 0
           ? ordered.Skip(skip).Take(length)
           : ordered;

        var items = await paging.ToArrayAsync();
        var total = await _query.CountAsync();

        _sort = (null, Ordering.ASC);
        _query = _contextSet;

        if (_showDeleted)
        {
            _showDeleted = false;
            return new(items, total);
        }

        var show = items.Where(p => p.Audit.DeletedAt == null).ToArray();
        var diff = total - show.Length;

        return new(show, total - diff);
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
}