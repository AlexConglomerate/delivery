using FluentAssertions;
using Xunit;
using DeliveryApp.Core.Domain.Model.SharedKernel;
using System.Collections.Generic;

namespace DeliveryApp.UnitTests.Domain.Model.SharedKernel;

public class LocationShould
{
    public static IEnumerable<object[]> GetLocationsForDistance()
    {
        yield return [Location.Create(1, 1).Value, 0];
        yield return [Location.Create(1, 2).Value, 1];
        yield return [Location.Create(2, 1).Value, 1];
        yield return [Location.Create(2, 2).Value, 2];
        yield return [Location.Create(10, 10).Value, 18];
    }

    public static IEnumerable<object[]> GetValidLocations()
    {
        yield return [1, 1];
        yield return [10, 10];
        yield return [5, 7];
    }

    [Theory]
    [MemberData(nameof(GetValidLocations))]
    public void CreateValidLocation(int x, int y)
    {
        var location = Location.Create(x, y).Value;

        location.X.Should().Be(x);
        location.Y.Should().Be(y);
    }

    [Theory]
    [InlineData(0, 5)]
    [InlineData(11, 5)]
    [InlineData(-5, 5)]
    [InlineData(5, -5)]
    [InlineData(5, 0)]
    [InlineData(5, 11)]
    public void CreateInvalidLocation(int x, int y)
    {
        var location = Location.Create(x, y);

        location.IsSuccess.Should().BeFalse();
        location.Error.Should().NotBeNull();
    }

    [Theory]
    [MemberData(nameof(GetValidLocations))]
    public void CompareLocations(int x, int y)
    {
        var location1 = Location.Create(x, y).Value;
        var location2 = Location.Create(x, y).Value;

        var result = location1 == location2;
        result.Should().BeTrue();
    }
    
    [Theory]
    [MemberData(nameof(GetLocationsForDistance))]
    public void ReturnDistanceBetweenTwoLocations(Location anotherLocation, int distance)
    {
        var location = Location.Create(1, 1).Value;
        var result = location.DistanceTo(anotherLocation);

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be(distance);
    }

    [Fact]
    public void CreateRandom()
    {
        var location = Location.CreateRandom();

        location.X.Should().BeInRange(Location.MinLocation.X, Location.MaxLocation.Y);
        location.Y.Should().BeInRange(Location.MinLocation.X, Location.MaxLocation.Y);
    }
}
