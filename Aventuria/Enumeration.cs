using System.Numerics;
using System.Reflection;

namespace Aventuria;


/// <summary>
/// Class to allow more elaborate enumerations
/// </summary>
/// <remarks>Inspired by https://learn.microsoft.com/en-us/dotnet/architecture/microservices/microservice-ddd-cqrs-patterns/enumeration-classes-over-enum-types</remarks>
public abstract class Enumeration : IComparable, 
    IEquatable<Enumeration>, IEqualityOperators<Enumeration, Enumeration, bool>, IEqualityOperators<Enumeration, int, bool>
{
    /// <summary>
    /// Gets the name.
    /// </summary>
    /// <value>A <see cref="string"/> that is the name of the <see cref="Enumeration"/>.</value>
    public readonly string Name;

    /// <summary>
    /// Gets the value.
    /// </summary>
    /// <value>A <c>int</c> that is the value of the <see cref="Enumeration"/>.</value>
    public readonly int Value;


    protected Enumeration(string name, int value)
    {
        if (String.IsNullOrEmpty(name))
            throw new ArgumentNullException(nameof(name));

        Name = name;
        Value = value;
    }

    /// <inheritdoc />
    public override string ToString() => Name;

    
    public static IEnumerable<T> GetAll<T>() where T : Enumeration =>
        typeof(T).GetFields(BindingFlags.Public |
                            BindingFlags.Static |
                            BindingFlags.DeclaredOnly)
                    .Select(f => f.GetValue(null))
                    .Cast<T>();


    /// <inheritdoc />
    public bool Equals(Enumeration? other)
    {
        return other is not null && Value == other.Value;
    }

    /// <inheritdoc />
    public static bool operator ==(Enumeration? left, int right)
    {
        return left?.Equals(right) ?? false;
    }
    /// <inheritdoc />
    public static bool operator !=(Enumeration? left, int right)
    {
        return !left?.Equals(right) ?? true;
    }


    #region IMPLEMENT IComparable

    /// <inheritdoc />
    public override bool Equals(object? obj)
    {
        if (obj is not Enumeration other)
        {
            if (obj is int otherValue)
                return Value.Equals(otherValue);
            return false;
        }

        var typeMatches = GetType().Equals(obj.GetType());
        var valueMatches = Value.Equals(other.Value);

        return typeMatches && valueMatches;
    }

    /// <inheritdoc />
    public int CompareTo(object? other) => 
        other is not null ? Value.CompareTo(((Enumeration)other).Value) : throw new ArgumentNullException(nameof(other));

    /// <inheritdoc />
    public override int GetHashCode() => Value.GetHashCode();


    /// <inheritdoc />
    public static bool operator ==(Enumeration? left, Enumeration? right)
    {
        return left?.Equals(right) ?? right is null;
    }

    /// <inheritdoc />
    public static bool operator !=(Enumeration? left, Enumeration? right)
    {
        return !left?.Equals(right) ?? right is not null;
    }

    /// <inheritdoc />
    public static bool operator <(Enumeration left, Enumeration right)
    {
        return left.CompareTo(right) < 0;
    }

    /// <inheritdoc />
    public static bool operator <=(Enumeration left, Enumeration right)
    {
        return left.CompareTo(right) <= 0;
    }

    /// <inheritdoc />
    public static bool operator >(Enumeration left, Enumeration right)
    {
        return left.CompareTo(right) > 0;
    }

    /// <inheritdoc />
    public static bool operator >=(Enumeration left, Enumeration right)
    {
        return left.CompareTo(right) >= 0;
    }

    #endregion
}