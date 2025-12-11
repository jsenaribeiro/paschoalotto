namespace Cobranca.Domain;

using System;

public interface IEntity<I> where I : IEquatable<I>
{
   I Id { get; }

   Audit Audit { get; }
}

public abstract class Entity<I> : IEntity<I> where I : IEquatable<I>
{
   public I Id { get; protected set; } = default!;

   public Audit Audit { get; set; } = Audit.Empty;

   protected Entity() { }

   protected Entity(I id) { Id = id; }

   public override bool Equals(object? obj)
   {
      if (obj is not Entity<I> other) return false;
      if (ReferenceEquals(this, other)) return true;
      if (GetType() != other.GetType()) return false;

      return Id.Equals(other.Id);
   }

   public override int GetHashCode() => Id.GetHashCode();

   public static bool operator ==(Entity<I>? left, Entity<I>? right)
   {
      if (left is null && right is null) return true;
      if (left is null || right is null) return false;

      return left.Equals(right);
   }

   public static bool operator !=(Entity<I>? left, Entity<I>? right) => !(left == right);
}