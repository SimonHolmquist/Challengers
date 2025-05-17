namespace Challengers.Domain.Common;
public abstract class Entity<TKey>
{
    public TKey Id { get; protected set; } = default!;
    public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;
    public DateTime? ModifiedAt { get; private set; }
    protected Entity() { }

    protected Entity(TKey id)
    {
        Id = id;
    }

    public void SetModified()
    {
        ModifiedAt = DateTime.UtcNow;
    }

    public override bool Equals(object? obj)
    {
        return obj is Entity<TKey> other && EqualityComparer<TKey>.Default.Equals(Id, other.Id);
    }

    public override int GetHashCode() => Id?.GetHashCode() ?? 0;
}
