namespace Cobranca.Domain;

using System;
using System.Linq.Expressions;

public interface IRepository<E, I>
   : IReadRepository<E, I>
   , IWriteRepository<E, I>
      where E : Entity<I>
      where I : IEquatable<I>
{
    async Task<E?> UpdateAsync(I id, Action<E> updateAction)
    {
        var entity = await LoadAsync(id);
        if (entity is null) return null;

        updateAction(entity);

        return await UpdateAsync(entity);
    }

    async Task<bool> DeleteAsync(I id) =>
       await DeleteAsync(await LoadAsync(id));

    async Task<bool> DeleteAsync(Expression<Func<E, bool>> predicate)
    {
        var returns = new List<bool>();
        var founds = await Where(predicate).ListAsync();

        foreach (var entity in founds)
            returns.Add(await DeleteAsync(entity.Id));

        return returns.All(x => x);
    }
}
