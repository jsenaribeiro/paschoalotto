namespace Cobranca.Domain;

public record Audit
{
    public DateTime CreatedAt { get; init; } = DateTime.UtcNow;

    public DateTime? UpdatedAt { get; init; }

    public DateTime? DeletedAt { get; init; }

    public static Audit CreateOf<I>(Entity<I> entity) where I : IEquatable<I> =>
    entity.Audit with { CreatedAt = DateTime.UtcNow };

    public static Audit UpdateOf<I>(Entity<I> entity) where I : IEquatable<I> =>
       entity.Audit with { UpdatedAt = DateTime.UtcNow };

    public static Audit DeleteOf<I>(Entity<I> entity) where I : IEquatable<I> =>
       entity.Audit with { DeletedAt = DateTime.UtcNow };

    public static Audit Empty => new();
}