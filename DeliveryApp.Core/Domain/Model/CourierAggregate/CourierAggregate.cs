using Primitives;
using System.Diagnostics.CodeAnalysis;
using CSharpFunctionalExtensions;
using DeliveryApp.Core.Domain.Model.SharedKernel;
using DeliveryApp.Core.Domain.Model.StoragePlaces;

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

    public string Name { get; private set; } /// <summary> Идентификатор курьера </summary>
    public int Speed { get; private set; } /// <summary> Скорость курьера </summary>
    public Location Location { get; private set; } /// <summary> Местоположение курьера </summary>
    public List<StoragePlace> StoragePlaces { get; private set; } = new(); /// <summary> Места хранения курьера </summary>

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
}