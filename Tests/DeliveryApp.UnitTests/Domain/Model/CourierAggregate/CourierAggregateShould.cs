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

        var canTakeOrder_expectNot = courier.CanTakeOrder(order);
        canTakeOrder_expectNot.Value.Should().BeFalse(); // сейчас курьер может взять только объем 10

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
}
