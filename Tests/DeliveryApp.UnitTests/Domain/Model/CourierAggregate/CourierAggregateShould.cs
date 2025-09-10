using FluentAssertions;
using Xunit;
using System.Collections.Generic;
using System;
using System.Linq;
using System.Reflection;
using CSharpFunctionalExtensions;
using DeliveryApp.Core.Domain.Model.CourierAggregate;
using DeliveryApp.Core.Domain.Model.SharedKernel;

namespace DeliveryApp.UnitTests.Domain.Model.CourierAggregates;

public class CourierAggregateShould
{
    public static IEnumerable<object[]> GetValidCourier()
    {
        yield return ["Vasya", 1, Location.Create(1, 1).Value];
        yield return ["Petya", 3, Location.Create(5, 5).Value];
        yield return ["Kolya", 5, Location.Create(10, 10).Value];
    }

    public static IEnumerable<object[]> GetInvalidCourierAggregate()
    {
        yield return [null, 10];
        yield return ["", 10];
        yield return ["bagazhnik", 0];
        yield return ["bagazhnik", -1];
    }

    [Fact]
    public void DerivedEntity()
    {
        var isDerivedEntity = typeof(DeliveryApp.Core.Domain.Model.CourierAggregate.Courier).IsSubclassOf(typeof(Entity<Guid>));
        isDerivedEntity.Should().BeTrue();
    }

    [Fact]
    public void ConstructorShouldBePrivate()
    {
        var typeInfo = typeof(DeliveryApp.Core.Domain.Model.CourierAggregate.Courier).GetTypeInfo();
        typeInfo.DeclaredConstructors.All(x => x.IsPrivate).Should().BeTrue();
    }

    [Theory]
    [MemberData(nameof(GetValidCourier))]
    public void CreateValidCourier(string name, int speed, Location location)
    {
        var storage = Courier.Create(name, speed, location).Value;

        storage.Name.Should().Be(name);
        storage.Speed.Should().Be(speed);
        storage.Location.Should().Be(location);
    }

    // [Theory]
    // [MemberData(nameof(GetInvalidCourierAggregate))]
    // public void CreateInValidCourierAggregate(string name, int volume)
    // {
    //     var storage = DeliveryApp.Core.Domain.Model.CourierAggregate.Courier.Create(name, volume);

    //     storage.IsSuccess.Should().BeFalse();
    //     storage.Error.Should().NotBeNull();
    // }

    // [Fact]
    // public void CanStore()
    // {
    //     var storage = DeliveryApp.Core.Domain.Model.CourierAggregate.Courier.Create("name", 10).Value;

    //     storage.CanStore(0).IsFailure.Should().BeTrue();
    //     storage.CanStore(-1).IsFailure.Should().BeTrue();

    //     storage.CanStore(5).Value.Should().BeTrue();
    //     storage.CanStore(10).Value.Should().BeTrue();

    //     storage.CanStore(11).Value.Should().BeFalse();
    // }

    // [Fact]
    // public void Store()
    // {
    //     var orderId = Guid.NewGuid();
    //     var storage = DeliveryApp.Core.Domain.Model.CourierAggregate.Courier.Create("bagazhnik", 10).Value;

    //     storage.Store(0, orderId).IsFailure.Should().BeTrue();
    //     storage.Store(-10, orderId).IsFailure.Should().BeTrue();
    //     storage.Clear(orderId).IsFailure.Should().BeTrue();

    //     storage.Store(5, orderId).IsSuccess.Should().BeTrue();
    //     storage.Clear(orderId).IsSuccess.Should().BeTrue();

    //     storage.Clear(orderId).IsFailure.Should().BeTrue();
    // }
}
