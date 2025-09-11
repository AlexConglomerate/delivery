using FluentAssertions;
using Xunit;
using System.Collections.Generic;
using System;
using System.Linq;
using System.Reflection;
using DeliveryApp.Core.Domain.Model.CourierAggregate;
using DeliveryApp.Core.Domain.Model.SharedKernel;
using DeliveryApp.Core.Domain.Model.OrderAggregate;
using Primitives;

namespace DeliveryApp.UnitTests.Domain.Model.CourierAggregates;

public class CourierAggregateShould
{
    public static IEnumerable<object[]> GetValidCourier()
    {
        yield return ["Vasya", 1, Location.Create(1, 1).Value];
        yield return ["Petya", 3, Location.Create(5, 5).Value];
        yield return ["Kolya", 5, Location.Create(10, 10).Value];
    }

    public static IEnumerable<object[]> GetInvalidCourier()
    {
        yield return ["", 1, Location.Create(1, 1).Value];
        yield return [null, 3, Location.Create(5, 5).Value];
        yield return ["Kolya", null, Location.Create(10, 10).Value];
        yield return ["Kolya", -1, Location.Create(10, 10).Value];
        yield return ["Kolya", 5, null];
    }

    public static IEnumerable<object[]> GetCouriersAndLocations()
    {
        // Пешеход, заказ X:совпадает, Y: совпадает
        yield return
        [
            Courier.Create("Pedestrian", 1, Location.Create(1, 1).Value).Value,
            Location.Create(1, 1).Value,
            Location.Create(1, 1).Value
        ];
        yield return
        [
            Courier.Create("Pedestrian", 1, Location.Create(5, 5).Value).Value,
            Location.Create(5, 5).Value,
            Location.Create(5, 5).Value
        ];

        // Пешеход, заказ X:совпадает, Y: выше
        yield return
        [
            Courier.Create("Pedestrian", 1, Location.Create(1, 1).Value).Value,
            Location.Create(1, 2).Value,
            Location.Create(1, 2).Value
        ];
        yield return
        [
            Courier.Create("Pedestrian", 1, Location.Create(1, 1).Value).Value,
            Location.Create(1, 5).Value,
            Location.Create(1, 2).Value
        ];

        // Пешеход, заказ X:правее, Y: совпадает
        yield return
        [
            Courier.Create("Pedestrian", 1, Location.Create(2, 2).Value).Value,
            Location.Create(3, 2).Value,
            Location.Create(3, 2).Value
        ];
        yield return
        [
            Courier.Create("Pedestrian", 1, Location.Create(5, 5).Value).Value,
            Location.Create(6, 5).Value,
            Location.Create(6, 5).Value
        ];

        // Пешеход, заказ X:правее, Y: выше
        yield return
        [
            Courier.Create("Pedestrian", 1,Location.Create(2, 2).Value).Value,
            Location.Create(3, 3).Value,
            Location.Create(3, 2).Value
        ];
        yield return
        [
            Courier.Create("Pedestrian", 1,Location.Create(1, 1).Value).Value,
            Location.Create(5, 5).Value,
            Location.Create(2, 1).Value
        ];

        // Пешеход, заказ X:совпадает, Y: ниже
        yield return
        [
            Courier.Create("Pedestrian", 1, Location.Create(1, 2).Value).Value,
            Location.Create(1, 1).Value,
            Location.Create(1, 1).Value
        ];
        yield return
        [
            Courier.Create("Pedestrian", 1, Location.Create(5, 5).Value).Value,
            Location.Create(5, 1).Value,
            Location.Create(5, 4).Value
        ];

        // Пешеход, заказ X:левее, Y: совпадает
        yield return
        [
            Courier.Create("Pedestrian", 1, Location.Create(2, 2).Value).Value,
            Location.Create(1, 2).Value,
            Location.Create(1, 2).Value
        ];
        yield return
        [
            Courier.Create("Pedestrian", 1, Location.Create(5, 5).Value).Value,
            Location.Create(1, 5).Value,
            Location.Create(4, 5).Value
        ];

        // Пешеход, заказ X:левее, Y: ниже
        yield return
        [
            Courier.Create("Pedestrian", 1, Location.Create(2, 2).Value).Value,
            Location.Create(1, 1).Value,
            Location.Create(1, 2).Value
        ];
        yield return
        [
            Courier.Create("Pedestrian", 1, Location.Create(5, 5).Value).Value,
            Location.Create(1, 1).Value,
            Location.Create(4, 5).Value
        ];


        // Велосипедист, заказ X:совпадает, Y: совпадает
        yield return
        [
            Courier.Create("Bicycle", 2, Location.Create(1, 1).Value).Value,
            Location.Create(1, 1).Value,
            Location.Create(1, 1).Value
        ];
        yield return
        [
            Courier.Create("Bicycle", 2, Location.Create(5, 5).Value).Value,
            Location.Create(5, 5).Value,
            Location.Create(5, 5).Value
        ];

        // Велосипедист, заказ X:совпадает, Y: выше
        yield return
        [
            Courier.Create("Bicycle", 2, Location.Create(1, 1).Value).Value,
            Location.Create(1, 3).Value,
            Location.Create(1, 3).Value
        ];
        yield return
        [
            Courier.Create("Bicycle", 2, Location.Create(1, 1).Value).Value,
            Location.Create(1, 5).Value,
            Location.Create(1, 3).Value
        ];

        // Велосипедист, заказ X:правее, Y: совпадает
        yield return
        [
            Courier.Create("Bicycle", 2, Location.Create(2, 2).Value).Value,
            Location.Create(4, 2).Value,
            Location.Create(4, 2).Value
        ];
        yield return
        [
            Courier.Create("Bicycle", 2, Location.Create(5, 5).Value).Value,
            Location.Create(8, 5).Value,
            Location.Create(7, 5).Value
        ];

        // Велосипедист, заказ X:правее, Y: выше
        yield return
        [
            Courier.Create("Bicycle", 2, Location.Create(2, 2).Value).Value,
            Location.Create(4, 4).Value,
            Location.Create(4, 2).Value
        ];
        yield return
        [
            Courier.Create("Bicycle", 2, Location.Create(1, 1).Value).Value,
            Location.Create(5, 5).Value,
            Location.Create(3, 1).Value
        ];

        // Велосипедист, заказ X:совпадает, Y: ниже
        yield return
        [
            Courier.Create("Bicycle", 2, Location.Create(1, 3).Value).Value,
            Location.Create(1, 1).Value,
            Location.Create(1, 1).Value
        ];
        yield return
        [
            Courier.Create("Bicycle", 2, Location.Create(5, 5).Value).Value,
            Location.Create(5, 1).Value,
            Location.Create(5, 3).Value
        ];

        // Велосипедист, заказ X:левее, Y: совпадает
        yield return
        [
            Courier.Create("Bicycle", 2, Location.Create(3, 2).Value).Value,
            Location.Create(1, 2).Value,
            Location.Create(1, 2).Value
        ];
        yield return
        [
            Courier.Create("Bicycle", 2, Location.Create(5, 5).Value).Value,
            Location.Create(1, 5).Value,
            Location.Create(3, 5).Value
        ];

        // Велосипедист, заказ X:левее, Y: ниже
        yield return
        [
            Courier.Create("Bicycle", 2, Location.Create(3, 3).Value).Value,
            Location.Create(1, 1).Value,
            Location.Create(1, 3).Value
        ];
        yield return
        [
            Courier.Create("Bicycle", 2, Location.Create(5, 5).Value).Value,
            Location.Create(1, 1).Value,
            Location.Create(3, 5).Value
        ];

        // Велосипедист, заказ ближе чем скорость
        yield return
        [
            Courier.Create("Bicycle", 2, Location.Create(1, 1).Value).Value,
            Location.Create(1, 2).Value,
            Location.Create(1, 2).Value
        ];
        yield return
        [
            Courier.Create("Bicycle", 2, Location.Create(1, 1).Value).Value,
            Location.Create(2, 1).Value,
            Location.Create(2, 1).Value
        ];
        yield return
        [
            Courier.Create("Bicycle", 2, Location.Create(5, 5).Value).Value,
            Location.Create(5, 4).Value,
            Location.Create(5, 4).Value
        ];
        yield return
        [
            Courier.Create("Bicycle", 2, Location.Create(5, 5).Value).Value,
            Location.Create(4, 5).Value,
            Location.Create(4, 5).Value
        ];

        // Велосипедист, заказ с шагами по 2 осям
        yield return
        [
            Courier.Create("Bicycle", 2, Location.Create(1, 1).Value).Value,
            Location.Create(2, 2).Value,
            Location.Create(2, 2).Value
        ];
        yield return
        [
            Courier.Create("Bicycle", 2, Location.Create(5, 5).Value).Value,
            Location.Create(4, 4).Value,
            Location.Create(4, 4).Value
        ];
        yield return
        [
            Courier.Create("Bicycle", 2, Location.Create(1, 1).Value).Value,
            Location.Create(1, 2).Value,
            Location.Create(1, 2).Value
        ];
        yield return
        [
            Courier.Create("Bicycle", 2, Location.Create(5, 5).Value).Value,
            Location.Create(5, 4).Value,
            Location.Create(5, 4).Value
        ];
    }

