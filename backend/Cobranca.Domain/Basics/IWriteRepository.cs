namespace Cobranca.Domain;

public interface IWriteRepository<E, I> where E : Entity<I> where I : IEquatable<I>
{
   Task<E> CreateAsync(E? entity);

   Task<E> UpdateAsync(E? entity);

   Task<bool> DeleteAsync(E? entity);
}