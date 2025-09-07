using System;
using FluentAssertions;
using Xunit;
using DeliveryApp.Core.Domain.Model.SharedKernel;
using System.Collections.Generic;


namespace DeliveryApp.UnitTests.а.Model.SharedKernel;

public class LocationShould
{

    public static IEnumerable<object[]> GetLocations()
    {
        yield return [Location.Create(1, 1).Value, 0];
        yield return [Location.Create(1, 2).Value, 1];
        yield return [Location.Create(2, 1).Value, 1];
        yield return [Location.Create(2, 2).Value, 2];
        yield return [Location.Create(10, 10).Value, 18];
    }

    [Theory]
    [InlineData(1, 1)]
    [InlineData(10, 10)]
    [InlineData(5, 7)]
    public void CreateLocation_WhenCoordinatesAreWithinRange(int x, int y)
    {
        // Act
        var location = Location.Create(x, y).Value;

        // Assert
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
    public void ThrowException_WhenCoordinatesAreOutOfRange(int x, int y)
    {
        var location = Location.Create(x, y);

        location.IsSuccess.Should().BeFalse();
        location.Error.Should().NotBeNull();
    }

    [Fact]
    public void BeImmutable()
    {
        var location = Location.Create(3, 4).Value;

        location.X.Should().Be(3);
        location.Y.Should().Be(4);
    }

    [Fact]
    public void CreateRandom()
    {
        var location = Location.CreateRandom();

        location.X.Should().BeInRange(Location.MinLocation.X, Location.MaxLocation.Y);
        location.Y.Should().BeInRange(Location.MinLocation.X, Location.MaxLocation.Y);
    }


    [Theory]
    [MemberData(nameof(GetLocations))]
    public void ReturnDistanceBetweenTwoLocations(Location anotherLocation, int distance)
    {
        var location = Location.Create(1, 1).Value;

        var result = location.DistanceTo(anotherLocation);

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be(distance);
    }
}

