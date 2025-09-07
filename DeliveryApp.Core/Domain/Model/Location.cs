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


    [ExcludeFromCodeCoverage]
    protected override IEnumerable<IComparable> GetEqualityComponents()
    {
        yield return X;
        yield return Y;
    }
}