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
        var location = new Location(x, y);

        // Assert
        location.X.Should().Be(x);
        location.Y.Should().Be(y);
    }

    [Theory]
    [InlineData(0, 5)]   // X меньше минимального
    [InlineData(11, 5)]  // X больше максимального
    [InlineData(5, 0)]   // Y меньше минимального
    [InlineData(5, 11)]  // Y больше максимального
    public void ThrowException_WhenCoordinatesAreOutOfRange(int x, int y)
    {
        // Act
        Action act = () => new Location(x, y);

        // Assert
        act.Should().Throw<ArgumentOutOfRangeException>();
    }

    [Fact]
    public void BeImmutable()
    {
        // Arrange
        var location = new Location(3, 4);

        // Act
        Action act = () => { var _ = location.X + location.Y; };

        // Assertф
        location.X.Should().Be(3);
        location.Y.Should().Be(4);
    }
}
