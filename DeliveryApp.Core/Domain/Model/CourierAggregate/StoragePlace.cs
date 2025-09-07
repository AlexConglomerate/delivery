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

    private StoragePlace(String Name, int TotalVolume, Guid OrderId) : this()
    {
        Id = Guid.NewGuid();
        Name = Name;
        TotalVolume = TotalVolume;
        OrderId = OrderId;
    }

    public String Name { get; private set; }
    public String TotalVolume { get; private set; }
    public String OrderId { get; private set; }

    // public static Result<Item, Error> Create(Good good, int quantity)
    // {
    //     if (good == null) return GeneralErrors.ValueIsRequired(nameof(good));
    //     if (quantity <= 0) return GeneralErrors.ValueIsInvalid(nameof(quantity));

    //     return new Item(good, quantity);
    // }
}