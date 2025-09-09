using FluentAssertions;
using Xunit;
using DeliveryApp.Core.Domain.Model.CourierAggregate;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Infrastructure.Internal;
using System;

namespace DeliveryApp.UnitTests.Domain.Model.CourierAggregate;

public class CourierAggregateShould
{
    // public static IEnumerable<object[]> GetLocationsForDistance()
    // {
    //     yield return [Location.Create(1, 1).Value, 0];
    //     yield return [Location.Create(1, 2).Value, 1];
    //     yield return [Location.Create(2, 1).Value, 1];
    //     yield return [Location.Create(2, 2).Value, 2];
    //     yield return [Location.Create(10, 10).Value, 18];
    // }

    public static IEnumerable<object[]> GetValidLocations()
    {
        yield return ["bagazhnik", 1];
        yield return ["ryukzak", 10];
        yield return ["sumka", 100];
    }

    public static IEnumerable<object[]> GetInvalidLocations()
    {
        yield return [null, 10];
        yield return ["", 10];
        yield return ["bagazhnik", 0];
        yield return ["bagazhnik", -1];
    }

    [Theory]
    [MemberData(nameof(GetValidLocations))]
    public void CreateValidStoragePlace(string name, int volume)
    {
        var location = StoragePlace.Create(name, volume).Value;

        location.Name.Should().Be(name);
        location.TotalVolume.Should().Be(volume);
    }

    [Theory]
    [MemberData(nameof(GetInvalidLocations))]
    public void CreateInValidStoragePlace(string name, int volume)
    {
        var location = StoragePlace.Create(name, volume);

        location.IsSuccess.Should().BeFalse();
        location.Error.Should().NotBeNull();
    }

    [Fact]
    public void CanStore()
    {
        var storage = StoragePlace.Create("name", 10).Value;

        storage.CanStore(0).IsFailure.Should().BeTrue();
        storage.CanStore(-1).IsFailure.Should().BeTrue();

        storage.CanStore(5).Value.Should().BeTrue();
        storage.CanStore(10).Value.Should().BeTrue();
        storage.CanStore(11).Value.Should().BeFalse();
    }

    [Fact]
    public void Store()
    {
        var storage = StoragePlace.Create("name", 10).Value;
        var orderId = Guid.NewGuid();

        var err1 = storage.Store(0, orderId);
        err1.IsFailure.Should().BeTrue();

        var err2 = storage.Store(-10, orderId);
        err2.IsFailure.Should().BeTrue();

        var err3 = storage.Clear(orderId);
        err3.IsFailure.Should().BeTrue();

        var addedToStorage = storage.Store(5, orderId);
        addedToStorage.IsSuccess.Should().BeTrue();

        var cleared = storage.Clear(orderId);
        cleared.IsSuccess.Should().BeTrue();
    }
}
