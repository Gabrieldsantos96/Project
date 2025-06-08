using System.ComponentModel.DataAnnotations;

namespace Project.Domain.Entities;

public abstract class Entity
{
    public virtual int Id { get; init; }

    public DateTime CreatedAt { get; set; }

    [MaxLength(255)]
    public string? CreatedBy { get; set; }

    public DateTime? UpdatedAt { get; set; }

    [MaxLength(255)]
    public string? UpdatedBy { get; set; }

    public bool IsTransient()
    {
        return Id == default;
    }

    public override bool Equals(object? obj)
    {
        if (obj is not Entity entity) return false;

        if (ReferenceEquals(this, entity)) return true;

        // Entidades transitórias não devem ser comparadas
        if (IsTransient() || entity.IsTransient()) return false;

        return entity.Id == Id;
    }

    public override int GetHashCode()
    {
        // ReSharper disable once BaseObjectGetHashCodeCallInGetHashCode
        return IsTransient() ? base.GetHashCode() : HashCode.Combine(Id);
    }

    public static bool operator ==(Entity? left, Entity? right)
    {
        if (left is null) return right is null;
        return left.Equals(right);
    }

    public static bool operator !=(Entity? left, Entity? right)
    {
        return !(left == right);
    }
}
