using Primitives;
using System.Diagnostics.CodeAnalysis;
using CSharpFunctionalExtensions;
using DeliveryApp.Core.Domain.Model.SharedKernel;
using DeliveryApp.Core.Domain.Model.StoragePlaces;
using DeliveryApp.Core.Domain.Model.OrderAggregate;

namespace DeliveryApp.Core.Domain.Model.CourierAggregate;

public sealed class Courier : Aggregate<Guid>
{
    [ExcludeFromCodeCoverage]
    private Courier()
    {
    }

    private Courier(string name, int speed, Location location, StoragePlace storage) : this()
    {
        Id = Guid.NewGuid();
        Name = name;
        Speed = speed;
        Location = location;
        StoragePlaces.Add(storage);
    }

    /// <summary> Идентификатор курьера </summary>
    public string Name { get; private set; }

    /// <summary> Скорость курьера </summary>
    public int Speed { get; private set; }

    /// <summary> Местоположение курьера </summary>
    public Location Location { get; private set; }
    
     /// <summary> Места хранения курьера </summary>
    public List<StoragePlace> StoragePlaces { get; private set; } = new();

    public static Result<Courier, Error> Create(string name, int speed, Location location)
    {
        if (string.IsNullOrEmpty(name)) return GeneralErrors.ValueIsRequired(nameof(name));
        if (speed <= 0) return GeneralErrors.ValueIsInvalid(nameof(speed));
        if (location == null) return GeneralErrors.ValueIsRequired(nameof(location));

        var storage = StoragePlace.Create("Сумка", 10);
        if (storage.IsFailure) return storage.Error;

        return new Courier(name, speed, location, storage.Value);
    }

    public UnitResult<Error> AddStoragePlace(string name, int volume)
    {
        var storage = StoragePlace.Create(name, volume);
        if (storage.IsFailure) return storage.Error;

        StoragePlaces.Add(storage.Value);
        return UnitResult.Success<Error>();
    }

    public Result<bool, Error> CanTakeOrder(Order order)
    {
        foreach (var storage in StoragePlaces)
        {
            var canStore = storage.CanStore(order.Volume);
            if (canStore.Value == true) return true;
        }
        return false;
    }

    public Result<bool, Error> TakeOrder(Order order)
    {
        foreach (var storage in StoragePlaces)
        {
            var canStore = storage.CanStore(order.Volume);
            if (canStore.Value == true)
            {
                storage.Store(order.Volume, order.Id);
                return true;
            }
        }
        return false;
    }

    public UnitResult<Error> CompleteOrder(Order order)
    {
        if (order is null) return GeneralErrors.ValueIsRequired(nameof(order));

        var storage = StoragePlaces.FirstOrDefault(s => s.OrderId == order.Id);
        if (storage is null) return GeneralErrors.ValueIsRequired(nameof(storage));

        var clearResult = storage.Clear(order.Id);
        return clearResult;
    }

    public Result<double, Error> CalculateTimeToLocation(Location location)
    {
        if (location == null) return GeneralErrors.ValueIsRequired(nameof(location));

        var distanceToResult = Location.DistanceTo(location);
        if (distanceToResult.IsFailure) return distanceToResult.Error;
        var distance = distanceToResult.Value;

        var time = (double)distance / Speed;
        return time;
    }

    public UnitResult<Error> Move(Location target)
    {
        if (target == null) return GeneralErrors.ValueIsRequired(nameof(target));

        var difX = target.X - Location.X;
        var difY = target.Y - Location.Y;
        var cruisingRange = Speed;

        var moveX = Math.Clamp(difX, -cruisingRange, cruisingRange);
        cruisingRange -= Math.Abs(moveX);

        var moveY = Math.Clamp(difY, -cruisingRange, cruisingRange);

        var locationCreateResult = Location.Create(Location.X + moveX, Location.Y + moveY);
        if (locationCreateResult.IsFailure) return locationCreateResult.Error;
        Location = locationCreateResult.Value;

        return UnitResult.Success<Error>();
    }
}