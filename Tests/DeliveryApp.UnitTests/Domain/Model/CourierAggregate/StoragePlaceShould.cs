using FluentAssertions;
using Xunit;
using System.Collections.Generic;
using System;
using System.Linq;
using System.Reflection;
using CSharpFunctionalExtensions;

namespace DeliveryApp.UnitTests.Domain.Model.StoragePlaces;

public class StoragePlaceShould
{
    public static IEnumerable<object[]> GetValidStoragePlace()
    {
        yield return ["bagazhnik", 1];
        yield return ["ryukzak", 10];
        yield return ["sumka", 100];
    }

    public static IEnumerable<object[]> GetInvalidStoragePlace()
    {
        yield return [null, 10];
        yield return ["", 10];
        yield return ["bagazhnik", 0];
        yield return ["bagazhnik", -1];
    }

    [Fact]
    public void DerivedEntity()
    {
        var isDerivedEntity = typeof(Core.Domain.Model.StoragePlaces.StoragePlace).IsSubclassOf(typeof(Entity<Guid>));
        isDerivedEntity.Should().BeTrue();
    }

    [Fact]
    public void ConstructorShouldBePrivate()
    {
        var typeInfo = typeof(Core.Domain.Model.StoragePlaces.StoragePlace).GetTypeInfo();
        typeInfo.DeclaredConstructors.All(x => x.IsPrivate).Should().BeTrue();
    }

    [Theory]
    [MemberData(nameof(GetValidStoragePlace))]
    public void CreateValidStoragePlace(string name, int volume)
    {
        var storage = Core.Domain.Model.StoragePlaces.StoragePlace.Create(name, volume).Value;

        storage.Name.Should().Be(name);
        storage.Volume.Should().Be(volume);
    }

    [Theory]
    [MemberData(nameof(GetInvalidStoragePlace))]
    public void CreateInValidStoragePlace(string name, int volume)
    {
        var storage = Core.Domain.Model.StoragePlaces.StoragePlace.Create(name, volume);

        storage.IsSuccess.Should().BeFalse();
        storage.Error.Should().NotBeNull();
    }

    [Fact]
    public void CanStore()
    {
        var storage = Core.Domain.Model.StoragePlaces.StoragePlace.Create("name", 10).Value;

        storage.CanStore(0).IsFailure.Should().BeTrue();
        storage.CanStore(-1).IsFailure.Should().BeTrue();

        storage.CanStore(5).Value.Should().BeTrue();
        storage.CanStore(10).Value.Should().BeTrue();

        storage.CanStore(11).Value.Should().BeFalse();
    }

    [Fact]
    public void Store()
    {
        var orderId = Guid.NewGuid();
        var storage = Core.Domain.Model.StoragePlaces.StoragePlace.Create("bagazhnik", 10).Value;

        storage.Store(0, orderId).IsFailure.Should().BeTrue();
        storage.Store(-10, orderId).IsFailure.Should().BeTrue();
        storage.Clear(orderId).IsFailure.Should().BeTrue();

        storage.Store(5, orderId).IsSuccess.Should().BeTrue();
        storage.Clear(orderId).IsSuccess.Should().BeTrue();

        storage.Clear(orderId).IsFailure.Should().BeTrue();
    }
}