    [Fact]
    public void DerivedEntity()
    {
        var isDerivedEntity = typeof(Courier).IsSubclassOf(typeof(Aggregate<Guid>));
        isDerivedEntity.Should().BeTrue();
    }

    [Fact]
    public void ConstructorShouldBePrivate()
    {
        var typeInfo = typeof(Courier).GetTypeInfo();
        typeInfo.DeclaredConstructors.All(x => x.IsPrivate).Should().BeTrue();
    }

    [Theory]
    [MemberData(nameof(GetValidCourier))]
    public void CreateValidCourier(string name, int speed, Location location)
    {
        var courier = Courier.Create(name, speed, location).Value;

        courier.Name.Should().Be(name);
        courier.Speed.Should().Be(speed);
        courier.Location.Should().Be(location);
    }

    [Theory]
    [MemberData(nameof(GetInvalidCourier))]
    public void CreateInvalidCourier(string name, int speed, Location location)
    {
        var courier = Courier.Create(name, speed, location);

        courier.IsSuccess.Should().BeFalse();
        courier.Error.Should().NotBeNull();
    }

    [Fact]
    public void AddStoragePlace()
    {
        var courier = Courier.Create("Vasya", 1, Location.Create(1, 1).Value).Value;
        courier.AddStoragePlace("ryukzak", 5).IsSuccess.Should().BeTrue();

        courier.StoragePlaces.Count.Should().Be(2);
    }

