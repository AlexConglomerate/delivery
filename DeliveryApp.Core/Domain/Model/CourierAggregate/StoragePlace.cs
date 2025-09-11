using System.Diagnostics.CodeAnalysis;
using CSharpFunctionalExtensions;
using Primitives;

namespace DeliveryApp.Core.Domain.Model.StoragePlaces;

/// <summary>
///     Место хранения курьера (рюкзак, багажник и т.п.)
/// </summary>
public class StoragePlace : Entity<Guid>
{
    [ExcludeFromCodeCoverage]
    private StoragePlace()
    {
    }

    private StoragePlace(string name, int volume) : this()
    {
        Id = Guid.NewGuid();
        Name = name;
        Volume = volume;
    }

    public string Name { get; private set; }
    public int Volume { get; private set; }
    public Guid? OrderId { get; private set; }

    public static Result<StoragePlace, Error> Create(string name, int volume)
    {
        if (string.IsNullOrEmpty(name)) return GeneralErrors.ValueIsRequired(nameof(name));
        if (volume <= 0) return GeneralErrors.ValueIsInvalid(nameof(volume));

        return new StoragePlace(name, volume);
    }

    private bool IsOccupied()
    {
        var isOccupied = OrderId != null;
        return isOccupied;
    }

    public Result<bool, Error> CanStore(int volume)
    {
        if (volume <= 0) return GeneralErrors.ValueIsRequired(nameof(volume));

        if (IsOccupied()) return false;
        if (volume > Volume) return false;

        return true;
    }

    public UnitResult<Error> Store(int volume, Guid orderId)
    {
        if (volume <= 0) return GeneralErrors.ValueIsRequired(nameof(volume));
        if (orderId == Guid.Empty) return GeneralErrors.ValueIsRequired(nameof(orderId));
        if (IsOccupied()) return Errors.ErrCannotStoreOrderInThisStoragePlace();

        if (CanStore(volume).Value == false) return Errors.ErrCannotStoreOrderInThisStoragePlace();

        OrderId = orderId;
        return UnitResult.Success<Error>();
    }

    public UnitResult<Error> Clear(Guid orderId)
    {
        if (OrderId == Guid.Empty) return GeneralErrors.ValueIsRequired(nameof(orderId));
        if (OrderId != orderId) return Errors.ErrOrderNotStoredInThisPlace();

        OrderId = null;
        return UnitResult.Success<Error>();
    }

    [ExcludeFromCodeCoverage]
    public static class Errors
    {
        public static Error ErrCannotStoreOrderInThisStoragePlace()
        {
            return new Error($"{nameof(StoragePlace).ToLowerInvariant()}.cannot.store.order.in.this.storage.place",
                "Нельзя поместить заказ в это место хранения");
        }

        public static Error ErrOrderNotStoredInThisPlace()
        {
            return new Error($"{nameof(StoragePlace).ToLowerInvariant()}.order.is.not.stored.in.this.place",
                "В месте хранения нет заказа, который пытаются извлечь");
        }
    }
}