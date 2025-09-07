using System;
using FluentAssertions;
using Xunit;
using DeliveryApp.Core.Domain.Model.SharedKernel;

namespace DeliveryApp.UnitTests.а.Model.SharedKernel;

public class LocationShould
{
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
}