    [Fact]
    public void CanTakeOrder()
    {
        var courier = Courier.Create("Vasya", 1, Location.Create(1, 1).Value).Value;
        var order = Order.Create(Guid.NewGuid(), Location.Create(2, 2).Value, 20).Value;

        // сейчас курьер может взять только объем 10, поэтому будет ошибка
        var canTakeOrder_expectNot = courier.CanTakeOrder(order);
        canTakeOrder_expectNot.Value.Should().BeFalse();

        courier.AddStoragePlace("ryukzak", 30).IsSuccess.Should().BeTrue();
        var canTakeOrder = courier.CanTakeOrder(order);
        canTakeOrder.Value.Should().BeTrue();
    }

    [Fact]
    public void TakeOrder()
    {
        var courier = Courier.Create("Vasya", 1, Location.Create(1, 1).Value).Value;
        var order = Order.Create(Guid.NewGuid(), Location.Create(2, 2).Value, 5).Value;
        var takeOrder = courier.TakeOrder(order);

        takeOrder.Value.Should().BeTrue();
    }

    [Fact]
    public void CompleteOrder()
    {
        var courier = Courier.Create("Vasya", 1, Location.Create(1, 1).Value).Value;
        var order = Order.Create(Guid.NewGuid(), Location.Create(2, 2).Value, 5).Value;
        var takeOrder = courier.TakeOrder(order);
        takeOrder.Value.Should().BeTrue();

        var completeOrder = courier.CompleteOrder(order);
        completeOrder.IsSuccess.Should().BeTrue();

        // пробуем завершить несуществующий заказ
        var fakeOrder = Order.Create(Guid.NewGuid(), Location.Create(2, 2).Value, 5).Value;
        var completeOrder_err = courier.CompleteOrder(fakeOrder);
        completeOrder_err.IsFailure.Should().BeTrue();
    }

    [Fact]
    public void CanCalculateTimeToLocation()
    {
        /*
        Изначальная точка курьера: [1,1]
        Целевая точка: [5,10]
        Количество шагов, необходимое курьеру: 13 (4 по горизонтали и 9 по вертикали)
        Скорость транспорта (пешехода): 1 шаг в 1 такт
        Время подлета: 13/13 = 13.0 тактов потребуется курьеру, чтобы доставить заказ
        */
        var location = Location.Create(5, 10).Value;
        var courier = Courier.Create("Vasya", 1, Location.MinLocation).Value;

        var result = courier.CalculateTimeToLocation(location);

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be(13);
    }

    [Theory]
    [MemberData(nameof(GetCouriersAndLocations))]
    public void CanMove(Courier courier, Location targetLocation, Location locationAfterMove)
    {
        var result = courier.Move(targetLocation);

        result.IsSuccess.Should().BeTrue();
        courier.Location.Should().Be(locationAfterMove);
    }
}
