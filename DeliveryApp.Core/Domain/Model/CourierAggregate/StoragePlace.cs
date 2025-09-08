using System.Diagnostics.CodeAnalysis;
using DeliveryApp.Core.Domain.Model.CourierAggregate;
using CSharpFunctionalExtensions;
using Primitives;

namespace DeliveryApp.Core.Domain.Model.CourierAggregate;

/// <summary>
///     Место хранения курьера (рюкзак, багажник и т.п.)
/// </summary>
public class StoragePlace : Entity<Guid>
{
    [ExcludeFromCodeCoverage]
    private StoragePlace()
    {
    }

    private StoragePlace(string name, int totalVolume) : this()
    {
        Id = Guid.NewGuid();
        Name = name;
        TotalVolume = totalVolume;
    }

    public string Name { get; private set; }
    public int TotalVolume { get; private set; }
    public Guid? OrderId { get; private set; }

    public static Result<StoragePlace, Error> Create(string name, int volume)
    {
        if (string.IsNullOrEmpty(name)) return GeneralErrors.ValueIsRequired(nameof(name));
        if (volume <= 0) return GeneralErrors.ValueIsInvalid(nameof(volume));

        return new StoragePlace(name, volume);
    }
}