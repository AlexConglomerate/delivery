using FluentAssertions;
using Xunit;
using DeliveryApp.Core.Domain.Model.CourierAggregate;
using System.Collections.Generic;

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

    // [Theory]
    // [MemberData(nameof(GetValidLocations))]
    // public void CreateValidLocation(int x, int y)
    // {
    //     var location = Location.Create(x, y).Value;

    //     location.X.Should().Be(x);
    //     location.Y.Should().Be(y);
    // }

    // [Theory]
    // [MemberData(nameof(GetInvalidLocations))]
    // public void CreateInvalidLocation(int x, int y)
    // {
    //     var location = Location.Create(x, y);

    //     location.IsSuccess.Should().BeFalse();
    //     location.Error.Should().NotBeNull();
    // }

    // [Theory]
    // [MemberData(nameof(GetValidLocations))]
    // public void CompareLocations(int x, int y)
    // {
    //     var location1 = Location.Create(x, y).Value;
    //     var location2 = Location.Create(x, y).Value;

    //     var result = location1 == location2;
    //     result.Should().BeTrue();
    // }
    
    // [Theory]
    // [MemberData(nameof(GetLocationsForDistance))]
    // public void ReturnDistanceBetweenTwoLocations(Location anotherLocation, int distance)
    // {
    //     var location = Location.Create(1, 1).Value;
    //     var result = location.DistanceTo(anotherLocation);

    //     result.IsSuccess.Should().BeTrue();
    //     result.Value.Should().Be(distance);
    // }

    // [Fact]
    // public void CreateRandomLocation()
    // {
    //     var location = Location.CreateRandom();

    //     location.X.Should().BeInRange(Location.MinLocation.X, Location.MaxLocation.Y);
    //     location.Y.Should().BeInRange(Location.MinLocation.X, Location.MaxLocation.Y);
    // }
}
