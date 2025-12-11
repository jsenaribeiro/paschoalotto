namespace Cobranca.Domain;

using System;
using System.Linq.Expressions;
using System.Text.Json.Serialization;


[JsonConverter(typeof(JsonStringEnumConverter))]
public enum Ordering { ASC = 0, DESC }

public interface IRepository<E, I>
   : IReadRepository<E, I>
   , IWriteRepository<E, I>
      where E : Entity<I>
      where I : IEquatable<I>
{
    #region defaults

    async Task<bool> DeleteAsync(I id) =>
       await DeleteAsync(await LoadAsync(id));

    async Task<bool> DeleteAsync(Expression<Func<E, bool>> predicate)
    {
        var returns = new List<bool>();
        var founds = await FilterBy(predicate).ListAsync(true);

        foreach (var entity in founds)
            returns.Add(await DeleteAsync(entity.Id));

        return returns.All(x => x);
    }

    #endregion
}

public interface IWriteRepository<E, I> where E : Entity<I> where I : IEquatable<I>
{
    Task<E> CreateAsync(E? entity);

    Task<E> UpdateAsync(E? entity);

    Task<bool> DeleteAsync(E? entity);
}

public interface IReadRepository<E, I> where E : Entity<I> where I : IEquatable<I>
{
    Task<bool> ExistsAsync();

    Task<long> CountAsync();

    Task<E?> LoadAsync(I Id);

    Task<E?> LoadAsync();

    Task<E[]> ListAsync(bool isReadOnly);

    Task<PageList<E>> ListAsync(int number, int length);

    IReadRepository<E, I> OrderBy(string? field, Ordering order);

    IReadRepository<E, I> FilterBy(Expression<Func<E, bool>> predicate);

    IReadRepository<E, I> WithDeletedRecords();

    #region defaults

    Task<E[]> ListAsync() => ListAsync(false);

    Task<bool> ExistsAsync(I id) => FilterBy(x => x.Id.Equals(id)).ExistsAsync();

    Task<bool> ExistsAsync(Expression<Func<E, bool>> predicate) =>
       FilterBy(predicate).ExistsAsync();

    #endregion
}