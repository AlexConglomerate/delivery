using System.Diagnostics.CodeAnalysis;
using CSharpFunctionalExtensions;
using Primitives;

namespace DeliveryApp.Core.Domain.Model.SharedKernel;

public sealed class Location : ValueObject
{
    public int X { get; }
    public int Y { get; }

    private const int MinCoordinate = 1;
    private const int MaxCoordinate = 10;

    public static Location MinLocation => new(1, 1);
    public static Location MaxLocation => new(10, 10);

    [ExcludeFromCodeCoverage]
    private Location() { } // For EF Core

    private Location(int x, int y)
    {
        X = x;
        Y = y;
    }

    public static Location Create(int x, int y)
    {
        if (x < MinCoordinate || x > MaxCoordinate) throw new ArgumentOutOfRangeException(nameof(x), $"X должен быть от {MinCoordinate} до {MaxCoordinate}");
        if (y < MinCoordinate || y > MaxCoordinate) throw new ArgumentOutOfRangeException(nameof(y), $"Y должен быть от {MinCoordinate} до {MaxCoordinate}");

        return new Location(x, y);
    }

    public static Location CreateRandom()
    {
        var rnd = new Random(Guid.NewGuid().GetHashCode());
        var x = rnd.Next(MinLocation.X, MaxLocation.X + 1);
        var y = rnd.Next(MinLocation.Y, MaxLocation.Y + 1);
        var location = Create(x, y);
        return location;
    }


    [ExcludeFromCodeCoverage]
    protected override IEnumerable<IComparable> GetEqualityComponents()
    {
        yield return X;
        yield return Y;
    }
}