using System.Diagnostics.CodeAnalysis;
using CSharpFunctionalExtensions;
using Primitives;

namespace DeliveryApp.Core.Domain.Model.SharedKernel;

public sealed class Location : ValueObject
{
    public int X { get; }
    public int Y { get; }

    public static Location MinLocation => new(1, 1);
    public static Location MaxLocation => new(10, 10);

    [ExcludeFromCodeCoverage]
    private Location() { } // For EF Core

    private Location(int x, int y)
    {
        X = x;
        Y = y;
    }

    public static Result<Location, Error> Create(int x, int y)
    {
        if (x < MinLocation.X || x > MaxLocation.Y) return GeneralErrors.ValueIsInvalid(nameof(x));
        if (y < MinLocation.X || y > MaxLocation.Y) return GeneralErrors.ValueIsInvalid(nameof(y));

        return new Location(x, y);
    }

    public static Location CreateRandom()
    {
        var rnd = new Random(Guid.NewGuid().GetHashCode());
        var x = rnd.Next(MinLocation.X, MaxLocation.X + 1);
        var y = rnd.Next(MinLocation.Y, MaxLocation.Y + 1);
        var location = Create(x, y).Value;
        return location;
    }

    public Result<int, Error> DistanceTo(Location target)
    {
        if (target == null) return GeneralErrors.ValueIsRequired(nameof(target));
        var diffX = Math.Abs(X - target.X);
        var diffY = Math.Abs(Y - target.Y);
        var distance = diffX + diffY;
        return distance;
    }

    [ExcludeFromCodeCoverage]
    protected override IEnumerable<IComparable> GetEqualityComponents()
    {
        yield return X;
        yield return Y;
    }
}