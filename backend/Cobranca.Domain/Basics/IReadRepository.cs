using System.Linq.Expressions;

namespace Cobranca.Domain;

public interface IReadRepository<E, I> where E : Entity<I> where I : IEquatable<I>
{
   // fluent methods

   IReadRepository<E, I> OrderBy(string? field, Order order);

   IReadRepository<E, I> Where(Expression<Func<E, bool>> predicate);

   IReadRepository<E, I> As(QueryFlags flags);

   // terminal methodss

   Task<E?> LoadAsync(I Id);

   Task<E[]> ListAsync();

   Task<PageList<E>> ListAsync((int number, int length) page);

   IAsyncEnumerable<E> LoopAsync();

   Task<bool> ExistsAsync();

   Task<long> CountAsync();

   // default implementations

   Task<bool> ExistsAsync(I id) => Where(x => x.Id.Equals(id)).ExistsAsync();

   Task<bool> ExistsAsync(Expression<Func<E, bool>> predicate) =>
      Where(predicate).ExistsAsync();
}